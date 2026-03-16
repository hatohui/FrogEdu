As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

- There's no analytics for active classes, active exams, how many questions are generated, how many attempts are made by students, etc. We need to implement the API and dashboard to show these analytics to the teachers and Admin

- We need analytics for each exam session as well, such as how many students have attempted the exam, their scores, which questions are most difficult, etc. We need to implement the API and dashboard to show these analytics to the teachers.

- Admin needs more analytics such as how many teachers are using the platform, how many classes are created, how many exams are created, how many exam-sessions are created, how many questions are generated and when, etc. We need to implement the API and dashboard to show these analytics to the Admin.

- Currently there's no CRUD for Subjects and Topics, they are seeded manually. We will keep it that way for now, but we need to implement the API and dashboard to manage them in the future.

- The list should be paginated and searchable. Each of the Object in the list should be clickable and leads to the detail page where we can edit the information of that Object.
  There should be action buttons to create new Object, edit existing Object and delete an Object. The delete action should have a confirmation modal before executing the action.

- The Question Bank is also managed manually for now, but we will need to implement the API and dashboard for it in the future as well. Each question should have metadata tagging for Cognitive Level, Difficulty, and Topic.

- The Class creation form is bugged out, the UI is not well structured and the options allow up to 12 classes which is false. we only support grade 1-5.

- pages.classes.detail.edit_details button is also not working and the i18n keys are missing

- exams.cognitive_level and exams.difficulty keys are also missing in i18n, especially in app/exams/[examId]/questions/create page where we need to select the cognitive level and difficulty of each question in the exam.

- Teachers currently even on Pro subscription cannot see the Question bank nor can they search or see questions in the question bank. We need to implement the API and dashboard for them to manage the question bank as well as view and search questions in the question bank.

- /app/exam-sessions should also be that in calendar, but view is different, it should be a list of exam sessions with the ability to filter by class, subject, and date. Each exam session should be clickable and leads to the exam session detail page where they can see the list of students who have attempted the exam, their scores, and the questions in the exam. They should also be able to review each student's attempt and provide feedback. Remove the nav for exam sessions in the sidebar

- /profile/subscription should also show the usage of their subscription plan, such as how many attempts they have used, how many AI explanations they have used, etc.

- pages.dashboard.questions.table.points is missing i18n key, {t('pages.dashboard.classes.form.name')} {t('pages.dashboard.classes.form.grade')}
  missing keys

Bug:

- When AI generates exam questions and they're not saved yet, if the teacher navigates away from the page and comes back, the generated questions are lost. We need to implement a way to save the generated questions temporarily so that they can be retrieved when the teacher comes back to the page.

- When the teacher choose to edit the questions that was generated, and save it. The edited question is saved separately as a new question and the old one that we choose to edit is not deleted, which causes duplicates in the question bank. We need to implement a way to update the existing question instead of creating a new one when the teacher edits a generated question.

- analytics.ai_usage_over_time
  analytics.ai_usage_description key 'analytics.ai_usage (en)' returned an object instead of string.

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis, run EF migrations and update if needed
