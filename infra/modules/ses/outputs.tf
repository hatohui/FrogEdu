output "ses_domain_identity_arn" {
  description = "ARN of the SES domain identity"
  value       = module.ses.ses_domain_identity_arn
}

output "ses_domain_identity_verification_token" {
  description = "SES domain verification token for TXT record"
  value       = module.ses.ses_domain_identity_verification_token
}

output "ses_dkim_tokens" {
  description = "DKIM tokens for DNS CNAME records"
  value       = module.ses.ses_dkim_tokens
}

output "ses_smtp_password" {
  description = "SMTP password for sending emails"
  value       = module.ses.ses_smtp_password
  sensitive   = true
}

output "access_key_id" {
  description = "AWS Access Key ID for SES API"
  value       = module.ses.access_key_id
  sensitive   = true
}

output "secret_access_key" {
  description = "AWS Secret Access Key for SES API"
  value       = module.ses.secret_access_key
  sensitive   = true
}

output "user_arn" {
  description = "ARN of the IAM user created for SES"
  value       = module.ses.user_arn
}

output "spf_record" {
  description = "SPF record for the domain"
  value       = module.ses.spf_record
}
