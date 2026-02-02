module "ses" {
  source  = "cloudposse/ses/aws"
  version = "0.25.1"

  domain                = var.domain
  namespace             = var.namespace
  environment           = var.environment
  zone_id               = var.zone_id
  verify_domain         = var.verify_domain
  verify_dkim           = var.verify_dkim
  create_spf_record     = var.create_spf_record
  ses_user_enabled      = true
  ses_group_enabled     = false
  iam_permissions       = ["ses:SendRawEmail", "ses:SendEmail"]
  iam_allowed_resources = ["*"]
}
