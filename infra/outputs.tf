# =============================================================================
# Outputs - Main Infrastructure Values
# =============================================================================

output "environment" {
  description = "Current environment name"
  value       = local.environment
}

output "aws_region" {
  description = "AWS region"
  value       = local.aws_region
}

# =============================================================================
# API Gateway
# =============================================================================

output "api_gateway_url" {
  description = "API Gateway invoke URL"
  value       = module.api_gateway.invoke_url
}

output "api_gateway_id" {
  description = "API Gateway REST API ID"
  value       = module.api_gateway.api_id
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
# Lambda Functions
# =============================================================================

output "lambda_functions" {
  description = "Lambda function ARNs"
  value = {
    content    = module.content_lambda.function_arn
    user       = module.user_lambda.function_arn
    assessment = module.assessment_lambda.function_arn
    ai         = module.ai_lambda.function_arn
  }
}

# =============================================================================
# Frontend Environment Variables
# =============================================================================

output "frontend_env_vars" {
  description = "Environment variables for frontend .env file"
  value = {
    VITE_API_URL              = nonsensitive("https://${local.api_domain}")
    VITE_COGNITO_USER_POOL_ID = module.cognito.user_pool_id
    VITE_COGNITO_CLIENT_ID    = module.cognito.web_client_id
    VITE_AWS_REGION           = local.aws_region
  }
}
