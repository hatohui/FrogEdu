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
