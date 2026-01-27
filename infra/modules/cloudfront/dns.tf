
resource "aws_acm_certificate" "api" {
  count = var.custom_domain != "" ? 1 : 0

  provider          = aws.us_east_1
  domain_name       = var.custom_domain
  validation_method = "DNS"

  lifecycle {
    create_before_destroy = true
  }

  tags = {
    Name        = "${var.project_name}--api-cert"
    Environment = var.project_name
    Project     = var.project_name
  }
}

resource "aws_acm_certificate_validation" "api" {
  count = var.custom_domain != "" ? 1 : 0

  provider                = aws.us_east_1
  certificate_arn         = aws_acm_certificate.api[0].arn
  validation_record_fqdns = [for dvo in aws_acm_certificate.api[0].domain_validation_options : dvo.resource_record_name]

  timeouts {
    create = "30m"
  }
}
