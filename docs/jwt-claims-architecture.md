# JWT Claims Architecture

## Overview

This document describes how FrogEdu microservices use JWT claims for authentication and authorization, minimizing cross-service HTTP calls.

## Architecture Decision

**Principle**: Authorization data (role, permissions) belongs in JWT. Business data (subscriptions, profiles) uses service-to-service calls.

### What Uses JWT Claims

All services use JWT claims for:

- **User Identity**: `sub` (Cognito user ID)
- **Role/Permissions**: `custom:role` → `ClaimTypes.Role`
- **Email**: `email`

### What Uses HTTP Calls

Services make HTTP calls for:

- **Subscription Data**: Plan type, expiration dates (frequently changing business data)
- **User Profiles**: Full user details beyond auth

## Implementation by Service

### ✅ User Service

**JWT Claims Used:**

- `sub` → User identity
- `custom:role` → Set by `CognitoAttributeService.SyncRoleAttributeAsync()` after querying `/me`

**HTTP Calls Made:**

- → Subscription Service: Fetches subscription plan/expiration via `SubscriptionServiceClient`
- **Reason**: Subscription data changes frequently (upgrades, expirations), not suitable for JWT

**Key Code:**

- [User.Application/Queries/GetUserProfile/GetUserProfileQueryHandler.cs](d:/repositories/FrogEdu/backend/Services/User/User.Application/Queries/GetUserProfile/GetUserProfileQueryHandler.cs) - Syncs role to Cognito
- [User.Infrastructure/Services/SubscriptionServiceClient.cs](d:/repositories/FrogEdu/backend/Services/User/User.Infrastructure/Services/SubscriptionServiceClient.cs) - Fetches subscription data

### ✅ Class Service

**JWT Claims Used:**

- `sub` → User identity
- `ClaimTypes.Role` → Read via `BaseController.GetUserRole()` for authorization
- Role-based filtering in `GetMyClassesQueryHandler` (Admin→all, Teacher→own, Student→enrolled)

**HTTP Calls Made:** ❌ None (removed `UserRoleClient` - was unused)

**Key Code:**

- [Class.API/Controllers/BaseController.cs](d:/repositories/FrogEdu/backend/Services/Class/Class.API/Controllers/BaseController.cs#L55-L90) - Extracts role from JWT
- [Class.Application/Queries/GetMyClasses/GetMyClassesQueryHandler.cs](d:/repositories/FrogEdu/backend/Services/Class/Class.Application/Queries/GetMyClasses/GetMyClassesQueryHandler.cs) - Role-based query logic

### ✅ Exam Service

**JWT Claims Used:**

- `sub` → User identity
- `ClaimTypes.Role` → Authorization via `[Authorize(Roles=...)]` attributes

**HTTP Calls Made:** ❌ None

**Status**: Full JWT-based authorization, no cross-service dependencies

### ✅ Subscription Service

**JWT Claims Used:**

- `sub` → User identity
- `ClaimTypes.Role` → Authorization

**HTTP Calls Made:** ❌ None

**Status**: Full JWT-based authorization, provides subscription data to other services

### ✅ AI Service (Python/FastAPI)

**JWT Claims Used:**

- `sub` → User identity
- `custom:role` → Extracted in `auth.py`

**HTTP Calls Made:**

- → Subscription Service: Fetches subscription plan for feature gating
- **Reason**: Rate limits and feature access based on subscription tier

**Key Code:**

- [AI/app/auth.py](d:/repositories/FrogEdu/backend/Services/AI/app/auth.py) - JWT validation
- [AI/app/services/subscription_client.py](d:/repositories/FrogEdu/backend/Services/AI/app/services/subscription_client.py) - Subscription client

## Self-Healing Role Sync Flow

### Initial State (New User)

1. User signs up → Cognito creates account
2. JWT has no `custom:role` yet → defaults to `Student` in middleware

### Self-Healing (First `/me` Call)

1. Frontend calls `GET /me` on app load
2. `GetUserProfileQueryHandler` fetches role from database
3. **Awaits sync** to Cognito's `custom:role` attribute via `CognitoAttributeService`
4. Frontend auto-refreshes token with `fetchAuthSession({ forceRefresh: true })`
5. New JWT now has `custom:role` claim → **no more HTTP calls needed**

### Subsequent Requests

1. JWT includes `custom:role` claim
2. `OnTokenValidated` maps `custom:role` → `ClaimTypes.Role`
3. `RoleEnrichmentMiddleware` sees role in JWT → **skips HTTP call entirely**
4. All authorization uses JWT claims from this point forward

**See**: [docs/role-sync-fix.md](d:/repositories/FrogEdu/docs/role-sync-fix.md) for detailed flow

## Shared Components

### RoleEnrichmentMiddleware (Shared.Kernel)

Located: `backend/Shared/Shared.Kernel/Authorization/RoleEnrichmentMiddleware.cs`

**Purpose**: Self-healing role enrichment for all microservices

**Logic**:

1. Check JWT for `ClaimTypes.Role`
   - If Admin or Teacher found → trust it, skip HTTP
2. If not found or is Student → call `RoleClaimsHttpClient`
3. If HTTP fails → preserve existing JWT role (graceful degradation)
4. Add `RoleClaimsDto` to `HttpContext.Items` for downstream handlers

**Key Benefit**: Lambda services can't reach each other → JWT-first approach prevents failures

### RoleClaimsHttpClient (Shared.Kernel)

Located: `backend/Shared/Shared.Kernel/Authorization/RoleClaimsHttpClient.cs`

**Purpose**: Fallback HTTP client for fetching role from User service when JWT doesn't have it

**Usage**: Called only by `RoleEnrichmentMiddleware` when JWT lacks role information

**Registration**: `services.AddRoleClaimsClient()` in each service's `DependencyInjection.cs`

## Best Practices

### ✅ DO

- **Use JWT claims for**: User identity, roles, basic auth data
- **Check JWT first** before making HTTP calls (like `RoleEnrichmentMiddleware` does)
- **Gracefully degrade** when HTTP calls fail
- **Cache subscription data** (AI service caches for 5 minutes)
- **Auto-sync role** on app load via `useMe` hook

### ❌ DON'T

- **Don't put business data in JWT**: Subscriptions, full profiles (changes too frequently)
- **Don't make HTTP calls if JWT has the data**: Check `ClaimTypes.Role` first
- **Don't fail requests if enrichment fails**: Default to safe values (Student role, Free plan)
- **Don't expose implementation details**: Users shouldn't know about HTTP calls vs JWT

## Monitoring & Observability

### Log Patterns to Watch

**Role Sync Success:**

```
Successfully synced role 'Teacher' to Cognito for user {CognitoId}
```

**JWT-First Hit (No HTTP):**

```
Role 'Admin' already present in JWT for CognitoSub {Sub} — skipping HTTP call
```

**HTTP Fallback:**

```
Fetching role claims for CognitoSub {Sub} from http://user-service/by-cognito/{Sub}
```

**Graceful Degradation:**

```
HTTP error fetching role claims for CognitoSub {Sub} — defaulting to JWT role or Student
```

## Migration Path

If adding a new microservice:

1. **Add `RoleEnrichmentMiddleware`** to pipeline
2. **Register `RoleClaimsHttpClient`** in DI: `services.AddRoleClaimsClient()`
3. **Read role from JWT**: Use `BaseController.GetUserRole()` or `ClaimTypes.Role`
4. **Never create custom role clients**: Use shared `RoleClaimsHttpClient`

## Testing

### Verify JWT-First Behavior

1. **Login** → Check JWT has `custom:role`
2. **Call any endpoint** → No HTTP to User service (check logs)
3. **Force role change in DB** → Call `/me` → Token auto-refreshes → New role active

### Verify Graceful Degradation

1. **Stop User service**
2. **Make request with valid JWT** → Should work (uses JWT role)
3. **Make request without `custom:role` in JWT** → Should default to Student

## Related Documentation

- [role-sync-fix.md](d:/repositories/FrogEdu/docs/role-sync-fix.md) - Detailed self-healing flow
- [authorization.md](d:/repositories/FrogEdu/docs/authorization.md) - Authorization patterns
- [microservices-details.md](d:/repositories/FrogEdu/docs/microservices-details.md) - Service architecture

## Summary

**Current State**: ✅ All services use JWT claims for roles/auth. Only subscription data uses HTTP calls (appropriate for business data).

**Result**:

- ⚡ Reduced latency (no HTTP for auth)
- 🛡️ More resilient (works when User service unavailable)
- 🏗️ Scalable (no inter-service coupling for auth)
