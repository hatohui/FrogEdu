# =============================================================================
# Lambda Container Module - Variable Declarations
# =============================================================================

variable "function_name" {
  description = "Name of the Lambda function"
  type        = string
}

variable "ecr_image_uri" {
  description = "ECR image URI (e.g., 123456789.dkr.ecr.region.amazonaws.com/repo:tag)"
  type        = string
}

variable "project_name" {
  description = "Name of the project, used for resource naming"
  type        = string
}

variable "environment" {
  description = "Environment name (e.g., dev, staging, production)"
  type        = string
}

variable "execution_role" {
  description = "IAM execution role for the Lambda function"
  type = object({
    arn  = string
    name = string
  })
}

variable "memory_size" {
  description = "Amount of memory in MB allocated to the Lambda function"
  type        = number
  default     = 512
}

variable "timeout" {
  description = "Function timeout in seconds"
  type        = number
  default     = 30
}

variable "architectures" {
  description = "Instruction set architecture for the Lambda function"
  type        = list(string)
  default     = ["x86_64"]
}

variable "environment_variables" {
  description = "Environment variables to pass to the Lambda function"
  type        = map(string)
  default     = {}
}

variable "api_gateway_id" {
  description = "ID of the API Gateway REST API"
  type        = string
}

variable "api_gateway_root_id" {
  description = "Root resource ID of the API Gateway"
  type        = string
}

variable "api_gateway_execution_arn" {
  description = "Execution ARN of the API Gateway"
  type        = string
}

variable "cognito_authorizer_id" {
  description = "ID of the Cognito authorizer for API Gateway"
  type        = string
}

variable "request_validator_id" {
  description = "ID of the request validator for API Gateway (optional)"
  type        = string
  default     = ""
}

variable "origin_verify_secret" {
  description = "Secret value for X-Origin-Verify header to restrict access to CloudFront only"
  type        = string
  sensitive   = true
  default     = ""
}

variable "routes" {
  description = "List of API Gateway routes to create for this Lambda function"
  type = list(object({
    path          = string
    http_method   = string
    auth_required = bool
  }))
  default = []
}
