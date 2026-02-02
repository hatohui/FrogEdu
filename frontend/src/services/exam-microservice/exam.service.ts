import type {
	Subject,
	Exam,
	CognitiveLevel,
	Question,
	Topic,
} from '@/types/model/exam-service'
import apiService, { type ApiResponse } from '../api.service'

import type {
	CreateExamRequest,
	CreateMatrixRequest,
	CreateQuestionRequest,
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

	async getTopics(
		subjectId: string
	): Promise<ApiResponse<{ topics: Topic[] }>> {
		return apiService.get<{ topics: Topic[] }>(
			`${this.baseUrl}/subjects/${subjectId}/topics`
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
	async createMatrix(request: CreateMatrixRequest): Promise<
		ApiResponse<{
			id: string
			examId: string
			totalQuestions: number
			createdAt: string
		}>
	> {
		return apiService.post(`${this.baseUrl}/matrices`, request)
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
}

export default new AssessmentService()
