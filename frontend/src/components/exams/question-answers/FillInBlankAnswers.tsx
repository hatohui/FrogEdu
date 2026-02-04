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
import type { AnswerFieldProps } from './types'

/**
 * Fill in the Blank question answer component
 * Allows multiple acceptable answers for exact matching
 * All answers are considered "correct" as they are valid alternatives
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
			<div className='flex items-center justify-between'>
				<p className='text-sm text-muted-foreground'>
					Add acceptable answers. All listed answers will be considered correct.
				</p>
				<Button
					type='button'
					onClick={handleAddAnswer}
					variant='outline'
					size='sm'
					disabled={disabled || fields.length >= 10}
				>
					<Plus className='h-4 w-4 mr-2' />
					Add Alternative
				</Button>
			</div>

			{fields.map((field, index) => (
				<Card key={field.id} className='border-2'>
					<CardContent className='pt-4'>
						<div className='flex items-center gap-4'>
							{/* Answer number */}
							<div className='flex items-center justify-center w-8 h-8 rounded-full bg-primary/10 text-primary font-medium'>
								{index + 1}
							</div>

							{/* Answer content */}
							<FormField
								control={control}
								name={`answers.${index}.content`}
								render={({ field }) => (
									<FormItem className='flex-1'>
										<FormLabel className='sr-only'>
											Acceptable Answer {index + 1}
										</FormLabel>
										<FormControl>
											<Input
												placeholder={
													index === 0
														? 'Primary answer...'
														: 'Alternative answer (e.g., different spelling)...'
												}
												disabled={disabled}
												{...field}
											/>
										</FormControl>
										<FormMessage />
									</FormItem>
								)}
							/>

							{/* Remove button */}
							{fields.length > 1 && (
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

			{fields.length === 0 && (
				<p className='text-sm text-destructive'>
					At least 1 answer is required
				</p>
			)}

			<div className='p-4 bg-muted rounded-lg'>
				<p className='text-sm'>
					<strong>Tip:</strong> Add multiple variations of the correct answer to
					account for:
				</p>
				<ul className='text-sm text-muted-foreground list-disc list-inside mt-2'>
					<li>Different spellings (color / colour)</li>
					<li>Abbreviations (USA / United States)</li>
					<li>Common variations (1000 / 1,000 / one thousand)</li>
				</ul>
			</div>
		</div>
	)
}
