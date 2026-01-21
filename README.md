# FrogEdu - Edu-AI Classroom

[![backend-cd](https://img.shields.io/badge/Backend-.NET%209-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/) [![frontend-cd](https://img.shields.io/badge/Frontend-React%2019-61DAFB?style=for-the-badge&logo=react&logoColor=black)](https://react.dev/)

## [Documentations](specs/)

FrogEdu is a comprehensive AI-powered educational platform designed for Vietnamese primary schools. It provides intelligent exam generation, Socratic AI tutoring, and digital content management to enhance the teaching and learning experience.

## Project Information

### 💻 Tech Stack

#### 🖥️ Frontend

![React](https://img.shields.io/badge/React-19-61DAFB?style=for-the-badge&logo=react&logoColor=black)
![TypeScript](https://img.shields.io/badge/TypeScript-5.8-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![Vite](https://img.shields.io/badge/Vite-7.1-646CFF?style=for-the-badge&logo=vite&logoColor=white)
![TailwindCSS](https://img.shields.io/badge/TailwindCSS-4.1-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)
![Shadcn UI](https://img.shields.io/badge/Shadcn_UI-3.6-000000?style=for-the-badge)
![TanStack Query](https://img.shields.io/badge/TanStack_Query-5.89-FF4154?style=for-the-badge&logo=reactquery&logoColor=white)
![React Router](https://img.shields.io/badge/React_Router-7.8-CA4245?style=for-the-badge&logo=reactrouter&logoColor=white)
![Zod](https://img.shields.io/badge/Zod-4.1-EF4444?style=for-the-badge)
![i18next](https://img.shields.io/badge/i18next-25.5-26A69A?style=for-the-badge&logo=i18next&logoColor=white)
![ESLint](https://img.shields.io/badge/ESLint-9.33-4B32C3?style=for-the-badge&logo=eslint&logoColor=white)

#### 🧠 Backend

![.NET 9](https://img.shields.io/badge/.NET-9-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF_Core-9-6DB33F?style=for-the-badge)
![MediatR](https://img.shields.io/badge/MediatR-12-FF6B6B?style=for-the-badge)
![FluentValidation](https://img.shields.io/badge/FluentValidation-11-009688?style=for-the-badge)
![MassTransit](https://img.shields.io/badge/MassTransit-8-FF6B35?style=for-the-badge)
![gRPC](https://img.shields.io/badge/gRPC-2.0-244C5A?style=for-the-badge&logo=grpc&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)
![Serilog](https://img.shields.io/badge/Serilog-8-FFFFFF?style=for-the-badge)

#### 🛢️ Database & Cache

![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)

#### ☁️ Infrastructure & Cloud Services

![AWS S3](https://img.shields.io/badge/AWS_S3-569A31?style=for-the-badge&logo=amazon-s3&logoColor=white)
![AWS Cognito](https://img.shields.io/badge/AWS_Cognito-FF9900?style=for-the-badge&logo=amazonaws&logoColor=white)
![Cloudflare Pages](https://img.shields.io/badge/Cloudflare_Pages-F38020?style=for-the-badge&logo=cloudflare&logoColor=white)
![OpenAI API](https://img.shields.io/badge/OpenAI_API-412991?style=for-the-badge&logo=openai&logoColor=white)
![Terraform](https://img.shields.io/badge/Terraform-7C3EED?style=for-the-badge&logo=terraform&logoColor=white)

#### 🚀 DevOps / Monitoring / CI-CD

![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Docker Compose](https://img.shields.io/badge/Docker_Compose-1488C6?style=for-the-badge&logo=docker&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/GitHub_Actions-2088FF?style=for-the-badge&logo=githubactions&logoColor=white)

#### ⚙️ Dev Tools

![Visual Studio](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visualstudio&logoColor=white)
![VS Code](https://img.shields.io/badge/VS_Code-007ACC?style=for-the-badge&logo=visual-studio-code&logoColor=white)
![Git](https://img.shields.io/badge/Git-F05032?style=for-the-badge&logo=git&logoColor=white)
![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)

#### System Architecture

<p align="center">
  <img src="docs/architecture.png" alt="architecture.png" />
  System Architecture Diagram
</p>

## Development

### Getting started:

#### 1. Set up environment:

Clone this repository, then clone `.env.example` to `.env` and populate with necessary information. The API keys will be added in later, check the group.

#### 2. Install Docker

Go to [Docker Desktop](https://www.docker.com/products/docker-desktop) and install Docker.

After installation completed. Create a personal account and login until you can see the `Containers` page.

_p/s: Leave docker open when you're developing with docker containers._

#### 3. Development

Open the command line for the directory by pressing **Ctrl + `**

For backend:

```
cd backend
```

For frontend:

```
cd frontend
```

Then run:

```
docker-compose -f .\docker-compose.dev.yaml up
```

to start up the development server.

_Notes:_

- **Backend:**  
  The backend API is available at [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html).

- **Frontend:**  
  The web application runs at [http://localhost:3000/](http://localhost:3000/).

- **Database:**  
  The SQL Server database is exposed on port `1433`.  
  Use the credentials from `.env.example` to connect via your preferred database viewer (e.g., SQL Server Management Studio) in the development environment.

---

### Committing to the project

#### 1. Creating/Assigning an Issue

Before starting work on a new feature or bug fix, create an issue in the repository to track the task. If an issue already exists, assign it to yourself.

**Steps:**

1. Go to the repository's "Issues" tab.
2. Click **New Issue** to create a new one, or select an existing issue relevant to your work.
3. Provide a clear title and description outlining the feature, bug, or task.
4. Assign the issue to yourself or the appropriate team member.

This ensures all work is tracked, discussed, and linked to commits and pull requests for better project management.

##### Issue Naming Convention

When creating issues, use the following naming convention:

```
[FE | related-feature] concise issue description
[BE | related-feature] concise issue description
```

- Use `[FE | ...]` for frontend issues and `[BE | ...]` for backend issues.
- Replace `related-feature` with the relevant feature or context (e.g., `Auth`, `Content`, `Assessment`).
- Write a short, clear description of the issue after the prefix.

<div align="center">

| Issue Title                              | Valid? |
| ---------------------------------------- | :----: |
| `[FE \| Auth] implement login page`      |   ✅   |
| `[BE \| Content] fix textbook deletion`  |   ✅   |
| `[FE \| Dashboard] add analytics widget` |   ✅   |

Tb-0. example of valid issue titles.

</div>

---

#### 2. Branching

- **Create a new branch for each feature:**  
  Start every new branch from `main`, focusing on a single feature or fix.

- **Merging workflow:**  
  When your feature is complete, open a Pull Request (PR) to merge your branch into `main` for deployment.

- **Feature branch collaboration:**  
  If collaborating on a feature, create additional branches from the feature branch and use PRs to merge changes back into it.

---

##### Branch Naming Convention

Branch names must follow this pattern:

<div align="center">

```
^(main$|(feature|fix|task)\/.+(?:-.*)?)$
```

</div>

Start branch names with `feature/`, `fix/`, or `task/`.

<div align="center">

| Branch Name            | Valid? |
| ---------------------- | :----: |
| `feature/login-page`   |   ✅   |
| `fix/crash-on-startup` |   ✅   |
| `task/update-readme`   |   ✅   |
| `feature-login-page`   |   ❌   |
| `feature/`             |   ❌   |
| `feature/login page`   |   ❌   |

Tb-1. example of valid branch names.

</div>

---

#### 3. Commiting

##### Commits Naming Convention:

Commit messages must follow this pattern:

<div align="center">

```
^(fix|feat|chore): .+$
```

</div>

- **Type:** Start with `fix:`, `feat:`, or `chore:` to indicate the nature of the change.
  - `fix:` for bug fixes
  - `feat:` for new features
  - `chore:` for maintenance or non-functional changes
- **Message:** After the type, add a concise description of the change.

<div align="center">

| Commit Message                 | Valid? |
| ------------------------------ | :----: |
| `feat: add login form`         |   ✅   |
| `fix: resolve endpoint bug`    |   ✅   |
| `chore: update dependencies`   |   ✅   |
| `feature: implement JWT login` |   ❌   |
| `fix login bug`                |   ❌   |

Tb-2. example of valid commit messages.

</div>

##

#### 4. Pull Requests

##### Pull Request Title Convention

Pull Request (PR) titles must follow this pattern:

<div align="center">

```
^\[(FE|BE)(\s*\|\s*[a-zA-Z0-9-_]+)?\]\s+[a-zA-Z].+
```

</div>

- Start with `[FE | feature]` or `[BE | feature]` (the `| feature` part is optional, but recommended for clarity).
- Use `FE` for frontend and `BE` for backend.
- After the prefix, add a concise description of the PR, starting with a letter (uppercase or lowercase).

<div align="center">

| PR Title                                | Valid? |
| --------------------------------------- | :----: |
| `[FE \| auth] Add login page`           |   ✅   |
| `[BE \| content] Fix textbook deletion` |   ✅   |
| `[FE] Update dashboard layout`          |   ✅   |
| `[BE] Implement API endpoint`           |   ✅   |
| `[FE \| ] Add tests`                    |   ✅   |
| `[FE]add login page`                    |   ✅   |
| `[FE \| auth] add Login Page`           |   ✅   |
| `add login page`                        |   ❌   |

Tb-3. example of valid PR titles.

</div>

#### Commit & Pull Request Descriptions:

- **Commit Description:**  
  Clearly describe what changed in the commit. Include a brief summary of the new feature, bug fix, or technical change. Focus on what was modified, added, or removed.

  Special: include ``#skip` in the `commit message` to skip deployment

- **Pull Request Description:**  
  The PR description should include the commit description and reference the related issue using one of the following keywords:
  - `close`
  - `closes`
  - `closed`
  - `fix`
  - `fixes`
  - `fixed`
  - `resolve`
  - `resolves`
  - `resolved`

  Tag the issue by adding `#<issue_tag>` immediately after the keyword. If no related issue exists, create one before submitting the PR. For routine tasks (e.g., updating from `main`), tagging is optional.

**Example:**

```
[FE] add password reset functionality

- Implemented password reset form and API integration.
- Updated user authentication flow to support password resets.

closes #42
```

##

### Repository Rules

1. `main` is a protected branch, and is the production branch of this project, all commits made to it will be deployed via CI/CD, you can only do `Pull request` into it.

2. All `branches` & `commits` aside from `main` must follow a set naming convention.

3. All `commits` to a `feature` branch must be made through a `Pull Request`

4. At least `1` review is required for successful `PR`.

5. The code must pass the security check done by `CodeQL` and `GitGuardian`.

---

## Project Structure

### Backend (Microservices Architecture)

```
backend/
├── src/
│   ├── Services/
│   │   ├── Content/          # Textbook & lesson management
│   │   ├── Assessment/      # Exam generation & question bank
│   │   ├── User/            # User profiles & class management
│   │   └── AI/              # AI tutor orchestrator
│   └── Shared/
│       └── Shared.Kernel/   # Common domain primitives
```

### Frontend (Feature-Based Architecture)

```
frontend/
├── src/
│   ├── features/            # Feature modules
│   │   ├── auth/
│   │   ├── content/
│   │   ├── assessment/
│   │   └── ai-tutor/
│   ├── components/
│   │   ├── ui/              # Shadcn components
│   │   └── common/          # Shared components
│   ├── pages/               # Route entry points
│   └── services/            # API clients
```

---

## Key Features

### 🎓 For Teachers

- **Smart Exam Generator**: Create balanced exams using AI-powered question selection based on difficulty matrices
- **Content Library**: Browse and manage digital textbooks by grade, subject, and chapter
- **Class Management**: Create classes and manage student enrollments with invite codes
- **PDF Export**: Generate formatted exam papers for printing

### 🧑‍🎓 For Students

- **AI Tutor**: Get Socratic guidance on homework questions (no direct answers)
- **Textbook References**: Access relevant textbook content linked to questions
- **Conversation History**: Review past tutoring sessions

### 🔧 Technical Highlights

- **Microservices Architecture**: Clean separation of concerns with 4 autonomous services
- **Event-Driven Communication**: Asynchronous messaging via MassTransit/RabbitMQ
- **gRPC Integration**: High-performance inter-service communication
- **RAG Pipeline**: Retrieval-Augmented Generation for context-aware AI responses
- **Clean Architecture**: Domain-Driven Design with CQRS pattern

---

## Documentation

- [Backend Specifications](specs/backend/)
- [Frontend Specifications](specs/frontend/)
- [User Stories](specs/user-stories.md)
- [Architecture Overview](specs/backend/00-architecture-overview.md)

---

<div align="center">

### 🙏 Thank You for Being Part of FrogEdu!

</div>

We appreciate your contributions and dedication to the FrogEdu project.

- For questions or suggestions, please communicate within the project team or use the repository's [issue tracker](../../issues).
- **Note:** External contributions are not accepted, as this is a closed group project.

<div align="center">

Let's build something great together! 🚀

</div>
