import type {
	GenerateQuestionsRequest,
	GenerateQuestionsResponse,
	GenerateSingleQuestionRequest,
	AIGeneratedQuestion,
} from '@/types/model/ai-service'
import type {
	AIQuestionApiResponse,
	AIGenerateQuestionsApiResponse,
} from '@/types/dtos/ai'
import axiosInstance from './axios'

class AIService {
	private readonly baseUrl = '/ai'

	/**
	 * Generate multiple questions based on a matrix configuration
	 * Requires Pro subscription
	 */
	async generateQuestions(
		request: GenerateQuestionsRequest
	): Promise<GenerateQuestionsResponse> {
		const response = await axiosInstance.post<AIGenerateQuestionsApiResponse>(
			`${this.baseUrl}/questions/generate`,
			{
				subject: request.subject,
				grade: request.grade,
				matrix_topics: request.matrixTopics.map(topic => ({
					topic_id: topic.topicId,
					topic_name: topic.topicName,
					cognitive_level: topic.cognitiveLevel,
					quantity: topic.quantity,
				})),
				language: request.language ?? 'vi',
			}
		)
		return {
			questions: response.data.questions.map(q => this.mapQuestion(q)),
			totalCount: response.data.total_count,
		}
	}

	/**
	 * Generate a single question
	 * Requires Pro subscription
	 */
	async generateSingleQuestion(
		request: GenerateSingleQuestionRequest
	): Promise<AIGeneratedQuestion> {
		const response = await axiosInstance.post<AIQuestionApiResponse>(
			`${this.baseUrl}/questions/generate-single`,
			{
				subject: request.subject,
				grade: request.grade,
				topic_name: request.topicName,
				cognitive_level: request.cognitiveLevel,
				question_type: request.questionType,
				language: request.language ?? 'vi',
				topic_description: request.topicDescription ?? '',
			}
		)
		return this.mapQuestion(response.data)
	}

	/**
	 * Check AI service health
	 */
	async healthCheck(): Promise<{
		status: string
		serviceName: string
		geminiConnected: boolean
	}> {
		const response = await axiosInstance.get<{
			status: string
			service_name: string
			gemini_connected: boolean
		}>(`${this.baseUrl}/health`)
		return {
			status: response.data.status,
			serviceName: response.data.service_name,
			geminiConnected: response.data.gemini_connected,
		}
	}

	/**
	 * Map snake_case API response to camelCase frontend type
	 */
	private mapQuestion(q: AIQuestionApiResponse): AIGeneratedQuestion {
		return {
			content: q.content,
			questionType: q.question_type,
			cognitiveLevel: q.cognitive_level,
			point: q.point ?? 1,
			mediaUrl: q.media_url,
			topicId: q.topic_id,
			answers: (q.answers ?? []).map(a => ({
				content: a.content,
				isCorrect: a.is_correct,
				explanation: a.explanation,
				point: a.point,
			})),
		}
	}
}

export const aiService = new AIService()
export default aiService
