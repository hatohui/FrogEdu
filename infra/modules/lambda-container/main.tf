# =============================================================================
# Lambda Container Module - Docker-based Lambda Functions
# =============================================================================

# CloudWatch Log Group for Lambda
resource "aws_cloudwatch_log_group" "lambda" {
  name              = "/aws/lambda/${var.project_name}-${var.environment}-${var.function_name}"
  retention_in_days = 7 # Free tier: 7 days or less

  tags = {
    Environment = var.environment
    Service     = var.function_name
  }
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

  depends_on = [aws_cloudwatch_log_group.lambda]
}

# API Gateway Resources and Methods
# Handle both regular paths and proxy+ paths
locals {
  # Parse all routes - handle {proxy+} pattern
  parsed_routes = {
    for route in var.routes : route.path => {
      segments   = split("/", route.path)
      parent     = split("/", route.path)[0]
      has_child  = length(split("/", route.path)) > 1
      child_part = length(split("/", route.path)) > 1 ? split("/", route.path)[1] : null
      is_proxy   = length(split("/", route.path)) > 1 && can(regex("\\{proxy\\+\\}", split("/", route.path)[1]))
      route_data = route
    }
  }

  # Get unique parent paths
  parent_paths = distinct([
    for route_key, route_data in local.parsed_routes : route_data.parent
  ])

  # Routes that have children (for creating child resources)
  routes_with_children = {
    for route_key, route_data in local.parsed_routes : route_key => route_data
    if route_data.has_child
  }
}

# Handle resource migration from old structure to new structure
moved {
  from = aws_api_gateway_resource.parent_routes
  to   = aws_api_gateway_resource.parents
}

moved {
  from = aws_api_gateway_resource.child_routes
  to   = aws_api_gateway_resource.children
}

# Create parent resources (e.g., /contents, /users)
# Only create if not provided via shared_parent_resources
resource "aws_api_gateway_resource" "parents" {
  for_each = toset([for path in local.parent_paths : path if !contains(keys(var.shared_parent_resources), path)])

  rest_api_id = var.api_gateway_id
  parent_id   = var.api_gateway_root_id
  path_part   = each.value
}

# Combine created and shared parent resources
locals {
  all_parent_resources = merge(
    { for k, v in aws_api_gateway_resource.parents : k => v.id },
    var.shared_parent_resources
  )
}

# Create child resources (e.g., /{proxy+})
resource "aws_api_gateway_resource" "children" {
  for_each = local.routes_with_children

  rest_api_id = var.api_gateway_id
  parent_id   = local.all_parent_resources[each.value.parent]
  path_part   = each.value.child_part
}

resource "aws_api_gateway_method" "routes" {
  for_each = { for idx, route in var.routes : route.path => route }

  rest_api_id   = var.api_gateway_id
  resource_id   = local.parsed_routes[each.key].has_child ? aws_api_gateway_resource.children[each.key].id : local.all_parent_resources[local.parsed_routes[each.key].parent]
  http_method   = each.value.http_method
  authorization = each.value.auth_required ? "COGNITO_USER_POOLS" : "NONE"
  authorizer_id = each.value.auth_required ? var.cognito_authorizer_id : null

  # Require X-Origin-Verify header only for auth-required routes when validation is enabled
  # This allows health checks and public endpoints to be accessed directly
  request_validator_id = var.request_validator_id != "" && each.value.auth_required ? var.request_validator_id : null
  request_parameters = var.origin_verify_secret != "" && each.value.auth_required ? {
    "method.request.header.X-Origin-Verify" = true
  } : {}
}

resource "aws_api_gateway_integration" "routes" {
  for_each = { for idx, route in var.routes : route.path => route }

  rest_api_id             = var.api_gateway_id
  resource_id             = local.parsed_routes[each.key].has_child ? aws_api_gateway_resource.children[each.key].id : local.all_parent_resources[local.parsed_routes[each.key].parent]
  http_method             = aws_api_gateway_method.routes[each.key].http_method
  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.this.invoke_arn

  # Pass header validation only for auth-required routes
  request_parameters = var.origin_verify_secret != "" && each.value.auth_required ? {
    "integration.request.header.X-Origin-Verify" = "method.request.header.X-Origin-Verify"
  } : {}
}

# =============================================================================
# CORS OPTIONS Support - Mock Integration (recommended)
# =============================================================================
# Using MOCK integration instead of Lambda for OPTIONS preflight requests
# Benefits: Faster response, no Lambda invocation cost, no cold starts

# OPTIONS method for parent resources (e.g., /contents, /users)
resource "aws_api_gateway_method" "options_parent" {
  for_each = toset([for path in local.parent_paths : path if !contains(keys(var.shared_parent_resources), path)])

  rest_api_id   = var.api_gateway_id
  resource_id   = local.all_parent_resources[each.value]
  http_method   = "OPTIONS"
  authorization = "NONE"
}

# Mock integration for parent OPTIONS - returns 200 immediately
resource "aws_api_gateway_integration" "options_parent" {
  for_each = toset([for path in local.parent_paths : path if !contains(keys(var.shared_parent_resources), path)])

  rest_api_id = var.api_gateway_id
  resource_id = local.all_parent_resources[each.value]
  http_method = aws_api_gateway_method.options_parent[each.value].http_method
  type        = "MOCK"

  request_templates = {
    "application/json" = "{\"statusCode\": 200}"
  }
}

# Method response for parent OPTIONS
resource "aws_api_gateway_method_response" "options_parent_200" {
  for_each = toset([for path in local.parent_paths : path if !contains(keys(var.shared_parent_resources), path)])

  rest_api_id = var.api_gateway_id
  resource_id = local.all_parent_resources[each.value]
  http_method = aws_api_gateway_method.options_parent[each.value].http_method
  status_code = "200"

  response_parameters = {
    "method.response.header.Access-Control-Allow-Headers" = true
    "method.response.header.Access-Control-Allow-Methods" = true
    "method.response.header.Access-Control-Allow-Origin"  = true
  }

  response_models = {
    "application/json" = "Empty"
  }
}

# Integration response for parent OPTIONS - returns CORS headers
resource "aws_api_gateway_integration_response" "options_parent_200" {
  for_each = toset([for path in local.parent_paths : path if !contains(keys(var.shared_parent_resources), path)])

  rest_api_id = var.api_gateway_id
  resource_id = local.all_parent_resources[each.value]
  http_method = aws_api_gateway_method.options_parent[each.value].http_method
  status_code = aws_api_gateway_method_response.options_parent_200[each.value].status_code

  response_parameters = {
    "method.response.header.Access-Control-Allow-Headers" = "'Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token'"
    "method.response.header.Access-Control-Allow-Methods" = "'DELETE,GET,HEAD,OPTIONS,PATCH,POST,PUT'"
    "method.response.header.Access-Control-Allow-Origin"  = "'*'"
  }

  depends_on = [aws_api_gateway_integration.options_parent]
}

# OPTIONS method for child resources (e.g., /{proxy+})
resource "aws_api_gateway_method" "options_child" {
  for_each = local.routes_with_children

  rest_api_id   = var.api_gateway_id
  resource_id   = aws_api_gateway_resource.children[each.key].id
  http_method   = "OPTIONS"
  authorization = "NONE"
}

# Mock integration for child OPTIONS
resource "aws_api_gateway_integration" "options_child" {
  for_each = local.routes_with_children

  rest_api_id = var.api_gateway_id
  resource_id = aws_api_gateway_resource.children[each.key].id
  http_method = aws_api_gateway_method.options_child[each.key].http_method
  type        = "MOCK"

  request_templates = {
    "application/json" = "{\"statusCode\": 200}"
  }
}

# Method response for child OPTIONS
resource "aws_api_gateway_method_response" "options_child_200" {
  for_each = local.routes_with_children

  rest_api_id = var.api_gateway_id
  resource_id = aws_api_gateway_resource.children[each.key].id
  http_method = aws_api_gateway_method.options_child[each.key].http_method
  status_code = "200"

  response_parameters = {
    "method.response.header.Access-Control-Allow-Headers" = true
    "method.response.header.Access-Control-Allow-Methods" = true
    "method.response.header.Access-Control-Allow-Origin"  = true
  }

  response_models = {
    "application/json" = "Empty"
  }
}

# Integration response for child OPTIONS - returns CORS headers
resource "aws_api_gateway_integration_response" "options_child_200" {
  for_each = local.routes_with_children

  rest_api_id = var.api_gateway_id
  resource_id = aws_api_gateway_resource.children[each.key].id
  http_method = aws_api_gateway_method.options_child[each.key].http_method
  status_code = aws_api_gateway_method_response.options_child_200[each.key].status_code

  response_parameters = {
    "method.response.header.Access-Control-Allow-Headers" = "'Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token'"
    "method.response.header.Access-Control-Allow-Methods" = "'DELETE,GET,HEAD,OPTIONS,PATCH,POST,PUT'"
    "method.response.header.Access-Control-Allow-Origin"  = "'*'"
  }

  depends_on = [aws_api_gateway_integration.options_child]
}

# =============================================================================
# Lambda Permissions
# =============================================================================

resource "aws_lambda_permission" "api_gateway" {
  for_each = { for idx, route in var.routes : route.path => route }

  statement_id  = "AllowAPIGatewayInvoke-${replace(replace(replace(replace(each.value.path, "/", "-"), "{", ""), "}", ""), "+", "plus")}"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.this.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${var.api_gateway_execution_arn}/*/*"
}
