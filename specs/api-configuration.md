# API Config

## Overall

We're using Cloudflare frontend and AWS backend. The full architecture can be found in #architecture.

Our frontend is `www.frogedu.org`, `frogedu.org`, and `localhost:5173` (Vite default). CORS must allow all of these origins.

## API URL Configuration

| Environment | VITE_API_URL              | Description                         |
| ----------- | ------------------------- | ----------------------------------- |
| Development | `http://localhost:3000`   | Local docker-compose services       |
| Production  | `https://api.frogedu.org` | CloudFront -> API Gateway -> Lambda |

## Restriction

The flow for our API domain (configured in Cloudfront) is like this:

Domain: `api.frogedu.org`
Cloudfront -> API Gateway -> Lambda Services
Requires: Authorization from Cognito for protected routes
Exceptions (no auth required): `/api/{service}`, `/api/{service}/health`, `/api/{service}/health/db`

Those APIs should return without the need for authorization.

## CORS

Allowed origins:

- `https://frogedu.org`
- `https://www.frogedu.org`
- `http://localhost:5173`
- `http://localhost:5174`

CloudFront should not inject additional CORS headers for API responses. API Gateway handles OPTIONS responses and Lambdas emit `Access-Control-Allow-Origin` based on the request origin.

## The rules

Frontend should call: api.frogedu.org/api/{service}/{...query}
The API Gateway validates JWTs on protected routes and proxies the request to the service Lambda.

Example:

For users to check who they are: they call users service in /me

The frontends calls api to `api.frogedu.org/api/users/me`

The API Gateway directs to userService because of `/users` and (for proxy routes) strips `/api/users`, so the Lambda receives `/me`.

## Path Mapping (IMPORTANT)

### Protected Proxy Routes (JWT required)

For all protected endpoints, API Gateway uses `{proxy+}` and strips `/api/{service}` so the Lambda receives the inner path.

| Frontend Calls       | API Gateway Routes To | Lambda Receives | Backend Must Define |
| -------------------- | --------------------- | --------------- | ------------------- |
| `/api/users/me`      | user-api Lambda       | `/me`           | `/me`               |
| `/api/contents/list` | content-api Lambda    | `/list`         | `/list`             |
| `/api/ai/chat`       | ai-api Lambda         | `/chat`         | `/chat`             |

### Public Health Routes (no auth)

Health endpoints are exposed via explicit public routes. These do **not** strip the prefix, so Lambdas receive the full path and must also handle `/api/{service}/health` and `/api/{service}/health/db`.

| Frontend Calls               | API Gateway Routes To | Lambda Receives              | Backend Must Define          |
| ---------------------------- | --------------------- | ---------------------------- | ---------------------------- |
| `/api/users/health`          | user-api Lambda       | `/api/users/health`          | `/api/users/health`          |
| `/api/users/health/db`       | user-api Lambda       | `/api/users/health/db`       | `/api/users/health/db`       |
| `/api/contents/health`       | content-api Lambda    | `/api/contents/health`       | `/api/contents/health`       |
| `/api/assessments/health/db` | assessment-api Lambda | `/api/assessments/health/db` | `/api/assessments/health/db` |
| `/api/ai/health`             | ai-api Lambda         | `/api/ai/health`             | `/api/ai/health`             |

### Health Endpoints Summary

Each service exposes:

- `/health` - Basic service health check (no auth required, proxy routes)
- `/health/db` - Database connectivity check (no auth required, proxy routes)
- `/api/{service}/health` - Public health check (no auth required)
- `/api/{service}/health/db` - Public DB health check (no auth required)

**Frontend Usage:**
The axios instance is configured with `baseURL: {VITE_API_URL}/api`, so all API calls automatically get the `/api` prefix:

```typescript
// In code you call:
apiClient.get("/users/health");

// Axios makes request to:
// https://api.frogedu.org/api/users/health

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
