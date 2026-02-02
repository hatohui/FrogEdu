import type { CognitiveLevel } from './enums'

export interface MatrixTopicDto {
	topicId: string
	cognitiveLevel: CognitiveLevel
	quantity: number
}
