# =============================================================================
# CloudFront Module - Output Declarations
# =============================================================================

output "distribution_id" {
  description = "ID of the CloudFront distribution"
  value       = aws_cloudfront_distribution.api.id
}

output "distribution_domain" {
  description = "Domain name of the CloudFront distribution"
  value       = aws_cloudfront_distribution.api.domain_name
}

output "distribution_arn" {
  description = "ARN of the CloudFront distribution"
  value       = aws_cloudfront_distribution.api.arn
}

output "acm_certificate_arn" {
  description = "ACM certificate ARN for custom domain (if configured)"
  value       = var.custom_domain != "" ? aws_acm_certificate.api[0].arn : null
}

output "acm_certificate_validation_records" {
  description = "DNS validation records for ACM certificate"
  value = var.custom_domain != "" ? {
    for dvo in aws_acm_certificate.api[0].domain_validation_options : dvo.domain_name => {
      name   = dvo.resource_record_name
      type   = dvo.resource_record_type
      value  = dvo.resource_record_value
    }
  } : {}
}
