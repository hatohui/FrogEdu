// Request DTOs
export type { CreateExamRequest } from './exam'
export type {
	CreateMatrixRequest,
	UpdateMatrixRequest,
	AttachMatrixRequest,
} from './matrix'
export type { CreateQuestionRequest } from './question'
export type { CreateSubjectRequest, UpdateSubjectRequest } from './subject'
export type { CreateTopicRequest, UpdateTopicRequest } from './topic'

// Dashboard DTOs
export interface ExamDashboardStatsResponse {
	totalExams: number
	activeExams: number
	draftExams: number
	totalQuestions: number
	publicQuestions: number
}
