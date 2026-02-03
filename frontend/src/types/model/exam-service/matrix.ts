import type { CognitiveLevel } from './enums'

export interface MatrixTopicDto {
	topicId: string
	cognitiveLevel: CognitiveLevel
	quantity: number
}

export interface Matrix {
	id: string
	examId: string
	matrixTopics: MatrixTopicDto[]
	totalQuestionCount: number
	createdAt: string
}
