# =============================================================================
# CloudFront Module - CDN for API Gateway
# =============================================================================

terraform {
  required_providers {
    aws = {
      source                = "hashicorp/aws"
      version               = ">= 5.0"
      configuration_aliases = [aws.us_east_1]
    }
  }
}

# ACM Certificate for custom domain (must be in us-east-1 for CloudFront)
resource "aws_acm_certificate" "api" {
  count = var.custom_domain != "" ? 1 : 0

  provider          = aws.us_east_1
  domain_name       = var.custom_domain
  validation_method = "DNS"

  lifecycle {
    create_before_destroy = true
  }

  tags = {
    Name        = "${var.project_name}-${var.environment}-api-cert"
    Environment = var.environment
    Project     = var.project_name
  }
}

# ACM Certificate Validation (waits for DNS validation to complete)
resource "aws_acm_certificate_validation" "api" {
  count = var.custom_domain != "" ? 1 : 0

  provider                = aws.us_east_1
  certificate_arn         = aws_acm_certificate.api[0].arn
  validation_record_fqdns = [for dvo in aws_acm_certificate.api[0].domain_validation_options : dvo.resource_record_name]

  timeouts {
    create = "30m"
  }
}

resource "aws_cloudfront_distribution" "api" {
  enabled             = true
  is_ipv6_enabled     = true
  comment             = "${var.project_name}-${var.environment} API CDN"
  price_class         = "PriceClass_All"
  wait_for_deployment = false
  aliases             = var.custom_domain != "" ? [var.custom_domain] : []
  web_acl_id          = var.web_acl_id

  origin {
    domain_name = var.api_gateway_domain # execute-api domain
    origin_id   = "apigateway"
    origin_path = "/${var.api_gateway_stage}" # Prepends /dev to all requests

    custom_origin_config {
      http_port              = 80
      https_port             = 443
      origin_protocol_policy = "https-only"
      origin_ssl_protocols   = ["TLSv1.2"]
    }

    custom_header {
      name  = "X-Origin-Verify"
      value = var.origin_verify_secret
    }
  }

  default_cache_behavior {
    target_origin_id       = "apigateway"
    viewer_protocol_policy = "redirect-to-https"
    allowed_methods        = ["DELETE", "GET", "HEAD", "OPTIONS", "PATCH", "POST", "PUT"]
    cached_methods         = ["GET", "HEAD", "OPTIONS"]
    compress               = true

    # Use managed cache policy (CachingDisabled for API)
    cache_policy_id = "4135ea2d-6df8-44a3-9df3-4b5a84be39ad"

    # Use managed origin request policy (AllViewer)
    origin_request_policy_id = "216adef6-5c7f-47e4-b989-5492eafa07d3"

    # Use managed CORS response headers policy (CORS-With-Preflight)
    # This allows the Lambda CORS headers to pass through to the browser
    response_headers_policy_id = "5cc3b908-e619-4b99-88e5-2cf7f45965bd"
  }

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  viewer_certificate {
    cloudfront_default_certificate = var.custom_domain == ""
    acm_certificate_arn            = var.custom_domain != "" ? aws_acm_certificate.api[0].arn : null
    ssl_support_method             = var.custom_domain != "" ? "sni-only" : null
    minimum_protocol_version       = "TLSv1.2_2021"
  }

  tags = {
    Name        = "${var.project_name}-${var.environment}-api-cdn"
    Environment = var.environment
    Project     = var.project_name
  }

  depends_on = [aws_acm_certificate_validation.api]
}
