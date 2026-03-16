As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

in `app/classes/:id`

when a student is kicked:

1. They still can view the class when reload, not "Access Denied" or something then redirect back to the other
2. When they join back, they show 2 users even though its' the same account
3. Teacher can't invite students to the class. and students have no way of accepting it
4. In profile/subscription: the student should not be able to see question created. only explaination usage.
5. There should be a reinvite button for the teacher to reinvite the student, and the student can accept it to join back the class. The reinvite button should only be visible when the student is kicked out of the class.

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis, run EF migrations and update if needed
