# =============================================================================
# ECR Module - Outputs
# =============================================================================

output "repository_urls" {
  description = "Map of repository names to their URLs"
  value = {
    for k, v in aws_ecr_repository.this : k => v.repository_url
  }
}

output "repository_arns" {
  description = "Map of repository names to their ARNs"
  value = {
    for k, v in aws_ecr_repository.this : k => v.arn
  }
}

output "registry_id" {
  description = "Registry ID (AWS account ID)"
  value       = try(values(aws_ecr_repository.this)[0].registry_id, null)
}

output "repositories" {
  description = "Complete repository information"
  value = {
    for k, v in aws_ecr_repository.this : k => {
      name        = v.name
      url         = v.repository_url
      arn         = v.arn
      registry_id = v.registry_id
    }
  }
}
