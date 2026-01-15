# =============================================================================
# Lambda Container Module - Docker-based Lambda Functions
# =============================================================================

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
# Parse nested paths and create parent resources
locals {
  # Split paths into segments
  route_segments = {
    for route in var.routes : route.path => split("/", route.path)
  }

  # Get unique parent paths (first segment only)
  parent_paths = distinct([for route in var.routes : split("/", route.path)[0]])

  # Separate parent routes and child routes
  parent_routes = {
    for route in var.routes : route.path => route
    if length(split("/", route.path)) == 1
  }

  child_routes = {
    for route in var.routes : route.path => route
    if length(split("/", route.path)) > 1
  }
}

# Create parent resources (e.g., /users, /contents)
resource "aws_api_gateway_resource" "parent_routes" {
  for_each = toset(local.parent_paths)

  rest_api_id = var.api_gateway_id
  parent_id   = var.api_gateway_root_id
  path_part   = each.value
}

# Create child resources (e.g., /users/health)
resource "aws_api_gateway_resource" "child_routes" {
  for_each = local.child_routes

  rest_api_id = var.api_gateway_id
  parent_id   = aws_api_gateway_resource.parent_routes[split("/", each.key)[0]].id
  path_part   = split("/", each.key)[1]
}

resource "aws_api_gateway_method" "routes" {
  for_each = { for idx, route in var.routes : route.path => route }

  rest_api_id   = var.api_gateway_id
  resource_id   = length(split("/", each.key)) == 1 ? aws_api_gateway_resource.parent_routes[each.key].id : aws_api_gateway_resource.child_routes[each.key].id
  http_method   = each.value.http_method
  authorization = each.value.auth_required ? "COGNITO_USER_POOLS" : "NONE"
  authorizer_id = each.value.auth_required ? var.cognito_authorizer_id : null

  # Require X-Origin-Verify header when validation is enabled
  request_validator_id = var.request_validator_id != "" ? var.request_validator_id : null
  request_parameters = var.origin_verify_secret != "" ? {
    "method.request.header.X-Origin-Verify" = true
  } : null
}

resource "aws_api_gateway_integration" "routes" {
  for_each = { for idx, route in var.routes : route.path => route }

  rest_api_id             = var.api_gateway_id
  resource_id             = length(split("/", each.key)) == 1 ? aws_api_gateway_resource.parent_routes[each.key].id : aws_api_gateway_resource.child_routes[each.key].id
  http_method             = aws_api_gateway_method.routes[each.key].http_method
  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.this.invoke_arn

  # Pass header validation in request parameters
  request_parameters = var.origin_verify_secret != "" ? {
    "integration.request.header.X-Origin-Verify" = "method.request.header.X-Origin-Verify"
  } : null
}

# Lambda Permissions
resource "aws_lambda_permission" "api_gateway" {
  for_each = { for idx, route in var.routes : route.path => route }

  statement_id  = "AllowAPIGatewayInvoke-${replace(each.value.path, "/", "-")}"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.this.function_name
  principal     = "apigateway.amazonaws.com"
  source_arn    = "${var.api_gateway_execution_arn}/*/*"
}
