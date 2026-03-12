As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

- Currently the feature flow where student taking their exams is implemented but not complete:
- The score is saved but it's not very friendly for primary school students, For the student UI i think we might want to simplify it more, like the dashboard should just be upcoming exams.
- The student can only see their score after the exam is graded, but we can also show them the questions they got wrong and the correct answers, so they can learn from their mistakes.
- The questions should have a button that query to our AI service to explain the question and the correct answer in a way that is easy for primary school students to understand. This will help them learn from their mistakes and improve their understanding of the material.
- There's already explaination field but there's no UI showing it after the student finishes their exam.
- There should also be a mode where the students can review their past exams and see their scores, the questions they got wrong, and the explanations for those questions. This will help them track their progress and identify areas where they need to improve.
- For the teacher UI, we can have a dashboard that shows upcoming exams, and when they click on an exam, they can see the list of students who took the exam, their scores, and the questions they got wrong. This will help teachers identify which students need extra help and which topics are causing the most trouble for their students.
- The teacher can also click on a student to see their detailed performance, including the questions they got wrong and the explanations for those questions. This will help teachers provide targeted feedback to their students and help them improve their understanding of the material.
- For the AI explanation feature, we can have a button next to each question in the student's exam review page. When the student clicks on the button, it will send a request to our AI service with the question and the correct answer, and the AI will return an explanation that is easy for primary school students to understand. This will help students learn from their mistakes and improve their understanding of the material.
- The AI explanation feature can also be integrated into the teacher's dashboard, where teachers can click on a button next to each question in the student's performance page to get an explanation for that question. This will help teachers provide better feedback to their students and help them improve their understanding of the material.
- The student should also have the option to retake the exam if they want to improve their score and the teacher allows it. This will encourage students to keep trying and learning from their mistakes until they achieve a better understanding of the material.

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis, run EF migrations and update if needed
