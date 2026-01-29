
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
  # Domain configuration
  api_domain      = try(data.doppler_secrets.this.map.TF_API_DOMAIN, "")
  frontend_domain = try(data.doppler_secrets.this.map.TF_FRONTEND_DOMAIN, "localhost:5173")

  # Database configuration
  class_db_connection_string        = data.doppler_secrets.this.map.CLASS_DB_CONNECTION_STRING
  subscription_db_connection_string = data.doppler_secrets.this.map.SUBSCRIPTION_DB_CONNECTION_STRING
  user_db_connection_string         = data.doppler_secrets.this.map.USER_DB_CONNECTION_STRING
  exam_db_connection_string         = data.doppler_secrets.this.map.EXAM_DB_CONNECTION_STRING
}
