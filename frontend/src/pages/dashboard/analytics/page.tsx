import React, { useState } from 'react'
import {
	TrendingUp,
	Users,
	BookOpen,
	Activity,
	Calendar,
	ArrowUpRight,
	ArrowDownRight,
} from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
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

// Mock data for charts
const userGrowthData = [
	{ month: 'Jan', users: 1200, active: 980, newUsers: 145 },
	{ month: 'Feb', users: 1450, active: 1180, newUsers: 250 },
	{ month: 'Mar', users: 1680, active: 1350, newUsers: 230 },
	{ month: 'Apr', users: 1920, active: 1560, newUsers: 240 },
	{ month: 'May', users: 2180, active: 1780, newUsers: 260 },
	{ month: 'Jun', users: 2450, active: 2010, newUsers: 270 },
	{ month: 'Jul', users: 2680, active: 2190, newUsers: 230 },
	{ month: 'Aug', users: 2847, active: 2340, newUsers: 167 },
]

const examPerformanceData = [
	{ month: 'Jan', avgScore: 72, submissions: 156 },
	{ month: 'Feb', avgScore: 75, submissions: 189 },
	{ month: 'Mar', avgScore: 78, submissions: 201 },
	{ month: 'Apr', avgScore: 76, submissions: 234 },
	{ month: 'May', avgScore: 80, submissions: 267 },
	{ month: 'Jun', avgScore: 82, submissions: 289 },
	{ month: 'Jul', avgScore: 81, submissions: 312 },
	{ month: 'Aug', avgScore: 84, submissions: 345 },
]

const roleDistributionData = [
	{ name: 'Students', value: 2691, color: '#3b82f6' },
	{ name: 'Teachers', value: 156, color: '#8b5cf6' },
	{ name: 'Admins', value: 12, color: '#ef4444' },
]

const activityData = [
	{ day: 'Mon', logins: 1234, examsTaken: 89, questionsCreated: 45 },
	{ day: 'Tue', logins: 1456, examsTaken: 102, questionsCreated: 52 },
	{ day: 'Wed', logins: 1389, examsTaken: 95, questionsCreated: 48 },
	{ day: 'Thu', logins: 1567, examsTaken: 112, questionsCreated: 61 },
	{ day: 'Fri', logins: 1678, examsTaken: 125, questionsCreated: 58 },
	{ day: 'Sat', logins: 987, examsTaken: 67, questionsCreated: 32 },
	{ day: 'Sun', logins: 845, examsTaken: 54, questionsCreated: 28 },
]

const stats = [
	{
		title: 'Total Revenue',
		value: '$45,231',
		change: '+20.1%',
		trend: 'up',
		icon: TrendingUp,
		color: 'text-green-600',
		bgColor: 'bg-green-100 dark:bg-green-950',
	},
	{
		title: 'Active Users',
		value: '2,340',
		change: '+12.5%',
		trend: 'up',
		icon: Users,
		color: 'text-blue-600',
		bgColor: 'bg-blue-100 dark:bg-blue-950',
	},
	{
		title: 'Completion Rate',
		value: '84.2%',
		change: '+4.3%',
		trend: 'up',
		icon: Activity,
		color: 'text-purple-600',
		bgColor: 'bg-purple-100 dark:bg-purple-950',
	},
	{
		title: 'Avg. Score',
		value: '84/100',
		change: '-2.1%',
		trend: 'down',
		icon: BookOpen,
		color: 'text-orange-600',
		bgColor: 'bg-orange-100 dark:bg-orange-950',
	},
]

const AnalyticsPage = (): React.ReactElement => {
	const [timeRange, setTimeRange] = useState('7d')

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
							{entry.name}: {entry.value}
						</p>
					))}
				</div>
			)
		}
		return null
	}

	return (
		<div className='space-y-6 p-6'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div>
					<h1 className='text-3xl font-bold tracking-tight'>Analytics</h1>
					<p className='text-muted-foreground'>
						Platform insights and performance metrics
					</p>
				</div>
				<Select value={timeRange} onValueChange={setTimeRange}>
					<SelectTrigger className='w-[180px]'>
						<Calendar className='mr-2 h-4 w-4' />
						<SelectValue placeholder='Select time range' />
					</SelectTrigger>
					<SelectContent>
						<SelectItem value='7d'>Last 7 days</SelectItem>
						<SelectItem value='30d'>Last 30 days</SelectItem>
						<SelectItem value='90d'>Last 90 days</SelectItem>
						<SelectItem value='1y'>Last year</SelectItem>
					</SelectContent>
				</Select>
			</div>

			{/* Stats Cards */}
			<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-4'>
				{stats.map((stat, index) => {
					const Icon = stat.icon
					const TrendIcon = stat.trend === 'up' ? ArrowUpRight : ArrowDownRight
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
								<div className='text-2xl font-bold'>{stat.value}</div>
								<div className='flex items-center gap-1 mt-1'>
									<TrendIcon
										className={`h-4 w-4 ${
											stat.trend === 'up' ? 'text-green-600' : 'text-red-600'
										}`}
									/>
									<p
										className={`text-xs ${
											stat.trend === 'up' ? 'text-green-600' : 'text-red-600'
										}`}
									>
										{stat.change}
									</p>
									<p className='text-xs text-muted-foreground'>
										from last period
									</p>
								</div>
							</CardContent>
						</Card>
					)
				})}
			</div>

			{/* Charts Tabs */}
			<Tabs defaultValue='overview' className='space-y-4'>
				<TabsList>
					<TabsTrigger value='overview'>Overview</TabsTrigger>
					<TabsTrigger value='users'>Users</TabsTrigger>
					<TabsTrigger value='exams'>Exams</TabsTrigger>
					<TabsTrigger value='activity'>Activity</TabsTrigger>
				</TabsList>

				<TabsContent value='overview' className='space-y-4'>
					<div className='grid gap-4 md:grid-cols-7'>
						{/* User Growth Chart */}
						<Card className='md:col-span-4'>
							<CardHeader>
								<CardTitle>User Growth</CardTitle>
							</CardHeader>
							<CardContent>
								<ResponsiveContainer width='100%' height={350}>
									<LineChart
										data={userGrowthData}
										margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
									>
										<CartesianGrid strokeDasharray='3 3' />
										<XAxis dataKey='month' />
										<YAxis />
										<Tooltip content={<CustomTooltip />} />
										<Legend />
										<Line
											type='monotone'
											dataKey='users'
											stroke='#3b82f6'
											strokeWidth={2}
											name='Total Users'
										/>
										<Line
											type='monotone'
											dataKey='active'
											stroke='#10b981'
											strokeWidth={2}
											name='Active Users'
										/>
										<Line
											type='monotone'
											dataKey='newUsers'
											stroke='#8b5cf6'
											strokeWidth={2}
											name='New Users'
										/>
									</LineChart>
								</ResponsiveContainer>
							</CardContent>
						</Card>

						{/* Role Distribution Pie Chart */}
						<Card className='md:col-span-3'>
							<CardHeader>
								<CardTitle>User Distribution</CardTitle>
							</CardHeader>
							<CardContent>
								<ResponsiveContainer width='100%' height={350}>
									<PieChart>
										<Pie
											data={roleDistributionData}
											cx='50%'
											cy='50%'
											labelLine={false}
											label={entry => `${entry.name}: ${entry.value}`}
											outerRadius={100}
											fill='#8884d8'
											dataKey='value'
										>
											{roleDistributionData.map((entry, index) => (
												<Cell key={`cell-${index}`} fill={entry.color} />
											))}
										</Pie>
										<Tooltip />
									</PieChart>
								</ResponsiveContainer>
							</CardContent>
						</Card>
					</div>

					{/* Activity Bar Chart */}
					<Card>
						<CardHeader>
							<CardTitle>Weekly Activity</CardTitle>
						</CardHeader>
						<CardContent>
							<ResponsiveContainer width='100%' height={350}>
								<BarChart
									data={activityData}
									margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
								>
									<CartesianGrid strokeDasharray='3 3' />
									<XAxis dataKey='day' />
									<YAxis />
									<Tooltip content={<CustomTooltip />} />
									<Legend />
									<Bar dataKey='logins' fill='#3b82f6' name='Logins' />
									<Bar dataKey='examsTaken' fill='#10b981' name='Exams Taken' />
									<Bar
										dataKey='questionsCreated'
										fill='#8b5cf6'
										name='Questions Created'
									/>
								</BarChart>
							</ResponsiveContainer>
						</CardContent>
					</Card>
				</TabsContent>

				<TabsContent value='users' className='space-y-4'>
					<Card>
						<CardHeader>
							<CardTitle>User Metrics</CardTitle>
						</CardHeader>
						<CardContent>
							<ResponsiveContainer width='100%' height={400}>
								<LineChart
									data={userGrowthData}
									margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
								>
									<CartesianGrid strokeDasharray='3 3' />
									<XAxis dataKey='month' />
									<YAxis />
									<Tooltip content={<CustomTooltip />} />
									<Legend />
									<Line
										type='monotone'
										dataKey='users'
										stroke='#3b82f6'
										strokeWidth={2}
										name='Total Users'
									/>
									<Line
										type='monotone'
										dataKey='active'
										stroke='#10b981'
										strokeWidth={2}
										name='Active Users'
									/>
									<Line
										type='monotone'
										dataKey='newUsers'
										stroke='#8b5cf6'
										strokeWidth={2}
										name='New Users'
									/>
								</LineChart>
							</ResponsiveContainer>
						</CardContent>
					</Card>
				</TabsContent>

				<TabsContent value='exams' className='space-y-4'>
					<Card>
						<CardHeader>
							<CardTitle>Exam Performance Trends</CardTitle>
						</CardHeader>
						<CardContent>
							<ResponsiveContainer width='100%' height={400}>
								<BarChart
									data={examPerformanceData}
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
										dataKey='avgScore'
										fill='#3b82f6'
										name='Average Score'
									/>
									<Bar
										yAxisId='right'
										dataKey='submissions'
										fill='#10b981'
										name='Submissions'
									/>
								</BarChart>
							</ResponsiveContainer>
						</CardContent>
					</Card>
				</TabsContent>

				<TabsContent value='activity' className='space-y-4'>
					<Card>
						<CardHeader>
							<CardTitle>Daily Activity Breakdown</CardTitle>
						</CardHeader>
						<CardContent>
							<ResponsiveContainer width='100%' height={400}>
								<BarChart
									data={activityData}
									margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
								>
									<CartesianGrid strokeDasharray='3 3' />
									<XAxis dataKey='day' />
									<YAxis />
									<Tooltip content={<CustomTooltip />} />
									<Legend />
									<Bar dataKey='logins' fill='#3b82f6' name='Logins' />
									<Bar dataKey='examsTaken' fill='#10b981' name='Exams Taken' />
									<Bar
										dataKey='questionsCreated'
										fill='#8b5cf6'
										name='Questions Created'
									/>
								</BarChart>
							</ResponsiveContainer>
						</CardContent>
					</Card>
				</TabsContent>
			</Tabs>
		</div>
	)
}

export default AnalyticsPage
