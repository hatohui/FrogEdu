import React, { useState } from 'react'
import { useNavigate, useParams } from 'react-router'
import {
	ArrowLeft,
	Save,
	Sparkles,
	PenLine,
	Crown,
	Loader2,
} from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Form } from '@/components/ui/form'
import { Badge } from '@/components/ui/badge'
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group'
import { Label } from '@/components/ui/label'
import {
	useCreateQuestion,
	useMatrix,
	useExam,
	useTopics,
	useExamQuestions,
} from '@/hooks/useExams'
import { useSubscription } from '@/hooks/useSubscription'
import { useAIQuestionGeneration } from '@/hooks/useAIQuestionGeneration'
import { Info } from 'lucide-react'
import { MatrixProgressTracker } from '@/components/exams/MatrixProgressTracker'

// Local components
import { QuestionFormFields } from './components/QuestionFormFields'
import { QuestionAnswersSection } from './components/QuestionAnswersSection'
import { AIGeneratorSection } from './components/AIGeneratorSection'
import { useQuestionForm, type QuestionFormData } from '@/hooks/useQuestionForm'
import type { AIGeneratedQuestion } from '@/types/model/ai-service'
import examService from '@/services/exam-microservice/exam.service'

type CreationMode = 'manual' | 'ai'

/**
 * Page for creating new questions
 * Supports manual creation and AI generation
 */
const CreateQuestionPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const { examId } = useParams<{ examId: string }>()
	const { data: exam } = useExam(examId ?? '')
	const { data: matrix } = useMatrix(examId ?? '')
	const { data: topics = [] } = useTopics(exam?.subjectId ?? '')
	const { data: existingQuestions = [] } = useExamQuestions(examId ?? '')
	const createQuestionMutation = useCreateQuestion()
	const { isPro } = useSubscription()
	const {
		isGenerating,
		generatedQuestions,
		generateSingle,
		removeGeneratedQuestion,
		clearGeneratedQuestions,
	} = useAIQuestionGeneration()

	const [creationMode, setCreationMode] = useState<CreationMode>('manual')
	const [selectedAiTopic, setSelectedAiTopic] = useState('')

	// Use our custom form hook
	const {
		form,
		fields,
		questionType,
		handleCorrectAnswerChange,
		addAnswer,
		removeAnswer,
		loadAIQuestion,
		append,
		remove,
	} = useQuestionForm()

	// Handle form submission
	const onSubmit = async (data: QuestionFormData) => {
		if (!data || !examId) return

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
			}

			navigate(`/app/exams/${examId}/questions`)
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
			}

			removeGeneratedQuestion(index)
		} catch (error) {
			console.error('Failed to save question:', error)
		}
	}

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
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
				{/* Main Content */}
				<div className='lg:col-span-2 space-y-6'>
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
										onClick={() => navigate(`/app/exams/${examId}/questions`)}
									>
										Cancel
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
												Save Question
											</>
										)}
									</Button>
								</div>
							</form>
						</Form>
					)}
				</div>

				{/* Matrix Requirements Sidebar */}
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
										questions={existingQuestions}
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
