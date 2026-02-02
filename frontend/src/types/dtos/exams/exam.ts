export interface CreateExamRequest {
	title: string
	duration: number
	passScore: number
	maxAttempts: number
	startTime: string
	endTime: string
	topicId: string
	shouldShuffleQuestions: boolean
	shouldShuffleAnswerOptions: boolean
}
