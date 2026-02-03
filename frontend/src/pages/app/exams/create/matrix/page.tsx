import React, { useState } from 'react'
import { useNavigate, useSearchParams } from 'react-router'
import { ArrowLeft, Plus, Trash2, Save } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { TopicSelector } from '@/components/exams/topic-selector'
import { useCreateMatrix, useExam, useTopics } from '@/hooks/useExams'
import {
	type MatrixTopicDto,
	type MatrixRow,
	CognitiveLevel,
} from '@/types/model/exam-service'

const CreateMatrixPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const [searchParams] = useSearchParams()
	const examId = searchParams.get('examId')

	const { data: exam, isLoading: isLoadingExam } = useExam(examId ?? '')
	const { data: topics = [] } = useTopics(exam?.subjectId ?? '')
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

	const createMatrixMutation = useCreateMatrix()

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
			await createMatrixMutation.mutateAsync({
				examId,
				matrixTopics,
			})

			// Navigate to question bank
			navigate(`/app/exams/${examId}/questions`)
		} catch (error) {
			console.error('Failed to create matrix:', error)
		}
	}

	const handleSkip = () => {
		if (!examId) return
		navigate(`/app/exams/${examId}/questions`)
	}

	// Show loading state
	if (isLoadingExam) {
		return (
			<div className='p-6 flex items-center justify-center min-h-[400px]'>
				<div className='text-center'>
					<div className='text-lg font-medium'>Loading exam details...</div>
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
						Exam not found
					</div>
					<Button
						onClick={() => navigate('/app/exams')}
						variant='outline'
						className='mt-4'
					>
						Back to Exams
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
						<h1 className='text-3xl font-bold'>Create Exam Matrix</h1>
						<p className='text-muted-foreground'>
							Define question distribution by cognitive level
						</p>
					</div>
				</div>
				<div className='flex gap-2'>
					<Button variant='outline' onClick={handleSkip}>
						Skip Matrix
					</Button>
					<Button
						onClick={handleSave}
						disabled={createMatrixMutation.isPending}
					>
						<Save className='h-4 w-4 mr-2' />
						{createMatrixMutation.isPending ? 'Saving...' : 'Save & Continue'}
					</Button>
				</div>
			</div>

			{/* Info Card */}
			<Card>
				<CardHeader>
					<CardTitle>About the Matrix</CardTitle>
				</CardHeader>
				<CardContent className='space-y-4'>
					<p className='text-sm text-muted-foreground mb-4'>
						The matrix defines how many questions of each cognitive level should
						be included in your exam. This follows Bloom&apos;s Taxonomy for
						balanced assessment.
					</p>
					<div className='bg-muted p-4 rounded-lg'>
						<div className='text-sm font-medium mb-2'>
							Bloom&apos;s Taxonomy Cognitive Levels:
						</div>
						<ul className='text-sm text-muted-foreground space-y-1'>
							<li>• Remember: Recall facts and basic concepts</li>
							<li>• Understand: Explain ideas or concepts</li>
							<li>• Apply: Use information in new situations</li>
							<li>• Analyze: Draw connections among ideas</li>
						</ul>
					</div>
				</CardContent>
			</Card>

			{/* Matrix Table */}
			<Card>
				<CardHeader className='flex flex-row items-center justify-between'>
					<CardTitle>Question Distribution Matrix</CardTitle>
					<Button onClick={addRow} size='sm' variant='outline'>
						<Plus className='h-4 w-4 mr-2' />
						Add Row
					</Button>
				</CardHeader>
				<CardContent>
					<div className='space-y-4'>
						{/* Header */}
						<div className='grid grid-cols-10 gap-2 font-semibold text-xs pb-2 border-b'>
							<div className='col-span-4'>Topic</div>
							<div className='col-span-1 text-center'>Remember</div>
							<div className='col-span-1 text-center'>Understand</div>
							<div className='col-span-1 text-center'>Apply</div>
							<div className='col-span-1 text-center'>Analyze</div>
							<div className='col-span-1 text-center'>Total</div>
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
											placeholder='Select topic'
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
							<div className='text-sm font-medium'>Total Questions:</div>
							<div className='text-2xl font-bold'>{calculateTotal()}</div>
						</div>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default CreateMatrixPage
