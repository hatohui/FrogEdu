import React from 'react'
import { Card, CardContent } from '@/components/ui/card'
import {
	FormControl,
	FormField,
	FormItem,
	FormLabel,
	FormDescription,
} from '@/components/ui/form'
import { Textarea } from '@/components/ui/textarea'
import type { AnswerFieldProps } from './types'
import { useTranslation } from 'react-i18next'

/**
 * Essay question answer component
 * Provides a grading rubric/guidelines field
 * The answer will be AI or self graded
 */
export const EssayAnswer: React.FC<
	Omit<
		AnswerFieldProps,
		'append' | 'remove' | 'fields' | 'onCorrectAnswerChange'
	>
> = ({ control, disabled = false }) => {
	const { t } = useTranslation()
	return (
		<div className='space-y-4'>
			<div className='bg-blue-50 dark:bg-blue-950 border border-blue-200 dark:border-blue-800 rounded-lg p-4'>
				<p className='text-sm text-blue-700 dark:text-blue-300'>
					{t('pages.exams.questions.answers.essay.tip')}
				</p>
			</div>

			<Card className='border-2'>
				<CardContent className='pt-4 space-y-4'>
					<FormField
						control={control}
						name='answers.0.content'
						render={({ field }) => (
							<FormItem>
								<FormLabel>
									{t('pages.exams.questions.answers.essay.rubric_label')}
								</FormLabel>
								<FormDescription>
									{t('pages.exams.questions.answers.essay.rubric_help')}
								</FormDescription>
								<FormControl>
									<Textarea
										placeholder={t(
											'pages.exams.questions.answers.essay.rubric_placeholder'
										)}
										rows={6}
										disabled={disabled}
										{...field}
									/>
								</FormControl>
							</FormItem>
						)}
					/>

					<FormField
						control={control}
						name='answers.0.explanation'
						render={({ field }) => (
							<FormItem>
								<FormLabel className='text-sm text-muted-foreground'>
									{t('pages.exams.questions.answers.essay.sample_label')}
								</FormLabel>
								<FormDescription>
									{t('pages.exams.questions.answers.essay.sample_help')}
								</FormDescription>
								<FormControl>
									<Textarea
										placeholder={t(
											'pages.exams.questions.answers.essay.sample_placeholder'
										)}
										rows={4}
										disabled={disabled}
										{...field}
									/>
								</FormControl>
							</FormItem>
						)}
					/>
				</CardContent>
			</Card>
		</div>
	)
}
