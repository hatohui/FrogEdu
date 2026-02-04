import React, { useMemo } from 'react'
import { CardContent } from '@/components/ui/card'
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
		<div className='p-6'>
			<div className='space-y-4'>
				{/* Header Section */}
				<div className='flex items-start justify-between gap-4'>
					<div className='space-y-1'>
						<div className='flex items-center gap-2'>
							<h3 className='text-base font-semibold tracking-tight'>
								Matrix Progress
							</h3>
							{overallProgress === 100 && (
								<CheckCircle className='h-4 w-4 text-primary' />
							)}
						</div>
						<p className='text-sm text-muted-foreground'>
							Track your exam question distribution progress
						</p>
					</div>

					<div className='flex items-center gap-2'>
						{(onExportPdf || onExportExcel || onDelete) && (
							<div className='flex items-center gap-1'>
								{(onExportPdf || onExportExcel) && (
									<DropdownMenu>
										<DropdownMenuTrigger asChild>
											<Button
												variant='outline'
												size='sm'
												disabled={isExportingPdf || isExportingExcel}
											>
												<Download className='h-3.5 w-3.5 mr-1.5' />
												<span className='text-xs'>Export</span>
											</Button>
										</DropdownMenuTrigger>
										<DropdownMenuContent align='end'>
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
										variant='outline'
										size='sm'
										onClick={onDelete}
										disabled={isDeleting}
										className='text-destructive hover:text-destructive hover:bg-destructive/10'
									>
										<Trash2 className='h-3.5 w-3.5 mr-1.5' />
										<span className='text-xs'>
											{isDeleting ? 'Deleting...' : 'Delete'}
										</span>
									</Button>
								)}
							</div>
						)}
					</div>
				</div>

				{/* Progress Bar Section */}
				<div className='space-y-2'>
					<div className='flex items-center justify-between'>
						<span className='text-xs font-medium text-muted-foreground uppercase tracking-wider'>
							Overall Progress
						</span>
						<div className='flex items-baseline gap-1'>
							<span className='text-lg font-bold text-primary'>
								{Math.round(overallProgress)}%
							</span>
							<span className='text-xs text-muted-foreground'>Complete</span>
						</div>
					</div>
					<Progress className='h-2' value={overallProgress} />
				</div>
			</div>

			{/* Requirements List */}
			<CardContent className='space-y-2.5 mt-6 p-0'>
				{progressData.map((item, index) => {
					const isComplete = item.created >= item.required
					const isOver = item.created > item.required
					const canAddMore = !isComplete

					return (
						<div
							key={`${item.topicId}-${item.cognitiveLevel}-${index}`}
							className={`group relative p-4 rounded-lg border transition-all duration-200 ${
								interactive && canAddMore
									? 'cursor-pointer hover:shadow-sm hover:border-primary/50 hover:bg-accent/50'
									: 'hover:bg-accent/30'
							} ${isComplete ? 'border-primary/20 bg-primary/5' : 'border-border bg-card'}`}
							onClick={() => {
								if (interactive && canAddMore && onRequirementClick) {
									onRequirementClick(item.topicId, item.cognitiveLevel)
								}
							}}
						>
							<div className='space-y-2.5'>
								<div className='flex items-start justify-between gap-3'>
									<div className='flex-1 min-w-0 space-y-1.5'>
										<h4 className='text-sm font-medium leading-none truncate'>
											{item.topicName}
										</h4>
										<Badge
											variant={getCognitiveLevelVariant(item.cognitiveLevel)}
											className='text-xs'
										>
											{getCognitiveLevelLabel(item.cognitiveLevel)}
										</Badge>
									</div>

									<div className='flex items-center gap-2 flex-shrink-0'>
										<div className='text-right'>
											<div className='flex items-baseline gap-1'>
												<span className='text-sm font-semibold tabular-nums'>
													{item.created}
												</span>
												<span className='text-xs text-muted-foreground'>/</span>
												<span className='text-sm font-medium text-muted-foreground tabular-nums'>
													{item.required}
												</span>
											</div>
										</div>
										{isComplete && (
											<CheckCircle className='h-4 w-4 text-primary flex-shrink-0' />
										)}
										{isOver && (
											<AlertCircle className='h-4 w-4 text-amber-500 flex-shrink-0' />
										)}
										{interactive && canAddMore && (
											<Button
												variant='ghost'
												size='icon'
												className='h-7 w-7 opacity-0 group-hover:opacity-100 transition-opacity flex-shrink-0'
												onClick={e => {
													e.stopPropagation()
													if (onRequirementClick) {
														onRequirementClick(
															item.topicId,
															item.cognitiveLevel
														)
													}
												}}
											>
												<Plus className='h-3.5 w-3.5' />
											</Button>
										)}
									</div>
								</div>
								<Progress value={item.percentage} className='h-1.5' />
							</div>
						</div>
					)
				})}
			</CardContent>
		</div>
	)
}
