# SES Domain Identity
resource "aws_ses_domain_identity" "main" {
  domain = var.domain
}

# SES Domain DKIM
resource "aws_ses_domain_dkim" "main" {
  domain = aws_ses_domain_identity.main.domain
}

# SES Configuration Set
resource "aws_ses_configuration_set" "main" {
  name = "${var.namespace}-${var.environment}"

  delivery_options {
    tls_policy = "Require"
  }

  reputation_metrics_enabled = true
  sending_enabled            = true
}

# Custom MAIL FROM Domain (optional)
resource "aws_ses_domain_mail_from" "main" {
  count = var.mail_from_subdomain != "" ? 1 : 0

  domain           = aws_ses_domain_identity.main.domain
  mail_from_domain = "${var.mail_from_subdomain}.${var.domain}"

  behavior_on_mx_failure = "UseDefaultValue"
}

# Note: DNS records must be added manually to Cloudflare
# See outputs for the exact DNS records to create

# Data source to get current AWS region
data "aws_region" "current" {}

# IAM User for SES (for SMTP credentials)
resource "aws_iam_user" "ses_user" {
  name = "${var.namespace}-${var.environment}-ses-user"
  path = "/ses/"

  tags = {
    Environment = var.environment
    Namespace   = var.namespace
    Service     = "ses"
  }
}

# IAM Access Key for SES User
resource "aws_iam_access_key" "ses_user" {
  user = aws_iam_user.ses_user.name
}

# IAM Policy for SES User
resource "aws_iam_user_policy" "ses_user" {
  name = "${var.namespace}-${var.environment}-ses-policy"
  user = aws_iam_user.ses_user.name

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "ses:SendEmail",
          "ses:SendRawEmail"
        ]
        Resource = var.iam_allowed_resources
      }
    ]
  })
}
