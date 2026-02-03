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


# =============================================================================
# Email Service Module
# =============================================================================

module "ses" {
  source = "./modules/ses"

  domain              = local.ses_domain
  namespace           = local.project_name
  environment         = local.environment
  mail_from_subdomain = "mail"
}

# =============================================================================
# Repo Modules
# =============================================================================



module "ecr" {
  source = "./modules/ecr"

  project_name = local.project_name
  environment  = local.environment

  repositories = {
    exam-api = {
      image_tag_mutability  = "MUTABLE"
      scan_on_push          = true
      lifecycle_policy_keep = 3
      lifecycle_expire_days = 7
    }
    user-api = {
      image_tag_mutability  = "MUTABLE"
      scan_on_push          = true
      lifecycle_policy_keep = 3
      lifecycle_expire_days = 7
    }
    class-api = {
      image_tag_mutability  = "MUTABLE"
      scan_on_push          = true
      lifecycle_policy_keep = 3
      lifecycle_expire_days = 7
    }
    subscription-api = {
      image_tag_mutability  = "MUTABLE"
      scan_on_push          = true
      lifecycle_policy_keep = 3
      lifecycle_expire_days = 7
    }
    ai-api = {
      image_tag_mutability  = "MUTABLE"
      scan_on_push          = true
      lifecycle_policy_keep = 3
      lifecycle_expire_days = 7
    }
  }
}

# =============================================================================
# Microservice Modules
# =============================================================================


module "exam_service" {
  source = "./modules/microservice"

  service_name              = "exams"
  project_name              = local.project_name
  lambda_role_arn           = module.iam.lambda_execution_role.arn
  ecr_repository            = module.ecr.repository_urls["exam-api"]
  api_gateway_id            = module.api_gateway.api_gateway_id
  api_gateway_execution_arn = module.api_gateway.api_gateway_execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id

  no_auth_routes = [
    "/health/{proxy+}",
    "/health",
    "/swagger/{proxy+}",
    "/swagger",
  ]

  environment_variables = {
    ASPNETCORE_ENVIRONMENT    = "Production"
    EXAM_DB_CONNECTION_STRING = local.exam_db_connection_string
    MEDIAK_LICENSE_KEY        = local.mediak_license_key
    COGNITO_USER_POOL_ID      = module.cognito.user_pool_id
    AWS_COGNITO_REGION        = local.aws_region
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
    "/health/{proxy+}",
    "/health",
    "/swagger/{proxy+}",
    "/swagger",
    "/auth/webhook",
    "/roles/{proxy+}",
    "/roles",
  ]

  environment_variables = {
    ASPNETCORE_ENVIRONMENT    = "Production"
    USER_DB_CONNECTION_STRING = local.user_db_connection_string
    MEDIAK_LICENSE_KEY        = local.mediak_license_key
    COGNITO_USER_POOL_ID      = module.cognito.user_pool_id
    AWS_COGNITO_REGION        = local.aws_region
    SES_ACCESS_KEY_ID         = module.ses.access_key_id
    SES_SECRET_ACCESS_KEY     = module.ses.secret_access_key
    SES_REGION                = local.aws_region
    AWS__SES__FromEmail       = "noreply@${local.ses_domain}"
    AWS__SES__FromName        = "FrogEdu"
    Frontend__BaseUrl         = "https://${local.frontend_domain}"
    R2_ACCOUNT_ID             = local.r2_account_id
    R2_ACCESS_KEY             = local.r2_access_key_id
    R2_SECRET_KEY             = local.r2_secret_access_key
    R2_BUCKET_NAME            = local.r2_bucket_name
    R2_PUBLIC_ENDPOINT        = local.r2_public_endpoint
  }
}

module "class_service" {
  source = "./modules/microservice"

  service_name              = "classes"
  project_name              = local.project_name
  lambda_role_arn           = module.iam.lambda_execution_role.arn
  ecr_repository            = module.ecr.repository_urls["class-api"]
  api_gateway_id            = module.api_gateway.api_gateway_id
  api_gateway_execution_arn = module.api_gateway.api_gateway_execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id

  no_auth_routes = [
    "/health/{proxy+}",
    "/health",
    "/swagger/{proxy+}",
    "/swagger",
  ]

  environment_variables = {
    ASPNETCORE_ENVIRONMENT     = "Production"
    CLASS_DB_CONNECTION_STRING = local.class_db_connection_string
    MEDIAK_LICENSE_KEY         = local.mediak_license_key
    COGNITO_USER_POOL_ID       = module.cognito.user_pool_id
    AWS_COGNITO_REGION         = local.aws_region
  }
}

module "subscription_service" {
  source = "./modules/microservice"

  service_name              = "subscriptions"
  project_name              = local.project_name
  lambda_role_arn           = module.iam.lambda_execution_role.arn
  ecr_repository            = module.ecr.repository_urls["subscription-api"]
  api_gateway_id            = module.api_gateway.api_gateway_id
  api_gateway_execution_arn = module.api_gateway.api_gateway_execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id

  no_auth_routes = [
    "/health/{proxy+}",
    "/health",
    "/swagger/{proxy+}",
    "/swagger",
  ]

  environment_variables = {
    ASPNETCORE_ENVIRONMENT            = "Production"
    SUBSCRIPTION_DB_CONNECTION_STRING = local.subscription_db_connection_string
    MEDIAK_LICENSE_KEY                = local.mediak_license_key
    COGNITO_USER_POOL_ID              = module.cognito.user_pool_id
    AWS_COGNITO_REGION                = local.aws_region
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
    "/docs",
    "/redoc",
    "/openapi.json",
  ]

  environment_variables = {
    GEMINI_API_KEY        = local.gemini_api_key
    GEMINI_KEY_NAME       = local.gemini_key_name
    GEMINI_PROJECT_NUMBER = local.gemini_project_number
    GEMINI_PROJECT_NAME   = local.gemini_project_name
  }
}
