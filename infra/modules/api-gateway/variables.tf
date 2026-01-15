# =============================================================================
# API Gateway Module - Variables
# =============================================================================

variable "project_name" {
  description = "Project name for resource naming"
  type        = string
}

variable "environment" {
  description = "Environment name (dev, staging, production)"
  type        = string
}

variable "cognito_user_pool" {
  description = "Cognito user pool for authorization"
  type = object({
    id       = string
    arn      = string
    endpoint = string
  })
}

variable "custom_domain" {
  description = "Custom domain name for API (e.g., api.frogedu.org)"
  type        = string
  default     = ""
}

variable "acm_certificate_arn" {
  description = "ACM certificate ARN for custom domain (must be in same region as API Gateway)"
  type        = string
  default     = ""
}

variable "origin_verify_secret" {
  description = "Secret header value to verify requests come from CloudFront only"
  type        = string
  sensitive   = true
  default     = ""
}
