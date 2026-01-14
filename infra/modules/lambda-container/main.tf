# =============================================================================
# Lambda Container Module - Docker-based Lambda Functions
# =============================================================================

variable "function_name" {
  type        = string
  description = "Name of the Lambda function"
}

variable "ecr_image_uri" {
  type        = string
  description = "ECR image URI (e.g., 123456789.dkr.ecr.region.amazonaws.com/repo:tag)"
}

variable "project_name" {
  type = string
}

variable "environment" {
  type = string
}

variable "execution_role" {
  type = object({
    arn  = string
    name = string
  })
}

variable "memory_size" {
  type    = number
  default = 512
}

variable "timeout" {
  type    = number
  default = 30
}

variable "architectures" {
  type    = list(string)
  default = ["x86_64"]
}

variable "environment_variables" {
  type    = map(string)
  default = {}
}

variable "api_gateway_id" {
  type = string
}

variable "api_gateway_root_id" {
  type = string
}

variable "api_gateway_execution_arn" {
  type = string
}

variable "cognito_authorizer_id" {
  type = string
}

variable "routes" {
  type = list(object({
    path          = string
    http_method   = string
    auth_required = bool
  }))
  default = []
}

# Lambda Function
resource "aws_lambda_function" "this" {
  function_name = "${var.project_name}-${var.environment}-${var.function_name}"
  package_type  = "Image"
  image_uri     = var.ecr_image_uri
  role          = var.execution_role.arn

  architectures = var.architectures
  memory_size   = var.memory_size
  timeout       = var.timeout

  environment {
    variables = var.environment_variables
  }

  lifecycle {
    ignore_changes = [image_uri]
  }
}

# API Gateway Resources and Methods
resource "aws_api_gateway_resource" "routes" {
  for_each = { for idx, route in var.routes : route.path => route }

  rest_api_id = var.api_gateway_id
  parent_id   = var.api_gateway_root_id
  path_part   = each.value.path
}

resource "aws_api_gateway_method" "routes" {
  for_each = { for idx, route in var.routes : route.path => route }

  rest_api_id   = var.api_gateway_id
  resource_id   = aws_api_gateway_resource.routes[each.key].id
  http_method   = each.value.http_method
  authorization = each.value.auth_required ? "COGNITO_USER_POOLS" : "NONE"
  authorizer_id = each.value.auth_required ? var.cognito_authorizer_id : null
}

resource "aws_api_gateway_integration" "routes" {
  for_each = { for idx, route in var.routes : route.path => route }

  rest_api_id             = var.api_gateway_id
  resource_id             = aws_api_gateway_resource.routes[each.key].id
  http_method             = aws_api_gateway_method.routes[each.key].http_method
  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.this.invoke_arn
}

# Lambda Permissions
resource "aws_lambda_permission" "api_gateway" {
  for_each = { for idx, route in var.routes : route.path => route }

  statement_id  = "AllowAPIGatewayInvoke-${each.value.path}"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.this.function_name
  principal     = "apigateway.amazonaws.com"
  source_arn    = "${var.api_gateway_execution_arn}/*/*"
}

# Outputs
output "function_arn" {
  value = aws_lambda_function.this.arn
}

output "function_name" {
  value = aws_lambda_function.this.function_name
}

output "invoke_arn" {
  value = aws_lambda_function.this.invoke_arn
}
