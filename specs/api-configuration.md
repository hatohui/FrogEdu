# API Config

## Overall

We're using Cloudflare frontend and AWS backend, the full Architecture can be found in #architecture

Our frontend is `www.frogedu.org`, `frogedu.org` and `localhost:5173` (vite default) (CORS should definitely supports all of these)

## API URL Configuration

| Environment | VITE_API_URL              | Description                         |
| ----------- | ------------------------- | ----------------------------------- |
| Development | `http://localhost:3000`   | Local docker-compose services       |
| Production  | `https://api.frogedu.org` | CloudFront -> API Gateway -> Lambda |

## Restriction

The flow for our API domain (configured in Cloudfront) is like this:

Domain: `api.frogedu.org`
Cloudfront -> API Gateway -> Lambda Services
Requires: Authorization from Cognito
Exceptions: `/api/{service}/health`, `/api/{service}/health/db`, `/api/{service}`

Those APIs should return without the need for authorization.

## The rules

Frontend should call: api.frogedu.org/api/{service}/{...query}
The API Gateway should validate and strips everything so that the lambdas only receives {...query}

Example:

For users to check who they are: they call users service in /me

The frontends calls api to `api.frogedu.org/api/users/me`

The API Gateways directs to userService because of `/users`, strips away `/api/users` and the Lambda receives `/me`

## Path Mapping (IMPORTANT)

Due to API Gateway's path stripping behavior, backend services must define endpoints **without** the `/api/{service}` prefix:

| Frontend Calls            | API Gateway Routes To | Lambda Receives | Backend Must Define |
| ------------------------- | --------------------- | --------------- | ------------------- |
| `/api/users/health`       | user-api Lambda       | `/health`       | `/health`           |
| `/api/users/me`           | user-api Lambda       | `/me`           | `/me`               |
| `/api/contents/health`    | content-api Lambda    | `/health`       | `/health`           |
| `/api/assessments/health` | assessment-api Lambda | `/health`       | `/health`           |
| `/api/ai/health`          | ai-api Lambda         | `/health`       | `/health`           |

### Health Endpoints Summary

Each service exposes:

- `/health` - Basic service health check (no auth required)
- `/health/db` - Database connectivity check (no auth required)

**Frontend Usage:**
The axios instance is configured with `baseURL: {VITE_API_URL}/api`, so all API calls automatically get the `/api` prefix:

```typescript
// In code you call:
apiClient.get("/users/health");

// Axios makes request to:
{
  VITE_API_URL;
}
/api/erssu / health;

// API Gateway receives and routes based on /users
// Lambda receives: /health (prefix stripped)
```

## Available Services

| Service    | API Gateway Path            | Lambda Function                |
| ---------- | --------------------------- | ------------------------------ |
| User       | `/api/users/{proxy+}`       | `frogedu-{env}-user-api`       |
| Content    | `/api/contents/{proxy+}`    | `frogedu-{env}-content-api`    |
| Assessment | `/api/assessments/{proxy+}` | `frogedu-{env}-assessment-api` |
| AI         | `/api/ai/{proxy+}`          | `frogedu-{env}-ai-api`         |
