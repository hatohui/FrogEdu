import React from 'react'
import { useNavigate } from 'react-router'
import { useTranslation } from 'react-i18next'
import { useStudentExamSessions } from '@/hooks/useExamSessions'
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
import { History, Clock, TrendingUp, Eye, Lock } from 'lucide-react'
import { format } from 'date-fns'

const ExamHistoryPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	// Fetch ALL sessions (not just upcoming)
	const { data: sessions, isLoading } = useStudentExamSessions(false)

	const getSessionStatusBadge = (
		session: NonNullable<typeof sessions>[number]
	) => {
		if (session.isCurrentlyActive) {
			return (
				<Badge className='bg-green-100 text-green-800 border-green-200'>
					{t('pages.exam_sessions.history.status.active')}
				</Badge>
			)
		}
		if (session.isUpcoming) {
			return (
				<Badge variant='secondary'>
					{t('pages.exam_sessions.history.status.upcoming')}
				</Badge>
			)
		}
		return (
			<Badge variant='outline' className='text-muted-foreground'>
				{t('pages.exam_sessions.history.status.ended')}
			</Badge>
		)
	}

	if (isLoading) {
		return (
			<div className='p-6 space-y-6 max-w-6xl mx-auto'>
				<Skeleton className='h-8 w-48' />
				<div className='grid gap-4 md:grid-cols-3'>
					{[1, 2, 3].map(i => (
						<Skeleton key={i} className='h-24' />
					))}
				</div>
				<Skeleton className='h-96' />
			</div>
		)
	}

	const endedSessions = sessions?.filter(s => s.hasEnded) || []
	const activeSessions = sessions?.filter(s => s.isCurrentlyActive) || []
	const upcomingSessions = sessions?.filter(s => s.isUpcoming) || []

	return (
		<div className='p-6 space-y-6 max-w-6xl mx-auto'>
			{/* Header */}
			<div>
				<h1 className='text-2xl font-bold tracking-tight flex items-center gap-2'>
					<History className='h-6 w-6 text-primary' />
					{t('pages.exam_sessions.history.title')}
				</h1>
				<p className='text-muted-foreground mt-1'>
					{t('pages.exam_sessions.history.subtitle')}
				</p>
			</div>

			{/* Summary */}
			<div className='grid gap-4 md:grid-cols-3'>
				<Card>
					<CardHeader className='pb-2'>
						<CardDescription className='flex items-center gap-1'>
							<TrendingUp className='h-4 w-4 text-green-500' />
							{t('pages.exam_sessions.history.stats.active')}
						</CardDescription>
					</CardHeader>
					<CardContent>
						<p className='text-3xl font-bold text-green-600'>
							{activeSessions.length}
						</p>
					</CardContent>
				</Card>
				<Card>
					<CardHeader className='pb-2'>
						<CardDescription className='flex items-center gap-1'>
							<Clock className='h-4 w-4 text-blue-500' />
							{t('pages.exam_sessions.history.stats.upcoming')}
						</CardDescription>
					</CardHeader>
					<CardContent>
						<p className='text-3xl font-bold text-blue-600'>
							{upcomingSessions.length}
						</p>
					</CardContent>
				</Card>
				<Card>
					<CardHeader className='pb-2'>
						<CardDescription className='flex items-center gap-1'>
							<History className='h-4 w-4 text-muted-foreground' />
							{t('pages.exam_sessions.history.stats.completed')}
						</CardDescription>
					</CardHeader>
					<CardContent>
						<p className='text-3xl font-bold'>{endedSessions.length}</p>
					</CardContent>
				</Card>
			</div>

			{/* Sessions Table */}
			<Card>
				<CardHeader>
					<CardTitle>{t('pages.exam_sessions.history.all_exams')}</CardTitle>
					<CardDescription>
						{t('pages.exam_sessions.history.all_exams_description')}
					</CardDescription>
				</CardHeader>
				<CardContent>
					{!sessions || sessions.length === 0 ? (
						<div className='text-center py-12'>
							<History className='h-12 w-12 mx-auto text-muted-foreground mb-4 opacity-40' />
							<p className='text-muted-foreground'>
								{t('pages.exam_sessions.history.empty')}
							</p>
						</div>
					) : (
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead>
										{t('pages.exam_sessions.history.table.exam_id')}
									</TableHead>
									<TableHead>
										{t('pages.exam_sessions.history.table.start_time')}
									</TableHead>
									<TableHead>
										{t('pages.exam_sessions.history.table.end_time')}
									</TableHead>
									<TableHead>
										{t('pages.exam_sessions.history.table.attempts')}
									</TableHead>
									<TableHead>
										{t('pages.exam_sessions.history.table.status')}
									</TableHead>
									<TableHead className='text-right'>
										{t('pages.exam_sessions.results.table.actions')}
									</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{sessions.map(session => (
									<TableRow key={session.id}>
										<TableCell className='font-medium font-mono text-xs'>
											{session.examId.slice(0, 8)}…
										</TableCell>
										<TableCell>
											{format(new Date(session.startTime), 'PPp')}
										</TableCell>
										<TableCell>
											{format(new Date(session.endTime), 'PPp')}
										</TableCell>
										<TableCell>{session.attemptCount}</TableCell>
										<TableCell>{getSessionStatusBadge(session)}</TableCell>
										<TableCell className='text-right'>
											<div className='flex items-center justify-end gap-2'>
												{session.isCurrentlyActive &&
													session.attemptCount <= session.retryTimes && (
														<Button
															size='sm'
															onClick={() =>
																navigate(
																	`/app/exam-sessions/${session.id}/take`
																)
															}
														>
															{t(
																'pages.exam_sessions.history.actions.take_exam'
															)}
														</Button>
													)}
												{session.isCurrentlyActive &&
													session.attemptCount > session.retryTimes && (
														<Badge
															variant='outline'
															className='gap-1 text-muted-foreground'
														>
															<Lock className='h-3 w-3' />
															{t(
																'pages.exam_sessions.history.actions.no_attempts_left'
															)}
														</Badge>
													)}
												{session.attemptCount > 0 && (
													<Button
														size='sm'
														variant='outline'
														className='gap-1'
														onClick={() =>
															navigate(
																`/app/exam-sessions/${session.id}/my-results`
															)
														}
													>
														<Eye className='h-4 w-4' />
														{t(
															'pages.exam_sessions.history.actions.review_answers'
														)}
													</Button>
												)}
											</div>
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

export default ExamHistoryPage
