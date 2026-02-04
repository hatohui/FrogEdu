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
import { Checkbox } from '@/components/ui/checkbox'
import type { AnswerFieldProps } from './types'

/**
 * Multiple Choice question answer component
 * Allows adding multiple options with MULTIPLE correct answers (checkboxes)
 */
export const MultipleChoiceAnswers: React.FC<AnswerFieldProps> = ({
	control,
	fields,
	append,
	remove,
	onCorrectAnswerChange,
	disabled = false,
}) => {
	const handleAddAnswer = () => {
		append({ content: '', isCorrect: false, explanation: '' })
	}

	return (
		<div className='space-y-4'>
			<div className='flex items-center justify-between'>
				<p className='text-sm text-muted-foreground'>
					Add answer options. Select ALL correct answers.
				</p>
				<Button
					type='button'
					onClick={handleAddAnswer}
					variant='outline'
					size='sm'
					disabled={disabled || fields.length >= 8}
				>
					<Plus className='h-4 w-4 mr-2' />
					Add Option
				</Button>
			</div>

			{fields.map((field, index) => (
				<Card key={field.id} className='border-2'>
					<CardContent className='pt-4'>
						<div className='flex items-start gap-4'>
							{/* Checkbox for correct answer */}
							<FormField
								control={control}
								name={`answers.${index}.isCorrect`}
								render={({ field: checkboxField }) => (
									<FormItem className='flex items-center space-x-2 pt-2'>
										<FormControl>
											<Checkbox
												checked={checkboxField.value}
												onCheckedChange={checked =>
													onCorrectAnswerChange(index, !!checked)
												}
												disabled={disabled}
											/>
										</FormControl>
										<FormLabel className='!mt-0 font-medium'>
											{String.fromCharCode(65 + index)}.
										</FormLabel>
									</FormItem>
								)}
							/>

							{/* Answer content */}
							<div className='flex-1 space-y-4'>
								<FormField
									control={control}
									name={`answers.${index}.content`}
									render={({ field }) => (
										<FormItem>
											<FormLabel>Answer {index + 1} *</FormLabel>
											<FormControl>
												<Input
													placeholder='Enter answer option...'
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
												Explanation (Optional)
											</FormLabel>
											<FormControl>
												<Textarea
													placeholder='Explain why this answer is correct/incorrect...'
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
									title='Remove answer'
								>
									<Trash2 className='h-4 w-4 text-destructive' />
								</Button>
							)}
						</div>
					</CardContent>
				</Card>
			))}

			{fields.length < 2 && (
				<p className='text-sm text-destructive'>
					At least 2 answer options are required
				</p>
			)}
		</div>
	)
}
