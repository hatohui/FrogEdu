resource "aws_apigatewayv2_api" "api_gateway" {
  name          = "${var.project_name}-api"
  description   = "API Gateway for ${var.project_name} project"
  protocol_type = "HTTP"

  cors_configuration {
    allow_origins = var.cors_origins
    allow_methods = ["*"]
    allow_headers = ["Authorization", "Content-Type"]
  }

  disable_execute_api_endpoint = false
}

