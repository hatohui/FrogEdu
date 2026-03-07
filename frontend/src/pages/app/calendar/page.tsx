import React, { useMemo, useState } from 'react'
import { useTranslation } from 'react-i18next'
import { Link } from 'react-router'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Calendar } from '@/components/ui/calendar'
import { Skeleton } from '@/components/ui/skeleton'
import { CalendarDays, FileText, Clock, ChevronLeft } from 'lucide-react'
import { useStudentExamSessions } from '@/hooks/useExamSessions'
import type { ExamSession } from '@/types/model/class-service'

const CalendarPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const [selectedDate, setSelectedDate] = useState<Date | undefined>(new Date())
	const { data: examSessions, isLoading } = useStudentExamSessions(false)

	// Group exam sessions by date
	const sessionsByDate = useMemo(() => {
		if (!examSessions) return new Map<string, ExamSession[]>()
		const map = new Map<string, ExamSession[]>()
		for (const session of examSessions) {
			const dateKey = new Date(session.startTime).toISOString().split('T')[0]
			const existing = map.get(dateKey) || []
			existing.push(session)
			map.set(dateKey, existing)
		}
		return map
	}, [examSessions])

	// Get dates that have exams for calendar highlighting
	const examDates = useMemo(() => {
		return Array.from(sessionsByDate.keys()).map(
			dateStr => new Date(dateStr + 'T00:00:00')
		)
	}, [sessionsByDate])

	// Get sessions for selected date
	const selectedDateSessions = useMemo(() => {
		if (!selectedDate) return []
		const dateKey = selectedDate.toISOString().split('T')[0]
		return sessionsByDate.get(dateKey) || []
	}, [selectedDate, sessionsByDate])

	const getSessionStatus = (session: ExamSession) => {
		if (session.isCurrentlyActive)
			return {
				label: t('pages.exam_sessions.status.active'),
				variant: 'default' as const,
				className: 'bg-green-600',
			}
		if (session.isUpcoming)
			return {
				label: t('pages.exam_sessions.status.upcoming'),
				variant: 'secondary' as const,
				className: '',
			}
		return {
			label: t('pages.exam_sessions.status.ended'),
			variant: 'outline' as const,
			className: '',
		}
	}

	if (isLoading) {
		return (
			<div className='p-6 space-y-6 max-w-7xl mx-auto'>
				<Skeleton className='h-10 w-48' />
				<div className='grid gap-6 lg:grid-cols-[auto_1fr]'>
					<Skeleton className='h-80 w-80' />
					<Skeleton className='h-80' />
				</div>
			</div>
		)
	}

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div className='flex items-center gap-4'>
				<Link to='/app'>
					<Button variant='ghost' size='icon'>
						<ChevronLeft className='h-5 w-5' />
					</Button>
				</Link>
				<div>
					<h1 className='text-3xl font-bold tracking-tight flex items-center gap-2'>
						<CalendarDays className='h-8 w-8' />
						{t('pages.calendar.title')}
					</h1>
					<p className='text-muted-foreground'>
						{t('pages.calendar.subtitle')}
					</p>
				</div>
			</div>

			<div className='grid gap-6 lg:grid-cols-[auto_1fr]'>
				{/* Calendar */}
				<Card>
					<CardContent className='p-4'>
						<Calendar
							mode='single'
							selected={selectedDate}
							onSelect={setSelectedDate}
							modifiers={{ hasExam: examDates }}
							modifiersClassNames={{
								hasExam: 'bg-primary/20 text-primary font-bold rounded-md',
							}}
						/>
						<div className='mt-3 flex items-center gap-2 text-xs text-muted-foreground px-1'>
							<div className='w-3 h-3 rounded bg-primary/20' />
							<span>{t('pages.calendar.has_exams')}</span>
						</div>
					</CardContent>
				</Card>

				{/* Selected Date Sessions */}
				<Card>
					<CardHeader>
						<CardTitle className='flex items-center gap-2'>
							<Clock className='h-5 w-5' />
							{selectedDate
								? t('pages.calendar.exams_on', {
										date: selectedDate.toLocaleDateString(undefined, {
											weekday: 'long',
											year: 'numeric',
											month: 'long',
											day: 'numeric',
										}),
									})
								: t('pages.calendar.select_date')}
						</CardTitle>
					</CardHeader>
					<CardContent>
						{selectedDateSessions.length > 0 ? (
							<div className='space-y-3'>
								{selectedDateSessions.map(session => {
									const status = getSessionStatus(session)
									return (
										<Link
											key={session.id}
											to={`/app/exam-sessions/${session.id}`}
											className='block'
										>
											<div className='flex items-center justify-between p-4 rounded-lg border hover:bg-accent transition-colors'>
												<div className='flex items-center gap-3'>
													<div className='w-10 h-10 rounded-lg bg-primary/10 flex items-center justify-center'>
														<FileText className='h-5 w-5 text-primary' />
													</div>
													<div>
														<p className='font-medium text-sm'>
															{t('pages.calendar.exam_session')}
														</p>
														<p className='text-xs text-muted-foreground'>
															{new Date(session.startTime).toLocaleTimeString(
																[],
																{
																	hour: '2-digit',
																	minute: '2-digit',
																}
															)}{' '}
															-{' '}
															{new Date(session.endTime).toLocaleTimeString(
																[],
																{
																	hour: '2-digit',
																	minute: '2-digit',
																}
															)}
														</p>
														{session.isRetryable && (
															<p className='text-xs text-muted-foreground'>
																{t('pages.calendar.retries_allowed', {
																	count: session.retryTimes,
																})}
															</p>
														)}
													</div>
												</div>
												<div className='flex items-center gap-2'>
													{session.attemptCount > 0 && (
														<Badge variant='outline'>
															{t('pages.calendar.attempts', {
																count: session.attemptCount,
															})}
														</Badge>
													)}
													<Badge
														variant={status.variant}
														className={status.className}
													>
														{status.label}
													</Badge>
												</div>
											</div>
										</Link>
									)
								})}
							</div>
						) : (
							<div className='flex flex-col items-center justify-center py-12 text-center'>
								<CalendarDays className='h-12 w-12 text-muted-foreground/30 mb-3' />
								<p className='text-sm text-muted-foreground'>
									{t('pages.calendar.no_exams')}
								</p>
							</div>
						)}
					</CardContent>
				</Card>
			</div>

			{/* Upcoming Sessions List */}
			{examSessions &&
				examSessions.filter(s => s.isUpcoming || s.isCurrentlyActive).length >
					0 && (
					<Card>
						<CardHeader>
							<CardTitle>{t('pages.calendar.all_upcoming')}</CardTitle>
						</CardHeader>
						<CardContent>
							<div className='space-y-2'>
								{examSessions
									.filter(s => s.isUpcoming || s.isCurrentlyActive)
									.sort(
										(a, b) =>
											new Date(a.startTime).getTime() -
											new Date(b.startTime).getTime()
									)
									.map(session => {
										const status = getSessionStatus(session)
										return (
											<Link
												key={session.id}
												to={`/app/exam-sessions/${session.id}`}
												className='block'
											>
												<div className='flex items-center justify-between p-3 rounded-lg border hover:bg-accent transition-colors'>
													<div className='flex items-center gap-3'>
														<div className='w-8 h-8 rounded bg-primary/10 flex items-center justify-center'>
															<FileText className='h-4 w-4 text-primary' />
														</div>
														<div>
															<p className='font-medium text-sm'>
																{t('pages.calendar.exam_session')}
															</p>
															<p className='text-xs text-muted-foreground'>
																{new Date(
																	session.startTime
																).toLocaleDateString()}{' '}
																{new Date(session.startTime).toLocaleTimeString(
																	[],
																	{
																		hour: '2-digit',
																		minute: '2-digit',
																	}
																)}
															</p>
														</div>
													</div>
													<Badge
														variant={status.variant}
														className={status.className}
													>
														{status.label}
													</Badge>
												</div>
											</Link>
										)
									})}
							</div>
						</CardContent>
					</Card>
				)}
		</div>
	)
}

export default CalendarPage
