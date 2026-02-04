import React, { useState, useCallback } from 'react'
import { useNavigate, useParams } from 'react-router'
import {
	ArrowLeft,
	Save,
	Sparkles,
	PenLine,
	Crown,
	Loader2,
	Library,
	Plus,
} from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Form } from '@/components/ui/form'
import { Badge } from '@/components/ui/badge'
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group'
import { Label } from '@/components/ui/label'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import {
	useCreateQuestion,
	useMatrix,
	useExam,
	useTopics,
	useExamQuestions,
	useQuestions,
	useAddQuestionsToExam,
	useRemoveQuestionFromExam,
} from '@/hooks/useExams'
import { useSubscription } from '@/hooks/useSubscription'
import { useAIQuestionGeneration } from '@/hooks/useAIQuestionGeneration'
import { Info } from 'lucide-react'
import { MatrixProgressTracker } from '@/components/exams/MatrixProgressTracker'
import { ExamQuestionsPanel } from '@/components/exams/ExamQuestionsPanel'
import { QuestionBankDialog } from '@/components/exams/QuestionBankDialog'
import { toast } from 'sonner'

// Local components
import { QuestionFormFields } from './components/QuestionFormFields'
import { QuestionAnswersSection } from './components/QuestionAnswersSection'
import { AIGeneratorSection } from './components/AIGeneratorSection'
import { useQuestionForm, type QuestionFormData } from '@/hooks/useQuestionForm'
import type { AIGeneratedQuestion } from '@/types/model/ai-service'
import type { CognitiveLevel } from '@/types/model/exam-service'
import examService from '@/services/exam-microservice/exam.service'
import {
	validateMatrixProgress,
	wouldExceedMatrixLimit,
} from '@/utils/matrixValidation'

type CreationMode = 'manual' | 'ai'
type MainTab = 'create' | 'bank'

/**
 * Page for creating new questions
 * Supports manual creation, AI generation, and adding from question bank
 */
const CreateQuestionPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const { examId } = useParams<{ examId: string }>()
	const { data: exam } = useExam(examId ?? '')
	const { data: matrix } = useMatrix(examId ?? '')
	const { data: topics = [] } = useTopics(exam?.subjectId ?? '')
	const { data: existingQuestions = [], refetch: refetchExamQuestions } =
		useExamQuestions(examId ?? '')
	const { data: allQuestions = [], isLoading: isLoadingQuestions } =
		useQuestions({ isPublic: true })
	const createQuestionMutation = useCreateQuestion(examId)
	const addQuestionsMutation = useAddQuestionsToExam()
	const removeQuestionMutation = useRemoveQuestionFromExam()
	const { isPro } = useSubscription()
	const {
		isGenerating,
		generatedQuestions,
		generateSingle,
		removeGeneratedQuestion,
		clearGeneratedQuestions,
	} = useAIQuestionGeneration()

	const [mainTab, setMainTab] = useState<MainTab>('create')
	const [creationMode, setCreationMode] = useState<CreationMode>('manual')
	const [selectedAiTopic, setSelectedAiTopic] = useState('')
	const [showBankDialog, setShowBankDialog] = useState(false)

	// Track existing question IDs for quick lookup
	const existingQuestionIds = new Set(existingQuestions.map(q => q.id))

	// Matrix validation
	const matrixValidation = validateMatrixProgress(
		matrix,
		existingQuestions,
		topics
	)

	// Use our custom form hook
	const {
		form,
		fields,
		questionType,
		handleCorrectAnswerChange,
		addAnswer,
		removeAnswer,
		loadAIQuestion,
		resetForm,
		append,
		remove,
	} = useQuestionForm()

	// Handle clicking on a matrix requirement to pre-fill the form
	const handleMatrixRequirementClick = useCallback(
		(topicId: string, cognitiveLevel: CognitiveLevel) => {
			form.setValue('topicId', topicId)
			form.setValue('cognitiveLevel', cognitiveLevel)
			setMainTab('create')
			setCreationMode('manual')
			toast.info('Form pre-filled with matrix requirement')
		},
		[form]
	)

	// Handle form submission
	const onSubmit = async (data: QuestionFormData) => {
		if (!data || !examId) return

		// Check if question would exceed matrix limit
		if (matrix) {
			const wouldExceed = wouldExceedMatrixLimit(
				{ topicId: data.topicId, cognitiveLevel: data.cognitiveLevel },
				matrix,
				existingQuestions
			)

			if (wouldExceed) {
				const proceed = window.confirm(
					'This question exceeds the matrix requirements for this topic and cognitive level. Do you want to add it anyway?'
				)
				if (!proceed) return
			}
		}

		try {
			const result = await createQuestionMutation.mutateAsync({
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

			// Automatically associate the question with the exam
			if (result.data?.id) {
				await examService.addQuestionsToExam(examId, [result.data.id])
				// Refresh exam questions to show the new one
				refetchExamQuestions()
			}

			// Reset form for creating another question
			resetForm()
			toast.success('Question created and added to exam!')
		} catch (error) {
			console.error('Failed to create question:', error)
		}
	}

	// Edit AI generated question
	const handleEditGeneratedQuestion = (question: AIGeneratedQuestion) => {
		loadAIQuestion(question)
		setCreationMode('manual')
	}

	// Save AI generated question directly
	const handleSaveGeneratedQuestion = async (
		question: AIGeneratedQuestion,
		index: number
	) => {
		if (!examId) return

		try {
			const result = await createQuestionMutation.mutateAsync({
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

			if (result.data?.id) {
				await examService.addQuestionsToExam(examId, [result.data.id])
				refetchExamQuestions()
			}

			removeGeneratedQuestion(index)
		} catch (error) {
			console.error('Failed to save question:', error)
		}
	}

	// Handle adding questions from bank
	const handleAddFromBank = async (questionIds: string[]) => {
		if (!examId) return

		await addQuestionsMutation.mutateAsync({
			examId,
			questionIds,
		})
		refetchExamQuestions()
	}

	// Handle removing question from exam
	const handleRemoveQuestion = async (questionId: string) => {
		if (!examId) return

		await removeQuestionMutation.mutateAsync({
			examId,
			questionId,
		})
	}

	// Handle viewing question
	const handleViewQuestion = (questionId: string) => {
		navigate(`/app/exams/${examId}/questions/${questionId}`)
	}

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between mb-6'>
				<div className='flex items-center space-x-4'>
					<Button
						variant='ghost'
						size='icon'
						onClick={() => navigate(`/app/exams/${examId}`)}
					>
						<ArrowLeft className='h-5 w-5' />
					</Button>
					<div>
						<h1 className='text-3xl font-bold'>Manage Questions</h1>
						<p className='text-muted-foreground'>
							Add questions to your exam {exam?.name ? `- ${exam.name}` : ''}
						</p>
					</div>
				</div>
				{matrixValidation && (
					<Badge
						variant={matrixValidation.isComplete ? 'default' : 'secondary'}
						className='text-sm'
					>
						{matrixValidation.totalFulfilled} / {matrixValidation.totalRequired}{' '}
						questions
					</Badge>
				)}
			</div>

			{/* Main Tabs: Create vs Add from Bank */}
			<Tabs value={mainTab} onValueChange={v => setMainTab(v as MainTab)}>
				<TabsList className='grid w-full max-w-md grid-cols-2'>
					<TabsTrigger value='create' className='flex items-center gap-2'>
						<Plus className='h-4 w-4' />
						Create from Scratch
					</TabsTrigger>
					<TabsTrigger value='bank' className='flex items-center gap-2'>
						<Library className='h-4 w-4' />
						Add from Bank
					</TabsTrigger>
				</TabsList>

				<div className='grid grid-cols-1 lg:grid-cols-3 gap-6 mt-6'>
					{/* Main Content - Left Side */}
					<div className='lg:col-span-2 space-y-6'>
						<TabsContent value='create' className='mt-0 space-y-6'>
							{/* Creation Mode Toggle */}
							<Card>
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

							{creationMode === 'ai' ? (
								<AIGeneratorSection
									exam={exam}
									topics={topics}
									selectedTopic={selectedAiTopic}
									onTopicChange={setSelectedAiTopic}
									isGenerating={isGenerating}
									generatedQuestions={generatedQuestions}
									onGenerate={generateSingle}
									onEditQuestion={handleEditGeneratedQuestion}
									onSaveQuestion={handleSaveGeneratedQuestion}
									onRemoveQuestion={removeGeneratedQuestion}
									onClearAll={clearGeneratedQuestions}
									isSaving={createQuestionMutation.isPending}
								/>
							) : (
								<Form {...form}>
									<form
										onSubmit={form.handleSubmit(onSubmit)}
										className='space-y-6'
									>
										{/* Question Details */}
										<QuestionFormFields form={form} topics={topics} />

										{/* Answers Section */}
										<QuestionAnswersSection
											form={form}
											fields={fields}
											questionType={questionType}
											onCorrectAnswerChange={handleCorrectAnswerChange}
											onAddAnswer={addAnswer}
											onRemoveAnswer={removeAnswer}
											append={append}
											remove={remove}
										/>

										{/* Submit Button */}
										<div className='flex justify-end gap-2'>
											<Button
												type='button'
												variant='outline'
												onClick={resetForm}
											>
												Clear
											</Button>
											<Button
												type='submit'
												disabled={createQuestionMutation.isPending}
											>
												{createQuestionMutation.isPending ? (
													<>
														<Loader2 className='h-4 w-4 mr-2 animate-spin' />
														Saving...
													</>
												) : (
													<>
														<Save className='h-4 w-4 mr-2' />
														Save & Add to Exam
													</>
												)}
											</Button>
										</div>
									</form>
								</Form>
							)}
						</TabsContent>

						<TabsContent value='bank' className='mt-0'>
							<Card>
								<CardHeader>
									<CardTitle className='flex items-center gap-2'>
										<Library className='h-5 w-5' />
										Question Bank
									</CardTitle>
								</CardHeader>
								<CardContent className='space-y-4'>
									<p className='text-sm text-muted-foreground'>
										Browse and add existing questions from your question bank.
										{matrix &&
											' Questions will be filtered to match your matrix requirements.'}
									</p>
									<Button onClick={() => setShowBankDialog(true)}>
										<Plus className='h-4 w-4 mr-2' />
										Browse Question Bank
									</Button>
								</CardContent>
							</Card>
						</TabsContent>
					</div>

					{/* Right Sidebar */}
					<div className='lg:col-span-1 space-y-6'>
						{/* Exam Questions Panel */}
						<Card>
							<CardHeader className='py-3 px-4'>
								<CardTitle className='text-sm font-medium'>
									Questions in this Exam
								</CardTitle>
							</CardHeader>
							<CardContent className='pt-0'>
								<ExamQuestionsPanel
									questions={existingQuestions}
									topics={topics}
									matrix={matrix}
									onRemoveQuestion={handleRemoveQuestion}
									onViewQuestion={handleViewQuestion}
									isRemoving={removeQuestionMutation.isPending}
								/>
							</CardContent>
						</Card>

						{/* Matrix Requirements */}
						{matrix && (
							<Card className='border-primary/20 bg-primary/5'>
								<CardHeader className='py-3 px-4'>
									<div className='flex items-start gap-3'>
										<Info className='h-5 w-5 text-primary mt-0.5' />
										<div className='flex-1'>
											<CardTitle className='text-sm font-medium'>
												Matrix Requirements
											</CardTitle>
											<p className='text-xs text-muted-foreground mt-1'>
												Click a requirement to pre-fill the form
											</p>
										</div>
									</div>
								</CardHeader>
								<CardContent className='pt-0'>
									<MatrixProgressTracker
										matrix={matrix}
										questions={existingQuestions}
										topics={topics}
										onRequirementClick={handleMatrixRequirementClick}
										interactive
										compact
									/>
								</CardContent>
							</Card>
						)}
					</div>
				</div>
			</Tabs>

			{/* Question Bank Dialog */}
			<QuestionBankDialog
				open={showBankDialog}
				onOpenChange={setShowBankDialog}
				allQuestions={allQuestions}
				existingQuestionIds={existingQuestionIds}
				existingQuestions={existingQuestions}
				topics={topics}
				matrix={matrix}
				onAddQuestions={handleAddFromBank}
				isLoading={isLoadingQuestions}
				isAdding={addQuestionsMutation.isPending}
			/>
		</div>
	)
}

export default CreateQuestionPage
