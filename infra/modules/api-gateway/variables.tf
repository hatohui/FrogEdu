variable "project_name" {
  description = "Name of the project, used for resource naming"
  type        = string
}

variable "cognito_user_pool_arn" {
  description = "ARN of the Cognito User Pool for API Gateway authentication"
  type        = string
}

variable "origin_verify_secret" {
  description = "Secret header value for origin verification between CloudFront and API Gateway"
  type        = string
  sensitive   = true
}
