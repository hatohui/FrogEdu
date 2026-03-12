export enum AttemptStatus {
	InProgress = 'InProgress',
	Submitted = 'Submitted',
	Graded = 'Graded',
	TimedOut = 'TimedOut',
}

export interface ExamSession {
	id: string
	classId: string
	examId: string
	startTime: string
	endTime: string
	retryTimes: number
	isRetryable: boolean
	isActive: boolean
	shouldShuffleQuestions: boolean
	shouldShuffleAnswers: boolean
	allowPartialScoring: boolean
	isCurrentlyActive: boolean
	isUpcoming: boolean
	hasEnded: boolean
	attemptCount: number
	createdAt: string
	updatedAt: string | null
}

export interface StudentExamAttempt {
	id: string
	examSessionId: string
	studentId: string
	startedAt: string
	submittedAt: string | null
	score: number
	totalPoints: number
	attemptNumber: number
	status: AttemptStatus
	scorePercentage: number
	answers: StudentAnswer[]
}

export interface StudentAnswer {
	id: string
	attemptId: string
	questionId: string
	selectedAnswerIds: string
	score: number
	isCorrect: boolean
	isPartiallyCorrect: boolean
}

export interface ExamSessionResults {
	sessionId: string
	examId: string
	classId: string
	totalAttempts: number
	averageScore: number
	averagePercentage: number
	highestScore: number
	lowestScore: number
	attempts: AttemptSummary[]
}

export interface AttemptSummary {
	attemptId: string
	studentId: string
	studentName: string
	score: number
	totalPoints: number
	scorePercentage: number
	attemptNumber: number
	status: AttemptStatus
	submittedAt: string | null
}

/** Full review of an attempt including question content and correct answers */
export interface AttemptReview {
	id: string
	examSessionId: string
	studentId: string
	examName: string
	startedAt: string
	submittedAt: string | null
	score: number
	totalPoints: number
	scorePercentage: number
	attemptNumber: number
	status: AttemptStatus
	questions: QuestionReview[]
}

export interface QuestionReview {
	questionId: string
	content: string
	type: string
	point: number
	imageUrl: string | null
	answers: AnswerReview[]
	studentSelectedAnswerIds: string[]
	studentScore: number
	isCorrect: boolean
	isPartiallyCorrect: boolean
}

export interface AnswerReview {
	id: string
	content: string
	isCorrect: boolean
	explanation: string | null
	wasSelectedByStudent: boolean
}
