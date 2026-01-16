# =============================================================================
# Core Infrastructure Modules
# =============================================================================

# Cognito User Pool for authentication
module "cognito" {
  source = "./modules/cognito"

  project_name         = local.project_name
  environment          = local.environment
  aws_region           = local.aws_region
  frontend_domain      = local.frontend_domain
  google_client_id     = try(data.doppler_secrets.this.map.GOOGLE_CLIENT_ID, "placeholder-id")
  google_client_secret = try(data.doppler_secrets.this.map.GOOGLE_CLIENT_SECRET, "placeholder-secret")
}

# IAM roles and policies
module "iam" {
  source = "./modules/iam"

  project_name = local.project_name
  environment  = local.environment
  github_repo  = try(data.doppler_secrets.this.map.TF_GITHUB_REPO, "*")
}

# API Gateway REST API
module "api_gateway" {
  source = "./modules/api-gateway"

  project_name         = local.project_name
  environment          = local.environment
  cognito_user_pool    = module.cognito.user_pool
  origin_verify_secret = try(data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET, "")
}

# CloudFront CDN
module "cloudfront" {
  source = "./modules/cloudfront"

  providers = {
    aws.us_east_1 = aws.us_east_1
  }

  project_name         = local.project_name
  environment          = local.environment
  api_gateway_domain   = module.api_gateway.api_domain
  api_gateway_stage    = local.environment
  origin_verify_secret = try(data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET, "temp-secret-${local.environment}")
  custom_domain        = local.api_domain
  web_acl_id           = "arn:aws:wafv2:us-east-1:630633962130:global/webacl/CreatedByCloudFront-3152e2ae/a30508da-915f-4206-91a5-eb034e211fb1"
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
    user-api = {
      image_tag_mutability  = "MUTABLE"
      scan_on_push          = true
      lifecycle_policy_keep = 10
      lifecycle_expire_days = 7
    }
  }
}

# =============================================================================
# Lambda Functions - Containerized Microservices
# =============================================================================

# Content Service Lambda
module "content_lambda" {
  source = "./modules/lambda-container"

  project_name  = local.project_name
  environment   = local.environment
  function_name = "content-api"

  ecr_image_uri  = "${module.ecr.repository_urls["content-api"]}:latest"
  execution_role = module.iam.lambda_execution_role

  memory_size   = local.lambda_memory_size
  timeout       = local.lambda_timeout
  architectures = local.lambda_architectures

  environment_variables = {
    ASPNETCORE_ENVIRONMENT = local.environment
  }

  # API Gateway integration
  api_gateway_id            = module.api_gateway.api_id
  api_gateway_root_id       = module.api_gateway.api_resource_id # Use /api as parent
  api_gateway_execution_arn = module.api_gateway.execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id
  request_validator_id      = module.api_gateway.request_validator_id
  origin_verify_secret      = try(data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET, "")

  # Use proxy+ to strip /api/contents prefix - Lambda receives only the sub-path
  routes = [
    {
      path          = "contents/{proxy+}"
      http_method   = "ANY"
      auth_required = false
    }
  ]
}

# User Service Lambda
module "user_lambda" {
  source = "./modules/lambda-container"

  project_name  = local.project_name
  environment   = local.environment
  function_name = "user-api"

  ecr_image_uri  = "${module.ecr.repository_urls["user-api"]}:latest"
  execution_role = module.iam.lambda_execution_role

  memory_size   = local.lambda_memory_size
  timeout       = local.lambda_timeout
  architectures = local.lambda_architectures

  environment_variables = {
    ASPNETCORE_ENVIRONMENT = local.environment
  }

  # API Gateway integration
  api_gateway_id            = module.api_gateway.api_id
  api_gateway_root_id       = module.api_gateway.api_resource_id # Use /api as parent
  api_gateway_execution_arn = module.api_gateway.execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id
  request_validator_id      = module.api_gateway.request_validator_id
  origin_verify_secret      = try(data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET, "")

  # Use proxy+ to strip /api/users prefix - Lambda receives only the sub-path
  routes = [
    {
      path          = "users/{proxy+}"
      http_method   = "ANY"
      auth_required = false
    }
  ]
}

# Assessment Service Lambda
module "assessment_lambda" {
  source = "./modules/lambda-container"

  project_name  = local.project_name
  environment   = local.environment
  function_name = "assessment-api"

  ecr_image_uri  = "${module.ecr.repository_urls["assessment-api"]}:latest"
  execution_role = module.iam.lambda_execution_role

  memory_size   = local.lambda_memory_size
  timeout       = local.lambda_timeout
  architectures = local.lambda_architectures

  environment_variables = {
    ASPNETCORE_ENVIRONMENT = local.environment
  }

  # API Gateway integration
  api_gateway_id            = module.api_gateway.api_id
  api_gateway_root_id       = module.api_gateway.api_resource_id # Use /api as parent
  api_gateway_execution_arn = module.api_gateway.execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id
  request_validator_id      = module.api_gateway.request_validator_id
  origin_verify_secret      = try(data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET, "")

  # Use proxy+ to strip /api/assessments prefix - Lambda receives only the sub-path
  routes = [
    {
      path          = "assessments/{proxy+}"
      http_method   = "ANY"
      auth_required = false
    }
  ]
}

# AI Service Lambda
module "ai_lambda" {
  source = "./modules/lambda-container"

  project_name  = local.project_name
  environment   = local.environment
  function_name = "ai-api"

  ecr_image_uri  = "${module.ecr.repository_urls["ai-api"]}:latest"
  execution_role = module.iam.lambda_execution_role

  memory_size   = local.lambda_memory_size
  timeout       = local.lambda_timeout
  architectures = local.lambda_architectures

  environment_variables = {
    ASPNETCORE_ENVIRONMENT = local.environment
  }

  # API Gateway integration
  api_gateway_id            = module.api_gateway.api_id
  api_gateway_root_id       = module.api_gateway.api_resource_id # Use /api as parent
  api_gateway_execution_arn = module.api_gateway.execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id
  request_validator_id      = module.api_gateway.request_validator_id
  origin_verify_secret      = try(data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET, "")

  # Use proxy+ to strip /api/ai prefix - Lambda receives only the sub-path
  routes = [
    {
      path          = "ai/{proxy+}"
      http_method   = "ANY"
      auth_required = false
    }
  ]
}

# =============================================================================
# API Gateway Deployment & Stage (After all Lambda routes are created)
# =============================================================================

resource "aws_api_gateway_deployment" "main" {
  rest_api_id = module.api_gateway.api_id

  triggers = {
    # Redeploy when Lambda integrations change
    redeployment = sha1(jsonencode([
      module.user_lambda.function_arn,
      module.content_lambda.function_arn,
      module.assessment_lambda.function_arn,
      module.ai_lambda.function_arn,
    ]))
  }

  lifecycle {
    create_before_destroy = true
  }

  depends_on = [
    module.api_gateway,
    module.user_lambda,
    module.content_lambda,
    module.assessment_lambda,
    module.ai_lambda,
  ]
}

resource "aws_api_gateway_stage" "main" {
  deployment_id = aws_api_gateway_deployment.main.id
  rest_api_id   = module.api_gateway.api_id
  stage_name    = local.environment

  depends_on = [aws_api_gateway_deployment.main]
}
