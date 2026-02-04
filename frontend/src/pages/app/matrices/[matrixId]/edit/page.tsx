import React, { useState, useEffect } from 'react'
import { useNavigate, useParams } from 'react-router'
import { ArrowLeft, Plus, Trash2, Save } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import { TopicSelector } from '@/components/exams/topic-selector'
import {
	useMatrixById,
	useUpdateMatrix,
	useSubjects,
	useTopics,
} from '@/hooks/useExams'
import {
	type MatrixTopicDto,
	type MatrixRow,
	CognitiveLevel,
} from '@/types/model/exam-service'

const EditMatrixPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const { matrixId } = useParams<{ matrixId: string }>()

	const { data: matrix, isLoading } = useMatrixById(matrixId ?? '')
	const updateMatrixMutation = useUpdateMatrix()

	// Form state
	const [name, setName] = useState('')
	const [description, setDescription] = useState('')
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

	// Fetch subjects and topics based on matrix
	const { data: subjects = [] } = useSubjects(matrix?.grade)
	const { data: topics = [] } = useTopics(matrix?.subjectId ?? '')

	const subjectName = subjects.find(s => s.id === matrix?.subjectId)?.name

	// Initialize form from matrix data
	useEffect(() => {
		if (isInitialized || !matrix) return

		setName(matrix.name)
		setDescription(matrix.description || '')

		if (matrix.matrixTopics && matrix.matrixTopics.length > 0) {
			// Group matrix topics by topicId
			const topicMap = new Map<
				string,
				{ remember: number; understand: number; apply: number; analyze: number }
			>()

			matrix.matrixTopics.forEach(topic => {
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
			})

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
	}, [matrix, isInitialized])

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
		if (!matrixId) return

		if (!name.trim()) {
			alert('Please enter a matrix name')
			return
		}

		// Convert matrix rows to DTO format
		const matrixTopics: MatrixTopicDto[] = []

		matrixRows.forEach(row => {
			if (!row.topicId) return

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

		if (matrixTopics.length === 0) {
			alert('Please add at least one topic with questions')
			return
		}

		try {
			await updateMatrixMutation.mutateAsync({
				matrixId,
				data: {
					name,
					description: description || undefined,
					matrixTopics,
				},
			})

			navigate(`/app/matrices/${matrixId}`)
		} catch (error) {
			console.error('Failed to update matrix:', error)
		}
	}

	if (isLoading) {
		return (
			<div className='p-6 flex items-center justify-center min-h-[400px]'>
				<div className='text-center'>
					<div className='text-lg font-medium'>Loading matrix...</div>
				</div>
			</div>
		)
	}

	if (!matrix) {
		return (
			<div className='p-6 flex items-center justify-center min-h-[400px]'>
				<div className='text-center'>
					<div className='text-lg font-medium text-destructive'>
						Matrix not found
					</div>
					<Button
						onClick={() => navigate('/app/matrices')}
						variant='outline'
						className='mt-4'
					>
						Back to Matrices
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
						onClick={() => navigate(`/app/matrices/${matrixId}`)}
					>
						<ArrowLeft className='h-5 w-5' />
					</Button>
					<div>
						<h1 className='text-3xl font-bold'>Edit Matrix</h1>
						<p className='text-muted-foreground'>
							{subjectName || 'Unknown Subject'} • Grade {matrix.grade}
						</p>
					</div>
				</div>
			</div>

			{/* Matrix Details */}
			<Card>
				<CardHeader>
					<CardTitle>Matrix Details</CardTitle>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='grid grid-cols-2 gap-4'>
						<div className='space-y-2'>
							<Label htmlFor='name'>Matrix Name *</Label>
							<Input
								id='name'
								placeholder='e.g., Math Grade 10 Final Exam Template'
								value={name}
								onChange={e => setName(e.target.value)}
							/>
						</div>
						<div className='space-y-2'>
							<Label>Subject & Grade</Label>
							<Input
								value={`${subjectName || 'Unknown Subject'} - Grade ${matrix.grade}`}
								disabled
								className='bg-muted'
							/>
							<p className='text-xs text-muted-foreground'>
								Subject and grade cannot be changed after creation
							</p>
						</div>
					</div>

					<div className='space-y-2'>
						<Label htmlFor='description'>Description</Label>
						<Textarea
							id='description'
							placeholder='Optional description for this matrix blueprint'
							value={description}
							onChange={e => setDescription(e.target.value)}
							rows={2}
						/>
					</div>
				</CardContent>
			</Card>

			{/* Matrix Configuration */}
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

			{/* Actions */}
			<div className='flex justify-end gap-4'>
				<Button
					variant='outline'
					onClick={() => navigate(`/app/matrices/${matrixId}`)}
				>
					Cancel
				</Button>
				<Button onClick={handleSave} disabled={updateMatrixMutation.isPending}>
					<Save className='h-4 w-4 mr-2' />
					{updateMatrixMutation.isPending ? 'Saving...' : 'Save Changes'}
				</Button>
			</div>
		</div>
	)
}

export default EditMatrixPage
