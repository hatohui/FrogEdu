# =============================================================================
# Cloudflare Module - R2 Storage
# =============================================================================

# Note: cloudflare_accounts data source removed as we already have account_id from Doppler
# data "cloudflare_accounts" "main" {
#   name = var.cloudflare_account_name
# }

resource "cloudflare_r2_bucket" "assets" {
  account_id    = var.cloudflare_account_id
  name          = "${var.project_name}-${var.environment}-assets"
  location      = "apac"
  storage_class = "Standard"
}

resource "cloudflare_r2_bucket_lifecycle" "assets" {
  account_id  = var.cloudflare_account_id
  bucket_name = cloudflare_r2_bucket.assets.name

  rules = [{
    id      = "temp-cleanup"
    enabled = true
    conditions = {
      prefix = "temp/"
    }
    delete_objects_transition = {
      condition = {
        type    = "Age"
        max_age = 86400
      }
    }
  }]
}
