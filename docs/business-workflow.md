# Edu-AI Classroom: Business Workflow

## Overview

This document outlines the core business workflows for the Edu-AI Classroom platform, including user journeys, payment touchpoints, and system interactions.

---

## 1. Teacher Onboarding & Class Setup

```mermaid
flowchart TD
    Start([Teacher Visits Platform]) --> Register[Register/Login via Cognito]
    Register --> RoleCheck{Role Validation}
    RoleCheck -->|Teacher| Profile[Complete Profile Setup]
    RoleCheck -->|Student| StudentFlow[Redirect to Student Flow]

    Profile --> Dashboard[Access Teacher Dashboard]
    Dashboard --> CreateClass[Create First Class]
    CreateClass --> GenerateCode[Generate 6-Digit Invite Code]
    GenerateCode --> ShareCode[Share Code with Students]
    ShareCode --> MonitorClass[Monitor Class Activities]

    style Start fill:#e1f5ff
    style Dashboard fill:#fff4e1
    style GenerateCode fill:#e8f5e9
```

---

## 2. Smart Exam Generation Flow (Core Revenue Driver)

```mermaid
flowchart TD
    Start([Teacher: Need to Create Exam]) --> SelectClass[Select Class & Subject]
    SelectClass --> BrowseTextbook[Browse Textbook Library]
    BrowseTextbook --> CheckAccess{Has Access to Grade?}

    CheckAccess -->|Free Tier: Grade 1-3| DefineMatrix[Define Exam Matrix]
    CheckAccess -->|Locked: Grade 4-5| PaywallContent[🔒 Upgrade to Pro for Full Library]
    PaywallContent --> UpgradeContent{Upgrade?}
    UpgradeContent -->|Yes| ProcessPayment[Process Payment]
    UpgradeContent -->|No| End1([Exit])
    ProcessPayment --> DefineMatrix

    DefineMatrix --> SetDifficulty[Set Easy/Medium/Hard Distribution]
    SetDifficulty --> SelectChapters[Select Chapters to Cover]
    SelectChapters --> ChooseQuestionTypes[Choose Question Types: MCQ/Essay]
    ChooseQuestionTypes --> CheckQuota{Exams Generated This Month}

    CheckQuota -->|< 3 exams: Free| AIGeneration[🤖 AI Auto-Generates Exam]
    CheckQuota -->|≥ 3 exams| PaywallExam[🔒 Upgrade for Unlimited Exams]

    PaywallExam --> UpgradeExam{Upgrade to Pro?}
    UpgradeExam -->|Yes| ProcessPayment2[Process Payment 299k VND/mo]
    UpgradeExam -->|No| End2([Exit])
    ProcessPayment2 --> AIGeneration

    AIGeneration --> ReviewQuestions[Review Generated Questions]
    ReviewQuestions --> CheckQuality{Satisfied with Questions?}

    CheckQuality -->|No| SwapQuestions[Manually Swap Questions]
    SwapQuestions --> ReviewQuestions
    CheckQuality -->|Yes| FinalizeExam[Finalize Exam]

    FinalizeExam --> ExportFormat{Export Format}
    ExportFormat -->|Free: PDF| ExportPDF[Export to PDF - Basic Template]
    ExportFormat -->|Pro: Docx| CheckProStatus{Has Pro Plan?}

    CheckProStatus -->|Yes| ExportDocx[Export to Docx - Editable]
    CheckProStatus -->|No| PaywallExport[🔒 Upgrade for Advanced Formats]
    PaywallExport --> End3([Exit])

    ExportPDF --> S3Upload[Upload to S3: /generated-exams/]
    ExportDocx --> S3Upload
    S3Upload --> PresignedURL[Generate Presigned URL - 15min]
    PresignedURL --> Download[Download Exam]
    Download --> PrintExam[Print for Classroom Use]
    PrintExam --> End4([Complete])

    style AIGeneration fill:#e1f5ff
    style PaywallExam fill:#ffe1e1
    style PaywallContent fill:#ffe1e1
    style PaywallExport fill:#ffe1e1
    style ProcessPayment fill:#e8f5e9
    style ProcessPayment2 fill:#e8f5e9
    style ExportDocx fill:#fff4e1
```

---

## 3. Student AI Tutor Interaction Flow

```mermaid
flowchart TD
    Start([Student: Needs Homework Help]) --> Login[Student Login]
    Login --> ViewClasses[View Enrolled Classes]
    ViewClasses --> SelectClass[Select Class]
    SelectClass --> OpenTutor[Open AI Tutor Interface]

    OpenTutor --> CheckQuota{AI Queries This Month}
    CheckQuota -->|< 50 queries| AskQuestion[Ask Question in Natural Language]
    CheckQuota -->|≥ 50 queries| PaywallTutor[🔒 Query Limit Reached]

    PaywallTutor --> TeacherUpgrade[Notify: Ask Teacher to Upgrade]
    TeacherUpgrade --> End1([Exit])

    AskQuestion --> AIProcessing[🤖 AI Processing with RAG]
    AIProcessing --> ContextRetrieval[Retrieve Relevant Textbook Content]
    ContextRetrieval --> GenerateHints[Generate Socratic Hints]

    GenerateHints --> ShowResponse[Display Response:<br/>- Guiding Questions<br/>- Textbook References<br/>- No Direct Answers]
    ShowResponse --> StudentUnderstand{Understand?}

    StudentUnderstand -->|No| FollowUp[Ask Follow-up Question]
    FollowUp --> AIProcessing

    StudentUnderstand -->|Yes| SaveHistory[Save Conversation History]
    SaveHistory --> TeacherReview[Available for Teacher Review]
    TeacherReview --> End2([Learning Complete])

    style AIProcessing fill:#e1f5ff
    style PaywallTutor fill:#ffe1e1
    style ShowResponse fill:#e8f5e9
```

---

## 4. Payment & Subscription Flow

```mermaid
flowchart TD
    Start([User Hits Paywall]) --> ShowModal[Show Upgrade Modal]
    ShowModal --> ViewPricing[Display Pricing:<br/>Teacher Pro: 299k VND/mo<br/>School License: Custom]

    ViewPricing --> SelectPlan{Select Plan}
    SelectPlan -->|Monthly| MonthlyPlan[299,000 VND/month]
    SelectPlan -->|Yearly - 20% off| YearlyPlan[2,870,400 VND/year]
    SelectPlan -->|School License| ContactSales[Contact Sales Team]

    MonthlyPlan --> PaymentMethod{Payment Method}
    YearlyPlan --> PaymentMethod

    PaymentMethod -->|VNPay| VNPay[VNPay Gateway - Vietnam]
    PaymentMethod -->|Bank Transfer| BankTransfer[Local Bank Transfer]
    PaymentMethod -->|Credit Card| Stripe[Stripe - International]

    VNPay --> ProcessPayment[Process Payment]
    BankTransfer --> ProcessPayment
    Stripe --> ProcessPayment

    ProcessPayment --> PaymentStatus{Payment Success?}
    PaymentStatus -->|Failed| Retry[Show Error + Retry Option]
    Retry --> PaymentMethod

    PaymentStatus -->|Success| UpdateDB[Update User Subscription in DB]
    UpdateDB --> EmitEvent[Emit: SubscriptionActivated Event]
    EmitEvent --> UpdateJWT[Update JWT with Tier Info]
    UpdateJWT --> UnlockFeatures[Unlock Premium Features:<br/>✅ Unlimited Exams<br/>✅ Full Textbook Library<br/>✅ Unlimited AI Queries<br/>✅ Advanced Exports]

    UnlockFeatures --> SendReceipt[Send Receipt Email]
    SendReceipt --> RedirectDashboard[Redirect to Dashboard]
    RedirectDashboard --> ShowSuccess[Show Success Message]
    ShowSuccess --> End([Complete])

    ContactSales --> SalesTeam[Sales Team Follow-up]
    SalesTeam --> CustomQuote[Provide Custom Quote]
    CustomQuote --> End2([Enterprise Flow])

    style ProcessPayment fill:#fff4e1
    style UnlockFeatures fill:#e8f5e9
    style ShowSuccess fill:#e8f5e9
```

---

## 5. Content Management & Asset Upload Flow

```mermaid
flowchart TD
    Start([Teacher: Need Supplementary Materials]) --> Dashboard[Access Dashboard]
    Dashboard --> ContentLibrary[Go to Content Library]
    ContentLibrary --> BrowseOrUpload{Action}

    BrowseOrUpload -->|Browse| FilterContent[Filter by Grade/Subject/Chapter]
    BrowseOrUpload -->|Upload| UploadAsset[Upload Asset: PDF/Image/Video]

    FilterContent --> ViewContent[View Textbook Pages & Resources]
    ViewContent --> UseInLesson[Use in Lesson Plan/Exam]

    UploadAsset --> ValidateFile{File Valid?}
    ValidateFile -->|No| ShowError[Show Error: Size/Format]
    ShowError --> UploadAsset

    ValidateFile -->|Yes| S3Upload[Upload to S3: /user-uploads/{userId}/]
    S3Upload --> GenerateMetadata[Generate Metadata & Tags]
    GenerateMetadata --> SaveToDB[Save Reference in Content DB]
    SaveToDB --> ShareOption{Share with Other Teachers?}

    ShareOption -->|Yes| CheckPro{Has Pro Plan?}
    CheckPro -->|No| PaywallShare[🔒 Sharing Requires Pro]
    CheckPro -->|Yes| AddToShared[Add to Shared Library]

    ShareOption -->|No| PrivateLibrary[Keep in Private Library]
    AddToShared --> Success[Upload Complete]
    PrivateLibrary --> Success
    PaywallShare --> End1([Exit])

    Success --> UseInLesson
    UseInLesson --> End2([Complete])

    style S3Upload fill:#fff4e1
    style PaywallShare fill:#ffe1e1
    style Success fill:#e8f5e9
```

---

## 6. Student Enrollment Flow

```mermaid
flowchart TD
    Start([Student: Has Invite Code]) --> Login[Student Login/Register]
    Login --> Dashboard[Student Dashboard]
    Dashboard --> EnterCode[Enter 6-Digit Class Code]

    EnterCode --> ValidateCode{Code Valid?}
    ValidateCode -->|Invalid| ShowError[Show Error: Code Not Found]
    ShowError --> EnterCode

    ValidateCode -->|Valid| CheckCapacity{Class at Capacity?}
    CheckCapacity -->|Yes: Free Plan| PaywallCapacity[🔒 Teacher Needs to Upgrade]
    CheckCapacity -->|No| ConfirmEnroll[Confirm Enrollment]

    PaywallCapacity --> NotifyTeacher[Notify Teacher to Upgrade]
    NotifyTeacher --> End1([Wait for Teacher])

    ConfirmEnroll --> UpdateDB[Add Student to Class]
    UpdateDB --> SendNotification[Notify Teacher: New Student]
    SendNotification --> ShowWelcome[Show Welcome Message & Class Info]
    ShowWelcome --> AccessResources[Access Class Resources:<br/>- AI Tutor<br/>- Textbooks<br/>- Assignments]
    AccessResources --> End2([Enrolled Successfully])

    style ConfirmEnroll fill:#e8f5e9
    style PaywallCapacity fill:#ffe1e1
```

---

## 7. Analytics & Teacher Insights Flow

```mermaid
flowchart TD
    Start([Teacher: Review Performance]) --> Dashboard[Access Dashboard]
    Dashboard --> ViewAnalytics[View Analytics Section]

    ViewAnalytics --> CheckPlan{Has Pro Plan?}
    CheckPlan -->|No: Free Plan| BasicAnalytics[Basic Analytics:<br/>- Total Students<br/>- Exams Generated<br/>- Class Overview]
    CheckPlan -->|Yes: Pro Plan| AdvancedAnalytics[Advanced Analytics:<br/>- Student AI Usage Patterns<br/>- Common Questions<br/>- Difficult Topics<br/>- Exam Performance Predictions]

    BasicAnalytics --> UpgradePrompt[Show Upgrade Prompt for Advanced Features]
    UpgradePrompt --> End1([Exit or Upgrade])

    AdvancedAnalytics --> FilterData[Filter by:<br/>- Date Range<br/>- Class<br/>- Student<br/>- Subject]
    FilterData --> ViewCharts[View Interactive Charts & Graphs]
    ViewCharts --> ExportReport{Export Report?}

    ExportReport -->|Yes| GenerateReport[Generate PDF Report]
    ExportReport -->|No| End2([Complete])
    GenerateReport --> DownloadReport[Download Report]
    DownloadReport --> End2

    style AdvancedAnalytics fill:#e1f5ff
    style BasicAnalytics fill:#f5f5f5
    style UpgradePrompt fill:#ffe1e1
```

---

## 8. Question Bank Management Flow (Pro Feature)

```mermaid
flowchart TD
    Start([Teacher: Manage Question Bank]) --> Dashboard[Access Dashboard]
    Dashboard --> QuestionBank[Go to Question Bank]

    QuestionBank --> CheckPlan{Has Pro Plan?}
    CheckPlan -->|No| Paywall[🔒 Question Bank Management<br/>Requires Pro Plan]
    Paywall --> Upgrade{Upgrade?}
    Upgrade -->|No| End1([Exit])
    Upgrade -->|Yes| ProcessPayment[Process Payment]
    ProcessPayment --> ManageQuestions

    CheckPlan -->|Yes| ManageQuestions[Manage Questions]
    ManageQuestions --> Action{Action}

    Action -->|Add| CreateQuestion[Create New Question]
    Action -->|Edit| EditQuestion[Edit Existing Question]
    Action -->|Delete| DeleteQuestion[Delete Question]
    Action -->|Import| ImportBulk[Bulk Import from Excel/CSV]

    CreateQuestion --> SetMetadata[Set Metadata:<br/>- Difficulty<br/>- Chapter<br/>- Bloom's Taxonomy<br/>- Points<br/>- Textbook Reference]
    EditQuestion --> SetMetadata

    SetMetadata --> AddQuestion[Add to Question Bank]
    AddQuestion --> ShareQuestion{Share with Other Teachers?}

    ShareQuestion -->|Yes| SchoolLibrary[Add to School Shared Library]
    ShareQuestion -->|No| PrivateBank[Keep in Private Bank]

    SchoolLibrary --> Success[Question Saved]
    PrivateBank --> Success
    Success --> UseInExam[Available for Exam Generation]
    UseInExam --> End2([Complete])

    ImportBulk --> ValidateFormat{Valid Format?}
    ValidateFormat -->|No| ShowErrors[Show Import Errors]
    ShowErrors --> ImportBulk
    ValidateFormat -->|Yes| BulkInsert[Insert All Questions]
    BulkInsert --> Success

    DeleteQuestion --> ConfirmDelete{Confirm?}
    ConfirmDelete -->|Yes| RemoveQuestion[Remove from Bank]
    ConfirmDelete -->|No| End2
    RemoveQuestion --> End2

    style Paywall fill:#ffe1e1
    style Success fill:#e8f5e9
```

---

## Key Business Rules & Payment Touchpoints

### 🎯 **Monetization Points**

| Flow                | Free Tier Limit          | Paywall Trigger           | Action Required             |
| ------------------- | ------------------------ | ------------------------- | --------------------------- |
| **Exam Generation** | 3 exams/month            | 4th exam                  | Upgrade to Pro: 299k VND/mo |
| **AI Tutor**        | 50 queries/month/student | 51st query                | Teacher upgrades class      |
| **Textbook Access** | Grade 1-3 only           | Grade 4-5 content         | Upgrade to Pro              |
| **Class Limit**     | 1 Class, 30 Students     | 2nd Class or 31st Student | Upgrade to Pro              |
| **Export Formats**  | Basic PDF only           | Docx/Custom templates     | Pro feature                 |
| **Question Bank**   | View only                | Create/Edit/Import        | Pro feature                 |
| **Analytics**       | Basic metrics            | Advanced insights         | Pro feature                 |
| **Asset Sharing**   | Private only             | Share with colleagues     | Pro feature                 |

### 💡 **Conversion Strategy**

1. **Hook Phase (Free Trial):** 7-day full Pro access
2. **Habit Building:** Free tier allows core functionality
3. **Value Realization:** Teachers generate 2-3 exams (feel the value)
4. **Natural Paywall:** Hits limit during busy exam season
5. **Easy Upgrade:** One-click payment via VNPay (local) or Stripe

### 📊 **Success Metrics**

- **Free → Pro Conversion Rate Target:** 15-20%
- **Average Time to Conversion:** 14-21 days
- **Retention Rate:** 85% (annual)
- **Expansion Revenue:** School licenses from individual users

---

## System Architecture Context

These workflows interact with the following microservices:

- **User Service:** Authentication, profile management, subscription status
- **Content Service:** Textbook library, asset management
- **Assessment Service:** Exam generation, question bank
- **AI Service:** Tutor interactions, RAG processing
- **Payment Service:** Subscription management, payment processing
- **Analytics Service:** Usage tracking, insights generation

Each service communicates via HTTP/gRPC and shares correlation IDs for distributed tracing.
