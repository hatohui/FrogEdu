As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

1. Student joins a class via invite code
2. Teacher can assigns a test to that class
3. Student can see the class's details and upcoming exams
4. Admins have a dashboard to view all classes created by teachers, and view all exams and students in that class, they can also assign in the dashboard.

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis
