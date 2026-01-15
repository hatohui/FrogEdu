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

  project_name      = local.project_name
  environment       = local.environment
  cognito_user_pool = module.cognito.user_pool
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

# ECR Repositories for Lambda Container Images
module "ecr" {
  source = "./modules/ecr"

  project_name = local.project_name
  environment  = local.environment

  repositories = {
    ai-api = {
      image_tag_mutability  = "MUTABLE"
      scan_on_push          = true
      lifecycle_policy_keep = 10
      lifecycle_expire_days = 7
    }
    assessment-api = {
      image_tag_mutability  = "MUTABLE"
      scan_on_push          = true
      lifecycle_policy_keep = 10
      lifecycle_expire_days = 7
    }
    content-api = {
      image_tag_mutability  = "MUTABLE"
      scan_on_push          = true
      lifecycle_policy_keep = 10
      lifecycle_expire_days = 7
    }
  }
}