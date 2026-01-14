# =============================================================================
# FrogEdu Infrastructure - Main Configuration
# Purpose: Orchestrate all modules for serverless architecture
# =============================================================================

# =============================================================================
# Doppler Integration - Single Source of Truth
# =============================================================================
# All configuration values are pulled from Doppler (no tfvars needed)
data "doppler_secrets" "this" {
  project = var.doppler_project
  config  = var.doppler_config
}

# Local values derived from Doppler secrets
locals {
  # Environment configuration
  environment  = try(data.doppler_secrets.this.map.TF_ENVIRONMENT, "dev")
  project_name = try(data.doppler_secrets.this.map.TF_PROJECT_NAME, "frogedu")
  aws_region   = try(data.doppler_secrets.this.map.TF_AWS_REGION, "ap-southeast-1")
  
  # Domain configuration
  domain_name   = try(data.doppler_secrets.this.map.TF_DOMAIN_NAME, "frogedu.com")
  api_subdomain = try(data.doppler_secrets.this.map.TF_API_SUBDOMAIN, "api")
  
  # Lambda configuration
  lambda_package_type   = try(data.doppler_secrets.this.map.TF_LAMBDA_PACKAGE_TYPE, "Image")
  lambda_timeout        = try(tonumber(data.doppler_secrets.this.map.TF_LAMBDA_TIMEOUT), 30)
  lambda_memory_size    = try(tonumber(data.doppler_secrets.this.map.TF_LAMBDA_MEMORY_SIZE), 512)
  lambda_architectures  = try(split(",", data.doppler_secrets.this.map.TF_LAMBDA_ARCHITECTURES), ["arm64"])
  
  # Database configuration (optional)
  neon_project_id = try(data.doppler_secrets.this.map.TF_NEON_PROJECT_ID, "")
}

# =============================================================================
# Core Infrastructure Modules
# =============================================================================

# Cognito User Pool for authentication
module "cognito" {
  source = "./modules/cognito"

  project_name = local.project_name
  environment  = local.environment
  aws_region   = local.aws_region
}

# IAM roles and policies
module "iam" {
  source = "./modules/iam"

  project_name = local.project_name
  environment  = local.environment
}

# API Gateway REST API
module "api_gateway" {
  source = "./modules/api-gateway"

  project_name       = local.project_name
  environment        = local.environment
  cognito_user_pool  = module.cognito.user_pool
}

# CloudFront CDN
module "cloudfront" {
  source = "./modules/cloudfront"

  project_name         = local.project_name
  environment          = local.environment
  api_gateway_domain   = module.api_gateway.api_domain
  api_gateway_stage    = local.environment
  origin_verify_secret = try(data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET, "temp-secret-${local.environment}")
}

# Cloudflare R2 Storage
module "cloudflare" {
  source = "./modules/cloudflare"

  project_name             = local.project_name
  environment              = local.environment
  cloudflare_account_id    = data.doppler_secrets.this.map.CLOUDFLARE_ACCOUNT_ID
  cloudflare_account_name  = data.doppler_secrets.this.map.CLOUDFLARE_ACCOUNT_NAME
}

# =============================================================================
# Lambda Functions (to be added per service)
# =============================================================================
# Secrets are passed directly as environment variables from Doppler
# This saves $0.40/month per secret by not using AWS Secrets Manager

# Example: User Service Lambda
# module "lambda_user_service" {
#   source = "./modules/lambda-container"
#
#   function_name     = "user-service"
#   ecr_image_uri     = "<account-id>.dkr.ecr.ap-southeast-1.amazonaws.com/frogedu/user-service:latest"
#   project_name      = local.project_name
#   environment       = local.environment
#   execution_role    = module.iam.lambda_execution_role
#   memory_size       = local.lambda_memory_size
#   timeout           = local.lambda_timeout
#   architectures     = local.lambda_architectures
#
#   # Pass Doppler secrets directly as encrypted environment variables
#   environment_variables = {
#     ASPNETCORE_ENVIRONMENT           = local.environment
#     NEON_USER_DB_CONNECTION_STRING   = data.doppler_secrets.this.map.NEON_USER_DB_CONNECTION_STRING
#     JWT_SECRET_KEY                   = data.doppler_secrets.this.map.JWT_SECRET_KEY
#     CLOUDFLARE_R2_ACCESS_KEY         = data.doppler_secrets.this.map.CLOUDFLARE_R2_ACCESS_KEY
#     CLOUDFLARE_R2_SECRET_KEY         = data.doppler_secrets.this.map.CLOUDFLARE_R2_SECRET_KEY
#     CLOUDFLARE_ACCOUNT_ID            = data.doppler_secrets.this.map.CLOUDFLARE_ACCOUNT_ID
#   }
#
#   # API Gateway integration
#   api_gateway_id           = module.api_gateway.api_id
#   api_gateway_root_id      = module.api_gateway.api_root_resource_id
#   api_gateway_execution_arn = module.api_gateway.api_execution_arn
#   cognito_authorizer_id    = module.api_gateway.cognito_authorizer_id
#
#   routes = [
#     { path = "users", http_method = "POST", auth_required = false },
#     { path = "users/sync", http_method = "POST", auth_required = true }
#   ]
# }
