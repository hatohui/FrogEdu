# =============================================================================
# ECR Module - Elastic Container Registry
# =============================================================================

resource "aws_ecr_repository" "this" {
  for_each = var.repositories

  name                 = "${var.project_name}-${var.environment}-${each.key}"
  image_tag_mutability = each.value.image_tag_mutability
  force_delete         = true

  image_scanning_configuration {
    scan_on_push = each.value.scan_on_push
  }

  encryption_configuration {
    encryption_type = each.value.encryption_type
  }

  tags = merge(
    var.tags,
    {
      Name        = "${var.project_name}-${var.environment}-${each.key}"
      Environment = var.environment
      Project     = var.project_name
      Service     = each.key
    }
  )
}

# Lifecycle policy for each repository
resource "aws_ecr_lifecycle_policy" "this" {
  for_each   = var.repositories
  repository = aws_ecr_repository.this[each.key].name

  policy = jsonencode({
    rules = [
      {
        rulePriority = 1
        description  = "Expire untagged images after ${each.value.lifecycle_expire_days} days"
        selection = {
          tagStatus   = "untagged"
          countType   = "sinceImagePushed"
          countUnit   = "days"
          countNumber = each.value.lifecycle_expire_days
        }
        action = {
          type = "expire"
        }
      },
      {
        rulePriority = 2
        description  = "Keep last ${each.value.lifecycle_policy_keep} images"
        selection = {
          tagStatus   = "any"
          countType   = "imageCountMoreThan"
          countNumber = each.value.lifecycle_policy_keep
        }
        action = {
          type = "expire"
        }
      }
    ]
  })
}

# Repository policy to allow Lambda to pull images
resource "aws_ecr_repository_policy" "lambda_pull" {
  for_each   = var.repositories
  repository = aws_ecr_repository.this[each.key].name

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Sid    = "LambdaECRImagePull"
        Effect = "Allow"
        Principal = {
          Service = "lambda.amazonaws.com"
        }
        Action = [
          "ecr:BatchGetImage",
          "ecr:GetDownloadUrlForLayer"
        ]
      }
    ]
  })
}
