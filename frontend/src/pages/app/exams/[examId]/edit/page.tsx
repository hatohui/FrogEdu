import React, { useEffect } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import * as z from 'zod'
import { ArrowLeft, Save } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
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
import { Textarea } from '@/components/ui/textarea'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { useNavigate, useParams } from 'react-router'
import { useUpdateExam, useExam, useSubjects } from '@/hooks/useExams'

const examSchema = z.object({
	name: z
		.string()
		.min(3, 'Exam name must be at least 3 characters')
		.max(200, 'Exam name cannot exceed 200 characters'),
	description: z
		.string()
		.min(10, 'Description must be at least 10 characters')
		.max(1000, 'Description cannot exceed 1000 characters'),
	grade: z
		.number()
		.int()
		.min(1, 'Grade must be between 1 and 5')
		.max(5, 'Grade must be between 1 and 5'),
	subjectId: z.string().min(1, 'Subject is required'),
})

type ExamFormData = z.infer<typeof examSchema>

const EditExamPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const { examId } = useParams<{ examId: string }>()

	const { data: exam, isLoading: isLoadingExam } = useExam(examId ?? '')

	const form = useForm<ExamFormData>({
		resolver: zodResolver(examSchema),
		defaultValues: {
			name: '',
			description: '',
			grade: 1,
			subjectId: '',
		},
	})

	const watchGrade = form.watch('grade')
	const { data: subjects } = useSubjects(watchGrade)
	const updateExamMutation = useUpdateExam()

	// Populate form when exam data loads
	useEffect(() => {
		if (exam) {
			form.reset({
				name: exam.name,
				description: exam.description,
				grade: exam.grade,
				subjectId: exam.subjectId,
			})
		}
	}, [exam, form])

	const onSubmit = async (data: ExamFormData) => {
		if (!examId) return

		try {
			await updateExamMutation.mutateAsync({
				examId,
				data,
			})

			navigate(`/app/exams/${examId}`)
		} catch (error) {
			console.error('Failed to update exam:', error)
		}
	}

	if (isLoadingExam) {
		return (
			<div className='p-6 space-y-6 max-w-4xl mx-auto'>
				<div className='text-center py-12'>Loading...</div>
			</div>
		)
	}

	if (!exam) {
		return (
			<div className='p-6 space-y-6 max-w-4xl mx-auto'>
				<div className='text-center py-12'>
					<p className='text-muted-foreground mb-4'>Exam not found</p>
					<Button onClick={() => navigate('/app/exams')}>
						<ArrowLeft className='h-4 w-4 mr-2' />
						Back to Exams
					</Button>
				</div>
			</div>
		)
	}

	return (
		<div className='p-6 space-y-6 max-w-4xl mx-auto'>
			{/* Header */}
			<div className='flex items-center space-x-4'>
				<Button
					variant='ghost'
					size='icon'
					onClick={() => navigate(`/app/exams/${examId}`)}
				>
					<ArrowLeft className='h-5 w-5' />
				</Button>
				<div>
					<h1 className='text-3xl font-bold'>Edit Exam</h1>
					<p className='text-muted-foreground'>Update exam information</p>
				</div>
			</div>

			{/* Form */}
			<Form {...form}>
				<form onSubmit={form.handleSubmit(onSubmit)}>
					<Card>
						<CardHeader>
							<CardTitle>Exam Details</CardTitle>
						</CardHeader>
						<CardContent className='space-y-6'>
							{/* Exam Name */}
							<FormField
								control={form.control}
								name='name'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Exam Name</FormLabel>
										<FormControl>
											<Input
												placeholder='Enter exam name'
												{...field}
												disabled={updateExamMutation.isPending}
											/>
										</FormControl>
										<FormDescription>
											A clear and descriptive name for the exam
										</FormDescription>
										<FormMessage />
									</FormItem>
								)}
							/>

							{/* Description */}
							<FormField
								control={form.control}
								name='description'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Description</FormLabel>
										<FormControl>
											<Textarea
												placeholder='Enter exam description'
												className='min-h-[100px]'
												{...field}
												disabled={updateExamMutation.isPending}
											/>
										</FormControl>
										<FormDescription>
											Describe what this exam covers
										</FormDescription>
										<FormMessage />
									</FormItem>
								)}
							/>

							{/* Grade */}
							<FormField
								control={form.control}
								name='grade'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Grade</FormLabel>
										<Select
											onValueChange={value => field.onChange(parseInt(value))}
											value={field.value.toString()}
											disabled={updateExamMutation.isPending}
										>
											<FormControl>
												<SelectTrigger>
													<SelectValue placeholder='Select grade' />
												</SelectTrigger>
											</FormControl>
											<SelectContent>
												{[1, 2, 3, 4, 5].map(grade => (
													<SelectItem key={grade} value={grade.toString()}>
														Grade {grade}
													</SelectItem>
												))}
											</SelectContent>
										</Select>
										<FormDescription>
											Target grade for this exam
										</FormDescription>
										<FormMessage />
									</FormItem>
								)}
							/>

							{/* Subject */}
							<FormField
								control={form.control}
								name='subjectId'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Subject</FormLabel>
										<Select
											onValueChange={field.onChange}
											value={field.value}
											disabled={
												updateExamMutation.isPending || !subjects?.length
											}
										>
											<FormControl>
												<SelectTrigger>
													<SelectValue placeholder='Select subject' />
												</SelectTrigger>
											</FormControl>
											<SelectContent>
												{subjects?.map(subject => (
													<SelectItem key={subject.id} value={subject.id}>
														{subject.name}
													</SelectItem>
												))}
											</SelectContent>
										</Select>
										<FormDescription>
											Subject area for this exam
										</FormDescription>
										<FormMessage />
									</FormItem>
								)}
							/>

							{/* Actions */}
							<div className='flex justify-end gap-3'>
								<Button
									type='button'
									variant='outline'
									onClick={() => navigate(`/app/exams/${examId}`)}
									disabled={updateExamMutation.isPending}
								>
									Cancel
								</Button>
								<Button type='submit' disabled={updateExamMutation.isPending}>
									<Save className='h-4 w-4 mr-2' />
									{updateExamMutation.isPending ? 'Saving...' : 'Save Changes'}
								</Button>
							</div>
						</CardContent>
					</Card>
				</form>
			</Form>
		</div>
	)
}

export default EditExamPage
