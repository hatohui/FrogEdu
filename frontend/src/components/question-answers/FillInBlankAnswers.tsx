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
	const handleAddAnswer = () => {
		append({ content: '', isCorrect: true, explanation: '' })
	}

	return (
		<div className='space-y-4'>
			<div className='bg-amber-50 dark:bg-amber-950 border border-amber-200 dark:border-amber-800 rounded-lg p-4'>
				<p className='text-sm text-amber-700 dark:text-amber-300'>
					<strong>Fill in the Blank:</strong> Use underscores (_____) in your
					question to indicate where the blank is. Add all acceptable answers
					below - the student must type the <strong>exact word/phrase</strong>{' '}
					to be correct.
				</p>
			</div>

			<div className='flex items-center justify-between'>
				<p className='text-sm text-muted-foreground'>
					Add acceptable answers (alternative spellings, synonyms)
				</p>
				<Button
					type='button'
					onClick={handleAddAnswer}
					variant='outline'
					size='sm'
					disabled={disabled || fields.length >= 5}
				>
					<Plus className='h-4 w-4 mr-2' />
					Add Alternative
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
													? 'Correct Answer *'
													: `Alternative Answer ${index}`}
											</FormLabel>
											<FormDescription>
												{index === 0
													? 'The primary correct answer (exact match required)'
													: 'Alternative acceptable answer'}
											</FormDescription>
											<FormControl>
												<Input
													placeholder='Enter the correct word/phrase...'
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
													Explanation (Optional)
												</FormLabel>
												<FormControl>
													<Textarea
														placeholder='Explain why this is the correct answer...'
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
									title='Remove alternative answer'
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
