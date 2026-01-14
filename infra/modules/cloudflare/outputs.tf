# =============================================================================
# Cloudflare Module - Output Declarations
# =============================================================================

output "bucket_name" {
  description = "Name of the R2 bucket"
  value       = cloudflare_r2_bucket.assets.name
}

output "r2_endpoint" {
  description = "R2 storage endpoint URL"
  value       = "https://${var.cloudflare_account_id}.r2.cloudflarestorage.com"
}
