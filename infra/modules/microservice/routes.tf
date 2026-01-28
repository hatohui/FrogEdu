resource "aws_apigatewayv2_route" "default_route" {
  api_id             = var.api_gateway_id
  route_key          = "ANY /api/${var.service_name}/{proxy+}"
  target             = "integrations/${aws_apigatewayv2_integration.default_integration.id}"
  authorization_type = var.cognito_authorizer_id != "" ? "JWT" : "NONE"
  authorizer_id      = var.cognito_authorizer_id != "" ? var.cognito_authorizer_id : null
}

resource "aws_apigatewayv2_route" "no_auth_routes" {
  for_each = toset([for route in var.no_auth_routes : route if route != "/"])

  api_id             = var.api_gateway_id
  route_key          = "ANY /api/${var.service_name}${each.value}"
  target             = "integrations/${aws_apigatewayv2_integration.default_integration.id}"
  authorization_type = "NONE"
}

resource "aws_apigatewayv2_integration" "default_integration" {
  api_id             = var.api_gateway_id
  integration_uri    = aws_lambda_function.this.invoke_arn
  integration_type   = "AWS_PROXY"
  integration_method = "POST"

  request_parameters = {
    "overwrite:path" = "$request.path.proxy"
  }
}
