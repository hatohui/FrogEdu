export type {
	CreateClassRequest,
	JoinClassRequest,
	AssignExamRequest,
	UpdateAssignmentRequest,
} from './request'

export type {
	CreateClassResponse,
	JoinClassResponse,
	AssignmentResponse,
} from './response'

export type {
	CreateExamSessionRequest,
	UpdateExamSessionRequest,
	SubmitExamAttemptRequest,
	StudentAnswerSubmission,
} from './examSession.request'

// Dashboard DTOs
export interface ClassDashboardStatsResponse {
	totalClasses: number
	activeClasses: number
	totalExamSessions: number
	activeExamSessions: number
	totalAttempts: number
	submittedAttempts: number
}
