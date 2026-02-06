import React from 'react'
import { useNavigate, useParams } from 'react-router'
import { ArrowLeft, FileSpreadsheet, FileText, Loader2 } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'
import {
	useExamPreview,
	useExportExamToPdf,
	useExportExamToExcel,
} from '@/hooks/useExams'
import { useTranslation } from 'react-i18next'

const ExamPreviewPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const { examId } = useParams<{ examId: string }>()

	const { data: preview, isLoading } = useExamPreview(examId || '')
	const exportToPdf = useExportExamToPdf()
	const exportToExcel = useExportExamToExcel()

	if (isLoading) {
		return (
			<div className='flex h-screen items-center justify-center'>
				<Loader2 className='h-8 w-8 animate-spin text-primary' />
			</div>
		)
	}

	if (!preview) {
		return (
			<div className='flex h-screen flex-col items-center justify-center'>
				<p className='text-muted-foreground'>
					{t('pages.exams.preview.not_found')}
				</p>
				<Button onClick={() => navigate('/app/exams')} className='mt-4'>
					{t('pages.exams.preview.back_to_exams')}
				</Button>
			</div>
		)
	}

	const handleExportPdf = () => {
		if (examId) {
			exportToPdf.mutate(examId)
		}
	}

	const handleExportExcel = () => {
		if (examId) {
			exportToExcel.mutate(examId)
		}
	}

	return (
		<div className='container mx-auto max-w-5xl space-y-6 py-8'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<Button
					variant='ghost'
					onClick={() => navigate(`/app/exams/${examId}`)}
					className='gap-2'
				>
					<ArrowLeft className='h-4 w-4' />
					{t('pages.exams.preview.back_to_exam')}
				</Button>

				<div className='flex gap-2'>
					<Button
						variant='outline'
						onClick={handleExportPdf}
						disabled={exportToPdf.isPending}
						className='gap-2'
					>
						{exportToPdf.isPending ? (
							<Loader2 className='h-4 w-4 animate-spin' />
						) : (
							<FileText className='h-4 w-4' />
						)}
						{exportToPdf.isPending
							? t('pages.exams.preview.exporting_pdf')
							: t('pages.exams.preview.export_pdf')}
					</Button>
					<Button
						variant='outline'
						onClick={handleExportExcel}
						disabled={exportToExcel.isPending}
						className='gap-2'
					>
						{exportToExcel.isPending ? (
							<Loader2 className='h-4 w-4 animate-spin' />
						) : (
							<FileSpreadsheet className='h-4 w-4' />
						)}
						{exportToExcel.isPending
							? t('pages.exams.preview.exporting_excel')
							: t('pages.exams.preview.export_excel')}
					</Button>
				</div>
			</div>

			{/* Exam Info Card */}
			<Card>
				<CardHeader>
					<CardTitle className='text-3xl'>{preview.name}</CardTitle>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='grid grid-cols-2 gap-4 md:grid-cols-4'>
						<div>
							<p className='text-sm text-muted-foreground'>
								{t('pages.exams.preview.subject')}
							</p>
							<p className='font-medium'>{preview.subjectName}</p>
						</div>
						<div>
							<p className='text-sm text-muted-foreground'>
								{t('pages.exams.preview.grade')}
							</p>
							<p className='font-medium'>{preview.grade}</p>
						</div>
						<div>
							<p className='text-sm text-muted-foreground'>
								{t('pages.exams.preview.questions')}
							</p>
							<p className='font-medium'>{preview.questionCount}</p>
						</div>
						<div>
							<p className='text-sm text-muted-foreground'>
								{t('pages.exams.preview.total_points')}
							</p>
							<p className='font-medium'>{preview.totalPoints}</p>
						</div>
					</div>

					{preview.description && (
						<>
							<Separator />
							<div>
								<p className='text-sm font-medium text-muted-foreground'>
									{t('pages.exams.preview.description')}
								</p>
								<p className='mt-2'>{preview.description}</p>
							</div>
						</>
					)}
				</CardContent>
			</Card>

			{/* Questions */}
			<div className='space-y-6'>
				<h2 className='text-2xl font-bold'>
					{t('pages.exams.preview.questions_title')}
				</h2>

				{preview.questions.map(question => (
					<Card key={question.questionNumber} className='overflow-hidden'>
						<CardHeader className='bg-muted/50'>
							<div className='flex items-start justify-between'>
								<div className='flex-1'>
									<div className='flex items-center gap-2'>
										<span className='font-bold'>
											{t('pages.exams.preview.question_number', {
												number: question.questionNumber,
											})}
										</span>
										<span>{question.content}</span>
									</div>
									<div className='mt-2 flex items-center gap-2'>
										<Badge variant='secondary'>{question.type}</Badge>
										<Badge variant='outline'>{question.cognitiveLevel}</Badge>
									</div>
								</div>
								<Badge variant='default' className='ml-4'>
									{t('pages.exams.preview.points', { count: question.point })}
								</Badge>
							</div>
						</CardHeader>
						<CardContent className='pt-6'>
							<div className='space-y-2'>
								{question.answers.map(answer => (
									<div
										key={answer.label}
										className={`flex items-start gap-2 rounded-md border p-3 ${
											answer.isCorrect
												? 'border-green-500 bg-green-50 dark:bg-green-950/20'
												: 'border-border'
										}`}
									>
										<span className='font-medium'>{answer.label}.</span>
										<span className='flex-1'>{answer.content}</span>
										{answer.isCorrect && (
											<Badge variant='default' className='bg-green-600'>
												{t('pages.exams.preview.correct')}
											</Badge>
										)}
									</div>
								))}
							</div>
						</CardContent>
					</Card>
				))}
			</div>
		</div>
	)
}

export default ExamPreviewPage
