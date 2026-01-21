# Feature 07: Analytics

**Status:** 📝 Spec Phase  
**Priority:** P3 (Medium)  
**Effort:** 12-16 hours

---

## 1. Overview

Provides Teachers with insights into student performance (Exam results) and AI Tutor usage (common topics asked).

**Services:** `Assessment Service` (Results), `AI Service` (Logs), `Analytics Service` (Aggregator)  
**Frontend:** `/dashboard/analytics`

---

## 2. User Stories

| ID  | As a... | I want to...                 | So that...                                  |
| --- | ------- | ---------------------------- | ------------------------------------------- |
| 7.1 | Teacher | View Class Performance       | I know the average score on the last exam.  |
| 7.2 | Teacher | Identify Struggling Students | I can offer extra help.                     |
| 7.3 | Teacher | See AI Topics                | I know what students are asking about most. |

---

## 3. Technical Specifications

### 3.1 Data Source

- **Exams:** Aggregated from `frogedu-assessments`.
- **AI:** Aggregated from `frogedu-ai` (TutorSessions).

### 3.2 Frontend (Charts)

- Use `recharts` or `chart.js`.
- Diagrams: Bar charts (Scores), Word Cloud (AI Topics).

---

## 4. Implementation Checklist

### 4.1 Backend (Analytics Service)

- [ ] **Queries**:
  - [ ] `GetClassPerformanceQuery`: Avg scores per exam.
  - [ ] `GetStudentRiskQuery`: Students with <50% score trend.
  - [ ] `GetTopicHeatmapQuery`: Frequent keywords in AI chat.

### 4.2 Frontend (Analytics Feature)

- [ ] **Components**:
  - [ ] `ScoreDistributionChart`.
  - [ ] `TopicCloud`.
  - [ ] `AtRiskStudentList`.
- [ ] **Pages**:
  - [ ] `AnalyticsDashboardPage`.

---

## 5. Acceptance Criteria

- [ ] Charts render with real data.
- [ ] Privacy: Only Teacher sees data for THEIR class.
