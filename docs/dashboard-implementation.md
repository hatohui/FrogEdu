# Dashboard Pages Implementation

## Overview

This implementation adds three comprehensive dashboard pages for the FrogEdu platform:

- **User Management** (`/dashboard/users`)
- **Analytics** (`/dashboard/analytics`)
- **Settings** (`/dashboard/settings`)

## Pages Implemented

### 1. User Management Page (`/dashboard/users`)

**Location**: `frontend/src/pages/dashboard/users/page.tsx`

**Features**:

- **Statistics Cards**: Display total users, active users, teachers, and students
- **Search & Filter**: Search by name/email and filter by role (Admin, Teacher, Student)
- **User Table**: Comprehensive table with:
  - User avatar and name
  - Email address
  - Role badge (color-coded)
  - Verification status
  - Join date and last login
- **Actions**:
  - Edit user (dialog modal)
  - Send password reset email
  - Delete user (with confirmation dialog)
- **Responsive Design**: Mobile-friendly with appropriate breakpoints

**Components Used**:

- `Table`, `Card`, `Input`, `Select`, `Button`, `Dialog`, `Badge`, `Avatar`
- Icons from `lucide-react`

### 2. Analytics Page (`/dashboard/analytics`)

**Location**: `frontend/src/pages/dashboard/analytics/page.tsx`

**Features**:

- **Metrics Cards**: Revenue, active users, completion rate, average score
- **Time Range Selector**: Filter data by 7d, 30d, 90d, or 1y
- **Tabbed Interface** with 4 views:
  1. **Overview**: Combined view of all metrics
  2. **Users**: User growth and activity trends
  3. **Exams**: Exam performance and submission stats
  4. **Activity**: Daily activity breakdown

**Charts Implemented**:

- **Line Chart**: User growth trends (total, active, new users)
- **Pie Chart**: User role distribution
- **Bar Chart**: Weekly activity (logins, exams taken, questions created)
- **Dual-Axis Bar Chart**: Exam performance (average score vs submissions)

**Components Used**:

- Recharts: `LineChart`, `BarChart`, `PieChart`, `XAxis`, `YAxis`, `CartesianGrid`, `Tooltip`, `Legend`
- Custom tooltip with styled formatting
- Responsive containers for all charts

### 3. Settings Page (`/dashboard/settings`)

**Location**: `frontend/src/pages/dashboard/settings/page.tsx`

**Features**:

- **Tabbed Interface** with 4 sections:

#### Profile Tab

- Avatar upload (with file size limit note)
- Personal information (first name, last name, email, phone)
- Bio textarea
- Language preference (English, Vietnamese, Spanish, French)
- Timezone selection

#### Security Tab

- **Password Change**: Current password, new password, confirm password with show/hide toggle
- **Two-Factor Authentication**: Toggle with QR code setup and verification code input
- **Active Sessions**: Display current session with device/location info and option to revoke other sessions

#### Notifications Tab

- **Email Notifications**: Master toggle
  - Exam reminders
  - Class updates
  - Marketing emails (sub-toggles)
- **Push Notifications**: Toggle for push notifications

#### Appearance Tab

- **Theme Selection**: Light, Dark, or System
- **Display Preferences**:
  - Compact mode toggle
  - Show animations toggle

**Components Used**:

- `Tabs`, `Switch`, `Input`, `Textarea`, `Select`, `Button`, `Dialog`
- Password visibility toggles with eye icons
- Integrated with `useMe` hook for user data

## Services Created

### 1. Analytics Service

**Location**: `frontend/src/services/analytics.service.ts`

**API Endpoints**:

```typescript
// Get overview metrics
getOverviewMetrics(): Promise<ApiResponse<AnalyticsMetrics>>

// Get user growth data
getUserGrowth(timeRange: '7d' | '30d' | '90d' | '1y'): Promise<ApiResponse<UserGrowthData[]>>

// Get exam performance data
getExamPerformance(timeRange): Promise<ApiResponse<ExamPerformanceData[]>>

// Get activity data
getActivityData(timeRange): Promise<ApiResponse<ActivityData[]>>

// Get role distribution
getRoleDistribution(): Promise<ApiResponse<RoleDistribution[]>>
```

### 2. User Service Extensions

**Location**: `frontend/src/services/user-microservice/user.service.ts`

**New Admin Endpoints Added**:

```typescript
// Get all users with pagination and filters
getAllUsers(params?: {
  page?: number
  pageSize?: number
  search?: string
  role?: string
}): Promise<ApiResponse<PaginatedUsers>>

// Get user by ID
getUserById(userId: string): Promise<ApiResponse<GetMeResponse>>

// Update user
updateUser(userId: string, updates: Partial<UpdateProfileDto>): Promise<ApiResponse<void>>

// Delete user
deleteUser(userId: string): Promise<ApiResponse<void>>

// Send password reset email
sendPasswordResetEmail(email: string): Promise<ApiResponse<void>>
```

## Dependencies

### Installed

- ✅ `recharts`: For analytics charts and data visualization

### Existing (Already Available)

- `lucide-react`: Icons
- `@radix-ui/*`: UI primitives (dialog, select, switch, tabs, etc.)
- `react-hook-form`: Form handling
- `@tanstack/react-query`: Data fetching
- `tailwindcss`: Styling

## Backend Integration

These pages are designed to integrate with the following microservices:

### User Microservice

- **Base URL**: `/api/users`
- **Endpoints Used**:
  - `GET /users` - List all users (admin)
  - `GET /users/:id` - Get user details
  - `PUT /users/:id` - Update user
  - `DELETE /users/:id` - Delete user
  - `POST /users/auth/send-password-reset` - Send password reset

### Analytics Service (Planned)

- **Base URL**: `/api/analytics`
- **Endpoints**:
  - `GET /analytics/metrics/overview`
  - `GET /analytics/users/growth`
  - `GET /analytics/exams/performance`
  - `GET /analytics/activity`
  - `GET /analytics/users/distribution`

### User Settings

- Uses existing `/users/me` endpoints
- Password change via AWS Cognito
- 2FA integration with Cognito MFA

## Architecture Notes

### CQRS Pattern

Following the backend specification:

- **Commands**: Create, Update, Delete operations
- **Queries**: Read operations (get users, get analytics)
- Services return `Result<T>` or `ApiResponse<T>`

### DDD Principles

- Domain entities: User, Role, Analytics metrics
- Value objects: Email, UserRole
- Repository pattern for data access

### Clean Architecture

- **Presentation**: React components (pages)
- **Application**: Services and hooks
- **Domain**: Types and interfaces
- **Infrastructure**: API service and axios

## Routing

The pages are automatically registered by the dynamic router at:

- `/dashboard/users`
- `/dashboard/analytics`
- `/dashboard/settings`

They inherit the `AdminLayout` from `/dashboard/layout.tsx` and require admin authentication via `AdminRoute`.

## Next Steps

### Backend Implementation Needed

1. Create Analytics microservice with the defined endpoints
2. Add admin user management endpoints to User microservice
3. Implement pagination and filtering in user list endpoint
4. Add role-based access control for admin endpoints

### Frontend Enhancements

1. Connect mock data to real API endpoints
2. Add loading states and error handling
3. Implement real-time updates for analytics
4. Add export functionality for analytics data
5. Implement actual 2FA setup flow
6. Add file upload for avatar with S3 presigned URLs

### Testing

1. Unit tests for services
2. Integration tests for API calls
3. E2E tests for user flows

## File Structure

```
frontend/src/
├── pages/
│   └── dashboard/
│       ├── users/
│       │   └── page.tsx          # User management page
│       ├── analytics/
│       │   └── page.tsx          # Analytics dashboard
│       └── settings/
│           └── page.tsx          # Settings page
│
├── services/
│   ├── analytics.service.ts     # Analytics API service
│   └── user-microservice/
│       └── user.service.ts      # Extended user service (updated)
│
└── hooks/
    └── auth/
        └── useMe.ts             # Used in settings page
```

## Design System

All pages follow the existing design system:

- **Color Palette**: Consistent with theme (light/dark mode)
- **Typography**: Heading hierarchy and text styles
- **Spacing**: Using Tailwind spacing scale
- **Components**: shadcn/ui components
- **Icons**: lucide-react icons
- **Animations**: Smooth transitions and hover effects

## Accessibility

- Semantic HTML structure
- ARIA labels for interactive elements
- Keyboard navigation support
- Focus indicators
- Color contrast compliance
- Screen reader friendly

## Performance

- Lazy loading for charts
- Virtualization for large user lists (TODO)
- Debounced search
- Optimized re-renders with React.memo (where needed)
- Efficient data fetching with React Query
