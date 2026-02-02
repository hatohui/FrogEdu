import React, { useState } from 'react'
import { useNavigate, useParams } from 'react-router'
import {
	ArrowLeft,
	Plus,
	Search,
	Filter,
	Eye,
	Pencil,
	CheckCircle,
} from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import { Checkbox } from '@/components/ui/checkbox'
import { useQuestions } from '@/hooks/useExams'
import { CognitiveLevel, QuestionType } from '@/types/model/exams'

const QuestionBankPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const { examId } = useParams<{ examId: string }>()

	const [searchQuery, setSearchQuery] = useState('')
	const [filterLevel, setFilterLevel] = useState<string>('all')
	const [selectedQuestions, setSelectedQuestions] = useState<Set<string>>(
		new Set()
	)

	const { data: questions, isLoading } = useQuestions({
		isPublic: true,
	})

	const filteredQuestions = questions?.filter(question => {
		const matchesSearch = question.content
			.toLowerCase()
			.includes(searchQuery.toLowerCase())
		const matchesLevel =
			filterLevel === 'all' || question.cognitiveLevel === Number(filterLevel)
		return matchesSearch && matchesLevel
	})

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
		if (selectedQuestions.size === filteredQuestions?.length) {
			setSelectedQuestions(new Set())
		} else {
			setSelectedQuestions(new Set(filteredQuestions?.map(q => q.id) || []))
		}
	}

	const handleAddSelectedToExam = () => {
		// TODO: Implement adding selected questions to exam
		console.log('Adding questions to exam:', Array.from(selectedQuestions))
		navigate(`/app/exams/${examId}`)
	}

	const getCognitiveLevelLabel = (level: CognitiveLevel) => {
		switch (level) {
			case CognitiveLevel.Easy:
				return 'Easy'
			case CognitiveLevel.Medium:
				return 'Medium'
			case CognitiveLevel.Hard:
				return 'Hard'
			default:
				return 'Unknown'
		}
	}

	const getCognitiveLevelColor = (level: CognitiveLevel) => {
		switch (level) {
			case CognitiveLevel.Easy:
				return 'bg-green-500/10 text-green-700 dark:text-green-400'
			case CognitiveLevel.Medium:
				return 'bg-yellow-500/10 text-yellow-700 dark:text-yellow-400'
			case CognitiveLevel.Hard:
				return 'bg-red-500/10 text-red-700 dark:text-red-400'
			default:
				return ''
		}
	}

	const getQuestionTypeLabel = (type: QuestionType) => {
		switch (type) {
			case QuestionType.MultipleChoice:
				return 'Multiple Choice'
			case QuestionType.TrueFalse:
				return 'True/False'
			case QuestionType.Essay:
				return 'Essay'
			case QuestionType.FillInBlank:
				return 'Fill in Blank'
			default:
				return 'Unknown'
		}
	}

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div className='flex items-center space-x-4'>
					<Button
						variant='ghost'
						size='icon'
						onClick={() => navigate(`/app/exams/${examId}`)}
					>
						<ArrowLeft className='h-5 w-5' />
					</Button>
					<div>
						<h1 className='text-3xl font-bold'>Question Bank</h1>
						<p className='text-muted-foreground'>
							Select questions for your exam
						</p>
					</div>
				</div>
				<div className='flex gap-2'>
					<Button
						variant='outline'
						onClick={() => navigate(`/app/exams/${examId}/questions/create`)}
					>
						<Plus className='h-4 w-4 mr-2' />
						Create Question
					</Button>
					<Button
						onClick={handleAddSelectedToExam}
						disabled={selectedQuestions.size === 0}
					>
						<CheckCircle className='h-4 w-4 mr-2' />
						Add {selectedQuestions.size} Selected
					</Button>
				</div>
			</div>

			{/* Filters */}
			<Card>
				<CardContent className='pt-6'>
					<div className='flex gap-4'>
						<div className='flex-1 relative'>
							<Search className='absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-muted-foreground' />
							<Input
								placeholder='Search questions...'
								value={searchQuery}
								onChange={e => setSearchQuery(e.target.value)}
								className='pl-9'
							/>
						</div>
						<Select value={filterLevel} onValueChange={setFilterLevel}>
							<SelectTrigger className='w-[200px]'>
								<Filter className='h-4 w-4 mr-2' />
								<SelectValue placeholder='Difficulty' />
							</SelectTrigger>
							<SelectContent>
								<SelectItem value='all'>All Levels</SelectItem>
								<SelectItem value='0'>Easy</SelectItem>
								<SelectItem value='1'>Medium</SelectItem>
								<SelectItem value='2'>Hard</SelectItem>
							</SelectContent>
						</Select>
					</div>
				</CardContent>
			</Card>

			{/* Questions Table */}
			<Card>
				<CardHeader>
					<CardTitle>
						Available Questions ({filteredQuestions?.length || 0})
					</CardTitle>
				</CardHeader>
				<CardContent>
					{isLoading ? (
						<div className='text-center py-12 text-muted-foreground'>
							Loading questions...
						</div>
					) : filteredQuestions && filteredQuestions.length > 0 ? (
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead className='w-12'>
										<Checkbox
											checked={
												selectedQuestions.size === filteredQuestions.length &&
												filteredQuestions.length > 0
											}
											onCheckedChange={toggleAll}
										/>
									</TableHead>
									<TableHead className='w-[50%]'>Question</TableHead>
									<TableHead>Type</TableHead>
									<TableHead>Level</TableHead>
									<TableHead>Points</TableHead>
									<TableHead className='text-right'>Actions</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{filteredQuestions.map(question => (
									<TableRow key={question.id}>
										<TableCell>
											<Checkbox
												checked={selectedQuestions.has(question.id)}
												onCheckedChange={() => toggleQuestion(question.id)}
											/>
										</TableCell>
										<TableCell className='font-medium'>
											<div className='line-clamp-2'>{question.content}</div>
										</TableCell>
										<TableCell>
											<Badge variant='outline'>
												{getQuestionTypeLabel(question.type)}
											</Badge>
										</TableCell>
										<TableCell>
											<Badge
												className={getCognitiveLevelColor(
													question.cognitiveLevel
												)}
											>
												{getCognitiveLevelLabel(question.cognitiveLevel)}
											</Badge>
										</TableCell>
										<TableCell>{question.point}</TableCell>
										<TableCell className='text-right space-x-2'>
											<Button
												variant='ghost'
												size='icon'
												onClick={() =>
													navigate(
														`/app/exams/${examId}/questions/${question.id}/preview`
													)
												}
											>
												<Eye className='h-4 w-4' />
											</Button>
											<Button
												variant='ghost'
												size='icon'
												onClick={() =>
													navigate(
														`/app/exams/${examId}/questions/${question.id}/edit`
													)
												}
											>
												<Pencil className='h-4 w-4' />
											</Button>
										</TableCell>
									</TableRow>
								))}
							</TableBody>
						</Table>
					) : (
						<div className='text-center py-12'>
							<Search className='h-12 w-12 mx-auto text-muted-foreground mb-4' />
							<p className='text-muted-foreground mb-4'>No questions found</p>
							<Button
								onClick={() =>
									navigate(`/app/exams/${examId}/questions/create`)
								}
							>
								<Plus className='h-4 w-4 mr-2' />
								Create Your First Question
							</Button>
						</div>
					)}
				</CardContent>
			</Card>
		</div>
	)
}

export default QuestionBankPage
