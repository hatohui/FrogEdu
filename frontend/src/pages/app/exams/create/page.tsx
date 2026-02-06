import React from 'react'
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
import { useCreateExam, useSubjects } from '@/hooks/useExams'
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

const CreateExamPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const schema = React.useMemo(() => examSchema(t), [t])

	const form = useForm<ExamFormData>({
		resolver: zodResolver(schema),
		defaultValues: {
			name: '',
			description: '',
			grade: 1,
			subjectId: '',
		},
	})

	const watchGrade = form.watch('grade')
	const { data: subjects } = useSubjects(watchGrade)
	const createExamMutation = useCreateExam()

	const onSubmit = async (data: ExamFormData) => {
		try {
			const result = await createExamMutation.mutateAsync({
				name: data.name,
				description: data.description,
				grade: data.grade,
				subjectId: data.subjectId,
			})

			if (result.data?.id) {
				navigate(`/app/exams/create/matrix?examId=${result.data.id}`)
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
					<h1 className='text-3xl font-bold'>
						{t('pages.exams.create.title')}
					</h1>
					<p className='text-muted-foreground'>
						{t('pages.exams.create.subtitle')}
					</p>
				</div>
			</div>

			{/* Form */}
			<Form {...form}>
				<form onSubmit={form.handleSubmit(onSubmit)} className='space-y-6'>
					<Card>
						<CardHeader>
							<CardTitle>{t('pages.exams.create.form.title')}</CardTitle>
						</CardHeader>
						<CardContent className='space-y-4'>
							<FormField
								control={form.control}
								name='name'
								render={({ field }) => (
									<FormItem>
										<FormLabel>
											{t('pages.exams.create.form.fields.name')}
										</FormLabel>
										<FormControl>
											<Input
												placeholder={t(
													'pages.exams.create.form.placeholders.name'
												)}
												{...field}
												maxLength={200}
											/>
										</FormControl>
										<FormDescription>
											{t('pages.exams.create.form.char_count', {
												count: field.value.length,
												max: 200,
											})}
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
										<FormLabel>
											{t('pages.exams.create.form.fields.description')}
										</FormLabel>
										<FormControl>
											<Textarea
												placeholder={t(
													'pages.exams.create.form.placeholders.description'
												)}
												{...field}
												maxLength={1000}
												rows={4}
											/>
										</FormControl>
										<FormDescription>
											{t('pages.exams.create.form.char_count', {
												count: field.value.length,
												max: 1000,
											})}
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
										<FormLabel>
											{t('pages.exams.create.form.fields.grade')}
										</FormLabel>
										<Select
											onValueChange={value => field.onChange(Number(value))}
											value={String(field.value)}
										>
											<FormControl>
												<SelectTrigger>
													<SelectValue
														placeholder={t(
															'pages.exams.create.form.placeholders.grade'
														)}
													/>
												</SelectTrigger>
											</FormControl>
											<SelectContent>
												{Array.from({ length: 5 }, (_, i) => i + 1).map(
													grade => (
														<SelectItem key={grade} value={String(grade)}>
															{t('pages.exams.create.form.grade_option', {
																grade,
															})}
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
										<FormLabel>
											{t('pages.exams.create.form.fields.subject')}
										</FormLabel>
										<Select
											onValueChange={value => field.onChange(value)}
											value={field.value}
										>
											<FormControl>
												<SelectTrigger>
													<SelectValue
														placeholder={t(
															'pages.exams.create.form.placeholders.subject'
														)}
													/>
												</SelectTrigger>
											</FormControl>
											<SelectContent>
												{!subjects || subjects.length === 0 ? (
													<SelectItem value='_none' disabled>
														{t('pages.exams.create.form.no_subjects')}
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
						</CardContent>
					</Card>

					<div className='flex justify-between'>
						<Button
							type='button'
							variant='outline'
							onClick={() => navigate('/app/exams')}
						>
							<ArrowLeft className='h-4 w-4 mr-2' />
							{t('common.cancel')}
						</Button>

						<Button type='submit' disabled={createExamMutation.isPending}>
							{createExamMutation.isPending
								? t('pages.exams.create.actions.creating')
								: t('pages.exams.create.actions.create')}
						</Button>
					</div>
				</form>
			</Form>
		</div>
	)
}

export default CreateExamPage
