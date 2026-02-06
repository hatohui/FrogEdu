import React from 'react'
import { Plus, Trash2 } from 'lucide-react'
import { Card, CardContent } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import {
	FormControl,
	FormField,
	FormItem,
	FormLabel,
	FormDescription,
	FormMessage,
} from '@/components/ui/form'
import { Input } from '@/components/ui/input'
import { Textarea } from '@/components/ui/textarea'
import type { AnswerFieldProps } from './types'
import { useTranslation } from 'react-i18next'

/**
 * Fill in the Blank question answer component
 * Allows specifying one or more acceptable answers
 * All answers are correct (exact match required)
 */
export const FillInBlankAnswers: React.FC<AnswerFieldProps> = ({
	control,
	fields,
	append,
	remove,
	disabled = false,
}) => {
	const { t } = useTranslation()
	const handleAddAnswer = () => {
		append({ content: '', isCorrect: true, explanation: '' })
	}

	return (
		<div className='space-y-4'>
			<div className='bg-amber-50 dark:bg-amber-950 border border-amber-200 dark:border-amber-800 rounded-lg p-4'>
				<p className='text-sm text-amber-700 dark:text-amber-300'>
					{t('pages.exams.questions.answers.fill_blank.tip')}
				</p>
			</div>

			<div className='flex items-center justify-between'>
				<p className='text-sm text-muted-foreground'>
					{t('pages.exams.questions.answers.fill_blank.hint')}
				</p>
				<Button
					type='button'
					onClick={handleAddAnswer}
					variant='outline'
					size='sm'
					disabled={disabled || fields.length >= 5}
				>
					<Plus className='h-4 w-4 mr-2' />
					{t('pages.exams.questions.answers.fill_blank.add_alternative')}
				</Button>
			</div>

			{fields.map((field, index) => (
				<Card
					key={field.id}
					className='border-2 border-green-200 dark:border-green-800'
				>
					<CardContent className='pt-4'>
						<div className='flex items-start gap-4'>
							{/* Answer content */}
							<div className='flex-1 space-y-4'>
								<FormField
									control={control}
									name={`answers.${index}.content`}
									render={({ field }) => (
										<FormItem>
											<FormLabel>
												{index === 0
													? t(
															'pages.exams.questions.answers.fill_blank.correct_label'
														)
													: t(
															'pages.exams.questions.answers.fill_blank.alternative_label',
															{
																index,
															}
														)}
											</FormLabel>
											<FormDescription>
												{index === 0
													? t(
															'pages.exams.questions.answers.fill_blank.correct_help'
														)
													: t(
															'pages.exams.questions.answers.fill_blank.alternative_help'
														)}
											</FormDescription>
											<FormControl>
												<Input
													placeholder={t(
														'pages.exams.questions.answers.fill_blank.correct_placeholder'
													)}
													disabled={disabled}
													{...field}
												/>
											</FormControl>
											<FormMessage />
										</FormItem>
									)}
								/>

								{index === 0 && (
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
															'pages.exams.questions.answers.fill_blank.explanation_placeholder'
														)}
														rows={2}
														disabled={disabled}
														{...field}
													/>
												</FormControl>
											</FormItem>
										)}
									/>
								)}
							</div>

							{/* Remove button (not for the first answer) */}
							{index > 0 && (
								<Button
									type='button'
									variant='ghost'
									size='icon'
									onClick={() => remove(index)}
									disabled={disabled}
									title={t(
										'pages.exams.questions.answers.fill_blank.remove_alternative'
									)}
								>
									<Trash2 className='h-4 w-4 text-destructive' />
								</Button>
							)}
						</div>
					</CardContent>
				</Card>
			))}
		</div>
	)
}
