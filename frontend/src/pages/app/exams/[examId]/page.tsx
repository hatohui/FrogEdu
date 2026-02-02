import React, { useState } from 'react'
import { useNavigate, useParams } from 'react-router'
import {
	ArrowLeft,
	Edit,
	Eye,
	Trash2,
	CheckCircle,
	Clock,
	Calendar,
	BookOpen,
	Settings,
	Send,
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
import { CognitiveLevel, QuestionType } from '@/types/model/exams'

// Mock data - replace with actual API call
const mockExam = {
	id: '1',
	title: 'Mid-term Math Exam',
	duration: 90,
	passScore: 60,
	maxAttempts: 2,
	startTime: '2024-03-15T08:00:00Z',
	endTime: '2024-03-15T09:30:00Z',
	isDraft: true,
	isActive: false,
	questionCount: 5,
	totalPoints: 10,
	topicId: 'topic-1',
	topicName: 'Algebra - Linear Equations',
	createdAt: '2024-03-01T10:00:00Z',
	updatedAt: null,
}

const mockQuestions = [
	{
		id: '1',
		content: 'What is the solution to the equation 2x + 5 = 13?',
		point: 2,
		type: QuestionType.MultipleChoice,
		cognitiveLevel: CognitiveLevel.Easy,
		answerCount: 4,
	},
	{
		id: '2',
		content: 'Solve for x: 3x - 7 = 2x + 5',
		point: 2,
		type: QuestionType.MultipleChoice,
		cognitiveLevel: CognitiveLevel.Medium,
		answerCount: 4,
	},
	{
		id: '3',
		content: 'Graph the linear equation y = 2x + 3',
		point: 3,
		type: QuestionType.Essay,
		cognitiveLevel: CognitiveLevel.Hard,
		answerCount: 0,
	},
	{
		id: '4',
		content: 'Is 2x + 3 = 2(x + 1) + 1 an identity?',
		point: 1,
		type: QuestionType.TrueFalse,
		cognitiveLevel: CognitiveLevel.Easy,
		answerCount: 2,
	},
	{
		id: '5',
		content:
			'Complete: The slope-intercept form of a linear equation is y = ___ + b',
		point: 2,
		type: QuestionType.FillInBlank,
		cognitiveLevel: CognitiveLevel.Medium,
		answerCount: 1,
	},
]

const ExamDetailPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const { examId } = useParams<{ examId: string }>()
	const [isPublishDialogOpen, setIsPublishDialogOpen] = useState(false)

	// TODO: Replace with actual API calls
	const exam = mockExam
	const questions = mockQuestions

	const handlePublish = async () => {
		// TODO: Implement publish logic
		console.log('Publishing exam:', examId)
		setIsPublishDialogOpen(false)
		// Navigate or show success message
	}

	const handleDelete = async (questionId: string) => {
		// TODO: Implement delete logic
		console.log('Deleting question:', questionId)
	}

	const getCognitiveLevelLabel = (level: CognitiveLevel) => {
		switch (level) {
			case CognitiveLevel.Easy:
				return 'Easy'
			case CognitiveLevel.Medium:
				return 'Medium'
			case CognitiveLevel.Hard:
				return 'Hard'
			default:
				return 'Unknown'
		}
	}

	const getCognitiveLevelColor = (level: CognitiveLevel) => {
		switch (level) {
			case CognitiveLevel.Easy:
				return 'bg-green-500/10 text-green-700 dark:text-green-400'
			case CognitiveLevel.Medium:
				return 'bg-yellow-500/10 text-yellow-700 dark:text-yellow-400'
			case CognitiveLevel.Hard:
				return 'bg-red-500/10 text-red-700 dark:text-red-400'
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
			case QuestionType.FillInBlank:
				return 'Fill in Blank'
			default:
				return 'Unknown'
		}
	}

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
							<h1 className='text-3xl font-bold'>{exam.title}</h1>
							{exam.isDraft ? (
								<Badge variant='secondary'>
									<BookOpen className='h-3 w-3 mr-1' />
									Draft
								</Badge>
							) : (
								<Badge variant='default'>
									<CheckCircle className='h-3 w-3 mr-1' />
									Published
								</Badge>
							)}
						</div>
						<p className='text-muted-foreground'>
							{exam.questionCount} questions • {exam.totalPoints} points total
						</p>
					</div>
				</div>
				<div className='flex gap-2'>
					<Button
						variant='outline'
						onClick={() => navigate(`/app/exams/${examId}/edit`)}
					>
						<Settings className='h-4 w-4 mr-2' />
						Settings
					</Button>
					{exam.isDraft && (
						<Dialog
							open={isPublishDialogOpen}
							onOpenChange={setIsPublishDialogOpen}
						>
							<DialogTrigger asChild>
								<Button>
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
									<Button onClick={handlePublish}>Publish</Button>
								</DialogFooter>
							</DialogContent>
						</Dialog>
					)}
				</div>
			</div>

			{/* Exam Info Cards */}
			<div className='grid grid-cols-1 md:grid-cols-3 gap-4'>
				<Card>
					<CardContent className='pt-6'>
						<div className='flex items-center space-x-3'>
							<div className='p-2 bg-primary/10 rounded-lg'>
								<Clock className='h-5 w-5 text-primary' />
							</div>
							<div>
								<p className='text-sm text-muted-foreground'>Duration</p>
								<p className='text-2xl font-bold'>{exam.duration} min</p>
							</div>
						</div>
					</CardContent>
				</Card>

				<Card>
					<CardContent className='pt-6'>
						<div className='flex items-center space-x-3'>
							<div className='p-2 bg-primary/10 rounded-lg'>
								<CheckCircle className='h-5 w-5 text-primary' />
							</div>
							<div>
								<p className='text-sm text-muted-foreground'>Pass Score</p>
								<p className='text-2xl font-bold'>{exam.passScore}%</p>
							</div>
						</div>
					</CardContent>
				</Card>

				<Card>
					<CardContent className='pt-6'>
						<div className='flex items-center space-x-3'>
							<div className='p-2 bg-primary/10 rounded-lg'>
								<Calendar className='h-5 w-5 text-primary' />
							</div>
							<div>
								<p className='text-sm text-muted-foreground'>Max Attempts</p>
								<p className='text-2xl font-bold'>{exam.maxAttempts}</p>
							</div>
						</div>
					</CardContent>
				</Card>
			</div>

			{/* Exam Details */}
			<Card>
				<CardHeader>
					<CardTitle>Exam Details</CardTitle>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='grid grid-cols-2 gap-4'>
						<div>
							<p className='text-sm text-muted-foreground'>Start Time</p>
							<p className='font-medium'>
								{new Date(exam.startTime).toLocaleString()}
							</p>
						</div>
						<div>
							<p className='text-sm text-muted-foreground'>End Time</p>
							<p className='font-medium'>
								{new Date(exam.endTime).toLocaleString()}
							</p>
						</div>
						<div>
							<p className='text-sm text-muted-foreground'>Topic</p>
							<p className='font-medium'>{exam.topicName}</p>
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

			{/* Questions List */}
			<Card>
				<CardHeader className='flex flex-row items-center justify-between'>
					<CardTitle>Questions ({questions.length})</CardTitle>
					<Button
						variant='outline'
						onClick={() => navigate(`/app/exams/${examId}/questions`)}
					>
						Manage Questions
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
