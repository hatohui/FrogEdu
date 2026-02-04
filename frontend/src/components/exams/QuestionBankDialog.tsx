import React, { useState, useMemo } from 'react'
import {
	Search,
	Filter,
	CheckCircle,
	AlertCircle,
	Loader2,
	Library,
	Sparkles,
} from 'lucide-react'
import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogFooter,
	DialogHeader,
	DialogTitle,
} from '@/components/ui/dialog'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import { Checkbox } from '@/components/ui/checkbox'
import { ScrollArea } from '@/components/ui/scroll-area'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { Tabs, TabsList, TabsTrigger } from '@/components/ui/tabs'
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
import { filterQuestionsForMatrix } from '@/utils/matrixValidation'

interface QuestionBankDialogProps {
	open: boolean
	onOpenChange: (open: boolean) => void
	allQuestions: Question[]
	existingQuestionIds: Set<string>
	existingQuestions: Question[]
	topics: Topic[]
	matrix?: Matrix | null
	onAddQuestions: (questionIds: string[]) => Promise<void>
	isLoading?: boolean
	isAdding?: boolean
}

/**
 * Dialog for selecting questions from the question bank to add to an exam
 */
export const QuestionBankDialog: React.FC<QuestionBankDialogProps> = ({
	open,
	onOpenChange,
	allQuestions,
	existingQuestionIds,
	existingQuestions,
	topics,
	matrix,
	onAddQuestions,
	isLoading = false,
	isAdding = false,
}) => {
	const [searchQuery, setSearchQuery] = useState('')
	const [filterLevel, setFilterLevel] = useState<string>('all')
	const [filterTopic, setFilterTopic] = useState<string>('all')
	const [viewMode, setViewMode] = useState<'all' | 'matrix'>('matrix')
	const [selectedQuestions, setSelectedQuestions] = useState<Set<string>>(
		new Set()
	)

	// Filter out questions already in exam
	const availableQuestions = useMemo(() => {
		return allQuestions.filter(q => !existingQuestionIds.has(q.id))
	}, [allQuestions, existingQuestionIds])

	// Apply view mode filter (matrix vs all)
	const viewFilteredQuestions = useMemo(() => {
		if (viewMode === 'matrix' && matrix) {
			return filterQuestionsForMatrix(
				availableQuestions,
				matrix,
				existingQuestions
			)
		}
		return availableQuestions
	}, [availableQuestions, viewMode, matrix, existingQuestions])

	// Apply search and filter
	const filteredQuestions = useMemo(() => {
		return viewFilteredQuestions.filter(question => {
			const matchesSearch = question.content
				.toLowerCase()
				.includes(searchQuery.toLowerCase())
			const matchesLevel =
				filterLevel === 'all' || question.cognitiveLevel === Number(filterLevel)
			const matchesTopic =
				filterTopic === 'all' || question.topicId === filterTopic
			return matchesSearch && matchesLevel && matchesTopic
		})
	}, [viewFilteredQuestions, searchQuery, filterLevel, filterTopic])

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

	const toggleQuestion = (questionId: string) => {
		const newSelected = new Set(selectedQuestions)
		if (newSelected.has(questionId)) {
			newSelected.delete(questionId)
		} else {
			newSelected.add(questionId)
		}
		setSelectedQuestions(newSelected)
	}

	const toggleAll = () => {
		if (selectedQuestions.size === filteredQuestions.length) {
			setSelectedQuestions(new Set())
		} else {
			setSelectedQuestions(new Set(filteredQuestions.map(q => q.id)))
		}
	}

	const handleAddSelected = async () => {
		await onAddQuestions(Array.from(selectedQuestions))
		setSelectedQuestions(new Set())
		onOpenChange(false)
	}

	// Check if question would exceed matrix limit
	const wouldExceed = (question: Question): boolean => {
		if (!matrix) return false

		// Include currently selected questions in the check
		const selectedForThisSlot = Array.from(selectedQuestions)
			.map(id => availableQuestions.find(q => q.id === id))
			.filter(
				q =>
					q &&
					q.topicId === question.topicId &&
					q.cognitiveLevel === question.cognitiveLevel
			).length

		const currentCount = existingQuestions.filter(
			q =>
				q.topicId === question.topicId &&
				q.cognitiveLevel === question.cognitiveLevel
		).length

		const requirement = matrix.matrixTopics.find(
			mt =>
				mt.topicId === question.topicId &&
				mt.cognitiveLevel === question.cognitiveLevel
		)

		if (!requirement) return true

		return currentCount + selectedForThisSlot >= requirement.quantity
	}

	return (
		<Dialog open={open} onOpenChange={onOpenChange}>
			<DialogContent className='max-w-4xl max-h-[85vh] flex flex-col'>
				<DialogHeader>
					<DialogTitle className='flex items-center gap-2'>
						<Library className='h-5 w-5' />
						Add from Question Bank
					</DialogTitle>
					<DialogDescription>
						Select questions from your question bank to add to this exam.
						{matrix &&
							' Questions are filtered to match your matrix requirements.'}
					</DialogDescription>
				</DialogHeader>

				{/* Filters */}
				<div className='flex flex-col sm:flex-row gap-3'>
					<div className='flex-1'>
						<div className='relative'>
							<Search className='absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-muted-foreground' />
							<Input
								placeholder='Search questions...'
								value={searchQuery}
								onChange={e => setSearchQuery(e.target.value)}
								className='pl-9'
							/>
						</div>
					</div>
					<Select value={filterTopic} onValueChange={setFilterTopic}>
						<SelectTrigger className='w-[160px]'>
							<Filter className='h-4 w-4 mr-2' />
							<SelectValue placeholder='Topic' />
						</SelectTrigger>
						<SelectContent>
							<SelectItem value='all'>All Topics</SelectItem>
							{topics.map(topic => (
								<SelectItem key={topic.id} value={topic.id}>
									{topic.title}
								</SelectItem>
							))}
						</SelectContent>
					</Select>
					<Select value={filterLevel} onValueChange={setFilterLevel}>
						<SelectTrigger className='w-[160px]'>
							<Filter className='h-4 w-4 mr-2' />
							<SelectValue placeholder='Level' />
						</SelectTrigger>
						<SelectContent>
							<SelectItem value='all'>All Levels</SelectItem>
							<SelectItem value={String(CognitiveLevel.Remember)}>
								Remember
							</SelectItem>
							<SelectItem value={String(CognitiveLevel.Understand)}>
								Understand
							</SelectItem>
							<SelectItem value={String(CognitiveLevel.Apply)}>
								Apply
							</SelectItem>
							<SelectItem value={String(CognitiveLevel.Analyze)}>
								Analyze
							</SelectItem>
						</SelectContent>
					</Select>
					{matrix && (
						<Tabs
							value={viewMode}
							onValueChange={v => setViewMode(v as 'all' | 'matrix')}
						>
							<TabsList className='grid w-full grid-cols-2'>
								<TabsTrigger value='matrix'>
									<Sparkles className='h-3 w-3 mr-1' />
									Matrix
								</TabsTrigger>
								<TabsTrigger value='all'>All</TabsTrigger>
							</TabsList>
						</Tabs>
					)}
				</div>

				{/* Question List */}
				<ScrollArea className='flex-1 border rounded-lg'>
					{isLoading ? (
						<div className='flex items-center justify-center py-12'>
							<Loader2 className='h-8 w-8 animate-spin text-primary' />
						</div>
					) : filteredQuestions.length === 0 ? (
						<div className='flex flex-col items-center justify-center py-12 text-center'>
							<AlertCircle className='h-10 w-10 text-muted-foreground mb-3' />
							<p className='text-sm font-medium mb-1'>No questions found</p>
							<p className='text-xs text-muted-foreground'>
								{viewMode === 'matrix'
									? 'All matrix requirements are fulfilled or no matching questions exist'
									: 'Try adjusting your search or filters'}
							</p>
						</div>
					) : (
						<div className='divide-y'>
							{/* Select All Header */}
							<div className='p-3 bg-muted/50 sticky top-0 z-10'>
								<div className='flex items-center gap-3'>
									<Checkbox
										checked={
											selectedQuestions.size === filteredQuestions.length &&
											filteredQuestions.length > 0
										}
										onCheckedChange={toggleAll}
									/>
									<span className='text-sm font-medium'>
										{selectedQuestions.size > 0
											? `${selectedQuestions.size} selected`
											: 'Select all'}
									</span>
								</div>
							</div>

							{/* Questions */}
							{filteredQuestions.map(question => {
								const isSelected = selectedQuestions.has(question.id)
								const exceeds = !isSelected && wouldExceed(question)

								return (
									<div
										key={question.id}
										className={`p-3 hover:bg-muted/50 transition-colors ${
											isSelected ? 'bg-primary/5' : ''
										} ${exceeds ? 'opacity-60' : ''}`}
									>
										<div className='flex items-start gap-3'>
											<TooltipProvider>
												<Tooltip>
													<TooltipTrigger asChild>
														<div>
															<Checkbox
																checked={isSelected}
																onCheckedChange={() =>
																	!exceeds && toggleQuestion(question.id)
																}
																disabled={exceeds}
															/>
														</div>
													</TooltipTrigger>
													{exceeds && (
														<TooltipContent>
															<p>Matrix limit reached for this requirement</p>
														</TooltipContent>
													)}
												</Tooltip>
											</TooltipProvider>
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
												</div>
											</div>
										</div>
									</div>
								)
							})}
						</div>
					)}
				</ScrollArea>

				<DialogFooter className='gap-2'>
					<Button variant='outline' onClick={() => onOpenChange(false)}>
						Cancel
					</Button>
					<Button
						onClick={handleAddSelected}
						disabled={selectedQuestions.size === 0 || isAdding}
					>
						{isAdding ? (
							<>
								<Loader2 className='h-4 w-4 mr-2 animate-spin' />
								Adding...
							</>
						) : (
							<>
								<CheckCircle className='h-4 w-4 mr-2' />
								Add {selectedQuestions.size} Question
								{selectedQuestions.size !== 1 ? 's' : ''}
							</>
						)}
					</Button>
				</DialogFooter>
			</DialogContent>
		</Dialog>
	)
}

export default QuestionBankDialog
