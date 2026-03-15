export interface CreateExamSessionRequest {
	examId: string
	startTime: string
	endTime: string
	retryTimes: number
	isRetryable: boolean
	shouldShuffleQuestions: boolean
	shouldShuffleAnswers: boolean
	allowPartialScoring: boolean
	isPractice?: boolean
}

export interface UpdateExamSessionRequest {
	startTime?: string
	endTime?: string
	retryTimes?: number
	isRetryable?: boolean
	shouldShuffleQuestions?: boolean
	shouldShuffleAnswers?: boolean
	allowPartialScoring?: boolean
	isPractice?: boolean
}

export interface SubmitExamAttemptRequest {
	answers: StudentAnswerSubmission[]
}

export interface StudentAnswerSubmission {
	questionId: string
	/** For MC/MA/TF/FillInBlank: the answer IDs or typed text wrapped in array. For Essay: leave empty. */
	selectedAnswerIds: string[]
	/** For Essay type only: the student's free-text response. */
	essayText?: string
}
