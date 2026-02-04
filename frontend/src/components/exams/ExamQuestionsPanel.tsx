import React from 'react'
import { Trash2, AlertCircle, Eye } from 'lucide-react'
import { CardContent } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { ScrollArea } from '@/components/ui/scroll-area'
import {
	AlertDialog,
	AlertDialogAction,
	AlertDialogCancel,
	AlertDialogContent,
	AlertDialogDescription,
	AlertDialogFooter,
	AlertDialogHeader,
	AlertDialogTitle,
	AlertDialogTrigger,
} from '@/components/ui/alert-dialog'
import {
	Tooltip,
	TooltipContent,
	TooltipProvider,
	TooltipTrigger,
} from '@/components/ui/tooltip'
import type { Question, Topic, Matrix } from '@/types/model/exam-service'
import {
	CognitiveLevel,
	getCognitiveLevelLabel,
} from '@/types/model/exam-service'

interface ExamQuestionsPanelProps {
	questions: Question[]
	topics: Topic[]
	matrix?: Matrix | null
	onRemoveQuestion: (questionId: string) => void
	onViewQuestion?: (questionId: string) => void
	isRemoving?: boolean
}

/**
 * Panel displaying questions currently associated with the exam
 */
export const ExamQuestionsPanel: React.FC<ExamQuestionsPanelProps> = ({
	questions,
	topics,
	matrix,
	onRemoveQuestion,
	onViewQuestion,
	isRemoving = false,
}) => {
	const getTopicName = (topicId: string) => {
		const topic = topics.find(t => t.id === topicId)
		return topic?.title || 'Unknown Topic'
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

	// Check if question matches matrix requirement
	const isQuestionInMatrix = (question: Question): boolean => {
		if (!matrix) return true
		return matrix.matrixTopics.some(
			mt =>
				mt.topicId === question.topicId &&
				mt.cognitiveLevel === question.cognitiveLevel
		)
	}

	if (questions.length === 0) {
		return (
			<div>
				<CardContent className='flex flex-col items-center justify-center py-8 text-center'>
					<AlertCircle className='h-10 w-10 text-muted-foreground mb-3' />
					<p className='text-sm font-medium mb-1'>No questions added yet</p>
					<p className='text-xs text-muted-foreground'>
						Create questions or add from the question bank
					</p>
				</CardContent>
			</div>
		)
	}

	return (
		<>
			<CardContent className='p-0'>
				<ScrollArea className='h-[300px] space-y-4'>
					<div className='divide-y'>
						{questions.map((question, index) => {
							const inMatrix = isQuestionInMatrix(question)

							return (
								<div
									key={question.id}
									className={`p-3 hover:bg-muted/50 rounded-lg mx-4 border-dashed border-accent border transition-colors ${
										!inMatrix ? 'bg-yellow-50/50 dark:bg-yellow-900/10' : ''
									}`}
									onContextMenu={e => {
										e.preventDefault()
										if (onViewQuestion) {
											onViewQuestion(question.id)
										}
									}}
								>
									<div className='flex items-start gap-3'>
										<span className='flex-shrink-0 w-6 h-6 rounded-full bg-primary/10 text-primary text-xs font-medium flex items-center justify-center'>
											{index + 1}
										</span>
										<div className='flex-1 min-w-0'>
											<p className='text-sm line-clamp-2 mb-2'>
												{question.content}
											</p>
											<div className='flex items-center gap-2 flex-wrap'>
												<Badge variant='outline' className='text-xs'>
													{getTopicName(question.topicId)}
												</Badge>
												<Badge
													variant={getCognitiveLevelVariant(
														question.cognitiveLevel
													)}
													className='text-xs'
												>
													{getCognitiveLevelLabel(question.cognitiveLevel)}
												</Badge>
												<span className='text-xs text-muted-foreground'>
													{question.point} pts
												</span>
												{!inMatrix && (
													<TooltipProvider>
														<Tooltip>
															<TooltipTrigger>
																<Badge
																	variant='outline'
																	className='text-xs text-yellow-600 border-yellow-400'
																>
																	Not in matrix
																</Badge>
															</TooltipTrigger>
															<TooltipContent>
																<p>
																	This question doesn't match any matrix
																	requirement
																</p>
															</TooltipContent>
														</Tooltip>
													</TooltipProvider>
												)}
											</div>
										</div>
										<div className='flex items-center gap-1'>
											{onViewQuestion && (
												<TooltipProvider>
													<Tooltip>
														<TooltipTrigger asChild>
															<Button
																variant='ghost'
																size='icon'
																className='h-8 w-8'
																onClick={() => onViewQuestion(question.id)}
															>
																<Eye className='h-4 w-4' />
															</Button>
														</TooltipTrigger>
														<TooltipContent>View question</TooltipContent>
													</Tooltip>
												</TooltipProvider>
											)}
											<AlertDialog>
												<TooltipProvider>
													<Tooltip>
														<TooltipTrigger asChild>
															<AlertDialogTrigger asChild>
																<Button
																	variant='ghost'
																	size='icon'
																	className='h-8 w-8 text-destructive hover:text-destructive'
																	disabled={isRemoving}
																>
																	<Trash2 className='h-4 w-4' />
																</Button>
															</AlertDialogTrigger>
														</TooltipTrigger>
														<TooltipContent>Remove from exam</TooltipContent>
													</Tooltip>
												</TooltipProvider>
												<AlertDialogContent>
													<AlertDialogHeader>
														<AlertDialogTitle>
															Remove question?
														</AlertDialogTitle>
														<AlertDialogDescription>
															This will remove the question from this exam only.
															The question will remain in the question bank.
														</AlertDialogDescription>
													</AlertDialogHeader>
													<AlertDialogFooter>
														<AlertDialogCancel>Cancel</AlertDialogCancel>
														<AlertDialogAction
															onClick={() => onRemoveQuestion(question.id)}
															className='bg-destructive hover:bg-destructive/90'
														>
															Remove
														</AlertDialogAction>
													</AlertDialogFooter>
												</AlertDialogContent>
											</AlertDialog>
										</div>
									</div>
								</div>
							)
						})}
					</div>
				</ScrollArea>
			</CardContent>
		</>
	)
}

export default ExamQuestionsPanel
