# =============================================================================
# API Gateway Module - Outputs
# =============================================================================

output "api_id" {
  description = "REST API ID"
  value       = aws_api_gateway_rest_api.main.id
}

output "root_resource_id" {
  description = "REST API root resource ID"
  value       = aws_api_gateway_rest_api.main.root_resource_id
}

output "execution_arn" {
  description = "Execution ARN for Lambda permissions"
  value       = aws_api_gateway_rest_api.main.execution_arn
}

output "invoke_url" {
  description = "API Gateway invoke URL"
  value       = aws_api_gateway_stage.main.invoke_url
}

output "authorizer_id" {
  description = "Cognito authorizer ID"
  value       = aws_api_gateway_authorizer.cognito.id
}

output "cognito_authorizer_id" {
  description = "Cognito authorizer ID (alias for compatibility)"
  value       = aws_api_gateway_authorizer.cognito.id
}

output "api_domain" {
  description = "API Gateway domain name"
  value       = replace(aws_api_gateway_stage.main.invoke_url, "/^https?://([^/]*).*/", "$1")
}
