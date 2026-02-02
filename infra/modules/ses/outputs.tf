output "ses_domain_identity_arn" {
  description = "ARN of the SES domain identity"
  value       = aws_ses_domain_identity.main.arn
}

output "ses_domain_identity_verification_token" {
  description = "SES domain verification token for TXT record"
  value       = aws_ses_domain_identity.main.verification_token
}

output "ses_dkim_tokens" {
  description = "DKIM tokens for DNS CNAME records"
  value       = aws_ses_domain_dkim.main.dkim_tokens
}

output "ses_smtp_password" {
  description = "SMTP password for sending emails"
  value       = aws_iam_access_key.ses_user.ses_smtp_password_v4
  sensitive   = true
}

output "access_key_id" {
  description = "AWS Access Key ID for SES API"
  value       = aws_iam_access_key.ses_user.id
  sensitive   = true
}

output "secret_access_key" {
  description = "AWS Secret Access Key for SES API"
  value       = aws_iam_access_key.ses_user.secret
  sensitive   = true
}

output "user_arn" {
  description = "ARN of the IAM user created for SES"
  value       = aws_iam_user.ses_user.arn
}

output "user_name" {
  description = "Name of the IAM user created for SES"
  value       = aws_iam_user.ses_user.name
}

output "configuration_set_name" {
  description = "Name of the SES configuration set"
  value       = aws_ses_configuration_set.main.name
}

output "configuration_set_arn" {
  description = "ARN of the SES configuration set"
  value       = aws_ses_configuration_set.main.arn
}

output "mail_from_domain" {
  description = "The custom MAIL FROM domain"
  value       = var.mail_from_subdomain != "" ? aws_ses_domain_mail_from.main[0].mail_from_domain : ""
}

output "domain" {
  description = "The domain identity"
  value       = aws_ses_domain_identity.main.domain
}

output "spf_record" {
  description = "SPF record for the domain"
  value       = var.mail_from_subdomain != "" ? "v=spf1 include:amazonses.com ~all" : "v=spf1 include:amazonses.com -all"
}

# DNS Records for Cloudflare Configuration
output "dns_records_for_cloudflare" {
  description = "DNS records to add manually in Cloudflare"
  value = {
    domain_verification = {
      type    = "TXT"
      name    = "_amazonses.${var.domain}"
      value   = aws_ses_domain_identity.main.verification_token
      ttl     = 600
      comment = "SES Domain Verification"
    }
    dkim_records = [
      for i, token in aws_ses_domain_dkim.main.dkim_tokens : {
        type    = "CNAME"
        name    = "${token}._domainkey"
        value   = "${token}.dkim.amazonses.com"
        ttl     = 600
        comment = "DKIM Record ${i + 1} of 3"
      }
    ]
    mail_from_mx = var.mail_from_subdomain != "" ? {
      type     = "MX"
      name     = "${var.mail_from_subdomain}.${var.domain}"
      value    = "10 feedback-smtp.${data.aws_region.current.id}.amazonses.com"
      ttl      = 600
      priority = 10
      comment  = "Custom MAIL FROM - MX Record"
    } : null
    mail_from_spf = var.mail_from_subdomain != "" ? {
      type    = "TXT"
      name    = "${var.mail_from_subdomain}.${var.domain}"
      value   = "v=spf1 include:amazonses.com ~all"
      ttl     = 600
      comment = "Custom MAIL FROM - SPF Record"
    } : null
  }
}
