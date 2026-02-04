import React from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { QuestionType } from '@/types/model/exam-service'
import {
	QuestionAnswersRenderer,
	getAnswersSectionTitle,
} from '@/components/exams/question-answers'
import type { UseFormReturn, UseFieldArrayReturn } from 'react-hook-form'
import type { QuestionFormData } from '@/hooks/useQuestionForm'

interface QuestionAnswersSectionProps {
	form: UseFormReturn<QuestionFormData>
	fields: UseFieldArrayReturn<QuestionFormData, 'answers', 'id'>['fields']
	questionType: QuestionType
	onCorrectAnswerChange: (index: number, isChecked: boolean) => void
	onAddAnswer: () => boolean
	onRemoveAnswer: (index: number) => boolean
	append: UseFieldArrayReturn<QuestionFormData, 'answers', 'id'>['append']
	remove: UseFieldArrayReturn<QuestionFormData, 'answers', 'id'>['remove']
}

/**
 * Answers section for question form
 * Renders appropriate answer input based on question type
 */
export const QuestionAnswersSection: React.FC<QuestionAnswersSectionProps> = ({
	form,
	fields,
	questionType,
	onCorrectAnswerChange,
	append,
	remove,
}) => {
	return (
		<Card>
			<CardHeader>
				<CardTitle>{getAnswersSectionTitle(questionType)}</CardTitle>
			</CardHeader>
			<CardContent>
				<QuestionAnswersRenderer
					questionType={questionType}
					control={form.control}
					fields={fields}
					append={append}
					remove={remove}
					onCorrectAnswerChange={onCorrectAnswerChange}
				/>
			</CardContent>
		</Card>
	)
}

export default QuestionAnswersSection
