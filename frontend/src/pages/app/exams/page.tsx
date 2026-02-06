import React, { useState } from 'react'
import {
	Plus,
	FileText,
	BookOpen,
	CheckCircle2,
	MoreHorizontal,
	Pencil,
	Trash2,
	Eye,
} from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { ExamListSkeleton } from '@/components/common/skeletons'
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
	useExams,
	useUpdateExam,
	useDeleteExam,
	useSubjects,
} from '@/hooks/useExams'
import { Badge } from '@/components/ui/badge'
import { useNavigate } from 'react-router'
import type { Exam } from '@/types/model/exam-service'
import type { CreateExamRequest } from '@/types/dtos/exams'
import { useTranslation } from 'react-i18next'

const ExamsPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const [filter, setFilter] = useState<'all' | 'draft' | 'published'>('all')
	const [editingExam, setEditingExam] = useState<Exam | null>(null)
	const [deletingExam, setDeletingExam] = useState<Exam | null>(null)
	const [openMenuId, setOpenMenuId] = useState<string | null>(null)
	const [formData, setFormData] = useState<Partial<CreateExamRequest>>({
		name: '',
		description: '',
		subjectId: '',
		grade: 10,
	})

	const isDraft =
		filter === 'draft' ? true : filter === 'published' ? false : undefined
	const { data: exams, isLoading } = useExams(isDraft)
	const { data: subjects } = useSubjects(formData.grade)
	const updateExam = useUpdateExam()
	const deleteExam = useDeleteExam()

	const handleCreateExam = () => {
		navigate('/app/exams/create')
	}

	const handleUpdate = async () => {
		if (!editingExam) return
		await updateExam.mutateAsync({
			examId: editingExam.id,
			data: formData,
		})
		setEditingExam(null)
	}

	const handleDelete = async () => {
		if (!deletingExam) return
		await deleteExam.mutateAsync(deletingExam.id)
		setDeletingExam(null)
	}

	const openEditDialog = (exam: Exam) => {
		setEditingExam(exam)
		setFormData({
			name: exam.name,
			description: exam.description,
			subjectId: exam.subjectId,
			grade: exam.grade,
		})
	}

	// Ensure formData.subjectId is valid when subjects change
	React.useEffect(() => {
		if (editingExam && subjects && subjects.length > 0) {
			const subjectExists = subjects.some(s => s.id === formData.subjectId)
			if (!subjectExists && formData.subjectId) {
				// If the current subjectId doesn't exist in the loaded subjects, keep it
				// This handles cross-grade subject selection
			}
		}
	}, [subjects, editingExam, formData.subjectId])

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div className='space-y-2'>
					<h1 className='text-3xl font-bold tracking-tight flex items-center space-x-2'>
						<FileText className='h-8 w-8' />
						<span>{t('pages.exams.list.title')}</span>
					</h1>
					<p className='text-muted-foreground'>
						{t('pages.exams.list.subtitle')}
					</p>
				</div>
				<Button onClick={handleCreateExam} size='lg'>
					<Plus className='h-5 w-5 mr-2' />
					{t('pages.exams.list.actions.new')}
				</Button>
			</div>

			{/* Filter Tabs */}
			<div className='flex space-x-2'>
				<Button
					variant={filter === 'all' ? 'default' : 'outline'}
					onClick={() => setFilter('all')}
				>
					{t('pages.exams.list.filters.all')}
				</Button>
				<Button
					variant={filter === 'draft' ? 'default' : 'outline'}
					onClick={() => setFilter('draft')}
				>
					{t('pages.exams.list.filters.draft')}
				</Button>
				<Button
					variant={filter === 'published' ? 'default' : 'outline'}
					onClick={() => setFilter('published')}
				>
					{t('pages.exams.list.filters.published')}
				</Button>
			</div>

			{/* Exams Table */}
			<Card>
				<CardHeader>
					<CardTitle>{t('pages.exams.list.table.title')}</CardTitle>
				</CardHeader>
				<CardContent>
					{isLoading ? (
						<ExamListSkeleton />
					) : exams && exams.length > 0 ? (
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead>{t('pages.exams.list.table.name')}</TableHead>
									<TableHead>{t('pages.exams.list.table.subject')}</TableHead>
									<TableHead>{t('pages.exams.list.table.grade')}</TableHead>
									<TableHead>{t('pages.exams.list.table.status')}</TableHead>
									<TableHead>{t('pages.exams.list.table.created')}</TableHead>
									<TableHead className='text-right'>
										{t('pages.exams.list.table.actions')}
									</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{exams.map(exam => (
									<TableRow
										key={exam.id}
										className='cursor-pointer hover:bg-muted/50'
										onClick={() => navigate(`/app/exams/${exam.id}`)}
										onContextMenu={e => {
											e.preventDefault()
											setOpenMenuId(exam.id)
										}}
									>
										<TableCell className='font-medium'>{exam.name}</TableCell>
										<TableCell>-</TableCell>
										<TableCell>{exam.grade}</TableCell>
										<TableCell>
											{exam.isDraft ? (
												<Badge variant='secondary'>
													<BookOpen className='h-3 w-3 mr-1' />
													{t('pages.exams.list.status.draft')}
												</Badge>
											) : (
												<Badge variant='default'>
													<CheckCircle2 className='h-3 w-3 mr-1' />
													{t('pages.exams.list.status.active')}
												</Badge>
											)}
										</TableCell>
										<TableCell>
											{new Date(exam.createdAt).toLocaleDateString()}
										</TableCell>
										<TableCell
											className='text-right'
											onClick={e => e.stopPropagation()}
										>
											<DropdownMenu
												open={openMenuId === exam.id}
												onOpenChange={open =>
													setOpenMenuId(open ? exam.id : null)
												}
											>
												<DropdownMenuTrigger asChild>
													<Button variant='ghost' size='icon'>
														<MoreHorizontal className='h-4 w-4' />
													</Button>
												</DropdownMenuTrigger>
												<DropdownMenuContent align='end'>
													<DropdownMenuItem
														onClick={() => navigate(`/app/exams/${exam.id}`)}
													>
														<Eye className='h-4 w-4 mr-2' />
														{t('pages.exams.list.actions.view')}
													</DropdownMenuItem>
													<DropdownMenuItem
														onClick={() => openEditDialog(exam)}
													>
														<Pencil className='h-4 w-4 mr-2' />
														{t('pages.exams.list.actions.edit')}
													</DropdownMenuItem>
													<DropdownMenuItem
														onClick={() => setDeletingExam(exam)}
														className='text-destructive'
													>
														<Trash2 className='h-4 w-4 mr-2' />
														{t('pages.exams.list.actions.delete')}
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
							<FileText className='h-12 w-12 mx-auto text-muted-foreground mb-4' />
							<p className='text-muted-foreground mb-4'>
								{t('pages.exams.list.empty.title')}
							</p>
							<Button onClick={handleCreateExam}>
								<Plus className='h-4 w-4 mr-2' />
								{t('pages.exams.list.empty.action')}
							</Button>
						</div>
					)}
				</CardContent>
			</Card>

			{/* Edit Dialog */}
			<Dialog
				open={!!editingExam}
				onOpenChange={open => !open && setEditingExam(null)}
			>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>{t('pages.exams.list.edit.title')}</DialogTitle>
						<DialogDescription>
							{t('pages.exams.list.edit.description')}
						</DialogDescription>
					</DialogHeader>
					<div className='space-y-4 py-4'>
						<div className='space-y-2'>
							<Label htmlFor='name'>
								{t('pages.exams.list.edit.fields.name')}
							</Label>
							<Input
								id='name'
								value={formData.name}
								onChange={e =>
									setFormData({ ...formData, name: e.target.value })
								}
								placeholder={t('pages.exams.list.edit.placeholders.name')}
							/>
						</div>
						<div className='space-y-2'>
							<Label htmlFor='description'>
								{t('pages.exams.list.edit.fields.description')}
							</Label>
							<Input
								id='description'
								value={formData.description}
								onChange={e =>
									setFormData({ ...formData, description: e.target.value })
								}
								placeholder={t(
									'pages.exams.list.edit.placeholders.description'
								)}
							/>
						</div>
						<div className='space-y-2'>
							<Label htmlFor='grade'>
								{t('pages.exams.list.edit.fields.grade')}
							</Label>
							<Select
								value={formData.grade?.toString()}
								onValueChange={value =>
									setFormData({ ...formData, grade: Number(value) })
								}
							>
								<SelectTrigger id='grade'>
									<SelectValue />
								</SelectTrigger>
								<SelectContent>
									{[1, 2, 3, 4, 5].map(grade => (
										<SelectItem key={grade} value={grade.toString()}>
											{t('pages.exams.list.edit.grade_option', { grade })}
										</SelectItem>
									))}
								</SelectContent>
							</Select>
						</div>
						<div className='space-y-2'>
							<Label htmlFor='subject'>
								{t('pages.exams.list.edit.fields.subject')}
							</Label>
							<Select
								value={formData.subjectId || undefined}
								onValueChange={value =>
									setFormData({ ...formData, subjectId: value })
								}
							>
								<SelectTrigger id='subject'>
									<SelectValue
										placeholder={t(
											'pages.exams.list.edit.placeholders.subject'
										)}
									/>
								</SelectTrigger>
								<SelectContent>
									{subjects?.map(subject => (
										<SelectItem key={subject.id} value={subject.id}>
											{subject.name}
										</SelectItem>
									))}
								</SelectContent>
							</Select>
						</div>
					</div>
					<DialogFooter>
						<Button variant='outline' onClick={() => setEditingExam(null)}>
							{t('common.cancel')}
						</Button>
						<Button
							onClick={handleUpdate}
							disabled={updateExam.isPending || !formData.name}
						>
							{t('pages.exams.list.edit.actions.update')}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Delete Confirmation Dialog */}
			<AlertDialog
				open={!!deletingExam}
				onOpenChange={open => !open && setDeletingExam(null)}
			>
				<AlertDialogContent>
					<AlertDialogHeader>
						<AlertDialogTitle>
							{t('pages.exams.list.delete.title')}
						</AlertDialogTitle>
						<AlertDialogDescription>
							{t('pages.exams.list.delete.description', {
								name: deletingExam?.name,
							})}
						</AlertDialogDescription>
					</AlertDialogHeader>
					<AlertDialogFooter>
						<AlertDialogCancel>{t('common.cancel')}</AlertDialogCancel>
						<AlertDialogAction
							onClick={handleDelete}
							disabled={deleteExam.isPending}
							className='bg-destructive text-destructive-foreground hover:bg-destructive/90'
						>
							{t('pages.exams.list.actions.delete')}
						</AlertDialogAction>
					</AlertDialogFooter>
				</AlertDialogContent>
			</AlertDialog>
		</div>
	)
}

export default ExamsPage
