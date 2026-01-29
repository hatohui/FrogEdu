import apiService, {
	type ApiResponse,
	type PaginatedResponse,
} from './api.service'

/**
 * Content DTOs
 */
export interface Textbook {
	id: string
	title: string
	grade: number
	subject: string
	publisher?: string
	coverImageUrl?: string
	totalPages: number
	totalChapters: number
	isbn?: string
	createdAt: string
	updatedAt: string
}

export interface Chapter {
	id: string
	textbookId: string
	title: string
	chapterNumber: number
	description?: string
	totalPages: number
	totalQuestions: number
	createdAt: string
	updatedAt: string
}

export interface TextbookPage {
	id: string
	chapterId: string
	pageNumber: number
	content: string
	imageUrl?: string
	questionsOnPage?: Array<{ id: string; type: string }>
}

export interface TextbookAsset {
	id: string
	textbookId: string
	name: string
	type: 'PDF' | 'Image' | 'Video' | 'Document'
	url: string
	uploadedAt: string
	uploadedBy: string
}

/**
 * Content Service
 * Handles textbook browsing, chapter navigation, and assets
 */
class ContentService {
	private readonly baseUrl = '/exams'

	/**
	 * List all textbooks with filtering and pagination
	 */
	async getTextbooks(
		filters?: {
			grade?: number
			subject?: string
		},
		page = 1,
		pageSize = 12
	): Promise<ApiResponse<PaginatedResponse<Textbook>>> {
		return apiService.get<PaginatedResponse<Textbook>>(
			`${this.baseUrl}/textbooks`,
			{
				...filters,
				page,
				pageSize,
			}
		)
	}

	/**
	 * Get a specific textbook
	 */
	async getTextbook(textbookId: string): Promise<ApiResponse<Textbook>> {
		return apiService.get<Textbook>(`${this.baseUrl}/textbooks/${textbookId}`)
	}

	/**
	 * Get all chapters for a textbook
	 */
	async getChapters(
		textbookId: string,
		page = 1,
		pageSize = 50
	): Promise<ApiResponse<PaginatedResponse<Chapter>>> {
		return apiService.get<PaginatedResponse<Chapter>>(
			`${this.baseUrl}/textbooks/${textbookId}/chapters`,
			{
				page,
				pageSize,
			}
		)
	}

	/**
	 * Get a specific chapter
	 */
	async getChapter(
		textbookId: string,
		chapterId: string
	): Promise<ApiResponse<Chapter>> {
		return apiService.get<Chapter>(
			`${this.baseUrl}/textbooks/${textbookId}/chapters/${chapterId}`
		)
	}

	/**
	 * Get pages for a chapter
	 */
	async getChapterPages(
		textbookId: string,
		chapterId: string,
		page = 1,
		pageSize = 20
	): Promise<ApiResponse<PaginatedResponse<TextbookPage>>> {
		return apiService.get<PaginatedResponse<TextbookPage>>(
			`${this.baseUrl}/textbooks/${textbookId}/chapters/${chapterId}/pages`,
			{
				page,
				pageSize,
			}
		)
	}

	/**
	 * Get a specific page (with presigned URL for images)
	 */
	async getPage(
		textbookId: string,
		chapterId: string,
		pageNumber: number
	): Promise<ApiResponse<TextbookPage>> {
		return apiService.get<TextbookPage>(
			`${this.baseUrl}/textbooks/${textbookId}/chapters/${chapterId}/pages/${pageNumber}`
		)
	}

	/**
	 * Get presigned URL for page image (for rendering)
	 */
	async getPageImageUrl(
		textbookId: string,
		chapterId: string,
		pageNumber: number
	): Promise<ApiResponse<{ url: string }>> {
		return apiService.get<{ url: string }>(
			`${this.baseUrl}/textbooks/${textbookId}/chapters/${chapterId}/pages/${pageNumber}/image`
		)
	}

	/**
	 * Get all assets (PDFs, images, etc.) for a textbook
	 */
	async getTextbookAssets(
		textbookId: string,
		page = 1,
		pageSize = 20
	): Promise<ApiResponse<PaginatedResponse<TextbookAsset>>> {
		return apiService.get<PaginatedResponse<TextbookAsset>>(
			`${this.baseUrl}/textbooks/${textbookId}/assets`,
			{
				page,
				pageSize,
			}
		)
	}

	/**
	 * Upload new asset to textbook
	 */
	async uploadAsset(
		textbookId: string,
		file: File,
		name: string
	): Promise<ApiResponse<TextbookAsset>> {
		const formData = new FormData()
		formData.append('file', file)
		formData.append('name', name)

		// Note: This requires special handling due to FormData
		// Would need to modify axios instance to handle file uploads
		return apiService.post<TextbookAsset>(
			`${this.baseUrl}/textbooks/${textbookId}/assets`,
			formData
		)
	}

	/**
	 * Search textbooks and content
	 */
	async search(
		query: string,
		filters?: {
			grade?: number
			subject?: string
			type?: 'textbook' | 'chapter' | 'page'
		},
		page = 1,
		pageSize = 20
	): Promise<ApiResponse<PaginatedResponse<Textbook | Chapter>>> {
		return apiService.get(`${this.baseUrl}/search`, {
			q: query,
			...filters,
			page,
			pageSize,
		})
	}

	/**
	 * Get available grades and subjects
	 */
	async getFilters(): Promise<
		ApiResponse<{
			grades: number[]
			subjects: string[]
		}>
	> {
		return apiService.get(`${this.baseUrl}/filters`)
	}
}

export default new ContentService()
