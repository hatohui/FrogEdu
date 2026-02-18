export interface CreateClassRequest {
	name: string
	description: string
	grade: string
	maxStudents: number
	bannerUrl?: string
}

export interface JoinClassRequest {
	inviteCode: string
}

export interface AssignExamRequest {
	examId: string
	startDate: string
	dueDate: string
	isMandatory?: boolean
	weight?: number
	retryTimes?: number
	isRetryable?: boolean
	shouldShuffleQuestions?: boolean
	shouldShuffleAnswers?: boolean
	allowPartialScoring?: boolean
}
