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
