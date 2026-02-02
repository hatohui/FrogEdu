export interface Subject {
	id: string
	subjectCode: string
	name: string
	description: string
	imageUrl: string
	grade: number
}

export interface Topic {
	id: string
	title: string
	description: string
	isCurriculum: boolean
	subjectId: string
}
