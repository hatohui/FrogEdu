resource "aws_apigatewayv2_api" "api_gateway" {
  name          = "${var.project_name}-api"
  description   = "API Gateway for ${var.project_name} project"
  protocol_type = "HTTP"

  cors_configuration {
    allow_origins     = var.cors_origins
    allow_methods     = ["GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS"]
    allow_headers     = ["Authorization", "Content-Type", "X-Amz-Date", "X-Api-Key", "X-Amz-Security-Token"]
    allow_credentials = true
    max_age           = 3600
  }

  disable_execute_api_endpoint = false
}

resource "aws_apigatewayv2_stage" "default" {
  api_id      = aws_apigatewayv2_api.api_gateway.id
  name        = "$default"
  auto_deploy = true
}

resource "aws_apigatewayv2_authorizer" "cognito" {
  count = var.cognito_user_pool_id != "" ? 1 : 0

  api_id           = aws_apigatewayv2_api.api_gateway.id
  authorizer_type  = "JWT"
  identity_sources = ["$request.header.Authorization"]
  name             = "${var.project_name}-cognito-authorizer"

  jwt_configuration {
    audience = var.cognito_web_client_id != "" ? [var.cognito_web_client_id] : []
    issuer   = var.cognito_issuer_url
  }
}
