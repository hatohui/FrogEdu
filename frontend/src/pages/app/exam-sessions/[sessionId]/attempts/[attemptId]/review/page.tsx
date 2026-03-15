import React, { useState } from 'react'
import { useParams, useNavigate, Link } from 'react-router'
import { useTranslation } from 'react-i18next'
import { useAttemptReview } from '@/hooks/useExamSessions'
import { useExplainQuestion } from '@/hooks/useAIExplain'
import { useSocraticHints } from '@/hooks/useSocraticHints'
import { useEffectiveRole } from '@/hooks/useEffectiveRole'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Skeleton } from '@/components/ui/skeleton'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Separator } from '@/components/ui/separator'
import {
	ArrowLeft,
	CheckCircle2,
	XCircle,
	AlertCircle,
	Sparkles,
	BookOpen,
	Trophy,
	RotateCcw,
	FileText,
	Lightbulb,
} from 'lucide-react'
import { AttemptStatus } from '@/types/model/class-service'
import type { QuestionReview } from '@/types/model/class-service'
import { format } from 'date-fns'
import { toast } from 'sonner'

// ─── Question Review Card ───

interface QuestionCardProps {
	question: QuestionReview
	index: number
	grade: number
	subject: string
	isTeacher?: boolean
}

const QuestionCard = ({
	question,
	index,
	grade,
	subject,
	isTeacher,
}: QuestionCardProps) => {
	const { t } = useTranslation()
	const explainMutation = useExplainQuestion()
	const socraticMutation = useSocraticHints()
	const [aiExplanation, setAiExplanation] = useState<string | null>(null)
	const [socraticHints, setSocraticHints] = useState<string[] | null>(null)
	const [teachingNote, setTeachingNote] = useState<string | null>(null)

	const isEssay = question.type?.toLowerCase() === 'essay'

	const correctAnswers = question.answers.filter(a => a.isCorrect)
	const correctAnswerText = correctAnswers.map(a => a.content).join(', ')

	const handleAIExplain = async () => {
		try {
			const explanation = await explainMutation.mutateAsync({
				questionContent: question.content,
				correctAnswer: correctAnswerText,
				grade,
				subject,
				studentAnswer: question.answers
					.filter(a => a.wasSelectedByStudent)
					.map(a => a.content)
					.join(', '),
				language: 'vi',
			})
			setAiExplanation(explanation)
		} catch {
			toast.error(t('pages.exam_sessions.review.ai_explanation_error'))
		}
	}

	const handleSocraticHints = async () => {
		try {
			const studentAnswerText = question.answers
				.filter(a => a.wasSelectedByStudent)
				.map(a => a.content)
				.join(', ')
			const result = await socraticMutation.mutateAsync({
				questionContent: question.content,
				studentAnswer: studentAnswerText || '(no answer)',
				correctAnswer: correctAnswerText,
				subject,
				grade,
				language: 'vi',
			})
			setSocraticHints(result.hints)
			setTeachingNote(result.teachingNote)
		} catch {
			toast.error(t('pages.exam_sessions.review.socratic_error'))
		}
	}

	const statusBadge = question.isCorrect ? (
		<Badge className='bg-green-100 text-green-800 border-green-200'>
			<CheckCircle2 className='h-3 w-3 mr-1' />
			{t('pages.exam_sessions.review.correct_badge')}
		</Badge>
	) : question.isPartiallyCorrect ? (
		<Badge className='bg-yellow-100 text-yellow-800 border-yellow-200'>
			<AlertCircle className='h-3 w-3 mr-1' />
			{t('pages.exam_sessions.review.partial_badge')}
		</Badge>
	) : (
		<Badge className='bg-red-100 text-red-800 border-red-200'>
			<XCircle className='h-3 w-3 mr-1' />
			{t('pages.exam_sessions.review.incorrect_badge')}
		</Badge>
	)

	// ─── Essay question card ───
	if (isEssay) {
		return (
			<Card
				className={`border-l-4 ${
					question.isCorrect
						? 'border-l-green-500'
						: question.studentScore > 0
							? 'border-l-yellow-500'
							: 'border-l-red-500'
				}`}
			>
				<CardHeader className='pb-3'>
					<div className='flex items-start justify-between gap-3'>
						<div className='flex-1'>
							<p className='text-xs text-muted-foreground mb-1 flex items-center gap-1'>
								<FileText className='h-3 w-3' />
								{t('pages.exam_sessions.review.essay_type_label')}
							</p>
							<p className='font-medium leading-relaxed'>{question.content}</p>
						</div>
						<div className='flex items-center gap-2 shrink-0'>
							{statusBadge}
							<span className='text-xs text-muted-foreground'>
								{question.studentScore.toFixed(1)} / {question.point.toFixed(1)}{' '}
								pts
							</span>
						</div>
					</div>
				</CardHeader>
				<CardContent className='space-y-4'>
					{/* Student's written answer */}
					<div>
						<p className='text-xs font-semibold text-muted-foreground mb-1'>
							{t('pages.exam_sessions.review.essay_your_answer')}
						</p>
						{question.essayStudentText ? (
							<div className='bg-muted/40 rounded-md p-3 text-sm whitespace-pre-wrap leading-relaxed'>
								{question.essayStudentText}
							</div>
						) : (
							<p className='text-sm text-muted-foreground italic'>
								{t('pages.exam_sessions.review.essay_no_answer')}
							</p>
						)}
					</div>

					{/* Grading rubric */}
					{correctAnswers.length > 0 && (
						<div>
							<p className='text-xs font-semibold text-muted-foreground mb-1'>
								{t('pages.exam_sessions.review.essay_rubric_label')}
							</p>
							<div className='bg-blue-50 border border-blue-200 rounded-md p-3 text-sm text-blue-900'>
								{correctAnswerText}
							</div>
						</div>
					)}

					{/* AI Feedback */}
					{question.essayAiFeedback ? (
						<div className='bg-purple-50 border border-purple-200 rounded-md p-3'>
							<p className='text-xs font-semibold text-purple-800 mb-1 flex items-center gap-1'>
								<Sparkles className='h-3 w-3' />
								{t('pages.exam_sessions.review.essay_ai_feedback_title')}
							</p>
							<p className='text-sm text-purple-900 leading-relaxed'>
								{question.essayAiFeedback}
							</p>
						</div>
					) : null}
				</CardContent>
			</Card>
		)
	}

	return (
		<Card
			className={`border-l-4 ${
				question.isCorrect
					? 'border-l-green-500'
					: question.isPartiallyCorrect
						? 'border-l-yellow-500'
						: 'border-l-red-500'
			}`}
		>
			<CardHeader className='pb-3'>
				<div className='flex items-start justify-between gap-3'>
					<div className='flex-1'>
						<p className='text-xs text-muted-foreground mb-1'>
							{t('pages.exam_sessions.review.question_of', {
								current: index + 1,
								total: '?',
							}).replace('/ ?', '')}
						</p>
						<p className='font-medium leading-relaxed'>{question.content}</p>
					</div>
					<div className='flex items-center gap-2 shrink-0'>
						{statusBadge}
						<span className='text-xs text-muted-foreground'>
							{question.studentScore.toFixed(1)} / {question.point.toFixed(1)}{' '}
							pts
						</span>
					</div>
				</div>
			</CardHeader>
			<CardContent className='space-y-3'>
				{/* Answer options */}
				<div className='space-y-2'>
					{question.answers.map(answer => (
						<div
							key={answer.id}
							className={`flex items-start gap-2 p-2 rounded-md text-sm ${
								answer.isCorrect
									? 'bg-green-50 border border-green-200 text-green-900'
									: answer.wasSelectedByStudent && !answer.isCorrect
										? 'bg-red-50 border border-red-200 text-red-900'
										: 'bg-muted/40 border border-transparent text-muted-foreground'
							}`}
						>
							<span className='mt-0.5'>
								{answer.isCorrect ? (
									<CheckCircle2 className='h-4 w-4 text-green-600' />
								) : answer.wasSelectedByStudent ? (
									<XCircle className='h-4 w-4 text-red-500' />
								) : (
									<span className='h-4 w-4 inline-block' />
								)}
							</span>
							<div className='flex-1'>
								<p>{answer.content}</p>
								{answer.explanation && (
									<p className='text-xs mt-1 opacity-70 italic'>
										{answer.explanation}
									</p>
								)}
							</div>
							{answer.wasSelectedByStudent && (
								<Badge variant='outline' className='text-xs shrink-0'>
									{t('pages.exam_sessions.review.your_answer')}
								</Badge>
							)}
						</div>
					))}
				</div>

				{/* AI Explanation */}
				{!question.isCorrect && (
					<>
						<Separator />
						{aiExplanation ? (
							<div className='bg-blue-50 border border-blue-200 rounded-md p-3'>
								<p className='text-xs font-semibold text-blue-800 mb-1 flex items-center gap-1'>
									<Sparkles className='h-3 w-3' />
									{t('pages.exam_sessions.review.ai_explanation_title')}
								</p>
								<p className='text-sm text-blue-900 leading-relaxed'>
									{aiExplanation}
								</p>
							</div>
						) : (
							<Button
								variant='outline'
								size='sm'
								onClick={handleAIExplain}
								disabled={explainMutation.isPending}
								className='gap-2 text-blue-700 border-blue-300 hover:bg-blue-50'
							>
								<Sparkles className='h-4 w-4' />
								{explainMutation.isPending
									? t('pages.exam_sessions.review.ai_explaining')
									: t('pages.exam_sessions.review.ai_explain_button')}
							</Button>
						)}
					</>
				)}

				{/* Socratic Hints (Teacher only) */}
				{isTeacher && !question.isCorrect && (
					<>
						<Separator />
						{socraticHints ? (
							<div className='bg-amber-50 border border-amber-200 rounded-md p-3 space-y-2'>
								<p className='text-xs font-semibold text-amber-800 flex items-center gap-1'>
									<Lightbulb className='h-3 w-3' />
									{t('pages.exam_sessions.review.socratic_title')}
								</p>
								<ol className='list-decimal list-inside space-y-1'>
									{socraticHints.map((hint, i) => (
										<li
											key={i}
											className='text-sm text-amber-900 leading-relaxed'
										>
											{hint}
										</li>
									))}
								</ol>
								{teachingNote && (
									<p className='text-xs text-amber-700 italic mt-2 border-t border-amber-200 pt-2'>
										{t('pages.exam_sessions.review.teaching_note')}:{' '}
										{teachingNote}
									</p>
								)}
							</div>
						) : (
							<Button
								variant='outline'
								size='sm'
								onClick={handleSocraticHints}
								disabled={socraticMutation.isPending}
								className='gap-2 text-amber-700 border-amber-300 hover:bg-amber-50'
							>
								<Lightbulb className='h-4 w-4' />
								{socraticMutation.isPending
									? t('pages.exam_sessions.review.socratic_loading')
									: t('pages.exam_sessions.review.socratic_button')}
							</Button>
						)}
					</>
				)}
			</CardContent>
		</Card>
	)
}

// ─── Main Page ───

const AttemptReviewPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const { sessionId, attemptId } = useParams<{
		sessionId: string
		attemptId: string
	}>()
	const navigate = useNavigate()

	const { isTeacher, isAdmin } = useEffectiveRole()
	const { data: review, isLoading } = useAttemptReview(attemptId || '')

	if (isLoading) {
		return (
			<div className='p-6 space-y-6 max-w-4xl mx-auto'>
				<Skeleton className='h-8 w-48' />
				<div className='grid gap-4 md:grid-cols-3'>
					{[1, 2, 3].map(i => (
						<Skeleton key={i} className='h-24' />
					))}
				</div>
				{[1, 2, 3].map(i => (
					<Skeleton key={i} className='h-48' />
				))}
			</div>
		)
	}

	if (!review) {
		return (
			<div className='p-6 max-w-4xl mx-auto text-center py-12'>
				<p className='text-destructive'>
					{t('pages.exam_sessions.review.not_found')}
				</p>
				<Button variant='ghost' onClick={() => navigate(-1)} className='mt-4'>
					<ArrowLeft className='h-4 w-4 mr-2' />
					{t('common.back')}
				</Button>
			</div>
		)
	}

	const correctCount = review.questions.filter(q => q.isCorrect).length
	const incorrectCount = review.questions.filter(
		q => !q.isCorrect && !q.isPartiallyCorrect
	).length
	const partialCount = review.questions.filter(q => q.isPartiallyCorrect).length

	const isRetryable =
		review.status === AttemptStatus.Submitted ||
		review.status === AttemptStatus.Graded

	return (
		<div className='p-6 space-y-6 max-w-4xl mx-auto'>
			{/* Back */}
			<Button
				variant='ghost'
				onClick={() => navigate(`/app/exam-sessions/${sessionId}/my-results`)}
				className='-ml-2 gap-2'
			>
				<ArrowLeft className='h-4 w-4' />
				{t('pages.exam_sessions.review.back_to_results')}
			</Button>

			{/* Header */}
			<div>
				<h1 className='text-2xl font-bold tracking-tight flex items-center gap-2'>
					<BookOpen className='h-6 w-6 text-primary' />
					{t('pages.exam_sessions.review.title')}
				</h1>
				<p className='text-muted-foreground mt-1'>
					{review.examName} —{' '}
					{review.submittedAt
						? format(new Date(review.submittedAt), 'PPpp')
						: ''}
				</p>
			</div>

			{/* Score summary */}
			<div className='grid gap-4 md:grid-cols-4'>
				<Card className='md:col-span-1'>
					<CardHeader className='pb-2'>
						<CardTitle className='text-sm text-muted-foreground flex items-center gap-1'>
							<Trophy className='h-4 w-4 text-yellow-500' />
							{t('pages.exam_sessions.review.score_card.your_score')}
						</CardTitle>
					</CardHeader>
					<CardContent>
						<p className='text-3xl font-bold'>
							{t('pages.exam_sessions.review.score_card.percentage', {
								value: review.scorePercentage.toFixed(1),
							})}
						</p>
						<p className='text-xs text-muted-foreground mt-1'>
							{t('pages.exam_sessions.review.score_card.points', {
								score: review.score.toFixed(1),
								total: review.totalPoints.toFixed(1),
							})}
						</p>
					</CardContent>
				</Card>
				<Card>
					<CardHeader className='pb-2'>
						<CardTitle className='text-sm text-muted-foreground flex items-center gap-1'>
							<CheckCircle2 className='h-4 w-4 text-green-500' />
							{t('pages.exam_sessions.attempt_result.correct')}
						</CardTitle>
					</CardHeader>
					<CardContent>
						<p className='text-3xl font-bold text-green-600'>{correctCount}</p>
					</CardContent>
				</Card>
				<Card>
					<CardHeader className='pb-2'>
						<CardTitle className='text-sm text-muted-foreground flex items-center gap-1'>
							<AlertCircle className='h-4 w-4 text-yellow-500' />
							{t('pages.exam_sessions.attempt_result.partial')}
						</CardTitle>
					</CardHeader>
					<CardContent>
						<p className='text-3xl font-bold text-yellow-600'>{partialCount}</p>
					</CardContent>
				</Card>
				<Card>
					<CardHeader className='pb-2'>
						<CardTitle className='text-sm text-muted-foreground flex items-center gap-1'>
							<XCircle className='h-4 w-4 text-red-500' />
							{t('pages.exam_sessions.attempt_result.incorrect')}
						</CardTitle>
					</CardHeader>
					<CardContent>
						<p className='text-3xl font-bold text-red-600'>{incorrectCount}</p>
					</CardContent>
				</Card>
			</div>

			{/* Retake button if allowed */}
			{isRetryable && (
				<div className='flex justify-end'>
					<Link to={`/app/exam-sessions/${sessionId}/take`}>
						<Button variant='outline' className='gap-2'>
							<RotateCcw className='h-4 w-4' />
							{t('pages.exam_sessions.review.retake_button')}
						</Button>
					</Link>
				</div>
			)}

			{/* Questions */}
			{review.questions.length === 0 ? (
				<Card>
					<CardContent className='py-12 text-center text-muted-foreground'>
						{t('pages.exam_sessions.review.empty_answers')}
					</CardContent>
				</Card>
			) : (
				<div className='space-y-4'>
					{review.questions.map((question, idx) => (
						<QuestionCard
							key={question.questionId}
							question={question}
							index={idx}
							grade={3}
							subject='General'
							isTeacher={isTeacher || isAdmin}
						/>
					))}
				</div>
			)}
		</div>
	)
}

export default AttemptReviewPage
