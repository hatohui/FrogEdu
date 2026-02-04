import type { CognitiveLevel, QuestionType, QuestionSource } from './enums'

export interface Answer {
	id: string
	content: string
	isCorrect: boolean
	point: number
	explanation?: string
}

export interface Question {
	id: string
	content: string
	point: number
	type: QuestionType
	cognitiveLevel: CognitiveLevel
	source: QuestionSource
	topicId: string
	mediaUrl?: string
	isPublic: boolean
	answerCount?: number
	answers?: Answer[]
	createdAt: string
}
