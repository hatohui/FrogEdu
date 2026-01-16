# =============================================================================
# API Gateway CORS Configuration Helper
# =============================================================================
# Note: The Lambda functions handle CORS at the application level
locals {
  cors_headers = {
    "method.response.header.Access-Control-Allow-Origin"      = true
    "method.response.header.Access-Control-Allow-Headers"     = true
    "method.response.header.Access-Control-Allow-Methods"     = true
    "method.response.header.Access-Control-Allow-Credentials" = true
  }

  # Note: Origin will be set by the Lambda function based on the request
  # These are defaults if needed at API Gateway level
  cors_header_values = {
    "method.response.header.Access-Control-Allow-Origin"      = "'https://frogedu.org'"
    "method.response.header.Access-Control-Allow-Headers"     = "'Content-Type,Authorization,X-Amz-Date,X-Api-Key,X-Amz-Security-Token'"
    "method.response.header.Access-Control-Allow-Methods"     = "'GET,POST,PUT,DELETE,OPTIONS'"
    "method.response.header.Access-Control-Allow-Credentials" = "'true'"
  }
}
