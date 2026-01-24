import apiService, {
	type ApiResponse,
	type PaginatedResponse,
} from './api.service'

/**
 * Exam DTOs
 */
export interface ExamMatrixConfig {
	chapters: ChapterConfig[]
	totalQuestions: number
}

export interface ChapterConfig {
	chapterId: string
	chapterName: string
	easy: number
	medium: number
	hard: number
}

export interface Question {
	id: string
	content: string
	chapterId: string
	difficulty: 'Easy' | 'Medium' | 'Hard'
	type: 'MCQ' | 'Essay'
	options?: string[]
	correctAnswer?: string
	bloomsLevel?: string
	pageReference?: number
}

export interface Exam {
	id: string
	title: string
	description?: string
	status: 'Draft' | 'Published' | 'Archived'
	matrixConfig: ExamMatrixConfig
	questions: Question[]
	createdAt: string
	updatedAt: string
	publishedAt?: string
	pdfUrl?: string
}

export interface CreateExamRequest {
	title: string
	description?: string
	matrixConfig: ExamMatrixConfig
}

export interface GenerateExamRequest {
	title: string
	description?: string
	matrixConfig: ExamMatrixConfig
}

/**
 * Assessment Service
 * Handles exam creation, generation, and management
 */
class AssessmentService {
	private readonly baseUrl = '/assessments'

	/**
	 * List all exams for the teacher
	 */
	async getExams(
		page = 1,
		pageSize = 10
	): Promise<ApiResponse<PaginatedResponse<Exam>>> {
		return apiService.get<PaginatedResponse<Exam>>(`${this.baseUrl}/exams`, {
			page,
			pageSize,
		})
	}

	/**
	 * Get a specific exam by ID
	 */
	async getExam(examId: string): Promise<ApiResponse<Exam>> {
		return apiService.get<Exam>(`${this.baseUrl}/exams/${examId}`)
	}

	/**
	 * Create a draft exam
	 */
	async createExam(request: CreateExamRequest): Promise<ApiResponse<Exam>> {
		return apiService.post<Exam, CreateExamRequest>(
			`${this.baseUrl}/exams`,
			request
		)
	}

	/**
	 * Generate exam with AI selection (auto-populate questions)
	 */
	async generateExam(request: GenerateExamRequest): Promise<ApiResponse<Exam>> {
		return apiService.post<Exam, GenerateExamRequest>(
			`${this.baseUrl}/exams/generate`,
			request
		)
	}

	/**
	 * Update exam (change title, description, matrix config)
	 */
	async updateExam(
		examId: string,
		updates: Partial<Exam>
	): Promise<ApiResponse<Exam>> {
		return apiService.put<Exam, Partial<Exam>>(
			`${this.baseUrl}/exams/${examId}`,
			updates
		)
	}

	/**
	 * Replace a question in the exam
	 */
	async replaceQuestion(
		examId: string,
		questionIndex: number,
		newQuestion: Question
	): Promise<ApiResponse<Exam>> {
		return apiService.patch<Exam>(`${this.baseUrl}/exams/${examId}/questions`, {
			questionIndex,
			newQuestion,
		})
	}

	/**
	 * Publish exam (finalize and make it unavailable for editing)
	 */
	async publishExam(examId: string): Promise<ApiResponse<Exam>> {
		return apiService.post<Exam>(`${this.baseUrl}/exams/${examId}/publish`)
	}

	/**
	 * Get download URL for exam PDF
	 * Returns presigned S3 URL
	 */
	async getExamDownloadUrl(
		examId: string
	): Promise<ApiResponse<{ url: string }>> {
		return apiService.get<{ url: string }>(
			`${this.baseUrl}/exams/${examId}/download`
		)
	}

	/**
	 * Delete exam
	 */
	async deleteExam(examId: string): Promise<ApiResponse<void>> {
		return apiService.delete<void>(`${this.baseUrl}/exams/${examId}`)
	}

	/**
	 * Get question bank (all available questions)
	 */
	async getQuestionBank(
		chapterId?: string,
		difficulty?: 'Easy' | 'Medium' | 'Hard',
		page = 1,
		pageSize = 20
	): Promise<ApiResponse<PaginatedResponse<Question>>> {
		return apiService.get<PaginatedResponse<Question>>(
			`${this.baseUrl}/questions`,
			{
				chapterId,
				difficulty,
				page,
				pageSize,
			}
		)
	}

	/**
	 * Get chapters for matrix builder
	 */
	async getChapters(
		textbookId?: string
	): Promise<
		ApiResponse<Array<{ id: string; name: string; totalQuestions: number }>>
	> {
		return apiService.get(
			`${this.baseUrl}/chapters`,
			textbookId ? { textbookId } : undefined
		)
	}
}

export default new AssessmentService()
