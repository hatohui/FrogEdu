As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

- in `/app/exams/:id`, i got
  `System.UnauthorizedAccessException: You do not have permission to view this exam's questions`
  though i was admin

- in `/app/exams/:id/questions`, i got
  `System.UnauthorizedAccessException: You do not have permission to view this exam's questions`
  though i was admin

{
"type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
"title": "Not Found",
"status": 404,
"traceId": "00-6a464bcf1ab27a1d3dfcf35d9c9c054e-b0fc4f42ea6591c4-00"
}

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis, run EF migrations and update if needed
