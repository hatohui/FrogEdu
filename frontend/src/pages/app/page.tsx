import React, { useMemo } from 'react'
import { useTranslation } from 'react-i18next'
import { useMe } from '@/hooks/auth/useMe'
import { useClasses } from '@/hooks/useClasses'
import { useEffectiveRole } from '@/hooks/useEffectiveRole'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Progress } from '@/components/ui/progress'
import {
	BookOpen,
	FileText,
	Users,
	TrendingUp,
	Plus,
	Crown,
	CreditCard,
	Sparkles,
	CalendarDays,
} from 'lucide-react'
import { Link } from 'react-router'
import { Skeleton } from '@/components/ui/skeleton'
import { ClassCard } from '@/components/classes'
import { useSubscription, useAIUsageLimit } from '@/hooks/useSubscription'
import { useStudentExamSessions } from '@/hooks/useExamSessions'

interface StatCardProps {
	title: string
	value: number
	icon: React.ComponentType<{ className?: string }>
	trend?: {
		value: number
		isPositive: boolean
	}
}

const StatCard = ({ title, value, icon: Icon, trend }: StatCardProps) => {
	const { t } = useTranslation()
	return (
		<Card className='hover:shadow-md transition-shadow'>
			<CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
				<CardTitle className='text-sm font-medium text-muted-foreground'>
					{title}
				</CardTitle>
				<Icon className='h-4 w-4 text-muted-foreground' />
			</CardHeader>
			<CardContent>
				<div className='text-2xl font-bold'>{value}</div>
				{trend && (
					<p
						className={`text-xs flex items-center mt-1 ${
							trend.isPositive ? 'text-green-600' : 'text-red-600'
						}`}
					>
						<TrendingUp className='h-3 w-3 mr-1' />
						{t('pages.app_dashboard.trend_label', { value: trend.value })}
					</p>
				)}
			</CardContent>
		</Card>
	)
}

const DashboardPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const { user } = useMe()
	const { isTeacher, isStudent, isAdmin, isViewingAs } = useEffectiveRole()

	// Fetch real data using TanStack Query
	const { data: classes, isLoading: classesLoading } = useClasses()
	const { subscription, isPro, isFree } = useSubscription()
	const { usedCount, maxAllowed, remaining, canUseAI, isUnlimited } =
		useAIUsageLimit()
	const { data: upcomingExams, isLoading: examsLoading } =
		useStudentExamSessions(true)

	const stats = useMemo(() => {
		if (!classes) return { classCount: 0, studentCount: 0, assignmentCount: 0 }
		return {
			classCount: classes.length,
			studentCount: classes.reduce((sum, c) => sum + c.studentCount, 0),
			assignmentCount: classes.reduce((sum, c) => sum + c.assignmentCount, 0),
		}
	}, [classes])

	const recentClasses = classes?.slice(0, 3) || []

	const getUserDisplayName = () => {
		if (!user) return 'User'
		return user?.firstName || user?.email?.split('@')[0] || 'User'
	}

	const isLoading = classesLoading

	if (isLoading) {
		return (
			<div className='p-6 space-y-6'>
				<Skeleton className='h-12 w-64' />
				<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-4'>
					{[1, 2, 3, 4].map(i => (
						<Skeleton key={i} className='h-32' />
					))}
				</div>
				<Skeleton className='h-64' />
			</div>
		)
	}

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Welcome Header */}
			<div className='flex items-center justify-between'>
				<div className='space-y-2'>
					<h1 className='text-3xl font-bold tracking-tight'>
						{t('pages.app_dashboard.welcome', { name: getUserDisplayName() })}
					</h1>
					<p className='text-muted-foreground'>
						{isViewingAs
							? t('pages.app_dashboard.subtitle_preview', {
									role: isTeacher ? t('roles.teacher') : t('roles.student'),
								})
							: t('pages.app_dashboard.subtitle')}
					</p>
				</div>
				{!isAdmin && (
					<Link to='/profile/subscription'>
						{isPro ? (
							<Badge className='bg-gradient-to-r from-amber-500 to-orange-500 text-white px-3 py-1'>
								<Crown className='h-4 w-4 mr-1' />
								{t('badges.pro')}
							</Badge>
						) : (
							<Button variant='outline' size='sm'>
								<CreditCard className='h-4 w-4 mr-2' />
								{t('actions.upgrade_to_pro')}
							</Button>
						)}
					</Link>
				)}
			</div>

			{/* Subscription & AI Usage Card for Teachers */}
			{isTeacher && (
				<div className='grid gap-4 md:grid-cols-2'>
					{/* Subscription Status */}
					<Card>
						<CardHeader className='pb-3'>
							<CardTitle className='text-base flex items-center gap-2'>
								<CreditCard className='h-4 w-4' />
								{t('pages.app_dashboard.subscription_status')}
							</CardTitle>
						</CardHeader>
						<CardContent>
							<div className='flex items-center justify-between'>
								<div>
									<p className='text-2xl font-bold'>
										{subscription?.planName || t('badges.free')}
									</p>
									{subscription?.isActive && subscription?.endDate && (
										<p className='text-xs text-muted-foreground mt-1'>
											{t('pages.app_dashboard.expires_on', {
												date: new Date(
													subscription.endDate
												).toLocaleDateString(),
											})}
										</p>
									)}
								</div>
								{isFree && (
									<Link to='/profile/subscription'>
										<Button size='sm' variant='default'>
											{t('actions.upgrade_to_pro')}
										</Button>
									</Link>
								)}
							</div>
						</CardContent>
					</Card>

					{/* AI Usage */}
					<Card>
						<CardHeader className='pb-3'>
							<CardTitle className='text-base flex items-center gap-2'>
								<Sparkles className='h-4 w-4' />
								{t('pages.app_dashboard.ai_usage')}
							</CardTitle>
						</CardHeader>
						<CardContent>
							{isUnlimited ? (
								<div>
									<p className='text-2xl font-bold text-green-600'>
										{t('pages.app_dashboard.unlimited')}
									</p>
									<p className='text-xs text-muted-foreground mt-1'>
										{t('pages.app_dashboard.ai_used_count', {
											count: usedCount,
										})}
									</p>
								</div>
							) : (
								<div className='space-y-2'>
									<div className='flex justify-between text-sm'>
										<span>
											{usedCount} / {maxAllowed}{' '}
											{t('pages.app_dashboard.generations')}
										</span>
										<span className={!canUseAI ? 'text-destructive' : ''}>
											{remaining} {t('pages.app_dashboard.remaining')}
										</span>
									</div>
									<Progress
										value={maxAllowed ? (usedCount / maxAllowed) * 100 : 0}
										className='h-2'
									/>
									{!canUseAI && (
										<p className='text-xs text-destructive'>
											{t('pages.app_dashboard.ai_limit_reached')}
										</p>
									)}
								</div>
							)}
						</CardContent>
					</Card>
				</div>
			)}

			{/* Student: Upcoming Exams */}
			{isStudent && (
				<Card>
					<CardHeader className='flex flex-row items-center justify-between'>
						<CardTitle className='flex items-center gap-2'>
							<CalendarDays className='h-5 w-5' />
							{t('pages.app_dashboard.upcoming_exams')}
						</CardTitle>
						<Link to='/app/calendar'>
							<Button variant='ghost' size='sm'>
								{t('pages.app_dashboard.view_calendar')}
							</Button>
						</Link>
					</CardHeader>
					<CardContent>
						{examsLoading ? (
							<div className='space-y-3'>
								{[1, 2, 3].map(i => (
									<Skeleton key={i} className='h-16' />
								))}
							</div>
						) : upcomingExams && upcomingExams.length > 0 ? (
							<div className='space-y-3'>
								{upcomingExams.slice(0, 5).map(session => (
									<Link
										key={session.id}
										to={`/app/exam-sessions/${session.id}`}
										className='block'
									>
										<div className='flex items-center justify-between p-3 rounded-lg border hover:bg-accent transition-colors'>
											<div className='flex items-center gap-3'>
												<div className='w-10 h-10 rounded-lg bg-primary/10 flex items-center justify-center'>
													<FileText className='h-5 w-5 text-primary' />
												</div>
												<div>
													<p className='font-medium text-sm'>
														{t('pages.app_dashboard.exam_session')}
													</p>
													<p className='text-xs text-muted-foreground'>
														{new Date(session.startTime).toLocaleDateString()} -{' '}
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
											{session.isCurrentlyActive ? (
												<Badge className='bg-green-600'>
													{t('pages.exam_sessions.status.active')}
												</Badge>
											) : session.isUpcoming ? (
												<Badge variant='secondary'>
													{t('pages.exam_sessions.status.upcoming')}
												</Badge>
											) : (
												<Badge variant='outline'>
													{t('pages.exam_sessions.status.ended')}
												</Badge>
											)}
										</div>
									</Link>
								))}
							</div>
						) : (
							<p className='text-sm text-muted-foreground text-center py-6'>
								{t('pages.app_dashboard.no_upcoming_exams')}
							</p>
						)}
					</CardContent>
				</Card>
			)}

			{/* Stats Grid */}
			<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-3'>
				<StatCard
					title={t('pages.app_dashboard.stats.active_classes')}
					value={stats.classCount}
					icon={Users}
				/>
				<StatCard
					title={t('pages.app_dashboard.stats.students_enrolled')}
					value={stats.studentCount}
					icon={Users}
				/>
				<StatCard
					title={t('pages.app_dashboard.stats.assignments')}
					value={stats.assignmentCount}
					icon={FileText}
				/>
			</div>

			{/* Recent Classes */}
			{recentClasses.length > 0 && (
				<Card>
					<CardHeader className='flex flex-row items-center justify-between'>
						<CardTitle>{t('pages.app_dashboard.recent_classes')}</CardTitle>
						<Link to='/app/classes'>
							<Button variant='ghost' size='sm'>
								{t('pages.app_dashboard.view_all')}
							</Button>
						</Link>
					</CardHeader>
					<CardContent>
						<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-3'>
							{recentClasses.map(classData => (
								<ClassCard
									key={classData.id}
									classData={classData}
									isTeacher={isTeacher}
								/>
							))}
						</div>
					</CardContent>
				</Card>
			)}

			{/* Quick Actions - role-aware */}
			<Card>
				<CardHeader>
					<CardTitle>{t('pages.app_dashboard.quick_actions.title')}</CardTitle>
				</CardHeader>
				<CardContent className='flex flex-wrap gap-4'>
					{(isTeacher || isAdmin) && (
						<Link to='/app/exams/create'>
							<Button className='space-x-2'>
								<Plus className='h-4 w-4' />
								<span>
									{t('pages.app_dashboard.quick_actions.create_exam')}
								</span>
							</Button>
						</Link>
					)}
					<Link to='/app/classes'>
						<Button variant='outline' className='space-x-2'>
							<Users className='h-4 w-4' />
							<span>
								{isStudent
									? t('pages.app_dashboard.quick_actions.my_classes')
									: t('pages.app_dashboard.quick_actions.manage_classes')}
							</span>
						</Button>
					</Link>
					{isStudent && (
						<Link to='/app/calendar'>
							<Button variant='outline' className='space-x-2'>
								<CalendarDays className='h-4 w-4' />
								<span>
									{t('pages.app_dashboard.quick_actions.view_calendar')}
								</span>
							</Button>
						</Link>
					)}
					{(isTeacher || isAdmin) && (
						<Link to='/app/content'>
							<Button variant='outline' className='space-x-2'>
								<BookOpen className='h-4 w-4' />
								<span>
									{t('pages.app_dashboard.quick_actions.browse_content')}
								</span>
							</Button>
						</Link>
					)}
				</CardContent>
			</Card>
		</div>
	)
}

export default DashboardPage
