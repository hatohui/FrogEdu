import React from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogFooter,
	DialogHeader,
	DialogTitle,
} from '@/components/ui/dialog'
import {
	Form,
	FormControl,
	FormDescription,
	FormField,
	FormItem,
	FormLabel,
	FormMessage,
} from '@/components/ui/form'
import { Input } from '@/components/ui/input'
import { Button } from '@/components/ui/button'
import { Textarea } from '@/components/ui/textarea'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { Loader2 } from 'lucide-react'
import { useCreateClass } from '@/hooks/useClasses'

const createClassSchema = z.object({
	name: z
		.string()
		.min(1, 'Class name is required')
		.max(100, 'Name cannot exceed 100 characters'),
	subject: z.string().max(50, 'Subject cannot exceed 50 characters').optional(),
	grade: z.string().min(1, 'Grade is required'),
	school: z
		.string()
		.max(100, 'School name cannot exceed 100 characters')
		.optional(),
	description: z
		.string()
		.max(500, 'Description cannot exceed 500 characters')
		.optional(),
	maxStudents: z.string().optional(),
})

type CreateClassFormData = z.infer<typeof createClassSchema>

interface CreateClassModalProps {
	open: boolean
	onOpenChange: (open: boolean) => void
}

const SUBJECTS = [
	'Mathematics',
	'English',
	'Science',
	'History',
	'Geography',
	'Physics',
	'Chemistry',
	'Biology',
	'Literature',
	'Art',
	'Music',
	'Physical Education',
	'Computer Science',
	'Other',
]

const CreateClassModal: React.FC<CreateClassModalProps> = ({
	open,
	onOpenChange,
}) => {
	const createClass = useCreateClass()

	const form = useForm<CreateClassFormData>({
		resolver: zodResolver(createClassSchema),
		defaultValues: {
			name: '',
			subject: '',
			grade: '1',
			school: '',
			description: '',
			maxStudents: '',
		},
	})

	const onSubmit = async (data: CreateClassFormData) => {
		await createClass.mutateAsync({
			name: data.name,
			subject: data.subject || undefined,
			grade: parseInt(data.grade, 10),
			school: data.school || undefined,
			description: data.description || undefined,
			maxStudents: data.maxStudents
				? parseInt(data.maxStudents, 10)
				: undefined,
		})
		form.reset()
		onOpenChange(false)
	}

	return (
		<Dialog open={open} onOpenChange={onOpenChange}>
			<DialogContent className='sm:max-w-[500px]'>
				<DialogHeader>
					<DialogTitle>Create New Class</DialogTitle>
					<DialogDescription>
						Create a new class and get an invite code to share with your
						students.
					</DialogDescription>
				</DialogHeader>

				<Form {...form}>
					<form onSubmit={form.handleSubmit(onSubmit)} className='space-y-4'>
						<FormField
							control={form.control}
							name='name'
							render={({ field }) => (
								<FormItem>
									<FormLabel>Class Name *</FormLabel>
									<FormControl>
										<Input
											placeholder='e.g., Math 101 - Fall 2026'
											{...field}
										/>
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>

						<div className='grid grid-cols-2 gap-4'>
							<FormField
								control={form.control}
								name='grade'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Grade Level *</FormLabel>
										<Select
											onValueChange={field.onChange}
											defaultValue={field.value}
										>
											<FormControl>
												<SelectTrigger>
													<SelectValue placeholder='Select grade' />
												</SelectTrigger>
											</FormControl>
											<SelectContent>
												{Array.from({ length: 12 }, (_, i) => i + 1).map(
													grade => (
														<SelectItem key={grade} value={grade.toString()}>
															Grade {grade}
														</SelectItem>
													)
												)}
											</SelectContent>
										</Select>
										<FormMessage />
									</FormItem>
								)}
							/>

							<FormField
								control={form.control}
								name='subject'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Subject</FormLabel>
										<Select onValueChange={field.onChange} value={field.value}>
											<FormControl>
												<SelectTrigger>
													<SelectValue placeholder='Select subject' />
												</SelectTrigger>
											</FormControl>
											<SelectContent>
												{SUBJECTS.map(subject => (
													<SelectItem key={subject} value={subject}>
														{subject}
													</SelectItem>
												))}
											</SelectContent>
										</Select>
										<FormMessage />
									</FormItem>
								)}
							/>
						</div>

						<FormField
							control={form.control}
							name='school'
							render={({ field }) => (
								<FormItem>
									<FormLabel>School</FormLabel>
									<FormControl>
										<Input placeholder='e.g., Lincoln High School' {...field} />
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>

						<FormField
							control={form.control}
							name='description'
							render={({ field }) => (
								<FormItem>
									<FormLabel>Description</FormLabel>
									<FormControl>
										<Textarea
											placeholder='Brief description of the class...'
											className='resize-none'
											{...field}
										/>
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>

						<FormField
							control={form.control}
							name='maxStudents'
							render={({ field }) => (
								<FormItem>
									<FormLabel>Maximum Students</FormLabel>
									<FormControl>
										<Input
											type='number'
											placeholder='Leave empty for unlimited'
											{...field}
											value={field.value ?? ''}
										/>
									</FormControl>
									<FormDescription>
										Optional limit on the number of students who can join.
									</FormDescription>
									<FormMessage />
								</FormItem>
							)}
						/>

						<DialogFooter>
							<Button
								type='button'
								variant='outline'
								onClick={() => onOpenChange(false)}
							>
								Cancel
							</Button>
							<Button type='submit' disabled={createClass.isPending}>
								{createClass.isPending && (
									<Loader2 className='mr-2 h-4 w-4 animate-spin' />
								)}
								Create Class
							</Button>
						</DialogFooter>
					</form>
				</Form>
			</DialogContent>
		</Dialog>
	)
}

export default CreateClassModal
