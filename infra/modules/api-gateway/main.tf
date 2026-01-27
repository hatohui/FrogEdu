resource "aws_api_gateway_rest_api" "api_gateway" {
  name        = "${var.project_name}-api"
  description = "API Gateway for ${var.project_name} project"

  endpoint_configuration {
    types = ["REGIONAL"]
  }
}

resource "aws_api_gateway_resource" "root" {
  rest_api_id = aws_api_gateway_rest_api.api_gateway.id
  parent_id   = aws_api_gateway_rest_api.api_gateway.root_resource_id
  path_part   = "api"
}

# =============================================================================
# Cognito Authorizer
# =============================================================================
resource "aws_api_gateway_authorizer" "cognito" {
  name            = "${var.project_name}-cognito-authorizer"
  rest_api_id     = aws_api_gateway_rest_api.api_gateway.id
  type            = "COGNITO_USER_POOLS"
  provider_arns   = [var.cognito_user_pool_arn]
  identity_source = "method.request.header.Authorization"

  # Cache authorizer results for 5 minutes (300s)
  # Reduces Lambda invocations and stays within free tier
  authorizer_result_ttl_in_seconds = 300
}
