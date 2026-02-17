import React from 'react'
import {
	Users,
	BookOpen,
	FileText,
	GraduationCap,
	TrendingUp,
	Activity,
	AlertCircle,
	CheckCircle2,
	Clock,
	ArrowUpRight,
	ArrowDownRight,
	MoreVertical,
} from 'lucide-react'
import { useTranslation } from 'react-i18next'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import { Badge } from '@/components/ui/badge'
import { Progress } from '@/components/ui/progress'
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import { Skeleton } from '@/components/ui/skeleton'
import { useNavigate } from 'react-router'
import { useMe } from '@/hooks/auth/useMe'
import { useUserStatistics } from '@/hooks/useUsers'

const recentActivities = [
	{
		id: 1,
		user: 'John Doe',
		action: 'created a new exam',
		target: 'Mathematics Final 2026',
		time: '2 minutes ago',
		type: 'exam',
	},
	{
		id: 2,
		user: 'Sarah Wilson',
		action: 'published',
		target: 'Physics Quiz #12',
		time: '15 minutes ago',
		type: 'publish',
	},
	{
		id: 3,
		user: 'Mike Johnson',
		action: 'joined class',
		target: 'Advanced Chemistry',
		time: '1 hour ago',
		type: 'join',
	},
	{
		id: 4,
		user: 'Emily Chen',
		action: 'submitted',
		target: 'Biology Midterm',
		time: '2 hours ago',
		type: 'submit',
	},
	{
		id: 5,
		user: 'David Lee',
		action: 'created',
		target: '45 new questions',
		time: '3 hours ago',
		type: 'question',
	},
]

const systemHealth = [
	{ name: 'API Response Time', value: 95, status: 'healthy' },
	{ name: 'Database Performance', value: 88, status: 'healthy' },
	{ name: 'Storage Usage', value: 67, status: 'warning' },
	{ name: 'User Authentication', value: 99, status: 'healthy' },
]

const topSubjects = [
	{ name: 'Mathematics', exams: 45, questions: 892, students: 456 },
	{ name: 'Physics', exams: 38, questions: 743, students: 398 },
	{ name: 'Chemistry', exams: 32, questions: 651, students: 367 },
	{ name: 'Biology', exams: 28, questions: 589, students: 342 },
	{ name: 'English', exams: 23, questions: 478, students: 298 },
]

const DashboardPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const { user } = useMe()
	const { data: statistics, isLoading: isLoadingStats } = useUserStatistics()

	// Build stats from real data
	const stats = [
		{
			title: t('dashboard.total_users'),
			value: statistics?.totalUsers?.toString() || '0',
			change: `+${statistics?.usersCreatedLast30Days || 0}`,
			trend: 'up' as const,
			icon: Users,
			color: 'text-blue-600',
			bgColor: 'bg-blue-100 dark:bg-blue-950',
		},
		{
			title: t('dashboard.active_exams'),
			value: '0', // TODO: Replace with real exam statistics
			change: '+0',
			trend: 'up' as const,
			icon: FileText,
			color: 'text-green-600',
			bgColor: 'bg-green-100 dark:bg-green-950',
		},
		{
			title: t('dashboard.total_questions'),
			value: '0', // TODO: Replace with real question statistics
			change: '+0',
			trend: 'up' as const,
			icon: Activity,
			color: 'text-purple-600',
			bgColor: 'bg-purple-100 dark:bg-purple-950',
		},
		{
			title: t('dashboard.active_classes'),
			value: '0', // TODO: Replace with real class statistics
			change: '+0',
			trend: 'up' as const,
			icon: GraduationCap,
			color: 'text-orange-600',
			bgColor: 'bg-orange-100 dark:bg-orange-950',
		},
	]

	const getActivityIcon = (type: string) => {
		switch (type) {
			case 'exam':
				return <FileText className='h-4 w-4' />
			case 'publish':
				return <CheckCircle2 className='h-4 w-4' />
			case 'join':
				return <Users className='h-4 w-4' />
			case 'submit':
				return <Activity className='h-4 w-4' />
			case 'question':
				return <BookOpen className='h-4 w-4' />
			default:
				return <Activity className='h-4 w-4' />
		}
	}

	const getActivityColor = (type: string) => {
		switch (type) {
			case 'exam':
				return 'bg-blue-100 text-blue-700 dark:bg-blue-950 dark:text-blue-400'
			case 'publish':
				return 'bg-green-100 text-green-700 dark:bg-green-950 dark:text-green-400'
			case 'join':
				return 'bg-purple-100 text-purple-700 dark:bg-purple-950 dark:text-purple-400'
			case 'submit':
				return 'bg-orange-100 text-orange-700 dark:bg-orange-950 dark:text-orange-400'
			case 'question':
				return 'bg-yellow-100 text-yellow-700 dark:bg-yellow-950 dark:text-yellow-400'
			default:
				return 'bg-gray-100 text-gray-700 dark:bg-gray-950 dark:text-gray-400'
		}
	}

	return (
		<div className='p-6 space-y-6 max-w-[1600px] mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div className='space-y-2'>
					<h1 className='text-3xl font-bold tracking-tight'>
						{t('dashboard.welcome', { name: user?.firstName || 'Admin' })}
					</h1>
					<p className='text-muted-foreground'>{t('dashboard.subtitle')}</p>
				</div>
				<div className='flex gap-2'>
					<Button
						variant='outline'
						onClick={() => navigate('/dashboard/subjects')}
					>
						<BookOpen className='h-4 w-4 mr-2' />
						{t('dashboard.manage_subjects')}
					</Button>
					<Button onClick={() => navigate('/app/exams')}>
						<FileText className='h-4 w-4 mr-2' />
						{t('dashboard.view_all_exams')}
					</Button>
				</div>
			</div>

			{/* Stats Grid */}
			<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-4'>
				{isLoadingStats
					? Array.from({ length: 4 }).map((_, index) => (
							<Card key={index} className='hover:shadow-md transition-shadow'>
								<CardHeader className='flex flex-row items-center justify-between pb-2'>
									<Skeleton className='h-4 w-24' />
									<Skeleton className='h-10 w-10 rounded-lg' />
								</CardHeader>
								<CardContent>
									<Skeleton className='h-8 w-16 mb-2' />
									<Skeleton className='h-3 w-32' />
								</CardContent>
							</Card>
						))
					: stats.map(stat => {
							const Icon = stat.icon
							return (
								<Card
									key={stat.title}
									className='hover:shadow-md transition-shadow'
								>
									<CardHeader className='flex flex-row items-center justify-between pb-2'>
										<CardTitle className='text-sm font-medium text-muted-foreground'>
											{stat.title}
										</CardTitle>
										<div className={`p-2 rounded-lg ${stat.bgColor}`}>
											<Icon className={`h-4 w-4 ${stat.color}`} />
										</div>
									</CardHeader>
									<CardContent>
										<div className='text-2xl font-bold'>{stat.value}</div>
										<div className='flex items-center text-xs mt-1'>
											{stat.trend === 'up' ? (
												<ArrowUpRight className='h-3 w-3 text-green-600 mr-1' />
											) : (
												<ArrowDownRight className='h-3 w-3 text-red-600 mr-1' />
											)}
											<span
												className={
													stat.trend === 'up'
														? 'text-green-600'
														: 'text-red-600'
												}
											>
												{stat.change}
											</span>
											<span className='text-muted-foreground ml-1'>
												{t('label.from_last_month', 'from last month')}
											</span>
										</div>
									</CardContent>
								</Card>
							)
						})}
			</div>

			{/* Main Content Grid */}
			<div className='grid gap-6 lg:grid-cols-7'>
				{/* Left Column - Activity & Subjects */}
				<div className='lg:col-span-4 space-y-6'>
					{/* Recent Activity */}
					<Card>
						<CardHeader>
							<CardTitle className='flex items-center justify-between'>
								<span>{t('dashboard.recent_activities')}</span>
								<Button variant='ghost' size='sm'>
									{t('action.view_all')}
								</Button>
							</CardTitle>
						</CardHeader>
						<CardContent>
							<div className='space-y-4'>
								{recentActivities.map(activity => (
									<div
										key={activity.id}
										className='flex items-start gap-4 pb-4 border-b last:border-0 last:pb-0'
									>
										<div
											className={`p-2 rounded-lg ${getActivityColor(activity.type)}`}
										>
											{getActivityIcon(activity.type)}
										</div>
										<div className='flex-1 space-y-1'>
											<p className='text-sm'>
												<span className='font-medium'>{activity.user}</span>{' '}
												<span className='text-muted-foreground'>
													{activity.action}
												</span>{' '}
												<span className='font-medium'>{activity.target}</span>
											</p>
											<div className='flex items-center text-xs text-muted-foreground'>
												<Clock className='h-3 w-3 mr-1' />
												{activity.time}
											</div>
										</div>
									</div>
								))}
							</div>
						</CardContent>
					</Card>

					{/* Top Subjects */}
					<Card>
						<CardHeader>
							<CardTitle>{t('dashboard.top_subjects')}</CardTitle>
						</CardHeader>
						<CardContent>
							<Table>
								<TableHeader>
									<TableRow>
										<TableHead>{t('label.subject', 'Subject')}</TableHead>
										<TableHead className='text-right'>
											{t('label.exams', 'Exams')}
										</TableHead>
										<TableHead className='text-right'>
											{t('label.questions', 'Questions')}
										</TableHead>
										<TableHead className='text-right'>
											{t('label.students', 'Students')}
										</TableHead>
										<TableHead className='w-12'></TableHead>
									</TableRow>
								</TableHeader>
								<TableBody>
									{topSubjects.map(subject => (
										<TableRow key={subject.name}>
											<TableCell className='font-medium'>
												{subject.name}
											</TableCell>
											<TableCell className='text-right'>
												{subject.exams}
											</TableCell>
											<TableCell className='text-right'>
												{subject.questions}
											</TableCell>
											<TableCell className='text-right'>
												{subject.students}
											</TableCell>
											<TableCell>
												<DropdownMenu>
													<DropdownMenuTrigger asChild>
														<Button variant='ghost' size='icon'>
															<MoreVertical className='h-4 w-4' />
														</Button>
													</DropdownMenuTrigger>
													<DropdownMenuContent align='end'>
														<DropdownMenuItem>
															{t('action.view_details')}
														</DropdownMenuItem>
														<DropdownMenuItem>
															{t('action.edit', 'Edit')}
														</DropdownMenuItem>
														<DropdownMenuItem>
															{t('action.view', 'View')} {t('label.exams')}
														</DropdownMenuItem>
													</DropdownMenuContent>
												</DropdownMenu>
											</TableCell>
										</TableRow>
									))}
								</TableBody>
							</Table>
						</CardContent>
					</Card>
				</div>

				{/* Right Column - System Health & Quick Actions */}
				<div className='lg:col-span-3 space-y-6'>
					{/* System Health */}
					<Card>
						<CardHeader>
							<CardTitle className='flex items-center'>
								<Activity className='h-5 w-5 mr-2' />
								{t('dashboard.system_health')}
							</CardTitle>
						</CardHeader>
						<CardContent className='space-y-4'>
							{systemHealth.map(metric => (
								<div key={metric.name} className='space-y-2'>
									<div className='flex items-center justify-between text-sm'>
										<span className='text-muted-foreground'>
											{t(
												`dashboard.${metric.name.toLowerCase().replace(/ /g, '_')}`,
												metric.name
											)}
										</span>
										<div className='flex items-center gap-2'>
											<span className='font-medium'>{metric.value}%</span>
											{metric.status === 'healthy' ? (
												<CheckCircle2 className='h-4 w-4 text-green-600' />
											) : (
												<AlertCircle className='h-4 w-4 text-yellow-600' />
											)}
										</div>
									</div>
									<Progress
										value={metric.value}
										className={
											metric.status === 'healthy'
												? 'h-2 [&>[data-slot=progress-indicator]]:bg-green-600'
												: 'h-2 [&>[data-slot=progress-indicator]]:bg-yellow-600'
										}
									/>
								</div>
							))}
						</CardContent>
					</Card>

					{/* Quick Actions */}
					<Card>
						<CardHeader>
							<CardTitle>{t('label.quick_actions', 'Quick Actions')}</CardTitle>
						</CardHeader>
						<CardContent className='space-y-2'>
							<Button
								variant='outline'
								className='w-full justify-start'
								onClick={() => navigate('/dashboard/subjects')}
							>
								<BookOpen className='h-4 w-4 mr-2' />
								{t('dashboard.manage_subjects')}
							</Button>
							<Button
								variant='outline'
								className='w-full justify-start'
								onClick={() => navigate('/app/exams/create')}
							>
								<FileText className='h-4 w-4 mr-2' />
								{t('action.create', 'Create')} {t('label.exam', 'Exam')}
							</Button>
							<Button
								variant='outline'
								className='w-full justify-start'
								onClick={() => navigate('/app/exams')}
							>
								<TrendingUp className='h-4 w-4 mr-2' />
								{t('action.view', 'View')} {t('label.analytics', 'Analytics')}
							</Button>
							<Button
								variant='outline'
								className='w-full justify-start'
								onClick={() => navigate('/dashboard/users')}
							>
								<Users className='h-4 w-4 mr-2' />
								{t('action.manage')} {t('label.users', 'Users')}
							</Button>
						</CardContent>
					</Card>

					{/* Quick Stats */}
					<Card>
						<CardHeader>
							<CardTitle>
								{t('dashboard.todays_overview', "Today's Overview")}
							</CardTitle>
						</CardHeader>
						<CardContent className='space-y-3'>
							<div className='flex items-center justify-between'>
								<span className='text-sm text-muted-foreground'>
									{t('label.exams_created', 'Exams Created')}
								</span>
								<Badge variant='secondary'>0</Badge>
							</div>
							<div className='flex items-center justify-between'>
								<span className='text-sm text-muted-foreground'>
									{t('label.new_users', 'New Users')}
								</span>
								<Badge variant='secondary'>
									{statistics?.usersCreatedLast7Days || 0}
								</Badge>
							</div>
							<div className='flex items-center justify-between'>
								<span className='text-sm text-muted-foreground'>
									{t('label.questions_added', 'Questions Added')}
								</span>
								<Badge variant='secondary'>0</Badge>
							</div>
							<div className='flex items-center justify-between'>
								<span className='text-sm text-muted-foreground'>
									{t('label.verified_users', 'Verified Users')}
								</span>
								<Badge variant='secondary'>
									{statistics?.verifiedUsers || 0}
								</Badge>
							</div>
						</CardContent>
					</Card>
				</div>
			</div>
		</div>
	)
}

export default DashboardPage
