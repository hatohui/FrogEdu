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
	return (
		<div className='space-y-4'>
			<p className='text-sm text-muted-foreground'>
				Select which answer is correct for this True/False statement
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
											{index === 0 ? 'A (True)' : 'B (False)'}
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
										Explanation (Optional)
									</FormLabel>
									<FormControl>
										<Textarea
											placeholder={`Explain why ${index === 0 ? 'A (True)' : 'B (False)'} is correct or incorrect...`}
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
