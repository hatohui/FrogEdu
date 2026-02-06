import React from 'react'
import {
	CheckSquare,
	Circle,
	ToggleLeft,
	FileText,
	PenTool,
} from 'lucide-react'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { QuestionType, QUESTION_TYPE_CONFIGS } from '@/types/model/exam-service'
import { useTranslation } from 'react-i18next'

interface QuestionTypeSelectorProps {
	value: QuestionType
	onValueChange: (value: QuestionType) => void
	disabled?: boolean
}

/**
 * Get icon for question type
 */
function getQuestionTypeIcon(type: QuestionType) {
	switch (type) {
		case QuestionType.TrueFalse:
			return <ToggleLeft className='h-4 w-4' />
		case QuestionType.MultipleChoice:
			return <Circle className='h-4 w-4' />
		case QuestionType.MultipleAnswer:
			return <CheckSquare className='h-4 w-4' />
		case QuestionType.Essay:
			return <FileText className='h-4 w-4' />
		case QuestionType.FillInTheBlank:
			return <PenTool className='h-4 w-4' />
		default:
			return <Circle className='h-4 w-4' />
	}
}

/**
 * Question type selector with icons and descriptions
 */
export const QuestionTypeSelector: React.FC<QuestionTypeSelectorProps> = ({
	value,
	onValueChange,
	disabled = false,
}) => {
	const { t } = useTranslation()
	const config = QUESTION_TYPE_CONFIGS[value]
	const resolvedLabel =
		config?.label ?? t('components.exams.question_type.select_placeholder')

	return (
		<Select
			value={String(value)}
			onValueChange={v => onValueChange(Number(v) as QuestionType)}
			disabled={disabled}
		>
			<SelectTrigger className='w-full'>
				<SelectValue>
					<div className='flex items-center gap-2'>
						{getQuestionTypeIcon(value)}
						<span>{resolvedLabel}</span>
					</div>
				</SelectValue>
			</SelectTrigger>
			<SelectContent>
				{/* True/False */}
				<SelectItem value={String(QuestionType.TrueFalse)}>
					<div className='flex items-center gap-2'>
						<ToggleLeft className='h-4 w-4' />
						<div>
							<div className='font-medium'>
								{t('exams.question_types.true_false')}
							</div>
							<div className='text-xs text-muted-foreground'>
								{t('components.exams.question_type.descriptions.true_false')}
							</div>
						</div>
					</div>
				</SelectItem>

				{/* Single Choice */}
				<SelectItem value={String(QuestionType.MultipleChoice)}>
					<div className='flex items-center gap-2'>
						<Circle className='h-4 w-4' />
						<div>
							<div className='font-medium'>
								{t('components.exams.question_type.labels.single_choice')}
							</div>
							<div className='text-xs text-muted-foreground'>
								{t('components.exams.question_type.descriptions.single_choice')}
							</div>
						</div>
					</div>
				</SelectItem>

				{/* Multiple Choices */}
				<SelectItem value={String(QuestionType.MultipleAnswer)}>
					<div className='flex items-center gap-2'>
						<CheckSquare className='h-4 w-4' />
						<div>
							<div className='font-medium'>
								{t('components.exams.question_type.labels.multiple_choice')}
							</div>
							<div className='text-xs text-muted-foreground'>
								{t(
									'components.exams.question_type.descriptions.multiple_choice'
								)}
							</div>
						</div>
					</div>
				</SelectItem>

				{/* Essay */}
				<SelectItem value={String(QuestionType.Essay)}>
					<div className='flex items-center gap-2'>
						<FileText className='h-4 w-4' />
						<div>
							<div className='font-medium'>
								{t('exams.question_types.essay')}
							</div>
							<div className='text-xs text-muted-foreground'>
								{t('components.exams.question_type.descriptions.essay')}
							</div>
						</div>
					</div>
				</SelectItem>

				{/* Fill in the Blank */}
				<SelectItem value={String(QuestionType.FillInTheBlank)}>
					<div className='flex items-center gap-2'>
						<PenTool className='h-4 w-4' />
						<div>
							<div className='font-medium'>
								{t('exams.question_types.fill_in_blank')}
							</div>
							<div className='text-xs text-muted-foreground'>
								{t('components.exams.question_type.descriptions.fill_blank')}
							</div>
						</div>
					</div>
				</SelectItem>
			</SelectContent>
		</Select>
	)
}

export default QuestionTypeSelector
