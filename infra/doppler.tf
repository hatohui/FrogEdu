
# =============================================================================
# Doppler Integration - Single Source of Truth
# =============================================================================
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
  # lambda_package_type  = try(data.doppler_secrets.this.map.TF_LAMBDA_PACKAGE_TYPE, "Image")
  # lambda_timeout       = try(tonumber(data.doppler_secrets.this.map.TF_LAMBDA_TIMEOUT), 60)
  # lambda_memory_size   = try(tonumber(data.doppler_secrets.this.map.TF_LAMBDA_MEMORY_SIZE), 1024)
  # lambda_architectures = try(split(",", data.doppler_secrets.this.map.TF_LAMBDA_ARCHITECTURES), ["x86_64"])

  # Domain configuration
  api_domain      = try(data.doppler_secrets.this.map.TF_API_DOMAIN, "")
  frontend_domain = try(data.doppler_secrets.this.map.TF_FRONTEND_DOMAIN, "localhost:5173")
}
