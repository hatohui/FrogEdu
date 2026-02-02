import React, { useState } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import * as z from 'zod'
import { ArrowLeft, ArrowRight, Check } from 'lucide-react'
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
	title: z.string().min(3, 'Title must be at least 3 characters').max(200),
	duration: z.number().min(1).max(480),
	passScore: z.number().min(0).max(100),
	maxAttempts: z.number().min(1).max(10),
	grade: z.number().min(1).max(5),
	subjectId: z.string().min(1, 'Subject is required'),
	topicId: z.string().min(1, 'Topic is required'),
	shouldShuffleQuestions: z.boolean(),
	shouldShuffleAnswerOptions: z.boolean(),
})

type ExamFormData = z.infer<typeof examSchema>

const CreateExamPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const [step, setStep] = useState(1)
	const [selectedSubjectId, setSelectedSubjectId] = useState<string>('')

	const form = useForm<ExamFormData>({
		resolver: zodResolver(examSchema),
		defaultValues: {
			title: '',
			duration: 60,
			passScore: 50,
			maxAttempts: 1,
			grade: 1,
			subjectId: '',
			topicId: '',
			shouldShuffleQuestions: false,
			shouldShuffleAnswerOptions: false,
		},
	})

	const watchGrade = form.watch('grade')
	const { data: subjects } = useSubjects(watchGrade)
	const { data: topics } = useTopics(selectedSubjectId)
	const createExamMutation = useCreateExam()

	const onSubmit = async (data: ExamFormData) => {
		try {
			// Use default dates for now - scheduling will be done separately
			const now = new Date()
			const futureDate = new Date(now.getTime() + 365 * 24 * 60 * 60 * 1000) // 1 year from now

			const result = await createExamMutation.mutateAsync({
				title: data.title,
				duration: data.duration,
				passScore: data.passScore,
				maxAttempts: data.maxAttempts,
				startTime: now.toISOString(),
				endTime: futureDate.toISOString(),
				topicId: data.topicId,
				shouldShuffleQuestions: data.shouldShuffleQuestions,
				shouldShuffleAnswerOptions: data.shouldShuffleAnswerOptions,
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

	const nextStep = async () => {
		let fieldsToValidate: (keyof ExamFormData)[] = []

		if (step === 1) {
			fieldsToValidate = ['title', 'duration', 'passScore', 'maxAttempts']
		} else if (step === 2) {
			fieldsToValidate = ['grade', 'subjectId', 'topicId']
		}

		const isValid = await form.trigger(fieldsToValidate)
		if (isValid && step < 2) {
			setStep(step + 1)
		}
	}

	const prevStep = () => {
		if (step > 1) {
			setStep(step - 1)
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
					<p className='text-muted-foreground'>Step {step} of 2</p>
				</div>
			</div>

			{/* Progress Steps */}
			<div className='flex items-center justify-between'>
				{[1, 2].map(num => (
					<div key={num} className='flex items-center flex-1'>
						<div
							className={`flex items-center justify-center w-10 h-10 rounded-full border-2 ${
								step >= num
									? 'border-primary bg-primary text-primary-foreground'
									: 'border-muted-foreground text-muted-foreground'
							}`}
						>
							{step > num ? <Check className='h-5 w-5' /> : num}
						</div>
						{num < 3 && (
							<div
								className={`flex-1 h-1 mx-2 ${
									step > num ? 'bg-primary' : 'bg-muted'
								}`}
							/>
						)}
					</div>
				))}
			</div>

			{/* Form */}
			<Form {...form}>
				<form onSubmit={form.handleSubmit(onSubmit)} className='space-y-6'>
					{/* Step 1: Basic Info */}
					{step === 1 && (
						<Card>
							<CardHeader>
								<CardTitle>Basic Information</CardTitle>
							</CardHeader>
							<CardContent className='space-y-4'>
								<FormField
									control={form.control}
									name='title'
									render={({ field }) => (
										<FormItem>
											<FormLabel>Exam Title *</FormLabel>
											<FormControl>
												<Input placeholder='Mid-term Math Exam' {...field} />
											</FormControl>
											<FormMessage />
										</FormItem>
									)}
								/>

								<div className='grid grid-cols-2 gap-4'>
									<FormField
										control={form.control}
										name='duration'
										render={({ field }) => (
											<FormItem>
												<FormLabel>Duration (minutes) *</FormLabel>
												<FormControl>
													<Input
														type='number'
														{...field}
														onChange={e =>
															field.onChange(Number(e.target.value))
														}
													/>
												</FormControl>
												<FormMessage />
											</FormItem>
										)}
									/>

									<FormField
										control={form.control}
										name='maxAttempts'
										render={({ field }) => (
											<FormItem>
												<FormLabel>Max Attempts *</FormLabel>
												<FormControl>
													<Input
														type='number'
														{...field}
														onChange={e =>
															field.onChange(Number(e.target.value))
														}
													/>
												</FormControl>
												<FormMessage />
											</FormItem>
										)}
									/>
								</div>

								<FormField
									control={form.control}
									name='passScore'
									render={({ field }) => (
										<FormItem>
											<FormLabel>Pass Score (%) *</FormLabel>
											<FormControl>
												<Input
													type='number'
													{...field}
													onChange={e => field.onChange(Number(e.target.value))}
												/>
											</FormControl>
											<FormDescription>
												Minimum score to pass (0-100)
											</FormDescription>
											<FormMessage />
										</FormItem>
									)}
								/>
							</CardContent>
						</Card>
					)}

					{/* Step 2: Subject & Topic */}
					{step === 2 && (
						<Card>
							<CardHeader>
								<CardTitle>Subject & Topic</CardTitle>
							</CardHeader>
							<CardContent className='space-y-4'>
								<FormField
									control={form.control}
									name='grade'
									render={({ field }) => (
										<FormItem>
											<FormLabel>Grade *</FormLabel>
											<Select
												onValueChange={value => field.onChange(Number(value))}
												defaultValue={String(field.value)}
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
												}}
												value={field.value}
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
					)}

					<div className='flex justify-between'>
						<Button
							type='button'
							variant='outline'
							onClick={prevStep}
							disabled={step === 1}
						>
							<ArrowLeft className='h-4 w-4 mr-2' />
							Previous
						</Button>

						{step < 2 ? (
							<Button type='button' onClick={nextStep}>
								Next
								<ArrowRight className='h-4 w-4 ml-2' />
							</Button>
						) : (
							<Button type='submit' disabled={createExamMutation.isPending}>
								{createExamMutation.isPending ? 'Creating...' : 'Create Exam'}
							</Button>
						)}
					</div>
				</form>
			</Form>
		</div>
	)
}

export default CreateExamPage
