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
import { CognitiveLevel, QuestionType } from '@/types/model/exam-service/enums'
import axiosInstance from './axios'

/**
 * Mapping from frontend QuestionType enum to AI backend string format
 */
const QUESTION_TYPE_TO_API: Record<QuestionType, string> = {
	[QuestionType.MultipleChoice]: 'select', // Single choice
	[QuestionType.MultipleAnswer]: 'multiple_choice', // Multiple correct answers
	[QuestionType.TrueFalse]: 'true_false',
	[QuestionType.Essay]: 'essay',
	[QuestionType.FillInTheBlank]: 'fill_in_blank',
}

/**
 * Mapping from AI backend string format to frontend QuestionType enum
 */
const API_TO_QUESTION_TYPE: Record<string, QuestionType> = {
	select: QuestionType.MultipleChoice,
	multiple_choice: QuestionType.MultipleAnswer,
	true_false: QuestionType.TrueFalse,
	essay: QuestionType.Essay,
	fill_in_blank: QuestionType.FillInTheBlank,
	short_answer: QuestionType.FillInTheBlank,
}

class AIService {
	private readonly baseUrl = '/ai'

	/**
	 * Map frontend numeric CognitiveLevel enum to API string format
	 */
	private mapCognitiveLevel(level: CognitiveLevel): string {
		switch (level) {
			case CognitiveLevel.Remember:
				return 'remember'
			case CognitiveLevel.Understand:
				return 'understand'
			case CognitiveLevel.Apply:
				return 'apply'
			case CognitiveLevel.Analyze:
				return 'analyze'
			default:
				return 'remember'
		}
	}

	/**
	 * Map frontend numeric QuestionType enum to API string format
	 */
	private mapQuestionType(type: QuestionType): string {
		return QUESTION_TYPE_TO_API[type] ?? 'select'
	}

	/**
	 * Map API string format back to frontend numeric enum
	 */
	private mapCognitiveLevelFromApi(level: string | number): CognitiveLevel {
		if (typeof level === 'number') {
			return level as CognitiveLevel
		}
		switch (level.toLowerCase()) {
			case 'remember':
				return CognitiveLevel.Remember
			case 'understand':
				return CognitiveLevel.Understand
			case 'apply':
				return CognitiveLevel.Apply
			case 'analyze':
				return CognitiveLevel.Analyze
			default:
				return CognitiveLevel.Remember
		}
	}

	/**
	 * Map API string format back to frontend numeric enum
	 */
	private mapQuestionTypeFromApi(type: string | number): QuestionType {
		if (typeof type === 'number') {
			return type as QuestionType
		}
		return (
			API_TO_QUESTION_TYPE[type.toLowerCase()] ?? QuestionType.MultipleChoice
		)
	}

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
					cognitive_level: this.mapCognitiveLevel(topic.cognitiveLevel),
					quantity: topic.quantity,
					// Only send question_type if explicitly set (null/undefined = randomize on backend)
					...(topic.questionType != null && {
						question_type: this.mapQuestionType(topic.questionType),
					}),
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
				topic_id: request.topicId,
				cognitive_level: this.mapCognitiveLevel(request.cognitiveLevel),
				question_type: this.mapQuestionType(request.questionType),
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
			questionType: this.mapQuestionTypeFromApi(String(q.question_type)),
			cognitiveLevel: this.mapCognitiveLevelFromApi(String(q.cognitive_level)),
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
