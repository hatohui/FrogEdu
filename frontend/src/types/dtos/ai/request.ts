import type {
	CognitiveLevel,
	QuestionType,
} from '@/types/model/exam-service/enums'

/**
 * Topic configuration for AI generation matrix request.
 * questionType: null means the AI will randomize the type per question.
 */
export interface AIMatrixTopicConfig {
	topicId: string
	topicName: string
	cognitiveLevel: CognitiveLevel
	quantity: number
	/** Specific question type to generate. null/undefined = randomize. */
	questionType?: QuestionType | null
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
