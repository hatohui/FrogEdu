import React, { useState } from 'react'
import { useNavigate, useSearchParams } from 'react-router'
import { ArrowLeft, Plus, Trash2, Save } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { useCreateMatrix } from '@/hooks/useExams'
import { CognitiveLevel } from '@/types/model/exams'
import type { MatrixTopicDto } from '@/types/model/exams'

interface MatrixRow {
	id: string
	topicId: string
	easy: number
	medium: number
	hard: number
}

const CreateMatrixPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const [searchParams] = useSearchParams()
	const examId = searchParams.get('examId')
	const topicId = searchParams.get('topicId')

	const [matrixRows, setMatrixRows] = useState<MatrixRow[]>([
		{
			id: crypto.randomUUID(),
			topicId: topicId || '',
			easy: 0,
			medium: 0,
			hard: 0,
		},
	])

	const createMatrixMutation = useCreateMatrix()

	const addRow = () => {
		setMatrixRows([
			...matrixRows,
			{
				id: crypto.randomUUID(),
				topicId: '',
				easy: 0,
				medium: 0,
				hard: 0,
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
			(sum, row) => sum + row.easy + row.medium + row.hard,
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
			if (row.easy > 0) {
				matrixTopics.push({
					topicId: row.topicId,
					cognitiveLevel: CognitiveLevel.Easy,
					quantity: row.easy,
				})
			}
			if (row.medium > 0) {
				matrixTopics.push({
					topicId: row.topicId,
					cognitiveLevel: CognitiveLevel.Medium,
					quantity: row.medium,
				})
			}
			if (row.hard > 0) {
				matrixTopics.push({
					topicId: row.topicId,
					cognitiveLevel: CognitiveLevel.Hard,
					quantity: row.hard,
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

			{/* Matrix Info Card */}
			<Card>
				<CardHeader>
					<CardTitle>Exam Matrix Blueprint</CardTitle>
				</CardHeader>
				<CardContent>
					<p className='text-sm text-muted-foreground mb-4'>
						The matrix defines how many questions of each difficulty level
						should be included in your exam. This follows the Vietnamese
						curriculum standards for balanced assessment.
					</p>
					<div className='bg-muted p-4 rounded-lg'>
						<div className='text-sm font-medium mb-2'>
							Standard Distribution Guide:
						</div>
						<ul className='text-sm text-muted-foreground space-y-1'>
							<li>• Easy (Knowledge/Comprehension): 40-50%</li>
							<li>• Medium (Application/Analysis): 30-40%</li>
							<li>• Hard (Synthesis/Evaluation): 10-20%</li>
						</ul>
					</div>
				</CardContent>
			</Card>

			{/* Matrix Table */}
			<Card>
				<CardHeader className='flex flex-row items-center justify-between'>
					<CardTitle>Question Distribution</CardTitle>
					<Button onClick={addRow} variant='outline' size='sm'>
						<Plus className='h-4 w-4 mr-2' />
						Add Row
					</Button>
				</CardHeader>
				<CardContent>
					<div className='space-y-4'>
						{/* Header */}
						<div className='grid grid-cols-12 gap-4 font-semibold text-sm pb-2 border-b'>
							<div className='col-span-6'>Topic</div>
							<div className='col-span-2 text-center'>Easy</div>
							<div className='col-span-2 text-center'>Medium</div>
							<div className='col-span-1 text-center'>Hard</div>
							<div className='col-span-1'></div>
						</div>

						{/* Rows */}
						{matrixRows.map(row => (
							<div
								key={row.id}
								className='grid grid-cols-12 gap-4 items-center'
							>
								<div className='col-span-6'>
									<Select
										value={row.topicId}
										onValueChange={value => updateRow(row.id, 'topicId', value)}
									>
										<SelectTrigger>
											<SelectValue placeholder='Select topic' />
										</SelectTrigger>
										<SelectContent>
											<SelectItem value='topic-1'>
												Algebra - Linear Equations
											</SelectItem>
											<SelectItem value='topic-2'>
												Geometry - Triangles
											</SelectItem>
											<SelectItem value='topic-3'>
												Calculus - Derivatives
											</SelectItem>
										</SelectContent>
									</Select>
								</div>
								<div className='col-span-2'>
									<Input
										type='number'
										min='0'
										value={row.easy}
										onChange={e =>
											updateRow(row.id, 'easy', Number(e.target.value))
										}
										className='text-center'
									/>
								</div>
								<div className='col-span-2'>
									<Input
										type='number'
										min='0'
										value={row.medium}
										onChange={e =>
											updateRow(row.id, 'medium', Number(e.target.value))
										}
										className='text-center'
									/>
								</div>
								<div className='col-span-1'>
									<Input
										type='number'
										min='0'
										value={row.hard}
										onChange={e =>
											updateRow(row.id, 'hard', Number(e.target.value))
										}
										className='text-center'
									/>
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
						))}

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
