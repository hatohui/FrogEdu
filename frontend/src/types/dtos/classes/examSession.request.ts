export interface CreateExamSessionRequest {
	examId: string
	startTime: string
	endTime: string
	retryTimes: number
	isRetryable: boolean
	shouldShuffleQuestions: boolean
	shouldShuffleAnswers: boolean
	allowPartialScoring: boolean
}

export interface UpdateExamSessionRequest {
	startTime?: string
	endTime?: string
	retryTimes?: number
	isRetryable?: boolean
	shouldShuffleQuestions?: boolean
	shouldShuffleAnswers?: boolean
	allowPartialScoring?: boolean
}

export interface SubmitExamAttemptRequest {
	answers: StudentAnswerSubmission[]
}

export interface StudentAnswerSubmission {
	questionId: string
	selectedAnswerIds: string[]
}
