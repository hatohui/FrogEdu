import type {
	ExamSession,
	StudentExamAttempt,
	ExamSessionResults,
} from '@/types/model/class-service'
import type {
	CreateExamSessionRequest,
	UpdateExamSessionRequest,
	SubmitExamAttemptRequest,
} from '@/types/dtos/classes'
import axiosInstance from '../axios'

const baseUrl = '/classes/exam-sessions'

export const examSessionService = {
	// ─── Teacher: Session Management ───

	createExamSession: async (
		classId: string,
		data: CreateExamSessionRequest
	): Promise<ExamSession> => {
		const response = await axiosInstance.post<ExamSession>(
			`${baseUrl}/classes/${classId}`,
			data
		)
		return response.data
	},

	updateExamSession: async (
		sessionId: string,
		data: UpdateExamSessionRequest
	): Promise<ExamSession> => {
		const response = await axiosInstance.put<ExamSession>(
			`${baseUrl}/${sessionId}`,
			data
		)
		return response.data
	},

	deleteExamSession: async (sessionId: string): Promise<void> => {
		await axiosInstance.delete(`${baseUrl}/${sessionId}`)
	},

	// ─── Session Queries ───

	getExamSessions: async (classId: string): Promise<ExamSession[]> => {
		const response = await axiosInstance.get<ExamSession[]>(
			`${baseUrl}/classes/${classId}`
		)
		return response.data
	},

	getExamSessionDetail: async (sessionId: string): Promise<ExamSession> => {
		const response = await axiosInstance.get<ExamSession>(
			`${baseUrl}/${sessionId}`
		)
		return response.data
	},

	getStudentExamSessions: async (
		upcomingOnly: boolean = false
	): Promise<ExamSession[]> => {
		const response = await axiosInstance.get<ExamSession[]>(
			`${baseUrl}/student`,
			{ params: { upcomingOnly } }
		)
		return response.data
	},

	// ─── Student: Attempt Management ───

	startExamAttempt: async (sessionId: string): Promise<StudentExamAttempt> => {
		const response = await axiosInstance.post<StudentExamAttempt>(
			`${baseUrl}/${sessionId}/attempts`
		)
		return response.data
	},

	submitExamAttempt: async (
		sessionId: string,
		attemptId: string,
		data: SubmitExamAttemptRequest
	): Promise<StudentExamAttempt> => {
		const response = await axiosInstance.post<StudentExamAttempt>(
			`${baseUrl}/${sessionId}/attempts/${attemptId}/submit`,
			data
		)
		return response.data
	},

	// ─── Attempt Queries ───

	getSessionAttempts: async (
		sessionId: string
	): Promise<StudentExamAttempt[]> => {
		const response = await axiosInstance.get<StudentExamAttempt[]>(
			`${baseUrl}/${sessionId}/attempts`
		)
		return response.data
	},

	getAttemptDetail: async (attemptId: string): Promise<StudentExamAttempt> => {
		const response = await axiosInstance.get<StudentExamAttempt>(
			`${baseUrl}/attempts/${attemptId}`
		)
		return response.data
	},

	/**
	 * Get all of the current student's own attempts for a session (includes scores).
	 */
	getMySessionAttempts: async (
		sessionId: string
	): Promise<StudentExamAttempt[]> => {
		const response = await axiosInstance.get<StudentExamAttempt[]>(
			`${baseUrl}/${sessionId}/my-attempts`
		)
		return response.data
	},

	// ─── Teacher: Results ───

	getSessionResults: async (sessionId: string): Promise<ExamSessionResults> => {
		const response = await axiosInstance.get<ExamSessionResults>(
			`${baseUrl}/${sessionId}/results`
		)
		return response.data
	},
}
