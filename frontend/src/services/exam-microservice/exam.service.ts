import type {
	Subject,
	Exam,
	CognitiveLevel,
	Question,
	Topic,
	Matrix,
} from '@/types/model/exam-service'
import apiService, { type ApiResponse } from '../api.service'

import type {
	CreateExamRequest,
	CreateMatrixRequest,
	UpdateMatrixRequest,
	AttachMatrixRequest,
	CreateQuestionRequest,
	CreateSubjectRequest,
	UpdateSubjectRequest,
	CreateTopicRequest,
	UpdateTopicRequest,
} from '@/types/dtos/exams'

class AssessmentService {
	private readonly baseUrl = '/exams'

	async getSubjects(
		grade?: number
	): Promise<ApiResponse<{ subjects: Subject[] }>> {
		return apiService.get<{ subjects: Subject[] }>(`${this.baseUrl}/subjects`, {
			...(grade && { grade }),
		})
	}

	async createSubject(
		request: CreateSubjectRequest
	): Promise<ApiResponse<Subject>> {
		return apiService.post<Subject, CreateSubjectRequest>(
			`${this.baseUrl}/subjects`,
			request
		)
	}

	async updateSubject(
		subjectId: string,
		request: UpdateSubjectRequest
	): Promise<ApiResponse<Subject>> {
		return apiService.put<Subject, UpdateSubjectRequest>(
			`${this.baseUrl}/subjects/${subjectId}`,
			request
		)
	}

	async deleteSubject(subjectId: string): Promise<ApiResponse<void>> {
		return apiService.delete(`${this.baseUrl}/subjects/${subjectId}`)
	}

	async getTopics(
		subjectId: string
	): Promise<ApiResponse<{ topics: Topic[] }>> {
		return apiService.get<{ topics: Topic[] }>(
			`${this.baseUrl}/subjects/${subjectId}/topics`
		)
	}

	async createTopic(request: CreateTopicRequest): Promise<ApiResponse<Topic>> {
		return apiService.post<Topic, CreateTopicRequest>(
			`${this.baseUrl}/subjects/${request.subjectId}/topics`,
			request
		)
	}

	async updateTopic(
		subjectId: string,
		topicId: string,
		request: UpdateTopicRequest
	): Promise<ApiResponse<Topic>> {
		return apiService.put<Topic, UpdateTopicRequest>(
			`${this.baseUrl}/subjects/${subjectId}/topics/${topicId}`,
			request
		)
	}

	async deleteTopic(
		subjectId: string,
		topicId: string
	): Promise<ApiResponse<void>> {
		return apiService.delete(
			`${this.baseUrl}/subjects/${subjectId}/topics/${topicId}`
		)
	}

	async getExams(isDraft?: boolean): Promise<ApiResponse<{ exams: Exam[] }>> {
		return apiService.get<{ exams: Exam[] }>(`${this.baseUrl}/exams`, {
			...(isDraft !== undefined && { isDraft }),
		})
	}

	async getExamById(examId: string): Promise<ApiResponse<Exam>> {
		return apiService.get<Exam>(`${this.baseUrl}/exams/${examId}`)
	}

	async createExam(request: CreateExamRequest): Promise<ApiResponse<Exam>> {
		return apiService.post<Exam, CreateExamRequest>(
			`${this.baseUrl}/exams`,
			request
		)
	}

	async updateExam(
		examId: string,
		request: Partial<CreateExamRequest>
	): Promise<ApiResponse<Exam>> {
		return apiService.put<Exam, Partial<CreateExamRequest>>(
			`${this.baseUrl}/exams/${examId}`,
			request
		)
	}

	async publishExam(examId: string): Promise<ApiResponse<Exam>> {
		return apiService.post<Exam>(`${this.baseUrl}/exams/${examId}/publish`)
	}

	async deleteExam(examId: string): Promise<ApiResponse<void>> {
		return apiService.delete(`${this.baseUrl}/exams/${examId}`)
	}

	// ========== Matrices ==========
	async getMatrices(): Promise<ApiResponse<{ matrices: Matrix[] }>> {
		return apiService.get<{ matrices: Matrix[] }>(`${this.baseUrl}/matrices`)
	}

	async getMatrixById(matrixId: string): Promise<ApiResponse<Matrix>> {
		return apiService.get<Matrix>(`${this.baseUrl}/matrices/${matrixId}`)
	}

	async createMatrix(request: CreateMatrixRequest): Promise<
		ApiResponse<{
			id: string
			name: string
			subjectId: string
			grade: number
			totalQuestions: number
			createdAt: string
		}>
	> {
		return apiService.post(`${this.baseUrl}/matrices`, request)
	}

	async updateMatrix(
		matrixId: string,
		request: UpdateMatrixRequest
	): Promise<ApiResponse<void>> {
		return apiService.put(`${this.baseUrl}/matrices/${matrixId}`, request)
	}

	async deleteMatrix(matrixId: string): Promise<ApiResponse<void>> {
		return apiService.delete(`${this.baseUrl}/matrices/${matrixId}`)
	}

	async getMatrixByExamId(examId: string): Promise<ApiResponse<Matrix>> {
		return apiService.get<Matrix>(`${this.baseUrl}/matrices/exam/${examId}`)
	}

	async attachMatrixToExam(
		examId: string,
		request: AttachMatrixRequest
	): Promise<ApiResponse<void>> {
		return apiService.post(`${this.baseUrl}/exams/${examId}/matrix`, request)
	}

	async detachMatrixFromExam(examId: string): Promise<ApiResponse<void>> {
		return apiService.delete(`${this.baseUrl}/exams/${examId}/matrix`)
	}

	// ========== Questions ==========
	async getQuestions(params?: {
		topicId?: string
		cognitiveLevel?: CognitiveLevel
		isPublic?: boolean
	}): Promise<ApiResponse<{ questions: Question[] }>> {
		return apiService.get<{ questions: Question[] }>(
			`${this.baseUrl}/questions`,
			params
		)
	}

	async getQuestionById(questionId: string): Promise<ApiResponse<Question>> {
		return apiService.get<Question>(`${this.baseUrl}/questions/${questionId}`)
	}

	async createQuestion(
		request: CreateQuestionRequest
	): Promise<ApiResponse<Question>> {
		return apiService.post<Question, CreateQuestionRequest>(
			`${this.baseUrl}/questions`,
			request
		)
	}

	async updateQuestion(
		questionId: string,
		request: Partial<CreateQuestionRequest>
	): Promise<ApiResponse<Question>> {
		return apiService.put<Question, Partial<CreateQuestionRequest>>(
			`${this.baseUrl}/questions/${questionId}`,
			request
		)
	}

	async deleteQuestion(questionId: string): Promise<ApiResponse<void>> {
		return apiService.delete(`${this.baseUrl}/questions/${questionId}`)
	}

	// ========== Exam Questions ==========
	async getExamQuestions(
		examId: string
	): Promise<ApiResponse<{ questions: Question[] }>> {
		return apiService.get<{ questions: Question[] }>(
			`${this.baseUrl}/exams/${examId}/questions`
		)
	}

	async addQuestionsToExam(
		examId: string,
		questionIds: string[]
	): Promise<ApiResponse<void>> {
		return apiService.post(`${this.baseUrl}/exams/${examId}/questions`, {
			questionIds,
		})
	}

	async removeQuestionFromExam(
		examId: string,
		questionId: string
	): Promise<ApiResponse<void>> {
		return apiService.delete(
			`${this.baseUrl}/exams/${examId}/questions/${questionId}`
		)
	}

	// ========== Preview & Export ==========
	async getExamPreview(examId: string): Promise<
		ApiResponse<{
			id: string
			name: string
			description: string
			subjectName: string
			grade: number
			questionCount: number
			totalPoints: number
			createdAt: string
			questions: Array<{
				questionNumber: number
				content: string
				point: number
				type: string
				cognitiveLevel: string
				mediaUrl?: string
				answers: Array<{
					label: string
					content: string
					isCorrect: boolean
				}>
			}>
		}>
	> {
		return apiService.get(`${this.baseUrl}/exams/${examId}/preview`)
	}

	async exportExamToPdf(examId: string): Promise<Blob> {
		return apiService.getBlob(`${this.baseUrl}/exams/${examId}/export/pdf`)
	}

	async exportExamToExcel(examId: string): Promise<Blob> {
		return apiService.getBlob(`${this.baseUrl}/exams/${examId}/export/excel`)
	}

	// ========== Matrix Export ==========
	async exportMatrixToPdf(matrixId: string): Promise<Blob> {
		return apiService.getBlob(`${this.baseUrl}/matrices/${matrixId}/export/pdf`)
	}

	async exportMatrixToExcel(matrixId: string): Promise<Blob> {
		return apiService.getBlob(
			`${this.baseUrl}/matrices/${matrixId}/export/excel`
		)
	}
}

export default new AssessmentService()
