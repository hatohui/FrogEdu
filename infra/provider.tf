# =============================================================================
# Provider Configuration
# =============================================================================
# Note: Providers must use variables directly, not locals or data sources
# The Doppler data source will be available after providers are initialized

# Doppler Provider - Must be configured first
provider "doppler" {
  doppler_token = var.doppler_token
}

# AWS Provider - All serverless compute and auth services
# Note: Uses variables with defaults, will be overridden by Doppler values in resources
provider "aws" {
  region = var.aws_region

  default_tags {
    tags = {
      Project     = var.project_name
      Environment = var.environment
      ManagedBy   = "Terraform"
      Repository  = "FrogEdu"
    }
  }
}

# Cloudflare Provider - Frontend hosting and object storage
# Note: Token must be set via TF_VAR_cloudflare_api_token environment variable
provider "cloudflare" {
  api_token = var.cloudflare_api_token != "" ? var.cloudflare_api_token : null
}