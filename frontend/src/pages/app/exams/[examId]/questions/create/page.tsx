import React, { useState, useEffect } from 'react'
import { useNavigate, useParams } from 'react-router'
import { useForm, useFieldArray } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import * as z from 'zod'
import {
	ArrowLeft,
	Plus,
	Trash2,
	Upload,
	Save,
	Sparkles,
	PenLine,
	Crown,
	Loader2,
	Check,
	X,
} from 'lucide-react'
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
import { Badge } from '@/components/ui/badge'
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group'
import { Label } from '@/components/ui/label'
import { Slider } from '@/components/ui/slider'
import {
	useCreateQuestion,
	useMatrix,
	useExam,
	useTopics,
} from '@/hooks/useExams'
import { useSubscription } from '@/hooks/useSubscription'
import { useAIQuestionGeneration } from '@/hooks/useAIQuestionGeneration'
import { useMediaUpload } from '@/hooks/image/useMediaUpload'
import { QuestionType, CognitiveLevel } from '@/types/model/exam-service'
import type { AIGeneratedQuestion } from '@/types/model/ai-service'
import { TopicSelector } from '@/components/exams/topic-selector'
import { MatrixProgressTracker } from '@/components/exams/MatrixProgressTracker'
import { Info } from 'lucide-react'

type CreationMode = 'manual' | 'ai'

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
	const { isPro } = useSubscription()
	const {
		isGenerating,
		generatedQuestions,
		generateSingle,
		removeGeneratedQuestion,
		clearGeneratedQuestions,
	} = useAIQuestionGeneration()
	const {
		isUploading,
		preview: mediaPreview,
		handleFileChange,
		clearPreview,
	} = useMediaUpload()

	const [creationMode, setCreationMode] = useState<CreationMode>('manual')
	const [aiQuantity, setAiQuantity] = useState(1)
	const [selectedAiTopic, setSelectedAiTopic] = useState('')
	const [aiQuestionType, setAiQuestionType] = useState<QuestionType>(
		QuestionType.MultipleChoice
	)
	const [aiCognitiveLevel, setAiCognitiveLevel] = useState<CognitiveLevel>(
		CognitiveLevel.Remember
	)

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
			// Fill in blank: exactly one answer (exact word match)
			if (currentAnswers.length !== 1 || currentAnswers[0].content === 'True') {
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

	/**
	 * Handle media file upload for question
	 * Uses the reusable useMediaUpload hook with proper validation
	 */
	const handleMediaUpload = handleFileChange(async (file: File) => {
		// TODO: Replace with actual S3 presigned URL upload when backend is ready
		// For now, generate a placeholder URL based on file name
		// The actual URL will be returned once exam service supports media upload
		return `https://cdn.frogedu.com/questions/media/${encodeURIComponent(file.name)}`
	})

	const onMediaFileChange = async (
		event: React.ChangeEvent<HTMLInputElement>
	) => {
		const result = await handleMediaUpload(event)
		if (result.success && result.url) {
			form.setValue('mediaUrl', result.url)
		}
	}

	const addAnswer = () => {
		const defaultCorrect = questionType === QuestionType.FillInTheBlank
		append({ content: '', isCorrect: defaultCorrect, explanation: '' })
	}

	// AI Generation handler
	const handleAIGenerate = async () => {
		if (!exam || !selectedAiTopic) return

		const selectedTopicData = topics.find(t => t.id === selectedAiTopic)
		if (!selectedTopicData) return

		for (let i = 0; i < aiQuantity; i++) {
			await generateSingle(
				exam.name ?? 'General',
				exam.grade ?? 10,
				selectedTopicData.title,
				aiCognitiveLevel,
				aiQuestionType,
				'vi',
				selectedTopicData.description
			)
		}
	}

	// Use AI generated question - populate form with it
	const useGeneratedQuestion = (question: AIGeneratedQuestion) => {
		form.setValue('content', question.content)
		form.setValue('type', question.questionType)
		form.setValue('cognitiveLevel', question.cognitiveLevel)
		form.setValue('point', question.point)
		form.setValue(
			'answers',
			question.answers.map(a => ({
				content: a.content,
				isCorrect: a.isCorrect,
				explanation: a.explanation ?? '',
			}))
		)
		if (question.topicId) {
			form.setValue('topicId', question.topicId)
		}
		setCreationMode('manual') // Switch to manual to edit/save
	}

	// Save AI generated question directly
	const saveGeneratedQuestion = async (
		question: AIGeneratedQuestion,
		index: number
	) => {
		try {
			await createQuestionMutation.mutateAsync({
				content: question.content,
				point: question.point,
				type: question.questionType,
				cognitiveLevel: question.cognitiveLevel,
				topicId: selectedAiTopic || question.topicId || '',
				isPublic: true,
				source: 1, // QuestionSource.AIGenerated
				answers: question.answers.map(a => ({
					content: a.content,
					isCorrect: a.isCorrect,
					explanation: a.explanation ?? '',
				})),
			})
			removeGeneratedQuestion(index)
		} catch (error) {
			console.error('Failed to save question:', error)
		}
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

			{/* Creation Mode Toggle */}
			<Card className='mb-6'>
				<CardContent className='pt-6'>
					<div className='flex items-center gap-4'>
						<span className='text-sm font-medium'>Creation Mode:</span>
						<RadioGroup
							value={creationMode}
							onValueChange={v => setCreationMode(v as CreationMode)}
							className='flex gap-4'
						>
							<div className='flex items-center space-x-2'>
								<RadioGroupItem value='manual' id='manual' />
								<Label
									htmlFor='manual'
									className='flex items-center gap-2 cursor-pointer'
								>
									<PenLine className='h-4 w-4' />
									Manual
								</Label>
							</div>
							<div className='flex items-center space-x-2'>
								<RadioGroupItem value='ai' id='ai' disabled={!isPro} />
								<Label
									htmlFor='ai'
									className={`flex items-center gap-2 cursor-pointer ${!isPro ? 'opacity-50' : ''}`}
								>
									<Sparkles className='h-4 w-4' />
									AI Generate
									{isPro ? (
										<Badge className='bg-gradient-to-r from-amber-500 to-orange-500 text-white text-xs'>
											<Crown className='h-3 w-3 mr-1' />
											Pro
										</Badge>
									) : (
										<Badge variant='secondary' className='text-xs'>
											Pro Required
										</Badge>
									)}
								</Label>
							</div>
						</RadioGroup>
					</div>
				</CardContent>
			</Card>

			<div className='grid grid-cols-1 lg:grid-cols-3 gap-6'>
				{/* Main Content - Left/Center Column */}
				<div className='lg:col-span-2 space-y-6'>
					{creationMode === 'ai' ? (
						<>
							{/* AI Generation Card */}
							<Card>
								<CardHeader>
									<CardTitle className='flex items-center gap-2'>
										<Sparkles className='h-5 w-5 text-amber-500' />
										AI Question Generator
									</CardTitle>
								</CardHeader>
								<CardContent className='space-y-4'>
									<div className='grid grid-cols-2 gap-4'>
										<div className='space-y-2'>
											<Label>Topic *</Label>
											<TopicSelector
												topics={topics}
												value={selectedAiTopic}
												onValueChange={setSelectedAiTopic}
												placeholder='Select a topic...'
											/>
										</div>
										<div className='space-y-2'>
											<Label>Question Type</Label>
											<Select
												value={String(aiQuestionType)}
												onValueChange={v =>
													setAiQuestionType(Number(v) as QuestionType)
												}
											>
												<SelectTrigger>
													<SelectValue />
												</SelectTrigger>
												<SelectContent>
													<SelectItem value='0'>Multiple Choice</SelectItem>
													<SelectItem value='1'>True/False</SelectItem>
													<SelectItem value='2'>Essay</SelectItem>
													<SelectItem value='3'>Fill in Blank</SelectItem>
												</SelectContent>
											</Select>
										</div>
									</div>

									<div className='grid grid-cols-2 gap-4'>
										<div className='space-y-2'>
											<Label>Cognitive Level</Label>
											<Select
												value={String(aiCognitiveLevel)}
												onValueChange={v =>
													setAiCognitiveLevel(Number(v) as CognitiveLevel)
												}
											>
												<SelectTrigger>
													<SelectValue />
												</SelectTrigger>
												<SelectContent>
													<SelectItem value='0'>Remember</SelectItem>
													<SelectItem value='1'>Understand</SelectItem>
													<SelectItem value='2'>Apply</SelectItem>
													<SelectItem value='3'>Analyze</SelectItem>
												</SelectContent>
											</Select>
										</div>
										<div className='space-y-2'>
											<Label>Quantity: {aiQuantity}</Label>
											<Slider
												value={[aiQuantity]}
												onValueChange={([v]) => setAiQuantity(v)}
												min={1}
												max={5}
												step={1}
												className='mt-2'
											/>
										</div>
									</div>

									<Button
										onClick={handleAIGenerate}
										disabled={isGenerating || !selectedAiTopic}
										className='w-full bg-gradient-to-r from-amber-500 to-orange-500 hover:from-amber-600 hover:to-orange-600'
									>
										{isGenerating ? (
											<>
												<Loader2 className='h-4 w-4 mr-2 animate-spin' />
												Generating...
											</>
										) : (
											<>
												<Sparkles className='h-4 w-4 mr-2' />
												Generate {aiQuantity} Question
												{aiQuantity > 1 ? 's' : ''}
											</>
										)}
									</Button>
								</CardContent>
							</Card>

							{/* Generated Questions Preview */}
							{generatedQuestions.length > 0 && (
								<Card>
									<CardHeader className='flex flex-row items-center justify-between'>
										<CardTitle>
											Generated Questions ({generatedQuestions.length})
										</CardTitle>
										<Button
											variant='outline'
											size='sm'
											onClick={clearGeneratedQuestions}
										>
											Clear All
										</Button>
									</CardHeader>
									<CardContent className='space-y-4'>
										{generatedQuestions.map((q, index) => (
											<Card key={index} className='border-2'>
												<CardContent className='pt-4'>
													<div className='flex justify-between items-start gap-4'>
														<div className='flex-1'>
															<p className='font-medium mb-2'>{q.content}</p>
															<div className='flex gap-2 flex-wrap mb-2'>
																<Badge variant='outline'>
																	{
																		[
																			'Multiple Choice',
																			'True/False',
																			'Essay',
																			'Fill in Blank',
																		][q.questionType]
																	}
																</Badge>
																<Badge variant='outline'>
																	{
																		[
																			'Remember',
																			'Understand',
																			'Apply',
																			'Analyze',
																		][q.cognitiveLevel]
																	}
																</Badge>
																<Badge variant='secondary'>{q.point} pts</Badge>
															</div>
															<div className='text-sm text-muted-foreground'>
																{q.answers.length} answers •{' '}
																{q.answers.filter(a => a.isCorrect).length}{' '}
																correct
															</div>
														</div>
														<div className='flex gap-2'>
															<Button
																size='sm'
																variant='outline'
																onClick={() => useGeneratedQuestion(q)}
															>
																Edit
															</Button>
															<Button
																size='sm'
																onClick={() => saveGeneratedQuestion(q, index)}
																disabled={createQuestionMutation.isPending}
															>
																<Check className='h-4 w-4 mr-1' />
																Save
															</Button>
															<Button
																size='sm'
																variant='ghost'
																onClick={() => removeGeneratedQuestion(index)}
															>
																<X className='h-4 w-4' />
															</Button>
														</div>
													</div>
												</CardContent>
											</Card>
										))}
									</CardContent>
								</Card>
							)}
						</>
					) : (
						<Form {...form}>
							<form
								onSubmit={form.handleSubmit(onSubmit)}
								className='space-y-6'
							>
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
																<SelectItem value='0'>
																	Multiple Choice
																</SelectItem>
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
													onChange={onMediaFileChange}
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
											{mediaPreview && (
												<div className='mt-2 relative'>
													<img
														src={mediaPreview}
														alt='Preview'
														className='max-h-32 rounded border'
													/>
													<Button
														type='button'
														variant='ghost'
														size='icon'
														className='absolute top-1 right-1 h-6 w-6'
														onClick={() => {
															clearPreview()
															form.setValue('mediaUrl', '')
														}}
													>
														<X className='h-4 w-4' />
													</Button>
												</div>
											)}
											{form.watch('mediaUrl') && !mediaPreview && (
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
													? 'Correct Answer'
													: 'Answer Options'}
										</CardTitle>
										{questionType === QuestionType.MultipleChoice && (
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
														{questionType !== QuestionType.Essay &&
															questionType !== QuestionType.FillInTheBlank && (
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
																				? 'Grading Rubric / Notes'
																				: questionType ===
																					  QuestionType.FillInTheBlank
																					? 'Correct Answer (exact match) *'
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
																		<FormLabel>
																			Explanation (Optional)
																		</FormLabel>
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
															questionType !== QuestionType.Essay &&
															questionType !== QuestionType.FillInTheBlank && (
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
																		remove(index)
																	}}
																	disabled={
																		questionType ===
																			QuestionType.MultipleChoice &&
																		fields.length <= 2
																	}
																	title={
																		questionType ===
																			QuestionType.MultipleChoice &&
																		fields.length <= 2
																			? 'Multiple choice needs at least 2 answers'
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
					)}
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
