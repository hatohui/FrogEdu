provider "doppler" {
  doppler_token = var.doppler_token
}

provider "aws" {
  region     = var.aws_region
  access_key = var.AWS_ACCESS_KEY_ID
  secret_key = var.AWS_SECRET_ACCESS_KEY

  default_tags {
    tags = {
      Project     = var.project_name
      Environment = var.environment
      ManagedBy   = "Terraform"
      Repository  = "FrogEdu"
    }
  }
}

provider "aws" {
  alias      = "us_east_1"
  region     = "us-east-1"
  access_key = var.AWS_ACCESS_KEY_ID
  secret_key = var.AWS_SECRET_ACCESS_KEY

  default_tags {
    tags = {
      Project     = var.project_name
      Environment = var.environment
      ManagedBy   = "Terraform"
      Repository  = "FrogEdu"
    }
  }
}
