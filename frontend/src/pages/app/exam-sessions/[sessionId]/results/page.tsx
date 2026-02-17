import React from 'react'
import { useParams, useNavigate } from 'react-router'
import { useTranslation } from 'react-i18next'
import {
	useExamSessionDetail,
	useSessionResults,
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
	BarChart3,
	TrendingDown,
	TrendingUp,
	Users,
} from 'lucide-react'
import { AttemptStatus } from '@/types/model/class-service'

const SessionResultsPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const { sessionId } = useParams<{ sessionId: string }>()
	const navigate = useNavigate()

	const { data: session, isLoading: loadingSession } = useExamSessionDetail(
		sessionId || ''
	)
	const { data: results, isLoading: loadingResults } = useSessionResults(
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

	if (loadingSession || loadingResults) {
		return (
			<div className='p-6 space-y-6 max-w-7xl mx-auto'>
				<Skeleton className='h-8 w-48' />
				<div className='grid gap-4 md:grid-cols-4'>
					{[1, 2, 3, 4].map(i => (
						<Skeleton key={i} className='h-24' />
					))}
				</div>
				<Skeleton className='h-96' />
			</div>
		)
	}

	if (!session || !results) {
		return (
			<div className='p-6 max-w-7xl mx-auto text-center py-12'>
				<p className='text-destructive'>Session not found</p>
				<Button variant='ghost' onClick={() => navigate(-1)} className='mt-4'>
					<ArrowLeft className='h-4 w-4 mr-2' />
					{t('common.back')}
				</Button>
			</div>
		)
	}

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Back */}
			<Button variant='ghost' onClick={() => navigate(-1)} className='gap-2'>
				<ArrowLeft className='h-4 w-4' />
				{t('common.back')}
			</Button>

			{/* Header */}
			<div>
				<h1 className='text-3xl font-bold tracking-tight flex items-center gap-2'>
					<BarChart3 className='h-8 w-8' />
					{t('pages.exam_sessions.results.title')}
				</h1>
				<p className='text-muted-foreground mt-1'>
					{t('pages.exam_sessions.results.subtitle')}
				</p>
			</div>

			{/* Summary Cards */}
			<div className='grid gap-4 md:grid-cols-4'>
				<Card>
					<CardHeader className='flex flex-row items-center justify-between pb-2'>
						<CardTitle className='text-sm font-medium'>
							{t('pages.exam_sessions.results.summary.total_attempts')}
						</CardTitle>
						<Users className='h-4 w-4 text-muted-foreground' />
					</CardHeader>
					<CardContent>
						<div className='text-2xl font-bold'>{results.totalAttempts}</div>
					</CardContent>
				</Card>
				<Card>
					<CardHeader className='flex flex-row items-center justify-between pb-2'>
						<CardTitle className='text-sm font-medium'>
							{t('pages.exam_sessions.results.summary.average_score')}
						</CardTitle>
						<BarChart3 className='h-4 w-4 text-muted-foreground' />
					</CardHeader>
					<CardContent>
						<div className='text-2xl font-bold'>
							{results.averagePercentage.toFixed(1)}%
						</div>
						<p className='text-xs text-muted-foreground'>
							{results.averageScore.toFixed(1)} pts
						</p>
					</CardContent>
				</Card>
				<Card>
					<CardHeader className='flex flex-row items-center justify-between pb-2'>
						<CardTitle className='text-sm font-medium'>
							{t('pages.exam_sessions.results.summary.highest_score')}
						</CardTitle>
						<TrendingUp className='h-4 w-4 text-green-500' />
					</CardHeader>
					<CardContent>
						<div className='text-2xl font-bold text-green-600'>
							{results.highestScore.toFixed(1)}
						</div>
					</CardContent>
				</Card>
				<Card>
					<CardHeader className='flex flex-row items-center justify-between pb-2'>
						<CardTitle className='text-sm font-medium'>
							{t('pages.exam_sessions.results.summary.lowest_score')}
						</CardTitle>
						<TrendingDown className='h-4 w-4 text-red-500' />
					</CardHeader>
					<CardContent>
						<div className='text-2xl font-bold text-red-600'>
							{results.lowestScore.toFixed(1)}
						</div>
					</CardContent>
				</Card>
			</div>

			{/* Attempts Table */}
			<Card>
				<CardHeader>
					<CardTitle>{t('pages.exam_sessions.results.title')}</CardTitle>
					<CardDescription>
						{t('pages.exam_sessions.results.subtitle')}
					</CardDescription>
				</CardHeader>
				<CardContent>
					{results.attempts.length > 0 ? (
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead>
										{t('pages.exam_sessions.results.table.student')}
									</TableHead>
									<TableHead>
										{t('pages.exam_sessions.results.table.score')}
									</TableHead>
									<TableHead>
										{t('pages.exam_sessions.results.table.percentage')}
									</TableHead>
									<TableHead>
										{t('pages.exam_sessions.results.table.attempt_number')}
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
								{results.attempts.map(attempt => (
									<TableRow key={attempt.attemptId}>
										<TableCell className='font-medium'>
											{attempt.studentName}
										</TableCell>
										<TableCell>
											{attempt.score.toFixed(1)} /{' '}
											{attempt.totalPoints.toFixed(1)}
										</TableCell>
										<TableCell>
											<Badge
												variant={
													attempt.scorePercentage >= 80
														? 'default'
														: attempt.scorePercentage >= 50
															? 'secondary'
															: 'destructive'
												}
											>
												{attempt.scorePercentage.toFixed(1)}%
											</Badge>
										</TableCell>
										<TableCell>#{attempt.attemptNumber}</TableCell>
										<TableCell>{getStatusBadge(attempt.status)}</TableCell>
										<TableCell>
											{attempt.submittedAt
												? new Date(attempt.submittedAt).toLocaleString()
												: '-'}
										</TableCell>
										<TableCell className='text-right'>
											<Button
												variant='ghost'
												size='sm'
												onClick={() =>
													navigate(
														`/app/exam-sessions/attempts/${attempt.attemptId}`
													)
												}
											>
												{t('pages.exam_sessions.results.view_detail')}
											</Button>
										</TableCell>
									</TableRow>
								))}
							</TableBody>
						</Table>
					) : (
						<div className='text-center py-12'>
							<Users className='h-12 w-12 mx-auto text-muted-foreground mb-4 opacity-50' />
							<p className='text-muted-foreground'>
								{t('pages.exam_sessions.results.empty')}
							</p>
						</div>
					)}
				</CardContent>
			</Card>
		</div>
	)
}

export default SessionResultsPage
