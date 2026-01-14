# =============================================================================
# CloudFront Module - CDN for API Gateway
# =============================================================================

variable "project_name" {
  type = string
}

variable "environment" {
  type = string
}

variable "api_gateway_domain" {
  type = string
}

variable "api_gateway_stage" {
  type = string
}

variable "origin_verify_secret" {
  type      = string
  sensitive = true
}

resource "aws_cloudfront_distribution" "api" {
  enabled             = true
  is_ipv6_enabled     = true
  comment             = "${var.project_name}-${var.environment} API CDN"
  price_class         = "PriceClass_200"
  wait_for_deployment = false

  origin {
    domain_name = var.api_gateway_domain
    origin_id   = "apigateway"
    origin_path = "/${var.api_gateway_stage}"

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

    forwarded_values {
      query_string = true
      headers      = ["Authorization", "Accept", "Content-Type"]

      cookies {
        forward = "none"
      }
    }

    min_ttl     = 0
    default_ttl = 0
    max_ttl     = 0
  }

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  viewer_certificate {
    cloudfront_default_certificate = true
    minimum_protocol_version       = "TLSv1.2_2021"
  }
}

output "distribution_id" {
  value = aws_cloudfront_distribution.api.id
}

output "distribution_domain" {
  value = aws_cloudfront_distribution.api.domain_name
}

output "distribution_arn" {
  value = aws_cloudfront_distribution.api.arn
}
