import React, { useState, useEffect } from 'react'
import { useNavigate, useSearchParams } from 'react-router'
import { ArrowLeft, Plus, Trash2, Save } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { TopicSelector } from '@/components/exams/topic-selector'
import {
	useCreateMatrix,
	useUpdateMatrix,
	useExam,
	useTopics,
	useMatrixByExamId,
	useAttachMatrixToExam,
} from '@/hooks/useExams'
import {
	type MatrixTopicDto,
	type MatrixRow,
	CognitiveLevel,
} from '@/types/model/exam-service'
import { useTranslation } from 'react-i18next'

const CreateMatrixPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const [searchParams] = useSearchParams()
	const examId = searchParams.get('examId')

	const { data: exam, isLoading: isLoadingExam } = useExam(examId ?? '')
	const { data: topics = [] } = useTopics(exam?.subjectId ?? '')
	const { data: existingMatrix, isLoading: isLoadingMatrix } =
		useMatrixByExamId(examId ?? '')
	const attachMatrixMutation = useAttachMatrixToExam()
	const [matrixRows, setMatrixRows] = useState<MatrixRow[]>([
		{
			id: crypto.randomUUID(),
			topicId: '',
			remember: 0,
			understand: 0,
			apply: 0,
			analyze: 0,
		},
	])
	const [isInitialized, setIsInitialized] = useState(false)

	// Initialize matrix rows from existing matrix data
	useEffect(() => {
		if (isInitialized || isLoadingMatrix) return

		if (
			existingMatrix?.matrixTopics &&
			existingMatrix.matrixTopics.length > 0
		) {
			// Group matrix topics by topicId
			const topicMap = new Map<
				string,
				{ remember: number; understand: number; apply: number; analyze: number }
			>()

			existingMatrix.matrixTopics.forEach(
				(topic: {
					topicId: string
					cognitiveLevel: CognitiveLevel
					quantity: number
				}) => {
					const existing = topicMap.get(topic.topicId) || {
						remember: 0,
						understand: 0,
						apply: 0,
						analyze: 0,
					}

					switch (topic.cognitiveLevel) {
						case CognitiveLevel.Remember:
							existing.remember = topic.quantity
							break
						case CognitiveLevel.Understand:
							existing.understand = topic.quantity
							break
						case CognitiveLevel.Apply:
							existing.apply = topic.quantity
							break
						case CognitiveLevel.Analyze:
							existing.analyze = topic.quantity
							break
					}

					topicMap.set(topic.topicId, existing)
				}
			)

			// Convert to MatrixRow format
			const rows: MatrixRow[] = Array.from(topicMap.entries()).map(
				([topicId, levels]) => ({
					id: crypto.randomUUID(),
					topicId,
					...levels,
				})
			)

			if (rows.length > 0) {
				setMatrixRows(rows)
			}
		}
		setIsInitialized(true)
	}, [existingMatrix, isLoadingMatrix, isInitialized])

	const createMatrixMutation = useCreateMatrix()
	const updateMatrixMutation = useUpdateMatrix()

	// Check if we're editing an existing matrix
	const isEditing = !!existingMatrix?.id

	const addRow = () => {
		setMatrixRows([
			...matrixRows,
			{
				id: crypto.randomUUID(),
				topicId: '',
				remember: 0,
				understand: 0,
				apply: 0,
				analyze: 0,
			},
		])
	}

	const removeRow = (id: string) => {
		if (matrixRows.length > 1) {
			setMatrixRows(matrixRows.filter(row => row.id !== id))
		}
	}

	const updateRow = (
		id: string,
		field: keyof MatrixRow,
		value: string | number
	) => {
		setMatrixRows(
			matrixRows.map(row => (row.id === id ? { ...row, [field]: value } : row))
		)
	}

	const calculateTotal = () => {
		return matrixRows.reduce(
			(sum, row) =>
				sum + row.remember + row.understand + row.apply + row.analyze,
			0
		)
	}

	const handleSave = async () => {
		if (!examId) {
			console.error('No exam ID provided')
			return
		}

		// Convert matrix rows to DTO format
		const matrixTopics: MatrixTopicDto[] = []

		matrixRows.forEach(row => {
			if (row.remember > 0) {
				matrixTopics.push({
					topicId: row.topicId,
					cognitiveLevel: CognitiveLevel.Remember,
					quantity: row.remember,
				})
			}
			if (row.understand > 0) {
				matrixTopics.push({
					topicId: row.topicId,
					cognitiveLevel: CognitiveLevel.Understand,
					quantity: row.understand,
				})
			}
			if (row.apply > 0) {
				matrixTopics.push({
					topicId: row.topicId,
					cognitiveLevel: CognitiveLevel.Apply,
					quantity: row.apply,
				})
			}
			if (row.analyze > 0) {
				matrixTopics.push({
					topicId: row.topicId,
					cognitiveLevel: CognitiveLevel.Analyze,
					quantity: row.analyze,
				})
			}
		})

		try {
			if (isEditing && existingMatrix?.id) {
				// Update existing matrix
				await updateMatrixMutation.mutateAsync({
					matrixId: existingMatrix.id,
					data: {
						name: existingMatrix.name,
						description: existingMatrix.description ?? undefined,
						matrixTopics,
					},
				})
			} else {
				// Create new matrix and attach to exam
				const response = await createMatrixMutation.mutateAsync({
					name: t('pages.exams.matrix.auto_name', {
						exam: exam?.name || t('pages.exams.matrix.default_exam'),
					}),
					description: t('pages.exams.matrix.auto_description'),
					subjectId: exam?.subjectId || '',
					grade: exam?.grade || 1,
					matrixTopics,
				})

				// Attach the new matrix to the exam
				if (response.data?.id) {
					await attachMatrixMutation.mutateAsync({
						examId,
						matrixId: response.data.id,
					})
				}
			}

			// Navigate to question bank
			navigate(`/app/exams/${examId}`)
		} catch (error) {
			console.error(
				`Failed to ${isEditing ? 'update' : 'create'} matrix:`,
				error
			)
		}
	}

	const handleSkip = () => {
		if (!examId) return
		navigate(`/app/exams/${examId}`)
	}

	// Show loading state
	if (isLoadingExam || isLoadingMatrix) {
		return (
			<div className='p-6 flex items-center justify-center min-h-[400px]'>
				<div className='text-center'>
					<div className='text-lg font-medium'>
						{t('pages.exams.matrix.loading_exam')}
					</div>
				</div>
			</div>
		)
	}

	// Show error if exam not found
	if (!exam && !isLoadingExam) {
		return (
			<div className='p-6 flex items-center justify-center min-h-[400px]'>
				<div className='text-center'>
					<div className='text-lg font-medium text-destructive'>
						{t('pages.exams.matrix.not_found')}
					</div>
					<Button
						onClick={() => navigate('/app/exams')}
						variant='outline'
						className='mt-4'
					>
						{t('pages.exams.matrix.back_to_exams')}
					</Button>
				</div>
			</div>
		)
	}

	return (
		<div className='p-6 space-y-6 max-w-6xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
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
							{isEditing
								? t('pages.exams.matrix.title_edit')
								: t('pages.exams.matrix.title_create')}
						</h1>
						<p className='text-muted-foreground'>
							{t('pages.exams.matrix.subtitle')}
						</p>
					</div>
				</div>
				<div className='flex gap-2'>
					<Button variant='outline' onClick={handleSkip}>
						{t('pages.exams.matrix.actions.skip')}
					</Button>
					<Button
						onClick={handleSave}
						disabled={
							createMatrixMutation.isPending ||
							updateMatrixMutation.isPending ||
							matrixRows.some(row => !row.topicId)
						}
					>
						<Save className='h-4 w-4 mr-2' />
						{createMatrixMutation.isPending || updateMatrixMutation.isPending
							? t('pages.exams.matrix.actions.saving')
							: isEditing
								? t('pages.exams.matrix.actions.update_continue')
								: t('pages.exams.matrix.actions.save_continue')}
					</Button>
				</div>
			</div>

			{/* Info Card */}
			<Card>
				<CardHeader>
					<CardTitle>{t('pages.exams.matrix.about.title')}</CardTitle>
				</CardHeader>
				<CardContent className='space-y-4'>
					<p className='text-sm text-muted-foreground mb-4'>
						{t('pages.exams.matrix.about.description')}
					</p>
					<div className='bg-muted p-4 rounded-lg'>
						<div className='text-sm font-medium mb-2'>
							{t('pages.exams.matrix.about.levels_title')}
						</div>
						<ul className='text-sm text-muted-foreground space-y-1'>
							<li>{t('pages.exams.matrix.about.levels.remember')}</li>
							<li>{t('pages.exams.matrix.about.levels.understand')}</li>
							<li>{t('pages.exams.matrix.about.levels.apply')}</li>
							<li>{t('pages.exams.matrix.about.levels.analyze')}</li>
						</ul>
					</div>
				</CardContent>
			</Card>

			{/* Matrix Table */}
			<Card>
				<CardHeader className='flex flex-row items-center justify-between'>
					<CardTitle>{t('pages.exams.matrix.table.title')}</CardTitle>
					<Button onClick={addRow} size='sm' variant='outline'>
						<Plus className='h-4 w-4 mr-2' />
						{t('pages.exams.matrix.actions.add_row')}
					</Button>
				</CardHeader>
				<CardContent>
					<div className='space-y-4'>
						{/* Header */}
						<div className='grid grid-cols-10 gap-2 font-semibold text-xs pb-2 border-b'>
							<div className='col-span-4'>
								{t('pages.exams.matrix.table.topic')}
							</div>
							<div className='col-span-1 text-center'>
								{t('exams.cognitive_levels.remember')}
							</div>
							<div className='col-span-1 text-center'>
								{t('exams.cognitive_levels.understand')}
							</div>
							<div className='col-span-1 text-center'>
								{t('exams.cognitive_levels.apply')}
							</div>
							<div className='col-span-1 text-center'>
								{t('exams.cognitive_levels.analyze')}
							</div>
							<div className='col-span-1 text-center'>
								{t('pages.exams.matrix.table.total')}
							</div>
							<div className='col-span-1'></div>
						</div>

						{/* Rows */}
						{matrixRows.map(row => {
							const rowTotal =
								row.remember + row.understand + row.apply + row.analyze
							return (
								<div
									key={row.id}
									className='grid grid-cols-10 gap-2 items-center'
								>
									<div className='col-span-4'>
										<TopicSelector
											topics={topics}
											value={row.topicId}
											onValueChange={value =>
												updateRow(row.id, 'topicId', value)
											}
											placeholder={t('pages.exams.matrix.table.select_topic')}
										/>
									</div>
									<div className='col-span-1'>
										<Input
											type='number'
											min='0'
											value={row.remember}
											onChange={e =>
												updateRow(row.id, 'remember', Number(e.target.value))
											}
											className='text-center'
										/>
									</div>
									<div className='col-span-1'>
										<Input
											type='number'
											min='0'
											value={row.understand}
											onChange={e =>
												updateRow(row.id, 'understand', Number(e.target.value))
											}
											className='text-center'
										/>
									</div>
									<div className='col-span-1'>
										<Input
											type='number'
											min='0'
											value={row.apply}
											onChange={e =>
												updateRow(row.id, 'apply', Number(e.target.value))
											}
											className='text-center'
										/>
									</div>
									<div className='col-span-1'>
										<Input
											type='number'
											min='0'
											value={row.analyze}
											onChange={e =>
												updateRow(row.id, 'analyze', Number(e.target.value))
											}
											className='text-center'
										/>
									</div>
									<div className='col-span-1 text-center font-semibold'>
										{rowTotal}
									</div>
									<div className='col-span-1 flex justify-end'>
										<Button
											variant='ghost'
											size='icon'
											onClick={() => removeRow(row.id)}
											disabled={matrixRows.length === 1}
										>
											<Trash2 className='h-4 w-4 text-destructive' />
										</Button>
									</div>
								</div>
							)
						})}

						{/* Total */}
						<div className='pt-4 border-t flex justify-between items-center'>
							<div className='text-sm font-medium'>
								{t('pages.exams.matrix.table.total_questions')}
							</div>
							<div className='text-2xl font-bold'>{calculateTotal()}</div>
						</div>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default CreateMatrixPage
