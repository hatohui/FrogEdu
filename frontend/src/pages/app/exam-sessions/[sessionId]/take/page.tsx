import React, { useState, useMemo, useCallback } from 'react'
import { useParams, useNavigate } from 'react-router'
import { useTranslation } from 'react-i18next'
import {
	useExamSessionDetail,
	useStartExamAttempt,
	useSubmitExamAttempt,
} from '@/hooks/useExamSessions'
import { useExamSessionData } from '@/hooks/useExams'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Skeleton } from '@/components/ui/skeleton'
import {
	Card,
	CardContent,
	CardDescription,
	CardFooter,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import {
	AlertDialog,
	AlertDialogAction,
	AlertDialogCancel,
	AlertDialogContent,
	AlertDialogDescription,
	AlertDialogFooter,
	AlertDialogHeader,
	AlertDialogTitle,
} from '@/components/ui/alert-dialog'
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group'
import { Checkbox } from '@/components/ui/checkbox'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Progress } from '@/components/ui/progress'
import {
	ArrowLeft,
	ArrowRight,
	CheckCircle2,
	Clock,
	LayoutList,
	List,
	Send,
	XCircle,
} from 'lucide-react'
import { QuestionType } from '@/types/model/exam-service'
import type { StudentAnswerSubmission } from '@/types/dtos/classes'

interface AnswerState {
	[questionId: string]: string[]
}

const ExamTakePage = (): React.ReactElement => {
	const { t } = useTranslation()
	const { sessionId } = useParams<{ sessionId: string }>()
	const navigate = useNavigate()

	// Data fetching
	const { data: session, isLoading: loadingSession } = useExamSessionDetail(
		sessionId || ''
	)
	const { data: examData, isLoading: loadingExam } = useExamSessionData(
		session?.examId || ''
	)

	// Mutations
	const startAttempt = useStartExamAttempt()
	const submitAttempt = useSubmitExamAttempt()

	// UI state
	const [viewMode, setViewMode] = useState<'paged' | 'full'>('paged')
	const [currentQuestion, setCurrentQuestion] = useState(0)
	const [highlightedQuestion, setHighlightedQuestion] = useState<string | null>(
		null
	)
	const [answers, setAnswers] = useState<AnswerState>({})
	const [attemptId, setAttemptId] = useState<string | null>(null)
	const [showSubmitDialog, setShowSubmitDialog] = useState(false)
	const [isSubmitted, setIsSubmitted] = useState(false)
	const [submittedResult, setSubmittedResult] = useState<{
		score: number
		totalPoints: number
		scorePercentage: number
	} | null>(null)

	const questions = useMemo(() => examData?.questions || [], [examData])

	const answeredCount = useMemo(
		() => Object.keys(answers).filter(k => answers[k].length > 0).length,
		[answers]
	)

	/** List of { id, index } for every question that has no answer yet */
	const unansweredQuestions = useMemo(
		() =>
			questions
				.map((q, idx) => ({ id: q.id, index: idx }))
				.filter(q => !answers[q.id] || answers[q.id].length === 0),
		[questions, answers]
	)

	const unansweredCount = unansweredQuestions.length

	/** Jump to a specific question and briefly highlight it */
	const jumpToQuestion = useCallback(
		(questionId: string, index: number) => {
			setShowSubmitDialog(false)
			setHighlightedQuestion(questionId)
			if (viewMode === 'paged') {
				setCurrentQuestion(index)
			} else {
				setTimeout(() => {
					document
						.getElementById(`question-${questionId}`)
						?.scrollIntoView({ behavior: 'smooth', block: 'center' })
				}, 50)
			}
			setTimeout(() => setHighlightedQuestion(null), 2000)
		},
		[viewMode]
	)

	const progress =
		questions.length > 0 ? (answeredCount / questions.length) * 100 : 0

	// ─── Handlers ───

	const handleStartAttempt = useCallback(async () => {
		if (!sessionId) return
		try {
			const result = await startAttempt.mutateAsync(sessionId)
			setAttemptId(result.id)
		} catch {
			// Error handled by hook
		}
	}, [sessionId, startAttempt])

	const handleSelectAnswer = useCallback(
		(questionId: string, answerId: string, questionType: QuestionType) => {
			setAnswers(prev => {
				const current = prev[questionId] || []

				if (questionType === QuestionType.MultipleAnswer) {
					// Toggle selection for multi-answer
					const updated = current.includes(answerId)
						? current.filter(id => id !== answerId)
						: [...current, answerId]
					return { ...prev, [questionId]: updated }
				}

				// Single selection for MC, TF
				return { ...prev, [questionId]: [answerId] }
			})
		},
		[]
	)

	const handleFillInBlank = useCallback((questionId: string, value: string) => {
		setAnswers(prev => ({
			...prev,
			[questionId]: value.trim() ? [value.trim()] : [],
		}))
	}, [])

	const handleSubmit = useCallback(async () => {
		if (!sessionId || !attemptId) return

		const submissionAnswers: StudentAnswerSubmission[] = questions.map(q => ({
			questionId: q.id,
			selectedAnswerIds: answers[q.id] || [],
		}))

		try {
			const result = await submitAttempt.mutateAsync({
				sessionId,
				attemptId,
				data: { answers: submissionAnswers },
			})
			setIsSubmitted(true)
			setSubmittedResult({
				score: result.score,
				totalPoints: result.totalPoints,
				scorePercentage: result.scorePercentage,
			})
			setShowSubmitDialog(false)
		} catch {
			// Error handled by hook
		}
	}, [sessionId, attemptId, questions, answers, submitAttempt])

	// ─── Loading ───

	if (loadingSession || loadingExam) {
		return (
			<div className='p-6 space-y-6 max-w-4xl mx-auto'>
				<Skeleton className='h-8 w-48' />
				<Skeleton className='h-4 w-96' />
				<Skeleton className='h-96 w-full' />
			</div>
		)
	}

	if (!session) {
		return (
			<div className='p-6 max-w-4xl mx-auto text-center py-12'>
				<p className='text-destructive'>Exam session not found</p>
				<Button
					variant='ghost'
					onClick={() => navigate('/app/exam-sessions')}
					className='mt-4'
				>
					<ArrowLeft className='h-4 w-4 mr-2' />
					{t('pages.exam_sessions.attempt_result.back_to_sessions')}
				</Button>
			</div>
		)
	}

	// ─── Result View (after submission) ───

	if (isSubmitted && submittedResult) {
		const percentage = submittedResult.scorePercentage
		return (
			<div className='p-6 space-y-6 max-w-2xl mx-auto'>
				<Card>
					<CardHeader className='text-center'>
						<div className='mx-auto mb-4'>
							{percentage >= 50 ? (
								<CheckCircle2 className='h-16 w-16 text-green-500' />
							) : (
								<XCircle className='h-16 w-16 text-red-500' />
							)}
						</div>
						<CardTitle className='text-2xl'>
							{t('pages.exam_sessions.attempt_result.title')}
						</CardTitle>
					</CardHeader>
					<CardContent className='space-y-4'>
						<div className='grid grid-cols-2 gap-4 text-center'>
							<div className='p-4 rounded-lg bg-muted'>
								<p className='text-sm text-muted-foreground'>
									{t('pages.exam_sessions.attempt_result.score')}
								</p>
								<p className='text-3xl font-bold'>
									{submittedResult.score.toFixed(1)}
								</p>
							</div>
							<div className='p-4 rounded-lg bg-muted'>
								<p className='text-sm text-muted-foreground'>
									{t('pages.exam_sessions.attempt_result.total_points')}
								</p>
								<p className='text-3xl font-bold'>
									{submittedResult.totalPoints.toFixed(1)}
								</p>
							</div>
						</div>
						<div className='text-center p-4 rounded-lg bg-muted'>
							<p className='text-sm text-muted-foreground'>
								{t('pages.exam_sessions.attempt_result.percentage')}
							</p>
							<p className='text-4xl font-bold'>{percentage.toFixed(1)}%</p>
						</div>
					</CardContent>
					<CardFooter className='justify-center'>
						<Button onClick={() => navigate('/app/exam-sessions')}>
							<ArrowLeft className='h-4 w-4 mr-2' />
							{t('pages.exam_sessions.attempt_result.back_to_sessions')}
						</Button>
					</CardFooter>
				</Card>
			</div>
		)
	}

	// ─── Pre-start View ───

	if (!attemptId) {
		return (
			<div className='p-6 space-y-6 max-w-2xl mx-auto'>
				<Button
					variant='ghost'
					onClick={() => navigate('/app/exam-sessions')}
					className='gap-2'
				>
					<ArrowLeft className='h-4 w-4' />
					{t('pages.exam_sessions.attempt_result.back_to_sessions')}
				</Button>

				<Card>
					<CardHeader>
						<CardTitle>{examData?.name || 'Exam'}</CardTitle>
						<CardDescription>{examData?.description || ''}</CardDescription>
					</CardHeader>
					<CardContent className='space-y-3'>
						<div className='flex items-center gap-2 text-sm text-muted-foreground'>
							<Clock className='h-4 w-4' />
							<span>
								{new Date(session.startTime).toLocaleString()} -{' '}
								{new Date(session.endTime).toLocaleString()}
							</span>
						</div>
						<div className='flex items-center gap-2 text-sm'>
							<span>{questions.length} questions</span>
							{session.isRetryable && (
								<Badge variant='outline'>
									{session.retryTimes} {t('pages.exam_sessions.table.retries')}
								</Badge>
							)}
							{session.allowPartialScoring && (
								<Badge variant='secondary'>
									{t('pages.exam_sessions.create.fields.partial_scoring')}
								</Badge>
							)}
						</div>
					</CardContent>
					<CardFooter>
						<Button
							onClick={handleStartAttempt}
							disabled={startAttempt.isPending || !session.isCurrentlyActive}
							className='w-full'
							size='lg'
						>
							{startAttempt.isPending
								? t('common.loading')
								: t('pages.exam_sessions.actions.start_exam')}
						</Button>
					</CardFooter>
				</Card>
			</div>
		)
	}

	// ─── Shared header (progress + pills + view toggle) ───

	const sharedHeader = (
		<div className='space-y-3'>
			<div className='flex items-center justify-between text-sm'>
				<span className='font-medium'>{examData?.name}</span>
				<span className='text-muted-foreground'>
					{t('pages.exam_sessions.take.answered')}: {answeredCount} /{' '}
					{questions.length}
				</span>
			</div>
			<Progress value={progress} className='h-2' />
			<div className='flex items-center justify-between'>
				{/* Question navigation pills */}
				<div className='flex flex-wrap gap-1'>
					{questions.map((q, idx) => (
						<Button
							key={q.id}
							variant={
								idx === currentQuestion && viewMode === 'paged'
									? 'default'
									: answers[q.id]?.length > 0
										? 'secondary'
										: 'outline'
							}
							size='sm'
							className='w-8 h-8 p-0 text-xs'
							onClick={() => {
								if (viewMode === 'paged') {
									setCurrentQuestion(idx)
								} else {
									document
										.getElementById(`question-${q.id}`)
										?.scrollIntoView({ behavior: 'smooth', block: 'center' })
								}
							}}
						>
							{idx + 1}
						</Button>
					))}
				</div>
				{/* View mode toggle */}
				<div className='flex items-center gap-1 ml-2 shrink-0'>
					<Button
						variant={viewMode === 'paged' ? 'default' : 'ghost'}
						size='sm'
						onClick={() => setViewMode('paged')}
						title='One question at a time'
					>
						<LayoutList className='h-4 w-4' />
					</Button>
					<Button
						variant={viewMode === 'full' ? 'default' : 'ghost'}
						size='sm'
						onClick={() => setViewMode('full')}
						title='All questions'
					>
						<List className='h-4 w-4' />
					</Button>
				</div>
			</div>
		</div>
	)

	// ─── Submit dialog (shared between both views) ───

	const submitDialog = (
		<AlertDialog open={showSubmitDialog} onOpenChange={setShowSubmitDialog}>
			<AlertDialogContent>
				<AlertDialogHeader>
					<AlertDialogTitle>
						{t('pages.exam_sessions.take.submit_confirm_title')}
					</AlertDialogTitle>
					<AlertDialogDescription asChild>
						<div>
							<p>{t('pages.exam_sessions.take.submit_confirm_description')}</p>
							{unansweredCount > 0 && (
								<div className='mt-3 rounded-md border border-amber-300 bg-amber-50 p-3 space-y-2'>
									<p className='text-amber-700 font-medium text-sm'>
										{t('pages.exam_sessions.take.submit_confirm_unanswered', {
											count: unansweredCount,
										})}
									</p>
									<div className='flex flex-wrap gap-1'>
										{unansweredQuestions.map(q => (
											<button
												key={q.id}
												className='inline-flex items-center justify-center w-8 h-8 rounded border border-amber-400 bg-white text-amber-700 text-xs font-medium hover:bg-amber-100 transition-colors'
												onClick={() => jumpToQuestion(q.id, q.index)}
											>
												{q.index + 1}
											</button>
										))}
									</div>
								</div>
							)}
						</div>
					</AlertDialogDescription>
				</AlertDialogHeader>
				<AlertDialogFooter>
					<AlertDialogCancel>{t('common.cancel')}</AlertDialogCancel>
					<AlertDialogAction
						onClick={handleSubmit}
						disabled={submitAttempt.isPending}
					>
						{submitAttempt.isPending
							? t('pages.exam_sessions.take.submitting')
							: t('pages.exam_sessions.take.submit_exam')}
					</AlertDialogAction>
				</AlertDialogFooter>
			</AlertDialogContent>
		</AlertDialog>
	)

	// ─── Full-page view (all questions at once) ───

	if (viewMode === 'full') {
		return (
			<div className='p-6 space-y-6 max-w-4xl mx-auto'>
				{sharedHeader}

				<div className='space-y-6'>
					{questions.map((q, idx) => (
						<Card
							key={q.id}
							id={`question-${q.id}`}
							className={
								highlightedQuestion === q.id ? 'ring-2 ring-amber-400' : ''
							}
						>
							<CardHeader>
								<div className='flex items-start justify-between gap-4'>
									<CardTitle className='text-base leading-relaxed'>
										<span className='text-muted-foreground mr-2'>
											{idx + 1}.
										</span>
										{q.content}
									</CardTitle>
									<Badge variant='outline' className='shrink-0'>
										{q.points} pts
									</Badge>
								</div>
								{q.imageUrl && (
									<img
										src={q.imageUrl}
										alt='Question'
										className='rounded-lg max-h-64 object-contain mt-2'
									/>
								)}
							</CardHeader>
							<CardContent>
								{(q.questionType === QuestionType.MultipleChoice ||
									q.questionType === QuestionType.TrueFalse) && (
									<RadioGroup
										value={answers[q.id]?.[0] || ''}
										onValueChange={value =>
											handleSelectAnswer(q.id, value, q.questionType)
										}
										className='space-y-3'
									>
										{q.answers.map(answer => (
											<div
												key={answer.id}
												className='flex items-center space-x-3 rounded-lg border p-3 hover:bg-muted/50 cursor-pointer'
											>
												<RadioGroupItem
													value={answer.id}
													id={`${q.id}-${answer.id}`}
												/>
												<Label
													htmlFor={`${q.id}-${answer.id}`}
													className='cursor-pointer flex-1'
												>
													{answer.content}
												</Label>
											</div>
										))}
									</RadioGroup>
								)}
								{q.questionType === QuestionType.MultipleAnswer && (
									<div className='space-y-3'>
										{q.answers.map(answer => (
											<div
												key={answer.id}
												className='flex items-center space-x-3 rounded-lg border p-3 hover:bg-muted/50 cursor-pointer'
												onClick={() =>
													handleSelectAnswer(q.id, answer.id, q.questionType)
												}
											>
												<Checkbox
													checked={answers[q.id]?.includes(answer.id) || false}
													onCheckedChange={() =>
														handleSelectAnswer(q.id, answer.id, q.questionType)
													}
												/>
												<Label className='cursor-pointer flex-1'>
													{answer.content}
												</Label>
											</div>
										))}
									</div>
								)}
								{q.questionType === QuestionType.FillInTheBlank && (
									<Input
										value={answers[q.id]?.[0] || ''}
										onChange={e => handleFillInBlank(q.id, e.target.value)}
										placeholder='Type your answer...'
									/>
								)}
							</CardContent>
						</Card>
					))}
				</div>

				<div className='flex justify-end pt-2'>
					<Button
						variant='destructive'
						onClick={() => setShowSubmitDialog(true)}
					>
						<Send className='h-4 w-4 mr-2' />
						{t('pages.exam_sessions.take.submit_exam')}
					</Button>
				</div>

				{submitDialog}
			</div>
		)
	}

	// ─── Paged view (one question at a time) ───

	const currentQ = questions[currentQuestion]
	if (!currentQ) return <></>

	return (
		<div
			className='p-6 space-y-4 max-w-4xl mx-auto'
			id={`question-${currentQ.id}`}
		>
			{sharedHeader}

			{/* Question Card */}
			<Card
				className={
					highlightedQuestion === currentQ.id ? 'ring-2 ring-amber-400' : ''
				}
			>
				<CardHeader>
					<div className='flex items-start justify-between'>
						<CardTitle className='text-lg'>{currentQ.content}</CardTitle>
						<Badge variant='outline'>{currentQ.points} pts</Badge>
					</div>
					{currentQ.imageUrl && (
						<img
							src={currentQ.imageUrl}
							alt='Question'
							className='rounded-lg max-h-64 object-contain mt-2'
						/>
					)}
				</CardHeader>
				<CardContent>
					{/* Multiple Choice / True-False */}
					{(currentQ.questionType === QuestionType.MultipleChoice ||
						currentQ.questionType === QuestionType.TrueFalse) && (
						<RadioGroup
							value={answers[currentQ.id]?.[0] || ''}
							onValueChange={value =>
								handleSelectAnswer(currentQ.id, value, currentQ.questionType)
							}
							className='space-y-3'
						>
							{currentQ.answers.map(answer => (
								<div
									key={answer.id}
									className='flex items-center space-x-3 rounded-lg border p-3 hover:bg-muted/50 cursor-pointer'
								>
									<RadioGroupItem value={answer.id} id={answer.id} />
									<Label htmlFor={answer.id} className='cursor-pointer flex-1'>
										{answer.content}
									</Label>
								</div>
							))}
						</RadioGroup>
					)}

					{/* Multiple Answer */}
					{currentQ.questionType === QuestionType.MultipleAnswer && (
						<div className='space-y-3'>
							{currentQ.answers.map(answer => (
								<div
									key={answer.id}
									className='flex items-center space-x-3 rounded-lg border p-3 hover:bg-muted/50 cursor-pointer'
									onClick={() =>
										handleSelectAnswer(
											currentQ.id,
											answer.id,
											currentQ.questionType
										)
									}
								>
									<Checkbox
										checked={answers[currentQ.id]?.includes(answer.id) || false}
										onCheckedChange={() =>
											handleSelectAnswer(
												currentQ.id,
												answer.id,
												currentQ.questionType
											)
										}
									/>
									<Label className='cursor-pointer flex-1'>
										{answer.content}
									</Label>
								</div>
							))}
						</div>
					)}

					{/* Fill in the Blank */}
					{currentQ.questionType === QuestionType.FillInTheBlank && (
						<div className='space-y-2'>
							<Input
								value={answers[currentQ.id]?.[0] || ''}
								onChange={e => handleFillInBlank(currentQ.id, e.target.value)}
								placeholder='Type your answer...'
							/>
						</div>
					)}
				</CardContent>
			</Card>

			{/* Navigation */}
			<div className='flex items-center justify-between'>
				<Button
					variant='outline'
					onClick={() => setCurrentQuestion(prev => prev - 1)}
					disabled={currentQuestion === 0}
				>
					<ArrowLeft className='h-4 w-4 mr-2' />
					{t('pages.exam_sessions.take.previous')}
				</Button>

				<Button variant='destructive' onClick={() => setShowSubmitDialog(true)}>
					<Send className='h-4 w-4 mr-2' />
					{t('pages.exam_sessions.take.submit_exam')}
				</Button>

				<Button
					variant='outline'
					onClick={() => setCurrentQuestion(prev => prev + 1)}
					disabled={currentQuestion === questions.length - 1}
				>
					{t('pages.exam_sessions.take.next')}
					<ArrowRight className='h-4 w-4 ml-2' />
				</Button>
			</div>

			{submitDialog}
		</div>
	)
}

export default ExamTakePage
