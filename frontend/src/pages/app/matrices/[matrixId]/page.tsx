import React from 'react'
import { useNavigate, useParams } from 'react-router'
import {
	ArrowLeft,
	Edit,
	Trash2,
	Grid3x3,
	Download,
	FileText,
	Link,
} from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import {
	useMatrixById,
	useDeleteMatrix,
	useExportMatrixToPdf,
	useExportMatrixToExcel,
	useSubjects,
	useTopics,
} from '@/hooks/useExams'
import { useConfirm } from '@/hooks/useConfirm'
import { ConfirmDialog } from '@/components/common/ConfirmDialog'
import { CognitiveLevel } from '@/types/model/exam-service'

const MatrixDetailPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const { matrixId } = useParams<{ matrixId: string }>()

	const { data: matrix, isLoading } = useMatrixById(matrixId ?? '')
	const { data: subjects = [] } = useSubjects(matrix?.grade)
	const { data: topics = [] } = useTopics(matrix?.subjectId ?? '')
	const deleteMatrixMutation = useDeleteMatrix()
	const exportToPdf = useExportMatrixToPdf()
	const exportToExcel = useExportMatrixToExcel()
	const {
		confirm,
		confirmState,
		handleConfirm,
		handleCancel,
		handleOpenChange,
	} = useConfirm()

	const subjectName = subjects.find(s => s.id === matrix?.subjectId)?.name

	const handleDelete = async () => {
		if (!matrixId) return

		const confirmed = await confirm({
			title: 'Delete Matrix',
			description:
				'Are you sure you want to delete this matrix? This action cannot be undone. Any exams using this matrix will have their matrix detached.',
			confirmText: 'Delete',
			variant: 'destructive',
		})

		if (confirmed) {
			try {
				await deleteMatrixMutation.mutateAsync(matrixId)
				navigate('/app/matrices')
			} catch (error) {
				console.error('Failed to delete matrix:', error)
			}
		}
	}

	const handleExportPdf = async () => {
		if (!matrixId) return
		await exportToPdf.mutateAsync(matrixId)
	}

	const handleExportExcel = async () => {
		if (!matrixId) return
		await exportToExcel.mutateAsync(matrixId)
	}

	const getCognitiveLevelLabel = (level: CognitiveLevel) => {
		switch (level) {
			case CognitiveLevel.Remember:
				return 'Remember'
			case CognitiveLevel.Understand:
				return 'Understand'
			case CognitiveLevel.Apply:
				return 'Apply'
			case CognitiveLevel.Analyze:
				return 'Analyze'
			default:
				return 'Unknown'
		}
	}

	const getCognitiveLevelColor = (level: CognitiveLevel) => {
		switch (level) {
			case CognitiveLevel.Remember:
				return 'bg-blue-500/10 text-blue-700 dark:text-blue-400'
			case CognitiveLevel.Understand:
				return 'bg-cyan-500/10 text-cyan-700 dark:text-cyan-400'
			case CognitiveLevel.Apply:
				return 'bg-green-500/10 text-green-700 dark:text-green-400'
			case CognitiveLevel.Analyze:
				return 'bg-yellow-500/10 text-yellow-700 dark:text-yellow-400'
			default:
				return ''
		}
	}

	if (isLoading) {
		return (
			<div className='p-6 space-y-6 max-w-7xl mx-auto'>
				<div className='text-center py-12'>Loading...</div>
			</div>
		)
	}

	if (!matrix) {
		return (
			<div className='p-6 space-y-6 max-w-7xl mx-auto'>
				<div className='text-center py-12'>
					<p className='text-muted-foreground mb-4'>Matrix not found</p>
					<Button onClick={() => navigate('/app/matrices')}>
						<ArrowLeft className='h-4 w-4 mr-2' />
						Back to Matrices
					</Button>
				</div>
			</div>
		)
	}

	// Group matrix topics by topic
	const topicGroups = matrix.matrixTopics.reduce(
		(acc, mt) => {
			if (!acc[mt.topicId]) {
				acc[mt.topicId] = []
			}
			acc[mt.topicId].push(mt)
			return acc
		},
		{} as Record<string, typeof matrix.matrixTopics>
	)

	return (
		<>
			<div className='p-6 space-y-6 max-w-7xl mx-auto'>
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
							<div className='flex items-center gap-3'>
								<Grid3x3 className='h-8 w-8' />
								<h1 className='text-3xl font-bold'>{matrix.name}</h1>
							</div>
							<p className='text-muted-foreground mt-1'>
								{subjectName || 'Unknown Subject'} • Grade {matrix.grade} •{' '}
								{matrix.totalQuestionCount} questions
							</p>
						</div>
					</div>
					<div className='flex gap-2'>
						<DropdownMenu>
							<DropdownMenuTrigger asChild>
								<Button variant='outline'>
									<Download className='h-4 w-4 mr-2' />
									Export
								</Button>
							</DropdownMenuTrigger>
							<DropdownMenuContent>
								<DropdownMenuItem
									onClick={handleExportPdf}
									disabled={exportToPdf.isPending}
								>
									<FileText className='h-4 w-4 mr-2' />
									{exportToPdf.isPending ? 'Exporting...' : 'Export as PDF'}
								</DropdownMenuItem>
								<DropdownMenuItem
									onClick={handleExportExcel}
									disabled={exportToExcel.isPending}
								>
									<FileText className='h-4 w-4 mr-2' />
									{exportToExcel.isPending ? 'Exporting...' : 'Export as Excel'}
								</DropdownMenuItem>
							</DropdownMenuContent>
						</DropdownMenu>
						<Button
							variant='outline'
							onClick={() => navigate(`/app/matrices/${matrixId}/edit`)}
						>
							<Edit className='h-4 w-4 mr-2' />
							Edit
						</Button>
						<Button
							variant='outline'
							onClick={handleDelete}
							disabled={deleteMatrixMutation.isPending}
							className='text-destructive hover:text-destructive'
						>
							<Trash2 className='h-4 w-4 mr-2' />
							{deleteMatrixMutation.isPending ? 'Deleting...' : 'Delete'}
						</Button>
					</div>
				</div>

				{/* Matrix Info */}
				<Card>
					<CardHeader>
						<CardTitle>Matrix Information</CardTitle>
					</CardHeader>
					<CardContent className='space-y-4'>
						{matrix.description && (
							<div>
								<p className='text-sm text-muted-foreground mb-1'>
									Description
								</p>
								<p className='font-medium'>{matrix.description}</p>
							</div>
						)}

						<div className='grid grid-cols-3 gap-4'>
							<div>
								<p className='text-sm text-muted-foreground mb-1'>Subject</p>
								<p className='font-medium'>
									{subjectName || 'Unknown Subject'}
								</p>
							</div>
							<div>
								<p className='text-sm text-muted-foreground mb-1'>Grade</p>
								<p className='font-medium'>Grade {matrix.grade}</p>
							</div>
							<div>
								<p className='text-sm text-muted-foreground mb-1'>Created</p>
								<p className='font-medium'>
									{new Date(matrix.createdAt).toLocaleDateString()}
								</p>
							</div>
						</div>

						<div className='flex items-center gap-2 pt-2'>
							<Link className='h-4 w-4 text-muted-foreground' />
							<span className='text-sm text-muted-foreground'>
								This matrix can be attached to multiple exams as a reusable
								blueprint.
							</span>
						</div>
					</CardContent>
				</Card>

				{/* Topic Distribution */}
				<Card>
					<CardHeader>
						<div className='flex items-center justify-between'>
							<CardTitle>Question Distribution</CardTitle>
							<Badge variant='outline'>
								{matrix.matrixTopics.length} Configurations
							</Badge>
						</div>
					</CardHeader>
					<CardContent>
						<div className='space-y-4'>
							{Object.entries(topicGroups).map(([topicId, topicItems]) => {
								const topic = topics.find(t => t.id === topicId)
								const topicTotal = topicItems.reduce(
									(sum, item) => sum + item.quantity,
									0
								)

								return (
									<div
										key={topicId}
										className='p-4 border rounded-lg space-y-3'
									>
										<div className='flex items-center justify-between'>
											<h4 className='font-medium'>
												{topic?.title || 'Unknown Topic'}
											</h4>
											<Badge variant='secondary'>{topicTotal} questions</Badge>
										</div>
										<div className='flex flex-wrap gap-2'>
											{topicItems.map((item, idx) => (
												<Badge
													key={idx}
													className={getCognitiveLevelColor(
														item.cognitiveLevel
													)}
												>
													{getCognitiveLevelLabel(item.cognitiveLevel)}:{' '}
													{item.quantity}
												</Badge>
											))}
										</div>
									</div>
								)
							})}
						</div>
					</CardContent>
				</Card>

				{/* Summary */}
				<Card>
					<CardHeader>
						<CardTitle>Summary</CardTitle>
					</CardHeader>
					<CardContent>
						<div className='grid grid-cols-4 gap-4 text-center'>
							<div className='p-4 bg-blue-500/10 rounded-lg'>
								<p className='text-2xl font-bold text-blue-700 dark:text-blue-400'>
									{matrix.matrixTopics
										.filter(mt => mt.cognitiveLevel === CognitiveLevel.Remember)
										.reduce((sum, mt) => sum + mt.quantity, 0)}
								</p>
								<p className='text-sm text-muted-foreground'>Remember</p>
							</div>
							<div className='p-4 bg-cyan-500/10 rounded-lg'>
								<p className='text-2xl font-bold text-cyan-700 dark:text-cyan-400'>
									{matrix.matrixTopics
										.filter(
											mt => mt.cognitiveLevel === CognitiveLevel.Understand
										)
										.reduce((sum, mt) => sum + mt.quantity, 0)}
								</p>
								<p className='text-sm text-muted-foreground'>Understand</p>
							</div>
							<div className='p-4 bg-green-500/10 rounded-lg'>
								<p className='text-2xl font-bold text-green-700 dark:text-green-400'>
									{matrix.matrixTopics
										.filter(mt => mt.cognitiveLevel === CognitiveLevel.Apply)
										.reduce((sum, mt) => sum + mt.quantity, 0)}
								</p>
								<p className='text-sm text-muted-foreground'>Apply</p>
							</div>
							<div className='p-4 bg-yellow-500/10 rounded-lg'>
								<p className='text-2xl font-bold text-yellow-700 dark:text-yellow-400'>
									{matrix.matrixTopics
										.filter(mt => mt.cognitiveLevel === CognitiveLevel.Analyze)
										.reduce((sum, mt) => sum + mt.quantity, 0)}
								</p>
								<p className='text-sm text-muted-foreground'>Analyze</p>
							</div>
						</div>
					</CardContent>
				</Card>
			</div>

			<ConfirmDialog
				open={confirmState.isOpen}
				onOpenChange={handleOpenChange}
				title={confirmState.title}
				description={confirmState.description}
				onConfirm={handleConfirm}
				onCancel={handleCancel}
				confirmText={confirmState.confirmText}
				cancelText={confirmState.cancelText}
				variant={confirmState.variant}
			/>
		</>
	)
}

export default MatrixDetailPage
