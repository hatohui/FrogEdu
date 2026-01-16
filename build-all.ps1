Write-Host 'Pushing Content API...' -ForegroundColor Cyan
doppler run -- docker push 630633962130.dkr.ecr.ap-southeast-1.amazonaws.com/frogedu-dev-content-api:latest

Write-Host 'Building and pushing User API...' -ForegroundColor Cyan
cd backend
docker build --provenance=false -f Services/User/Dockerfile -t 630633962130.dkr.ecr.ap-southeast-1.amazonaws.com/frogedu-dev-user-api:latest .
doppler run -- docker push 630633962130.dkr.ecr.ap-southeast-1.amazonaws.com/frogedu-dev-user-api:latest

Write-Host 'Building and pushing Assessment API...' -ForegroundColor Cyan
docker build --provenance=false -f Services/Assessment/Dockerfile -t 630633962130.dkr.ecr.ap-southeast-1.amazonaws.com/frogedu-dev-assessment-api:latest .
doppler run -- docker push 630633962130.dkr.ecr.ap-southeast-1.amazonaws.com/frogedu-dev-assessment-api:latest

Write-Host 'Building and pushing AI API...' -ForegroundColor Cyan
docker build --provenance=false -f Services/AI/Dockerfile -t 630633962130.dkr.ecr.ap-southeast-1.amazonaws.com/frogedu-dev-ai-api:latest .
doppler run -- docker push 630633962130.dkr.ecr.ap-southeast-1.amazonaws.com/frogedu-dev-ai-api:latest

Write-Host 'Done!' -ForegroundColor Green
