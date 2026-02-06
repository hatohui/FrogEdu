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
import { CognitiveLevel } from '@/types/model/exam-service'
import { useTranslation } from 'react-i18next'

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
	const { t } = useTranslation()
	const getTopicName = (topicId: string) => {
		const topic = topics.find(t => t.id === topicId)
		return topic?.title || t('common.unknown')
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

	const getLevelLabel = (level: CognitiveLevel) => {
		switch (level) {
			case CognitiveLevel.Remember:
				return t('exams.cognitive_levels.remember')
			case CognitiveLevel.Understand:
				return t('exams.cognitive_levels.understand')
			case CognitiveLevel.Apply:
				return t('exams.cognitive_levels.apply')
			case CognitiveLevel.Analyze:
				return t('exams.cognitive_levels.analyze')
			default:
				return t('common.unknown')
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
					<p className='text-sm font-medium mb-1'>
						{t('components.exams.questions_panel.empty_title')}
					</p>
					<p className='text-xs text-muted-foreground'>
						{t('components.exams.questions_panel.empty_subtitle')}
					</p>
				</CardContent>
			</div>
		)
	}

	return (
		<>
			<CardContent className='p-0'>
				<ScrollArea className='h-[300px]'>
					<div className='space-y-4 p-4'>
						{questions.map((question, index) => {
							const inMatrix = isQuestionInMatrix(question)

							return (
								<div
									key={question.id}
									className={`p-3 hover:bg-muted/50 rounded-lg border-dashed border-accent border transition-colors ${
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
													{getLevelLabel(question.cognitiveLevel)}
												</Badge>
												<span className='text-xs text-muted-foreground'>
													{t('components.exams.questions_panel.points', {
														count: question.point,
													})}
												</span>
												{!inMatrix && (
													<TooltipProvider>
														<Tooltip>
															<TooltipTrigger>
																<Badge
																	variant='outline'
																	className='text-xs text-yellow-600 border-yellow-400'
																>
																	{t(
																		'components.exams.questions_panel.not_in_matrix'
																	)}
																</Badge>
															</TooltipTrigger>
															<TooltipContent>
																<p>
																	{t(
																		'components.exams.questions_panel.not_in_matrix_hint'
																	)}
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
														<TooltipContent>
															{t('components.exams.questions_panel.view')}
														</TooltipContent>
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
														<TooltipContent>
															{t('components.exams.questions_panel.remove')}
														</TooltipContent>
													</Tooltip>
												</TooltipProvider>
												<AlertDialogContent>
													<AlertDialogHeader>
														<AlertDialogTitle>
															{t(
																'components.exams.questions_panel.remove_title'
															)}
														</AlertDialogTitle>
														<AlertDialogDescription>
															{t(
																'components.exams.questions_panel.remove_description'
															)}
														</AlertDialogDescription>
													</AlertDialogHeader>
													<AlertDialogFooter>
														<AlertDialogCancel>
															{t('common.cancel')}
														</AlertDialogCancel>
														<AlertDialogAction
															onClick={() => onRemoveQuestion(question.id)}
															className='bg-destructive hover:bg-destructive/90'
														>
															{t(
																'components.exams.questions_panel.remove_confirm'
															)}
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
