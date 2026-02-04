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
	return (
		<div className='space-y-4'>
			<div className='bg-blue-50 dark:bg-blue-950 border border-blue-200 dark:border-blue-800 rounded-lg p-4'>
				<p className='text-sm text-blue-700 dark:text-blue-300'>
					<strong>Essay questions</strong> are open-ended and will be graded
					based on content accuracy and relevance. You can set grading rubric
					below to guide the evaluation.
				</p>
			</div>

			<Card className='border-2'>
				<CardContent className='pt-4 space-y-4'>
					<FormField
						control={control}
						name='answers.0.content'
						render={({ field }) => (
							<FormItem>
								<FormLabel>Grading Rubric / Expected Points *</FormLabel>
								<FormDescription>
									Describe what a good answer should include. This will be used
									for grading.
								</FormDescription>
								<FormControl>
									<Textarea
										placeholder={`Example:
                      • Key points to cover (40%)
                      • Logical structure and argumentation (30%)
                      • Supporting examples or evidence (20%)
                      • Language clarity and grammar (10%)`}
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
									Sample Answer (Optional)
								</FormLabel>
								<FormDescription>
									Provide a sample answer for reference
								</FormDescription>
								<FormControl>
									<Textarea
										placeholder='Write a sample high-quality answer here...'
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
