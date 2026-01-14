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
