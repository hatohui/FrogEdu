import React, { useMemo } from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Progress } from '@/components/ui/progress'
import { Badge } from '@/components/ui/badge'
import { CheckCircle, AlertCircle } from 'lucide-react'
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

	const getCognitiveLevelColor = (level: CognitiveLevel) => {
		switch (level) {
			case CognitiveLevel.Remember:
				return 'bg-blue-500 text-white'
			case CognitiveLevel.Understand:
				return 'bg-green-500 text-white'
			case CognitiveLevel.Apply:
				return 'bg-yellow-500 text-white'
			case CognitiveLevel.Analyze:
				return 'bg-red-500 text-white'
			default:
				return 'bg-gray-500 text-white'
		}
	}

	return (
		<Card className='border-2 border-primary/20 bg-gradient-to-br from-primary/5 to-background'>
			<CardHeader>
				<div className='flex items-center justify-between'>
					<CardTitle className='flex items-center gap-2'>
						<span>Matrix Progress</span>
						{overallProgress === 100 && (
							<CheckCircle className='h-5 w-5 text-green-500' />
						)}
					</CardTitle>
					<div className='text-sm font-medium'>
						<span className='text-2xl font-bold'>
							{Math.round(overallProgress)}%
						</span>
						<span className='text-muted-foreground ml-1'>Complete</span>
					</div>
				</div>
				<Progress value={overallProgress} className='h-3' />
			</CardHeader>
			<CardContent className='space-y-3'>
				{progressData.map((item, index) => {
					const isComplete = item.created >= item.required
					const isOver = item.created > item.required

					return (
						<div
							key={`${item.topicId}-${item.cognitiveLevel}-${index}`}
							className='p-3 rounded-lg border bg-card hover:bg-accent/50 transition-colors'
						>
							<div className='flex items-center justify-between mb-2'>
								<div className='flex items-center gap-2'>
									<span className='text-sm font-medium'>{item.topicName}</span>
									<Badge
										className={getCognitiveLevelColor(item.cognitiveLevel)}
										variant='default'
									>
										{getCognitiveLevelLabel(item.cognitiveLevel)}
									</Badge>
								</div>
								<div className='flex items-center gap-2'>
									<span className='text-sm'>
										{item.created}/{item.required}
									</span>
									{isComplete && (
										<CheckCircle className='h-4 w-4 text-green-500' />
									)}
									{isOver && (
										<AlertCircle className='h-4 w-4 text-yellow-500' />
									)}
								</div>
							</div>
							<Progress value={item.percentage} className='h-2' />
						</div>
					)
				})}
			</CardContent>
		</Card>
	)
}
