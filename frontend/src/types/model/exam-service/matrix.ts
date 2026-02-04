import type { CognitiveLevel } from './enums'

export interface MatrixTopicDto {
	topicId: string
	cognitiveLevel: CognitiveLevel
	quantity: number
}

export interface Matrix {
	id: string
	name: string
	description: string | null
	subjectId: string
	grade: number
	matrixTopics: MatrixTopicDto[]
	totalQuestionCount: number
	createdAt: string
}
