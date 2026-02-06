import React from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { QuestionType } from '@/types/model/exam-service'
import { Alert, AlertDescription } from '@/components/ui/alert'
import { AlertCircle } from 'lucide-react'

import type { UseFormReturn, UseFieldArrayReturn } from 'react-hook-form'
import type { QuestionFormData } from '@/hooks/useQuestionForm'
import { QuestionAnswersRenderer } from '@/components/question-answers'
import { useTranslation } from 'react-i18next'

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
	const { t } = useTranslation()
	const answersError = form.formState.errors.answers
	const answersTitle = (() => {
		switch (questionType) {
			case QuestionType.TrueFalse:
				return t('pages.exams.questions.answers.title.true_false')
			case QuestionType.MultipleChoice:
				return t('pages.exams.questions.answers.title.single_choice')
			case QuestionType.MultipleAnswer:
				return t('pages.exams.questions.answers.title.multiple_choice')
			case QuestionType.Essay:
				return t('pages.exams.questions.answers.title.essay')
			case QuestionType.FillInTheBlank:
				return t('pages.exams.questions.answers.title.fill_blank')
			default:
				return t('pages.exams.questions.answers.title.default')
		}
	})()

	return (
		<Card>
			<CardHeader>
				<CardTitle>{answersTitle}</CardTitle>
				<p className='text-sm text-muted-foreground mt-2'>
					{questionType === QuestionType.Essay ||
					questionType === QuestionType.FillInTheBlank
						? t('pages.exams.questions.answers.hint_simple')
						: t('pages.exams.questions.answers.hint_multiple')}
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
