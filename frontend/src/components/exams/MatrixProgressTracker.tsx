import React, { useMemo } from 'react'
import { CardContent, CardTitle } from '@/components/ui/card'
import { Progress } from '@/components/ui/progress'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import {
	CheckCircle,
	AlertCircle,
	Plus,
	Download,
	FileText,
	Trash2,
} from 'lucide-react'
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import type {
	Matrix,
	MatrixTopicDto,
	Question,
	Topic,
} from '@/types/model/exam-service'
import { CognitiveLevel } from '@/types/model/exam-service'

interface MatrixProgressTrackerProps {
	matrix: Matrix
	questions: Question[]
	topics: Topic[]
	onRequirementClick?: (topicId: string, cognitiveLevel: CognitiveLevel) => void
	interactive?: boolean
	onExportPdf?: () => void
	onExportExcel?: () => void
	onDelete?: () => void
	isExportingPdf?: boolean
	isExportingExcel?: boolean
	isDeleting?: boolean
}

interface ProgressItem {
	topicId: string
	topicName: string
	cognitiveLevel: CognitiveLevel
	required: number
	created: number
	percentage: number
}

export const MatrixProgressTracker: React.FC<MatrixProgressTrackerProps> = ({
	matrix,
	questions,
	topics,
	onRequirementClick,
	interactive = false,
	onExportPdf,
	onExportExcel,
	onDelete,
	isExportingPdf = false,
	isExportingExcel = false,
	isDeleting = false,
}) => {
	const progressData = useMemo(() => {
		const data: ProgressItem[] = []

		matrix.matrixTopics.forEach((mt: MatrixTopicDto) => {
			const topic = topics.find(t => t.id === mt.topicId)
			const createdCount = questions.filter(
				q => q.topicId === mt.topicId && q.cognitiveLevel === mt.cognitiveLevel
			).length

			data.push({
				topicId: mt.topicId,
				topicName: topic?.title || 'Unknown Topic',
				cognitiveLevel: mt.cognitiveLevel,
				required: mt.quantity,
				created: createdCount,
				percentage: mt.quantity > 0 ? (createdCount / mt.quantity) * 100 : 0,
			})
		})

		return data
	}, [matrix, questions, topics])

	const overallProgress = useMemo(() => {
		const totalRequired = progressData.reduce(
			(sum, item) => sum + item.required,
			0
		)
		const totalCreated = progressData.reduce(
			(sum, item) => sum + item.created,
			0
		)
		return totalRequired > 0 ? (totalCreated / totalRequired) * 100 : 0
	}, [progressData])

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

	const getCognitiveLevelVariant = (
		level: CognitiveLevel
	): 'default' | 'secondary' | 'outline' | 'destructive' => {
		switch (level) {
			case CognitiveLevel.Remember:
				return 'secondary'
			case CognitiveLevel.Understand:
				return 'default'
			case CognitiveLevel.Apply:
				return 'outline'
			case CognitiveLevel.Analyze:
				return 'destructive'
			default:
				return 'outline'
		}
	}

	return (
		<div className='px-6'>
			<div className='px-2 pb-2'>
				<div className='flex items-center justify-between'>
					<CardTitle className={`flex items-center gap-2`}>
						<span>Matrix Progress</span>
						{overallProgress === 100 && (
							<CheckCircle className='h-5 w-5 text-primary' />
						)}
					</CardTitle>
					<div className='flex items-center gap-2'>
						{(onExportPdf || onExportExcel || onDelete) && (
							<div className='flex items-center gap-1'>
								{(onExportPdf || onExportExcel) && (
									<DropdownMenu>
										<DropdownMenuTrigger asChild>
											<Button
												variant='ghost'
												size='sm'
												disabled={isExportingPdf || isExportingExcel}
											>
												<Download className='h-4 w-4 mr-1' />
												Export
											</Button>
										</DropdownMenuTrigger>
										<DropdownMenuContent>
											{onExportPdf && (
												<DropdownMenuItem
													onClick={onExportPdf}
													disabled={isExportingPdf}
												>
													<FileText className='h-4 w-4 mr-2' />
													{isExportingPdf
														? 'Exporting PDF...'
														: 'Export as PDF'}
												</DropdownMenuItem>
											)}
											{onExportExcel && (
												<DropdownMenuItem
													onClick={onExportExcel}
													disabled={isExportingExcel}
												>
													<FileText className='h-4 w-4 mr-2' />
													{isExportingExcel
														? 'Exporting Excel...'
														: 'Export as Excel'}
												</DropdownMenuItem>
											)}
										</DropdownMenuContent>
									</DropdownMenu>
								)}
								{onDelete && (
									<Button
										variant='ghost'
										size='sm'
										onClick={onDelete}
										disabled={isDeleting}
										className='text-destructive hover:text-destructive'
									>
										<Trash2 className='h-4 w-4 mr-1' />
										{isDeleting ? 'Deleting...' : 'Delete'}
									</Button>
								)}
							</div>
						)}
						<div className='text-sm font-medium'>
							<span className={`font-bold text-primary `}>
								{Math.round(overallProgress)}%
							</span>
							<span className='text-muted-foreground ml-1'>Complete</span>
						</div>
					</div>
				</div>
				<Progress className='mb-2' value={overallProgress} />
			</div>
			<CardContent className='space-y-2 mt-2 p-0'>
				{progressData.map((item, index) => {
					const isComplete = item.created >= item.required
					const isOver = item.created > item.required
					const canAddMore = !isComplete

					return (
						<div
							key={`${item.topicId}-${item.cognitiveLevel}-${index}`}
							className={`p-3 rounded-lg border bg-card transition-colors ${
								interactive && canAddMore
									? 'cursor-pointer hover:bg-accent hover:border-primary/50'
									: 'hover:bg-accent'
							} ${isComplete ? 'border-primary/30 bg-primary/5' : ''}`}
							onClick={() => {
								if (interactive && canAddMore && onRequirementClick) {
									onRequirementClick(item.topicId, item.cognitiveLevel)
								}
							}}
						>
							<div className='flex items-center justify-between mb-1.5'>
								<div className='flex items-center gap-2'>
									<span className='text-sm font-medium'>{item.topicName}</span>
									<Badge
										variant={getCognitiveLevelVariant(item.cognitiveLevel)}
									>
										{getCognitiveLevelLabel(item.cognitiveLevel)}
									</Badge>
								</div>
								<div className='flex items-center gap-2'>
									<span className='text-sm text-muted-foreground'>
										{item.created}/{item.required}
									</span>
									{isComplete && (
										<CheckCircle className='h-4 w-4 text-primary' />
									)}
									{isOver && (
										<AlertCircle className='h-4 w-4 text-muted-foreground' />
									)}
									{interactive && canAddMore && (
										<Button
											variant='ghost'
											size='icon'
											className='h-6 w-6'
											onClick={e => {
												e.stopPropagation()
												if (onRequirementClick) {
													onRequirementClick(item.topicId, item.cognitiveLevel)
												}
											}}
										>
											<Plus className='h-4 w-4' />
										</Button>
									)}
								</div>
							</div>
							<Progress value={item.percentage} className='h-2' />
						</div>
					)
				})}
			</CardContent>
		</div>
	)
}
