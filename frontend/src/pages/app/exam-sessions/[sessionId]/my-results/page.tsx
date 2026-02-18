import React from 'react'
import { useParams, useNavigate } from 'react-router'
import { useTranslation } from 'react-i18next'
import {
	useExamSessionDetail,
	useMySessionAttempts,
} from '@/hooks/useExamSessions'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Skeleton } from '@/components/ui/skeleton'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import {
	ArrowLeft,
	Trophy,
	Target,
	RotateCcw,
	CheckCircle2,
} from 'lucide-react'
import { format } from 'date-fns'
import { AttemptStatus } from '@/types/model/class-service'

const MySessionResultsPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const { sessionId } = useParams<{ sessionId: string }>()
	const navigate = useNavigate()

	const { data: session, isLoading: loadingSession } = useExamSessionDetail(
		sessionId || ''
	)
	const { data: attempts, isLoading: loadingAttempts } = useMySessionAttempts(
		sessionId || ''
	)

	const getStatusBadge = (status: string) => {
		switch (status) {
			case AttemptStatus.Graded:
				return (
					<Badge variant='default'>
						{t('pages.exam_sessions.attempt_status.graded')}
					</Badge>
				)
			case AttemptStatus.Submitted:
				return (
					<Badge variant='secondary'>
						{t('pages.exam_sessions.attempt_status.submitted')}
					</Badge>
				)
			case AttemptStatus.InProgress:
				return (
					<Badge variant='outline'>
						{t('pages.exam_sessions.attempt_status.in_progress')}
					</Badge>
				)
			case AttemptStatus.TimedOut:
				return (
					<Badge variant='destructive'>
						{t('pages.exam_sessions.attempt_status.timed_out')}
					</Badge>
				)
			default:
				return <Badge variant='outline'>{status}</Badge>
		}
	}

	if (loadingSession || loadingAttempts) {
		return (
			<div className='p-6 space-y-6 max-w-4xl mx-auto'>
				<Skeleton className='h-8 w-48' />
				<div className='grid gap-4 md:grid-cols-3'>
					{[1, 2, 3].map(i => (
						<Skeleton key={i} className='h-24' />
					))}
				</div>
				<Skeleton className='h-64' />
			</div>
		)
	}

	if (!session) {
		return (
			<div className='p-6 max-w-4xl mx-auto text-center py-12'>
				<p className='text-destructive'>
					{t('pages.exam_sessions.my_results.not_found')}
				</p>
				<Button
					variant='ghost'
					onClick={() => navigate('/app/exam-sessions')}
					className='mt-4'
				>
					<ArrowLeft className='h-4 w-4 mr-2' />
					{t('common.back')}
				</Button>
			</div>
		)
	}

	const submittedAttempts =
		attempts?.filter(a => a.status !== AttemptStatus.InProgress) || []
	const bestAttempt = submittedAttempts.reduce<
		(typeof submittedAttempts)[number] | null
	>(
		(best, a) =>
			best === null || a.scorePercentage > best.scorePercentage ? a : best,
		null
	)

	return (
		<div className='p-6 space-y-6 max-w-4xl mx-auto'>
			{/* Back */}
			<Button
				variant='ghost'
				onClick={() => navigate('/app/exam-sessions')}
				className='-ml-2'
			>
				<ArrowLeft className='h-4 w-4 mr-2' />
				{t('common.back')}
			</Button>

			{/* Header */}
			<div>
				<h1 className='text-2xl font-bold tracking-tight flex items-center gap-2'>
					<Trophy className='h-6 w-6 text-yellow-500' />
					{t('pages.exam_sessions.my_results.title')}
				</h1>
				<p className='text-muted-foreground mt-1'>
					{t('pages.exam_sessions.my_results.subtitle')}
				</p>
			</div>

			{/* Summary stats */}
			<div className='grid gap-4 md:grid-cols-3'>
				<Card>
					<CardHeader className='pb-2'>
						<CardDescription className='flex items-center gap-1'>
							<RotateCcw className='h-4 w-4' />
							{t('pages.exam_sessions.my_results.total_attempts')}
						</CardDescription>
					</CardHeader>
					<CardContent>
						<p className='text-3xl font-bold'>{submittedAttempts.length}</p>
					</CardContent>
				</Card>

				<Card>
					<CardHeader className='pb-2'>
						<CardDescription className='flex items-center gap-1'>
							<Target className='h-4 w-4' />
							{t('pages.exam_sessions.my_results.best_score')}
						</CardDescription>
					</CardHeader>
					<CardContent>
						<p className='text-3xl font-bold text-green-600'>
							{bestAttempt !== null
								? `${bestAttempt.scorePercentage.toFixed(1)}%`
								: '—'}
						</p>
					</CardContent>
				</Card>

				<Card>
					<CardHeader className='pb-2'>
						<CardDescription className='flex items-center gap-1'>
							<CheckCircle2 className='h-4 w-4' />
							{t('pages.exam_sessions.my_results.retries_left')}
						</CardDescription>
					</CardHeader>
					<CardContent>
						<p className='text-3xl font-bold'>
							{session.isRetryable
								? Math.max(0, session.retryTimes - (attempts?.length || 0))
								: '—'}
						</p>
					</CardContent>
				</Card>
			</div>

			{/* Attempts table */}
			<Card>
				<CardHeader>
					<CardTitle>
						{t('pages.exam_sessions.my_results.attempts_title')}
					</CardTitle>
				</CardHeader>
				<CardContent>
					{submittedAttempts.length === 0 ? (
						<div className='text-center py-8 text-muted-foreground'>
							{t('pages.exam_sessions.my_results.no_attempts')}
						</div>
					) : (
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead>
										{t('pages.exam_sessions.results.table.attempt_number')}
									</TableHead>
									<TableHead>
										{t('pages.exam_sessions.results.table.score')}
									</TableHead>
									<TableHead>
										{t('pages.exam_sessions.results.table.percentage')}
									</TableHead>
									<TableHead>
										{t('pages.exam_sessions.results.table.status')}
									</TableHead>
									<TableHead>
										{t('pages.exam_sessions.results.table.submitted_at')}
									</TableHead>
									<TableHead className='text-right'>
										{t('pages.exam_sessions.results.table.actions')}
									</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{submittedAttempts.map(attempt => (
									<TableRow key={attempt.id}>
										<TableCell className='font-medium'>
											# {attempt.attemptNumber}
										</TableCell>
										<TableCell>
											{attempt.score} / {attempt.totalPoints}
										</TableCell>
										<TableCell>
											<span
												className={
													attempt.scorePercentage >= 80
														? 'text-green-600 font-medium'
														: attempt.scorePercentage >= 50
															? 'text-yellow-600 font-medium'
															: 'text-red-600 font-medium'
												}
											>
												{attempt.scorePercentage.toFixed(1)}%
											</span>
										</TableCell>
										<TableCell>{getStatusBadge(attempt.status)}</TableCell>
										<TableCell>
											{attempt.submittedAt
												? format(new Date(attempt.submittedAt), 'PPp')
												: '—'}
										</TableCell>
										<TableCell className='text-right'>
											<Button
												size='sm'
												variant='outline'
												onClick={() =>
													navigate(`/app/exam-sessions/attempts/${attempt.id}`)
												}
											>
												{t('pages.exam_sessions.results.view_detail')}
											</Button>
										</TableCell>
									</TableRow>
								))}
							</TableBody>
						</Table>
					)}
				</CardContent>
			</Card>
		</div>
	)
}

export default MySessionResultsPage
