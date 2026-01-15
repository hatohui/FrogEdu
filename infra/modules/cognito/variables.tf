# =============================================================================
# Cognito Module - Variable Declarations
# =============================================================================

variable "project_name" {
  description = "Name of the project, used for resource naming"
  type        = string
}

variable "environment" {
  description = "Environment name (e.g., dev, staging, production)"
  type        = string
}

variable "aws_region" {
  description = "AWS region where resources will be created"
  type        = string
}

variable "frontend_domain" {
  description = "Frontend domain for OAuth callbacks (e.g., frogedu.org)"
  type        = string
  default     = "localhost:5173"
}
