# User Stories: Edu-AI Classroom

## Epics

1.  **Authentication & User Management**
2.  **Teacher Dashboard & Class Management**
3.  **Content Library Management**
4.  **Smart Exam Generation (Assessment)**
5.  **AI Student Tutor**
6.  **Payment & Subscription**
7.  **Question Bank Management (Pro)**
8.  **Analytics**

---

## 1. Authentication & User Management

### Story 1.1: User Registration & Login

**As a** generic user (Teacher/Student),
**I want to** log in using my secure credentials (via AWS Cognito),
**So that** I can access my personalized dashboard and data.

**Acceptance Criteria:**

- [ ] Support email/password login.
- [ ] Validate roles (Teacher vs. Student) upon login.
- [ ] Redirect to the appropriate dashboard (Teacher → Dashboard, Student → Tutor Interface).

### Story 1.2: Profile Management

**As a** User,
**I want to** update my profile (avatar, bio),
**So that** my identity is successfully represented in the classroom.

---

## 2. Teacher Dashboard & Class Management

### Story 2.1: Teacher Dashboard

**As a** Teacher,
**I want to** see an overview of my classes and recent activities,
**So that** I can quickly navigate to urgent tasks.

### Story 2.2: Class Creation

**As a** Teacher,
**I want to** create a new Class and generate an invite code,
**So that** students can enroll in my course.

---

## 3. Content Library Management

### Story 3.1: Textbook Navigation

**As a** Teacher,
**I want to** browse the digital textbook library by Grade, Subject, and Chapter,
**So that** I can find material to create lessons or exams for.

### Story 3.2: Lesson Asset Management

**As a** Teacher,
**I want to** upload or view supplementary assets (PDFs, Images),
**So that** I can enhance the standard curriculum.

---

## 4. Smart Exam Generator (Assessment)

### Story 4.1: Exam Matrix Configuration

**As a** Teacher,
**I want to** define an "Exam Matrix" (Ma trận) specifying the number of questions per difficulty (Easy/Medium/Hard) and per Chapter,
**So that** the system builds a balanced test.

### Story 4.2: Automated Question Selection

**As a** Teacher,
**I want to** have the system automatically pick questions from the Question Bank based on my Matrix,
**So that** I save time searching for individual questions.

### Story 4.3: Manual Override

**As a** Teacher,
**I want to** swap out a specific question if I don't like the AI's selection,
**So that** I ensure the test quality meets my specific standards.

### Story 4.4: Export to PDF

**As a** Teacher,
**I want to** export the finalized exam to a formatted PDF (stored in S3),
**So that** I can print it for a physical classroom test.

---

## 5. AI Student Tutor

### Story 5.1: Ask a Question

**As a** Student,
**I want to** ask the AI Tutor a question about my homework in natural language,
**So that** I can get help without waiting for the teacher.

**Acceptance Criteria:**

- [ ] Interface allows text input for questions.
- [ ] Integration with AI Service (Gemini) to process queries.
- [ ] Response includes relevant textbook context (RAG).
- [ ] System checks monthly query quota for free tier.

### Story 5.2: Socratic Guidance

**As a** Student,
**I want to** receive hints and guiding questions instead of direct answers,
**So that** I actually learn the logic behind the problem.

**Acceptance Criteria:**

- [ ] AI prompt is engineered to refuse direct answers.
- [ ] AI provides step-by-step reasoning or hints.
- [ ] Follow-up questions are supported.

### Story 5.3: Textbook References

**As a** Student,
**I want to** see links or snippets from the textbook relevant to my question,
**So that** I can read the source material to support my understanding.

**Acceptance Criteria:**

- [ ] Response includes citations/links to specific textbook chapters/pages.
- [ ] Clicking citation opens the Textbook viewer.

---

## 6. Payment & Subscription

### Story 6.1: Upgrade to Pro

**As a** Teacher,
**I want to** upgrade my account to the Pro plan,
**So that** I can access unlimited exams, all grades, and advanced features.

**Acceptance Criteria:**

- [ ] Display pricing plans (Monthly/Yearly).
- [ ] Integrate with Payment Gateway (VNPay/Stripe).
- [ ] Update user role/claims upon successful payment.
- [ ] Send email receipt.

### Story 6.2: Subscription Management

**As a** Pro Teacher,
**I want to** view my subscription status and next billing date,
**So that** I can manage my expenses.

**Acceptance Criteria:**

- [ ] Dashboard shows "Pro" badge and valid-until date.
- [ ] Option to cancel subscription (revert to Free at end of period).

---

## 7. Question Bank Management (Pro)

### Story 7.1: Create Custom Question

**As a** Pro Teacher,
**I want to** add my own questions to a private question bank,
**So that** I can use them in future exams.

**Acceptance Criteria:**

- [ ] Form to input question text, answer options, and correct answer.
- [ ] Ability to tag with Grade, Subject, Chapter, Difficulty.
- [ ] Support for rich text or image attachments.

### Story 7.2: Bulk Import

**As a** Pro Teacher,
**I want to** import questions from an Excel/CSV file,
**So that** I can quickly migrate my existing resources.

**Acceptance Criteria:**

- [ ] Provide a downloadable template.
- [ ] Validate uploaded file format and content.
- [ ] Show summary of successful vs failed imports.

---

## 8. Analytics

### Story 8.1: Basic Stats (Free)

**As a** Teacher,
**I want to** see basic usage statistics,
**So that** I know how many students are active.

**Acceptance Criteria:**

- [ ] Dashboard shows total students count.
- [ ] Dashboard shows total exams generated count.

### Story 8.2: Advanced Insights (Pro)

**As a** Pro Teacher,
**I want to** see detailed student performance and AI usage patterns,
**So that** I can identify struggling students.

**Acceptance Criteria:**

- [ ] Charts showing class performance trends.
- [ ] List of "Struggling Students" based on quiz results (if available) or AI query topics.
- [ ] Export data to PDF/Excel.
      **So that** I can get help without waiting for the teacher.

### Story 5.2: Socratic Guidance

**As a** Student,
**I want to** receive hints and guiding questions instead of direct answers,
**So that** I actually learn the logic behind the problem.

### Story 5.3: Textbook References

**As a** Student,
**I want to** see links or snippets from the textbook relevant to my question,
**So that** I can read the source material to support my understanding.
