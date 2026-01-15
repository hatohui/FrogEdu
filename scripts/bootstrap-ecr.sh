#!/bin/bash
# Bootstrap ECR Images for Initial Deployment
# Run this BEFORE terraform apply on first deployment
# Usage: doppler run -- ./scripts/bootstrap-ecr.sh

set -e

REGION="${AWS_REGION:-ap-southeast-1}"
ACCOUNT_ID="${AWS_ACCOUNT_ID:-630633962130}"

echo "[*] Bootstrapping ECR Images..."

# Service mapping
declare -A services
services["content"]="Content"
services["user"]="User"
services["assessment"]="Assessment"
services["ai"]="AI"

# AWS ECR Login
echo -e "\n[*] Logging into Amazon ECR..."

# Test AWS credentials first
echo "   Checking AWS credentials..."
if ! aws sts get-caller-identity > /dev/null 2>&1; then
    echo "[ERROR] AWS credentials not configured properly"
    exit 1
fi
echo "   AWS Identity verified"

# Check if ECR repositories exist
echo "   Checking ECR repositories..."
for service in "${!services[@]}"; do
    service_name="${service}-api"
    repo_name="frogedu-dev-${service_name}"
    
    if ! aws ecr describe-repositories --repository-names "$repo_name" --region "$REGION" > /dev/null 2>&1; then
        echo "[WARNING] ECR repository $repo_name does not exist"
        echo "   Creating repository..."
        aws ecr create-repository --repository-name "$repo_name" --region "$REGION" --image-scanning-configuration scanOnPush=true > /dev/null
        echo "   Created $repo_name"
    fi
done

# Login to ECR
echo "   Logging into ECR..."
aws ecr get-login-password --region "$REGION" | docker login --username AWS --password-stdin "$ACCOUNT_ID.dkr.ecr.$REGION.amazonaws.com"

if [ $? -ne 0 ]; then
    echo "[ERROR] Failed to login to ECR"
    exit 1
fi
echo "   ECR login successful"

for service in "${!services[@]}"; do
    folder_name="${services[$service]}"
    service_name="${service}-api"
    repo_name="frogedu-dev-${service_name}"
    image_uri="$ACCOUNT_ID.dkr.ecr.$REGION.amazonaws.com/$repo_name"
    
    echo -e "\n[*] Building $service_name..."
    echo "   Dockerfile: backend/Services/$folder_name/Dockerfile"
    
    # Build the image
    docker build \
        --provenance=false \
        -f "backend/Services/$folder_name/Dockerfile" \
        -t "$image_uri:latest" \
        backend/
    
    if [ $? -ne 0 ]; then
        echo "[ERROR] Failed to build $service_name"
        exit 1
    fi
    
    echo "[*] Pushing $service_name to ECR..."
    docker push "$image_uri:latest"
    
    if [ $? -ne 0 ]; then
        echo "[ERROR] Failed to push $service_name"
        exit 1
    fi
    
    echo "[SUCCESS] $service_name deployed to ECR"
done

echo -e "\n[SUCCESS] All images bootstrapped successfully!"
echo "Now you can run: doppler run -- terraform apply"
