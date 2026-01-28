output "api_gateway_domain" {
  description = "Domain name of the API Gateway"
  value       = aws_apigatewayv2_api.api_gateway.api_endpoint
}

output "api_gateway_id" {
  description = "ID of the API Gateway"
  value       = aws_apigatewayv2_api.api_gateway.id
}

output "api_gateway_execution_arn" {
  description = "Execution ARN of the API Gateway"
  value       = aws_apigatewayv2_api.api_gateway.execution_arn
}

output "cognito_authorizer_id" {
  description = "ID of the Cognito authorizer"
  value       = try(aws_apigatewayv2_authorizer.cognito[0].id, "")
}

