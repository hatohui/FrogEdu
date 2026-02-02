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

variable "zone_id" {
  description = "Route53 zone ID for DNS verification records (optional)"
  type        = string
  default     = ""
}

variable "verify_domain" {
  description = "Whether to verify the domain (requires zone_id)"
  type        = bool
  default     = false
}

variable "verify_dkim" {
  description = "Whether to verify DKIM (requires zone_id)"
  type        = bool
  default     = false
}

variable "create_spf_record" {
  description = "Whether to create SPF record"
  type        = bool
  default     = false
}
