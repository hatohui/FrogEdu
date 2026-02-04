from pydantic_settings import BaseSettings, SettingsConfigDict
from functools import lru_cache


class Settings(BaseSettings):
    """Application settings loaded from environment variables."""
    
    # Gemini API Configuration
    gemini_api_key: str
    
    # Application Configuration
    app_name: str = "FrogEdu AI Service"
    debug: bool = False
    
    # CORS Configuration
    cors_origins: list[str] = ["*"]
    
    # Rate Limiting (for future use)
    max_tokens_per_user: int = 100000
    
    # AWS Cognito Configuration for JWT validation
    cognito_region: str = "ap-southeast-1"
    cognito_user_pool_id: str = ""
    
    # Subscription Service Configuration (for Backend Token Enrichment)
    subscription_service_url: str = "http://localhost:5003/api/subscriptions"
    
    @property
    def cognito_issuer(self) -> str:
        """Get the Cognito issuer URL for JWT validation."""
        return f"https://cognito-idp.{self.cognito_region}.amazonaws.com/{self.cognito_user_pool_id}"
    
    @property
    def cognito_jwks_url(self) -> str:
        """Get the Cognito JWKS URL for fetching public keys."""
        return f"{self.cognito_issuer}/.well-known/jwks.json"
    
    model_config = SettingsConfigDict(
        env_file=".env",
        env_file_encoding="utf-8",
        case_sensitive=False,
        extra="ignore" 
    )


@lru_cache()
def get_settings() -> Settings:
    """Get cached settings instance."""
    return Settings(_env_file=".env") # type: ignore
