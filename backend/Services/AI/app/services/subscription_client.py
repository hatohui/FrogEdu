"""
Subscription service client for fetching subscription claims from the Subscription microservice.
This implements Backend Token Enrichment - fetching subscription data at runtime
instead of relying on Cognito custom claims.
"""

import logging
from typing import Any

import httpx
from pydantic import BaseModel, Field

from app.config import get_settings, Settings

logger = logging.getLogger(__name__)


class SubscriptionClaimsResponse(BaseModel):
    """Response from the Subscription service claims endpoint."""
    userId: str = Field(alias="userId")
    plan: str = Field(default="free")
    expiresAt: int = Field(default=0, alias="expiresAt")
    hasActiveSubscription: bool = Field(default=False, alias="hasActiveSubscription")

    class Config:
        populate_by_name = True


class SubscriptionClient:
    """
    HTTP client for fetching subscription claims from the Subscription microservice.
    
    This client is used for Backend Token Enrichment - instead of relying on
    Cognito custom claims (which don't exist), we fetch subscription data
    directly from the Subscription service using the user's ID from the JWT.
    """
    
    def __init__(self, settings: Settings) -> None:
        self._settings = settings
        self._base_url = settings.subscription_service_url
        self._timeout = 10.0
        self._cache: dict[str, tuple[SubscriptionClaimsResponse, float]] = {}
        self._cache_ttl = 300  # 5 minutes cache
    
    async def get_subscription_claims(self, user_id: str) -> SubscriptionClaimsResponse:
        """
        Fetch subscription claims for a user from the Subscription service.
        
        Args:
            user_id: The user's ID (from JWT sub claim or custom:user_id)
            
        Returns:
            SubscriptionClaimsResponse with the user's subscription data
        """
        import time
        
        # Check cache first
        cached = self._cache.get(user_id)
        if cached:
            claims, timestamp = cached
            if time.time() - timestamp < self._cache_ttl:
                logger.debug(f"Using cached subscription claims for user {user_id}")
                return claims
        
        try:
            logger.info(f"Fetching subscription claims for user {user_id} from {self._base_url}")
            
            async with httpx.AsyncClient() as client:
                response = await client.get(
                    f"{self._base_url}/claims/{user_id}",
                    timeout=self._timeout,
                    headers={"Accept": "application/json"}
                )
                
                if response.status_code == 200:
                    data = response.json()
                    claims = SubscriptionClaimsResponse(**data)
                    
                    # Cache the result
                    self._cache[user_id] = (claims, time.time())
                    
                    logger.info(
                        f"Retrieved subscription for user {user_id}: "
                        f"plan={claims.plan}, active={claims.hasActiveSubscription}"
                    )
                    return claims
                else:
                    logger.warning(
                        f"Failed to fetch subscription claims for user {user_id}. "
                        f"Status: {response.status_code}"
                    )
                    return self._default_claims(user_id)
                    
        except httpx.TimeoutException:
            logger.warning(f"Timeout fetching subscription claims for user {user_id}")
            return self._default_claims(user_id)
        except httpx.RequestError as e:
            logger.error(f"HTTP error fetching subscription claims: {e}")
            return self._default_claims(user_id)
        except Exception as e:
            logger.error(f"Unexpected error fetching subscription claims: {e}")
            return self._default_claims(user_id)
    
    def _default_claims(self, user_id: str) -> SubscriptionClaimsResponse:
        """Return default (free plan) claims when service is unavailable."""
        return SubscriptionClaimsResponse(
            userId=user_id,
            plan="free",
            expiresAt=0,
            hasActiveSubscription=False
        )
    
    def clear_cache(self, user_id: str | None = None) -> None:
        """
        Clear the subscription cache.
        
        Args:
            user_id: If provided, only clear cache for this user.
                     If None, clear entire cache.
        """
        if user_id:
            self._cache.pop(user_id, None)
        else:
            self._cache.clear()


# Global singleton instance
_subscription_client: SubscriptionClient | None = None


def get_subscription_client() -> SubscriptionClient:
    """Get the subscription client singleton instance."""
    global _subscription_client
    if _subscription_client is None:
        _subscription_client = SubscriptionClient(get_settings())
    return _subscription_client
