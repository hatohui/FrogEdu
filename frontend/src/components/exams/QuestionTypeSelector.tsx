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
	const config = QUESTION_TYPE_CONFIGS[value]

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
						<span>{config?.label ?? 'Select type'}</span>
					</div>
				</SelectValue>
			</SelectTrigger>
			<SelectContent>
				{/* True/False */}
				<SelectItem value={String(QuestionType.TrueFalse)}>
					<div className='flex items-center gap-2'>
						<ToggleLeft className='h-4 w-4' />
						<div>
							<div className='font-medium'>True/False</div>
							<div className='text-xs text-muted-foreground'>
								Statement is correct or wrong
							</div>
						</div>
					</div>
				</SelectItem>

				{/* Single Choice */}
				<SelectItem value={String(QuestionType.MultipleChoice)}>
					<div className='flex items-center gap-2'>
						<Circle className='h-4 w-4' />
						<div>
							<div className='font-medium'>Select (Single Choice)</div>
							<div className='text-xs text-muted-foreground'>
								Select ONE correct answer
							</div>
						</div>
					</div>
				</SelectItem>

				{/* Multiple Choices */}
				<SelectItem value={String(QuestionType.MultipleAnswer)}>
					<div className='flex items-center gap-2'>
						<CheckSquare className='h-4 w-4' />
						<div>
							<div className='font-medium'>Multiple Choices</div>
							<div className='text-xs text-muted-foreground'>
								Select ONE or MORE correct answers
							</div>
						</div>
					</div>
				</SelectItem>

				{/* Essay */}
				<SelectItem value={String(QuestionType.Essay)}>
					<div className='flex items-center gap-2'>
						<FileText className='h-4 w-4' />
						<div>
							<div className='font-medium'>Essay</div>
							<div className='text-xs text-muted-foreground'>
								Open-ended, AI or self graded
							</div>
						</div>
					</div>
				</SelectItem>

				{/* Fill in the Blank */}
				<SelectItem value={String(QuestionType.FillInTheBlank)}>
					<div className='flex items-center gap-2'>
						<PenTool className='h-4 w-4' />
						<div>
							<div className='font-medium'>Fill in the Blank</div>
							<div className='text-xs text-muted-foreground'>
								Type exact word/phrase to match
							</div>
						</div>
					</div>
				</SelectItem>
			</SelectContent>
		</Select>
	)
}

export default QuestionTypeSelector
