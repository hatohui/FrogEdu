# Feature 02: Class Management & Dashboard

**Status:** ✅ Complete  
**Priority:** P1 (High)  
**Effort:** 12-16 hours

---

## 1. Overview

Allows Teachers to organize students into classes and view a dashboard of activities. Students can join classes via code.

**Services:** `Class Service`  
**Frontend:** `/dashboard`, `/classes/*`

---

## 2. User Stories

| ID  | As a... | I want to...         | So that...                                 |
| --- | ------- | -------------------- | ------------------------------------------ |
| 2.1 | Teacher | Create a Class       | I can group my students.                   |
| 2.2 | Teacher | View Dashboard       | I see an overview of my classes and exams. |
| 2.3 | Teacher | Generate Invite Code | Students can join easily (6-digit code).   |
| 2.4 | Student | Join Class (Code)    | I can access my teacher's content.         |
| 2.5 | Teacher | View Class Roster    | I know who is enrolled.                    |

---

## 3. Technical Specifications

### 3.1 Domain Model (Backend)

**Aggregate Root:** `Class`

- **Properties:** `Name`, `Subject`, `GradeLevel`, `InviteCode`, `TeacherId`.
- **Logic:** `GenerateInviteCode`, `EnrollStudent`, `RemoveStudent`.

**Entity:** `ClassMembership`

- **Properties:** `ClassId`, `UserId` (Student), `JoinedAt`.

### 3.2 Database Schema (Neon: `frogedu-users` or `frogedu-classes`?)

_Note: Depending on microservice boundaries, this typically fits in a `ClassService` or `UserService`. Assuming `User` context handles membership or distinct `Class` context._
_Decision: Separate Schema in `frogedu-users` database or distinct DB if strictly separated. Let's use `frogedu-classes` DB._

```sql
CREATE TABLE Classes (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    TeacherId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(100),
    Subject NVARCHAR(50),
    GradeLevel INT,
    InviteCode CHAR(6) UNIQUE,
    InviteCodeExpiry DATETIME2,
    IsArchived BIT DEFAULT 0
);

CREATE TABLE ClassMemberships (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ClassId UNIQUEIDENTIFIER REFERENCES Classes(Id),
    StudentId UNIQUEIDENTIFIER,
    JoinedAt DATETIME2
);
```

### 3.3 Frontend Architecture

**Routes:**

- `/dashboard` (Teacher Home)
- `/classes/create`
- `/classes/:id`
- `/join-class` (Student)

**State:**

- `useClassStore`: Optional, mostly Server state via TanStack Query is enough.

---

## 4. Implementation Checklist

### 4.1 Backend (Class Service)

- [x] **Domain**:
  - [x] `Class` Entity, `InviteCode` Value Object.
  - [ ] Domain Events: `StudentEnrolled`.
- [x] **Application**:
  - [x] `CreateClassCommand`.
  - [x] `JoinClassCommand` (Validate code).
  - [x] `GetTeacherClassesQuery`.
  - [x] `GetClassDetailsQuery` (includes roster).
- [x] **API**:
  - [x] `POST /api/classes`
  - [x] `POST /api/classes/join`
  - [x] `GET /api/classes/:id`

### 4.2 Frontend (Dashboard & Classes)

- [x] **Components**:
  - [x] `DashboardStats`: Cards for class count, student count.
  - [x] `ClassCard`: Navigate to detail.
  - [x] `CreateClassModal`: Form with Grade/Subject select.
  - [x] `JoinClassForm`: 6-digit input.
  - [x] `StudentList`: Table with avatars.
- [x] **Pages**:
  - [x] `DashboardPage`: Layout with Sidebar.
  - [x] `ClassDetailPage`.

### 4.3 Integration

- [x] Verify Invite Code generation uniqueness.
- [x] Ensure students cannot join same class twice.

---

## 5. Acceptance Criteria

- [x] Teacher can create a class -> shows on Dashboard.
- [x] Invite code is 6 chars alphanumeric.
- [x] Student joining updates the Roster immediately.
