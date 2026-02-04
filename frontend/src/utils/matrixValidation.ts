import type {
	Matrix,
	MatrixTopicDto,
	Question,
	Topic,
	CognitiveLevel,
} from '@/types/model/exam-service'

/**
 * Represents the progress for a single matrix requirement
 */
export interface MatrixRequirementProgress {
	topicId: string
	topicName: string
	cognitiveLevel: CognitiveLevel
	required: number
	fulfilled: number
	remaining: number
	percentage: number
	isComplete: boolean
	isExceeded: boolean
}

/**
 * Overall matrix validation result
 */
export interface MatrixValidationResult {
	isComplete: boolean
	totalRequired: number
	totalFulfilled: number
	totalRemaining: number
	overallPercentage: number
	requirements: MatrixRequirementProgress[]
	incompleteRequirements: MatrixRequirementProgress[]
}

/**
 * Calculate matrix progress and validation for a set of questions
 */
export function validateMatrixProgress(
	matrix: Matrix | null | undefined,
	questions: Question[],
	topics: Topic[]
): MatrixValidationResult | null {
	if (!matrix) return null

	const requirements: MatrixRequirementProgress[] = matrix.matrixTopics.map(
		(mt: MatrixTopicDto) => {
			const topic = topics.find(t => t.id === mt.topicId)
			const fulfilledCount = questions.filter(
				q => q.topicId === mt.topicId && q.cognitiveLevel === mt.cognitiveLevel
			).length

			const remaining = Math.max(0, mt.quantity - fulfilledCount)
			const percentage =
				mt.quantity > 0
					? Math.min(100, (fulfilledCount / mt.quantity) * 100)
					: 100

			return {
				topicId: mt.topicId,
				topicName: topic?.title || 'Unknown Topic',
				cognitiveLevel: mt.cognitiveLevel,
				required: mt.quantity,
				fulfilled: fulfilledCount,
				remaining,
				percentage,
				isComplete: fulfilledCount >= mt.quantity,
				isExceeded: fulfilledCount > mt.quantity,
			}
		}
	)

	const totalRequired = requirements.reduce((sum, r) => sum + r.required, 0)
	const totalFulfilled = requirements.reduce((sum, r) => sum + r.fulfilled, 0)
	const totalRemaining = requirements.reduce((sum, r) => sum + r.remaining, 0)
	const overallPercentage =
		totalRequired > 0 ? (totalFulfilled / totalRequired) * 100 : 100

	const incompleteRequirements = requirements.filter(r => !r.isComplete)

	return {
		isComplete: incompleteRequirements.length === 0,
		totalRequired,
		totalFulfilled,
		totalRemaining,
		overallPercentage,
		requirements,
		incompleteRequirements,
	}
}

/**
 * Check if a question matches matrix requirements and is needed
 */
export function isQuestionNeededForMatrix(
	question: Question,
	matrix: Matrix | null | undefined,
	existingQuestions: Question[]
): boolean {
	if (!matrix) return true // No matrix = all questions are valid

	// Find matching matrix topic
	const matchingRequirement = matrix.matrixTopics.find(
		mt =>
			mt.topicId === question.topicId &&
			mt.cognitiveLevel === question.cognitiveLevel
	)

	if (!matchingRequirement) return false

	// Check if we still need more questions for this requirement
	const existingCount = existingQuestions.filter(
		q =>
			q.topicId === question.topicId &&
			q.cognitiveLevel === question.cognitiveLevel
	).length

	return existingCount < matchingRequirement.quantity
}

/**
 * Get available slots for a specific topic and cognitive level
 */
export function getAvailableSlots(
	topicId: string,
	cognitiveLevel: CognitiveLevel,
	matrix: Matrix | null | undefined,
	existingQuestions: Question[]
): number {
	if (!matrix) return Infinity

	const requirement = matrix.matrixTopics.find(
		mt => mt.topicId === topicId && mt.cognitiveLevel === cognitiveLevel
	)

	if (!requirement) return 0

	const existingCount = existingQuestions.filter(
		q => q.topicId === topicId && q.cognitiveLevel === cognitiveLevel
	).length

	return Math.max(0, requirement.quantity - existingCount)
}

/**
 * Filter questions from bank that are needed for matrix
 */
export function filterQuestionsForMatrix(
	questions: Question[],
	matrix: Matrix | null | undefined,
	existingQuestions: Question[]
): Question[] {
	if (!matrix) return questions

	return questions.filter(question => {
		// Check if this question type is needed by matrix
		const matchingRequirement = matrix.matrixTopics.find(
			mt =>
				mt.topicId === question.topicId &&
				mt.cognitiveLevel === question.cognitiveLevel
		)

		if (!matchingRequirement) return false

		// Check if we still need more of this type
		const existingCount = existingQuestions.filter(
			q =>
				q.topicId === question.topicId &&
				q.cognitiveLevel === question.cognitiveLevel
		).length

		return existingCount < matchingRequirement.quantity
	})
}

/**
 * Get the next unfulfilled matrix requirement
 */
export function getNextUnfulfilledRequirement(
	matrix: Matrix | null | undefined,
	existingQuestions: Question[],
	topics: Topic[]
): MatrixRequirementProgress | null {
	const validation = validateMatrixProgress(matrix, existingQuestions, topics)
	if (!validation || validation.isComplete) return null

	return validation.incompleteRequirements[0] || null
}

/**
 * Check if adding a question would exceed matrix limits
 */
export function wouldExceedMatrixLimit(
	question: { topicId: string; cognitiveLevel: CognitiveLevel },
	matrix: Matrix | null | undefined,
	existingQuestions: Question[]
): boolean {
	if (!matrix) return false

	const requirement = matrix.matrixTopics.find(
		mt =>
			mt.topicId === question.topicId &&
			mt.cognitiveLevel === question.cognitiveLevel
	)

	if (!requirement) return true // Not in matrix = exceeds limit

	const existingCount = existingQuestions.filter(
		q =>
			q.topicId === question.topicId &&
			q.cognitiveLevel === question.cognitiveLevel
	).length

	return existingCount >= requirement.quantity
}
