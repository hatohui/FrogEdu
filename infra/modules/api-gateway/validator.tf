resource "aws_api_gateway_request_validator" "request_validator" {
  rest_api_id                 = aws_api_gateway_rest_api.api_gateway.id
  name                        = "${var.project_name}-request-validator"
  validate_request_body       = true
  validate_request_parameters = true
}

# =============================================================================
# Gateway Responses - CloudFront Origin Verification
# =============================================================================


resource "aws_api_gateway_gateway_response" "missing_authentication_token" {
  rest_api_id   = aws_api_gateway_rest_api.api_gateway.id
  response_type = "MISSING_AUTHENTICATION_TOKEN"
  status_code   = "403"

  response_templates = {
    "application/json" = jsonencode({
      message = "Access Denied: Invalid request origin"
      error   = "This API can only be accessed through api.frogedu.org"
    })
  }
}

resource "aws_api_gateway_gateway_response" "unauthorized" {
  rest_api_id   = aws_api_gateway_rest_api.api_gateway.id
  response_type = "UNAUTHORIZED"
  status_code   = "401"

  response_templates = {
    "application/json" = jsonencode({
      message = "Unauthorized: Authentication required"
      error   = "Please provide a valid Authorization token"
    })
  }
}

resource "aws_api_gateway_gateway_response" "access_denied" {
  rest_api_id   = aws_api_gateway_rest_api.api_gateway.id
  response_type = "ACCESS_DENIED"
  status_code   = "403"

  response_templates = {
    "application/json" = jsonencode({
      message = "Access Denied: Insufficient permissions"
      error   = "Your token does not have permission to access this resource"
    })
  }
}

