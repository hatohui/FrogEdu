import type {
	FieldArrayWithId,
	UseFieldArrayAppend,
	UseFieldArrayRemove,
	Control,
	FieldValues,
} from 'react-hook-form'

/**
 * Individual answer data structure
 */
export interface AnswerData {
	content: string
	isCorrect: boolean
	explanation?: string
}

/**
 * Form type for question creation
 */
export interface QuestionFormValues extends FieldValues {
	content: string
	point: number
	type: number
	cognitiveLevel: number
	topicId: string
	isPublic: boolean
	mediaUrl?: string
	answers: AnswerData[]
}

/**
 * Props for answer field components
 */
export interface AnswerFieldProps {
	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	control: Control<any>
	fields: FieldArrayWithId<QuestionFormValues, 'answers', 'id'>[]
	append: UseFieldArrayAppend<QuestionFormValues, 'answers'>
	remove: UseFieldArrayRemove
	onCorrectAnswerChange: (index: number, isChecked: boolean) => void
	disabled?: boolean
}

/**
 * Props for read-only answer preview
 */
export interface AnswerPreviewProps {
	answers: AnswerData[]
	showExplanations?: boolean
}
