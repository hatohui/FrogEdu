import React from 'react'
import { Check, X, PenLine, Save, Trash2 } from 'lucide-react'
import { Card, CardContent } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import {
	QuestionType,
	getQuestionTypeLabel,
	getCognitiveLevelLabel,
} from '@/types/model/exam-service'
import type { AIGeneratedQuestion } from '@/types/model/ai-service'

interface AIQuestionPreviewProps {
	question: AIGeneratedQuestion
	index: number
	onEdit: (question: AIGeneratedQuestion) => void
	onSave: (question: AIGeneratedQuestion, index: number) => void
	onRemove: (index: number) => void
	isSaving?: boolean
}

/**
 * Preview component for AI-generated questions
 * Displays the question with type-specific answer formatting
 */
export const AIQuestionPreview: React.FC<AIQuestionPreviewProps> = ({
	question,
	index,
	onEdit,
	onSave,
	onRemove,
	isSaving = false,
}) => {
	return (
		<Card className='border-2'>
			<CardContent className='pt-4 space-y-4'>
				{/* Header with question content and actions */}
				<div className='flex justify-between items-start gap-4'>
					<div className='flex-1'>
						<p className='font-medium mb-3'>{question.content}</p>
						<div className='flex gap-2 flex-wrap'>
							<Badge variant='default'>
								{getQuestionTypeLabel(question.questionType)}
							</Badge>
							<Badge variant='outline'>
								{getCognitiveLevelLabel(question.cognitiveLevel)}
							</Badge>
							<Badge variant='secondary'>{question.point} pts</Badge>
						</div>
					</div>
					<div className='flex gap-2'>
						<Button
							size='sm'
							variant='outline'
							onClick={() => onEdit(question)}
						>
							<PenLine className='h-4 w-4 mr-1' />
							Edit
						</Button>
						<Button
							size='sm'
							onClick={() => onSave(question, index)}
							disabled={isSaving}
						>
							<Save className='h-4 w-4 mr-1' />
							Save
						</Button>
						<Button size='sm' variant='ghost' onClick={() => onRemove(index)}>
							<Trash2 className='h-4 w-4' />
						</Button>
					</div>
				</div>

				{/* Answers Section */}
				<AnswersPreview
					questionType={question.questionType}
					answers={question.answers}
				/>
			</CardContent>
		</Card>
	)
}

interface AnswersPreviewProps {
	questionType: QuestionType
	answers: AIGeneratedQuestion['answers']
}

/**
 * Renders answer preview based on question type
 */
const AnswersPreview: React.FC<AnswersPreviewProps> = ({
	questionType,
	answers,
}) => {
	// Render type-specific answer preview
	const renderAnswers = () => {
		switch (questionType) {
			case QuestionType.TrueFalse:
				return (
					<div className='space-y-2'>
						{answers.map((answer, idx) => (
							<div
								key={idx}
								className={`p-3 rounded-lg border-2 ${
									answer.isCorrect
										? 'bg-green-50 border-green-300 dark:bg-green-950 dark:border-green-800'
										: 'bg-gray-50 border-gray-200 dark:bg-gray-900 dark:border-gray-800'
								}`}
							>
								<div className='flex items-center gap-2'>
									<CorrectIcon isCorrect={answer.isCorrect} />
									<span className='font-medium'>{answer.content}</span>
								</div>
								{answer.explanation && (
									<p className='text-xs text-muted-foreground mt-1 ml-6'>
										💡 {answer.explanation}
									</p>
								)}
							</div>
						))}
					</div>
				)

			case QuestionType.Essay:
				return (
					<div className='bg-blue-50 dark:bg-blue-950 border border-blue-200 dark:border-blue-800 rounded-lg p-4'>
						<p className='text-sm font-medium text-blue-700 dark:text-blue-300 mb-2'>
							Grading Rubric:
						</p>
						<p className='text-sm whitespace-pre-wrap'>
							{answers[0]?.content || 'No rubric provided'}
						</p>
						{answers[0]?.explanation && (
							<div className='mt-3 pt-3 border-t border-blue-200 dark:border-blue-700'>
								<p className='text-sm font-medium text-blue-700 dark:text-blue-300 mb-1'>
									Sample Answer:
								</p>
								<p className='text-sm'>{answers[0].explanation}</p>
							</div>
						)}
					</div>
				)

			case QuestionType.FillInTheBlank:
				return (
					<div className='space-y-2'>
						<p className='text-sm font-medium text-muted-foreground'>
							Acceptable Answers:
						</p>
						<div className='flex flex-wrap gap-2'>
							{answers.map((answer, idx) => (
								<Badge
									key={idx}
									variant='outline'
									className='bg-green-50 border-green-300 text-green-700 dark:bg-green-950 dark:border-green-700 dark:text-green-300'
								>
									{answer.content}
								</Badge>
							))}
						</div>
						{answers[0]?.explanation && (
							<p className='text-xs text-muted-foreground mt-2'>
								💡 {answers[0].explanation}
							</p>
						)}
					</div>
				)

			case QuestionType.MultipleChoice:
			case QuestionType.MultipleAnswer:
			default:
				return (
					<div className='space-y-2'>
						{answers.map((answer, idx) => (
							<div
								key={idx}
								className={`p-3 rounded-lg border-2 ${
									answer.isCorrect
										? 'bg-green-50 border-green-300 dark:bg-green-950 dark:border-green-800'
										: 'bg-gray-50 border-gray-200 dark:bg-gray-900 dark:border-gray-800'
								}`}
							>
								<div className='flex items-start gap-2'>
									<CorrectIcon isCorrect={answer.isCorrect} />
									<div className='flex-1'>
										<span className='font-medium mr-2'>
											{String.fromCharCode(65 + idx)}.
										</span>
										<span>{answer.content}</span>
										{answer.explanation && (
											<p className='text-xs text-muted-foreground mt-1'>
												💡 {answer.explanation}
											</p>
										)}
									</div>
								</div>
							</div>
						))}
					</div>
				)
		}
	}

	return (
		<div className='space-y-2'>
			<p className='text-sm font-medium text-muted-foreground'>Answers:</p>
			{renderAnswers()}
		</div>
	)
}

const CorrectIcon: React.FC<{ isCorrect: boolean }> = ({ isCorrect }) => (
	<div className='flex-shrink-0 mt-0.5'>
		{isCorrect ? (
			<Check className='h-4 w-4 text-green-600 dark:text-green-400' />
		) : (
			<X className='h-4 w-4 text-gray-400' />
		)}
	</div>
)

export default AIQuestionPreview
