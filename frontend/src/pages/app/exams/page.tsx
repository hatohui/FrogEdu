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

const ExamsPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const [filter, setFilter] = useState<'all' | 'draft' | 'published'>('all')
	const [editingExam, setEditingExam] = useState<Exam | null>(null)
	const [deletingExam, setDeletingExam] = useState<Exam | null>(null)
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

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div className='space-y-2'>
					<h1 className='text-3xl font-bold tracking-tight flex items-center space-x-2'>
						<FileText className='h-8 w-8' />
						<span>Exams</span>
					</h1>
					<p className='text-muted-foreground'>
						Create and manage your assessments
					</p>
				</div>
				<Button onClick={handleCreateExam} size='lg'>
					<Plus className='h-5 w-5 mr-2' />
					New Exam
				</Button>
			</div>

			{/* Filter Tabs */}
			<div className='flex space-x-2'>
				<Button
					variant={filter === 'all' ? 'default' : 'outline'}
					onClick={() => setFilter('all')}
				>
					All Exams
				</Button>
				<Button
					variant={filter === 'draft' ? 'default' : 'outline'}
					onClick={() => setFilter('draft')}
				>
					Drafts
				</Button>
				<Button
					variant={filter === 'published' ? 'default' : 'outline'}
					onClick={() => setFilter('published')}
				>
					Published
				</Button>
			</div>

			{/* Exams Table */}
			<Card>
				<CardHeader>
					<CardTitle>Your Exams</CardTitle>
				</CardHeader>
				<CardContent>
					{isLoading ? (
						<div className='text-center py-8 text-muted-foreground'>
							Loading exams...
						</div>
					) : exams && exams.length > 0 ? (
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead>Name</TableHead>
									<TableHead>Subject</TableHead>
									<TableHead>Grade</TableHead>
									<TableHead>Status</TableHead>
									<TableHead>Created</TableHead>
									<TableHead className='text-right'>Actions</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{exams.map(exam => (
									<TableRow key={exam.id}>
										<TableCell className='font-medium'>{exam.name}</TableCell>
										<TableCell>-</TableCell>
										<TableCell>{exam.grade}</TableCell>
										<TableCell>
											{exam.isDraft ? (
												<Badge variant='secondary'>
													<BookOpen className='h-3 w-3 mr-1' />
													Draft
												</Badge>
											) : (
												<Badge variant='default'>
													<CheckCircle2 className='h-3 w-3 mr-1' />
													Active
												</Badge>
											)}
										</TableCell>
										<TableCell>
											{new Date(exam.createdAt).toLocaleDateString()}
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
														onClick={() => navigate(`/app/exams/${exam.id}`)}
													>
														<Eye className='h-4 w-4 mr-2' />
														View
													</DropdownMenuItem>
													<DropdownMenuItem
														onClick={() => openEditDialog(exam)}
													>
														<Pencil className='h-4 w-4 mr-2' />
														Edit
													</DropdownMenuItem>
													<DropdownMenuItem
														onClick={() => setDeletingExam(exam)}
														className='text-destructive'
													>
														<Trash2 className='h-4 w-4 mr-2' />
														Delete
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
							<p className='text-muted-foreground mb-4'>No exams found</p>
							<Button onClick={handleCreateExam}>
								<Plus className='h-4 w-4 mr-2' />
								Create Your First Exam
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
						<DialogTitle>Edit Exam</DialogTitle>
						<DialogDescription>
							Update the exam information below.
						</DialogDescription>
					</DialogHeader>
					<div className='space-y-4 py-4'>
						<div className='space-y-2'>
							<Label htmlFor='name'>Name</Label>
							<Input
								id='name'
								value={formData.name}
								onChange={e =>
									setFormData({ ...formData, name: e.target.value })
								}
								placeholder='Exam name'
							/>
						</div>
						<div className='space-y-2'>
							<Label htmlFor='description'>Description</Label>
							<Input
								id='description'
								value={formData.description}
								onChange={e =>
									setFormData({ ...formData, description: e.target.value })
								}
								placeholder='Exam description'
							/>
						</div>
						<div className='space-y-2'>
							<Label htmlFor='grade'>Grade</Label>
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
									{[10, 11, 12].map(grade => (
										<SelectItem key={grade} value={grade.toString()}>
											Grade {grade}
										</SelectItem>
									))}
								</SelectContent>
							</Select>
						</div>
						<div className='space-y-2'>
							<Label htmlFor='subject'>Subject</Label>
							<Select
								value={formData.subjectId}
								onValueChange={value =>
									setFormData({ ...formData, subjectId: value })
								}
							>
								<SelectTrigger id='subject'>
									<SelectValue placeholder='Select a subject' />
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
							Cancel
						</Button>
						<Button
							onClick={handleUpdate}
							disabled={updateExam.isPending || !formData.name}
						>
							Update
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
						<AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
						<AlertDialogDescription>
							This will permanently delete the exam &quot;{deletingExam?.name}
							&quot;. This action cannot be undone.
						</AlertDialogDescription>
					</AlertDialogHeader>
					<AlertDialogFooter>
						<AlertDialogCancel>Cancel</AlertDialogCancel>
						<AlertDialogAction
							onClick={handleDelete}
							disabled={deleteExam.isPending}
							className='bg-destructive text-destructive-foreground hover:bg-destructive/90'
						>
							Delete
						</AlertDialogAction>
					</AlertDialogFooter>
				</AlertDialogContent>
			</AlertDialog>
		</div>
	)
}

export default ExamsPage
