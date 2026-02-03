"""
Authentication module for JWT validation with AWS Cognito.
Validates JWT tokens and extracts subscription claims.
"""

import logging
from datetime import datetime, timezone
from typing import Annotated, Any

import httpx
from fastapi import Depends, HTTPException, status
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from jose import jwt, jwk, JWTError
from pydantic import BaseModel, Field

from app.config import get_settings, Settings

logger = logging.getLogger(__name__)

# Security scheme for JWT Bearer token
security = HTTPBearer(auto_error=False)


class SubscriptionClaims(BaseModel):
    """Subscription claims extracted from JWT token."""
    plan: str = Field(default="free")
    expires_at: int = Field(default=0)
    has_active_subscription: bool = Field(default=False)


class TokenUser(BaseModel):
    """User information extracted from JWT token."""
    sub: str = Field(description="Cognito user ID")
    email: str | None = Field(default=None)
    role: str | None = Field(default=None)
    subscription: SubscriptionClaims = Field(default_factory=SubscriptionClaims)
    
    @property
    def has_pro_subscription(self) -> bool:
        """Check if user has an active Pro subscription."""
        if not self.subscription.has_active_subscription:
            return False
        if self.subscription.plan.lower() != "pro":
            return False
        # Check if subscription is not expired
        if self.subscription.expires_at > 0:
            return datetime.now(timezone.utc).timestamp() < self.subscription.expires_at
        return False


class JWKSCache:
    """Cache for Cognito JWKS (JSON Web Key Set)."""
    
    def __init__(self) -> None:
        self._keys: dict[str, dict[str, Any]] = {}
        self._last_fetched: datetime | None = None
        self._cache_duration_seconds: int = 3600  # 1 hour
    
    async def get_key(self, kid: str, jwks_url: str) -> dict[str, Any] | None:
        """Get a key from the JWKS cache, refreshing if needed."""
        # Check if cache needs refresh
        if self._should_refresh():
            await self._refresh_keys(jwks_url)
        
        return self._keys.get(kid)
    
    def _should_refresh(self) -> bool:
        """Check if the cache should be refreshed."""
        if not self._last_fetched:
            return True
        elapsed = (datetime.now(timezone.utc) - self._last_fetched).total_seconds()
        return elapsed > self._cache_duration_seconds
    
    async def _refresh_keys(self, jwks_url: str) -> None:
        """Refresh the JWKS cache from Cognito."""
        try:
            async with httpx.AsyncClient() as client:
                response = await client.get(jwks_url, timeout=10.0)
                response.raise_for_status()
                jwks: dict[str, Any] = response.json()
                
                keys_list: list[dict[str, Any]] = jwks.get("keys", [])
                self._keys = {key["kid"]: key for key in keys_list if "kid" in key}
                self._last_fetched = datetime.now(timezone.utc)
                logger.info(f"Refreshed JWKS cache with {len(self._keys)} keys")
        except Exception as e:
            logger.error(f"Failed to refresh JWKS cache: {e}")
            # Keep existing keys if refresh fails


# Global JWKS cache instance
_jwks_cache = JWKSCache()


async def validate_token(
    token: str,
    settings: Settings
) -> TokenUser:
    """
    Validate a JWT token from Cognito and extract user/subscription claims.
    
    Args:
        token: The JWT token to validate
        settings: Application settings
        
    Returns:
        TokenUser with extracted claims
        
    Raises:
        HTTPException: If token is invalid
    """
    logger.info("🔐 validate_token called")
    try:
        # Decode header to get key ID
        unverified_header = jwt.get_unverified_header(token)
        kid = unverified_header.get("kid")
        logger.info(f"   Token kid: {kid}")
        
        if not kid:
            logger.error("❌ Token missing key ID")
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED,
                detail="Token missing key ID"
            )
        
        # Get the signing key from JWKS
        key_data = await _jwks_cache.get_key(kid, settings.cognito_jwks_url)
        logger.info(f"   Key data found: {key_data is not None}")
        
        if not key_data:
            logger.error("❌ Unable to find signing key")
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED,
                detail="Unable to find signing key"
            )
        
        # Convert JWK to public key
        public_key = jwk.construct(key_data)
        
        # Decode and validate the token
        payload = jwt.decode(
            token,
            public_key,
            algorithms=["RS256"],
            issuer=settings.cognito_issuer,
            options={
                "verify_aud": False,  # Cognito doesn't always include audience
                "verify_exp": True,
                "verify_iss": True,
            }
        )
        
        # Extract subscription claims from token
        subscription = SubscriptionClaims(
            plan=payload.get("custom:plan", payload.get("plan", "free")),
            expires_at=int(payload.get("custom:subscription_expires_at", payload.get("subscription_expires_at", 0))),
            has_active_subscription=payload.get("custom:has_subscription", payload.get("has_subscription", False)) in [True, "true", "True", 1, "1"]
        )
        
        user = TokenUser(
            sub=payload.get("sub", ""),
            email=payload.get("email"),
            role=payload.get("custom:role"),
            subscription=subscription
        )
        logger.info(f"✅ Token validated for user: {user.sub}, role: {user.role}, plan: {user.subscription.plan}")
        return user
        
    except JWTError as e:
        logger.error(f"❌ JWT validation failed: {e}")
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail=f"Invalid token: {str(e)}"
        )
    except Exception as e:
        logger.error(f"Unexpected error validating token: {e}")
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Token validation failed"
        )


async def get_current_user(
    credentials: Annotated[HTTPAuthorizationCredentials | None, Depends(security)],
    settings: Annotated[Settings, Depends(get_settings)]
) -> TokenUser:
    """
    FastAPI dependency to get the current authenticated user.
    
    Args:
        credentials: Bearer token credentials
        settings: Application settings
        
    Returns:
        TokenUser with validated claims
        
    Raises:
        HTTPException: If not authenticated or token is invalid
    """
    logger.info("🔍 get_current_user called")
    if not credentials:
        logger.error("❌ No credentials provided")
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Not authenticated",
            headers={"WWW-Authenticate": "Bearer"}
        )
    
    logger.info("   Credentials present, validating token...")
    return await validate_token(credentials.credentials, settings)


async def get_optional_user(
    credentials: Annotated[HTTPAuthorizationCredentials | None, Depends(security)],
    settings: Annotated[Settings, Depends(get_settings)]
) -> TokenUser | None:
    """
    FastAPI dependency to get the current user if authenticated, or None.
    
    Args:
        credentials: Bearer token credentials (optional)
        settings: Application settings
        
    Returns:
        TokenUser if authenticated, None otherwise
    """
    if not credentials:
        return None
    
    try:
        return await validate_token(credentials.credentials, settings)
    except HTTPException:
        return None


def require_subscription(user: TokenUser) -> TokenUser:
    """
    Verify that the user has an active Pro subscription.
    
    Args:
        user: The authenticated user
        
    Returns:
        The user if they have a subscription
        
    Raises:
        HTTPException: If user doesn't have an active subscription
    """
    logger.info(f"   Checking subscription: has_pro={user.has_pro_subscription}")
    if not user.has_pro_subscription:
        logger.warning(f"❌ User {user.sub} does not have active Pro subscription")
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN,
            detail="Active Pro subscription required to access this feature"
        )
    logger.info(f"✅ User {user.sub} has valid Pro subscription")
    return user


async def get_subscribed_user(
    user: Annotated[TokenUser, Depends(get_current_user)]
) -> TokenUser:
    """
    FastAPI dependency to get an authenticated user with active subscription.
    
    Args:
        user: The authenticated user
        
    Returns:
        TokenUser with verified subscription
        
    Raises:
        HTTPException: If not subscribed
    """
    logger.info(f"🎫 get_subscribed_user called for user: {user.sub}")
    logger.info(f"   Subscription: plan={user.subscription.plan}, active={user.subscription.has_active_subscription}")
    return require_subscription(user)
