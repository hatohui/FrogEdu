import React, { useState, useEffect } from 'react'
import { useNavigate, useParams } from 'react-router'
import { useForm, useFieldArray } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import * as z from 'zod'
import { ArrowLeft, Plus, Trash2, Upload, Save } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import {
	Form,
	FormControl,
	FormDescription,
	FormField,
	FormItem,
	FormLabel,
	FormMessage,
} from '@/components/ui/form'
import { Input } from '@/components/ui/input'
import { Textarea } from '@/components/ui/textarea'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { Checkbox } from '@/components/ui/checkbox'
import {
	useCreateQuestion,
	useMatrix,
	useExam,
	useTopics,
} from '@/hooks/useExams'
import { QuestionType, CognitiveLevel } from '@/types/model/exam-service'
import { TopicSelector } from '@/components/exams/topic-selector'
import { MatrixProgressTracker } from '@/components/exams/MatrixProgressTracker'
import { Badge } from '@/components/ui/badge'
import { Info } from 'lucide-react'

const answerSchema = z.object({
	content: z.string().min(1, 'Answer content is required'),
	isCorrect: z.boolean(),
	explanation: z.string().optional(),
})

const questionSchema = z.object({
	content: z.string().min(10, 'Question must be at least 10 characters'),
	point: z.number().min(0.5).max(100),
	type: z.nativeEnum(QuestionType),
	cognitiveLevel: z.nativeEnum(CognitiveLevel),
	topicId: z.string().min(1, 'Topic is required'),
	isPublic: z.boolean(),
	mediaUrl: z.string().optional(),
	answers: z.array(answerSchema).min(2, 'At least 2 answers are required'),
})

type QuestionFormData = z.infer<typeof questionSchema>

const CreateQuestionPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const { examId } = useParams<{ examId: string }>()
	const { data: exam } = useExam(examId ?? '')
	const { data: matrix } = useMatrix(examId ?? '')
	const { data: topics = [] } = useTopics(exam?.subjectId ?? '')
	const createQuestionMutation = useCreateQuestion()
	const [isUploading, setIsUploading] = useState(false)

	const form = useForm<QuestionFormData>({
		resolver: zodResolver(questionSchema),
		defaultValues: {
			content: '',
			point: 1,
			type: QuestionType.MultipleChoice,
			cognitiveLevel: CognitiveLevel.Remember,
			topicId: '',
			isPublic: true,
			mediaUrl: '',
			answers: [
				{ content: '', isCorrect: false, explanation: '' },
				{ content: '', isCorrect: false, explanation: '' },
			],
		},
	})

	const { fields, append, remove } = useFieldArray({
		control: form.control,
		name: 'answers',
	})

	const questionType = form.watch('type')

	// Handle single correct answer for multiple choice and true/false
	const handleCorrectAnswerChange = (index: number, isChecked: boolean) => {
		if (
			questionType === QuestionType.MultipleChoice ||
			questionType === QuestionType.TrueFalse
		) {
			// Only one answer can be correct
			const updatedAnswers = form.getValues('answers').map((ans, i) => ({
				...ans,
				isCorrect: i === index ? isChecked : false,
			}))
			form.setValue('answers', updatedAnswers)
		} else {
			// Fill in the blank allows multiple correct answers
			form.setValue(`answers.${index}.isCorrect`, isChecked)
		}
	}

	// Update answers when question type changes
	useEffect(() => {
		const currentAnswers = form.getValues('answers')

		if (questionType === QuestionType.TrueFalse) {
			// True/False: exactly 2 answers (True and False)
			if (currentAnswers.length !== 2 || currentAnswers[0].content !== 'True') {
				form.setValue('answers', [
					{ content: 'True', isCorrect: false, explanation: '' },
					{ content: 'False', isCorrect: false, explanation: '' },
				])
			}
		} else if (questionType === QuestionType.Essay) {
			// Essay: single grading rubric field
			if (currentAnswers.length !== 1 || currentAnswers[0].content === 'True') {
				form.setValue('answers', [
					{
						content: '',
						isCorrect: true,
						explanation: '',
					},
				])
			}
		} else if (questionType === QuestionType.FillInTheBlank) {
			// Fill in blank: at least one acceptable answer
			if (currentAnswers.length === 0) {
				form.setValue('answers', [
					{ content: '', isCorrect: true, explanation: '' },
				])
			}
		} else if (questionType === QuestionType.MultipleChoice) {
			// Multiple choice: at least 2 options
			if (currentAnswers.length < 2 || currentAnswers[0].content === 'True') {
				form.setValue('answers', [
					{ content: '', isCorrect: false, explanation: '' },
					{ content: '', isCorrect: false, explanation: '' },
				])
			}
		}
	}, [questionType])

	const onSubmit = async (data: QuestionFormData) => {
		try {
			await createQuestionMutation.mutateAsync({
				content: data.content,
				point: data.point,
				type: data.type,
				cognitiveLevel: data.cognitiveLevel,
				topicId: data.topicId,
				isPublic: data.isPublic,
				mediaUrl: data.mediaUrl || undefined,
				source: 0, // QuestionSource.Manual
				answers: data.answers,
			})

			navigate(`/app/exams/${examId}/questions`)
		} catch (error) {
			console.error('Failed to create question:', error)
		}
	}

	const handleMediaUpload = async (
		event: React.ChangeEvent<HTMLInputElement>
	) => {
		const file = event.target.files?.[0]
		if (!file) return

		setIsUploading(true)
		try {
			// TODO: Implement actual file upload to S3
			// For now, just simulate upload
			await new Promise(resolve => setTimeout(resolve, 1000))
			const mockUrl = `https://example.com/media/${file.name}`
			form.setValue('mediaUrl', mockUrl)
		} catch (error) {
			console.error('Upload failed:', error)
		} finally {
			setIsUploading(false)
		}
	}

	const addAnswer = () => {
		const defaultCorrect = questionType === QuestionType.FillInTheBlank
		append({ content: '', isCorrect: defaultCorrect, explanation: '' })
	}

	return (
		<div className='p-6 max-w-[1600px] mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between mb-6'>
				<div className='flex items-center space-x-4'>
					<Button
						variant='ghost'
						size='icon'
						onClick={() => navigate(`/app/exams/${examId}/questions`)}
					>
						<ArrowLeft className='h-5 w-5' />
					</Button>
					<div>
						<h1 className='text-3xl font-bold'>Create Question</h1>
						<p className='text-muted-foreground'>
							Add a new question to the bank
						</p>
					</div>
				</div>
			</div>

			<div className='grid grid-cols-1 lg:grid-cols-3 gap-6'>
				{/* Main Form - Left/Center Column */}
				<div className='lg:col-span-2 space-y-6'>
					<Form {...form}>
						<form onSubmit={form.handleSubmit(onSubmit)} className='space-y-6'>
							{/* Question Details Card */}
							<Card>
								<CardHeader>
									<CardTitle>Question Details</CardTitle>
								</CardHeader>
								<CardContent className='space-y-4'>
									<FormField
										control={form.control}
										name='content'
										render={({ field }) => (
											<FormItem>
												<FormLabel>Question Content *</FormLabel>
												<FormControl>
													<Textarea
														placeholder='Enter your question here...'
														rows={4}
														{...field}
													/>
												</FormControl>
												<FormMessage />
											</FormItem>
										)}
									/>

									<div className='grid grid-cols-2 gap-4'>
										<FormField
											control={form.control}
											name='type'
											render={({ field }) => (
												<FormItem>
													<FormLabel>Question Type *</FormLabel>
													<Select
														onValueChange={value =>
															field.onChange(Number(value))
														}
														defaultValue={String(field.value)}
													>
														<FormControl>
															<SelectTrigger>
																<SelectValue />
															</SelectTrigger>
														</FormControl>
														<SelectContent>
															<SelectItem value='0'>Multiple Choice</SelectItem>
															<SelectItem value='1'>True/False</SelectItem>
															<SelectItem value='2'>Essay</SelectItem>
															<SelectItem value='3'>Fill in Blank</SelectItem>
														</SelectContent>
													</Select>
													<FormMessage />
												</FormItem>
											)}
										/>

										<FormField
											control={form.control}
											name='cognitiveLevel'
											render={({ field }) => (
												<FormItem>
													<FormLabel>Cognitive Level *</FormLabel>
													<Select
														onValueChange={value =>
															field.onChange(Number(value))
														}
														defaultValue={String(field.value)}
													>
														<FormControl>
															<SelectTrigger>
																<SelectValue />
															</SelectTrigger>
														</FormControl>
														<SelectContent>
															<SelectItem value='0'>Remember</SelectItem>
															<SelectItem value='1'>Understand</SelectItem>
															<SelectItem value='2'>Apply</SelectItem>
															<SelectItem value='3'>Analyze</SelectItem>
														</SelectContent>
													</Select>
													<FormMessage />
												</FormItem>
											)}
										/>
									</div>

									<div className='grid grid-cols-2 gap-4'>
										<FormField
											control={form.control}
											name='point'
											render={({ field }) => (
												<FormItem>
													<FormLabel>Points *</FormLabel>
													<FormControl>
														<Input
															type='number'
															step='0.5'
															{...field}
															onChange={e =>
																field.onChange(Number(e.target.value))
															}
														/>
													</FormControl>
													<FormMessage />
												</FormItem>
											)}
										/>

										<FormField
											control={form.control}
											name='topicId'
											render={({ field }) => (
												<FormItem>
													<FormLabel>Topic *</FormLabel>
													<FormControl>
														<TopicSelector
															topics={[]}
															value={field.value}
															onValueChange={field.onChange}
															placeholder='Search and select a topic...'
														/>
													</FormControl>
													<FormDescription>
														Search by topic name or number (e.g., 1.1, 2.3)
													</FormDescription>
													<FormMessage />
												</FormItem>
											)}
										/>
									</div>

									<FormField
										control={form.control}
										name='isPublic'
										render={({ field }) => (
											<FormItem className='flex flex-row items-start space-x-3 space-y-0'>
												<FormControl>
													<Checkbox
														checked={field.value}
														onCheckedChange={field.onChange}
													/>
												</FormControl>
												<div className='space-y-1 leading-none'>
													<FormLabel>Make this question public</FormLabel>
													<FormDescription>
														Public questions can be used by other teachers
													</FormDescription>
												</div>
											</FormItem>
										)}
									/>

									{/* Media Upload */}
									<div className='space-y-2'>
										<FormLabel>Media (Optional)</FormLabel>
										<div className='flex gap-2'>
											<Input
												type='file'
												accept='image/*,video/*'
												onChange={handleMediaUpload}
												disabled={isUploading}
											/>
											<Button
												type='button'
												variant='outline'
												disabled={isUploading}
											>
												<Upload className='h-4 w-4 mr-2' />
												{isUploading ? 'Uploading...' : 'Upload'}
											</Button>
										</div>
										{form.watch('mediaUrl') && (
											<p className='text-sm text-muted-foreground'>
												File uploaded: {form.watch('mediaUrl')}
											</p>
										)}
									</div>
								</CardContent>
							</Card>

							{/* Answers Card */}
							<Card>
								<CardHeader className='flex flex-row items-center justify-between'>
									<CardTitle>
										{questionType === QuestionType.Essay
											? 'Grading Guidelines'
											: questionType === QuestionType.FillInTheBlank
												? 'Acceptable Answers'
												: 'Answer Options'}
									</CardTitle>
									{(questionType === QuestionType.MultipleChoice ||
										questionType === QuestionType.FillInTheBlank) && (
										<Button
											type='button'
											onClick={addAnswer}
											variant='outline'
											size='sm'
										>
											<Plus className='h-4 w-4 mr-2' />
											Add Answer
										</Button>
									)}
								</CardHeader>
								<CardContent className='space-y-4'>
									{fields.map((field, index) => (
										<Card key={field.id} className='border-2'>
											<CardContent className='pt-4 space-y-4'>
												<div className='flex items-start gap-4'>
													{questionType !== QuestionType.Essay && (
														<FormField
															control={form.control}
															name={`answers.${index}.isCorrect`}
															render={({ field }) => (
																<FormItem className='flex items-center space-x-2 pt-2'>
																	<FormControl>
																		<Checkbox
																			checked={field.value}
																			onCheckedChange={checked =>
																				handleCorrectAnswerChange(
																					index,
																					!!checked
																				)
																			}
																		/>
																	</FormControl>
																	<FormLabel className='!mt-0 font-normal'>
																		{questionType ===
																			QuestionType.MultipleChoice ||
																		questionType === QuestionType.TrueFalse
																			? 'Correct Answer'
																			: 'Correct'}
																	</FormLabel>
																</FormItem>
															)}
														/>
													)}

													<div className='flex-1 space-y-4'>
														<FormField
															control={form.control}
															name={`answers.${index}.content`}
															render={({ field }) => (
																<FormItem>
																	<FormLabel>
																		{questionType === QuestionType.Essay
																			? 'Grading Rubric'
																			: questionType ===
																				  QuestionType.FillInTheBlank
																				? `Acceptable Answer ${index + 1} *`
																				: `Answer ${index + 1} *`}
																	</FormLabel>
																	<FormControl>
																		{questionType === QuestionType.Essay ? (
																			<Textarea
																				placeholder='Describe the grading criteria...'
																				rows={4}
																				{...field}
																			/>
																		) : (
																			<Input
																				placeholder='Enter answer...'
																				disabled={
																					questionType ===
																					QuestionType.TrueFalse
																				}
																				{...field}
																			/>
																		)}
																	</FormControl>
																	<FormMessage />
																</FormItem>
															)}
														/>

														<FormField
															control={form.control}
															name={`answers.${index}.explanation`}
															render={({ field }) => (
																<FormItem>
																	<FormLabel>Explanation (Optional)</FormLabel>
																	<FormControl>
																		<Textarea
																			placeholder='Explain why this answer is correct/incorrect...'
																			rows={2}
																			{...field}
																		/>
																	</FormControl>
																</FormItem>
															)}
														/>
													</div>

													{questionType !== QuestionType.TrueFalse &&
														questionType !== QuestionType.Essay && (
															<Button
																type='button'
																variant='ghost'
																size='icon'
																onClick={() => {
																	if (
																		questionType ===
																			QuestionType.MultipleChoice &&
																		fields.length <= 2
																	) {
																		return
																	}
																	if (
																		questionType ===
																			QuestionType.FillInTheBlank &&
																		fields.length <= 1
																	) {
																		return
																	}
																	remove(index)
																}}
																disabled={
																	(questionType ===
																		QuestionType.MultipleChoice &&
																		fields.length <= 2) ||
																	(questionType ===
																		QuestionType.FillInTheBlank &&
																		fields.length <= 1)
																}
																title={
																	questionType ===
																		QuestionType.MultipleChoice &&
																	fields.length <= 2
																		? 'Multiple choice needs at least 2 answers'
																		: questionType ===
																					QuestionType.FillInTheBlank &&
																			  fields.length <= 1
																			? 'At least one answer is required'
																			: 'Remove answer'
																}
															>
																<Trash2 className='h-4 w-4 text-destructive' />
															</Button>
														)}
												</div>
											</CardContent>
										</Card>
									))}
								</CardContent>
							</Card>

							{/* Submit Button */}
							<div className='flex justify-end gap-2'>
								<Button
									type='button'
									variant='outline'
									onClick={() => navigate(`/app/exams/${examId}/questions`)}
								>
									Cancel
								</Button>
								<Button
									type='submit'
									disabled={createQuestionMutation.isPending}
								>
									<Save className='h-4 w-4 mr-2' />
									{createQuestionMutation.isPending
										? 'Saving...'
										: 'Save Question'}
								</Button>
							</div>
						</form>
					</Form>
				</div>

				{/* Matrix Requirements - Right Sidebar */}
				{matrix && (
					<div className='lg:col-span-1'>
						<div className='sticky top-6'>
							<Card className='border-primary/20 bg-primary/5'>
								<CardHeader>
									<div className='flex items-start gap-3'>
										<Info className='h-5 w-5 text-primary mt-0.5' />
										<div className='flex-1'>
											<CardTitle className='text-base'>
												Matrix Requirements
											</CardTitle>
											<p className='text-sm text-muted-foreground mt-1'>
												Create questions based on your exam matrix
											</p>
										</div>
									</div>
								</CardHeader>
								<CardContent>
									<MatrixProgressTracker
										matrix={matrix}
										questions={[]}
										topics={topics}
									/>
								</CardContent>
							</Card>
						</div>
					</div>
				)}
			</div>
		</div>
	)
}

export default CreateQuestionPage
