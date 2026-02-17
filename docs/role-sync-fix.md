# Role Synchronization Fix

## Problem

The Class service was defaulting to "Student" role for Admin users because:

1. Lambda functions cannot reach other services via `localhost:5001`
2. RoleEnrichmentMiddleware was failing to fetch role claims from User service
3. The fallback was defaulting to "Student"

## Solution: Self-Healing Role Sync

The role is synced to Cognito's `custom:role` attribute, and the middleware trusts JWT claims when the HTTP fallback is unavailable.

### Architecture

```
User calls GET /me
    → GetUserProfileQueryHandler fetches user from DB
    → Awaits CognitoAttributeService.SyncRoleAttributeAsync()
    → Cognito custom:role updated

Next request with refreshed JWT
    → OnTokenValidated maps custom:role → ClaimTypes.Role
    → RoleEnrichmentMiddleware checks ClaimTypes.Role
    → If Admin/Teacher found → SKIP HTTP call, trust JWT ✓
    → If HTTP call needed and fails → preserve JWT role ✓
    → Only default to Student if no role info exists anywhere
```

### Self-Healing Flow

1. **User fetches profile** (`GET /me` on User service)
   - `GetUserProfileQueryHandler` fetches user from database
   - **Awaits** sync of database role to Cognito's `custom:role` attribute
   - Returns user profile

2. **Subsequent requests** (any service)
   - JWT includes `custom:role` claim (set by Cognito)
   - `OnTokenValidated` maps `custom:role` → `ClaimTypes.Role`
   - `RoleEnrichmentMiddleware` sees non-Student role → skips HTTP call entirely
   - No inter-service communication needed

3. **Graceful degradation**
   - If HTTP call to User service fails, JWT role is preserved
   - Only defaults to Student when no role information exists anywhere
   - Logs actionable guidance: "User should call GET /me to sync"

### Changes Made

#### 1. RoleEnrichmentMiddleware (Shared Kernel)

- Three-tier role resolution:
  1. JWT has Admin/Teacher → trust it, skip HTTP
  2. HTTP call succeeds → use authoritative response
  3. HTTP fails but JWT has role → keep JWT role
- Prevents unnecessary inter-service HTTP calls in Lambda
- Clear logging at each decision point

#### 2. Cognito Attribute Service (User Service)

- `ICognitoAttributeService` interface + `CognitoAttributeService` implementation
- Uses AWS SDK `AdminUpdateUserAttributesAsync`
- Syncs `custom:role` attribute on every `/me` call

#### 3. GetUserProfileQueryHandler (User Service)

- Changed from fire-and-forget to properly awaited Cognito sync
- Catches sync failures gracefully without blocking response

#### 4. BaseController (All Services)

- Simplified `GetUserRole()` with clean fallback chain
- `ClaimTypes.Role` → `cognito:groups` → `custom:role` → "Student"
- `NormalizeRole()` helper for consistent casing

#### 5. JWT Token Mapping (All Services)

- `OnTokenValidated` maps `custom:role` → `ClaimTypes.Role`
- Consistent across User, Class, Exam, Subscription services

## Usage

### Self-Healing Behavior

No manual intervention required. When a user's role changes:

1. User calls `GET /api/users/me` (happens on every page load via `useMe` hook)
2. Role is synced to Cognito automatically
3. Next token refresh includes the correct `custom:role`
4. All services read the role from JWT — no logout/login needed

### For Existing Users

If role was recently changed in database and JWT doesn't reflect it:

1. The `RoleEnrichmentMiddleware` will log a warning and preserve whatever role it can find
2. On next `GET /api/users/me` call, the role is synced to Cognito
3. Token refresh picks up the new role automatically

## Testing

1. **Check role in JWT**:

   ```bash
   # Decode your JWT token and look for:
   # - "custom:role": "admin" (or "teacher", "student")
   # - "cognito:groups": ["Admin"] (optional)
   ```

2. **Call GET /classes**:

   ```bash
   curl -H "Authorization: Bearer YOUR_TOKEN" https://your-api.com/api/classes
   # Admin should see ALL classes
   # Teacher should see their classes
   # Student should see enrolled classes
   ```

3. **Check logs**:
   ```
   # You should see:
   "Successfully synced custom:role attribute to 'Admin' for Cognito user {id}"
   "Role found in ClaimTypes.Role: Admin"
   ```

## Environment Variables

User service needs these Cognito config vars:

```
AWS__Cognito__UserPoolId=us-east-1_XXX
AWS__Cognito__AccessKeyId=AKIA...
AWS__Cognito__SecretAccessKey=...
AWS__Cognito__Region=ap-southeast-1
```

## Notes

- Role sync is awaited (not fire-and-forget) for reliability
- Self-healing: one successful `/me` call permanently fixes the role
- RoleEnrichmentMiddleware skips HTTP when JWT already has Admin/Teacher role
- Falls back gracefully at every level — never crashes on missing role data
- Authorization per role: Admin → all classes, Teacher → own classes, Student → enrolled classes
