# =============================================================================
# Cognito Module - Output Declarations
# =============================================================================

output "user_pool_id" {
  description = "ID of the Cognito User Pool"
  value       = aws_cognito_user_pool.main.id
}

output "user_pool_arn" {
  description = "ARN of the Cognito User Pool"
  value       = aws_cognito_user_pool.main.arn
}

output "user_pool_endpoint" {
  description = "Endpoint of the Cognito User Pool"
  value       = aws_cognito_user_pool.main.endpoint
}

output "web_client_id" {
  description = "ID of the Cognito User Pool web client"
  value       = aws_cognito_user_pool_client.web_client.id
}

output "domain" {
  description = "Cognito User Pool domain"
  value       = aws_cognito_user_pool_domain.main.domain
}

output "issuer_url" {
  description = "OIDC issuer URL for the Cognito User Pool"
  value       = "https://cognito-idp.${var.aws_region}.amazonaws.com/${aws_cognito_user_pool.main.id}"
}

output "user_pool" {
  description = "Complete User Pool object with id, arn, and endpoint"
  value = {
    id       = aws_cognito_user_pool.main.id
    arn      = aws_cognito_user_pool.main.arn
    endpoint = aws_cognito_user_pool.main.endpoint
  }
}
