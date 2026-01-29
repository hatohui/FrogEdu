resource "aws_apigatewayv2_integration" "default_integration" {
  api_id             = var.api_gateway_id
  integration_uri    = aws_lambda_function.this.invoke_arn
  integration_type   = "AWS_PROXY"
  integration_method = "POST"

  request_parameters = {
    "overwrite:path" = "$request.path.proxy"
  }
}
