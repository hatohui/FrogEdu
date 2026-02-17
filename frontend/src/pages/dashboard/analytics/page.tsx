import React from 'react'
import {
	TrendingUp,
	Users,
	CreditCard,
	ShieldCheck,
	DollarSign,
	UserCheck,
} from 'lucide-react'
import { useTranslation } from 'react-i18next'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Skeleton } from '@/components/ui/skeleton'
import {
	LineChart,
	Line,
	BarChart,
	Bar,
	XAxis,
	YAxis,
	CartesianGrid,
	Tooltip,
	Legend,
	ResponsiveContainer,
	PieChart,
	Pie,
	Cell,
} from 'recharts'
import { useUserDashboardStats, useUserStatistics } from '@/hooks/useUsers'
import { useSubscriptionDashboardStats } from '@/hooks/useSubscription'

const ROLE_COLORS: Record<string, string> = {
	Admin: '#ef4444',
	Teacher: '#8b5cf6',
	Student: '#3b82f6',
}

const VERIFICATION_COLORS: Record<string, string> = {
	Verified: '#10b981',
	Unverified: '#f59e0b',
}

const SUBSCRIPTION_STATUS_COLORS: Record<string, string> = {
	Active: '#10b981',
	Expired: '#6b7280',
	Cancelled: '#ef4444',
	Suspended: '#f59e0b',
}

const REVENUE_COLOR = '#3b82f6'
const TRANSACTION_COLOR = '#8b5cf6'

const ChartSkeleton = () => (
	<div className='space-y-3'>
		<Skeleton className='h-[350px] w-full' />
	</div>
)

const CustomTooltip = ({
	active,
	payload,
	label,
}: {
	active?: boolean
	payload?: Array<{ name: string; value: number; color: string }>
	label?: string
}) => {
	if (active && payload && payload.length) {
		return (
			<div className='rounded-lg border bg-background p-3 shadow-lg'>
				<p className='font-semibold'>{label}</p>
				{payload.map((entry, index: number) => (
					<p key={index} className='text-sm' style={{ color: entry.color }}>
						{entry.name}:{' '}
						{typeof entry.value === 'number' &&
						entry.name.toLowerCase().includes('revenue')
							? `$${entry.value.toLocaleString()}`
							: entry.value.toLocaleString()}
					</p>
				))}
			</div>
		)
	}
	return null
}

const AnalyticsPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const { data: userStats, isLoading: isLoadingUserStats } = useUserStatistics()
	const { data: userDashboard, isLoading: isLoadingUserDashboard } =
		useUserDashboardStats()
	const { data: subStats, isLoading: isLoadingSubStats } =
		useSubscriptionDashboardStats()

	const isLoading =
		isLoadingUserStats || isLoadingUserDashboard || isLoadingSubStats

	const statCards = [
		{
			title: t('analytics.total_revenue'),
			value: subStats ? `$${subStats.totalRevenue.toLocaleString()}` : '$0',
			icon: DollarSign,
			color: 'text-green-600',
			bgColor: 'bg-green-100 dark:bg-green-950',
		},
		{
			title: t('analytics.total_subscriptions'),
			value: subStats?.totalSubscriptions?.toString() ?? '0',
			icon: CreditCard,
			color: 'text-blue-600',
			bgColor: 'bg-blue-100 dark:bg-blue-950',
		},
		{
			title: t('analytics.active_subscriptions'),
			value: subStats?.activeSubscriptions?.toString() ?? '0',
			icon: TrendingUp,
			color: 'text-purple-600',
			bgColor: 'bg-purple-100 dark:bg-purple-950',
		},
		{
			title: t('analytics.verified_users'),
			value: userStats?.verifiedUsers?.toString() ?? '0',
			icon: UserCheck,
			color: 'text-emerald-600',
			bgColor: 'bg-emerald-100 dark:bg-emerald-950',
		},
	]

	const roleDistributionData =
		userDashboard?.roleDistribution.map(item => ({
			name: t(`roles.${item.role.toLowerCase()}`),
			value: item.count,
			color: ROLE_COLORS[item.role] ?? '#6b7280',
		})) ?? []

	const verificationData =
		userDashboard?.verificationStatus.map(item => ({
			name: t(`analytics.${item.status.toLowerCase()}`),
			value: item.count,
			color: VERIFICATION_COLORS[item.status] ?? '#6b7280',
		})) ?? []

	const subscriptionStatusData =
		subStats?.statusDistribution
			.filter(item => item.count > 0)
			.map(item => ({
				name: t(`analytics.${item.status.toLowerCase()}`),
				value: item.count,
				color: SUBSCRIPTION_STATUS_COLORS[item.status] ?? '#6b7280',
			})) ?? []

	const growthChartData =
		userDashboard?.userGrowthLast30Days.map(item => ({
			date: new Date(item.date).toLocaleDateString(undefined, {
				month: 'short',
				day: 'numeric',
			}),
			count: item.count,
		})) ?? []

	const revenueChartData =
		subStats?.monthlyRevenue.map(item => ({
			month: item.month,
			revenue: item.revenue,
			transactions: item.transactionCount,
		})) ?? []

	return (
		<div className='space-y-6 p-6'>
			{/* Header */}
			<div>
				<h1 className='text-3xl font-bold tracking-tight'>
					{t('analytics.title')}
				</h1>
				<p className='text-muted-foreground'>{t('analytics.subtitle')}</p>
			</div>

			{/* Stats Cards */}
			<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-4'>
				{statCards.map((stat, index) => {
					const Icon = stat.icon
					return (
						<Card key={index}>
							<CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
								<CardTitle className='text-sm font-medium'>
									{stat.title}
								</CardTitle>
								<div className={`rounded-full p-2 ${stat.bgColor}`}>
									<Icon className={`h-4 w-4 ${stat.color}`} />
								</div>
							</CardHeader>
							<CardContent>
								{isLoading ? (
									<Skeleton className='h-8 w-24' />
								) : (
									<div className='text-2xl font-bold'>{stat.value}</div>
								)}
							</CardContent>
						</Card>
					)
				})}
			</div>

			{/* Charts Tabs */}
			<Tabs defaultValue='overview' className='space-y-4'>
				<TabsList>
					<TabsTrigger value='overview'>{t('analytics.overview')}</TabsTrigger>
					<TabsTrigger value='users'>{t('analytics.users')}</TabsTrigger>
					<TabsTrigger value='subscriptions'>
						{t('analytics.subscriptions')}
					</TabsTrigger>
				</TabsList>

				{/* Overview Tab */}
				<TabsContent value='overview' className='space-y-4'>
					<div className='grid gap-4 md:grid-cols-7'>
						{/* User Growth Line Chart */}
						<Card className='md:col-span-4'>
							<CardHeader>
								<CardTitle className='flex items-center gap-2'>
									<Users className='h-5 w-5' />
									{t('analytics.user_growth')}
								</CardTitle>
							</CardHeader>
							<CardContent>
								{isLoadingUserDashboard ? (
									<ChartSkeleton />
								) : (
									<ResponsiveContainer width='100%' height={350}>
										<LineChart
											data={growthChartData}
											margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
										>
											<CartesianGrid strokeDasharray='3 3' />
											<XAxis
												dataKey='date'
												tick={{ fontSize: 12 }}
												interval='preserveStartEnd'
											/>
											<YAxis allowDecimals={false} />
											<Tooltip content={<CustomTooltip />} />
											<Legend />
											<Line
												type='monotone'
												dataKey='count'
												stroke='#3b82f6'
												strokeWidth={2}
												name={t('analytics.daily_registrations')}
												dot={false}
												activeDot={{ r: 4 }}
											/>
										</LineChart>
									</ResponsiveContainer>
								)}
							</CardContent>
						</Card>

						{/* Role Distribution Pie Chart */}
						<Card className='md:col-span-3'>
							<CardHeader>
								<CardTitle className='flex items-center gap-2'>
									<ShieldCheck className='h-5 w-5' />
									{t('analytics.role_distribution')}
								</CardTitle>
							</CardHeader>
							<CardContent>
								{isLoadingUserDashboard ? (
									<ChartSkeleton />
								) : (
									<ResponsiveContainer width='100%' height={350}>
										<PieChart>
											<Pie
												data={roleDistributionData}
												cx='50%'
												cy='50%'
												labelLine={false}
												label={({ name, percent }) =>
													`${name}: ${(percent * 100).toFixed(0)}%`
												}
												outerRadius={100}
												fill='#8884d8'
												dataKey='value'
											>
												{roleDistributionData.map((entry, index) => (
													<Cell key={`cell-${index}`} fill={entry.color} />
												))}
											</Pie>
											<Tooltip />
											<Legend />
										</PieChart>
									</ResponsiveContainer>
								)}
							</CardContent>
						</Card>
					</div>

					{/* Verification Status + Subscription Status */}
					<div className='grid gap-4 md:grid-cols-2'>
						<Card>
							<CardHeader>
								<CardTitle className='flex items-center gap-2'>
									<UserCheck className='h-5 w-5' />
									{t('analytics.verification_status')}
								</CardTitle>
							</CardHeader>
							<CardContent>
								{isLoadingUserDashboard ? (
									<ChartSkeleton />
								) : (
									<ResponsiveContainer width='100%' height={300}>
										<PieChart>
											<Pie
												data={verificationData}
												cx='50%'
												cy='50%'
												labelLine={false}
												label={({ name, percent }) =>
													`${name}: ${(percent * 100).toFixed(0)}%`
												}
												outerRadius={90}
												fill='#8884d8'
												dataKey='value'
											>
												{verificationData.map((entry, index) => (
													<Cell key={`cell-${index}`} fill={entry.color} />
												))}
											</Pie>
											<Tooltip />
											<Legend />
										</PieChart>
									</ResponsiveContainer>
								)}
							</CardContent>
						</Card>

						<Card>
							<CardHeader>
								<CardTitle className='flex items-center gap-2'>
									<CreditCard className='h-5 w-5' />
									{t('analytics.subscription_status')}
								</CardTitle>
							</CardHeader>
							<CardContent>
								{isLoadingSubStats ? (
									<ChartSkeleton />
								) : subscriptionStatusData.length === 0 ? (
									<div className='flex h-[300px] items-center justify-center text-muted-foreground'>
										{t('analytics.no_data')}
									</div>
								) : (
									<ResponsiveContainer width='100%' height={300}>
										<PieChart>
											<Pie
												data={subscriptionStatusData}
												cx='50%'
												cy='50%'
												labelLine={false}
												label={({ name, percent }) =>
													`${name}: ${(percent * 100).toFixed(0)}%`
												}
												outerRadius={90}
												fill='#8884d8'
												dataKey='value'
											>
												{subscriptionStatusData.map((entry, index) => (
													<Cell key={`cell-${index}`} fill={entry.color} />
												))}
											</Pie>
											<Tooltip />
											<Legend />
										</PieChart>
									</ResponsiveContainer>
								)}
							</CardContent>
						</Card>
					</div>
				</TabsContent>

				{/* Users Tab */}
				<TabsContent value='users' className='space-y-4'>
					<Card>
						<CardHeader>
							<CardTitle>{t('analytics.user_growth')}</CardTitle>
						</CardHeader>
						<CardContent>
							{isLoadingUserDashboard ? (
								<ChartSkeleton />
							) : (
								<ResponsiveContainer width='100%' height={400}>
									<LineChart
										data={growthChartData}
										margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
									>
										<CartesianGrid strokeDasharray='3 3' />
										<XAxis dataKey='date' tick={{ fontSize: 12 }} />
										<YAxis allowDecimals={false} />
										<Tooltip content={<CustomTooltip />} />
										<Legend />
										<Line
											type='monotone'
											dataKey='count'
											stroke='#3b82f6'
											strokeWidth={2}
											name={t('analytics.daily_registrations')}
											dot={{ r: 3 }}
											activeDot={{ r: 6 }}
										/>
									</LineChart>
								</ResponsiveContainer>
							)}
						</CardContent>
					</Card>

					<div className='grid gap-4 md:grid-cols-2'>
						<Card>
							<CardHeader>
								<CardTitle>{t('analytics.role_distribution')}</CardTitle>
							</CardHeader>
							<CardContent>
								{isLoadingUserDashboard ? (
									<ChartSkeleton />
								) : (
									<ResponsiveContainer width='100%' height={350}>
										<PieChart>
											<Pie
												data={roleDistributionData}
												cx='50%'
												cy='50%'
												labelLine={false}
												label={({ name, percent }) =>
													`${name}: ${(percent * 100).toFixed(0)}%`
												}
												outerRadius={100}
												fill='#8884d8'
												dataKey='value'
											>
												{roleDistributionData.map((entry, index) => (
													<Cell key={`cell-${index}`} fill={entry.color} />
												))}
											</Pie>
											<Tooltip />
											<Legend />
										</PieChart>
									</ResponsiveContainer>
								)}
							</CardContent>
						</Card>

						<Card>
							<CardHeader>
								<CardTitle>{t('analytics.verification_status')}</CardTitle>
							</CardHeader>
							<CardContent>
								{isLoadingUserDashboard ? (
									<ChartSkeleton />
								) : (
									<ResponsiveContainer width='100%' height={350}>
										<PieChart>
											<Pie
												data={verificationData}
												cx='50%'
												cy='50%'
												labelLine={false}
												label={({ name, percent }) =>
													`${name}: ${(percent * 100).toFixed(0)}%`
												}
												outerRadius={100}
												fill='#8884d8'
												dataKey='value'
											>
												{verificationData.map((entry, index) => (
													<Cell key={`cell-${index}`} fill={entry.color} />
												))}
											</Pie>
											<Tooltip />
											<Legend />
										</PieChart>
									</ResponsiveContainer>
								)}
							</CardContent>
						</Card>
					</div>
				</TabsContent>

				{/* Subscriptions Tab */}
				<TabsContent value='subscriptions' className='space-y-4'>
					<Card>
						<CardHeader>
							<CardTitle>{t('analytics.monthly_revenue')}</CardTitle>
						</CardHeader>
						<CardContent>
							{isLoadingSubStats ? (
								<ChartSkeleton />
							) : (
								<ResponsiveContainer width='100%' height={400}>
									<BarChart
										data={revenueChartData}
										margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
									>
										<CartesianGrid strokeDasharray='3 3' />
										<XAxis dataKey='month' />
										<YAxis yAxisId='left' orientation='left' />
										<YAxis yAxisId='right' orientation='right' />
										<Tooltip content={<CustomTooltip />} />
										<Legend />
										<Bar
											yAxisId='left'
											dataKey='revenue'
											fill={REVENUE_COLOR}
											name={t('analytics.revenue')}
										/>
										<Bar
											yAxisId='right'
											dataKey='transactions'
											fill={TRANSACTION_COLOR}
											name={t('analytics.transactions')}
										/>
									</BarChart>
								</ResponsiveContainer>
							)}
						</CardContent>
					</Card>

					<Card>
						<CardHeader>
							<CardTitle>{t('analytics.subscription_status')}</CardTitle>
						</CardHeader>
						<CardContent>
							{isLoadingSubStats ? (
								<ChartSkeleton />
							) : subscriptionStatusData.length === 0 ? (
								<div className='flex h-[350px] items-center justify-center text-muted-foreground'>
									{t('analytics.no_data')}
								</div>
							) : (
								<ResponsiveContainer width='100%' height={350}>
									<PieChart>
										<Pie
											data={subscriptionStatusData}
											cx='50%'
											cy='50%'
											labelLine={false}
											label={({ name, percent }) =>
												`${name}: ${(percent * 100).toFixed(0)}%`
											}
											outerRadius={120}
											fill='#8884d8'
											dataKey='value'
										>
											{subscriptionStatusData.map((entry, index) => (
												<Cell key={`cell-${index}`} fill={entry.color} />
											))}
										</Pie>
										<Tooltip />
										<Legend />
									</PieChart>
								</ResponsiveContainer>
							)}
						</CardContent>
					</Card>
				</TabsContent>
			</Tabs>
		</div>
	)
}

export default AnalyticsPage
