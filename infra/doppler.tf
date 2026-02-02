
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
  ses_domain      = try(data.doppler_secrets.this.map.TF_SES_DOMAIN, "frogedu.org")

  # Database configuration
  class_db_connection_string        = data.doppler_secrets.this.map.CLASS_DB_CONNECTION_STRING
  subscription_db_connection_string = data.doppler_secrets.this.map.SUBSCRIPTION_DB_CONNECTION_STRING
  user_db_connection_string         = data.doppler_secrets.this.map.USER_DB_CONNECTION_STRING
  exam_db_connection_string         = data.doppler_secrets.this.map.EXAM_DB_CONNECTION_STRING

  #license key
  mediak_license_key = data.doppler_secrets.this.map.MEDIAK_LICENSE_KEY

  #r2 configuration
  r2_access_key_id     = data.doppler_secrets.this.map.R2_ACCESS_KEY
  r2_secret_access_key = data.doppler_secrets.this.map.R2_SECRET_KEY
  r2_bucket_name       = data.doppler_secrets.this.map.R2_BUCKET_NAME
  r2_account_id        = data.doppler_secrets.this.map.R2_ACCOUNT_ID
  r2_public_endpoint   = data.doppler_secrets.this.map.R2_PUBLIC_ENDPOINT
}
