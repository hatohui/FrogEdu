import React from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { QuestionType } from '@/types/model/exam-service'
import { Alert, AlertDescription } from '@/components/ui/alert'
import { AlertCircle } from 'lucide-react'

import type { UseFormReturn, UseFieldArrayReturn } from 'react-hook-form'
import type { QuestionFormData } from '@/hooks/useQuestionForm'
import { QuestionAnswersRenderer } from '@/components/question-answers'
import { getAnswersSectionTitle } from '@/components/question-answers/QuestionAnswersRenderer'

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
	const answersError = form.formState.errors.answers

	return (
		<Card>
			<CardHeader>
				<CardTitle>{getAnswersSectionTitle(questionType)}</CardTitle>
				<p className='text-sm text-muted-foreground mt-2'>
					{questionType === QuestionType.Essay ||
					questionType === QuestionType.FillInTheBlank
						? 'You must mark at least one answer as correct'
						: 'You must mark at least one answer as correct (minimum 2 answers required)'}
				</p>
			</CardHeader>
			<CardContent className='space-y-4'>
				{answersError?.root && (
					<Alert variant='destructive'>
						<AlertCircle className='h-4 w-4' />
						<AlertDescription>{answersError.root.message}</AlertDescription>
					</Alert>
				)}
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
