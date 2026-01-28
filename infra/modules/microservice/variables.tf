### Microservice Metadata

variable "project_name" {
  description = "Name of the project, used for resource naming"
  type        = string
}

variable "service_name" {
  description = "Name of the microservice, used for resource naming"
  type        = string
}

### Permission Variabless

variable "lambda_role_arn" {
  description = "ARN of the IAM role to be assumed by the Lambda function"
  type        = string
}

### ECR Repository

variable "ecr_repository" {
  description = "ECR repository URL for the microservice Docker image"
  type        = string
}

### API Gateway Integration

variable "api_gateway_id" {
  description = "ID of the API Gateway to integrate with"
  type        = string
}

variable "api_gateway_execution_arn" {
  description = "Execution ARN of the API Gateway"
  type        = string
}

### Lambda Configuration
variable "environment_variables" {
  description = "Environment variables for Lambda function"
  type        = map(string)
  default     = {}
}

### Route Bypass

variable "no_auth_routes" {
  description = "List of routes that bypass authentication"
  type        = list(string)
  default     = []
}

### Cognito Authorizer

variable "cognito_authorizer_id" {
  description = "ID of the Cognito authorizer to use for authenticated routes"
  type        = string
  default     = ""
}
