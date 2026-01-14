
# =============================================================================
# Doppler Integration - Single Source of Truth
# =============================================================================
# All configuration values are pulled from Doppler (no tfvars needed)
data "doppler_secrets" "this" {
  project = var.doppler_project
  config  = var.doppler_config
}

# Local values derived from Doppler secrets
locals {
  # Environment configuration
  environment  = try(data.doppler_secrets.this.map.TF_ENVIRONMENT, "dev")
  project_name = try(data.doppler_secrets.this.map.TF_PROJECT_NAME, "frogedu")
  aws_region   = try(data.doppler_secrets.this.map.TF_AWS_REGION, "ap-southeast-1")

  # Lambda configuration
  lambda_package_type  = try(data.doppler_secrets.this.map.TF_LAMBDA_PACKAGE_TYPE, "Image")
  lambda_timeout       = try(tonumber(data.doppler_secrets.this.map.TF_LAMBDA_TIMEOUT), 30)
  lambda_memory_size   = try(tonumber(data.doppler_secrets.this.map.TF_LAMBDA_MEMORY_SIZE), 512)
  lambda_architectures = try(split(",", data.doppler_secrets.this.map.TF_LAMBDA_ARCHITECTURES), ["arm64"])
}