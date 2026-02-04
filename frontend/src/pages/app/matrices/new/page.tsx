import React, { useState } from 'react'
import { useNavigate } from 'react-router'
import { ArrowLeft, Plus, Trash2, Save } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { TopicSelector } from '@/components/exams/topic-selector'
import { useCreateMatrix, useSubjects, useTopics } from '@/hooks/useExams'
import {
	type MatrixTopicDto,
	type MatrixRow,
	CognitiveLevel,
} from '@/types/model/exam-service'

const CreateStandaloneMatrixPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const createMatrixMutation = useCreateMatrix()

	// Form state
	const [name, setName] = useState('')
	const [description, setDescription] = useState('')
	const [grade, setGrade] = useState<number | null>(null)
	const [subjectId, setSubjectId] = useState('')
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

	// Fetch subjects based on grade
	const { data: subjects = [] } = useSubjects(grade ?? undefined)
	const { data: topics = [] } = useTopics(subjectId)

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
		if (!name.trim()) {
			alert('Please enter a matrix name')
			return
		}
		if (!subjectId) {
			alert('Please select a subject')
			return
		}
		if (!grade) {
			alert('Please select a grade')
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
			await createMatrixMutation.mutateAsync({
				name,
				description: description || undefined,
				subjectId,
				grade,
				matrixTopics,
			})

			navigate('/app/matrices')
		} catch (error) {
			console.error('Failed to create matrix:', error)
		}
	}

	return (
		<div className='p-6 space-y-6 max-w-6xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div className='flex items-center space-x-4'>
					<Button
						variant='ghost'
						size='icon'
						onClick={() => navigate('/app/matrices')}
					>
						<ArrowLeft className='h-5 w-5' />
					</Button>
					<div>
						<h1 className='text-3xl font-bold'>Create Matrix Blueprint</h1>
						<p className='text-muted-foreground'>
							Define a reusable exam structure template
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
							<Label htmlFor='grade'>Grade *</Label>
							<Select
								value={grade?.toString() || ''}
								onValueChange={value => {
									setGrade(parseInt(value))
									setSubjectId('') // Reset subject when grade changes
								}}
							>
								<SelectTrigger>
									<SelectValue placeholder='Select grade' />
								</SelectTrigger>
								<SelectContent>
									{[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12].map(g => (
										<SelectItem key={g} value={g.toString()}>
											Grade {g}
										</SelectItem>
									))}
								</SelectContent>
							</Select>
						</div>
					</div>

					<div className='grid grid-cols-2 gap-4'>
						<div className='space-y-2'>
							<Label htmlFor='subject'>Subject *</Label>
							<Select
								value={subjectId}
								onValueChange={setSubjectId}
								disabled={!grade}
							>
								<SelectTrigger>
									<SelectValue
										placeholder={
											grade ? 'Select subject' : 'Select grade first'
										}
									/>
								</SelectTrigger>
								<SelectContent>
									{subjects.map(subject => (
										<SelectItem key={subject.id} value={subject.id}>
											{subject.name}
										</SelectItem>
									))}
								</SelectContent>
							</Select>
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
											value={row.topicId}
											onValueChange={(value: string) =>
												updateRow(row.id, 'topicId', value)
											}
											topics={topics}
											disabled={!subjectId}
											placeholder={
												subjectId ? 'Select topic' : 'Select subject first'
											}
										/>
									</div>
									<div className='col-span-1'>
										<Input
											type='number'
											min='0'
											value={row.remember}
											onChange={e =>
												updateRow(
													row.id,
													'remember',
													Math.max(0, parseInt(e.target.value) || 0)
												)
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
												updateRow(
													row.id,
													'understand',
													Math.max(0, parseInt(e.target.value) || 0)
												)
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
												updateRow(
													row.id,
													'apply',
													Math.max(0, parseInt(e.target.value) || 0)
												)
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
												updateRow(
													row.id,
													'analyze',
													Math.max(0, parseInt(e.target.value) || 0)
												)
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
											className='text-destructive hover:text-destructive'
										>
											<Trash2 className='h-4 w-4' />
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
				<Button variant='outline' onClick={() => navigate('/app/matrices')}>
					Cancel
				</Button>
				<Button onClick={handleSave} disabled={createMatrixMutation.isPending}>
					<Save className='h-4 w-4 mr-2' />
					{createMatrixMutation.isPending ? 'Creating...' : 'Create Matrix'}
				</Button>
			</div>
		</div>
	)
}

export default CreateStandaloneMatrixPage
