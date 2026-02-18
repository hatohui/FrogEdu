As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

- Teachers generates exams based on the matrixes via AI generation

- Currently, there's no selection for question_type if you generate by matrix, only by single question but as an user I want to design my exam too, there should be matrix to defines the exams's scopes and everything, then there should be a design (optional) that defines what question types there are, and an option to randomize the question type if user just want to click twice for an exam.

- The generated content is only in vietnamese, it should adapt to the frontend's lang=en or vi

- The UI UX for the generation is not CLEAN enough, its very cluttered and the feature to generate/template by the matrix is unclear for uesrs to see

- When I try to assign as Admin w/o subscription, it fails:

info: System.Net.Http.HttpClient.ISubscriptionClaimsClient.LogicalHandler[100]
2026-02-17T23:28:58.130Z
Start processing HTTP request GET https://api.frogedu.org/api/subscriptions/claims/39eae57c-b001-7052-b6dd-1c4838574d8f
2026-02-17T23:28:58.131Z
info: System.Net.Http.HttpClient.ISubscriptionClaimsClient.ClientHandler[100]
2026-02-17T23:28:58.131Z
Sending HTTP request GET https://api.frogedu.org/api/subscriptions/claims/39eae57c-b001-7052-b6dd-1c4838574d8f
2026-02-17T23:28:58.203Z
info: System.Net.Http.HttpClient.ISubscriptionClaimsClient.ClientHandler[101]
2026-02-17T23:28:58.203Z
Received HTTP response headers after 43.8211ms - 401
2026-02-17T23:28:58.205Z
info: System.Net.Http.HttpClient.ISubscriptionClaimsClient.LogicalHandler[101]
2026-02-17T23:28:58.205Z
End processing HTTP request after 80.1662ms - 401

Though this feature requires no subscription, you can just assign. the only things that require subscription is AI service. You can check terraform to see if this is errored here too.

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis, run EF migrations and update if needed
