import React, { useState, useMemo } from 'react'
import { useParams, useNavigate } from 'react-router'
import { useTranslation } from 'react-i18next'
import {
	ArrowLeft,
	Plus,
	Pencil,
	Trash2,
	BookOpen,
	Search,
	MoreHorizontal,
} from 'lucide-react'
import {
	Card,
	CardContent,
	CardHeader,
	CardTitle,
	CardDescription,
} from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Skeleton } from '@/components/ui/skeleton'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Separator } from '@/components/ui/separator'
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
import {
	useSubjectById,
	useTopics,
	useUpdateSubject,
	useCreateTopic,
	useUpdateTopic,
	useDeleteTopic,
} from '@/hooks/useExams'
import type { Topic } from '@/types/model/exam-service'
import type {
	UpdateSubjectRequest,
	CreateTopicRequest,
	UpdateTopicRequest,
} from '@/types/dtos/exams'

const SubjectDetailPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const { subjectId } = useParams<{ subjectId: string }>()

	// Subject edit state
	const [isEditSubjectOpen, setIsEditSubjectOpen] = useState(false)
	const [subjectForm, setSubjectForm] = useState<UpdateSubjectRequest>({
		subjectCode: '',
		name: '',
		description: '',
		imageUrl: '',
		grade: 1,
	})

	// Topic state
	const [topicSearch, setTopicSearch] = useState('')
	const [isCreateTopicOpen, setIsCreateTopicOpen] = useState(false)
	const [editingTopic, setEditingTopic] = useState<Topic | null>(null)
	const [deletingTopic, setDeletingTopic] = useState<Topic | null>(null)
	const [topicForm, setTopicForm] = useState<CreateTopicRequest>({
		title: '',
		description: '',
		isCurriculum: false,
		subjectId: subjectId ?? '',
	})

	const { data: subject, isLoading: isLoadingSubject } = useSubjectById(
		subjectId ?? ''
	)
	const { data: topics, isLoading: isLoadingTopics } = useTopics(
		subjectId ?? ''
	)
	const updateSubject = useUpdateSubject()
	const createTopic = useCreateTopic()
	const updateTopic = useUpdateTopic()
	const deleteTopic = useDeleteTopic()

	const filteredTopics = useMemo(() => {
		if (!topics) return []
		const q = topicSearch.toLowerCase()
		if (!q) return topics
		return topics.filter(
			t =>
				t.title.toLowerCase().includes(q) ||
				t.description.toLowerCase().includes(q)
		)
	}, [topics, topicSearch])

	const openEditSubject = () => {
		if (!subject) return
		setSubjectForm({
			subjectCode: subject.subjectCode,
			name: subject.name,
			description: subject.description,
			imageUrl: subject.imageUrl,
			grade: subject.grade,
		})
		setIsEditSubjectOpen(true)
	}

	const handleUpdateSubject = async () => {
		if (!subjectId) return
		await updateSubject.mutateAsync({ subjectId, data: subjectForm })
		setIsEditSubjectOpen(false)
	}

	const openCreateTopic = () => {
		setTopicForm({
			title: '',
			description: '',
			isCurriculum: false,
			subjectId: subjectId ?? '',
		})
		setIsCreateTopicOpen(true)
	}

	const openEditTopic = (topic: Topic) => {
		setEditingTopic(topic)
		setTopicForm({
			title: topic.title,
			description: topic.description,
			isCurriculum: topic.isCurriculum,
			subjectId: subjectId ?? '',
		})
	}

	const handleCreateTopic = async () => {
		await createTopic.mutateAsync(topicForm)
		setIsCreateTopicOpen(false)
	}

	const handleUpdateTopic = async () => {
		if (!editingTopic || !subjectId) return
		const updateData: UpdateTopicRequest = {
			title: topicForm.title,
			description: topicForm.description,
			isCurriculum: topicForm.isCurriculum,
		}
		await updateTopic.mutateAsync({
			subjectId,
			topicId: editingTopic.id,
			data: updateData,
		})
		setEditingTopic(null)
	}

	const handleDeleteTopic = async () => {
		if (!deletingTopic || !subjectId) return
		await deleteTopic.mutateAsync({ subjectId, topicId: deletingTopic.id })
		setDeletingTopic(null)
	}

	const TopicFormFields = () => (
		<div className='space-y-4 py-4'>
			<div className='space-y-2'>
				<Label htmlFor='topic-title'>
					{t('pages.dashboard.subject_detail.create_topic_dialog.fields.title')}
				</Label>
				<Input
					id='topic-title'
					value={topicForm.title}
					onChange={e => setTopicForm({ ...topicForm, title: e.target.value })}
					placeholder='e.g., Fractions and Decimals'
				/>
			</div>
			<div className='space-y-2'>
				<Label htmlFor='topic-description'>
					{t(
						'pages.dashboard.subject_detail.create_topic_dialog.fields.description'
					)}
				</Label>
				<Input
					id='topic-description'
					value={topicForm.description}
					onChange={e =>
						setTopicForm({ ...topicForm, description: e.target.value })
					}
					placeholder='Brief description of the topic'
				/>
			</div>
			<div className='space-y-2'>
				<Label htmlFor='topic-type'>
					{t(
						'pages.dashboard.subject_detail.create_topic_dialog.fields.is_curriculum'
					)}
				</Label>
				<Select
					value={topicForm.isCurriculum ? 'true' : 'false'}
					onValueChange={v =>
						setTopicForm({ ...topicForm, isCurriculum: v === 'true' })
					}
				>
					<SelectTrigger id='topic-type'>
						<SelectValue />
					</SelectTrigger>
					<SelectContent>
						<SelectItem value='true'>
							{t('pages.dashboard.subject_detail.topic_curriculum')}
						</SelectItem>
						<SelectItem value='false'>
							{t('pages.dashboard.subject_detail.topic_custom')}
						</SelectItem>
					</SelectContent>
				</Select>
			</div>
		</div>
	)

	if (isLoadingSubject) {
		return (
			<div className='p-6 space-y-6 max-w-5xl mx-auto'>
				<Skeleton className='h-8 w-32' />
				<Skeleton className='h-24 w-full' />
				<Skeleton className='h-64 w-full' />
			</div>
		)
	}

	if (!subject) {
		return (
			<div className='p-6 text-center'>
				<BookOpen className='h-12 w-12 mx-auto text-muted-foreground mb-4 opacity-50' />
				<p className='font-medium text-lg'>
					{t('pages.dashboard.subject_detail.not_found')}
				</p>
				<Button
					variant='outline'
					className='mt-4'
					onClick={() => navigate('/dashboard/subjects')}
				>
					<ArrowLeft className='h-4 w-4 mr-2' />
					{t('pages.dashboard.subject_detail.back')}
				</Button>
			</div>
		)
	}

	return (
		<div className='p-6 space-y-6 max-w-5xl mx-auto'>
			{/* Back */}
			<Button
				variant='ghost'
				size='sm'
				onClick={() => navigate('/dashboard/subjects')}
			>
				<ArrowLeft className='h-4 w-4 mr-2' />
				{t('pages.dashboard.subject_detail.back')}
			</Button>

			{/* Subject Info */}
			<Card>
				<CardHeader>
					<div className='flex items-start justify-between'>
						<div className='space-y-2'>
							<div className='flex items-center gap-3'>
								<CardTitle className='text-2xl'>{subject.name}</CardTitle>
								<Badge variant='outline'>
									{t('pages.dashboard.subject_detail.grade_badge', {
										grade: subject.grade,
									})}
								</Badge>
								<Badge variant='secondary' className='font-mono'>
									{subject.subjectCode}
								</Badge>
							</div>
							<CardDescription>{subject.description}</CardDescription>
						</div>
						<Button variant='outline' size='sm' onClick={openEditSubject}>
							<Pencil className='h-4 w-4 mr-2' />
							{t('pages.dashboard.subject_detail.edit_subject')}
						</Button>
					</div>
				</CardHeader>
			</Card>

			<Separator />

			{/* Topics Section */}
			<Card>
				<CardHeader>
					<div className='flex items-center justify-between'>
						<div>
							<CardTitle>
								{t('pages.dashboard.subject_detail.topics_title')}
							</CardTitle>
							<CardDescription className='mt-1'>
								{t('pages.dashboard.subject_detail.topics_subtitle')}
							</CardDescription>
						</div>
						<Button size='sm' onClick={openCreateTopic}>
							<Plus className='h-4 w-4 mr-2' />
							{t('pages.dashboard.subject_detail.new_topic')}
						</Button>
					</div>
					{/* Search */}
					<div className='relative mt-3 max-w-sm'>
						<Search className='absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground' />
						<Input
							className='pl-9'
							placeholder={t('pages.dashboard.subject_detail.search_topics')}
							value={topicSearch}
							onChange={e => setTopicSearch(e.target.value)}
						/>
					</div>
				</CardHeader>
				<CardContent>
					{isLoadingTopics ? (
						<div className='space-y-3'>
							{Array.from({ length: 4 }).map((_, i) => (
								<Skeleton key={i} className='h-12 w-full' />
							))}
						</div>
					) : filteredTopics.length > 0 ? (
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead>
										{t('pages.dashboard.subject_detail.table.title')}
									</TableHead>
									<TableHead>
										{t('pages.dashboard.subject_detail.table.description')}
									</TableHead>
									<TableHead>
										{t('pages.dashboard.subject_detail.table.type')}
									</TableHead>
									<TableHead className='text-right'>
										{t('pages.dashboard.subject_detail.table.actions')}
									</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{filteredTopics.map(topic => (
									<TableRow key={topic.id}>
										<TableCell className='font-medium'>{topic.title}</TableCell>
										<TableCell className='max-w-sm truncate text-muted-foreground'>
											{topic.description}
										</TableCell>
										<TableCell>
											{topic.isCurriculum ? (
												<Badge variant='default'>
													{t('pages.dashboard.subject_detail.topic_curriculum')}
												</Badge>
											) : (
												<Badge variant='secondary'>
													{t('pages.dashboard.subject_detail.topic_custom')}
												</Badge>
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
														onClick={() => openEditTopic(topic)}
													>
														<Pencil className='h-4 w-4 mr-2' />
														{t(
															'pages.dashboard.subject_detail.edit_topic_dialog.title'
														)}
													</DropdownMenuItem>
													<DropdownMenuItem
														onClick={() => setDeletingTopic(topic)}
														className='text-destructive'
													>
														<Trash2 className='h-4 w-4 mr-2' />
														{t(
															'pages.dashboard.subject_detail.delete_topic_dialog.confirm'
														)}
													</DropdownMenuItem>
												</DropdownMenuContent>
											</DropdownMenu>
										</TableCell>
									</TableRow>
								))}
							</TableBody>
						</Table>
					) : (
						<div className='text-center py-12'>
							<BookOpen className='h-10 w-10 mx-auto text-muted-foreground mb-3 opacity-50' />
							<p className='font-medium'>
								{t('pages.dashboard.subject_detail.topics_empty.title')}
							</p>
							<p className='text-sm text-muted-foreground mt-1'>
								{t('pages.dashboard.subject_detail.topics_empty.description')}
							</p>
						</div>
					)}
				</CardContent>
			</Card>

			{/* Edit Subject Dialog */}
			<Dialog open={isEditSubjectOpen} onOpenChange={setIsEditSubjectOpen}>
				<DialogContent className='max-w-lg'>
					<DialogHeader>
						<DialogTitle>
							{t('pages.dashboard.subject_detail.edit_subject')}
						</DialogTitle>
						<DialogDescription>
							{t('pages.dashboard.subjects.edit_dialog.description')}
						</DialogDescription>
					</DialogHeader>
					<div className='space-y-4 py-4'>
						<div className='grid grid-cols-2 gap-4'>
							<div className='space-y-2'>
								<Label htmlFor='edit-code'>
									{t('pages.dashboard.subjects.create_dialog.fields.code')}
								</Label>
								<Input
									id='edit-code'
									value={subjectForm.subjectCode}
									onChange={e =>
										setSubjectForm({
											...subjectForm,
											subjectCode: e.target.value,
										})
									}
								/>
							</div>
							<div className='space-y-2'>
								<Label htmlFor='edit-grade'>
									{t('pages.dashboard.subjects.create_dialog.fields.grade')}
								</Label>
								<Select
									value={String(subjectForm.grade)}
									onValueChange={v =>
										setSubjectForm({ ...subjectForm, grade: Number(v) })
									}
								>
									<SelectTrigger id='edit-grade'>
										<SelectValue />
									</SelectTrigger>
									<SelectContent>
										{Array.from({ length: 5 }, (_, i) => i + 1).map(g => (
											<SelectItem key={g} value={String(g)}>
												{t('pages.dashboard.subject_detail.grade_badge', {
													grade: g,
												})}
											</SelectItem>
										))}
									</SelectContent>
								</Select>
							</div>
						</div>
						<div className='space-y-2'>
							<Label htmlFor='edit-name'>
								{t('pages.dashboard.subjects.create_dialog.fields.name')}
							</Label>
							<Input
								id='edit-name'
								value={subjectForm.name}
								onChange={e =>
									setSubjectForm({ ...subjectForm, name: e.target.value })
								}
							/>
						</div>
						<div className='space-y-2'>
							<Label htmlFor='edit-description'>
								{t('pages.dashboard.subjects.create_dialog.fields.description')}
							</Label>
							<Input
								id='edit-description'
								value={subjectForm.description}
								onChange={e =>
									setSubjectForm({
										...subjectForm,
										description: e.target.value,
									})
								}
							/>
						</div>
						<div className='space-y-2'>
							<Label htmlFor='edit-imageUrl'>
								{t('pages.dashboard.subjects.create_dialog.fields.image_url')}
							</Label>
							<Input
								id='edit-imageUrl'
								value={subjectForm.imageUrl}
								onChange={e =>
									setSubjectForm({ ...subjectForm, imageUrl: e.target.value })
								}
							/>
						</div>
					</div>
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => setIsEditSubjectOpen(false)}
						>
							{t('pages.dashboard.subjects.delete_dialog.cancel')}
						</Button>
						<Button
							onClick={handleUpdateSubject}
							disabled={
								updateSubject.isPending ||
								!subjectForm.name ||
								!subjectForm.subjectCode
							}
						>
							{updateSubject.isPending
								? t('pages.dashboard.subjects.edit_dialog.submitting')
								: t('pages.dashboard.subjects.edit_dialog.submit')}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Create Topic Dialog */}
			<Dialog open={isCreateTopicOpen} onOpenChange={setIsCreateTopicOpen}>
				<DialogContent className='max-w-lg'>
					<DialogHeader>
						<DialogTitle>
							{t('pages.dashboard.subject_detail.create_topic_dialog.title')}
						</DialogTitle>
						<DialogDescription>
							{t(
								'pages.dashboard.subject_detail.create_topic_dialog.description'
							)}
						</DialogDescription>
					</DialogHeader>
					<TopicFormFields />
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => setIsCreateTopicOpen(false)}
						>
							{t('pages.dashboard.subjects.delete_dialog.cancel')}
						</Button>
						<Button
							onClick={handleCreateTopic}
							disabled={createTopic.isPending || !topicForm.title}
						>
							{createTopic.isPending
								? t(
										'pages.dashboard.subject_detail.create_topic_dialog.submitting'
									)
								: t(
										'pages.dashboard.subject_detail.create_topic_dialog.submit'
									)}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Edit Topic Dialog */}
			<Dialog
				open={!!editingTopic}
				onOpenChange={open => !open && setEditingTopic(null)}
			>
				<DialogContent className='max-w-lg'>
					<DialogHeader>
						<DialogTitle>
							{t('pages.dashboard.subject_detail.edit_topic_dialog.title')}
						</DialogTitle>
						<DialogDescription>
							{t(
								'pages.dashboard.subject_detail.edit_topic_dialog.description'
							)}
						</DialogDescription>
					</DialogHeader>
					<TopicFormFields />
					<DialogFooter>
						<Button variant='outline' onClick={() => setEditingTopic(null)}>
							{t('pages.dashboard.subjects.delete_dialog.cancel')}
						</Button>
						<Button
							onClick={handleUpdateTopic}
							disabled={updateTopic.isPending || !topicForm.title}
						>
							{updateTopic.isPending
								? t(
										'pages.dashboard.subject_detail.edit_topic_dialog.submitting'
									)
								: t('pages.dashboard.subject_detail.edit_topic_dialog.submit')}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Delete Topic Confirmation */}
			<AlertDialog
				open={!!deletingTopic}
				onOpenChange={open => !open && setDeletingTopic(null)}
			>
				<AlertDialogContent>
					<AlertDialogHeader>
						<AlertDialogTitle>
							{t('pages.dashboard.subject_detail.delete_topic_dialog.title')}
						</AlertDialogTitle>
						<AlertDialogDescription>
							{t(
								'pages.dashboard.subject_detail.delete_topic_dialog.description',
								{
									title: deletingTopic?.title ?? '',
								}
							)}
						</AlertDialogDescription>
					</AlertDialogHeader>
					<AlertDialogFooter>
						<AlertDialogCancel>
							{t('pages.dashboard.subjects.delete_dialog.cancel')}
						</AlertDialogCancel>
						<AlertDialogAction
							onClick={handleDeleteTopic}
							className='bg-destructive text-destructive-foreground hover:bg-destructive/90'
						>
							{t('pages.dashboard.subject_detail.delete_topic_dialog.confirm')}
						</AlertDialogAction>
					</AlertDialogFooter>
				</AlertDialogContent>
			</AlertDialog>
		</div>
	)
}

export default SubjectDetailPage
