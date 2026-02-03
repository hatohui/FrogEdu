/**
 * API response types from AI service (snake_case)
 */

/**
 * Answer as returned from AI API
 */
export interface AIAnswerApiResponse {
	content: string
	is_correct: boolean
	explanation?: string
	point?: number
}

/**
 * Question as returned from AI API
 */
export interface AIQuestionApiResponse {
	content: string
	question_type: number
	cognitive_level: number
	point?: number
	media_url?: string
	answers: AIAnswerApiResponse[]
	topic_id?: string
}

/**
 * Batch generation response from AI API
 */
export interface AIGenerateQuestionsApiResponse {
	questions: AIQuestionApiResponse[]
	total_count: number
}
