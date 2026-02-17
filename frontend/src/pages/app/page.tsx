import React, { useMemo } from 'react'
import { useTranslation } from 'react-i18next'
import { useMe } from '@/hooks/auth/useMe'
import { useClasses } from '@/hooks/useClasses'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import {
	BookOpen,
	FileText,
	Users,
	TrendingUp,
	Plus,
	Clock,
} from 'lucide-react'
import { Link } from 'react-router'
import { Skeleton } from '@/components/ui/skeleton'
import { ClassCard } from '@/components/classes'

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
	const { user, isLoading: authLoading } = useMe()
	const isTeacher = user?.role?.name === 'Teacher'

	// Fetch real data using TanStack Query
	const { data: classes, isLoading: classesLoading } = useClasses()

	const stats = useMemo(() => {
		if (!classes) return { classCount: 0, studentCount: 0, assignmentCount: 0 }
		return {
			classCount: classes.length,
			studentCount: classes.reduce((sum, c) => sum + c.studentCount, 0),
			assignmentCount: classes.reduce((sum, c) => sum + c.assignmentCount, 0),
		}
	}, [classes])

	const recentClasses = classes?.slice(0, 3) || []

	const recentActivities = [
		{ id: 1, action: 'Created exam "Math Quiz 1"', time: '2 hours ago' },
		{ id: 2, action: 'Edited "Science Test Chapter 3"', time: '1 day ago' },
		{ id: 3, action: 'Added content "Biology Notes"', time: '2 days ago' },
		{ id: 4, action: 'Reviewed student submissions', time: '3 days ago' },
	]

	const getUserDisplayName = () => {
		if (!user) return 'User'
		return user?.firstName || user?.email?.split('@')[0] || 'User'
	}

	const isLoading = authLoading || classesLoading

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
			<div className='space-y-2'>
				<h1 className='text-3xl font-bold tracking-tight'>
					{t('pages.app_dashboard.welcome', { name: getUserDisplayName() })}
				</h1>
				<p className='text-muted-foreground'>
					{t('pages.app_dashboard.subtitle')}
				</p>
			</div>

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
						<Link to='/dashboard/classes'>
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

			{/* Quick Actions */}
			<Card>
				<CardHeader>
					<CardTitle>{t('pages.app_dashboard.quick_actions.title')}</CardTitle>
				</CardHeader>
				<CardContent className='flex flex-wrap gap-4'>
					<Link to='/app/exams/create'>
						<Button className='space-x-2'>
							<Plus className='h-4 w-4' />
							<span>{t('pages.app_dashboard.quick_actions.create_exam')}</span>
						</Button>
					</Link>
					<Link to='/app/content'>
						<Button variant='outline' className='space-x-2'>
							<BookOpen className='h-4 w-4' />
							<span>
								{t('pages.app_dashboard.quick_actions.browse_content')}
							</span>
						</Button>
					</Link>
					<Link to='/app/classes'>
						<Button variant='outline' className='space-x-2'>
							<Users className='h-4 w-4' />
							<span>
								{t('pages.app_dashboard.quick_actions.manage_classes')}
							</span>
						</Button>
					</Link>
				</CardContent>
			</Card>

			{/* Recent Activity */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center space-x-2'>
						<Clock className='h-5 w-5' />
						<span>{t('pages.app_dashboard.recent_activity')}</span>
					</CardTitle>
				</CardHeader>
				<CardContent>
					<div className='space-y-4'>
						{recentActivities.map(activity => (
							<div
								key={activity.id}
								className='flex items-start space-x-3 p-3 rounded-lg hover:bg-accent transition-colors'
							>
								<div className='flex-shrink-0 w-2 h-2 mt-2 rounded-full bg-primary' />
								<div className='flex-1'>
									<p className='text-sm font-medium'>{activity.action}</p>
									<p className='text-xs text-muted-foreground'>
										{activity.time}
									</p>
								</div>
							</div>
						))}
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default DashboardPage
