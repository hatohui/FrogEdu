import apiService, { type ApiResponse } from './api.service'
import type {
	CognitiveLevel,
	Subject,
	Topic,
	Question,
	Exam,
} from '@/types/model/exams'
import type {
	CreateExamRequest,
	CreateMatrixRequest,
	CreateQuestionRequest,
} from '@/types/dtos/exams'

class AssessmentService {
	private readonly baseUrl = '/api/exams'

	// ========== Subjects ==========
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

	// ========== Exams ==========
	async getExams(isDraft?: boolean): Promise<ApiResponse<{ exams: Exam[] }>> {
		return apiService.get<{ exams: Exam[] }>(`${this.baseUrl}/exams`, {
			...(isDraft !== undefined && { isDraft }),
		})
	}

	async createExam(request: CreateExamRequest): Promise<ApiResponse<Exam>> {
		return apiService.post<Exam, CreateExamRequest>(
			`${this.baseUrl}/exams`,
			request
		)
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

	async createQuestion(
		request: CreateQuestionRequest
	): Promise<ApiResponse<Question>> {
		return apiService.post<Question, CreateQuestionRequest>(
			`${this.baseUrl}/questions`,
			request
		)
	}
}

export default new AssessmentService()
