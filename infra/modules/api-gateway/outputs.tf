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

output "request_validator_id" {
  description = "Request validator ID for header validation"
  value       = var.origin_verify_secret != "" ? aws_api_gateway_request_validator.header_validator[0].id : ""
}

output "api_domain" {
  description = "API Gateway domain name"
  value       = replace(aws_api_gateway_stage.main.invoke_url, "/^https?://([^/]*).*/", "$1")
}

output "custom_domain_name" {
  description = "Custom domain name for API (if configured)"
  value       = var.custom_domain != "" ? aws_api_gateway_domain_name.main[0].domain_name : ""
}

output "custom_domain_target" {
  description = "Custom domain regional domain name for DNS configuration"
  value       = var.custom_domain != "" ? aws_api_gateway_domain_name.main[0].regional_domain_name : ""
}

output "custom_domain_hosted_zone_id" {
  description = "Custom domain hosted zone ID for Route53 alias"
  value       = var.custom_domain != "" ? aws_api_gateway_domain_name.main[0].regional_zone_id : ""
}
