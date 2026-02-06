import React from 'react'
import { Card, CardContent } from '@/components/ui/card'
import {
	FormControl,
	FormField,
	FormItem,
	FormLabel,
} from '@/components/ui/form'
import { Checkbox } from '@/components/ui/checkbox'
import { Textarea } from '@/components/ui/textarea'
import type { AnswerFieldProps } from './types'
import { useTranslation } from 'react-i18next'

/**
 * True/False question answer component
 * Displays exactly 2 fixed options: A and B
 * Only one can be marked as correct
 */
export const TrueFalseAnswers: React.FC<AnswerFieldProps> = ({
	control,
	fields,
	onCorrectAnswerChange,
	disabled = false,
}) => {
	const { t } = useTranslation()
	return (
		<div className='space-y-4'>
			<p className='text-sm text-muted-foreground'>
				{t('pages.exams.questions.answers.true_false.hint')}
			</p>
			{fields.map((field, index) => (
				<Card key={field.id} className='border-2'>
					<CardContent className='pt-4'>
						<div className='flex items-center gap-4'>
							<FormField
								control={control}
								name={`answers.${index}.isCorrect`}
								render={({ field: checkboxField }) => (
									<FormItem className='flex items-center space-x-2'>
										<FormControl>
											<Checkbox
												checked={checkboxField.value}
												onCheckedChange={checked =>
													onCorrectAnswerChange(index, !!checked)
												}
												disabled={disabled}
											/>
										</FormControl>
										<FormLabel className='!mt-0 font-medium text-lg'>
											{index === 0
												? t(
														'pages.exams.questions.answers.true_false.option_true'
													)
												: t(
														'pages.exams.questions.answers.true_false.option_false'
													)}
										</FormLabel>
									</FormItem>
								)}
							/>
						</div>
						{/* Explanation (optional) */}
						<FormField
							control={control}
							name={`answers.${index}.explanation`}
							render={({ field }) => (
								<FormItem className='mt-4'>
									<FormLabel className='text-sm text-muted-foreground'>
										{t(
											'pages.exams.questions.answers.common.explanation_label'
										)}
									</FormLabel>
									<FormControl>
										<Textarea
											placeholder={t(
												'pages.exams.questions.answers.true_false.explanation_placeholder',
												{
													option:
														index === 0
															? t(
																	'pages.exams.questions.answers.true_false.option_true'
																)
															: t(
																	'pages.exams.questions.answers.true_false.option_false'
																),
												}
											)}
											rows={2}
											disabled={disabled}
											{...field}
										/>
									</FormControl>
								</FormItem>
							)}
						/>
					</CardContent>
				</Card>
			))}
		</div>
	)
}
