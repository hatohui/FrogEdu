import React from 'react'
import { Card, CardContent } from '@/components/ui/card'
import {
	FormControl,
	FormField,
	FormItem,
	FormLabel,
	FormMessage,
} from '@/components/ui/form'
import { Textarea } from '@/components/ui/textarea'
import type { Control } from 'react-hook-form'

interface EssayAnswerProps {
	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	control: Control<any>
	disabled?: boolean
}

/**
 * Essay question answer component
 * Provides a grading rubric/criteria field
 * Essays don't have pre-defined "correct" answers
 */
export const EssayAnswer: React.FC<EssayAnswerProps> = ({
	control,
	disabled = false,
}) => {
	return (
		<div className='space-y-4'>
			<p className='text-sm text-muted-foreground'>
				Essay questions require manual grading. Define the grading criteria and
				expected key points.
			</p>

			<Card className='border-2'>
				<CardContent className='pt-4 space-y-4'>
					{/* Grading Rubric */}
					<FormField
						control={control}
						name='answers.0.content'
						render={({ field }) => (
							<FormItem>
								<FormLabel>Grading Rubric / Key Points *</FormLabel>
								<FormControl>
									<Textarea
										placeholder='Define the key points, criteria, and expectations for grading this essay...

Example:
- Main argument clearly stated (2 points)
- Supporting evidence provided (3 points)
- Proper structure and flow (2 points)
- Correct grammar and spelling (1 point)
- Conclusion summarizes main points (2 points)'
										rows={8}
										disabled={disabled}
										{...field}
									/>
								</FormControl>
								<FormMessage />
							</FormItem>
						)}
					/>

					{/* Model Answer (optional) */}
					<FormField
						control={control}
						name='answers.0.explanation'
						render={({ field }) => (
							<FormItem>
								<FormLabel className='text-sm text-muted-foreground'>
									Model Answer (Optional)
								</FormLabel>
								<FormControl>
									<Textarea
										placeholder='Provide a sample model answer for reference...'
										rows={6}
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
