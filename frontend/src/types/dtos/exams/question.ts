import {
	CognitiveLevel,
	QuestionType,
	QuestionSource,
} from '@/types/model/exam-service'

export interface CreateQuestionRequest {
	content: string
	point: number
	type: QuestionType
	cognitiveLevel: CognitiveLevel
	source: QuestionSource
	topicId: string
	mediaUrl?: string
	isPublic: boolean
	answers: Array<{
		content: string
		isCorrect: boolean
		point: number
		explanation?: string
	}>
}
