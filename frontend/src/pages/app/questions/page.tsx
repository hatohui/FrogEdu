import React, { useState, useMemo } from 'react'
import { useTranslation } from 'react-i18next'
import {
	Search,
	HelpCircle,
	Eye,
	EyeOff,
	MoreHorizontal,
	Trash2,
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
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
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
	useSubjects,
	useTopics,
	useQuestions,
	useDeleteQuestion,
} from '@/hooks/useExams'
import { CognitiveLevel, QuestionType } from '@/types/model/exam-service'
import type { Question } from '@/types/model/exam-service'

const PAGE_SIZE = 10

const CognitiveLevelBadgeVariant: Record<
	CognitiveLevel,
	'default' | 'secondary' | 'outline' | 'destructive'
> = {
	[CognitiveLevel.Remember]: 'outline',
	[CognitiveLevel.Understand]: 'secondary',
	[CognitiveLevel.Apply]: 'default',
	[CognitiveLevel.Analyze]: 'destructive',
}

const TeacherQuestionBankPage = (): React.ReactElement => {
	const { t } = useTranslation()

	const [selectedSubjectId, setSelectedSubjectId] = useState<string>('')
	const [selectedTopicId, setSelectedTopicId] = useState<string>('')
	const [selectedLevel, setSelectedLevel] = useState<string>('all')
	const [selectedType, setSelectedType] = useState<string>('all')
	const [search, setSearch] = useState('')
	const [page, setPage] = useState(1)

	const [deletingQuestion, setDeletingQuestion] = useState<Question | null>(
		null
	)

	const { data: subjects = [] } = useSubjects()
	const { data: topics = [] } = useTopics(selectedSubjectId)
	const { data: questions = [], isLoading } = useQuestions({
		topicId: selectedTopicId || undefined,
		cognitiveLevel:
			selectedLevel !== 'all'
				? (Number(selectedLevel) as CognitiveLevel)
				: undefined,
		search: search || undefined,
	})

	const deleteQuestion = useDeleteQuestion()

	const filtered = useMemo(() => {
		if (selectedType === 'all') return questions
		return questions.filter(q => q.type === Number(selectedType))
	}, [questions, selectedType])

	const totalPages = Math.max(1, Math.ceil(filtered.length / PAGE_SIZE))
	const paginated = filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE)

	React.useEffect(() => {
		setPage(1)
	}, [selectedSubjectId, selectedTopicId, selectedLevel, selectedType, search])

	React.useEffect(() => {
		setSelectedTopicId('')
	}, [selectedSubjectId])

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

	const sourceLabel = (source: number) => {
		const map: Record<number, string> = {
			1: 'manual',
			2: 'ai_generated',
			3: 'imported',
			4: 'curriculum_bank',
		}
		return t(`pages.dashboard.questions.sources.${map[source] ?? 'manual'}`)
	}

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div className='space-y-1'>
				<h1 className='text-3xl font-bold tracking-tight flex items-center gap-2'>
					<HelpCircle className='h-8 w-8' />
					{t('navigation.question_bank')}
				</h1>
				<p className='text-muted-foreground'>
					{t('pages.dashboard.questions.subtitle')}
				</p>
			</div>

			{/* Filters */}
			<div className='flex flex-wrap gap-3 items-center'>
				<div className='relative flex-1 min-w-[180px] max-w-xs'>
					<Search className='absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground' />
					<Input
						className='pl-9'
						placeholder={t('pages.dashboard.questions.search_placeholder')}
						value={search}
						onChange={e => setSearch(e.target.value)}
					/>
				</div>

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
			</div>

			{/* Table */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						{t('navigation.question_bank')}
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
											{t('actions.previous')}
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
											{t('actions.next')}
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
						<AlertDialogCancel>{t('actions.cancel')}</AlertDialogCancel>
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

export default TeacherQuestionBankPage
