# =============================================================================
# IAM Module - Output Declarations
# =============================================================================

output "lambda_execution_role" {
  description = "Lambda execution role with ARN and name"
  value = {
    arn  = aws_iam_role.lambda_execution.arn
    name = aws_iam_role.lambda_execution.name
  }
}

output "github_actions_ecr_role" {
  description = "GitHub Actions ECR role with ARN and name"
  value = {
    arn  = aws_iam_role.github_actions_ecr.arn
    name = aws_iam_role.github_actions_ecr.name
  }
}

output "github_actions_ecr_role_arn" {
  description = "ARN of the GitHub Actions ECR role (for use in GitHub Actions workflows)"
  value       = aws_iam_role.github_actions_ecr.arn
}

output "github_oidc_provider_arn" {
  description = "ARN of the GitHub Actions OIDC provider"
  value       = aws_iam_openid_connect_provider.github_actions.arn
}
