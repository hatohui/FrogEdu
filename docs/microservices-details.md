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
        +string FullName
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

Exams

- Allow `Role:Teacher` to create, save and export exams.
- Allow `Role:Teacher` to create, save and export questions of an exam.
- Allow `Role:Teacher` to create, save and export answers of questions of exams.

Exam Matrix

- Allow `Role:Teacher` to create exam matrix, topic.

Subject _ex: Toán_

- Handles seeded Subject Data like Math Grade 1, Math Grade 2...

Curriculum Topic _ex: Toán lớp 1 chương 1, chương 2, ..._

- Allow `Role:Teacher` to create, save custom topics.

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
        +bool       IsShuffled
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
        +bool IsShuffled
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
    Topic "1" -- "*" Question : categorizes
```

---

## III. Subscription Service

### Responsibility

Payment and Transactions

- Handle Payment flow and saves all transactions for analysis
- Subsciption Tier and tracks what each user haves

### Requirements

- `Role:Student` and `Role:Teacher` have different subscription tier

- Integrate with PayOS, Momo or PayVN

### IV. AI Service

#### Responsibility

Exam question and answers generation

- Take in an exam matrix, grade, subject and topic -> Query to Gemini AI that have our MCP connected for metadata of each topic in that subject of that grade and generates the questions + answers following that matrix with proper grade and following a json -> api

- Can generate just one question + answers

- Can support query tutoring and guiding students on how to do a specific math, speaking to a student of that grade

#### Requirements

- `Role:Teacher` and `Role:Student` need to have a subscription
- Normal users have no access

### Class Service

#### Responsibility

// To do
