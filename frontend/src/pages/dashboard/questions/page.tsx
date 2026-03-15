import React, { useState, useMemo } from 'react'
import { useTranslation } from 'react-i18next'
import {
	Plus,
	MoreHorizontal,
	Trash2,
	Search,
	HelpCircle,
	Eye,
	EyeOff,
} from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Skeleton } from '@/components/ui/skeleton'
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
import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogFooter,
	DialogHeader,
	DialogTitle,
} from '@/components/ui/dialog'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { Form } from '@/components/ui/form'
import { ScrollArea } from '@/components/ui/scroll-area'
import {
	useSubjects,
	useTopics,
	useQuestions,
	useCreateQuestion,
	useDeleteQuestion,
} from '@/hooks/useExams'
import { useQuestionForm } from '@/hooks/useQuestionForm'
import {
	CognitiveLevel,
	QuestionType,
	QuestionSource,
} from '@/types/model/exam-service'
import type { Question } from '@/types/model/exam-service'
import { QuestionFormFields } from '@/pages/app/exams/[examId]/questions/create/components/QuestionFormFields'
import { QuestionAnswersSection } from '@/pages/app/exams/[examId]/questions/create/components/QuestionAnswersSection'
import type { CreateQuestionRequest } from '@/types/dtos/exams'

const PAGE_SIZE = 10

// ─── Label helpers ───────────────────────────────────────────────────────────

const CognitiveLevelBadgeVariant: Record<
	CognitiveLevel,
	'default' | 'secondary' | 'outline' | 'destructive'
> = {
	[CognitiveLevel.Remember]: 'outline',
	[CognitiveLevel.Understand]: 'secondary',
	[CognitiveLevel.Apply]: 'default',
	[CognitiveLevel.Analyze]: 'destructive',
}

const QuestionsPage = (): React.ReactElement => {
	const { t } = useTranslation()

	// Filters
	const [selectedSubjectId, setSelectedSubjectId] = useState<string>('')
	const [selectedTopicId, setSelectedTopicId] = useState<string>('')
	const [selectedLevel, setSelectedLevel] = useState<string>('all')
	const [selectedType, setSelectedType] = useState<string>('all')
	const [search, setSearch] = useState('')
	const [page, setPage] = useState(1)
	const [showPublicOnly, setShowPublicOnly] = useState(true)

	// Dialog state
	const [isCreateOpen, setIsCreateOpen] = useState(false)
	const [deletingQuestion, setDeletingQuestion] = useState<Question | null>(
		null
	)

	// Data
	const { data: subjects = [] } = useSubjects()
	const { data: topics = [] } = useTopics(selectedSubjectId)
	const { data: questions = [], isLoading } = useQuestions({
		topicId: selectedTopicId || undefined,
		cognitiveLevel:
			selectedLevel !== 'all'
				? (Number(selectedLevel) as CognitiveLevel)
				: undefined,
		isPublic: showPublicOnly ? true : undefined,
		search: search || undefined,
	})

	const createQuestion = useCreateQuestion()
	const deleteQuestion = useDeleteQuestion()

	// Form setup
	const {
		form,
		fields,
		questionType,
		handleCorrectAnswerChange,
		addAnswer,
		removeAnswer,
		append,
		remove,
	} = useQuestionForm()

	// Client-side filter by type (type isn't in the backend query)
	const filtered = useMemo(() => {
		if (selectedType === 'all') return questions
		return questions.filter(q => q.type === Number(selectedType))
	}, [questions, selectedType])

	const totalPages = Math.max(1, Math.ceil(filtered.length / PAGE_SIZE))
	const paginated = filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE)

	React.useEffect(() => {
		setPage(1)
	}, [
		selectedSubjectId,
		selectedTopicId,
		selectedLevel,
		selectedType,
		search,
		showPublicOnly,
	])

	// Reset topic when subject changes
	React.useEffect(() => {
		setSelectedTopicId('')
	}, [selectedSubjectId])

	const handleCreateSubmit = form.handleSubmit(async data => {
		const request: CreateQuestionRequest = {
			content: data.content,
			point: data.point,
			type: data.type,
			cognitiveLevel: data.cognitiveLevel,
			source: QuestionSource.Manual,
			topicId: data.topicId,
			mediaUrl: data.mediaUrl,
			isPublic: data.isPublic,
			answers: data.answers.map(a => ({ ...a, point: 0 })),
		}
		await createQuestion.mutateAsync(request)
		setIsCreateOpen(false)
		form.reset()
	})

	const handleDelete = async () => {
		if (!deletingQuestion) return
		await deleteQuestion.mutateAsync(deletingQuestion.id)
		setDeletingQuestion(null)
	}

	const cognitiveLevelLabel = (level: CognitiveLevel) => {
		const key = `pages.dashboard.questions.cognitive_levels.${
			level === CognitiveLevel.Remember
				? 'remember'
				: level === CognitiveLevel.Understand
					? 'understand'
					: level === CognitiveLevel.Apply
						? 'apply'
						: 'analyze'
		}`
		return t(key)
	}

	const questionTypeLabel = (type: QuestionType) => {
		const key = `pages.dashboard.questions.question_types.${
			type === QuestionType.MultipleChoice
				? 'multiple_choice'
				: type === QuestionType.MultipleAnswer
					? 'multiple_answer'
					: type === QuestionType.TrueFalse
						? 'true_false'
						: type === QuestionType.Essay
							? 'essay'
							: 'fill_in_blank'
		}`
		return t(key)
	}

	const sourceLabel = (source: QuestionSource) => {
		const key = `pages.dashboard.questions.sources.${
			source === QuestionSource.Manual
				? 'manual'
				: source === QuestionSource.AIGenerated
					? 'ai_generated'
					: source === QuestionSource.Imported
						? 'imported'
						: 'curriculum_bank'
		}`
		return t(key)
	}

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div className='space-y-1'>
					<h1 className='text-3xl font-bold tracking-tight flex items-center gap-2'>
						<HelpCircle className='h-8 w-8' />
						{t('pages.dashboard.questions.title')}
					</h1>
					<p className='text-muted-foreground'>
						{t('pages.dashboard.questions.subtitle')}
					</p>
				</div>
				<Button onClick={() => setIsCreateOpen(true)} size='lg'>
					<Plus className='h-5 w-5 mr-2' />
					{t('pages.dashboard.questions.new_question')}
				</Button>
			</div>

			{/* Filters */}
			<div className='flex flex-wrap gap-3 items-center'>
				{/* Search */}
				<div className='relative flex-1 min-w-[180px] max-w-xs'>
					<Search className='absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground' />
					<Input
						className='pl-9'
						placeholder={t('pages.dashboard.questions.search_placeholder')}
						value={search}
						onChange={e => setSearch(e.target.value)}
					/>
				</div>

				{/* Subject filter */}
				<Select
					value={selectedSubjectId || 'all'}
					onValueChange={v => setSelectedSubjectId(v === 'all' ? '' : v)}
				>
					<SelectTrigger className='w-44'>
						<SelectValue
							placeholder={t('pages.dashboard.questions.all_subjects')}
						/>
					</SelectTrigger>
					<SelectContent>
						<SelectItem value='all'>
							{t('pages.dashboard.questions.all_subjects')}
						</SelectItem>
						{subjects.map(s => (
							<SelectItem key={s.id} value={s.id}>
								{s.name}
							</SelectItem>
						))}
					</SelectContent>
				</Select>

				{/* Topic filter */}
				<Select
					value={selectedTopicId || 'all'}
					onValueChange={v => setSelectedTopicId(v === 'all' ? '' : v)}
					disabled={!selectedSubjectId}
				>
					<SelectTrigger className='w-44'>
						<SelectValue
							placeholder={t('pages.dashboard.questions.all_topics')}
						/>
					</SelectTrigger>
					<SelectContent>
						<SelectItem value='all'>
							{t('pages.dashboard.questions.all_topics')}
						</SelectItem>
						{topics.map(topic => (
							<SelectItem key={topic.id} value={topic.id}>
								{topic.title}
							</SelectItem>
						))}
					</SelectContent>
				</Select>

				{/* Cognitive Level filter */}
				<Select value={selectedLevel} onValueChange={setSelectedLevel}>
					<SelectTrigger className='w-40'>
						<SelectValue
							placeholder={t('pages.dashboard.questions.all_levels')}
						/>
					</SelectTrigger>
					<SelectContent>
						<SelectItem value='all'>
							{t('pages.dashboard.questions.all_levels')}
						</SelectItem>
						<SelectItem value={String(CognitiveLevel.Remember)}>
							{cognitiveLevelLabel(CognitiveLevel.Remember)}
						</SelectItem>
						<SelectItem value={String(CognitiveLevel.Understand)}>
							{cognitiveLevelLabel(CognitiveLevel.Understand)}
						</SelectItem>
						<SelectItem value={String(CognitiveLevel.Apply)}>
							{cognitiveLevelLabel(CognitiveLevel.Apply)}
						</SelectItem>
						<SelectItem value={String(CognitiveLevel.Analyze)}>
							{cognitiveLevelLabel(CognitiveLevel.Analyze)}
						</SelectItem>
					</SelectContent>
				</Select>

				{/* Type filter */}
				<Select value={selectedType} onValueChange={setSelectedType}>
					<SelectTrigger className='w-40'>
						<SelectValue
							placeholder={t('pages.dashboard.questions.all_types')}
						/>
					</SelectTrigger>
					<SelectContent>
						<SelectItem value='all'>
							{t('pages.dashboard.questions.all_types')}
						</SelectItem>
						<SelectItem value={String(QuestionType.MultipleChoice)}>
							{questionTypeLabel(QuestionType.MultipleChoice)}
						</SelectItem>
						<SelectItem value={String(QuestionType.MultipleAnswer)}>
							{questionTypeLabel(QuestionType.MultipleAnswer)}
						</SelectItem>
						<SelectItem value={String(QuestionType.TrueFalse)}>
							{questionTypeLabel(QuestionType.TrueFalse)}
						</SelectItem>
						<SelectItem value={String(QuestionType.Essay)}>
							{questionTypeLabel(QuestionType.Essay)}
						</SelectItem>
						<SelectItem value={String(QuestionType.FillInTheBlank)}>
							{questionTypeLabel(QuestionType.FillInTheBlank)}
						</SelectItem>
					</SelectContent>
				</Select>

				{/* Public toggle */}
				<Button
					variant={showPublicOnly ? 'default' : 'outline'}
					size='sm'
					onClick={() => setShowPublicOnly(p => !p)}
				>
					{showPublicOnly ? (
						<>
							<Eye className='h-4 w-4 mr-2' />
							{t('pages.dashboard.questions.filter_topic')}
						</>
					) : (
						<>
							<EyeOff className='h-4 w-4 mr-2' />
							All
						</>
					)}
				</Button>
			</div>

			{/* Table */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						{t('pages.dashboard.questions.title')}
						{filtered.length > 0 && (
							<Badge variant='secondary'>{filtered.length}</Badge>
						)}
					</CardTitle>
				</CardHeader>
				<CardContent>
					{isLoading ? (
						<div className='space-y-3'>
							{Array.from({ length: 6 }).map((_, i) => (
								<Skeleton key={i} className='h-12 w-full' />
							))}
						</div>
					) : paginated.length > 0 ? (
						<>
							<Table>
								<TableHeader>
									<TableRow>
										<TableHead className='w-[40%]'>
											{t('pages.dashboard.questions.table.content')}
										</TableHead>
										<TableHead>
											{t('pages.dashboard.questions.table.type')}
										</TableHead>
										<TableHead>
											{t('pages.dashboard.questions.table.cognitive_level')}
										</TableHead>
										<TableHead>
											{t('pages.dashboard.questions.table.points')}
										</TableHead>
										<TableHead>
											{t('pages.dashboard.questions.table.source')}
										</TableHead>
										<TableHead>
											{t('pages.dashboard.questions.table.public')}
										</TableHead>
										<TableHead className='text-right'>
											{t('pages.dashboard.questions.table.actions')}
										</TableHead>
									</TableRow>
								</TableHeader>
								<TableBody>
									{paginated.map(q => (
										<TableRow key={q.id}>
											<TableCell className='max-w-sm'>
												<p className='line-clamp-2 text-sm'>{q.content}</p>
											</TableCell>
											<TableCell>
												<Badge
													variant='outline'
													className='text-xs whitespace-nowrap'
												>
													{questionTypeLabel(q.type)}
												</Badge>
											</TableCell>
											<TableCell>
												<Badge
													variant={CognitiveLevelBadgeVariant[q.cognitiveLevel]}
													className='text-xs whitespace-nowrap'
												>
													{cognitiveLevelLabel(q.cognitiveLevel)}
												</Badge>
											</TableCell>
											<TableCell className='text-sm font-medium'>
												{q.point}
											</TableCell>
											<TableCell className='text-xs text-muted-foreground'>
												{sourceLabel(q.source)}
											</TableCell>
											<TableCell>
												{q.isPublic ? (
													<Eye className='h-4 w-4 text-green-500' />
												) : (
													<EyeOff className='h-4 w-4 text-muted-foreground' />
												)}
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
															onClick={() => setDeletingQuestion(q)}
															className='text-destructive'
														>
															<Trash2 className='h-4 w-4 mr-2' />
															{t(
																'pages.dashboard.questions.delete_dialog.confirm'
															)}
														</DropdownMenuItem>
													</DropdownMenuContent>
												</DropdownMenu>
											</TableCell>
										</TableRow>
									))}
								</TableBody>
							</Table>

							{/* Pagination */}
							{totalPages > 1 && (
								<div className='flex items-center justify-between mt-4'>
									<p className='text-sm text-muted-foreground'>
										{(page - 1) * PAGE_SIZE + 1}–
										{Math.min(page * PAGE_SIZE, filtered.length)} /{' '}
										{filtered.length}
									</p>
									<div className='flex gap-2'>
										<Button
											variant='outline'
											size='sm'
											onClick={() => setPage(p => Math.max(1, p - 1))}
											disabled={page === 1}
										>
											Previous
										</Button>
										<span className='flex items-center px-2 text-sm'>
											{page} / {totalPages}
										</span>
										<Button
											variant='outline'
											size='sm'
											onClick={() => setPage(p => Math.min(totalPages, p + 1))}
											disabled={page === totalPages}
										>
											Next
										</Button>
									</div>
								</div>
							)}
						</>
					) : (
						<div className='text-center py-12'>
							<HelpCircle className='h-12 w-12 mx-auto text-muted-foreground mb-4 opacity-50' />
							<p className='font-medium'>
								{t('pages.dashboard.questions.empty.title')}
							</p>
							<p className='text-sm text-muted-foreground mt-1'>
								{t('pages.dashboard.questions.empty.description')}
							</p>
						</div>
					)}
				</CardContent>
			</Card>

			{/* Create Question Dialog */}
			<Dialog
				open={isCreateOpen}
				onOpenChange={open => {
					if (!open) {
						setIsCreateOpen(false)
						form.reset()
					}
				}}
			>
				<DialogContent className='max-w-4xl max-h-[90vh]'>
					<DialogHeader>
						<DialogTitle>
							{t('pages.dashboard.questions.new_question')}
						</DialogTitle>
						<DialogDescription>
							{t('pages.dashboard.questions.subtitle')}
						</DialogDescription>
					</DialogHeader>
					<Form {...form}>
						<ScrollArea className='max-h-[calc(90vh-180px)] pr-4'>
							<div className='space-y-4 pb-4'>
								<QuestionFormFields form={form} topics={topics} />
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
							</div>
						</ScrollArea>
					</Form>
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => {
								setIsCreateOpen(false)
								form.reset()
							}}
						>
							{t('pages.dashboard.subjects.delete_dialog.cancel')}
						</Button>
						<Button
							onClick={handleCreateSubmit}
							disabled={createQuestion.isPending}
						>
							{createQuestion.isPending
								? t('pages.dashboard.subjects.create_dialog.submitting')
								: t('pages.dashboard.questions.new_question')}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Delete Confirmation */}
			<AlertDialog
				open={!!deletingQuestion}
				onOpenChange={open => !open && setDeletingQuestion(null)}
			>
				<AlertDialogContent>
					<AlertDialogHeader>
						<AlertDialogTitle>
							{t('pages.dashboard.questions.delete_dialog.title')}
						</AlertDialogTitle>
						<AlertDialogDescription>
							{t('pages.dashboard.questions.delete_dialog.description')}
						</AlertDialogDescription>
					</AlertDialogHeader>
					<AlertDialogFooter>
						<AlertDialogCancel>
							{t('pages.dashboard.subjects.delete_dialog.cancel')}
						</AlertDialogCancel>
						<AlertDialogAction
							onClick={handleDelete}
							className='bg-destructive text-destructive-foreground hover:bg-destructive/90'
						>
							{t('pages.dashboard.questions.delete_dialog.confirm')}
						</AlertDialogAction>
					</AlertDialogFooter>
				</AlertDialogContent>
			</AlertDialog>
		</div>
	)
}

export default QuestionsPage
