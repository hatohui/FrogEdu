As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

- Currently the feature flow where student taking their exams is implemented but not complete:
- There's a few more types of question type that should be implemented, the essay question type is the most important one to be implemented, and it requires integration with AI service to grade the answer and provide feedback, so you need to implement the API for that in exam service and also the integration with AI service, and also implement the frontend for that.
- Make sure the other types of question are implemented as well, such as true/false, fill in the blank, and matching type questions.
- After implementing the exam taking flow, implement the exam session history page for students where they can see their past attempts and results, and also implement the review page where they can review their answers and see the feedback for each question, especially for the essay type questions.

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis, run EF migrations and update if needed
