# =============================================================================
# Outputs - Main Infrastructure Values
# =============================================================================

output "environment" {
  description = "Current environment name"
  value       = local.environment
}

output "cors_origins" {
  description = "CORS allowed origins for API Gateway"
  value       = var.cors_origins
}

output "aws_region" {
  description = "AWS region"
  value       = local.aws_region
}

# =============================================================================
# Cognito
# =============================================================================

output "cognito_user_pool_id" {
  description = "Cognito User Pool ID (for frontend)"
  value       = module.cognito.user_pool_id
}

output "cognito_client_id" {
  description = "Cognito Web Client ID (for frontend)"
  value       = module.cognito.web_client_id
}

output "cognito_issuer_url" {
  description = "Cognito JWT issuer URL"
  value       = module.cognito.issuer_url
}

# =============================================================================
# CloudFront
# =============================================================================

output "cloudfront_domain" {
  description = "CloudFront distribution domain (use this for DNS CNAME)"
  value       = module.cloudfront.distribution_domain
}

output "cloudfront_distribution_id" {
  description = "CloudFront distribution ID"
  value       = module.cloudfront.distribution_id
}

output "acm_certificate_arn" {
  description = "ACM certificate ARN for custom domain"
  value       = module.cloudfront.acm_certificate_arn
  sensitive   = true
}

output "acm_certificate_validation_records" {
  description = "DNS records needed to validate the ACM certificate"
  value       = module.cloudfront.acm_certificate_validation_records
  sensitive   = true
}
# =============================================================================
# ECR
# =============================================================================

output "ecr_repositories" {
  description = "ECR repository URLs for Docker images"
  value       = module.ecr.repository_urls
}

output "ecr_registry_id" {
  description = "ECR registry ID (AWS account ID)"
  value       = module.ecr.registry_id
}

# =============================================================================
# IAM - GitHub Actions
# =============================================================================

output "github_actions_role_arn" {
  description = "ARN of the GitHub Actions role for ECR access (add this to GitHub secrets)"
  value       = module.iam.github_actions_ecr_role_arn
}

output "github_oidc_provider_arn" {
  description = "ARN of the GitHub Actions OIDC provider"
  value       = module.iam.github_oidc_provider_arn
}

# =============================================================================
# API Gateway
# =============================================================================
output "api_gateway_domain" {
  description = "Domain name of the API Gateway"
  value       = module.api_gateway.api_gateway_domain
}

output "api_gateway_id" {
  description = "ID of the API Gateway"
  value       = module.api_gateway.api_gateway_id
}

# =============================================================================
# Frontend Environment Variables
# =============================================================================

output "frontend_env_vars" {
  description = "Environment variables for frontend .env file"
  value = {
    VITE_API_URL              = nonsensitive("https://api.frogedu.org")
    VITE_COGNITO_USER_POOL_ID = module.cognito.user_pool_id
    VITE_COGNITO_CLIENT_ID    = module.cognito.web_client_id
    VITE_AWS_REGION           = local.aws_region
  }
}

# =============================================================================
# SES
# =============================================================================

output "ses_domain_identity_arn" {
  description = "ARN of the SES domain identity"
  value       = module.ses.ses_domain_identity_arn
}

output "ses_domain_identity_verification_token" {
  description = "SES domain verification token - Add as TXT record to DNS"
  value       = module.ses.ses_domain_identity_verification_token
}

output "ses_dkim_tokens" {
  description = "DKIM tokens - Add as CNAME records to DNS"
  value       = module.ses.ses_dkim_tokens
}

output "ses_smtp_password" {
  description = "SMTP password for SES (use with ses_access_key_id for SMTP)"
  value       = module.ses.ses_smtp_password
  sensitive   = true
}

output "ses_access_key_id" {
  description = "Access Key ID for SES API/SMTP"
  value       = module.ses.access_key_id
  sensitive   = true
}

output "ses_secret_access_key" {
  description = "Secret Access Key for SES API"
  value       = module.ses.secret_access_key
  sensitive   = true
}

output "ses_user_arn" {
  description = "ARN of the IAM user created for SES"
  value       = module.ses.user_arn
}

output "ses_spf_record" {
  description = "SPF record for domain - Add as TXT record to DNS"
  value       = module.ses.spf_record
}
