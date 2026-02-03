import type {
	GenerateQuestionsRequest,
	GenerateQuestionsResponse,
	GenerateSingleQuestionRequest,
	AIGeneratedQuestion,
} from '@/types/model/ai-service'
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
		const response = await axiosInstance.post<GenerateQuestionsResponse>(
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
			questions: response.data.questions.map(this.mapQuestion),
			totalCount: response.data.totalCount,
		}
	}

	/**
	 * Generate a single question
	 * Requires Pro subscription
	 */
	async generateSingleQuestion(
		request: GenerateSingleQuestionRequest
	): Promise<AIGeneratedQuestion> {
		const response = await axiosInstance.post<AIGeneratedQuestion>(
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
	 * Map snake_case API response to camelCase
	 */
	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	private mapQuestion(q: any): AIGeneratedQuestion {
		return {
			content: q.content as string,
			questionType: q.question_type as number,
			cognitiveLevel: q.cognitive_level as number,
			point: (q.point as number) ?? 1,
			mediaUrl: q.media_url as string | undefined,
			topicId: q.topic_id as string | undefined,
			answers: ((q.answers as any[]) ?? []).map((a: any) => ({
				content: a.content as string,
				isCorrect: a.is_correct as boolean,
				explanation: a.explanation as string | undefined,
				point: a.point as number | undefined,
			})),
		}
	}
}

export const aiService = new AIService()
export default aiService
