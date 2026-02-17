import React, { useState } from 'react'
import { Plus, MoreHorizontal, Pencil, Trash2, BookOpen } from 'lucide-react'
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

const SubjectsPage = (): React.ReactElement => {
	const [selectedGrade, setSelectedGrade] = useState<number | undefined>(
		undefined
	)
	const [editingSubject, setEditingSubject] = useState<Subject | null>(null)
	const [deletingSubject, setDeletingSubject] = useState<Subject | null>(null)
	const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false)
	const [formData, setFormData] = useState<CreateSubjectRequest>({
		subjectCode: '',
		name: '',
		description: '',
		imageUrl: '',
		grade: 10,
	})

	const { data: subjects, isLoading } = useSubjects(selectedGrade)
	const createSubject = useCreateSubject()
	const updateSubject = useUpdateSubject()
	const deleteSubject = useDeleteSubject()

	const handleCreate = async () => {
		await createSubject.mutateAsync(formData)
		setIsCreateDialogOpen(false)
		setFormData({
			subjectCode: '',
			name: '',
			description: '',
			imageUrl: '',
			grade: 10,
		})
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
		setFormData({
			subjectCode: '',
			name: '',
			description: '',
			imageUrl: '',
			grade: 10,
		})
	}

	return (
		<div className='p-6 space-y-6 max-w-6xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div className='space-y-2'>
					<h1 className='text-3xl font-bold tracking-tight flex items-center space-x-2'>
						<BookOpen className='h-8 w-8' />
						<span>Subjects</span>
					</h1>
					<p className='text-muted-foreground'>
						Manage subjects and their topics
					</p>
				</div>
				<Button onClick={openCreateDialog} size='lg'>
					<Plus className='h-5 w-5 mr-2' />
					New Subject
				</Button>
			</div>

			{/* Grade Filter */}
			<div className='flex items-center space-x-4'>
				<Label htmlFor='grade-filter'>Filter by Grade:</Label>
				<Select
					value={selectedGrade?.toString() ?? 'all'}
					onValueChange={value =>
						setSelectedGrade(value === 'all' ? undefined : Number(value))
					}
				>
					<SelectTrigger id='grade-filter' className='w-40'>
						<SelectValue placeholder='All Grades' />
					</SelectTrigger>
					<SelectContent>
						<SelectItem value='all'>All Grades</SelectItem>
						{Array.from({ length: 5 }, (_, i) => i + 1).map(grade => (
							<SelectItem key={grade} value={grade.toString()}>
								Grade {grade}
							</SelectItem>
						))}
					</SelectContent>
				</Select>
			</div>

			{/* Subjects Table */}
			<Card>
				<CardHeader>
					<CardTitle>All Subjects</CardTitle>
				</CardHeader>
				<CardContent>
					{isLoading ? (
						<div className='text-center py-8 text-muted-foreground'>
							Loading subjects...
						</div>
					) : subjects && subjects.length > 0 ? (
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead>Code</TableHead>
									<TableHead>Name</TableHead>
									<TableHead>Description</TableHead>
									<TableHead>Grade</TableHead>
									<TableHead className='text-right'>Actions</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{subjects.map(subject => (
									<TableRow key={subject.id}>
										<TableCell className='font-medium'>
											{subject.subjectCode}
										</TableCell>
										<TableCell>{subject.name}</TableCell>
										<TableCell className='max-w-md truncate'>
											{subject.description}
										</TableCell>
										<TableCell>{subject.grade}</TableCell>
										<TableCell className='text-right'>
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
														Edit
													</DropdownMenuItem>
													<DropdownMenuItem
														onClick={() => setDeletingSubject(subject)}
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
						<div className='text-center py-8 text-muted-foreground'>
							No subjects found. Create one to get started.
						</div>
					)}
				</CardContent>
			</Card>

			{/* Create/Edit Dialog */}
			<Dialog
				open={isCreateDialogOpen || !!editingSubject}
				onOpenChange={open => {
					if (!open) {
						setIsCreateDialogOpen(false)
						setEditingSubject(null)
					}
				}}
			>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>
							{editingSubject ? 'Edit Subject' : 'Create New Subject'}
						</DialogTitle>
						<DialogDescription>
							{editingSubject
								? 'Update the subject information below.'
								: 'Fill in the details to create a new subject.'}
						</DialogDescription>
					</DialogHeader>
					<div className='space-y-4 py-4'>
						<div className='space-y-2'>
							<Label htmlFor='subjectCode'>Subject Code</Label>
							<Input
								id='subjectCode'
								value={formData.subjectCode}
								onChange={e =>
									setFormData({ ...formData, subjectCode: e.target.value })
								}
								placeholder='e.g., MATH, PHYS'
							/>
						</div>
						<div className='space-y-2'>
							<Label htmlFor='name'>Name</Label>
							<Input
								id='name'
								value={formData.name}
								onChange={e =>
									setFormData({ ...formData, name: e.target.value })
								}
								placeholder='e.g., Mathematics'
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
								placeholder='Brief description of the subject'
							/>
						</div>
						<div className='space-y-2'>
							<Label htmlFor='imageUrl'>Image URL</Label>
							<Input
								id='imageUrl'
								value={formData.imageUrl}
								onChange={e =>
									setFormData({ ...formData, imageUrl: e.target.value })
								}
								placeholder='https://example.com/image.jpg'
							/>
						</div>
						<div className='space-y-2'>
							<Label htmlFor='grade'>Grade</Label>
							<Select
								value={formData.grade.toString()}
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
					</div>
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => {
								setIsCreateDialogOpen(false)
								setEditingSubject(null)
							}}
						>
							Cancel
						</Button>
						<Button
							onClick={editingSubject ? handleUpdate : handleCreate}
							disabled={
								createSubject.isPending ||
								updateSubject.isPending ||
								!formData.name ||
								!formData.subjectCode
							}
						>
							{editingSubject ? 'Update' : 'Create'}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Delete Confirmation Dialog */}
			<AlertDialog
				open={!!deletingSubject}
				onOpenChange={open => !open && setDeletingSubject(null)}
			>
				<AlertDialogContent>
					<AlertDialogHeader>
						<AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
						<AlertDialogDescription>
							This will permanently delete the subject &quot;
							{deletingSubject?.name}&quot; and all its associated topics. This
							action cannot be undone.
						</AlertDialogDescription>
					</AlertDialogHeader>
					<AlertDialogFooter>
						<AlertDialogCancel>Cancel</AlertDialogCancel>
						<AlertDialogAction
							onClick={handleDelete}
							disabled={deleteSubject.isPending}
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

export default SubjectsPage
