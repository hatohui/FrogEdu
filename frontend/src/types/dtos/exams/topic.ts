export interface CreateTopicRequest {
	title: string
	description: string
	isCurriculum: boolean
	subjectId: string
}

export interface UpdateTopicRequest {
	title?: string
	description?: string
	isCurriculum?: boolean
}
