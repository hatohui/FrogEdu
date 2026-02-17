As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

{
"id": "e56b83d0-5236-484d-81a3-770a5b9441b4",
"name": "adsasdasdasdsa",
"grade": "5",
"inviteCode": "CP85UK",
"maxStudents": 33,
"bannerUrl": null,
"isActive": true,
"teacherId": "a91a856c-e0f1-70fb-ec85-fb70b0c766fb",
"createdAt": "2026-02-17T08:56:14.876692Z",
"studentCount": 1,
"enrollments": [
{
"id": "7f8e4393-8090-4b1e-8584-3db1099cb7eb",
"studentId": "39eae57c-b001-7052-b6dd-1c4838574d8f",
"joinedAt": "2026-02-17T20:24:23.460844Z",
"status": "Active"
}
],
"assignments": []
}

Currently the backend returns this as the endpoint for getClassById, the studentllist should fills in the name and the avatar using UserAvatar component, not just the ID, the list should display UserAvatar, Name, JoinedAt and actionMenu for:

- remove from class
- /app/classes/:id
- disableAccess

The page shouldn't show the code for students.

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis
