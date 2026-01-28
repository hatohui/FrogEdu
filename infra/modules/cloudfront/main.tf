resource "aws_cloudfront_distribution" "api" {
  enabled         = true
  is_ipv6_enabled = true

  retain_on_delete    = true
  wait_for_deployment = false

  comment     = "${var.project_name} API CDN"
  price_class = "PriceClass_All"
  aliases     = var.custom_domain != "" ? [var.custom_domain] : []
  web_acl_id  = var.web_acl_id

  origin {
    domain_name = replace(var.api_gateway_domain, "https://", "")
    origin_id   = "apigateway"
    origin_path = ""

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

    cache_policy_id          = "4135ea2d-6df8-44a3-9df3-4b5a84be39ad"
    origin_request_policy_id = "59781a5b-3903-41f3-afcb-af62929ccde1"
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
    Name        = "${var.project_name}-api-cdn"
    Environment = var.project_name
    Project     = var.project_name
  }

  depends_on = [aws_acm_certificate_validation.api]
}
