As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

Create user growth chart (last 30 days)
Create role distribution pie chart
Add verification status chart
Statistics about revenue from subscriptions, status of those subscriptions for dashboard

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis
