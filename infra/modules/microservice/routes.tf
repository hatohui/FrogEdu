resource "aws_apigatewayv2_route" "default_route" {
  for_each = toset(["GET", "POST", "PUT", "DELETE", "PATCH", "HEAD"])

  api_id             = var.api_gateway_id
  route_key          = "${each.value} /api/${var.service_name}/{proxy+}"
  target             = "integrations/${aws_apigatewayv2_integration.default_integration.id}"
  authorization_type = var.cognito_authorizer_id != "" ? "JWT" : "NONE"
  authorizer_id      = var.cognito_authorizer_id != "" ? var.cognito_authorizer_id : null
}

resource "aws_apigatewayv2_route" "options_bypass" {
  api_id             = var.api_gateway_id
  route_key          = "OPTIONS /api/${var.service_name}/{proxy+}"
  authorization_type = "NONE"

  target = "integrations/${aws_apigatewayv2_integration.default_integration.id}"
}

resource "aws_apigatewayv2_route" "no_auth_routes" {
  for_each = toset([for route in var.no_auth_routes : route if route != "/"])

  api_id             = var.api_gateway_id
  route_key          = "ANY /api/${var.service_name}${each.value}"
  target             = "integrations/${aws_apigatewayv2_integration.default_integration.id}"
  authorization_type = "NONE"
}


