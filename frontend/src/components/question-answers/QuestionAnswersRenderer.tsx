import React from 'react'
import { QuestionType } from '@/types/model/exam-service'
import type { AnswerFieldProps } from './types'
import { TrueFalseAnswers } from './TrueFalseAnswers'
import { SelectAnswers } from './SelectAnswers'
import { MultipleChoiceAnswers } from './MultipleChoiceAnswers'
import { EssayAnswer } from './EssayAnswer'
import { FillInBlankAnswers } from './FillInBlankAnswers'

interface QuestionAnswersRendererProps extends AnswerFieldProps {
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
	switch (questionType) {
		case QuestionType.TrueFalse:
			return (
				<TrueFalseAnswers
					control={control}
					fields={fields}
					append={append}
					remove={remove}
					onCorrectAnswerChange={onCorrectAnswerChange}
					disabled={disabled}
				/>
			)

		case QuestionType.MultipleChoice:
			return (
				<SelectAnswers
					control={control}
					fields={fields}
					append={append}
					remove={remove}
					onCorrectAnswerChange={onCorrectAnswerChange}
					disabled={disabled}
				/>
			)

		case QuestionType.MultipleAnswer:
			return (
				<MultipleChoiceAnswers
					control={control}
					fields={fields}
					append={append}
					remove={remove}
					onCorrectAnswerChange={onCorrectAnswerChange}
					disabled={disabled}
				/>
			)

		case QuestionType.Essay:
			return <EssayAnswer control={control} disabled={disabled} />

		case QuestionType.FillInTheBlank:
			return (
				<FillInBlankAnswers
					control={control}
					fields={fields}
					append={append}
					remove={remove}
					onCorrectAnswerChange={onCorrectAnswerChange}
					disabled={disabled}
				/>
			)

		default:
			return (
				<SelectAnswers
					control={control}
					fields={fields}
					append={append}
					remove={remove}
					onCorrectAnswerChange={onCorrectAnswerChange}
					disabled={disabled}
				/>
			)
	}
}

/**
 * Get the title for the answers section based on question type
 */
export function getAnswersSectionTitle(questionType: QuestionType): string {
	switch (questionType) {
		case QuestionType.TrueFalse:
			return 'True/False Selection'
		case QuestionType.MultipleChoice:
			return 'Answer Options (Select One)'
		case QuestionType.MultipleAnswer:
			return 'Answer Options (Select Multiple)'
		case QuestionType.Essay:
			return 'Grading Guidelines'
		case QuestionType.FillInTheBlank:
			return 'Correct Answer(s)'
		default:
			return 'Answer Options'
	}
}
