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
	FileText,
	Download,
} from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Input } from '@/components/ui/input'
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
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import { CognitiveLevel, QuestionType } from '@/types/model/exam-service'
import {
	useExam,
	useExamQuestions,
	usePublishExam,
	useRemoveQuestionFromExam,
	useMatrixByExamId,
	useTopics,
	useSubjects,
	useExportExamToPdf,
	useExportExamToExcel,
	useDetachMatrixFromExam,
	useMatrices,
	useAttachMatrixToExam,
} from '@/hooks/useExams'
import { useConfirm } from '@/hooks/useConfirm'
import { ConfirmDialog } from '@/components/common/ConfirmDialog'

const ExamDetailPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const { examId } = useParams<{ examId: string }>()
	const [isPublishDialogOpen, setIsPublishDialogOpen] = useState(false)
	const [isAttachMatrixDialogOpen, setIsAttachMatrixDialogOpen] =
		useState(false)
	const [selectedMatrixId, setSelectedMatrixId] = useState<string>('')
	const [matrixSearchQuery, setMatrixSearchQuery] = useState('')

	const { data: exam, isLoading: isLoadingExam } = useExam(examId || '')
	const { data: questions = [], isLoading: isLoadingQuestions } =
		useExamQuestions(examId || '')
	const { data: matrix } = useMatrixByExamId(examId || '')
	const { data: topics = [] } = useTopics(exam?.subjectId ?? '')
	const { data: subjects = [] } = useSubjects(exam?.grade)
	const { data: matrices = [] } = useMatrices()
	const publishExamMutation = usePublishExam()
	const removeQuestionMutation = useRemoveQuestionFromExam()
	const exportToPdf = useExportExamToPdf()
	const exportToExcel = useExportExamToExcel()
	const detachMatrixMutation = useDetachMatrixFromExam()
	const attachMatrixMutation = useAttachMatrixToExam()
	const {
		confirm,
		confirmState,
		handleConfirm,
		handleCancel,
		handleOpenChange,
	} = useConfirm()

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

	const handleExportPdf = async () => {
		if (!examId) return
		await exportToPdf.mutateAsync(examId)
	}

	const handleExportExcel = async () => {
		if (!examId) return
		await exportToExcel.mutateAsync(examId)
	}

	const handleDelete = async (questionId: string) => {
		if (!examId) return

		const confirmed = await confirm({
			title: 'Remove Question',
			description:
				'Are you sure you want to remove this question from the exam?',
			confirmText: 'Remove',
			variant: 'destructive',
		})

		if (confirmed) {
			try {
				await removeQuestionMutation.mutateAsync({ examId, questionId })
			} catch (error) {
				console.error('Failed to delete question:', error)
			}
		}
	}

	const handleDetachMatrix = async () => {
		if (!examId) return

		const confirmed = await confirm({
			title: 'Detach Matrix',
			description:
				'Are you sure you want to detach this matrix from the exam? The matrix will still be available for other exams.',
			confirmText: 'Detach',
			variant: 'destructive',
		})

		if (confirmed) {
			try {
				await detachMatrixMutation.mutateAsync(examId)
			} catch (error) {
				console.error('Failed to detach matrix:', error)
			}
		}
	}

	const handleAttachMatrix = async () => {
		if (!examId || !selectedMatrixId) return

		try {
			await attachMatrixMutation.mutateAsync({
				examId,
				matrixId: selectedMatrixId,
			})
			setIsAttachMatrixDialogOpen(false)
			setSelectedMatrixId('')
		} catch (error) {
			console.error('Failed to attach matrix:', error)
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
		<>
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
							onClick={() => navigate(`/app/exams/${examId}/preview`)}
						>
							<FileText className='h-4 w-4 mr-2' />
							Preview
						</Button>
						<Button
							variant='outline'
							onClick={() => navigate(`/app/exams/${examId}/edit`)}
						>
							<Settings className='h-4 w-4 mr-2' />
							Edit
						</Button>
						<DropdownMenu>
							<DropdownMenuTrigger asChild>
								<Button variant='outline'>
									<Download className='h-4 w-4 mr-2' />
									Export
								</Button>
							</DropdownMenuTrigger>
							<DropdownMenuContent>
								<DropdownMenuItem
									onClick={handleExportPdf}
									disabled={exportToPdf.isPending}
								>
									<FileText className='h-4 w-4 mr-2' />
									{exportToPdf.isPending ? 'Exporting PDF...' : 'Export as PDF'}
								</DropdownMenuItem>
								<DropdownMenuItem
									onClick={handleExportExcel}
									disabled={exportToExcel.isPending}
								>
									<FileText className='h-4 w-4 mr-2' />
									{exportToExcel.isPending
										? 'Exporting Excel...'
										: 'Export as Excel'}
								</DropdownMenuItem>
							</DropdownMenuContent>
						</DropdownMenu>
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
											Are you sure you want to publish this exam? Students will
											be able to access it after publishing.
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
								<p className='text-sm text-muted-foreground mb-1'>
									Description
								</p>
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
							<div className='flex gap-2'>
								<Button
									variant='outline'
									onClick={() =>
										navigate(`/app/exams/create/matrix?examId=${examId}`)
									}
								>
									<Edit className='h-4 w-4 mr-2' />
									Edit Matrix
								</Button>
								<Button
									variant='outline'
									onClick={handleDetachMatrix}
									disabled={detachMatrixMutation.isPending}
									className='text-destructive hover:text-destructive'
								>
									<Trash2 className='h-4 w-4 mr-2' />
									{detachMatrixMutation.isPending ? 'Detaching...' : 'Detach'}
								</Button>
							</div>
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
									No matrix configured yet. Create a new matrix or attach an
									existing one.
								</p>
								<div className='flex gap-2 justify-center'>
									<Button
										onClick={() =>
											navigate(`/app/exams/create/matrix?examId=${examId}`)
										}
									>
										<Plus className='h-4 w-4 mr-2' />
										Create Matrix
									</Button>
									<Button
										variant='outline'
										onClick={() => setIsAttachMatrixDialogOpen(true)}
									>
										Attach Existing Matrix
									</Button>
								</div>
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
									onClick={() =>
										navigate(`/app/exams/${examId}/questions/create`)
									}
								>
									<Plus className='h-4 w-4 mr-2' />
									Add Questions
								</Button>
							</div>
						)}
					</CardContent>
				</Card>
			</div>
			<ConfirmDialog
				open={confirmState.isOpen}
				onOpenChange={handleOpenChange}
				title={confirmState.title}
				description={confirmState.description}
				onConfirm={handleConfirm}
				onCancel={handleCancel}
				confirmText={confirmState.confirmText}
				cancelText={confirmState.cancelText}
				variant={confirmState.variant}
			/>
			<Dialog
				open={isAttachMatrixDialogOpen}
				onOpenChange={setIsAttachMatrixDialogOpen}
			>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>Attach Existing Matrix</DialogTitle>
						<DialogDescription>
							Select a matrix to attach to this exam. The matrix must match the
							exam's subject and grade.
						</DialogDescription>
					</DialogHeader>
					<div className='space-y-4 py-4'>
						{matrices.length === 0 ? (
							<p className='text-sm text-muted-foreground text-center py-4'>
								No matrices available. Create a new matrix first.
							</p>
						) : (
							<>
								<Input
									placeholder='Search matrices...'
									value={matrixSearchQuery}
									onChange={e => setMatrixSearchQuery(e.target.value)}
									className='w-full'
								/>
								<div className='space-y-2 max-h-[400px] overflow-y-auto'>
									{matrices
										.filter(
											m =>
												m.subjectId === exam?.subjectId &&
												m.grade === exam?.grade &&
												(matrixSearchQuery === '' ||
													m.name
														.toLowerCase()
														.includes(matrixSearchQuery.toLowerCase()) ||
													m.description
														?.toLowerCase()
														.includes(matrixSearchQuery.toLowerCase()))
										)
										.map(matrix => (
											<div
												key={matrix.id}
												className={`p-4 border rounded-lg cursor-pointer transition-colors hover:bg-accent ${
													selectedMatrixId === matrix.id
														? 'border-primary bg-accent'
														: ''
												}`}
												onClick={() => setSelectedMatrixId(matrix.id)}
											>
												<div className='flex items-center justify-between gap-4'>
													<div className='flex-1 min-w-0'>
														<p className='font-medium truncate'>
															{matrix.name}
														</p>
														{matrix.description && (
															<p className='text-sm text-muted-foreground line-clamp-2'>
																{matrix.description}
															</p>
														)}
													</div>
													<Badge variant='outline' className='shrink-0'>
														{matrix.totalQuestionCount} questions
													</Badge>
												</div>
											</div>
										))}
									{matrices.filter(
										m =>
											m.subjectId === exam?.subjectId &&
											m.grade === exam?.grade &&
											(matrixSearchQuery === '' ||
												m.name
													.toLowerCase()
													.includes(matrixSearchQuery.toLowerCase()) ||
												m.description
													?.toLowerCase()
													.includes(matrixSearchQuery.toLowerCase()))
									).length === 0 && (
										<p className='text-sm text-muted-foreground text-center py-4'>
											{matrixSearchQuery
												? 'No matrices found matching your search.'
												: "No compatible matrices found. The matrix must match the exam's subject and grade."}
										</p>
									)}
								</div>
							</>
						)}
					</div>
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => {
								setIsAttachMatrixDialogOpen(false)
								setSelectedMatrixId('')
								setMatrixSearchQuery('')
							}}
						>
							Cancel
						</Button>
						<Button
							onClick={handleAttachMatrix}
							disabled={!selectedMatrixId || attachMatrixMutation.isPending}
						>
							{attachMatrixMutation.isPending
								? 'Attaching...'
								: 'Attach Matrix'}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>
		</>
	)
}

export default ExamDetailPage
