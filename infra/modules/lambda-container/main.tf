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
# Support both proxy routes and explicit public routes (e.g., /health)
locals {
  routes_by_path = { for route in var.routes : route.path => route }

  route_parts = {
    for path, route in local.routes_by_path : path => {
      parts    = split("/", path)
      is_proxy = contains(split("/", path), "{proxy+}")
      route    = route
    }
  }

  proxy_routes = {
    for path, data in local.route_parts : path => data
    if data.is_proxy
  }

  non_proxy_routes = {
    for path, data in local.route_parts : path => data
    if !data.is_proxy
  }

  services = distinct([
    for path, data in local.route_parts : data.parts[1]
    if length(data.parts) >= 2 && data.parts[0] == "api"
  ])

  health_services = distinct([
    for path, data in local.non_proxy_routes : data.parts[1]
    if length(data.parts) >= 3 && data.parts[0] == "api" && data.parts[2] == "health"
  ])

  health_db_services = distinct([
    for path, data in local.non_proxy_routes : data.parts[1]
    if length(data.parts) >= 4 && data.parts[0] == "api" && data.parts[2] == "health" && data.parts[3] == "db"
  ])

  proxy_route_map = {
    for path, data in local.proxy_routes : path => {
      route      = data.route
      service    = data.parts[1]
      proxy_part = data.parts[length(data.parts) - 1]
    }
  }

  non_proxy_route_map = {
    for path, data in local.non_proxy_routes : path => data.route
  }
}

# Create /api/{service} resources
resource "aws_api_gateway_resource" "service" {
  for_each = toset(local.services)

  rest_api_id = var.api_gateway_id
  parent_id   = var.shared_parent_resources["api"]
  path_part   = each.value
}

# Preserve existing API Gateway resources from previous module layout
moved {
  from = aws_api_gateway_resource.resources["api/users"]
  to   = aws_api_gateway_resource.service["users"]
}

moved {
  from = aws_api_gateway_resource.resources["api/users/health"]
  to   = aws_api_gateway_resource.health["users"]
}

moved {
  from = aws_api_gateway_resource.resources["api/users/health/db"]
  to   = aws_api_gateway_resource.health_db["users"]
}

moved {
  from = aws_api_gateway_resource.resources["api/contents"]
  to   = aws_api_gateway_resource.service["contents"]
}

moved {
  from = aws_api_gateway_resource.resources["api/contents/health"]
  to   = aws_api_gateway_resource.health["contents"]
}

moved {
  from = aws_api_gateway_resource.resources["api/contents/health/db"]
  to   = aws_api_gateway_resource.health_db["contents"]
}

moved {
  from = aws_api_gateway_resource.resources["api/assessments"]
  to   = aws_api_gateway_resource.service["assessments"]
}

moved {
  from = aws_api_gateway_resource.resources["api/assessments/health"]
  to   = aws_api_gateway_resource.health["assessments"]
}

moved {
  from = aws_api_gateway_resource.resources["api/assessments/health/db"]
  to   = aws_api_gateway_resource.health_db["assessments"]
}

moved {
  from = aws_api_gateway_resource.resources["api/ai"]
  to   = aws_api_gateway_resource.service["ai"]
}

moved {
  from = aws_api_gateway_resource.resources["api/ai/health"]
  to   = aws_api_gateway_resource.health["ai"]
}

moved {
  from = aws_api_gateway_resource.resources["api/ai/health/db"]
  to   = aws_api_gateway_resource.health_db["ai"]
}

# Create /api/{service}/health resources
resource "aws_api_gateway_resource" "health" {
  for_each = toset(local.health_services)

  rest_api_id = var.api_gateway_id
  parent_id   = aws_api_gateway_resource.service[each.value].id
  path_part   = "health"
}

# Create /api/{service}/health/db resources
resource "aws_api_gateway_resource" "health_db" {
  for_each = toset(local.health_db_services)

  rest_api_id = var.api_gateway_id
  parent_id   = aws_api_gateway_resource.health[each.value].id
  path_part   = "db"
}

# Create proxy resources (e.g., /api/users/{proxy+})
resource "aws_api_gateway_resource" "proxies" {
  for_each = local.proxy_route_map

  rest_api_id = var.api_gateway_id
  parent_id   = aws_api_gateway_resource.service[each.value.service].id
  path_part   = each.value.proxy_part
}

# Methods for proxy routes (protected endpoints)
resource "aws_api_gateway_method" "proxy_routes" {
  for_each = local.proxy_route_map

  rest_api_id   = var.api_gateway_id
  resource_id   = aws_api_gateway_resource.proxies[each.key].id
  http_method   = each.value.route.http_method
  authorization = each.value.route.auth_required ? "COGNITO_USER_POOLS" : "NONE"
  authorizer_id = each.value.route.auth_required ? var.cognito_authorizer_id : null

  # Require X-Origin-Verify header only for auth-required routes when validation is enabled
  # This allows health checks and public endpoints to be accessed directly
  request_validator_id = var.request_validator_id != "" && each.value.route.auth_required ? var.request_validator_id : null
  request_parameters = var.origin_verify_secret != "" && each.value.route.auth_required ? {
    "method.request.header.X-Origin-Verify" = true
  } : {}
}

resource "aws_api_gateway_integration" "proxy_routes" {
  for_each = local.proxy_route_map

  rest_api_id             = var.api_gateway_id
  resource_id             = aws_api_gateway_resource.proxies[each.key].id
  http_method             = aws_api_gateway_method.proxy_routes[each.key].http_method
  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.this.invoke_arn

  # Pass header validation only for auth-required routes
  request_parameters = var.origin_verify_secret != "" && each.value.route.auth_required ? {
    "integration.request.header.X-Origin-Verify" = "method.request.header.X-Origin-Verify"
  } : {}
}

# Methods for non-proxy routes (public endpoints like /health)
resource "aws_api_gateway_method" "non_proxy_routes" {
  for_each = local.non_proxy_route_map

  rest_api_id = var.api_gateway_id
  resource_id = (
    length(local.route_parts[each.key].parts) == 2
    ? aws_api_gateway_resource.service[local.route_parts[each.key].parts[1]].id
    : length(local.route_parts[each.key].parts) == 3
    ? aws_api_gateway_resource.health[local.route_parts[each.key].parts[1]].id
    : aws_api_gateway_resource.health_db[local.route_parts[each.key].parts[1]].id
  )
  http_method   = each.value.http_method
  authorization = each.value.auth_required ? "COGNITO_USER_POOLS" : "NONE"
  authorizer_id = each.value.auth_required ? var.cognito_authorizer_id : null

  request_validator_id = var.request_validator_id != "" && each.value.auth_required ? var.request_validator_id : null
  request_parameters = var.origin_verify_secret != "" && each.value.auth_required ? {
    "method.request.header.X-Origin-Verify" = true
  } : {}
}

resource "aws_api_gateway_integration" "non_proxy_routes" {
  for_each = local.non_proxy_route_map

  rest_api_id = var.api_gateway_id
  resource_id = (
    length(local.route_parts[each.key].parts) == 2
    ? aws_api_gateway_resource.service[local.route_parts[each.key].parts[1]].id
    : length(local.route_parts[each.key].parts) == 3
    ? aws_api_gateway_resource.health[local.route_parts[each.key].parts[1]].id
    : aws_api_gateway_resource.health_db[local.route_parts[each.key].parts[1]].id
  )
  http_method             = aws_api_gateway_method.non_proxy_routes[each.key].http_method
  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.this.invoke_arn

  request_parameters = var.origin_verify_secret != "" && each.value.auth_required ? {
    "integration.request.header.X-Origin-Verify" = "method.request.header.X-Origin-Verify"
  } : {}
}

# =============================================================================
# CORS OPTIONS Support - Mock Integration (recommended)
# =============================================================================
# OPTIONS method for non-proxy resources (e.g., /api/users, /api/users/health)
resource "aws_api_gateway_method" "options_resources" {
  for_each = toset(keys(local.non_proxy_route_map))

  rest_api_id = var.api_gateway_id
  resource_id = (
    length(local.route_parts[each.value].parts) == 2
    ? aws_api_gateway_resource.service[local.route_parts[each.value].parts[1]].id
    : length(local.route_parts[each.value].parts) == 3
    ? aws_api_gateway_resource.health[local.route_parts[each.value].parts[1]].id
    : aws_api_gateway_resource.health_db[local.route_parts[each.value].parts[1]].id
  )
  http_method   = "OPTIONS"
  authorization = "NONE"
}

# Mock integration for resource OPTIONS - returns 200 immediately
resource "aws_api_gateway_integration" "options_resources" {
  for_each = toset(keys(local.non_proxy_route_map))

  rest_api_id = var.api_gateway_id
  resource_id = (
    length(local.route_parts[each.value].parts) == 2
    ? aws_api_gateway_resource.service[local.route_parts[each.value].parts[1]].id
    : length(local.route_parts[each.value].parts) == 3
    ? aws_api_gateway_resource.health[local.route_parts[each.value].parts[1]].id
    : aws_api_gateway_resource.health_db[local.route_parts[each.value].parts[1]].id
  )
  http_method = aws_api_gateway_method.options_resources[each.value].http_method
  type        = "MOCK"

  request_templates = {
    "application/json" = "{\"statusCode\": 200}"
  }
}

# Method response for resource OPTIONS
resource "aws_api_gateway_method_response" "options_resources_200" {
  for_each = toset(keys(local.non_proxy_route_map))

  rest_api_id = var.api_gateway_id
  resource_id = (
    length(local.route_parts[each.value].parts) == 2
    ? aws_api_gateway_resource.service[local.route_parts[each.value].parts[1]].id
    : length(local.route_parts[each.value].parts) == 3
    ? aws_api_gateway_resource.health[local.route_parts[each.value].parts[1]].id
    : aws_api_gateway_resource.health_db[local.route_parts[each.value].parts[1]].id
  )
  http_method = aws_api_gateway_method.options_resources[each.value].http_method
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

# Integration response for resource OPTIONS - returns CORS headers
resource "aws_api_gateway_integration_response" "options_resources_200" {
  for_each = toset(keys(local.non_proxy_route_map))

  rest_api_id = var.api_gateway_id
  resource_id = (
    length(local.route_parts[each.value].parts) == 2
    ? aws_api_gateway_resource.service[local.route_parts[each.value].parts[1]].id
    : length(local.route_parts[each.value].parts) == 3
    ? aws_api_gateway_resource.health[local.route_parts[each.value].parts[1]].id
    : aws_api_gateway_resource.health_db[local.route_parts[each.value].parts[1]].id
  )
  http_method = aws_api_gateway_method.options_resources[each.value].http_method
  status_code = aws_api_gateway_method_response.options_resources_200[each.value].status_code

  response_parameters = {
    "method.response.header.Access-Control-Allow-Headers" = "'Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token'"
    "method.response.header.Access-Control-Allow-Methods" = "'DELETE,GET,HEAD,OPTIONS,PATCH,POST,PUT'"
    "method.response.header.Access-Control-Allow-Origin"  = "'*'"
  }

  depends_on = [aws_api_gateway_integration.options_resources]
}

# OPTIONS method for proxy resources (e.g., /api/users/{proxy+})
resource "aws_api_gateway_method" "options_proxy" {
  for_each = local.proxy_route_map

  rest_api_id   = var.api_gateway_id
  resource_id   = aws_api_gateway_resource.proxies[each.key].id
  http_method   = "OPTIONS"
  authorization = "NONE"
}

# Mock integration for proxy OPTIONS
resource "aws_api_gateway_integration" "options_proxy" {
  for_each = local.proxy_route_map

  rest_api_id = var.api_gateway_id
  resource_id = aws_api_gateway_resource.proxies[each.key].id
  http_method = aws_api_gateway_method.options_proxy[each.key].http_method
  type        = "MOCK"

  request_templates = {
    "application/json" = "{\"statusCode\": 200}"
  }
}

# Method response for proxy OPTIONS
resource "aws_api_gateway_method_response" "options_proxy_200" {
  for_each = local.proxy_route_map

  rest_api_id = var.api_gateway_id
  resource_id = aws_api_gateway_resource.proxies[each.key].id
  http_method = aws_api_gateway_method.options_proxy[each.key].http_method
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

# Integration response for proxy OPTIONS - returns CORS headers
resource "aws_api_gateway_integration_response" "options_proxy_200" {
  for_each = local.proxy_route_map

  rest_api_id = var.api_gateway_id
  resource_id = aws_api_gateway_resource.proxies[each.key].id
  http_method = aws_api_gateway_method.options_proxy[each.key].http_method
  status_code = aws_api_gateway_method_response.options_proxy_200[each.key].status_code

  response_parameters = {
    "method.response.header.Access-Control-Allow-Headers" = "'Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token'"
    "method.response.header.Access-Control-Allow-Methods" = "'DELETE,GET,HEAD,OPTIONS,PATCH,POST,PUT'"
    "method.response.header.Access-Control-Allow-Origin"  = "'*'"
  }

  depends_on = [aws_api_gateway_integration.options_proxy]
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
