As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

- Teachers creates an ExamSession which includes the session data including: examId, startTime, endTime, retryTimes, isRetryable, isActive, shouldShuffleQuestions, shouldShuffleAnswers
- Create a scoring system for the class where it allows partial correctness, run DB migration with EF like other services if required.
- Students should be able to take the test via class-assigned exams when the exam is active and such. Have the options to see upcoming tests in app/exams (make a conditional check and move to components to clean up). Admin should be able to see both
- Saves the answers of students and let teachers see the list of takes by who, scored how much

The MVP can simply ignore the essay type questions for now.

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis
