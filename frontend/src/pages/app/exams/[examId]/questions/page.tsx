import React, { useState } from 'react'
import { useNavigate, useParams } from 'react-router'
import {
	ArrowLeft,
	Plus,
	Search,
	Filter,
	Eye,
	Pencil,
	CheckCircle,
	MoreHorizontal,
	Trash2,
	AlertCircle,
	Sparkles,
} from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import {
	AlertDialog,
	AlertDialogAction,
	AlertDialogCancel,
	AlertDialogContent,
	AlertDialogDescription,
	AlertDialogFooter,
	AlertDialogHeader,
	AlertDialogTitle,
} from '@/components/ui/alert-dialog'
import { Checkbox } from '@/components/ui/checkbox'
import { Tabs, TabsList, TabsTrigger } from '@/components/ui/tabs'
import {
	Tooltip,
	TooltipContent,
	TooltipProvider,
	TooltipTrigger,
} from '@/components/ui/tooltip'
import {
	useQuestions,
	useDeleteQuestion,
	useAddQuestionsToExam,
	useMatrix,
	useExam,
	useTopics,
	useExamQuestions,
	useRemoveQuestionFromExam,
} from '@/hooks/useExams'
import { CognitiveLevel, QuestionType } from '@/types/model/exam-service'
import type { Question } from '@/types/model/exam-service'
import { MatrixProgressTracker } from '@/components/exams/MatrixProgressTracker'

const QuestionBankPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const { examId } = useParams<{ examId: string }>()

	const [searchQuery, setSearchQuery] = useState('')
	const [filterLevel, setFilterLevel] = useState<string>('all')
	const [filterTopic, setFilterTopic] = useState<string>('all')
	const [selectedQuestions, setSelectedQuestions] = useState<Set<string>>(
		new Set()
	)
	const [deletingQuestion, setDeletingQuestion] = useState<Question | null>(
		null
	)
	const [viewMode, setViewMode] = useState<'exam' | 'all' | 'matrix'>('exam')

	const { data: exam } = useExam(examId ?? '')
	const { data: matrix } = useMatrix(examId ?? '')
	const { data: topics = [] } = useTopics(exam?.subjectId ?? '')
	const { data: examQuestions = [] } = useExamQuestions(examId ?? '')
	const { data: questions, isLoading } = useQuestions({
		isPublic: true,
	})
	const deleteQuestion = useDeleteQuestion()
	const addQuestionsToExam = useAddQuestionsToExam()
	const removeQuestionFromExam = useRemoveQuestionFromExam()

	// Get existing question IDs for quick lookup
	const examQuestionIds = new Set(examQuestions.map(q => q.id))

	// Filter exam questions based on search and filters
	const filteredExamQuestions = examQuestions.filter(question => {
		const matchesSearch = question.content
			.toLowerCase()
			.includes(searchQuery.toLowerCase())
		const matchesLevel =
			filterLevel === 'all' || question.cognitiveLevel === Number(filterLevel)
		const matchesTopic =
			filterTopic === 'all' || question.topicId === filterTopic
		return matchesSearch && matchesLevel && matchesTopic
	})

	const filteredQuestions = questions?.filter(question => {
		const matchesSearch = question.content
			.toLowerCase()
			.includes(searchQuery.toLowerCase())
		const matchesLevel =
			filterLevel === 'all' || question.cognitiveLevel === Number(filterLevel)
		const matchesTopic =
			filterTopic === 'all' || question.topicId === filterTopic
		return matchesSearch && matchesLevel && matchesTopic
	})

	// Filter questions needed for matrix
	const matrixNeededQuestions = filteredQuestions?.filter(question => {
		if (!matrix) return true
		return matrix.matrixTopics.some(
			mt =>
				mt.topicId === question.topicId &&
				mt.cognitiveLevel === question.cognitiveLevel
		)
	})

	// Choose which questions to display based on view mode
	const displayQuestions =
		viewMode === 'exam'
			? filteredExamQuestions
			: viewMode === 'matrix' && matrix
				? matrixNeededQuestions
				: filteredQuestions

	const toggleQuestion = (questionId: string) => {
		const newSelected = new Set(selectedQuestions)
		if (newSelected.has(questionId)) {
			newSelected.delete(questionId)
		} else {
			newSelected.add(questionId)
		}
		setSelectedQuestions(newSelected)
	}

	const toggleAll = () => {
		if (selectedQuestions.size === displayQuestions?.length) {
			setSelectedQuestions(new Set())
		} else {
			setSelectedQuestions(new Set(displayQuestions?.map(q => q.id) || []))
		}
	}

	const handleAddSelectedToExam = async () => {
		if (!examId) return
		await addQuestionsToExam.mutateAsync({
			examId,
			questionIds: Array.from(selectedQuestions),
		})
		setSelectedQuestions(new Set())
		navigate(`/app/exams/${examId}`)
	}

	const handleDelete = async () => {
		if (!deletingQuestion) return
		await deleteQuestion.mutateAsync(deletingQuestion.id)
		setDeletingQuestion(null)
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

	const getCognitiveLevelVariant = (
		level: CognitiveLevel
	): 'default' | 'secondary' | 'outline' | 'destructive' => {
		switch (level) {
			case CognitiveLevel.Remember:
				return 'secondary'
			case CognitiveLevel.Understand:
				return 'default'
			case CognitiveLevel.Apply:
				return 'outline'
			case CognitiveLevel.Analyze:
				return 'destructive'
			default:
				return 'outline'
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

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div className='flex items-center space-x-4'>
					<Button
						variant='ghost'
						size='icon'
						onClick={() => navigate(`/app/exams/${examId}`)}
					>
						<ArrowLeft className='h-5 w-5' />
					</Button>
					<div>
						<h1 className='text-3xl font-bold'>Question Bank</h1>
						<p className='text-muted-foreground'>
							Select questions for your exam
						</p>
					</div>
				</div>
				<div className='flex gap-2'>
					<Button
						variant='outline'
						onClick={() => navigate(`/app/exams/${examId}/questions/create`)}
					>
						<Plus className='h-4 w-4 mr-2' />
						Manage Questions
					</Button>
					{viewMode !== 'exam' && (
						<Button
							onClick={handleAddSelectedToExam}
							disabled={selectedQuestions.size === 0}
						>
							<CheckCircle className='h-4 w-4 mr-2' />
							Add {selectedQuestions.size} Selected
						</Button>
					)}
				</div>
			</div>

			{/* Matrix Progress Tracker */}
			{matrix && (
				<MatrixProgressTracker
					matrix={matrix}
					questions={examQuestions}
					topics={topics}
				/>
			)}

			{/* Main Content */}
			<Card>
				<CardHeader>
					<div className='flex items-center justify-between'>
						<CardTitle>Available Questions</CardTitle>
						{matrix && (
							<TooltipProvider>
								<Tooltip>
									<TooltipTrigger asChild>
										<Badge
											variant='outline'
											className='cursor-pointer hover:bg-accent'
										>
											<Sparkles className='h-3 w-3 mr-1' />
											Matrix Mode Available
										</Badge>
									</TooltipTrigger>
									<TooltipContent>
										<p>Filter questions based on your exam matrix</p>
									</TooltipContent>
								</Tooltip>
							</TooltipProvider>
						)}
					</div>
				</CardHeader>
				<CardContent className='space-y-4'>
					{/* Filters */}
					<div className='flex flex-col sm:flex-row gap-4'>
						<div className='flex-1'>
							<div className='relative'>
								<Search className='absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-muted-foreground' />
								<Input
									placeholder='Search questions...'
									value={searchQuery}
									onChange={e => setSearchQuery(e.target.value)}
									className='pl-9'
								/>
							</div>
						</div>
						<Select value={filterTopic} onValueChange={setFilterTopic}>
							<SelectTrigger className='w-[200px]'>
								<Filter className='h-4 w-4 mr-2' />
								<SelectValue placeholder='Filter by topic' />
							</SelectTrigger>
							<SelectContent>
								<SelectItem value='all'>All Topics</SelectItem>
								{topics.map(topic => (
									<SelectItem key={topic.id} value={topic.id}>
										{topic.title}
									</SelectItem>
								))}
							</SelectContent>
						</Select>
						<Select value={filterLevel} onValueChange={setFilterLevel}>
							<SelectTrigger className='w-[200px]'>
								<Filter className='h-4 w-4 mr-2' />
								<SelectValue placeholder='Filter by level' />
							</SelectTrigger>
							<SelectContent>
								<SelectItem value='all'>All Levels</SelectItem>
								<SelectItem value={String(CognitiveLevel.Remember)}>
									Remember
								</SelectItem>
								<SelectItem value={String(CognitiveLevel.Understand)}>
									Understand
								</SelectItem>
								<SelectItem value={String(CognitiveLevel.Apply)}>
									Apply
								</SelectItem>
								<SelectItem value={String(CognitiveLevel.Analyze)}>
									Analyze
								</SelectItem>
							</SelectContent>
						</Select>
						<Tabs
							value={viewMode}
							onValueChange={(v: string) =>
								setViewMode(v as 'exam' | 'all' | 'matrix')
							}
						>
							<TabsList
								className={`grid w-full ${matrix ? 'grid-cols-3' : 'grid-cols-2'}`}
							>
								<TabsTrigger value='exam'>
									In Exam ({examQuestions.length})
								</TabsTrigger>
								<TabsTrigger value='all'>All Bank</TabsTrigger>
								{matrix && (
									<TabsTrigger value='matrix'>
										Matrix
										<Sparkles className='h-3 w-3 ml-1' />
									</TabsTrigger>
								)}
							</TabsList>
						</Tabs>
					</div>

					{/* Questions Table */}
					{isLoading ? (
						<div className='flex items-center justify-center py-12'>
							<div className='animate-spin rounded-full h-8 w-8 border-b-2 border-primary'></div>
						</div>
					) : displayQuestions && displayQuestions.length > 0 ? (
						<div className='border rounded-lg overflow-hidden'>
							<Table>
								<TableHeader>
									<TableRow className='bg-muted/50'>
										<TableHead className='w-[50px]'>
											<Checkbox
												checked={
													selectedQuestions.size === displayQuestions.length &&
													displayQuestions.length > 0
												}
												onCheckedChange={toggleAll}
											/>
										</TableHead>
										<TableHead>Content</TableHead>
										<TableHead>Topic</TableHead>
										<TableHead>Level</TableHead>
										<TableHead>Type</TableHead>
										<TableHead>Points</TableHead>
										<TableHead className='text-right'>Actions</TableHead>
									</TableRow>
								</TableHeader>
								<TableBody>
									{displayQuestions.map(question => {
										const topic = topics.find(t => t.id === question.topicId)
										return (
											<TableRow
												key={question.id}
												className='hover:bg-muted/50 transition-colors'
											>
												<TableCell>
													<Checkbox
														checked={selectedQuestions.has(question.id)}
														onCheckedChange={() => toggleQuestion(question.id)}
														disabled={viewMode === 'exam'}
													/>
												</TableCell>
												<TableCell className='max-w-md'>
													<div className='flex items-start gap-2'>
														<div className='line-clamp-2 text-sm'>
															{question.content}
														</div>
														{viewMode !== 'exam' &&
															examQuestionIds.has(question.id) && (
																<Badge
																	variant='secondary'
																	className='text-xs shrink-0'
																>
																	In Exam
																</Badge>
															)}
													</div>
												</TableCell>
												<TableCell>
													<span className='text-sm font-medium'>
														{topic?.title || 'N/A'}
													</span>
												</TableCell>
												<TableCell>
													<Badge
														variant={getCognitiveLevelVariant(
															question.cognitiveLevel
														)}
													>
														{getCognitiveLevelLabel(question.cognitiveLevel)}
													</Badge>
												</TableCell>
												<TableCell>
													<span className='text-sm text-muted-foreground'>
														{getQuestionTypeLabel(question.type)}
													</span>
												</TableCell>
												<TableCell>
													<span className='text-sm font-medium'>
														{question.point}
													</span>
												</TableCell>
												<TableCell className='text-right'>
													<DropdownMenu>
														<DropdownMenuTrigger asChild>
															<Button variant='ghost' size='icon'>
																<MoreHorizontal className='h-4 w-4' />
															</Button>
														</DropdownMenuTrigger>
														<DropdownMenuContent align='end'>
															<DropdownMenuItem
																onClick={() =>
																	navigate(
																		`/app/exams/${examId}/questions/${question.id}`
																	)
																}
															>
																<Eye className='h-4 w-4 mr-2' />
																View
															</DropdownMenuItem>
															{viewMode === 'exam' && (
																<DropdownMenuItem
																	onClick={async () => {
																		if (examId) {
																			await removeQuestionFromExam.mutateAsync({
																				examId,
																				questionId: question.id,
																			})
																		}
																	}}
																	className='text-destructive'
																>
																	<Trash2 className='h-4 w-4 mr-2' />
																	Remove from Exam
																</DropdownMenuItem>
															)}
															<DropdownMenuItem
																onClick={() =>
																	navigate(
																		`/app/exams/${examId}/questions/${question.id}/edit`
																	)
																}
															>
																<Pencil className='h-4 w-4 mr-2' />
																Edit
															</DropdownMenuItem>
															<DropdownMenuItem
																onClick={() => setDeletingQuestion(question)}
																className='text-destructive'
															>
																<Trash2 className='h-4 w-4 mr-2' />
																Delete
															</DropdownMenuItem>
														</DropdownMenuContent>
													</DropdownMenu>
												</TableCell>
											</TableRow>
										)
									})}
								</TableBody>
							</Table>
						</div>
					) : (
						<Card className='border-dashed'>
							<CardContent className='flex flex-col items-center justify-center py-12 text-center'>
								<AlertCircle className='h-12 w-12 text-muted-foreground mb-4' />
								<p className='text-lg font-medium mb-2'>No questions found</p>
								<p className='text-sm text-muted-foreground mb-4'>
									{viewMode === 'matrix'
										? 'No questions match your matrix requirements'
										: 'Create your first question to get started'}
								</p>
								<Button
									onClick={() =>
										navigate(`/app/exams/${examId}/questions/create`)
									}
								>
									<Plus className='h-4 w-4 mr-2' />
									Create Question
								</Button>
							</CardContent>
						</Card>
					)}
				</CardContent>
			</Card>

			{/* Delete Confirmation Dialog */}
			<AlertDialog
				open={!!deletingQuestion}
				onOpenChange={open => !open && setDeletingQuestion(null)}
			>
				<AlertDialogContent>
					<AlertDialogHeader>
						<AlertDialogTitle>Are you sure?</AlertDialogTitle>
						<AlertDialogDescription>
							This action cannot be undone. This will permanently delete the
							question.
						</AlertDialogDescription>
					</AlertDialogHeader>
					<AlertDialogFooter>
						<AlertDialogCancel>Cancel</AlertDialogCancel>
						<AlertDialogAction
							onClick={handleDelete}
							className='bg-destructive'
						>
							Delete
						</AlertDialogAction>
					</AlertDialogFooter>
				</AlertDialogContent>
			</AlertDialog>
		</div>
	)
}

export default QuestionBankPage
