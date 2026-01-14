# =============================================================================
# Cognito Module - User Authentication
# =============================================================================

variable "project_name" {
  type = string
}

variable "environment" {
  type = string
}

variable "aws_region" {
  type = string
}

# User Pool
resource "aws_cognito_user_pool" "main" {
  name = "${var.project_name}-${var.environment}-users"

  username_attributes      = ["email"]
  auto_verified_attributes = ["email"]

  schema {
    name                = "email"
    attribute_data_type = "String"
    required            = true
    mutable             = true
  }

  schema {
    name                = "role"
    attribute_data_type = "String"
    required            = false
    mutable             = true
  }

  password_policy {
    minimum_length                   = 8
    require_uppercase                = true
    require_lowercase                = true
    require_numbers                  = true
    require_symbols                  = false
    temporary_password_validity_days = 7
  }

  mfa_configuration = "OPTIONAL"

  software_token_mfa_configuration {
    enabled = true
  }

  account_recovery_setting {
    recovery_mechanism {
      name     = "verified_email"
      priority = 1
    }
  }

  deletion_protection = var.environment == "production" ? "ACTIVE" : "INACTIVE"
}

# User Pool Client
resource "aws_cognito_user_pool_client" "web_client" {
  name         = "${var.project_name}-${var.environment}-web-client"
  user_pool_id = aws_cognito_user_pool.main.id

  allowed_oauth_flows_user_pool_client = true
  allowed_oauth_flows                  = ["code", "implicit"]
  allowed_oauth_scopes                 = ["email", "openid", "profile"]

  callback_urls = [
    "http://localhost:5173/auth/callback",
    var.environment == "production" ? "https://frogedu.com/auth/callback" : "https://${var.environment}.frogedu.com/auth/callback"
  ]

  logout_urls = [
    "http://localhost:5173",
    var.environment == "production" ? "https://frogedu.com" : "https://${var.environment}.frogedu.com"
  ]

  supported_identity_providers = ["COGNITO"]

  access_token_validity  = 15
  id_token_validity      = 15
  refresh_token_validity = 30

  token_validity_units {
    access_token  = "minutes"
    id_token      = "minutes"
    refresh_token = "days"
  }

  prevent_user_existence_errors = "ENABLED"
  enable_token_revocation       = true
}

# User Pool Domain
resource "aws_cognito_user_pool_domain" "main" {
  domain       = "${var.project_name}-${var.environment}"
  user_pool_id = aws_cognito_user_pool.main.id
}

# User Groups
resource "aws_cognito_user_group" "teachers" {
  name         = "Teachers"
  user_pool_id = aws_cognito_user_pool.main.id
  description  = "Teachers group"
  precedence   = 1
}

resource "aws_cognito_user_group" "students" {
  name         = "Students"
  user_pool_id = aws_cognito_user_pool.main.id
  description  = "Students group"
  precedence   = 2
}

resource "aws_cognito_user_group" "admins" {
  name         = "Admins"
  user_pool_id = aws_cognito_user_pool.main.id
  description  = "Administrators"
  precedence   = 0
}

# Outputs
output "user_pool_id" {
  value = aws_cognito_user_pool.main.id
}

output "user_pool_arn" {
  value = aws_cognito_user_pool.main.arn
}

output "user_pool_endpoint" {
  value = aws_cognito_user_pool.main.endpoint
}

output "web_client_id" {
  value = aws_cognito_user_pool_client.web_client.id
}

output "domain" {
  value = aws_cognito_user_pool_domain.main.domain
}

output "issuer_url" {
  value = "https://cognito-idp.${var.aws_region}.amazonaws.com/${aws_cognito_user_pool.main.id}"
}

output "user_pool" {
  value = {
    id       = aws_cognito_user_pool.main.id
    arn      = aws_cognito_user_pool.main.arn
    endpoint = aws_cognito_user_pool.main.endpoint
  }
}
