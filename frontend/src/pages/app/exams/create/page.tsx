import React, { useState } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import * as z from 'zod'
import { ArrowLeft } from 'lucide-react'
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
import { useNavigate } from 'react-router'
import { useCreateExam, useSubjects, useTopics } from '@/hooks/useExams'
import { TopicSelector } from '@/components/exams/topic-selector'

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
	topicId: z.string().min(1, 'Topic is required'),
})

type ExamFormData = z.infer<typeof examSchema>

const CreateExamPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const [selectedSubjectId, setSelectedSubjectId] = useState<string>('')

	const form = useForm<ExamFormData>({
		resolver: zodResolver(examSchema),
		defaultValues: {
			name: '',
			description: '',
			grade: 1,
			subjectId: '',
			topicId: '',
		},
	})

	const watchGrade = form.watch('grade')
	const { data: subjects } = useSubjects(watchGrade)
	const { data: topics } = useTopics(selectedSubjectId)
	const createExamMutation = useCreateExam()

	const onSubmit = async (data: ExamFormData) => {
		try {
			const result = await createExamMutation.mutateAsync({
				name: data.name,
				description: data.description,
				grade: data.grade,
				subjectId: data.subjectId,
				topicId: data.topicId,
			})

			if (result.data?.id) {
				navigate(
					`/app/exams/create/matrix?examId=${result.data.id}&topicId=${data.topicId}`
				)
			}
		} catch (error) {
			console.error('Failed to create exam:', error)
		}
	}

	return (
		<div className='p-6 space-y-6 max-w-4xl mx-auto'>
			{/* Header */}
			<div className='flex items-center space-x-4'>
				<Button
					variant='ghost'
					size='icon'
					onClick={() => navigate('/app/exams')}
				>
					<ArrowLeft className='h-5 w-5' />
				</Button>
				<div>
					<h1 className='text-3xl font-bold'>Create New Exam</h1>
					<p className='text-muted-foreground'>Define basic exam information</p>
				</div>
			</div>

			{/* Form */}
			<Form {...form}>
				<form onSubmit={form.handleSubmit(onSubmit)} className='space-y-6'>
					<Card>
						<CardHeader>
							<CardTitle>Exam Information</CardTitle>
						</CardHeader>
						<CardContent className='space-y-4'>
							<FormField
								control={form.control}
								name='name'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Exam Name *</FormLabel>
										<FormControl>
											<Input
												placeholder='Mid-term Math Exam'
												{...field}
												maxLength={200}
											/>
										</FormControl>
										<FormDescription>
											{field.value.length}/200 characters
										</FormDescription>
										<FormMessage />
									</FormItem>
								)}
							/>

							<FormField
								control={form.control}
								name='description'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Description *</FormLabel>
										<FormControl>
											<Textarea
												placeholder='Describe the exam content and objectives...'
												{...field}
												maxLength={1000}
												rows={4}
											/>
										</FormControl>
										<FormDescription>
											{field.value.length}/1000 characters
										</FormDescription>
										<FormMessage />
									</FormItem>
								)}
							/>

							<FormField
								control={form.control}
								name='grade'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Grade *</FormLabel>
										<Select
											onValueChange={value => field.onChange(Number(value))}
											value={String(field.value)}
										>
											<FormControl>
												<SelectTrigger>
													<SelectValue placeholder='Select grade' />
												</SelectTrigger>
											</FormControl>
											<SelectContent>
												{Array.from({ length: 5 }, (_, i) => i + 1).map(
													grade => (
														<SelectItem key={grade} value={String(grade)}>
															{grade}
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
								name='subjectId'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Subject *</FormLabel>
										<Select
											onValueChange={value => {
												field.onChange(value)
												setSelectedSubjectId(value)
												form.setValue('topicId', '')
											}}
											value={field.value}
										>
											<FormControl>
												<SelectTrigger>
													<SelectValue placeholder='Select subject' />
												</SelectTrigger>
											</FormControl>
											<SelectContent>
												{!subjects || subjects.length === 0 ? (
													<SelectItem value='_none' disabled>
														Select a grade first
													</SelectItem>
												) : (
													subjects.map(subject => (
														<SelectItem key={subject.id} value={subject.id}>
															{subject.name}
														</SelectItem>
													))
												)}
											</SelectContent>
										</Select>
										<FormMessage />
									</FormItem>
								)}
							/>

							<FormField
								control={form.control}
								name='topicId'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Topic *</FormLabel>
										<FormControl>
											<TopicSelector
												topics={topics || []}
												value={field.value}
												onValueChange={field.onChange}
												placeholder='Search and select a topic...'
												disabled={!selectedSubjectId}
											/>
										</FormControl>
										<FormDescription>
											{!selectedSubjectId
												? 'Please select a subject first'
												: 'Search by topic name or number (e.g., 1.1, 2.3)'}
										</FormDescription>
										<FormMessage />
									</FormItem>
								)}
							/>
						</CardContent>
					</Card>

					<div className='flex justify-between'>
						<Button
							type='button'
							variant='outline'
							onClick={() => navigate('/app/exams')}
						>
							<ArrowLeft className='h-4 w-4 mr-2' />
							Cancel
						</Button>

						<Button type='submit' disabled={createExamMutation.isPending}>
							{createExamMutation.isPending ? 'Creating...' : 'Create Exam'}
						</Button>
					</div>
				</form>
			</Form>
		</div>
	)
}

export default CreateExamPage
