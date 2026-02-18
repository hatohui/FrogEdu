import React from 'react'
import { useNavigate } from 'react-router'
import { useMe } from '@/hooks/auth/useMe'
import { useStudentExamSessions } from '@/hooks/useExamSessions'
import { useTranslation } from 'react-i18next'
import { Skeleton } from '@/components/ui/skeleton'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import {
	Clock,
	CalendarClock,
	PlayCircle,
	CheckCircle2,
	RotateCcw,
	Eye,
} from 'lucide-react'
import { format } from 'date-fns'
import type { ExamSession } from '@/types/model/class-service'

const ExamSessionsPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const { user } = useMe()
	const isStudent = user?.role?.name === 'Student'

	const { data: sessions, isLoading } = useStudentExamSessions(false)

	const active = sessions?.filter(s => s.isCurrentlyActive) || []
	const upcoming = sessions?.filter(s => s.isUpcoming) || []
	const ended = sessions?.filter(s => s.hasEnded) || []

	const getStatusBadge = (session: ExamSession) => {
		if (session.isCurrentlyActive)
			return (
				<Badge variant='default' className='gap-1'>
					<PlayCircle className='h-3 w-3' />
					{t('pages.exam_sessions.status.active')}
				</Badge>
			)
		if (session.isUpcoming)
			return (
				<Badge variant='secondary' className='gap-1'>
					<CalendarClock className='h-3 w-3' />
					{t('pages.exam_sessions.status.upcoming')}
				</Badge>
			)
		if (session.hasEnded)
			return (
				<Badge variant='outline' className='gap-1'>
					<CheckCircle2 className='h-3 w-3' />
					{t('pages.exam_sessions.status.ended')}
				</Badge>
			)
		return (
			<Badge variant='outline'>
				{t('pages.exam_sessions.status.inactive')}
			</Badge>
		)
	}

	/**
	 * Determine whether the student can still take/retry this session.
	 * - `attemptCount === 0` → has never started → show "Start Exam"
	 * - `attemptCount > 0 && isRetryable && attemptCount < retryTimes` → can retry → show "Retry"
	 * - otherwise → exhausted attempts
	 */
	const canStart = (session: ExamSession) =>
		session.isCurrentlyActive && session.attemptCount === 0

	const canRetry = (session: ExamSession) =>
		session.isCurrentlyActive &&
		session.isRetryable &&
		session.attemptCount > 0 &&
		session.attemptCount < session.retryTimes

	const SessionTable = ({
		sessions,
		showAction = true,
	}: {
		sessions: ExamSession[]
		showAction?: boolean
	}) => (
		<Table>
			<TableHeader>
				<TableRow>
					<TableHead>{t('pages.exam_sessions.table.start_time')}</TableHead>
					<TableHead>{t('pages.exam_sessions.table.end_time')}</TableHead>
					<TableHead>{t('pages.exam_sessions.table.retries')}</TableHead>
					<TableHead>{t('pages.exam_sessions.table.status')}</TableHead>
					{showAction && (
						<TableHead className='text-right'>
							{t('pages.exam_sessions.table.actions')}
						</TableHead>
					)}
				</TableRow>
			</TableHeader>
			<TableBody>
				{sessions.map(session => (
					<TableRow key={session.id}>
						<TableCell>{format(new Date(session.startTime), 'PPp')}</TableCell>
						<TableCell>{format(new Date(session.endTime), 'PPp')}</TableCell>
						<TableCell>
							{session.isRetryable
								? `${session.attemptCount} / ${session.retryTimes}`
								: `${session.attemptCount} / 1`}
						</TableCell>
						<TableCell>{getStatusBadge(session)}</TableCell>
						{showAction && (
							<TableCell className='text-right space-x-2'>
								{canStart(session) && (
									<Button
										size='sm'
										onClick={() =>
											navigate(`/app/exam-sessions/${session.id}/take`)
										}
									>
										<PlayCircle className='h-4 w-4 mr-1' />
										{t('pages.exam_sessions.actions.start_exam')}
									</Button>
								)}
								{canRetry(session) && (
									<Button
										size='sm'
										onClick={() =>
											navigate(`/app/exam-sessions/${session.id}/take`)
										}
									>
										<RotateCcw className='h-4 w-4 mr-1' />
										{t('pages.exam_sessions.actions.retry_exam')}
									</Button>
								)}
								{session.attemptCount > 0 && (
									<Button
										size='sm'
										variant='outline'
										onClick={() =>
											navigate(`/app/exam-sessions/${session.id}/my-results`)
										}
									>
										<Eye className='h-4 w-4 mr-1' />
										{t('pages.exam_sessions.actions.view_my_results')}
									</Button>
								)}
							</TableCell>
						)}
					</TableRow>
				))}
			</TableBody>
		</Table>
	)

	if (isLoading) {
		return (
			<div className='p-6 space-y-6 max-w-7xl mx-auto'>
				<Skeleton className='h-8 w-48' />
				<Skeleton className='h-12 w-96' />
				<Skeleton className='h-64 w-full' />
			</div>
		)
	}

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div>
				<h1 className='text-3xl font-bold tracking-tight flex items-center gap-2'>
					<Clock className='h-8 w-8' />
					{t('pages.exam_sessions.title')}
				</h1>
				<p className='text-muted-foreground mt-1'>
					{isStudent
						? t('pages.exam_sessions.subtitle_student')
						: t('pages.exam_sessions.subtitle_teacher')}
				</p>
			</div>

			<Tabs defaultValue='active'>
				<TabsList>
					<TabsTrigger value='active' className='gap-2'>
						<PlayCircle className='h-4 w-4' />
						{t('pages.exam_sessions.tabs.active', {
							count: active.length,
						})}
					</TabsTrigger>
					<TabsTrigger value='upcoming' className='gap-2'>
						<CalendarClock className='h-4 w-4' />
						{t('pages.exam_sessions.tabs.upcoming', {
							count: upcoming.length,
						})}
					</TabsTrigger>
					<TabsTrigger value='ended' className='gap-2'>
						<CheckCircle2 className='h-4 w-4' />
						{t('pages.exam_sessions.tabs.ended', {
							count: ended.length,
						})}
					</TabsTrigger>
				</TabsList>

				<TabsContent value='active' className='mt-6'>
					<Card>
						<CardHeader>
							<CardTitle>{t('pages.exam_sessions.status.active')}</CardTitle>
						</CardHeader>
						<CardContent>
							{active.length > 0 ? (
								<SessionTable sessions={active} />
							) : (
								<div className='text-center py-12'>
									<PlayCircle className='h-12 w-12 mx-auto text-muted-foreground mb-4 opacity-50' />
									<p className='text-muted-foreground'>
										{t('pages.exam_sessions.empty.student_description')}
									</p>
								</div>
							)}
						</CardContent>
					</Card>
				</TabsContent>

				<TabsContent value='upcoming' className='mt-6'>
					<Card>
						<CardHeader>
							<CardTitle>{t('pages.exam_sessions.status.upcoming')}</CardTitle>
						</CardHeader>
						<CardContent>
							{upcoming.length > 0 ? (
								<SessionTable sessions={upcoming} showAction={false} />
							) : (
								<div className='text-center py-12'>
									<CalendarClock className='h-12 w-12 mx-auto text-muted-foreground mb-4 opacity-50' />
									<p className='text-muted-foreground'>
										{t('pages.exam_sessions.empty.student_description')}
									</p>
								</div>
							)}
						</CardContent>
					</Card>
				</TabsContent>

				<TabsContent value='ended' className='mt-6'>
					<Card>
						<CardHeader>
							<CardTitle>{t('pages.exam_sessions.status.ended')}</CardTitle>
						</CardHeader>
						<CardContent>
							{ended.length > 0 ? (
								<SessionTable sessions={ended} />
							) : (
								<div className='text-center py-12'>
									<CheckCircle2 className='h-12 w-12 mx-auto text-muted-foreground mb-4 opacity-50' />
									<p className='text-muted-foreground'>
										{t('pages.exam_sessions.empty.student_description')}
									</p>
								</div>
							)}
						</CardContent>
					</Card>
				</TabsContent>
			</Tabs>
		</div>
	)
}

export default ExamSessionsPage
