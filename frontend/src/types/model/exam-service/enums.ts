/**
 * Cognitive levels based on Bloom's Taxonomy (simplified to 4 levels)
 * Matches backend FrogEdu.Exam.Domain.Enums.CognitiveLevel
 */
export enum CognitiveLevel {
	Remember = 1, // Nhận biết
	Understand = 2, // Thông hiểu
	Apply = 3, // Vận dụng
	Analyze = 4, // Vận dụng cao
}

/**
 * Question types supported by the system
 * Matches backend FrogEdu.Exam.Domain.Enums.QuestionType
 *
 * 1. MultipleChoice: Select ONE correct answer (A, B, C, D)
 * 2. MultipleAnswer: Select ONE OR MANY correct answers
 * 3. TrueFalse: A statement is correct or wrong (A or B only)
 * 4. Essay: Open-ended, no correct answer, graded based on context (AI or self-graded)
 * 5. FillInTheBlank: User types the exact word to match
 */
export enum QuestionType {
	MultipleChoice = 1, // A, B, C, D
	MultipleAnswer = 2, // Multiple correct answers
	TrueFalse = 3, // True/False
	Essay = 4, // Long text response
	FillInTheBlank = 5, // Fill in the blank
}

export enum QuestionSource {
	Manual = 0,
	AIGenerated = 1,
}

/**
 * Configuration for each question type defining its behavior
 */
export interface QuestionTypeConfig {
	type: QuestionType
	label: string
	description: string
	minAnswers: number
	maxAnswers: number | null // null = unlimited
	allowMultipleCorrect: boolean
	requiresCorrectAnswer: boolean
	answerInputType: 'text' | 'textarea' | 'fixed' | 'none'
	fixedAnswers?: string[] // For TrueFalse
}

export const QUESTION_TYPE_CONFIGS: Record<QuestionType, QuestionTypeConfig> = {
	[QuestionType.MultipleChoice]: {
		type: QuestionType.MultipleChoice,
		label: 'Select (Single Choice)',
		description: 'Select ONE correct answer from multiple options',
		minAnswers: 2,
		maxAnswers: 6,
		allowMultipleCorrect: false,
		requiresCorrectAnswer: true,
		answerInputType: 'text',
	},
	[QuestionType.MultipleAnswer]: {
		type: QuestionType.MultipleAnswer,
		label: 'Multiple Choices',
		description: 'Select ONE or MORE correct answers from multiple options',
		minAnswers: 2,
		maxAnswers: 8,
		allowMultipleCorrect: true,
		requiresCorrectAnswer: true,
		answerInputType: 'text',
	},
	[QuestionType.TrueFalse]: {
		type: QuestionType.TrueFalse,
		label: 'True/False',
		description: 'Statement is either true or false',
		minAnswers: 2,
		maxAnswers: 2,
		allowMultipleCorrect: false,
		requiresCorrectAnswer: true,
		answerInputType: 'fixed',
		fixedAnswers: ['A', 'B'],
	},
	[QuestionType.Essay]: {
		type: QuestionType.Essay,
		label: 'Essay',
		description: 'Open-ended response, AI or self graded',
		minAnswers: 0,
		maxAnswers: 1,
		allowMultipleCorrect: false,
		requiresCorrectAnswer: false,
		answerInputType: 'textarea',
	},
	[QuestionType.FillInTheBlank]: {
		type: QuestionType.FillInTheBlank,
		label: 'Fill in the Blank',
		description: 'Type the exact word/phrase to match',
		minAnswers: 1,
		maxAnswers: 5, // Can have multiple acceptable answers
		allowMultipleCorrect: true, // All provided answers are correct
		requiresCorrectAnswer: true,
		answerInputType: 'text',
	},
}

/**
 * Get cognitive level label
 */
export const COGNITIVE_LEVEL_LABELS: Record<CognitiveLevel, string> = {
	[CognitiveLevel.Remember]: 'Remember',
	[CognitiveLevel.Understand]: 'Understand',
	[CognitiveLevel.Apply]: 'Apply',
	[CognitiveLevel.Analyze]: 'Analyze',
}

/**
 * Get question type label
 */
export function getQuestionTypeLabel(type: QuestionType): string {
	return QUESTION_TYPE_CONFIGS[type]?.label ?? 'Unknown'
}

/**
 * Get cognitive level label
 */
export function getCognitiveLevelLabel(level: CognitiveLevel): string {
	return COGNITIVE_LEVEL_LABELS[level] ?? 'Unknown'
}
