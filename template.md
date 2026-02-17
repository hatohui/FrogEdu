As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

- Teachers generates exams based on the matrixes via AI generation
- Currently, there's no selection for question_type if you generate by matrix, only by questions to questions, is there a better way to do this or an option to randomize it?
- The generated content is only in vietnamese, it should adapt to the frontend's lang=en or vi
- The UI UX for the generation is not CLEAN enough, its very cluttered and the feature to generate/template by the matrix is unclear for uesrs to see

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis
