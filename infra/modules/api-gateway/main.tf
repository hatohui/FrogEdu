# =============================================================================
# API Gateway Module - REST API
# =============================================================================

# REST API
resource "aws_api_gateway_rest_api" "main" {
  name        = "${var.project_name}-${var.environment}-api"
  description = "FrogEdu REST API"

  endpoint_configuration {
    types = ["REGIONAL"]
  }
}

# =============================================================================
# REST API Policy - Allow health checks from anywhere, restrict others to CloudFront
# =============================================================================
data "aws_iam_policy_document" "api_policy" {
  # Allow health check endpoints from anywhere (CloudFront or direct access)
  statement {
    sid    = "AllowHealthChecks"
    effect = "Allow"

    principals {
      type        = "*"
      identifiers = ["*"]
    }

    actions = ["execute-api:Invoke"]

    resources = [
      "${aws_api_gateway_rest_api.main.execution_arn}/${var.environment}/GET/api/*/health",
      "${aws_api_gateway_rest_api.main.execution_arn}/${var.environment}/GET/*/health",
    ]
  }

  # Allow all other routes only with X-Origin-Verify header (CloudFront only)
  dynamic "statement" {
    for_each = var.origin_verify_secret != "" ? [1] : []

    content {
      sid    = "AllowCloudFrontOnly"
      effect = "Allow"

      principals {
        type        = "*"
        identifiers = ["*"]
      }

      actions = ["execute-api:Invoke"]

      resources = [
        "${aws_api_gateway_rest_api.main.execution_arn}/*/*/*",
      ]

      condition {
        test     = "StringEquals"
        variable = "aws:UserAgent"
        values   = ["Amazon CloudFront"]
      }
    }
  }

  # Fallback: If no CloudFront protection, allow all
  dynamic "statement" {
    for_each = var.origin_verify_secret == "" ? [1] : []

    content {
      sid    = "AllowAll"
      effect = "Allow"

      principals {
        type        = "*"
        identifiers = ["*"]
      }

      actions   = ["execute-api:Invoke"]
      resources = ["${aws_api_gateway_rest_api.main.execution_arn}/*"]
    }
  }
}

resource "aws_api_gateway_rest_api_policy" "main" {
  rest_api_id = aws_api_gateway_rest_api.main.id
  policy      = data.aws_iam_policy_document.api_policy.json
}

# Cognito Authorizer
resource "aws_api_gateway_authorizer" "cognito" {
  name            = "${var.project_name}-${var.environment}-cognito"
  rest_api_id     = aws_api_gateway_rest_api.main.id
  type            = "COGNITO_USER_POOLS"
  identity_source = "method.request.header.Authorization"
  provider_arns   = [var.cognito_user_pool.arn]
}

# Request Validator - requires headers to be present (only for authenticated routes)
# Health routes will not use this validator, allowing them to bypass X-Origin-Verify
resource "aws_api_gateway_request_validator" "header_validator" {
  count = var.origin_verify_secret != "" ? 1 : 0

  name                        = "${var.project_name}-${var.environment}-header-validator"
  rest_api_id                 = aws_api_gateway_rest_api.main.id
  validate_request_body       = false
  validate_request_parameters = true
}

# Gateway Response for missing required header (only affects authenticated routes)
resource "aws_api_gateway_gateway_response" "unauthorized" {
  count = var.origin_verify_secret != "" ? 1 : 0

  rest_api_id   = aws_api_gateway_rest_api.main.id
  response_type = "UNAUTHORIZED"
  status_code   = "403"

  response_templates = {
    "application/json" = "{\"message\":\"Direct access not allowed. Use api.frogedu.org\"}"
  }
}

# Deployment
resource "aws_api_gateway_deployment" "main" {
  rest_api_id = aws_api_gateway_rest_api.main.id

  triggers = {
    redeployment = sha1(jsonencode([
      aws_api_gateway_rest_api.main.id,
      aws_api_gateway_method.options.id,
      aws_api_gateway_integration.options.id,
    ]))
  }

  lifecycle {
    create_before_destroy = true
  }

  depends_on = [
    aws_api_gateway_method.options,
    aws_api_gateway_integration.options,
  ]
}

# Stage
resource "aws_api_gateway_stage" "main" {
  deployment_id = aws_api_gateway_deployment.main.id
  rest_api_id   = aws_api_gateway_rest_api.main.id
  stage_name    = var.environment
}

# Method Settings (free tier only)
resource "aws_api_gateway_method_settings" "main" {
  rest_api_id = aws_api_gateway_rest_api.main.id
  stage_name  = aws_api_gateway_stage.main.stage_name
  method_path = "*/*"

  settings {
    metrics_enabled        = true
    logging_level          = "OFF"
    data_trace_enabled     = false
    throttling_rate_limit  = 1000
    throttling_burst_limit = 2000
    caching_enabled        = false
  }
}

# CORS OPTIONS method
resource "aws_api_gateway_method" "options" {
  rest_api_id   = aws_api_gateway_rest_api.main.id
  resource_id   = aws_api_gateway_rest_api.main.root_resource_id
  http_method   = "OPTIONS"
  authorization = "NONE"
}

resource "aws_api_gateway_integration" "options" {
  rest_api_id = aws_api_gateway_rest_api.main.id
  resource_id = aws_api_gateway_rest_api.main.root_resource_id
  http_method = aws_api_gateway_method.options.http_method
  type        = "MOCK"

  request_templates = {
    "application/json" = "{\"statusCode\": 200}"
  }
}

resource "aws_api_gateway_method_response" "options" {
  rest_api_id = aws_api_gateway_rest_api.main.id
  resource_id = aws_api_gateway_rest_api.main.root_resource_id
  http_method = aws_api_gateway_method.options.http_method
  status_code = "200"

  response_parameters = {
    "method.response.header.Access-Control-Allow-Headers" = true
    "method.response.header.Access-Control-Allow-Methods" = true
    "method.response.header.Access-Control-Allow-Origin"  = true
  }
}

# =============================================================================
# Custom Domain Configuration
# =============================================================================

# Custom Domain Name
resource "aws_api_gateway_domain_name" "main" {
  count = var.custom_domain != "" ? 1 : 0

  domain_name              = var.custom_domain
  regional_certificate_arn = var.acm_certificate_arn

  endpoint_configuration {
    types = ["REGIONAL"]
  }

  tags = {
    Name        = "${var.project_name}-${var.environment}-api-domain"
    Environment = var.environment
    Project     = var.project_name
  }
}

# Base Path Mapping - maps /api/* to the stage
resource "aws_api_gateway_base_path_mapping" "main" {
  count = var.custom_domain != "" ? 1 : 0

  api_id      = aws_api_gateway_rest_api.main.id
  stage_name  = aws_api_gateway_stage.main.stage_name
  domain_name = aws_api_gateway_domain_name.main[0].domain_name
  base_path   = "api"
}

resource "aws_api_gateway_integration_response" "options" {
  rest_api_id = aws_api_gateway_rest_api.main.id
  resource_id = aws_api_gateway_rest_api.main.root_resource_id
  http_method = aws_api_gateway_method.options.http_method
  status_code = "200"

  response_parameters = {
    "method.response.header.Access-Control-Allow-Headers" = "'Content-Type,Authorization,X-Amz-Date'"
    "method.response.header.Access-Control-Allow-Methods" = "'GET,POST,PUT,DELETE,OPTIONS'"
    "method.response.header.Access-Control-Allow-Origin"  = "'*'"
  }

  depends_on = [aws_api_gateway_integration.options]
}
