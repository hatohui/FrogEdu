variable "domain" {
  description = "The domain to create the SES identity for"
  type        = string
}

variable "namespace" {
  description = "Namespace for naming resources (e.g., 'frogedu')"
  type        = string
}

variable "environment" {
  description = "Environment name (e.g., 'dev', 'prod')"
  type        = string
}



variable "mail_from_subdomain" {
  description = "Subdomain to use for MAIL FROM (e.g., 'mail' or 'bounce'). Leave empty to disable custom MAIL FROM."
  type        = string
  default     = ""
}

variable "iam_allowed_resources" {
  description = "List of ARNs that the SES user is allowed to send email from"
  type        = list(string)
  default     = ["*"]
}
