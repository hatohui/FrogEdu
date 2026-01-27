output "execution_arn" {
  description = "Execution ARN of the API Gateway"
  value       = aws_api_gateway_rest_api.api_gateway.execution_arn
}

output "api_resource_id" {
  description = "Root resource ID of the API Gateway (api resource)"
  value       = aws_api_gateway_resource.root.id
}

output "api_gateway_id" {
  description = "ID of the API Gateway REST API"
  value       = aws_api_gateway_rest_api.api_gateway.id
}

output "request_validator_id" {
  description = "ID of the request validator for API Gateway"
  value       = aws_api_gateway_request_validator.request_validator.id
}

output "cognito_authorizer_id" {
  description = "ID of the Cognito authorizer for API Gateway"
  value       = aws_api_gateway_authorizer.cognito.id
}

