import React, { useState } from 'react'
import { useNavigate, useParams } from 'react-router'
import {
	ArrowLeft,
	Edit,
	Eye,
	Trash2,
	CheckCircle,
	BookOpen,
	Settings,
	Send,
	Plus,
	Grid3x3,
} from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogFooter,
	DialogHeader,
	DialogTitle,
	DialogTrigger,
} from '@/components/ui/dialog'
import { CognitiveLevel, QuestionType } from '@/types/model/exam-service'
import {
	useExam,
	useExamQuestions,
	usePublishExam,
	useRemoveQuestionFromExam,
	useMatrix,
	useTopics,
	useSubjects,
} from '@/hooks/useExams'

const ExamDetailPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const { examId } = useParams<{ examId: string }>()
	const [isPublishDialogOpen, setIsPublishDialogOpen] = useState(false)

	const { data: exam, isLoading: isLoadingExam } = useExam(examId || '')
	const { data: questions = [], isLoading: isLoadingQuestions } =
		useExamQuestions(examId || '')
	const { data: matrix } = useMatrix(examId || '')
	const { data: topics = [] } = useTopics(exam?.subjectId ?? '')
	const { data: subjects = [] } = useSubjects(exam?.grade)
	const publishExamMutation = usePublishExam()
	const removeQuestionMutation = useRemoveQuestionFromExam()

	const handlePublish = async () => {
		if (!examId) return

		if (questions.length === 0) {
			alert('Cannot publish an exam with no questions')
			return
		}

		try {
			await publishExamMutation.mutateAsync(examId)
			setIsPublishDialogOpen(false)
		} catch (error) {
			console.error('Failed to publish exam:', error)
		}
	}

	const handleDelete = async (questionId: string) => {
		if (!examId) return

		if (
			confirm('Are you sure you want to remove this question from the exam?')
		) {
			try {
				await removeQuestionMutation.mutateAsync({ examId, questionId })
			} catch (error) {
				console.error('Failed to delete question:', error)
			}
		}
	}

	const getCognitiveLevelLabel = (level: CognitiveLevel) => {
		switch (level) {
			case CognitiveLevel.Remember:
				return 'Remember'
			case CognitiveLevel.Understand:
				return 'Understand'
			case CognitiveLevel.Apply:
				return 'Apply'
			case CognitiveLevel.Analyze:
				return 'Analyze'
			default:
				return 'Unknown'
		}
	}

	const getCognitiveLevelColor = (level: CognitiveLevel) => {
		switch (level) {
			case CognitiveLevel.Remember:
				return 'bg-blue-500/10 text-blue-700 dark:text-blue-400'
			case CognitiveLevel.Understand:
				return 'bg-cyan-500/10 text-cyan-700 dark:text-cyan-400'
			case CognitiveLevel.Apply:
				return 'bg-green-500/10 text-green-700 dark:text-green-400'
			case CognitiveLevel.Analyze:
				return 'bg-yellow-500/10 text-yellow-700 dark:text-yellow-400'
			default:
				return ''
		}
	}

	const getQuestionTypeLabel = (type: QuestionType) => {
		switch (type) {
			case QuestionType.MultipleChoice:
				return 'Multiple Choice'
			case QuestionType.TrueFalse:
				return 'True/False'
			case QuestionType.Essay:
				return 'Essay'
			case QuestionType.FillInTheBlank:
				return 'Fill in the Blank'
			default:
				return 'Unknown'
		}
	}

	if (isLoadingExam || isLoadingQuestions) {
		return (
			<div className='p-6 space-y-6 max-w-7xl mx-auto'>
				<div className='text-center py-12'>Loading...</div>
			</div>
		)
	}

	if (!exam) {
		return (
			<div className='p-6 space-y-6 max-w-7xl mx-auto'>
				<div className='text-center py-12'>
					<p className='text-muted-foreground mb-4'>Exam not found</p>
					<Button onClick={() => navigate('/app/exams')}>
						<ArrowLeft className='h-4 w-4 mr-2' />
						Back to Exams
					</Button>
				</div>
			</div>
		)
	}

	const totalPoints = questions.reduce((sum, q) => sum + (q.point || 0), 0)

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div className='flex items-center space-x-4'>
					<Button
						variant='ghost'
						size='icon'
						onClick={() => navigate('/app/exams')}
					>
						<ArrowLeft className='h-5 w-5' />
					</Button>
					<div>
						<div className='flex items-center gap-3'>
							<h1 className='text-3xl font-bold'>{exam.name}</h1>
							{exam.isDraft ? (
								<Badge variant='secondary'>
									<BookOpen className='h-3 w-3 mr-1' />
									Draft
								</Badge>
							) : (
								<Badge variant='default'>
									<CheckCircle className='h-3 w-3 mr-1' />
									Active
								</Badge>
							)}
						</div>
						<p className='text-muted-foreground'>
							{questions.length} questions • {totalPoints} points total
						</p>
					</div>
				</div>
				<div className='flex gap-2'>
					<Button
						variant='outline'
						onClick={() => navigate(`/app/exams/${examId}/edit`)}
					>
						<Settings className='h-4 w-4 mr-2' />
						Edit
					</Button>
					{exam.isDraft && (
						<Dialog
							open={isPublishDialogOpen}
							onOpenChange={setIsPublishDialogOpen}
						>
							<DialogTrigger asChild>
								<Button disabled={questions.length === 0}>
									<Send className='h-4 w-4 mr-2' />
									Publish Exam
								</Button>
							</DialogTrigger>
							<DialogContent>
								<DialogHeader>
									<DialogTitle>Publish Exam</DialogTitle>
									<DialogDescription>
										Are you sure you want to publish this exam? Students will be
										able to access it after publishing.
									</DialogDescription>
								</DialogHeader>
								<DialogFooter>
									<Button
										variant='outline'
										onClick={() => setIsPublishDialogOpen(false)}
									>
										Cancel
									</Button>
									<Button
										onClick={handlePublish}
										disabled={publishExamMutation.isPending}
									>
										{publishExamMutation.isPending
											? 'Publishing...'
											: 'Publish'}
									</Button>
								</DialogFooter>
							</DialogContent>
						</Dialog>
					)}
				</div>
			</div>

			{/* Exam Details */}
			<Card>
				<CardHeader>
					<CardTitle>Exam Information</CardTitle>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='grid grid-cols-2 gap-4'>
						<div>
							<p className='text-sm text-muted-foreground mb-1'>Description</p>
							<p className='font-medium'>{exam.description}</p>
						</div>
						<div>
							<p className='text-sm text-muted-foreground mb-1'>Subject</p>
							<p className='font-medium'>
								{subjects.find(s => s.id === exam.subjectId)?.name ||
									'Unknown Subject'}
							</p>
						</div>
					</div>

					<div className='grid grid-cols-2 gap-4'>
						<div>
							<p className='text-sm text-muted-foreground'>Grade</p>
							<p className='font-medium'>Grade {exam.grade}</p>
						</div>

						<div>
							<p className='text-sm text-muted-foreground'>Created</p>
							<p className='font-medium'>
								{new Date(exam.createdAt).toLocaleDateString()}
							</p>
						</div>
					</div>
				</CardContent>
			</Card>

			{/* Matrix Section */}
			<Card>
				<CardHeader className='flex flex-row items-center justify-between'>
					<div>
						<CardTitle>Exam Matrix</CardTitle>
						<p className='text-sm text-muted-foreground mt-1'>
							Define question distribution by topic and cognitive level
						</p>
					</div>
					{matrix ? (
						<Button
							variant='outline'
							onClick={() =>
								navigate(`/app/exams/create/matrix?examId=${examId}`)
							}
						>
							<Edit className='h-4 w-4 mr-2' />
							Edit Matrix
						</Button>
					) : (
						<Button
							onClick={() =>
								navigate(`/app/exams/create/matrix?examId=${examId}`)
							}
						>
							<Plus className='h-4 w-4 mr-2' />
							Create Matrix
						</Button>
					)}
				</CardHeader>
				<CardContent>
					{matrix ? (
						<div className='space-y-4'>
							<div className='flex items-center justify-between p-4 bg-muted rounded-lg'>
								<div className='flex items-center gap-2'>
									<Grid3x3 className='h-5 w-5 text-primary' />
									<div>
										<p className='font-medium'>Matrix Configured</p>
										<p className='text-sm text-muted-foreground'>
											{matrix.matrixTopics.length} topic-level combinations
										</p>
									</div>
								</div>
								<Badge variant='default'>
									{matrix.totalQuestionCount} questions total
								</Badge>
							</div>

							<div className='space-y-2'>
								{matrix.matrixTopics.slice(0, 5).map((mt, idx) => {
									const topic = topics.find(t => t.id === mt.topicId)
									return (
										<div
											key={idx}
											className='flex items-center justify-between p-3 border rounded-lg'
										>
											<div className='flex items-center gap-3'>
												<span className='text-sm font-medium'>
													{topic?.title || 'Unknown Topic'}
												</span>
												<Badge variant='outline'>
													{getCognitiveLevelLabel(mt.cognitiveLevel)}
												</Badge>
											</div>
											<span className='text-sm text-muted-foreground'>
												{mt.quantity} questions
											</span>
										</div>
									)
								})}
								{matrix.matrixTopics.length > 5 && (
									<p className='text-sm text-muted-foreground text-center py-2'>
										+ {matrix.matrixTopics.length - 5} more combinations
									</p>
								)}
							</div>
						</div>
					) : (
						<div className='text-center py-8'>
							<Grid3x3 className='h-12 w-12 text-muted-foreground mx-auto mb-4' />
							<p className='text-sm text-muted-foreground mb-4'>
								No matrix configured yet. Create a matrix to define the
								structure of your exam.
							</p>
							<Button
								onClick={() =>
									navigate(`/app/exams/create/matrix?examId=${examId}`)
								}
							>
								<Plus className='h-4 w-4 mr-2' />
								Create Matrix
							</Button>
						</div>
					)}
				</CardContent>
			</Card>

			{/* Questions List */}
			<Card>
				<CardHeader className='flex flex-row items-center justify-between'>
					<CardTitle>Questions ({questions.length})</CardTitle>
					<Button
						variant='outline'
						onClick={() => navigate(`/app/exams/${examId}/questions/create`)}
					>
						<Plus className='h-4 w-4 mr-2' />
						Add Questions
					</Button>
				</CardHeader>
				<CardContent>
					{questions.length > 0 ? (
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead className='w-12'>#</TableHead>
									<TableHead className='w-[50%]'>Question</TableHead>
									<TableHead>Type</TableHead>
									<TableHead>Level</TableHead>
									<TableHead>Points</TableHead>
									<TableHead className='text-right'>Actions</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{questions.map((question, index) => (
									<TableRow key={question.id}>
										<TableCell className='font-medium'>{index + 1}</TableCell>
										<TableCell>
											<div className='line-clamp-2'>{question.content}</div>
										</TableCell>
										<TableCell>
											<Badge variant='outline'>
												{getQuestionTypeLabel(question.type)}
											</Badge>
										</TableCell>
										<TableCell>
											<Badge
												className={getCognitiveLevelColor(
													question.cognitiveLevel
												)}
											>
												{getCognitiveLevelLabel(question.cognitiveLevel)}
											</Badge>
										</TableCell>
										<TableCell>{question.point}</TableCell>
										<TableCell className='text-right space-x-2'>
											<Button
												variant='ghost'
												size='icon'
												onClick={() =>
													navigate(
														`/app/exams/${examId}/questions/${question.id}/preview`
													)
												}
											>
												<Eye className='h-4 w-4' />
											</Button>
											<Button
												variant='ghost'
												size='icon'
												onClick={() =>
													navigate(
														`/app/exams/${examId}/questions/${question.id}/edit`
													)
												}
											>
												<Edit className='h-4 w-4' />
											</Button>
											<Button
												variant='ghost'
												size='icon'
												onClick={() => handleDelete(question.id)}
												disabled={removeQuestionMutation.isPending}
											>
												<Trash2 className='h-4 w-4 text-destructive' />
											</Button>
										</TableCell>
									</TableRow>
								))}
							</TableBody>
						</Table>
					) : (
						<div className='text-center py-12'>
							<BookOpen className='h-12 w-12 mx-auto text-muted-foreground mb-4' />
							<p className='text-muted-foreground mb-4'>
								No questions added yet
							</p>
							<Button
								onClick={() => navigate(`/app/exams/${examId}/questions`)}
							>
								<Plus className='h-4 w-4 mr-2' />
								Add Questions
							</Button>
						</div>
					)}
				</CardContent>
			</Card>
		</div>
	)
}

export default ExamDetailPage
