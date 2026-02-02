export interface CreateSubjectRequest {
	subjectCode: string
	name: string
	description: string
	imageUrl: string
	grade: number
}

export interface UpdateSubjectRequest {
	subjectCode?: string
	name?: string
	description?: string
	imageUrl?: string
	grade?: number
}
