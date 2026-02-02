import apiService, { type ApiResponse } from '../api.service'

/**
 * AI Tutor DTOs
 */
export interface ConversationMessage {
	id: string
	conversationId: string
	role: 'user' | 'assistant'
	content: string
	references?: TextbookReference[]
	timestamp: string
}

export interface TextbookReference {
	textbookId: string
	textbookTitle: string
	chapterId: string
	chapterName: string
	pageNumber?: number
	excerpt?: string
	relevanceScore?: number
}

export interface Conversation {
	id: string
	studentId: string
	title: string
	grade?: number
	subject?: string
	currentLesson?: string
	messages: ConversationMessage[]
	createdAt: string
	updatedAt: string
	lastMessageAt?: string
	isArchived: boolean
}

export interface AskQuestionRequest {
	conversationId?: string
	question: string
	context?: {
		grade?: number
		subject?: string
		lesson?: string
	}
}

export interface AskQuestionResponse {
	conversationId: string
	messageId: string
	response: string
	references: TextbookReference[]
}

/**
 * AI Tutor Service
 * Handles student questions, Socratic dialogue, and reference retrieval
 */
class AiTutorService {
	private readonly baseUrl = '/ai'

	/**
	 * Get user's conversations (chat history)
	 */
	async getConversations(
		page = 1,
		pageSize = 20
	): Promise<
		ApiResponse<{
			conversations: Conversation[]
			total: number
			page: number
			pageSize: number
		}>
	> {
		return apiService.get(`${this.baseUrl}/conversations`, {
			page,
			pageSize,
		})
	}

	/**
	 * Get a specific conversation with all messages
	 */
	async getConversation(
		conversationId: string
	): Promise<ApiResponse<Conversation>> {
		return apiService.get<Conversation>(
			`${this.baseUrl}/conversations/${conversationId}`
		)
	}

	/**
	 * Create a new conversation
	 */
	async createConversation(context?: {
		grade?: number
		subject?: string
		lesson?: string
	}): Promise<ApiResponse<Conversation>> {
		return apiService.post<Conversation>(
			`${this.baseUrl}/conversations`,
			context
		)
	}

	/**
	 * Ask a question to the AI Tutor (supports streaming)
	 * This endpoint returns server-sent events for streaming responses
	 */
	async askQuestion(
		request: AskQuestionRequest
	): Promise<ApiResponse<AskQuestionResponse>> {
		return apiService.post<AskQuestionResponse, AskQuestionRequest>(
			`${this.baseUrl}/ask`,
			request
		)
	}

	/**
	 * Stream question response (for real-time chat)
	 * Use this with EventSource for streaming responses
	 */
	getQuestionStreamUrl(
		conversationId: string,
		question: string,
		context?: { grade?: number; subject?: string; lesson?: string }
	): string {
		const params = new URLSearchParams()
		params.append('conversationId', conversationId)
		params.append('question', question)
		if (context?.grade) params.append('grade', context.grade.toString())
		if (context?.subject) params.append('subject', context.subject)
		if (context?.lesson) params.append('lesson', context.lesson)
		return `${this.baseUrl}/ask/stream?${params.toString()}`
	}

	/**
	 * Get textbook references for a topic
	 * RAG: Retrieve relevant content from textbooks
	 */
	async getReferences(
		query: string,
		context?: {
			grade?: number
			subject?: string
			textbookId?: string
		}
	): Promise<ApiResponse<TextbookReference[]>> {
		return apiService.get<TextbookReference[]>(`${this.baseUrl}/references`, {
			q: query,
			...context,
		})
	}

	/**
	 * Get a hint for the current question (Socratic method)
	 * Instead of direct answer, provides guiding questions
	 */
	async getHint(
		conversationId: string,
		questionContent: string
	): Promise<
		ApiResponse<{
			hint: string
			guidingQuestions: string[]
		}>
	> {
		return apiService.post(`${this.baseUrl}/hint`, {
			conversationId,
			questionContent,
		})
	}

	/**
	 * Archive a conversation
	 */
	async archiveConversation(
		conversationId: string
	): Promise<ApiResponse<void>> {
		return apiService.patch<void>(
			`${this.baseUrl}/conversations/${conversationId}`,
			{ isArchived: true }
		)
	}

	/**
	 * Delete a conversation
	 */
	async deleteConversation(conversationId: string): Promise<ApiResponse<void>> {
		return apiService.delete<void>(
			`${this.baseUrl}/conversations/${conversationId}`
		)
	}

	/**
	 * Rate a response (for feedback and improvement)
	 */
	async rateResponse(
		messageId: string,
		rating: 1 | 2 | 3 | 4 | 5,
		feedback?: string
	): Promise<ApiResponse<void>> {
		return apiService.post<void>(`${this.baseUrl}/feedback`, {
			messageId,
			rating,
			feedback,
		})
	}

	/**
	 * Get conversation insights (for teachers)
	 * Shows what topics student is struggling with
	 */
	async getConversationInsights(
		studentId?: string,
		timeframe: '7d' | '30d' | '90d' = '30d'
	): Promise<
		ApiResponse<{
			topicsCovered: string[]
			strugglingTopics: string[]
			averageResponseTime: number
			totalInteractions: number
		}>
	> {
		return apiService.get(`${this.baseUrl}/insights`, {
			studentId,
			timeframe,
		})
	}
}

export default new AiTutorService()
