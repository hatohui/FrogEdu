import React, { useState, useMemo } from 'react'
import { useNavigate } from 'react-router'
import { useTranslation } from 'react-i18next'
import {
	Plus,
	MoreHorizontal,
	Pencil,
	Trash2,
	BookOpen,
	Search,
} from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Skeleton } from '@/components/ui/skeleton'
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
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import {
	useSubjects,
	useCreateSubject,
	useUpdateSubject,
	useDeleteSubject,
} from '@/hooks/useExams'
import type { Subject } from '@/types/model/exam-service'
import type {
	CreateSubjectRequest,
	UpdateSubjectRequest,
} from '@/types/dtos/exams'

const PAGE_SIZE = 10

const SubjectsPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	const [selectedGrade, setSelectedGrade] = useState<number | undefined>(
		undefined
	)
	const [search, setSearch] = useState('')
	const [page, setPage] = useState(1)
	const [editingSubject, setEditingSubject] = useState<Subject | null>(null)
	const [deletingSubject, setDeletingSubject] = useState<Subject | null>(null)
	const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false)
	const [formData, setFormData] = useState<CreateSubjectRequest>({
		subjectCode: '',
		name: '',
		description: '',
		imageUrl: '',
		grade: 1,
	})

	const { data: subjects, isLoading } = useSubjects(selectedGrade)
	const createSubject = useCreateSubject()
	const updateSubject = useUpdateSubject()
	const deleteSubject = useDeleteSubject()

	const filtered = useMemo(() => {
		if (!subjects) return []
		const q = search.toLowerCase()
		if (!q) return subjects
		return subjects.filter(
			s =>
				s.name.toLowerCase().includes(q) ||
				s.subjectCode.toLowerCase().includes(q) ||
				s.description.toLowerCase().includes(q)
		)
	}, [subjects, search])

	const totalPages = Math.max(1, Math.ceil(filtered.length / PAGE_SIZE))
	const paginated = filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE)

	React.useEffect(() => {
		setPage(1)
	}, [search, selectedGrade])

	const resetForm = () =>
		setFormData({
			subjectCode: '',
			name: '',
			description: '',
			imageUrl: '',
			grade: 1,
		})

	const handleCreate = async () => {
		await createSubject.mutateAsync(formData)
		setIsCreateDialogOpen(false)
		resetForm()
		setPage(1)
	}

	const handleUpdate = async () => {
		if (!editingSubject) return
		const updateData: UpdateSubjectRequest = {
			subjectCode: formData.subjectCode,
			name: formData.name,
			description: formData.description,
			imageUrl: formData.imageUrl,
			grade: formData.grade,
		}
		await updateSubject.mutateAsync({
			subjectId: editingSubject.id,
			data: updateData,
		})
		setEditingSubject(null)
	}

	const handleDelete = async () => {
		if (!deletingSubject) return
		await deleteSubject.mutateAsync(deletingSubject.id)
		setDeletingSubject(null)
	}

	const openEditDialog = (subject: Subject) => {
		setEditingSubject(subject)
		setFormData({
			subjectCode: subject.subjectCode,
			name: subject.name,
			description: subject.description,
			imageUrl: subject.imageUrl,
			grade: subject.grade,
		})
	}

	const openCreateDialog = () => {
		setIsCreateDialogOpen(true)
		resetForm()
	}

	const SubjectFormFields = () => (
		<div className='space-y-4 py-4'>
			<div className='grid grid-cols-2 gap-4'>
				<div className='space-y-2'>
					<Label htmlFor='subjectCode'>
						{t('pages.dashboard.subjects.create_dialog.fields.code')}
					</Label>
					<Input
						id='subjectCode'
						value={formData.subjectCode}
						onChange={e =>
							setFormData({ ...formData, subjectCode: e.target.value })
						}
						placeholder='e.g., MATH, VIET'
					/>
				</div>
				<div className='space-y-2'>
					<Label htmlFor='grade'>
						{t('pages.dashboard.subjects.create_dialog.fields.grade')}
					</Label>
					<Select
						value={String(formData.grade)}
						onValueChange={v => setFormData({ ...formData, grade: Number(v) })}
					>
						<SelectTrigger id='grade'>
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
				<Label htmlFor='name'>
					{t('pages.dashboard.subjects.create_dialog.fields.name')}
				</Label>
				<Input
					id='name'
					value={formData.name}
					onChange={e => setFormData({ ...formData, name: e.target.value })}
					placeholder='e.g., Mathematics'
				/>
			</div>
			<div className='space-y-2'>
				<Label htmlFor='description'>
					{t('pages.dashboard.subjects.create_dialog.fields.description')}
				</Label>
				<Input
					id='description'
					value={formData.description}
					onChange={e =>
						setFormData({ ...formData, description: e.target.value })
					}
					placeholder='Brief description'
				/>
			</div>
			<div className='space-y-2'>
				<Label htmlFor='imageUrl'>
					{t('pages.dashboard.subjects.create_dialog.fields.image_url')}
				</Label>
				<Input
					id='imageUrl'
					value={formData.imageUrl}
					onChange={e => setFormData({ ...formData, imageUrl: e.target.value })}
					placeholder='https://...'
				/>
			</div>
		</div>
	)

	return (
		<div className='p-6 space-y-6 max-w-6xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div className='space-y-1'>
					<h1 className='text-3xl font-bold tracking-tight flex items-center gap-2'>
						<BookOpen className='h-8 w-8' />
						{t('pages.dashboard.subjects.title')}
					</h1>
					<p className='text-muted-foreground'>
						{t('pages.dashboard.subjects.subtitle')}
					</p>
				</div>
				<Button onClick={openCreateDialog} size='lg'>
					<Plus className='h-5 w-5 mr-2' />
					{t('pages.dashboard.subjects.new_subject')}
				</Button>
			</div>

			{/* Filters */}
			<div className='flex items-center gap-4 flex-wrap'>
				<div className='relative flex-1 min-w-[200px] max-w-sm'>
					<Search className='absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground' />
					<Input
						className='pl-9'
						placeholder={t('pages.dashboard.subjects.search_placeholder')}
						value={search}
						onChange={e => setSearch(e.target.value)}
					/>
				</div>
				<div className='flex items-center gap-2'>
					<Label>{t('pages.dashboard.subjects.grade_filter_label')}:</Label>
					<Select
						value={selectedGrade?.toString() ?? 'all'}
						onValueChange={v =>
							setSelectedGrade(v === 'all' ? undefined : Number(v))
						}
					>
						<SelectTrigger className='w-36'>
							<SelectValue />
						</SelectTrigger>
						<SelectContent>
							<SelectItem value='all'>
								{t('pages.dashboard.subjects.filter_all_grades')}
							</SelectItem>
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

			{/* Table */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						{t('pages.dashboard.subjects.title')}
						{filtered.length > 0 && (
							<Badge variant='secondary'>{filtered.length}</Badge>
						)}
					</CardTitle>
				</CardHeader>
				<CardContent>
					{isLoading ? (
						<div className='space-y-3'>
							{Array.from({ length: 5 }).map((_, i) => (
								<Skeleton key={i} className='h-12 w-full' />
							))}
						</div>
					) : paginated.length > 0 ? (
						<>
							<Table>
								<TableHeader>
									<TableRow>
										<TableHead>
											{t('pages.dashboard.subjects.table.code')}
										</TableHead>
										<TableHead>
											{t('pages.dashboard.subjects.table.name')}
										</TableHead>
										<TableHead>
											{t('pages.dashboard.subjects.table.description')}
										</TableHead>
										<TableHead>
											{t('pages.dashboard.subjects.table.grade')}
										</TableHead>
										<TableHead className='text-right'>
											{t('pages.dashboard.subjects.table.actions')}
										</TableHead>
									</TableRow>
								</TableHeader>
								<TableBody>
									{paginated.map(subject => (
										<TableRow
											key={subject.id}
											className='cursor-pointer hover:bg-muted/50'
											onClick={() =>
												navigate(`/dashboard/subjects/${subject.id}`)
											}
										>
											<TableCell className='font-medium font-mono'>
												{subject.subjectCode}
											</TableCell>
											<TableCell className='font-medium'>
												{subject.name}
											</TableCell>
											<TableCell className='max-w-xs truncate text-muted-foreground'>
												{subject.description}
											</TableCell>
											<TableCell>
												<Badge variant='outline'>
													{t('pages.dashboard.subject_detail.grade_badge', {
														grade: subject.grade,
													})}
												</Badge>
											</TableCell>
											<TableCell
												className='text-right'
												onClick={e => e.stopPropagation()}
											>
												<DropdownMenu>
													<DropdownMenuTrigger asChild>
														<Button variant='ghost' size='icon'>
															<MoreHorizontal className='h-4 w-4' />
														</Button>
													</DropdownMenuTrigger>
													<DropdownMenuContent align='end'>
														<DropdownMenuItem
															onClick={() => openEditDialog(subject)}
														>
															<Pencil className='h-4 w-4 mr-2' />
															{t('pages.dashboard.subjects.edit_dialog.title')}
														</DropdownMenuItem>
														<DropdownMenuItem
															onClick={() => setDeletingSubject(subject)}
															className='text-destructive'
														>
															<Trash2 className='h-4 w-4 mr-2' />
															{t(
																'pages.dashboard.subjects.delete_dialog.confirm'
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
							<BookOpen className='h-12 w-12 mx-auto text-muted-foreground mb-4 opacity-50' />
							<p className='font-medium'>
								{t('pages.dashboard.subjects.empty.title')}
							</p>
							<p className='text-sm text-muted-foreground mt-1'>
								{t('pages.dashboard.subjects.empty.description')}
							</p>
						</div>
					)}
				</CardContent>
			</Card>

			{/* Create Dialog */}
			<Dialog open={isCreateDialogOpen} onOpenChange={setIsCreateDialogOpen}>
				<DialogContent className='max-w-lg'>
					<DialogHeader>
						<DialogTitle>
							{t('pages.dashboard.subjects.create_dialog.title')}
						</DialogTitle>
						<DialogDescription>
							{t('pages.dashboard.subjects.create_dialog.description')}
						</DialogDescription>
					</DialogHeader>
					<SubjectFormFields />
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => setIsCreateDialogOpen(false)}
						>
							{t('pages.dashboard.subjects.delete_dialog.cancel')}
						</Button>
						<Button
							onClick={handleCreate}
							disabled={
								createSubject.isPending ||
								!formData.name ||
								!formData.subjectCode
							}
						>
							{createSubject.isPending
								? t('pages.dashboard.subjects.create_dialog.submitting')
								: t('pages.dashboard.subjects.create_dialog.submit')}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Edit Dialog */}
			<Dialog
				open={!!editingSubject}
				onOpenChange={open => !open && setEditingSubject(null)}
			>
				<DialogContent className='max-w-lg'>
					<DialogHeader>
						<DialogTitle>
							{t('pages.dashboard.subjects.edit_dialog.title')}
						</DialogTitle>
						<DialogDescription>
							{t('pages.dashboard.subjects.edit_dialog.description')}
						</DialogDescription>
					</DialogHeader>
					<SubjectFormFields />
					<DialogFooter>
						<Button variant='outline' onClick={() => setEditingSubject(null)}>
							{t('pages.dashboard.subjects.delete_dialog.cancel')}
						</Button>
						<Button
							onClick={handleUpdate}
							disabled={
								updateSubject.isPending ||
								!formData.name ||
								!formData.subjectCode
							}
						>
							{updateSubject.isPending
								? t('pages.dashboard.subjects.edit_dialog.submitting')
								: t('pages.dashboard.subjects.edit_dialog.submit')}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Delete Confirmation */}
			<AlertDialog
				open={!!deletingSubject}
				onOpenChange={open => !open && setDeletingSubject(null)}
			>
				<AlertDialogContent>
					<AlertDialogHeader>
						<AlertDialogTitle>
							{t('pages.dashboard.subjects.delete_dialog.title')}
						</AlertDialogTitle>
						<AlertDialogDescription>
							{t('pages.dashboard.subjects.delete_dialog.description', {
								name: deletingSubject?.name ?? '',
							})}
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
							{t('pages.dashboard.subjects.delete_dialog.confirm')}
						</AlertDialogAction>
					</AlertDialogFooter>
				</AlertDialogContent>
			</AlertDialog>
		</div>
	)
}

export default SubjectsPage
