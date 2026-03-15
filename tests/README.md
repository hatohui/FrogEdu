# FrogEdu E2E Tests

Playwright-based end-to-end tests covering the 6 test modules from the test report.

## Setup

```bash
cd tests
npm install
npx playwright install chromium
cp .env.example .env   # fill in real credentials
```

## Pre-requisites

- Frontend running at `http://localhost:5173` (or set `BASE_URL`)
- Backend services running via `docker-compose`
- Test accounts created in Cognito with roles & subscriptions configured

## Running

```bash
npm test              # headless
npm run test:headed   # with browser visible
npm run test:ui       # interactive UI mode
npm run test:report   # open last HTML report
```

## Test Modules

| Spec File                  | Module                      | Test Cases          |
| -------------------------- | --------------------------- | ------------------- |
| `exam-taking.spec.ts`      | Student Exam Taking         | ST-001 → ST-018     |
| `exam-generation.spec.ts`  | Exam Generation             | EG-001 → EG-016     |
| `exam-history.spec.ts`     | Exam Session History        | EH-001 → EH-006     |
| `exam-review.spec.ts`      | Review Page                 | RV-001 → RV-012     |
| `class-enrollment.spec.ts` | Class Creation & Enrollment | CL-001 → CL-014     |
| `edge-cases.spec.ts`       | Edge Cases & Negative       | EDGE-001 → EDGE-007 |
