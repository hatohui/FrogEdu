# =============================================================================
# Core Infrastructure Modules
# =============================================================================

module "cognito" {
  source = "./modules/cognito"

  project_name         = local.project_name
  environment          = local.environment
  aws_region           = local.aws_region
  frontend_domain      = local.frontend_domain
  google_client_id     = try(data.doppler_secrets.this.map.GOOGLE_CLIENT_ID, "placeholder-id")
  google_client_secret = try(data.doppler_secrets.this.map.GOOGLE_CLIENT_SECRET, "placeholder-secret")
}

module "iam" {
  source = "./modules/iam"

  project_name = local.project_name
  environment  = local.environment
  github_repo  = try(data.doppler_secrets.this.map.TF_GITHUB_REPO, "*")
}

module "api_gateway" {
  source = "./modules/api-gateway"

  project_name          = local.project_name
  cors_origins          = var.cors_origins
  cognito_user_pool_id  = module.cognito.user_pool_id
  cognito_user_pool_arn = module.cognito.user_pool_arn
  cognito_issuer_url    = module.cognito.issuer_url
  cognito_web_client_id = module.cognito.web_client_id
}

module "cloudfront" {
  source = "./modules/cloudfront"

  providers = {
    aws.us_east_1 = aws.us_east_1
  }

  project_name         = local.project_name
  api_gateway_domain   = module.api_gateway.api_gateway_domain
  origin_verify_secret = try(data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET, "temp-secret-${local.environment}")
  custom_domain        = local.api_domain
  web_acl_id           = "arn:aws:wafv2:us-east-1:630633962130:global/webacl/CreatedByCloudFront-3152e2ae/a30508da-915f-4206-91a5-eb034e211fb1"
}

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
    user-api = {
      image_tag_mutability  = "MUTABLE"
      scan_on_push          = true
      lifecycle_policy_keep = 10
      lifecycle_expire_days = 7
    }
  }
}

module "content_service" {
  source = "./modules/microservice"

  service_name              = "contents"
  project_name              = local.project_name
  lambda_role_arn           = module.iam.lambda_execution_role.arn
  ecr_repository            = module.ecr.repository_urls["content-api"]
  api_gateway_id            = module.api_gateway.api_gateway_id
  api_gateway_execution_arn = module.api_gateway.api_gateway_execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id

  no_auth_routes = [
    "/health",
    "/swagger/{proxy+}",
  ]

  environment_variables = {
    ASPNETCORE_ENVIRONMENT = "Production"
  }
}

module "user_service" {
  source = "./modules/microservice"

  service_name              = "users"
  project_name              = local.project_name
  lambda_role_arn           = module.iam.lambda_execution_role.arn
  ecr_repository            = module.ecr.repository_urls["user-api"]
  api_gateway_id            = module.api_gateway.api_gateway_id
  api_gateway_execution_arn = module.api_gateway.api_gateway_execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id

  no_auth_routes = [
    "/health",
    "/swagger/{proxy+}",
  ]

  environment_variables = {
    ASPNETCORE_ENVIRONMENT = "Production"
  }
}

module "assessment_service" {
  source = "./modules/microservice"

  service_name              = "assessments"
  project_name              = local.project_name
  lambda_role_arn           = module.iam.lambda_execution_role.arn
  ecr_repository            = module.ecr.repository_urls["assessment-api"]
  api_gateway_id            = module.api_gateway.api_gateway_id
  api_gateway_execution_arn = module.api_gateway.api_gateway_execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id

  no_auth_routes = [
    "/health",
    "/swagger/{proxy+}",
  ]

  environment_variables = {
    ASPNETCORE_ENVIRONMENT = "Production"
  }
}


module "ai_service" {
  source = "./modules/microservice"

  service_name              = "ai"
  project_name              = local.project_name
  lambda_role_arn           = module.iam.lambda_execution_role.arn
  ecr_repository            = module.ecr.repository_urls["ai-api"]
  api_gateway_id            = module.api_gateway.api_gateway_id
  api_gateway_execution_arn = module.api_gateway.api_gateway_execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id

  no_auth_routes = [
    "/health",
    "/swagger/{proxy+}",
  ]

  environment_variables = {
    ASPNETCORE_ENVIRONMENT = "Production"
  }
}
