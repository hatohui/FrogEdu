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
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Skeleton } from '@/components/ui/skeleton'
import {
	ChartContainer,
	ChartTooltip,
	ChartTooltipContent,
	ChartLegend,
	ChartLegendContent,
	type ChartConfig,
} from '@/components/ui/chart'
import {
	Area,
	AreaChart,
	CartesianGrid,
	XAxis,
	YAxis,
	Pie,
	PieChart,
	Bar,
	BarChart,
	Label,
} from 'recharts'
import { useUserDashboardStats, useUserStatistics } from '@/hooks/useUsers'
import { useSubscriptionDashboardStats } from '@/hooks/useSubscription'

const userGrowthConfig = {
	count: {
		label: 'Daily Registrations',
		color: 'hsl(217, 91%, 60%)',
	},
} satisfies ChartConfig

const roleChartConfig = {
	count: { label: 'Users' },
	Admin: { label: 'Admin', color: 'hsl(0, 84%, 60%)' },
	Teacher: { label: 'Teacher', color: 'hsl(262, 83%, 58%)' },
	Student: { label: 'Student', color: 'hsl(217, 91%, 60%)' },
} satisfies ChartConfig

const verificationChartConfig = {
	count: { label: 'Users' },
	Verified: { label: 'Verified', color: 'hsl(160, 84%, 39%)' },
	Unverified: { label: 'Unverified', color: 'hsl(38, 92%, 50%)' },
} satisfies ChartConfig

const subscriptionStatusConfig = {
	count: { label: 'Subscriptions' },
	Active: { label: 'Active', color: 'hsl(160, 84%, 39%)' },
	Expired: { label: 'Expired', color: 'hsl(220, 9%, 46%)' },
	Cancelled: { label: 'Cancelled', color: 'hsl(0, 84%, 60%)' },
	Suspended: { label: 'Suspended', color: 'hsl(38, 92%, 50%)' },
} satisfies ChartConfig

const revenueChartConfig = {
	revenue: { label: 'Revenue', color: 'hsl(217, 91%, 60%)' },
	transactions: { label: 'Transactions', color: 'hsl(262, 83%, 58%)' },
} satisfies ChartConfig

const ChartSkeleton = () => (
	<div className='space-y-3'>
		<Skeleton className='h-[350px] w-full' />
	</div>
)

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
			name: item.role,
			value: item.count,
			fill: `var(--color-${item.role})`,
		})) ?? []

	const verificationData =
		userDashboard?.verificationStatus.map(item => ({
			name: item.status,
			value: item.count,
			fill: `var(--color-${item.status})`,
		})) ?? []

	const subscriptionStatusData =
		subStats?.statusDistribution
			.filter(item => item.count > 0)
			.map(item => ({
				name: item.status,
				value: item.count,
				fill: `var(--color-${item.status})`,
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

	const totalUsers = roleDistributionData.reduce(
		(sum, item) => sum + item.value,
		0
	)

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
						{/* User Growth Area Chart */}
						<Card className='md:col-span-4'>
							<CardHeader>
								<CardTitle className='flex items-center gap-2'>
									<Users className='h-5 w-5' />
									{t('analytics.user_growth')}
								</CardTitle>
								<CardDescription>
									{t('dashboard.new_registrations_trend')}
								</CardDescription>
							</CardHeader>
							<CardContent>
								{isLoadingUserDashboard ? (
									<ChartSkeleton />
								) : (
									<ChartContainer
										config={userGrowthConfig}
										className='h-[350px] w-full'
									>
										<AreaChart
											accessibilityLayer
											data={growthChartData}
											margin={{ top: 10, right: 10, left: 0, bottom: 0 }}
										>
											<CartesianGrid vertical={false} />
											<XAxis
												dataKey='date'
												tickLine={false}
												axisLine={false}
												tickMargin={8}
												interval='preserveStartEnd'
												tick={{ fontSize: 12 }}
											/>
											<YAxis
												allowDecimals={false}
												tickLine={false}
												axisLine={false}
											/>
											<ChartTooltip content={<ChartTooltipContent />} />
											<defs>
												<linearGradient
													id='fillGrowth'
													x1='0'
													y1='0'
													x2='0'
													y2='1'
												>
													<stop
														offset='5%'
														stopColor='var(--color-count)'
														stopOpacity={0.8}
													/>
													<stop
														offset='95%'
														stopColor='var(--color-count)'
														stopOpacity={0.1}
													/>
												</linearGradient>
											</defs>
											<Area
												dataKey='count'
												type='natural'
												fill='url(#fillGrowth)'
												stroke='var(--color-count)'
												strokeWidth={2}
											/>
										</AreaChart>
									</ChartContainer>
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
								<CardDescription>
									{t('dashboard.users_by_role')}
								</CardDescription>
							</CardHeader>
							<CardContent>
								{isLoadingUserDashboard ? (
									<ChartSkeleton />
								) : (
									<ChartContainer
										config={roleChartConfig}
										className='mx-auto aspect-square h-[320px]'
									>
										<PieChart>
											<ChartTooltip
												content={
													<ChartTooltipContent nameKey='name' hideLabel />
												}
											/>
											<Pie
												data={roleDistributionData}
												dataKey='value'
												nameKey='name'
												innerRadius={60}
												strokeWidth={5}
											>
												<Label
													content={({ viewBox }) => {
														if (viewBox && 'cx' in viewBox && 'cy' in viewBox) {
															return (
																<text
																	x={viewBox.cx}
																	y={viewBox.cy}
																	textAnchor='middle'
																	dominantBaseline='middle'
																>
																	<tspan
																		x={viewBox.cx}
																		y={viewBox.cy}
																		className='fill-foreground text-3xl font-bold'
																	>
																		{totalUsers.toLocaleString()}
																	</tspan>
																	<tspan
																		x={viewBox.cx}
																		y={(viewBox.cy || 0) + 24}
																		className='fill-muted-foreground text-sm'
																	>
																		{t('analytics.users')}
																	</tspan>
																</text>
															)
														}
													}}
												/>
											</Pie>
											<ChartLegend
												content={<ChartLegendContent nameKey='name' />}
											/>
										</PieChart>
									</ChartContainer>
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
								<CardDescription>
									{t('dashboard.email_verification_breakdown')}
								</CardDescription>
							</CardHeader>
							<CardContent>
								{isLoadingUserDashboard ? (
									<ChartSkeleton />
								) : (
									<ChartContainer
										config={verificationChartConfig}
										className='mx-auto aspect-square h-[300px]'
									>
										<PieChart>
											<ChartTooltip
												content={
													<ChartTooltipContent nameKey='name' hideLabel />
												}
											/>
											<Pie
												data={verificationData}
												dataKey='value'
												nameKey='name'
												innerRadius={50}
												strokeWidth={5}
											>
												<Label
													content={({ viewBox }) => {
														if (viewBox && 'cx' in viewBox && 'cy' in viewBox) {
															const verified = verificationData.find(
																d => d.name === 'Verified'
															)
															const pct =
																totalUsers > 0
																	? Math.round(
																			((verified?.value ?? 0) / totalUsers) *
																				100
																		)
																	: 0
															return (
																<text
																	x={viewBox.cx}
																	y={viewBox.cy}
																	textAnchor='middle'
																	dominantBaseline='middle'
																>
																	<tspan
																		x={viewBox.cx}
																		y={viewBox.cy}
																		className='fill-foreground text-3xl font-bold'
																	>
																		{pct}%
																	</tspan>
																	<tspan
																		x={viewBox.cx}
																		y={(viewBox.cy || 0) + 24}
																		className='fill-muted-foreground text-sm'
																	>
																		{t('analytics.verified')}
																	</tspan>
																</text>
															)
														}
													}}
												/>
											</Pie>
											<ChartLegend
												content={<ChartLegendContent nameKey='name' />}
											/>
										</PieChart>
									</ChartContainer>
								)}
							</CardContent>
						</Card>

						<Card>
							<CardHeader>
								<CardTitle className='flex items-center gap-2'>
									<CreditCard className='h-5 w-5' />
									{t('analytics.subscription_status')}
								</CardTitle>
								<CardDescription>
									{t('dashboard.subscription_breakdown')}
								</CardDescription>
							</CardHeader>
							<CardContent>
								{isLoadingSubStats ? (
									<ChartSkeleton />
								) : subscriptionStatusData.length === 0 ? (
									<div className='flex h-[300px] items-center justify-center text-muted-foreground'>
										{t('analytics.no_data')}
									</div>
								) : (
									<ChartContainer
										config={subscriptionStatusConfig}
										className='mx-auto aspect-square h-[300px]'
									>
										<PieChart>
											<ChartTooltip
												content={
													<ChartTooltipContent nameKey='name' hideLabel />
												}
											/>
											<Pie
												data={subscriptionStatusData}
												dataKey='value'
												nameKey='name'
												innerRadius={50}
												strokeWidth={5}
											>
												<Label
													content={({ viewBox }) => {
														if (viewBox && 'cx' in viewBox && 'cy' in viewBox) {
															const total = subscriptionStatusData.reduce(
																(sum, item) => sum + item.value,
																0
															)
															return (
																<text
																	x={viewBox.cx}
																	y={viewBox.cy}
																	textAnchor='middle'
																	dominantBaseline='middle'
																>
																	<tspan
																		x={viewBox.cx}
																		y={viewBox.cy}
																		className='fill-foreground text-3xl font-bold'
																	>
																		{total.toLocaleString()}
																	</tspan>
																	<tspan
																		x={viewBox.cx}
																		y={(viewBox.cy || 0) + 24}
																		className='fill-muted-foreground text-sm'
																	>
																		{t('analytics.total_subscriptions')}
																	</tspan>
																</text>
															)
														}
													}}
												/>
											</Pie>
											<ChartLegend
												content={<ChartLegendContent nameKey='name' />}
											/>
										</PieChart>
									</ChartContainer>
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
							<CardDescription>
								{t('dashboard.new_registrations_trend')}
							</CardDescription>
						</CardHeader>
						<CardContent>
							{isLoadingUserDashboard ? (
								<ChartSkeleton />
							) : (
								<ChartContainer
									config={userGrowthConfig}
									className='h-[400px] w-full'
								>
									<AreaChart
										accessibilityLayer
										data={growthChartData}
										margin={{ top: 10, right: 10, left: 0, bottom: 0 }}
									>
										<CartesianGrid vertical={false} />
										<XAxis
											dataKey='date'
											tickLine={false}
											axisLine={false}
											tickMargin={8}
											tick={{ fontSize: 12 }}
										/>
										<YAxis
											allowDecimals={false}
											tickLine={false}
											axisLine={false}
										/>
										<ChartTooltip content={<ChartTooltipContent />} />
										<defs>
											<linearGradient
												id='fillGrowthUsers'
												x1='0'
												y1='0'
												x2='0'
												y2='1'
											>
												<stop
													offset='5%'
													stopColor='var(--color-count)'
													stopOpacity={0.8}
												/>
												<stop
													offset='95%'
													stopColor='var(--color-count)'
													stopOpacity={0.1}
												/>
											</linearGradient>
										</defs>
										<Area
											dataKey='count'
											type='natural'
											fill='url(#fillGrowthUsers)'
											stroke='var(--color-count)'
											strokeWidth={2}
										/>
									</AreaChart>
								</ChartContainer>
							)}
						</CardContent>
					</Card>

					<div className='grid gap-4 md:grid-cols-2'>
						<Card>
							<CardHeader>
								<CardTitle>{t('analytics.role_distribution')}</CardTitle>
								<CardDescription>
									{t('dashboard.users_by_role')}
								</CardDescription>
							</CardHeader>
							<CardContent>
								{isLoadingUserDashboard ? (
									<ChartSkeleton />
								) : (
									<ChartContainer
										config={roleChartConfig}
										className='mx-auto aspect-square h-[320px]'
									>
										<PieChart>
											<ChartTooltip
												content={
													<ChartTooltipContent nameKey='name' hideLabel />
												}
											/>
											<Pie
												data={roleDistributionData}
												dataKey='value'
												nameKey='name'
												innerRadius={60}
												strokeWidth={5}
											>
												<Label
													content={({ viewBox }) => {
														if (viewBox && 'cx' in viewBox && 'cy' in viewBox) {
															return (
																<text
																	x={viewBox.cx}
																	y={viewBox.cy}
																	textAnchor='middle'
																	dominantBaseline='middle'
																>
																	<tspan
																		x={viewBox.cx}
																		y={viewBox.cy}
																		className='fill-foreground text-3xl font-bold'
																	>
																		{totalUsers.toLocaleString()}
																	</tspan>
																	<tspan
																		x={viewBox.cx}
																		y={(viewBox.cy || 0) + 24}
																		className='fill-muted-foreground text-sm'
																	>
																		{t('analytics.users')}
																	</tspan>
																</text>
															)
														}
													}}
												/>
											</Pie>
											<ChartLegend
												content={<ChartLegendContent nameKey='name' />}
											/>
										</PieChart>
									</ChartContainer>
								)}
							</CardContent>
						</Card>

						<Card>
							<CardHeader>
								<CardTitle>{t('analytics.verification_status')}</CardTitle>
								<CardDescription>
									{t('dashboard.email_verification_breakdown')}
								</CardDescription>
							</CardHeader>
							<CardContent>
								{isLoadingUserDashboard ? (
									<ChartSkeleton />
								) : (
									<ChartContainer
										config={verificationChartConfig}
										className='mx-auto aspect-square h-[320px]'
									>
										<PieChart>
											<ChartTooltip
												content={
													<ChartTooltipContent nameKey='name' hideLabel />
												}
											/>
											<Pie
												data={verificationData}
												dataKey='value'
												nameKey='name'
												innerRadius={50}
												strokeWidth={5}
											>
												<Label
													content={({ viewBox }) => {
														if (viewBox && 'cx' in viewBox && 'cy' in viewBox) {
															const verified = verificationData.find(
																d => d.name === 'Verified'
															)
															const pct =
																totalUsers > 0
																	? Math.round(
																			((verified?.value ?? 0) / totalUsers) *
																				100
																		)
																	: 0
															return (
																<text
																	x={viewBox.cx}
																	y={viewBox.cy}
																	textAnchor='middle'
																	dominantBaseline='middle'
																>
																	<tspan
																		x={viewBox.cx}
																		y={viewBox.cy}
																		className='fill-foreground text-3xl font-bold'
																	>
																		{pct}%
																	</tspan>
																	<tspan
																		x={viewBox.cx}
																		y={(viewBox.cy || 0) + 24}
																		className='fill-muted-foreground text-sm'
																	>
																		{t('analytics.verified')}
																	</tspan>
																</text>
															)
														}
													}}
												/>
											</Pie>
											<ChartLegend
												content={<ChartLegendContent nameKey='name' />}
											/>
										</PieChart>
									</ChartContainer>
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
							<CardDescription>
								{t('dashboard.revenue_and_transactions')}
							</CardDescription>
						</CardHeader>
						<CardContent>
							{isLoadingSubStats ? (
								<ChartSkeleton />
							) : (
								<ChartContainer
									config={revenueChartConfig}
									className='h-[400px] w-full'
								>
									<BarChart accessibilityLayer data={revenueChartData}>
										<CartesianGrid vertical={false} />
										<XAxis
											dataKey='month'
											tickLine={false}
											tickMargin={10}
											axisLine={false}
										/>
										<ChartTooltip content={<ChartTooltipContent />} />
										<ChartLegend content={<ChartLegendContent />} />
										<Bar
											dataKey='revenue'
											fill='var(--color-revenue)'
											radius={4}
										/>
										<Bar
											dataKey='transactions'
											fill='var(--color-transactions)'
											radius={4}
										/>
									</BarChart>
								</ChartContainer>
							)}
						</CardContent>
					</Card>

					<Card>
						<CardHeader>
							<CardTitle>{t('analytics.subscription_status')}</CardTitle>
							<CardDescription>
								{t('dashboard.subscription_breakdown')}
							</CardDescription>
						</CardHeader>
						<CardContent>
							{isLoadingSubStats ? (
								<ChartSkeleton />
							) : subscriptionStatusData.length === 0 ? (
								<div className='flex h-[350px] items-center justify-center text-muted-foreground'>
									{t('analytics.no_data')}
								</div>
							) : (
								<ChartContainer
									config={subscriptionStatusConfig}
									className='mx-auto aspect-square h-[350px]'
								>
									<PieChart>
										<ChartTooltip
											content={<ChartTooltipContent nameKey='name' hideLabel />}
										/>
										<Pie
											data={subscriptionStatusData}
											dataKey='value'
											nameKey='name'
											innerRadius={60}
											strokeWidth={5}
										>
											<Label
												content={({ viewBox }) => {
													if (viewBox && 'cx' in viewBox && 'cy' in viewBox) {
														const total = subscriptionStatusData.reduce(
															(sum, item) => sum + item.value,
															0
														)
														return (
															<text
																x={viewBox.cx}
																y={viewBox.cy}
																textAnchor='middle'
																dominantBaseline='middle'
															>
																<tspan
																	x={viewBox.cx}
																	y={viewBox.cy}
																	className='fill-foreground text-3xl font-bold'
																>
																	{total.toLocaleString()}
																</tspan>
																<tspan
																	x={viewBox.cx}
																	y={(viewBox.cy || 0) + 24}
																	className='fill-muted-foreground text-sm'
																>
																	{t('analytics.total_subscriptions')}
																</tspan>
															</text>
														)
													}
												}}
											/>
										</Pie>
										<ChartLegend
											content={<ChartLegendContent nameKey='name' />}
										/>
									</PieChart>
								</ChartContainer>
							)}
						</CardContent>
					</Card>
				</TabsContent>
			</Tabs>
		</div>
	)
}

export default AnalyticsPage
