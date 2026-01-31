# Microservices Details

Count: 5

## Shared between services

```mermaid
classDiagram
    class IAuditableEntity {
        <<interface>>
        +DateTime CreatedAt
        +Guid CreatedBy
        +DateTime? UpdatedAt
        +Guid? UpdatedBy
    }

    class BaseEntity {
        <<abstract>>
        +Guid Id
        +GenerateId()
    }

    class ISoftDeleteEntity {
        <<interface>>
        +bool IsDeleted
        +DateTime? DeletedAt
        +Guid? DeletedBy
        +UndoDelete()
    }

    BaseEntity ..|> IAuditableEntity : implements
```

## Microservices

### I. User Service

#### Responsibility

Roles:

- Admin: Can do everything, bypass all restrictions in the application
- Teacher: Able to do everything that allow role Teacher to do, usually teaching and exam related stuffs
- Student: Basic exam taking and practicing. anything that the role Student can do

Authentication:

- Intergrated with Cognito to enable Auth and sign-in, sign-ups, account related stuffs.

User Static-file:

- Intergated with S3 compatible SDK to sign-url all the assets that users upload, such as avatars, personal picture or question set thumbnail... etc

#### Class Diagram

```mermaid
classDiagram
    class User {
        +Guid Id
        +string CognitoSub
        +string Email
        +string FirstName
        +string LastName
        +string AvatarUrl
        +bool IsActive
        +UpdateProfile()
        +bool IsDeleted
        +DateTime? DeletedAt
        +Guid? DeletedBy
        +UndoDelete()
    }

    class Role {
        +string Guid
        +string Name
        +string Description
    }

    User "1" --> "1" Role : has
```

---

### II. Exam Service

#### Responsibility

Primary Goal: Manage the creation and structure of educational content.
Curriculum Management: CRUD for Subjects and Topics. (Initially seeded manually to follow the Vietnamese curriculum).
Question Bank: Manual CRUD for Questions and Answers. Includes metadata tagging (Cognitive Level, Difficulty, Topic).
Exam Orchestration: Building the Exam entity by linking multiple questions.
Matrix Engine: Creating the "Blueprint" (The Matrix) that defines how many questions per topic/difficulty an exam should have.
Export Engine: Converting the structured Exam data into PDF format for printing or offline use.

#### Class Diagram

```mermaid
classDiagram
    class Subject {
        +Guid         Id
        +string       SubjectCode
        +string       Description
        +string       Name
        +string       ImageURL
        +int          Grade
        +UpdateProfile()
    }

    class Exam {
        +Guid       Id
        +string     Title
        +int        Duration
        +string     AccessCode
        +int        PassScore
        +int        MaxAttempts
        +DateTime   StartTime
        +DateTime   EndTime
        +bool       ShouldShuffleQuestions
        +bool       ShouldShuffleAnswerOptions
        +bool       IsDraft
        +bool       IsActive
        +DateTime   CreatedAt
        +Guid       CreatedBy
        +DateTime?  UpdatedAt
        +Guid?      UpdatedBy
        +Publish()
        +Archive()
    }

    class Topic {
        +Guid       Id
        +string     Title
        +string     Description
        +boolean    IsCurriculum
        +DateTime   CreatedAt
        +Guid       CreatedBy
        +DateTime?  UpdatedAt
        +Guid?      UpdatedBy
        +GetTopics()
    }

    class MatrixTopic {
        +enum CognitiveLevel
        +Guid MatrixId
        +Guid ExamId
        +int Quantity
    }

    class Matrix {
        +Guid Id
        +DateTime   CreatedAt
        +Guid       CreatedBy
        +DateTime?  UpdatedAt
        +Guid?      UpdatedBy
    }

    class Question {
        +Guid Id
        +string Content
        +double Point
        +enum Type
        +string MediaUrl
        +enum CognitiveLevel
        +bool IsPublic
        +enum Source
        +DateTime   CreatedAt
        +Guid       CreatedBy
        +DateTime?  UpdatedAt
        +Guid?      UpdatedBy
        +GetAnswers()
    }

    class ExamQuestion {
      +Guid ExamId
      +Guid QuestionId
    }

    class Answer {
        +Guid Id
        +string Content
        +int Point
        +string Explanation
        +MarkAsCorrect()
    }

    Subject "1" --> "*" Topic : has
    Topic "1" --> "*" MatrixTopic
    MatrixTopic "*" --> "1" Matrix
    Matrix "*" --> "1" Exam
    Topic "1" --> "*" Exam
    Exam "1" --> "*" ExamQuestion
    Question "1" --> "*" ExamQuestion
    Question "1" --> "*" Answer
    Topic "1" --> "*" Question : categorizes
```

---

### III. Subscription Service

#### Responsibility

Primary Goal: Manage monetization and feature-gate access based on payment status.
Plan Management: Defining SubscriptionTiers (Free vs. Pro) and their specific constraints (e.g., "Max 30 students").
Payment Integration: Communicating with VNPay, Stripe, or PayOS to process transactions.
Entitlement Checking: Providing a "Gatekeeper" API that other services call to see if a user is allowed to perform an action (e.g., CanUserCreateMoreExams?).
Transaction Logging: Maintaining a permanent ledger of all payments and renewal dates for accounting.

#### Class Diagram

```mermaid
classDiagram
    class SubscriptionTier {
        +Guid     Id
        +string   Name
        +string   ImageUrl
        +string   Description
        +decimal  Price
        +int      DurationInDays
        +string   TargetRole
        +bool     IsActive
    }

    class UserSubscription {
        +Guid Id
        +Guid UserId
        +DateTime StartDate
        +DateTime EndDate
        +enum Status
        +IsExpired() bool
    }

    class Transaction {
        +Guid Id
        +string TransactionCode
        +decimal Amount
        +string Currency
        +enum PaymentProvider
        +enum PaymentStatus
        +string ProviderTransactionId
        +DateTime CreatedAt
        +UpdateStatus()
    }

    SubscriptionTier "1" -- "*" UserSubscription : defines
    UserSubscription "1" -- "*" Transaction : paid via
```

### IV. AI Service

#### Responsibility

Primary Goal: Augment the manual workflow with generative capabilities via Gemini + MCP.
Contextual Generation: Taking a Matrix (from Exam Svc) and generating Questions/Answers that fit the specific Vietnamese curriculum via the MCP tool.
Validation: Ensuring generated content matches the requested CognitiveLevel (Easy/Medium/Hard).
Student Tutoring: Providing a chat-based interface for students to ask questions about specific topics (using "Teacher-like" persona).
Usage Budgeting: Tracking token consumption per user to ensure the platform remains profitable (preventing AI spam).
Formatting: Ensuring AI output is always converted into the exact JSON structure the Exam Service expects.
Exam question and answers generation

- Take in an exam matrix, grade, subject and topic -> Query to Gemini AI that have our MCP connected for metadata of each topic in that subject of that grade and generates the questions + answers following that matrix with proper grade and following a json -> api
- Can generate just one question + answers

- Can support query tutoring and guiding students on how to do a specific math, speaking to a student of that grade

#### Requirements

- `Role:Teacher` and `Role:Student` need to have a subscription
- Normal users have no access

### Class Service

#### Responsibility

Primary Goal: Connect Users (Teachers/Students) to Content (Exams) in a structured environment.
Classroom Management: CRUD for ClassRoom entities (Name, Grade, Year).
Membership Logic: Handling the 6-digit Invite Code system for student enrollment.
Assignment Logic: The "Bridge" – linking an ExamId to a ClassId with a specific DueDate.
Attendance/Roster: Maintaining the list of active students within a specific teacher's classroom.
Progress Tracking: (Manual Phase) Storing simple records of which students have completed which assignments.

#### Class Diagram

```mermaid
classDiagram
    class ClassRoom {
      +Guid Id
      +string Name
      +string Grade
      +string InviteCode
      +int MaxStudents
      +string BannerUrl
      +bool IsActive
      +Guid TeacherId
      +RegenerateCode()
    }

    class ClassEnrollment {
        +Guid Id
        +Guid ClassId
        +Guid StudentId
        +DateTime JoinedAt
        +enum Status
        +KickStudent()
    }

    class Assignment {
        +Guid Id
        +Guid ClassId
        +Guid ExamId
        +DateTime StartDate
        +DateTime DueDate
        +bool IsMandatory
        +int Weight
    }

    %% Relationships
    ClassRoom "1" --> "*" ClassEnrollment : contains
    ClassRoom "1" --> "*" Assignment : has
```
