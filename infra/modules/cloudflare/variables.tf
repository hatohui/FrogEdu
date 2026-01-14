# =============================================================================
# Cloudflare Module - Variable Declarations
# =============================================================================

variable "project_name" {
  description = "Name of the project, used for resource naming"
  type        = string
}

variable "environment" {
  description = "Environment name (e.g., dev, staging, production)"
  type        = string
}

variable "cloudflare_account_id" {
  description = "Cloudflare account ID for R2 bucket creation"
  type        = string
}

variable "cloudflare_account_name" {
  description = "Cloudflare account name (for documentation purposes)"
  type        = string
}
