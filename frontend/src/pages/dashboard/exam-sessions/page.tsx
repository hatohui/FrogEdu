import React, { useMemo, useState } from 'react'
import { useNavigate } from 'react-router'
import { useTranslation } from 'react-i18next'
import { useQueries } from '@tanstack/react-query'
import {
	Clock,
	PlayCircle,
	CalendarClock,
	CheckCircle2,
	Users,
	BarChart3,
	Eye,
	ChevronsUpDown,
	Check,
} from 'lucide-react'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Skeleton } from '@/components/ui/skeleton'
import {
	Popover,
	PopoverContent,
	PopoverTrigger,
} from '@/components/ui/popover'
import {
	Command,
	CommandEmpty,
	CommandGroup,
	CommandInput,
	CommandItem,
	CommandList,
} from '@/components/ui/command'
import { cn } from '@/utils/cn'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import { format } from 'date-fns'
import { useClasses } from '@/hooks/useClasses'
import { useExamSessions, useSessionResults } from '@/hooks/useExamSessions'
import { userKeys } from '@/hooks/useUsers'
import userService from '@/services/user-microservice/user.service'
import type { ExamSession } from '@/types/model/class-service'
import { AttemptStatus } from '@/types/model/class-service'

const DashboardExamSessionsPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	const [selectedClassId, setSelectedClassId] = useState<string>('')
	const [selectedSessionId, setSelectedSessionId] = useState<string>('')
	const [classPickerOpen, setClassPickerOpen] = useState(false)

	const { data: classes, isLoading: loadingClasses } = useClasses()
	const { data: sessions, isLoading: loadingSessions } =
		useExamSessions(selectedClassId)
	const { data: results, isLoading: loadingResults } =
		useSessionResults(selectedSessionId)

	// Batch-fetch teacher data for all unique teacher IDs
	const uniqueTeacherIds = useMemo(
		() => [...new Set(classes?.map(c => c.teacherId) ?? [])],
		[classes]
	)
	const teacherQueries = useQueries({
		queries: uniqueTeacherIds.map(id => ({
			queryKey: userKeys.detail(id),
			queryFn: () => userService.getUserById(id),
			staleTime: 5 * 60 * 1000,
		})),
	})
	const teacherMap = useMemo(() => {
		const map: Record<string, string> = {}
		uniqueTeacherIds.forEach((id, i) => {
			const user = teacherQueries[i]?.data
			if (user) map[id] = `${user.firstName} ${user.lastName}`
		})
		return map
	}, [uniqueTeacherIds, teacherQueries])

	const selectedClass = classes?.find(c => c.id === selectedClassId)
	const selectedSession = sessions?.find(s => s.id === selectedSessionId)

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

	const getAttemptStatusBadge = (status: string) => {
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

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div>
				<h1 className='text-2xl font-bold tracking-tight flex items-center gap-2'>
					<Clock className='h-6 w-6' />
					{t('pages.dashboard.exam_sessions.title')}
				</h1>
				<p className='text-muted-foreground mt-1'>
					{t('pages.dashboard.exam_sessions.subtitle')}
				</p>
			</div>

			{/* Step 1: Class selector */}
			<Card>
				<CardHeader>
					<CardTitle>
						{t('pages.dashboard.exam_sessions.select_class')}
					</CardTitle>
				</CardHeader>
				<CardContent>
					{loadingClasses ? (
						<Skeleton className='h-10 w-72' />
					) : (
						<Popover open={classPickerOpen} onOpenChange={setClassPickerOpen}>
							<PopoverTrigger asChild>
								<Button
									variant='outline'
									role='combobox'
									aria-expanded={classPickerOpen}
									className='w-80 justify-between'
								>
									<span className='truncate'>
										{selectedClass
											? `${selectedClass.name} · Grade ${selectedClass.grade} · ${selectedClass.inviteCode}`
											: t(
													'pages.dashboard.exam_sessions.select_class_placeholder'
												)}
									</span>
									<ChevronsUpDown className='ml-2 h-4 w-4 shrink-0 opacity-50' />
								</Button>
							</PopoverTrigger>
							<PopoverContent className='w-[560px] p-0' align='start'>
								<Command>
									<CommandInput placeholder='Search by name, grade, invite code, teacher, or ID…' />
									<CommandList className='max-h-[380px]'>
										<CommandEmpty>No classes found.</CommandEmpty>
										<CommandGroup>
											{classes?.map(c => {
												const teacherName = teacherMap[c.teacherId]
												return (
													<CommandItem
														key={c.id}
														value={[
															c.name,
															`grade ${c.grade}`,
															c.inviteCode,
															c.id,
															teacherName ?? c.teacherId,
														].join(' ')}
														onSelect={() => {
															setSelectedClassId(c.id)
															setSelectedSessionId('')
															setClassPickerOpen(false)
														}}
													>
														<Check
															className={cn(
																'mr-2 h-4 w-4 shrink-0',
																selectedClassId === c.id
																	? 'opacity-100'
																	: 'opacity-0'
															)}
														/>
														<div className='flex-1 min-w-0'>
															<div className='flex items-center gap-2'>
																<span className='font-medium truncate'>
																	{c.name}
																</span>
																<Badge
																	variant='secondary'
																	className='shrink-0 text-xs'
																>
																	Grade {c.grade}
																</Badge>
																<code className='shrink-0 text-xs bg-muted px-1.5 py-0.5 rounded font-mono'>
																	{c.inviteCode}
																</code>
															</div>
															<div className='flex items-center gap-3 mt-0.5 text-xs text-muted-foreground'>
																<span>
																	{teacherName ?? (
																		<span className='italic opacity-60'>
																			{c.teacherId.slice(0, 8)}…
																		</span>
																	)}
																</span>
																<span>·</span>
																<span>{c.studentCount} students</span>
																<span>·</span>
																<span className='font-mono'>
																	{c.id.slice(0, 8)}…
																</span>
															</div>
														</div>
													</CommandItem>
												)
											})}
										</CommandGroup>
									</CommandList>
								</Command>
							</PopoverContent>
						</Popover>
					)}
				</CardContent>
			</Card>

			{/* Step 2: Sessions for selected class */}
			{selectedClassId && (
				<Card>
					<CardHeader>
						<CardTitle>
							{t('pages.dashboard.exam_sessions.sessions_in', {
								class: selectedClass?.name,
							})}
						</CardTitle>
						<CardDescription>
							{t('pages.dashboard.exam_sessions.click_session_to_see_results')}
						</CardDescription>
					</CardHeader>
					<CardContent>
						{loadingSessions ? (
							<Skeleton className='h-40 w-full' />
						) : !sessions || sessions.length === 0 ? (
							<div className='text-center py-8 text-muted-foreground'>
								{t('pages.dashboard.exam_sessions.no_sessions')}
							</div>
						) : (
							<Table>
								<TableHeader>
									<TableRow>
										<TableHead>
											{t('pages.exam_sessions.table.start_time')}
										</TableHead>
										<TableHead>
											{t('pages.exam_sessions.table.end_time')}
										</TableHead>
										<TableHead>
											{t('pages.exam_sessions.table.retries')}
										</TableHead>
										<TableHead>
											{t('pages.exam_sessions.table.status')}
										</TableHead>
										<TableHead className='text-right'>
											{t('pages.exam_sessions.table.actions')}
										</TableHead>
									</TableRow>
								</TableHeader>
								<TableBody>
									{sessions.map(session => (
										<TableRow
											key={session.id}
											className={
												selectedSessionId === session.id ? 'bg-muted' : ''
											}
										>
											<TableCell>
												{format(new Date(session.startTime), 'PPp')}
											</TableCell>
											<TableCell>
												{format(new Date(session.endTime), 'PPp')}
											</TableCell>
											<TableCell>
												{session.isRetryable
													? `${session.retryTimes} retries`
													: '1 attempt only'}
											</TableCell>
											<TableCell>{getStatusBadge(session)}</TableCell>
											<TableCell className='text-right'>
												<Button
													size='sm'
													variant={
														selectedSessionId === session.id
															? 'default'
															: 'outline'
													}
													onClick={() =>
														setSelectedSessionId(
															selectedSessionId === session.id ? '' : session.id
														)
													}
												>
													<BarChart3 className='h-4 w-4 mr-1' />
													{t('pages.exam_sessions.actions.view_results')}
												</Button>
											</TableCell>
										</TableRow>
									))}
								</TableBody>
							</Table>
						)}
					</CardContent>
				</Card>
			)}

			{/* Step 3: Results for selected session */}
			{selectedSessionId && (
				<Card>
					<CardHeader>
						<div className='flex items-start justify-between'>
							<div>
								<CardTitle className='flex items-center gap-2'>
									<Users className='h-5 w-5' />
									{t('pages.exam_sessions.results.title')}
								</CardTitle>
								<CardDescription>
									{format(new Date(selectedSession?.startTime || ''), 'PPp')} →{' '}
									{format(new Date(selectedSession?.endTime || ''), 'PPp')}
								</CardDescription>
							</div>
						</div>
					</CardHeader>
					<CardContent className='space-y-4'>
						{loadingResults ? (
							<>
								<div className='grid gap-4 md:grid-cols-4'>
									{[1, 2, 3, 4].map(i => (
										<Skeleton key={i} className='h-20' />
									))}
								</div>
								<Skeleton className='h-40' />
							</>
						) : !results ? (
							<p className='text-muted-foreground text-center py-8'>
								{t('pages.exam_sessions.results.empty')}
							</p>
						) : (
							<>
								{/* Stats */}
								<div className='grid gap-4 md:grid-cols-4'>
									<Card>
										<CardHeader className='pb-2'>
											<CardDescription>
												{t(
													'pages.exam_sessions.results.summary.total_attempts'
												)}
											</CardDescription>
										</CardHeader>
										<CardContent>
											<p className='text-2xl font-bold'>
												{results.totalAttempts}
											</p>
										</CardContent>
									</Card>
									<Card>
										<CardHeader className='pb-2'>
											<CardDescription>
												{t('pages.exam_sessions.results.summary.average_score')}
											</CardDescription>
										</CardHeader>
										<CardContent>
											<p className='text-2xl font-bold'>
												{results.averageScore.toFixed(1)}%
											</p>
										</CardContent>
									</Card>
									<Card>
										<CardHeader className='pb-2'>
											<CardDescription>
												{t('pages.exam_sessions.results.summary.highest_score')}
											</CardDescription>
										</CardHeader>
										<CardContent>
											<p className='text-2xl font-bold text-green-600'>
												{results.highestScore.toFixed(1)}%
											</p>
										</CardContent>
									</Card>
									<Card>
										<CardHeader className='pb-2'>
											<CardDescription>
												{t('pages.exam_sessions.results.summary.lowest_score')}
											</CardDescription>
										</CardHeader>
										<CardContent>
											<p className='text-2xl font-bold text-red-600'>
												{results.lowestScore.toFixed(1)}%
											</p>
										</CardContent>
									</Card>
								</div>

								{/* Attempts list */}
								{results.attempts.length === 0 ? (
									<div className='text-center py-8 text-muted-foreground'>
										{t('pages.exam_sessions.results.empty')}
									</div>
								) : (
									<Table>
										<TableHeader>
											<TableRow>
												<TableHead>
													{t('pages.exam_sessions.results.table.student')}
												</TableHead>
												<TableHead>
													{t(
														'pages.exam_sessions.results.table.attempt_number'
													)}
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
											{results.attempts.map(attempt => (
												<TableRow key={attempt.attemptId}>
													<TableCell className='font-medium'>
														{attempt.studentName}
													</TableCell>
													<TableCell># {attempt.attemptNumber}</TableCell>
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
													<TableCell>
														{getAttemptStatusBadge(attempt.status)}
													</TableCell>
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
																navigate(
																	`/app/exam-sessions/${selectedSessionId}/attempts/${attempt.attemptId}/review`
																)
															}
														>
															<Eye className='h-4 w-4 mr-1' />
															{t('pages.exam_sessions.results.view_detail')}
														</Button>
													</TableCell>
												</TableRow>
											))}
										</TableBody>
									</Table>
								)}
							</>
						)}
					</CardContent>
				</Card>
			)}
		</div>
	)
}

export default DashboardExamSessionsPage
