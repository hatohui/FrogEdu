export interface Exam {
	id: string
	name: string
	description: string
	subjectId: string
	grade: number
	isDraft: boolean
	isActive: boolean
	matrixId: string | null
	questionCount: number
	createdAt: string
	updatedAt: string | null
}
