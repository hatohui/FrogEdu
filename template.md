As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

[
{
"id": "400b28e0-6f30-4ccf-8abf-de2808b84ac4",
"classId": "e56b83d0-5236-484d-81a3-770a5b9441b4",
"examId": "68501b10-a3aa-4fe8-9b77-178ad2fed878",
"startTime": "2026-02-19T12:29:00Z",
"endTime": "2026-02-20T12:29:00Z",
"retryTimes": 1,
"isRetryable": false,
"isActive": true,
"shouldShuffleQuestions": false,
"shouldShuffleAnswers": false,
"allowPartialScoring": true,
"isCurrentlyActive": false,
"isUpcoming": true,
"hasEnded": false,
"attemptCount": 0,
"createdAt": "2026-02-18T12:29:38.75819Z"
},
{
"id": "37adbe39-5a40-4881-9bae-b651f9e79672",
"classId": "e56b83d0-5236-484d-81a3-770a5b9441b4",
"examId": "306d7ae1-970b-45a6-a38a-61a8f47cd4d5",
"startTime": "2026-02-18T12:36:00Z",
"endTime": "2026-02-19T12:36:00Z",
"retryTimes": 1,
"isRetryable": true,
"isActive": true,
"shouldShuffleQuestions": false,
"shouldShuffleAnswers": false,
"allowPartialScoring": true,
"isCurrentlyActive": true,
"isUpcoming": false,
"hasEnded": false,
"attemptCount": 0,
"createdAt": "2026-02-18T12:37:07.754546Z"
},
{
"id": "31422322-03e5-4988-890f-0be887ef1d58",
"classId": "e56b83d0-5236-484d-81a3-770a5b9441b4",
"examId": "306d7ae1-970b-45a6-a38a-61a8f47cd4d5",
"startTime": "2026-02-18T12:36:00Z",
"endTime": "2026-02-19T12:36:00Z",
"retryTimes": 1,
"isRetryable": true,
"isActive": true,
"shouldShuffleQuestions": false,
"shouldShuffleAnswers": false,
"allowPartialScoring": true,
"isCurrentlyActive": true,
"isUpcoming": false,
"hasEnded": false,
"attemptCount": 0,
"createdAt": "2026-02-18T12:37:08.014343Z"
}
]

the
https://api.frogedu.org/api/classes/exam-sessions/student?upcomingOnly=false

is returning duplicates so one item appear twice, this should not happen as they should only return enrolled exams once.

There's no way for you to see the attempts taken also

- Student can't see their own scores after submitting, if its their second try they don't show it as "retry" but as "start exam"
- Teachers can't see the attempts and score list taken by students
- Admin dashboard should also be able to see this in the dashboard under exams

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis, run EF migrations and update if needed
