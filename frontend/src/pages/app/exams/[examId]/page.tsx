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
import { ExamDetailSkeleton } from '@/components/common/skeletons'
import { useTranslation } from 'react-i18next'

const ExamDetailPage = (): React.ReactElement => {
	const { t } = useTranslation()
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
			alert(t('pages.exams.detail.publish.no_questions'))
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
			title: t('pages.exams.detail.questions.remove_title'),
			description: t('pages.exams.detail.questions.remove_description'),
			confirmText: t('pages.exams.detail.questions.remove_confirm'),
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
			title: t('pages.exams.detail.matrix.detach_title'),
			description: t('pages.exams.detail.matrix.detach_description'),
			confirmText: t('pages.exams.detail.matrix.detach_confirm'),
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
				return t('exams.cognitive_levels.remember')
			case CognitiveLevel.Understand:
				return t('exams.cognitive_levels.understand')
			case CognitiveLevel.Apply:
				return t('exams.cognitive_levels.apply')
			case CognitiveLevel.Analyze:
				return t('exams.cognitive_levels.analyze')
			default:
				return t('common.unknown')
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
				return t('exams.question_types.multiple_choice')
			case QuestionType.TrueFalse:
				return t('exams.question_types.true_false')
			case QuestionType.Essay:
				return t('exams.question_types.essay')
			case QuestionType.FillInTheBlank:
				return t('exams.question_types.fill_in_blank')
			default:
				return t('common.unknown')
		}
	}

	if (isLoadingExam || isLoadingQuestions) {
		return <ExamDetailSkeleton />
	}

	if (!exam) {
		return (
			<div className='p-6 space-y-6 max-w-7xl mx-auto'>
				<div className='text-center py-12'>
					<p className='text-muted-foreground mb-4'>
						{t('pages.exams.detail.not_found')}
					</p>
					<Button onClick={() => navigate('/app/exams')}>
						<ArrowLeft className='h-4 w-4 mr-2' />
						{t('pages.exams.detail.back_to_exams')}
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
										{t('pages.exams.list.status.draft')}
									</Badge>
								) : (
									<Badge variant='default'>
										<CheckCircle className='h-3 w-3 mr-1' />
										{t('pages.exams.list.status.active')}
									</Badge>
								)}
							</div>
							<p className='text-muted-foreground'>
								{t('pages.exams.detail.summary', {
									count: questions.length,
									points: totalPoints,
								})}
							</p>
						</div>
					</div>
					<div className='flex gap-2'>
						<Button
							variant='outline'
							onClick={() => navigate(`/app/exams/${examId}/preview`)}
						>
							<FileText className='h-4 w-4 mr-2' />
							{t('pages.exams.detail.actions.preview')}
						</Button>
						<Button
							variant='outline'
							onClick={() => navigate(`/app/exams/${examId}/edit`)}
						>
							<Settings className='h-4 w-4 mr-2' />
							{t('pages.exams.detail.actions.edit')}
						</Button>
						<DropdownMenu>
							<DropdownMenuTrigger asChild>
								<Button variant='outline'>
									<Download className='h-4 w-4 mr-2' />
									{t('pages.exams.detail.actions.export')}
								</Button>
							</DropdownMenuTrigger>
							<DropdownMenuContent>
								<DropdownMenuItem
									onClick={handleExportPdf}
									disabled={exportToPdf.isPending}
								>
									<FileText className='h-4 w-4 mr-2' />
									{exportToPdf.isPending
										? t('pages.exams.detail.actions.exporting_pdf')
										: t('pages.exams.detail.actions.export_pdf')}
								</DropdownMenuItem>
								<DropdownMenuItem
									onClick={handleExportExcel}
									disabled={exportToExcel.isPending}
								>
									<FileText className='h-4 w-4 mr-2' />
									{exportToExcel.isPending
										? t('pages.exams.detail.actions.exporting_excel')
										: t('pages.exams.detail.actions.export_excel')}
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
										{t('pages.exams.detail.actions.publish')}
									</Button>
								</DialogTrigger>
								<DialogContent>
									<DialogHeader>
										<DialogTitle>
											{t('pages.exams.detail.publish.title')}
										</DialogTitle>
										<DialogDescription>
											{t('pages.exams.detail.publish.description')}
										</DialogDescription>
									</DialogHeader>
									<DialogFooter>
										<Button
											variant='outline'
											onClick={() => setIsPublishDialogOpen(false)}
										>
											{t('common.cancel')}
										</Button>
										<Button
											onClick={handlePublish}
											disabled={publishExamMutation.isPending}
										>
											{publishExamMutation.isPending
												? t('pages.exams.detail.publish.publishing')
												: t('pages.exams.detail.publish.confirm')}
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
						<CardTitle>{t('pages.exams.detail.info.title')}</CardTitle>
					</CardHeader>
					<CardContent className='space-y-4'>
						<div className='grid grid-cols-2 gap-4'>
							<div>
								<p className='text-sm text-muted-foreground mb-1'>
									{t('pages.exams.detail.info.description')}
								</p>
								<p className='font-medium'>{exam.description}</p>
							</div>
							<div>
								<p className='text-sm text-muted-foreground mb-1'>
									{t('pages.exams.detail.info.subject')}
								</p>
								<p className='font-medium'>
									{subjects.find(s => s.id === exam.subjectId)?.name ||
										t('pages.exams.detail.info.subject_unknown')}
								</p>
							</div>
						</div>

						<div className='grid grid-cols-2 gap-4'>
							<div>
								<p className='text-sm text-muted-foreground'>
									{t('pages.exams.detail.info.grade')}
								</p>
								<p className='font-medium'>
									{t('pages.exams.detail.info.grade_value', {
										grade: exam.grade,
									})}
								</p>
							</div>

							<div>
								<p className='text-sm text-muted-foreground'>
									{t('pages.exams.detail.info.created')}
								</p>
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
							<CardTitle>{t('pages.exams.detail.matrix.title')}</CardTitle>
							<p className='text-sm text-muted-foreground mt-1'>
								{t('pages.exams.detail.matrix.subtitle')}
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
									{t('pages.exams.detail.matrix.edit')}
								</Button>
								<Button
									variant='outline'
									onClick={handleDetachMatrix}
									disabled={detachMatrixMutation.isPending}
									className='text-destructive hover:text-destructive'
								>
									<Trash2 className='h-4 w-4 mr-2' />
									{detachMatrixMutation.isPending
										? t('pages.exams.detail.matrix.detaching')
										: t('pages.exams.detail.matrix.detach')}
								</Button>
							</div>
						) : (
							<Button
								onClick={() =>
									navigate(`/app/exams/create/matrix?examId=${examId}`)
								}
							>
								<Plus className='h-4 w-4 mr-2' />
								{t('pages.exams.detail.matrix.create')}
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
											<p className='font-medium'>
												{t('pages.exams.detail.matrix.configured')}
											</p>
											<p className='text-sm text-muted-foreground'>
												{t('pages.exams.detail.matrix.summary', {
													count: matrix.matrixTopics.length,
												})}
											</p>
										</div>
									</div>
									<Badge variant='default'>
										{t('pages.exams.detail.matrix.total_questions', {
											count: matrix.totalQuestionCount,
										})}
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
														{topic?.title ||
															t('pages.exams.detail.matrix.topic_unknown')}
													</span>
													<Badge variant='outline'>
														{getCognitiveLevelLabel(mt.cognitiveLevel)}
													</Badge>
												</div>
												<span className='text-sm text-muted-foreground'>
													{t('pages.exams.detail.matrix.quantity', {
														count: mt.quantity,
													})}
												</span>
											</div>
										)
									})}
									{matrix.matrixTopics.length > 5 && (
										<p className='text-sm text-muted-foreground text-center py-2'>
											{t('pages.exams.detail.matrix.more_combinations', {
												count: matrix.matrixTopics.length - 5,
											})}
										</p>
									)}
								</div>
							</div>
						) : (
							<div className='text-center py-8'>
								<Grid3x3 className='h-12 w-12 text-muted-foreground mx-auto mb-4' />
								<p className='text-sm text-muted-foreground mb-4'>
									{t('pages.exams.detail.matrix.empty')}
								</p>
								<div className='flex gap-2 justify-center'>
									<Button
										onClick={() =>
											navigate(`/app/exams/create/matrix?examId=${examId}`)
										}
									>
										<Plus className='h-4 w-4 mr-2' />
										{t('pages.exams.detail.matrix.create')}
									</Button>
									<Button
										variant='outline'
										onClick={() => setIsAttachMatrixDialogOpen(true)}
									>
										{t('pages.exams.detail.matrix.attach_existing')}
									</Button>
								</div>
							</div>
						)}
					</CardContent>
				</Card>

				{/* Questions List */}
				<Card>
					<CardHeader className='flex flex-row items-center justify-between'>
						<CardTitle>
							{t('pages.exams.detail.questions.title', {
								count: questions.length,
							})}
						</CardTitle>
						<Button
							variant='outline'
							onClick={() => navigate(`/app/exams/${examId}/questions/create`)}
						>
							<Plus className='h-4 w-4 mr-2' />
							{t('pages.exams.detail.questions.add')}
						</Button>
					</CardHeader>
					<CardContent>
						{questions.length > 0 ? (
							<Table>
								<TableHeader>
									<TableRow>
										<TableHead className='w-12'>#</TableHead>
										<TableHead className='w-[50%]'>
											{t('pages.exams.detail.questions.table.question')}
										</TableHead>
										<TableHead>
											{t('pages.exams.detail.questions.table.type')}
										</TableHead>
										<TableHead>
											{t('pages.exams.detail.questions.table.level')}
										</TableHead>
										<TableHead>
											{t('pages.exams.detail.questions.table.points')}
										</TableHead>
										<TableHead className='text-right'>
											{t('pages.exams.detail.questions.table.actions')}
										</TableHead>
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
									{t('pages.exams.detail.questions.empty')}
								</p>
								<Button
									onClick={() =>
										navigate(`/app/exams/${examId}/questions/create`)
									}
								>
									<Plus className='h-4 w-4 mr-2' />
									{t('pages.exams.detail.questions.add')}
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
						<DialogTitle>
							{t('pages.exams.detail.matrix.attach_title')}
						</DialogTitle>
						<DialogDescription>
							{t('pages.exams.detail.matrix.attach_description')}
						</DialogDescription>
					</DialogHeader>
					<div className='space-y-4 py-4'>
						{matrices.length === 0 ? (
							<p className='text-sm text-muted-foreground text-center py-4'>
								{t('pages.exams.detail.matrix.attach_empty')}
							</p>
						) : (
							<>
								<Input
									placeholder={t(
										'pages.exams.detail.matrix.search_placeholder'
									)}
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
														{t('pages.exams.detail.matrix.quantity', {
															count: matrix.totalQuestionCount,
														})}
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
												? t('pages.exams.detail.matrix.search_empty')
												: t('pages.exams.detail.matrix.attach_incompatible')}
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
							{t('common.cancel')}
						</Button>
						<Button
							onClick={handleAttachMatrix}
							disabled={!selectedMatrixId || attachMatrixMutation.isPending}
						>
							{attachMatrixMutation.isPending
								? t('pages.exams.detail.matrix.attaching')
								: t('pages.exams.detail.matrix.attach')}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>
		</>
	)
}

export default ExamDetailPage
