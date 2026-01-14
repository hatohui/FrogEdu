terraform {
  required_version = ">= 1.5"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 6.28"
    }
    cloudflare = {
      source  = "cloudflare/cloudflare"
      version = "~> 5.15"
    }
    doppler = {
      source  = "dopplerhq/doppler"
      version = "~> 1.21"
    }
  }

  cloud { 
    organization = "soltunemontepre" 
    workspaces { 
      name = "dev" 
    } 
  } 
}