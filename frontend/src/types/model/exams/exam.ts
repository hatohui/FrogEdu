export interface Exam {
	id: string
	title: string
	duration: number
	passScore: number
	maxAttempts: number
	startTime: string
	endTime: string
	topicId: string
	isDraft: boolean
	isActive: boolean
	accessCode: string | null
	questionCount: number
	createdAt: string
	updatedAt: string | null
}
