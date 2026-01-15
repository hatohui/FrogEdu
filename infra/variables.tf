# =============================================================================
# Global Variables - Required for Providers
# =============================================================================
# NOTE: Providers cannot use data sources or locals, so we need basic variables
# These will be pulled from Doppler and used in resources via locals

# =============================================================================
# Doppler Configuration (Required)
# =============================================================================

variable "doppler_token" {
  description = "Doppler service token for accessing secrets (set via TF_VAR_doppler_token)"
  type        = string
  sensitive   = true
}

variable "doppler_project" {
  description = "Doppler project name"
  type        = string
  default     = "frogedu"
}

variable "doppler_config" {
  description = "Doppler config name (dev, staging, production)"
  type        = string
  default     = "dev"
}

# =============================================================================
# Provider Configuration Variables
# =============================================================================
# These are needed for provider blocks which cannot use data sources
# Set via environment variables or they'll use defaults

variable "aws_region" {
  description = "AWS region (can be overridden via TF_VAR_aws_region, otherwise uses Doppler value)"
  type        = string
  default     = "ap-southeast-1"
}

variable "environment" {
  description = "Environment name (can be overridden via TF_VAR_environment, otherwise uses Doppler value)"
  type        = string
  default     = "dev"
}

variable "project_name" {
  description = "Project name (can be overridden via TF_VAR_project_name, otherwise uses Doppler value)"
  type        = string
  default     = "frogedu"
}

variable "AWS_ACCESS_KEY_ID" {
  description = "AWS access key ID (pulled from Doppler via TF_VAR_AWS_ACCESS_KEY_ID)"
  type        = string
  sensitive   = true
}

variable "AWS_SECRET_ACCESS_KEY" {
  description = "AWS secret access key (pulled from Doppler via TF_VAR_AWS_SECRET_ACCESS_KEY)"
  type        = string
  sensitive   = true
}
