import React, { useState, useMemo } from 'react'
import { useNavigate } from 'react-router'
import { useMe } from '@/hooks/auth/useMe'
import { useStudentExamSessions } from '@/hooks/useExamSessions'
import { useClasses } from '@/hooks/useClasses'
import { useTranslation } from 'react-i18next'
import { Skeleton } from '@/components/ui/skeleton'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import {
	Clock,
	CalendarClock,
	PlayCircle,
	CheckCircle2,
	RotateCcw,
	Eye,
	Search,
	Filter,
} from 'lucide-react'
import { format } from 'date-fns'
import type { ExamSession } from '@/types/model/class-service'

const ExamSessionsPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const { user } = useMe()
	const isStudent = user?.role?.name === 'Student'

	const { data: sessions, isLoading } = useStudentExamSessions(false)
	const { data: classes } = useClasses()

	// Filters
	const [filterClass, setFilterClass] = useState<string>('all')
	const [filterStatus, setFilterStatus] = useState<string>('all')
	const [searchQuery, setSearchQuery] = useState('')

	const filteredSessions = useMemo(() => {
		if (!sessions) return []
		return sessions.filter(s => {
			if (filterClass !== 'all' && s.classId !== filterClass) return false
			if (filterStatus === 'active' && !s.isCurrentlyActive) return false
			if (filterStatus === 'upcoming' && !s.isUpcoming) return false
			if (filterStatus === 'ended' && !s.hasEnded) return false
			if (searchQuery) {
				const q = searchQuery.toLowerCase()
				const sessionDate = format(new Date(s.startTime), 'PPp').toLowerCase()
				if (!sessionDate.includes(q) && !s.id.toLowerCase().includes(q))
					return false
			}
			return true
		})
	}, [sessions, filterClass, filterStatus, searchQuery])

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

	const canStart = (session: ExamSession) =>
		session.isCurrentlyActive && session.attemptCount === 0

	const canRetry = (session: ExamSession) =>
		session.isCurrentlyActive &&
		session.isRetryable &&
		session.attemptCount > 0 &&
		session.attemptCount <= session.retryTimes

	// Get class name for display
	const getClassName = (classId: string) => {
		const cls = classes?.find(c => c.id === classId)
		return cls?.name ?? classId.slice(0, 8)
	}

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

			{/* Filters */}
			<div className='flex flex-wrap gap-3'>
				<div className='relative flex-1 min-w-[200px] max-w-sm'>
					<Search className='absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground' />
					<Input
						placeholder={t('actions.search')}
						value={searchQuery}
						onChange={e => setSearchQuery(e.target.value)}
						className='pl-9'
					/>
				</div>
				{classes && classes.length > 0 && (
					<Select value={filterClass} onValueChange={setFilterClass}>
						<SelectTrigger className='w-[200px]'>
							<Filter className='h-4 w-4 mr-2' />
							<SelectValue placeholder={t('navigation.classes')} />
						</SelectTrigger>
						<SelectContent>
							<SelectItem value='all'>{t('common.all')}</SelectItem>
							{classes.map(c => (
								<SelectItem key={c.id} value={c.id}>
									{c.name}
								</SelectItem>
							))}
						</SelectContent>
					</Select>
				)}
				<Select value={filterStatus} onValueChange={setFilterStatus}>
					<SelectTrigger className='w-[160px]'>
						<SelectValue placeholder={t('labels.status')} />
					</SelectTrigger>
					<SelectContent>
						<SelectItem value='all'>{t('common.all')}</SelectItem>
						<SelectItem value='active'>
							{t('pages.exam_sessions.status.active')}
						</SelectItem>
						<SelectItem value='upcoming'>
							{t('pages.exam_sessions.status.upcoming')}
						</SelectItem>
						<SelectItem value='ended'>
							{t('pages.exam_sessions.status.ended')}
						</SelectItem>
					</SelectContent>
				</Select>
			</div>

			{/* Sessions List */}
			<Card>
				<CardHeader>
					<CardTitle>
						{t('pages.exam_sessions.title')} ({filteredSessions.length})
					</CardTitle>
				</CardHeader>
				<CardContent>
					{filteredSessions.length > 0 ? (
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead>{t('navigation.classes')}</TableHead>
									<TableHead>
										{t('pages.exam_sessions.table.start_time')}
									</TableHead>
									<TableHead>
										{t('pages.exam_sessions.table.end_time')}
									</TableHead>
									<TableHead>
										{t('pages.exam_sessions.table.retries')}
									</TableHead>
									<TableHead>{t('pages.exam_sessions.table.status')}</TableHead>
									<TableHead className='text-right'>
										{t('pages.exam_sessions.table.actions')}
									</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{filteredSessions.map(session => (
									<TableRow
										key={session.id}
										className='cursor-pointer'
										onClick={() => navigate(`/app/exam-sessions/${session.id}`)}
									>
										<TableCell className='font-medium'>
											{getClassName(session.classId)}
										</TableCell>
										<TableCell>
											{format(new Date(session.startTime), 'PPp')}
										</TableCell>
										<TableCell>
											{format(new Date(session.endTime), 'PPp')}
										</TableCell>
										<TableCell>
											{session.isRetryable
												? `${session.attemptCount} / ${session.retryTimes}`
												: `${session.attemptCount} / 1`}
										</TableCell>
										<TableCell>{getStatusBadge(session)}</TableCell>
										<TableCell
											className='text-right space-x-2'
											onClick={e => e.stopPropagation()}
										>
											{isStudent && canStart(session) && (
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
											{isStudent && canRetry(session) && (
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
											{session.attemptCount > 0 && isStudent && (
												<Button
													size='sm'
													variant='outline'
													onClick={() =>
														navigate(
															`/app/exam-sessions/${session.id}/my-results`
														)
													}
												>
													<Eye className='h-4 w-4 mr-1' />
													{t('pages.exam_sessions.actions.view_my_results')}
												</Button>
											)}
											{!isStudent && (
												<Button
													size='sm'
													variant='outline'
													onClick={() =>
														navigate(`/app/exam-sessions/${session.id}`)
													}
												>
													<Eye className='h-4 w-4 mr-1' />
													{t('actions.view_details')}
												</Button>
											)}
										</TableCell>
									</TableRow>
								))}
							</TableBody>
						</Table>
					) : (
						<div className='text-center py-12'>
							<Clock className='h-12 w-12 mx-auto text-muted-foreground mb-4 opacity-50' />
							<p className='text-muted-foreground'>
								{t('pages.exam_sessions.empty.student_description')}
							</p>
						</div>
					)}
				</CardContent>
			</Card>
		</div>
	)
}

export default ExamSessionsPage
