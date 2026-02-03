from pydantic_settings import BaseSettings, SettingsConfigDict
from functools import lru_cache


class Settings(BaseSettings):
    """Application settings loaded from environment variables."""
    
    # Gemini API Configuration
    gemini_api_key: str
    gemini_project_name: str
    gemini_project_number: str
    gemini_key_name: str
    
    # Application Configuration
    app_name: str = "FrogEdu AI Service"
    debug: bool = False
    
    # CORS Configuration
    cors_origins: list[str] = ["*"]
    
    # Rate Limiting (for future use)
    max_tokens_per_user: int = 100000
    
    model_config = SettingsConfigDict(
        env_file=".env",
        env_file_encoding="utf-8",
        case_sensitive=False,
        extra="ignore"  # Ignore extra fields from Doppler
    )


@lru_cache()
def get_settings() -> Settings:
    """Get cached settings instance."""
    return Settings(_env_file=".env")
