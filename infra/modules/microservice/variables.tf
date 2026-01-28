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

variable "memory_size" {
  description = "Amount of memory in MB for Lambda function"
  type        = number
  default     = 1024
}

variable "timeout" {
  description = "Timeout in seconds for Lambda function"
  type        = number
  default     = 60
}

variable "environment_variables" {
  description = "Environment variables for Lambda function"
  type        = map(string)
  default     = {}
}
