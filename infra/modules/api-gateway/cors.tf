# =============================================================================
# API Gateway CORS Configuration Helper
# =============================================================================
# CORS configuration has been moved to main.tf to ensure consistent headers
# across all resources (root, /api, and service routes).
#
# The Lambda functions also handle CORS at the application level for
# dynamic origin validation based on the incoming request.
#
# CORS headers are configured in the following locations:
# - main.tf: API Gateway MOCK integration for OPTIONS methods
# - lambda-container/main.tf: OPTIONS methods for service proxy routes
# - Backend Program.cs: Application-level CORS for actual responses
#
# Allowed origins:
# - https://frogedu.org (production)
# - https://www.frogedu.org (production with www)
# - http://localhost:5173 (local development)
# - http://localhost:5174 (local development alternate port)
