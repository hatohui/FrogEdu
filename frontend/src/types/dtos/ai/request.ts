import type {
	CognitiveLevel,
	QuestionType,
} from '@/types/model/exam-service/enums'

/**
 * Topic configuration for AI generation matrix request
 */
export interface AIMatrixTopicConfig {
	topicId: string
	topicName: string
	cognitiveLevel: CognitiveLevel
	quantity: number
}

/**
 * Request to generate multiple questions based on a matrix
 */
export interface GenerateQuestionsRequest {
	subject: string
	grade: number
	matrixTopics: AIMatrixTopicConfig[]
	language?: 'vi' | 'en'
}

/**
 * Request to generate a single question
 */
export interface GenerateSingleQuestionRequest {
	subject: string
	grade: number
	topicName: string
	topicId?: string
	cognitiveLevel: CognitiveLevel
	questionType: QuestionType
	language?: 'vi' | 'en'
	topicDescription?: string
}
