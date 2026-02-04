import type { UseFieldArrayReturn, Control } from 'react-hook-form'

/**
 * Individual answer data structure
 */
export interface AnswerData {
	content: string
	isCorrect: boolean
	explanation?: string
}

/**
 * Props for answer field components
 */
export interface AnswerFieldProps {
	control: Control<any>
	fields: UseFieldArrayReturn<any, 'answers', 'id'>['fields']
	append: UseFieldArrayReturn<any, 'answers', 'id'>['append']
	remove: UseFieldArrayReturn<any, 'answers', 'id'>['remove']
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
