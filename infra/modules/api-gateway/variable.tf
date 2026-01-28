variable "project_name" {
  description = "Name of the project, used for resource naming"
  type        = string
}

### CORS Configuration
variable "cors_origins" {
  description = "List of allowed CORS origins for API Gateway"
  type        = list(string)
  default     = ["http://localhost:5173", "http://localhost:5174", "https://frogedu.org", "https://www.frogedu.org"]
}

### Cognito Configuration
variable "cognito_user_pool_id" {
  description = "Cognito User Pool ID for authentication"
  type        = string
  default     = ""
}

variable "cognito_user_pool_arn" {
  description = "Cognito User Pool ARN for authentication"
  type        = string
  default     = ""
}

variable "cognito_issuer_url" {
  description = "Cognito issuer URL for JWT validation"
  type        = string
  default     = ""
}

variable "cognito_web_client_id" {
  description = "Cognito web client ID for JWT audience validation"
  type        = string
  default     = ""
}
