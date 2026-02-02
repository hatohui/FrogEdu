export interface Exam {
	id: string
	name: string
	description: string
	topicId: string
	subjectId: string
	grade: number
	isDraft: boolean
	isActive: boolean
	createdAt: string
	updatedAt: string | null
}
