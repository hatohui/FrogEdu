As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

- Free Tier Subscription can only generate 3 times using AI.
- Paid Tier Subscription can generate unlimited times using AI.
- AI services should also track the user's usage and update the subscription status accordingly.
- The system should have a mechanism to notify users when they are approaching their usage limits or when their subscription is about to expire.
- The system should also have a mechanism to handle subscription renewals and cancellations.
- The user should be able to view their subscription status and usage history in their dashboard.
- When user subscribes to a plan, the system should create a subscription record in the database and associate it with the user. It should also create a transaction record for the payment and update the user's subscription status accordingly. -> Admins can see revenues in the dashboard and can also see the list of subscribers and their subscription status. Admins can also manage subscription plans and pricing.
- The NavBar and SideBar should only shows what the role of the user is allowed to see, for example, if the user is not an admin, they should not see the admin dashboard link. If the user is not logged in, they should only see the login and register links. ex: Students shouldn't be able to see the exam creation, there's already a exam taking or UPCOMING exam.

If anything, there should be a calendar for them to see upcoming exams and they can click on it to see the details of the exam, and if they are allowed to take the exam, they can click on the button to start the exam. If they are not allowed to take the exam, they should see a message saying that they are not allowed to take the exam.

The dashboard right now is hard code but it should be dynamic and should show the user's subscription status, usage history if user is a Teacher, upcoming exams if the user is a Student. The admin dashboard should show the list of subscribers, their subscription status, and the revenues generated from the subscriptions. The admin should also be able to manage subscription plans and pricing from the dashboard.

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis, run EF migrations and update if needed
