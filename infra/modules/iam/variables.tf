# =============================================================================
# IAM Module - Variable Declarations
# =============================================================================

variable "project_name" {
  description = "Name of the project, used for resource naming"
  type        = string
}

variable "environment" {
  description = "Environment name (e.g., dev, staging, production)"
  type        = string
}

variable "github_org" {
  description = "GitHub organization name for OIDC trust"
  type        = string
  default     = "*"
}

variable "github_repo" {
  description = "GitHub repository name for OIDC trust (format: org/repo or * for all repos)"
  type        = string
  default     = "*"
}
variable "cognito_user_pool_arn" {
  description = "ARN of the Cognito User Pool for admin operations"
  type        = string
  default     = ""
}
