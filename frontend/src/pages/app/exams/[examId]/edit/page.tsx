import React, { useEffect } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import * as z from 'zod'
import { ArrowLeft, Save } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { ExamFormSkeleton } from '@/components/common/skeletons'
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
import { useTranslation } from 'react-i18next'
import type { TFunction } from 'i18next'

const examSchema = (t: TFunction) =>
	z.object({
		name: z
			.string()
			.min(3, t('forms.exams.validation.name_min'))
			.max(200, t('forms.exams.validation.name_max')),
		description: z
			.string()
			.min(10, t('forms.exams.validation.description_min'))
			.max(1000, t('forms.exams.validation.description_max')),
		grade: z
			.number()
			.int()
			.min(1, t('forms.exams.validation.grade_range'))
			.max(5, t('forms.exams.validation.grade_range')),
		subjectId: z.string().min(1, t('forms.exams.validation.subject_required')),
	})

type ExamFormData = z.infer<ReturnType<typeof examSchema>>

const EditExamPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const { examId } = useParams<{ examId: string }>()

	const { data: exam, isLoading: isLoadingExam } = useExam(examId ?? '')

	const form = useForm<ExamFormData>({
		resolver: zodResolver(React.useMemo(() => examSchema(t), [t])),
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
		return <ExamFormSkeleton />
	}

	if (!exam) {
		return (
			<div className='p-6 space-y-6 max-w-4xl mx-auto'>
				<div className='text-center py-12'>
					<p className='text-muted-foreground mb-4'>
						{t('pages.exams.edit.not_found')}
					</p>
					<Button onClick={() => navigate('/app/exams')}>
						<ArrowLeft className='h-4 w-4 mr-2' />
						{t('pages.exams.edit.back_to_exams')}
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
					<h1 className='text-3xl font-bold'>{t('pages.exams.edit.title')}</h1>
					<p className='text-muted-foreground'>
						{t('pages.exams.edit.subtitle')}
					</p>
				</div>
			</div>

			{/* Form */}
			<Form {...form}>
				<form onSubmit={form.handleSubmit(onSubmit)}>
					<Card>
						<CardHeader>
							<CardTitle>{t('pages.exams.edit.form.title')}</CardTitle>
						</CardHeader>
						<CardContent className='space-y-6'>
							{/* Exam Name */}
							<FormField
								control={form.control}
								name='name'
								render={({ field }) => (
									<FormItem>
										<FormLabel>
											{t('pages.exams.edit.form.fields.name')}
										</FormLabel>
										<FormControl>
											<Input
												placeholder={t(
													'pages.exams.edit.form.placeholders.name'
												)}
												{...field}
												disabled={updateExamMutation.isPending}
											/>
										</FormControl>
										<FormDescription>
											{t('pages.exams.edit.form.help.name')}
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
										<FormLabel>
											{t('pages.exams.edit.form.fields.description')}
										</FormLabel>
										<FormControl>
											<Textarea
												placeholder={t(
													'pages.exams.edit.form.placeholders.description'
												)}
												className='min-h-[100px]'
												{...field}
												disabled={updateExamMutation.isPending}
											/>
										</FormControl>
										<FormDescription>
											{t('pages.exams.edit.form.help.description')}
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
										<FormLabel>
											{t('pages.exams.edit.form.fields.grade')}
										</FormLabel>
										<Select
											onValueChange={value => field.onChange(parseInt(value))}
											value={field.value.toString()}
											disabled={updateExamMutation.isPending}
										>
											<FormControl>
												<SelectTrigger>
													<SelectValue
														placeholder={t(
															'pages.exams.edit.form.placeholders.grade'
														)}
													/>
												</SelectTrigger>
											</FormControl>
											<SelectContent>
												{[1, 2, 3, 4, 5].map(grade => (
													<SelectItem key={grade} value={grade.toString()}>
														{t('pages.exams.edit.form.grade_option', {
															grade,
														})}
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
										<FormLabel>
											{t('pages.exams.edit.form.fields.subject')}
										</FormLabel>
										<Select
											onValueChange={field.onChange}
											value={field.value}
											disabled={
												updateExamMutation.isPending || !subjects?.length
											}
										>
											<FormControl>
												<SelectTrigger>
													<SelectValue
														placeholder={t(
															'pages.exams.edit.form.placeholders.subject'
														)}
													/>
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
									{t('common.cancel')}
								</Button>
								<Button type='submit' disabled={updateExamMutation.isPending}>
									<Save className='h-4 w-4 mr-2' />
									{updateExamMutation.isPending
										? t('pages.exams.edit.actions.saving')
										: t('pages.exams.edit.actions.save')}
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
