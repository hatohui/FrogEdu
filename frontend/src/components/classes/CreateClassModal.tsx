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
		.max(200, 'Name cannot exceed 200 characters'),
	grade: z.string().min(1, 'Grade is required'),
	description: z
		.string()
		.min(1, 'Description is required')
		.max(1000, 'Description cannot exceed 1000 characters'),
	maxStudents: z
		.string()
		.min(1, 'Maximum students is required')
		.refine(
			val => !isNaN(Number(val)) && Number(val) > 0,
			'Must be a positive number'
		)
		.refine(val => Number(val) <= 500, 'Cannot exceed 500 students'),
	bannerUrl: z.string().url('Must be a valid URL').optional().or(z.literal('')),
})

type CreateClassFormData = z.infer<typeof createClassSchema>

interface CreateClassModalProps {
	open: boolean
	onOpenChange: (open: boolean) => void
}

const CreateClassModal: React.FC<CreateClassModalProps> = ({
	open,
	onOpenChange,
}) => {
	const createClass = useCreateClass()

	const form = useForm<CreateClassFormData>({
		resolver: zodResolver(createClassSchema),
		defaultValues: {
			name: '',
			grade: '10',
			description: '',
			maxStudents: '30',
			bannerUrl: '',
		},
	})

	const onSubmit = async (data: CreateClassFormData) => {
		await createClass.mutateAsync({
			name: data.name,
			grade: data.grade,
			description: data.description,
			maxStudents: parseInt(data.maxStudents, 10),
			bannerUrl: data.bannerUrl || undefined,
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
								name='maxStudents'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Maximum Students *</FormLabel>
										<FormControl>
											<Input type='number' placeholder='30' {...field} />
										</FormControl>
										<FormDescription>
											Maximum number of students (1-500)
										</FormDescription>
										<FormMessage />
									</FormItem>
								)}
							/>
						</div>

						<FormField
							control={form.control}
							name='description'
							render={({ field }) => (
								<FormItem>
									<FormLabel>Description *</FormLabel>
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
							name='bannerUrl'
							render={({ field }) => (
								<FormItem>
									<FormLabel>Banner URL (Optional)</FormLabel>
									<FormControl>
										<Input
											type='url'
											placeholder='https://example.com/banner.jpg'
											{...field}
										/>
									</FormControl>
									<FormDescription>
										Optional banner image for the class
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
