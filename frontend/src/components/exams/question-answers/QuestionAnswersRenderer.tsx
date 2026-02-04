import React from 'react'
import { QuestionType } from '@/types/model/exam-service/enums'
import { TrueFalseAnswers } from './TrueFalseAnswers'
import { SelectAnswers } from './SelectAnswers'
import { MultipleChoiceAnswers } from './MultipleChoiceAnswers'
import { EssayAnswer } from './EssayAnswer'
import { FillInBlankAnswers } from './FillInBlankAnswers'
import type { AnswerFieldProps } from './types'
import type { Control } from 'react-hook-form'

interface QuestionAnswersRendererProps
	extends Omit<AnswerFieldProps, 'control'> {
	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	control: Control<any>
	questionType: QuestionType
}

/**
 * Renders the appropriate answer input component based on question type
 */
export const QuestionAnswersRenderer: React.FC<
	QuestionAnswersRendererProps
> = ({
	questionType,
	control,
	fields,
	append,
	remove,
	onCorrectAnswerChange,
	disabled = false,
}) => {
	const commonProps: AnswerFieldProps = {
		control,
		fields,
		append,
		remove,
		onCorrectAnswerChange,
		disabled,
	}

	switch (questionType) {
		case QuestionType.TrueFalse:
			return <TrueFalseAnswers {...commonProps} />

		case QuestionType.MultipleChoice:
			// MultipleChoice in our enum = single correct = Select UI
			return <SelectAnswers {...commonProps} />

		case QuestionType.MultipleAnswer:
			// MultipleAnswer = multiple correct = Multiple Choice UI
			return <MultipleChoiceAnswers {...commonProps} />

		case QuestionType.Essay:
			return <EssayAnswer control={control} disabled={disabled} />

		case QuestionType.FillInTheBlank:
			return <FillInBlankAnswers {...commonProps} />

		default:
			return (
				<div className='p-4 text-muted-foreground text-center'>
					Please select a question type to configure answers.
				</div>
			)
	}
}

/**
 * Helper function to get the section title based on question type
 */
export function getAnswersSectionTitle(questionType: QuestionType): string {
	switch (questionType) {
		case QuestionType.TrueFalse:
			return 'True/False Selection'
		case QuestionType.MultipleChoice:
			return 'Answer Options (Single Choice)'
		case QuestionType.MultipleAnswer:
			return 'Answer Options (Multiple Choice)'
		case QuestionType.Essay:
			return 'Grading Criteria'
		case QuestionType.FillInTheBlank:
			return 'Acceptable Answers'
		default:
			return 'Answers'
	}
}
