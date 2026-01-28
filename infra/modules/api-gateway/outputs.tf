output "api_gateway_domain" {
  description = "Domain name of the API Gateway"
  value       = aws_apigatewayv2_api.api_gateway.api_endpoint
}
