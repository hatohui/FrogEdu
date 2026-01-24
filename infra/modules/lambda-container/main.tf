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
resource "aws_api_gateway_resource" "parents" {
  for_each = toset(local.parent_paths)

  rest_api_id = var.api_gateway_id
  parent_id   = var.api_gateway_root_id
  path_part   = each.value
}

# Create child resources (e.g., /{proxy+})
resource "aws_api_gateway_resource" "children" {
  for_each = local.routes_with_children

  rest_api_id = var.api_gateway_id
  parent_id   = aws_api_gateway_resource.parents[each.value.parent].id
  path_part   = each.value.child_part
}

resource "aws_api_gateway_method" "routes" {
  for_each = { for idx, route in var.routes : route.path => route }

  rest_api_id   = var.api_gateway_id
  resource_id   = local.parsed_routes[each.key].has_child ? aws_api_gateway_resource.children[each.key].id : aws_api_gateway_resource.parents[local.parsed_routes[each.key].parent].id
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
  resource_id             = local.parsed_routes[each.key].has_child ? aws_api_gateway_resource.children[each.key].id : aws_api_gateway_resource.parents[local.parsed_routes[each.key].parent].id
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
# CORS OPTIONS Support for Proxy Routes
# =============================================================================
# OPTIONS requests are forwarded to Lambda so the .NET CORS middleware can
# dynamically return the correct Access-Control-Allow-Origin based on the
# incoming Origin header. This supports multiple origins (localhost, www, etc.)

# OPTIONS method for parent resources (e.g., /users, /contents)
# Forwards to Lambda for dynamic CORS handling
resource "aws_api_gateway_method" "options_parent" {
  for_each = toset(local.parent_paths)

  rest_api_id   = var.api_gateway_id
  resource_id   = aws_api_gateway_resource.parents[each.value].id
  http_method   = "OPTIONS"
  authorization = "NONE"
}

resource "aws_api_gateway_integration" "options_parent" {
  for_each = toset(local.parent_paths)

  rest_api_id             = var.api_gateway_id
  resource_id             = aws_api_gateway_resource.parents[each.value].id
  http_method             = aws_api_gateway_method.options_parent[each.value].http_method
  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.this.invoke_arn
}

# OPTIONS method for child resources (e.g., /{proxy+})
# Forwards to Lambda for dynamic CORS handling
resource "aws_api_gateway_method" "options_child" {
  for_each = local.routes_with_children

  rest_api_id   = var.api_gateway_id
  resource_id   = aws_api_gateway_resource.children[each.key].id
  http_method   = "OPTIONS"
  authorization = "NONE"
}

resource "aws_api_gateway_integration" "options_child" {
  for_each = local.routes_with_children

  rest_api_id             = var.api_gateway_id
  resource_id             = aws_api_gateway_resource.children[each.key].id
  http_method             = aws_api_gateway_method.options_child[each.key].http_method
  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.this.invoke_arn
}

# =============================================================================
# Lambda Permissions
# =============================================================================
resource "aws_lambda_permission" "api_gateway" {
  for_each = { for idx, route in var.routes : route.path => route }

  # Sanitize path for statement_id: replace /, {, }, + with dashes
  statement_id  = "AllowAPIGatewayInvoke-${replace(replace(replace(replace(each.value.path, "/", "-"), "{", ""), "}", ""), "+", "plus")}"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.this.function_name
  principal     = "apigateway.amazonaws.com"
  source_arn    = "${var.api_gateway_execution_arn}/*/*"
}
