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
  description = "CloudFront distribution domain"
  value       = module.cloudfront.distribution_domain
}

output "cloudfront_distribution_id" {
  description = "CloudFront distribution ID"
  value       = module.cloudfront.distribution_id
}

# =============================================================================
# Cloudflare
# =============================================================================

output "r2_bucket_name" {
  description = "Cloudflare R2 bucket name"
  value       = module.cloudflare.bucket_name
}

output "r2_endpoint" {
  description = "Cloudflare R2 S3-compatible endpoint"
  value       = module.cloudflare.r2_endpoint
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
# Frontend Environment Variables
# =============================================================================

output "frontend_env_vars" {
  description = "Environment variables for frontend .env file"
  value = {
    VITE_API_GATEWAY_URL      = module.api_gateway.invoke_url
    VITE_COGNITO_USER_POOL_ID = module.cognito.user_pool_id
    VITE_COGNITO_CLIENT_ID    = module.cognito.web_client_id
    VITE_AWS_REGION           = local.aws_region
  }
}
