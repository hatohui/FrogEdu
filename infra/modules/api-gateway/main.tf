# =============================================================================
# API Gateway REST API
# =============================================================================

resource "aws_api_gateway_rest_api" "api_gateway" {
  name        = "${var.project_name}-api"
  description = "API Gateway for ${var.project_name} project"

  endpoint_configuration {
    types = ["REGIONAL"]
  }

  disable_execute_api_endpoint = false
}

# =============================================================================
# API Gateway Resources
# =============================================================================

resource "aws_api_gateway_resource" "api" {
  rest_api_id = aws_api_gateway_rest_api.api_gateway.id
  parent_id   = aws_api_gateway_rest_api.api_gateway.root_resource_id
  path_part   = "api"
}
