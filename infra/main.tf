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
  cognito_user_pool_arn = module.cognito.user_pool_arn
  origin_verify_secret  = data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET
}

module "cloudfront" {
  source = "./modules/cloudfront"

  providers = {
    aws.us_east_1 = aws.us_east_1
  }

  project_name = local.project_name
  # Extract domain name from invoke_url: https://abc123.execute-api.region.amazonaws.com/prod
  # Remove https:// and everything after the domain (including /prod)
  api_gateway_domain   = regex("^https://([^/]+)", aws_api_gateway_stage.root.invoke_url)[0]
  origin_verify_secret = try(data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET, "temp-secret-${local.environment}")
  custom_domain        = local.api_domain                                                                                                       # Custom domain for CloudFront distribution
  web_acl_id           = "arn:aws:wafv2:us-east-1:630633962130:global/webacl/CreatedByCloudFront-3152e2ae/a30508da-915f-4206-91a5-eb034e211fb1" # Required by flat-rate pricing plan
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

# =============================================================================
# Lambda Functions - Containerized Microservices
# =============================================================================

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
  api_gateway_id            = module.api_gateway.api_gateway_id
  api_gateway_root_id       = module.api_gateway.api_resource_id
  api_gateway_execution_arn = module.api_gateway.execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id
  request_validator_id      = module.api_gateway.request_validator_id
  origin_verify_secret      = data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET

  shared_parent_resources = {
    "api" = module.api_gateway.api_path_resource_id
  }

  routes = [
    {
      path          = "api/contents"
      http_method   = "ANY"
      auth_required = false
    }
    ,
    {
      path          = "api/contents/health"
      http_method   = "ANY"
      auth_required = false
    }
    ,
    {
      path          = "api/contents/health/db"
      http_method   = "ANY"
      auth_required = false
    }
    ,
    {
      path          = "api/contents/{proxy+}"
      http_method   = "ANY"
      auth_required = true
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

  api_gateway_id            = module.api_gateway.api_gateway_id
  api_gateway_root_id       = module.api_gateway.api_resource_id
  api_gateway_execution_arn = module.api_gateway.execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id
  request_validator_id      = module.api_gateway.request_validator_id
  origin_verify_secret      = data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET

  shared_parent_resources = {
    "api" = module.api_gateway.api_path_resource_id
  }

  routes = [
    {
      path          = "api/users"
      http_method   = "ANY"
      auth_required = false
    }
    ,
    {
      path          = "api/users/health"
      http_method   = "ANY"
      auth_required = false
    }
    ,
    {
      path          = "api/users/health/db"
      http_method   = "ANY"
      auth_required = false
    }
    ,
    {
      path          = "api/users/{proxy+}"
      http_method   = "ANY"
      auth_required = true
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

  api_gateway_id            = module.api_gateway.api_gateway_id
  api_gateway_root_id       = module.api_gateway.api_resource_id
  api_gateway_execution_arn = module.api_gateway.execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id
  request_validator_id      = module.api_gateway.request_validator_id
  origin_verify_secret      = data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET

  shared_parent_resources = {
    "api" = module.api_gateway.api_path_resource_id
  }

  routes = [
    {
      path          = "api/assessments"
      http_method   = "ANY"
      auth_required = false
    }
    ,
    {
      path          = "api/assessments/health"
      http_method   = "ANY"
      auth_required = false
    }
    ,
    {
      path          = "api/assessments/health/db"
      http_method   = "ANY"
      auth_required = false
    }
    ,
    {
      path          = "api/assessments/{proxy+}"
      http_method   = "ANY"
      auth_required = true
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
  api_gateway_id            = module.api_gateway.api_gateway_id
  api_gateway_root_id       = module.api_gateway.api_resource_id
  api_gateway_execution_arn = module.api_gateway.execution_arn
  cognito_authorizer_id     = module.api_gateway.cognito_authorizer_id
  request_validator_id      = module.api_gateway.request_validator_id
  origin_verify_secret      = data.doppler_secrets.this.map.CLOUDFRONT_ORIGIN_VERIFY_SECRET

  shared_parent_resources = {
    "api" = module.api_gateway.api_path_resource_id
  }

  routes = [
    {
      path          = "api/ai"
      http_method   = "ANY"
      auth_required = false
    }
    ,
    {
      path          = "api/ai/health"
      http_method   = "ANY"
      auth_required = false
    }
    ,
    {
      path          = "api/ai/health/db"
      http_method   = "ANY"
      auth_required = false
    }
    ,
    {
      path          = "api/ai/{proxy+}"
      http_method   = "ANY"
      auth_required = true
    }
  ]
}

# =============================================================================
# API Gateway Deployment and Stage
# =============================================================================
# Must be created after all Lambda integrations are configured

resource "aws_api_gateway_deployment" "root" {
  rest_api_id = module.api_gateway.api_gateway_id

  # Depend on all Lambda modules to ensure routes are created first
  depends_on = [
    module.content_lambda,
    module.user_lambda,
    module.assessment_lambda,
    module.ai_lambda
  ]

  # Trigger redeployment when any Lambda module changes
  triggers = {
    redeployment = sha1(jsonencode([
      module.content_lambda.function_arn,
      module.user_lambda.function_arn,
      module.assessment_lambda.function_arn,
      module.ai_lambda.function_arn,
    ]))
  }

  lifecycle {
    create_before_destroy = true
  }
}

resource "aws_api_gateway_stage" "root" {
  deployment_id = aws_api_gateway_deployment.root.id
  rest_api_id   = module.api_gateway.api_gateway_id
  stage_name    = "prod"

  # Enable X-Ray tracing for debugging
  xray_tracing_enabled = false

  tags = {
    Environment = local.environment
    Name        = "${local.project_name}-api-stage"
  }
}

# CloudWatch Log Group for API Gateway access logs
resource "aws_cloudwatch_log_group" "api_gateway" {
  name              = "/aws/apigateway/${local.project_name}-api"
  retention_in_days = 7

  tags = {
    Environment = local.environment
  }
}
