import React from 'react'
import { Plus, Trash2 } from 'lucide-react'
import { Card, CardContent } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import {
	FormControl,
	FormField,
	FormItem,
	FormLabel,
	FormMessage,
} from '@/components/ui/form'
import { Input } from '@/components/ui/input'
import { Textarea } from '@/components/ui/textarea'
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group'
import { Label } from '@/components/ui/label'
import type { AnswerFieldProps } from './types'
import { useTranslation } from 'react-i18next'

/**
 * Select (Single Choice) question answer component
 * Allows adding multiple options with exactly ONE correct answer
 */
export const SelectAnswers: React.FC<AnswerFieldProps> = ({
	control,
	fields,
	append,
	remove,
	onCorrectAnswerChange,
	disabled = false,
}) => {
	const { t } = useTranslation()
	// Find the currently correct answer index
	const correctIndex = fields.findIndex(
		(_, i) => control._formValues?.answers?.[i]?.isCorrect
	)

	const handleAddAnswer = () => {
		append({ content: '', isCorrect: false, explanation: '' })
	}

	const handleRadioChange = (value: string) => {
		const newCorrectIndex = parseInt(value, 10)
		onCorrectAnswerChange(newCorrectIndex, true)
	}

	return (
		<div className='space-y-3'>
			<div className='flex items-center justify-between'>
				<p className='text-sm text-muted-foreground'>
					{t('pages.exams.questions.answers.select.hint')}
				</p>
				<Button
					type='button'
					onClick={handleAddAnswer}
					variant='outline'
					size='sm'
					disabled={disabled || fields.length >= 6}
				>
					<Plus className='h-4 w-4 mr-2' />
					{t('pages.exams.questions.answers.select.add_option')}
				</Button>
			</div>

			<RadioGroup
				value={correctIndex >= 0 ? String(correctIndex) : undefined}
				onValueChange={handleRadioChange}
				disabled={disabled}
			>
				{fields.map((field, index) => (
					<Card key={field.id} className='border-2'>
						<CardContent className='py-1'>
							<div className='flex items-start gap-4'>
								{/* Radio button for correct answer */}
								<div className='flex items-center space-x-2 pt-2'>
									<RadioGroupItem
										value={String(index)}
										id={`answer-${index}`}
									/>
									<Label htmlFor={`answer-${index}`} className='font-medium'>
										{String.fromCharCode(65 + index)}.
									</Label>
								</div>

								{/* Answer content */}
								<div className='flex-1 space-y-3'>
									<FormField
										control={control}
										name={`answers.${index}.content`}
										render={({ field }) => (
											<FormItem>
												<FormLabel>
													{t(
														'pages.exams.questions.answers.select.answer_label',
														{
															index: index + 1,
														}
													)}
												</FormLabel>
												<FormControl>
													<Input
														placeholder={t(
															'pages.exams.questions.answers.select.answer_placeholder'
														)}
														disabled={disabled}
														{...field}
													/>
												</FormControl>
												<FormMessage />
											</FormItem>
										)}
									/>

									<FormField
										control={control}
										name={`answers.${index}.explanation`}
										render={({ field }) => (
											<FormItem>
												<FormLabel className='text-sm text-muted-foreground'>
													{t(
														'pages.exams.questions.answers.common.explanation_label'
													)}
												</FormLabel>
												<FormControl>
													<Textarea
														placeholder={t(
															'pages.exams.questions.answers.common.explanation_placeholder'
														)}
														rows={2}
														disabled={disabled}
														{...field}
													/>
												</FormControl>
											</FormItem>
										)}
									/>
								</div>

								{/* Remove button */}
								{fields.length > 2 && (
									<Button
										type='button'
										variant='ghost'
										size='icon'
										onClick={() => remove(index)}
										disabled={disabled}
										title={t(
											'pages.exams.questions.answers.common.remove_answer'
										)}
									>
										<Trash2 className='h-4 w-4 text-destructive' />
									</Button>
								)}
							</div>
						</CardContent>
					</Card>
				))}
			</RadioGroup>

			{fields.length < 2 && (
				<p className='text-sm text-destructive'>
					{t('pages.exams.questions.answers.common.min_two')}
				</p>
			)}
		</div>
	)
}
