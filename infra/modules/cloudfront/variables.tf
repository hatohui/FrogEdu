# =============================================================================
# CloudFront Module - Variable Declarations
# =============================================================================

variable "project_name" {
  description = "Name of the project, used for resource naming"
  type        = string
}

variable "environment" {
  description = "Environment name (e.g., dev, staging, production)"
  type        = string
}

variable "api_gateway_domain" {
  description = "Domain name of the API Gateway origin"
  type        = string
}

variable "api_gateway_stage" {
  description = "API Gateway stage name to use as origin path"
  type        = string
}

variable "origin_verify_secret" {
  description = "Secret header value for origin verification between CloudFront and API Gateway"
  type        = string
  sensitive   = true
}

variable "custom_domain" {
  description = "Custom domain name for the API (e.g., frogedu.org). Leave empty to use CloudFront default domain"
  type        = string
  default     = ""
}
