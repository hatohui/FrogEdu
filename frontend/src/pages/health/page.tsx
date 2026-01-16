import React from 'react'
import { useQueries } from '@tanstack/react-query'
import axios from 'axios'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Alert, AlertDescription } from '@/components/ui/alert'
import { Button } from '@/components/ui/button'
import {
	CheckCircle,
	XCircle,
	Loader2,
	RefreshCcw,
	Server,
	Activity,
	AlertTriangle,
} from 'lucide-react'

// Health check configuration for all services
const healthEndpoints = [
	{
		name: 'Content Service',
		url: 'https://api.frogedu.org/api/contents/health',
		description: 'Content management and library service',
		critical: true,
	},
	{
		name: 'User Service',
		url: 'https://api.frogedu.org/api/users/health',
		description: 'User authentication and profile service',
		critical: true,
	},
	{
		name: 'Assessment Service',
		url: 'https://api.frogedu.org/api/assessments/health',
		description: 'Quiz and assessment engine',
		critical: false,
	},
	{
		name: 'AI Service',
		url: 'https://api.frogedu.org/api/ai/health',
		description: 'AI tutoring and smart assistance',
		critical: false,
	},
	{
		name: 'API Gateway',
		url: 'https://api.frogedu.org/api',
		description: 'Main API Gateway routing',
		critical: true,
	},
]

interface HealthStatus {
	status: 'healthy' | 'unhealthy' | 'loading' | 'unknown'
	responseTime?: number
	timestamp?: string
	service?: string
	error?: string
}

const HealthPage = (): React.JSX.Element => {
	const [lastRefresh, setLastRefresh] = React.useState<Date>(new Date())

	// Individual health check queries using useQueries to avoid hooks in loops
	const healthQueries = useQueries({
		queries: healthEndpoints.map(endpoint => ({
			queryKey: ['health', endpoint.name],
			queryFn: async (): Promise<HealthStatus> => {
				const startTime = Date.now()

				try {
					const response = await axios.get(endpoint.url, {
						timeout: 10000,
						validateStatus: status => status < 500, // Accept anything below 500 as potentially healthy
					})

					const responseTime = Date.now() - startTime

					// Check response structure
					if (response.status === 200) {
						return {
							status: 'healthy',
							responseTime,
							timestamp: response.data?.timestamp || new Date().toISOString(),
							service: response.data?.service || endpoint.name,
						}
					} else {
						return {
							status: 'unhealthy',
							responseTime,
							error: `HTTP ${response.status}: ${response.statusText}`,
						}
					}
				} catch (error) {
					const responseTime = Date.now() - startTime

					if (axios.isAxiosError(error)) {
						return {
							status: 'unhealthy',
							responseTime,
							error: error.response?.status
								? `HTTP ${error.response.status}: ${error.response.statusText}`
								: error.message,
						}
					}

					return {
						status: 'unhealthy',
						responseTime,
						error: 'Network error or timeout',
					}
				}
			},
			refetchInterval: 30000, // Refetch every 30 seconds
			retry: 1,
			staleTime: 10000, // Consider data stale after 10 seconds
		})),
	})

	const handleRefreshAll = () => {
		healthQueries.forEach(query => query.refetch())
		setLastRefresh(new Date())
	}

	// Calculate overall system status
	const criticalServices = healthQueries.filter(
		(_, index) => healthEndpoints[index].critical
	)
	const healthyServices = healthQueries.filter(
		query => query.data?.status === 'healthy'
	).length
	const unhealthyServices = healthQueries.filter(
		query => query.data?.status === 'unhealthy'
	).length
	const criticalDown = criticalServices.some(
		query => query.data?.status === 'unhealthy'
	)

	const overallStatus = criticalDown
		? 'critical'
		: unhealthyServices > 0
			? 'degraded'
			: 'healthy'

	const getStatusBadge = (
		status: HealthStatus['status'],
		critical: boolean
	) => {
		switch (status) {
			case 'healthy':
				return (
					<Badge className='bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200'>
						Healthy
					</Badge>
				)
			case 'unhealthy':
				return (
					<Badge
						className={`${critical ? 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200' : 'bg-orange-100 text-orange-800 dark:bg-orange-900 dark:text-orange-200'}`}
					>
						{critical ? 'Critical' : 'Unhealthy'}
					</Badge>
				)
			case 'loading':
				return <Badge variant='secondary'>Loading...</Badge>
			default:
				return <Badge variant='outline'>Unknown</Badge>
		}
	}

	const getStatusIcon = (status: HealthStatus['status']) => {
		switch (status) {
			case 'healthy':
				return <CheckCircle className='h-5 w-5 text-green-500' />
			case 'unhealthy':
				return <XCircle className='h-5 w-5 text-red-500' />
			case 'loading':
				return <Loader2 className='h-5 w-5 animate-spin text-blue-500' />
			default:
				return <AlertTriangle className='h-5 w-5 text-gray-500' />
		}
	}

	return (
		<div className='min-h-screen bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900 p-4'>
			<div className='max-w-6xl mx-auto'>
				{/* Header */}
				<div className='flex items-center justify-between mb-8'>
					<div className='flex items-center space-x-3'>
						<Activity className='h-8 w-8 text-green-700' />
						<div>
							<h1 className='text-3xl font-bold text-gray-900 dark:text-white'>
								System Health Check
							</h1>
							<p className='text-gray-600 dark:text-gray-300'>
								Monitor all FrogEdu services and infrastructure
							</p>
						</div>
					</div>
					<div className='flex items-center space-x-4'>
						<p className='text-sm text-gray-500'>
							Last updated: {lastRefresh.toLocaleTimeString()}
						</p>
						<Button
							onClick={handleRefreshAll}
							variant='outline'
							size='sm'
							className='flex items-center space-x-2'
						>
							<RefreshCcw className='h-4 w-4' />
							<span>Refresh All</span>
						</Button>
					</div>
				</div>

				{/* Overall Status Alert */}
				<Alert
					className={`mb-6 ${
						overallStatus === 'critical'
							? 'border-red-500 bg-red-50 dark:bg-red-950'
							: overallStatus === 'degraded'
								? 'border-orange-500 bg-orange-50 dark:bg-orange-950'
								: 'border-green-500 bg-green-50 dark:bg-green-950'
					}`}
				>
					<Server className='h-4 w-4' />
					<AlertDescription className='flex items-center justify-between'>
						<span>
							<strong>System Status: </strong>
							{overallStatus === 'critical' &&
								'Critical - One or more critical services are down'}
							{overallStatus === 'degraded' &&
								'Degraded - Some services experiencing issues'}
							{overallStatus === 'healthy' && 'All systems operational'}
						</span>
						<Badge
							className={
								overallStatus === 'critical'
									? 'bg-red-100 text-red-800'
									: overallStatus === 'degraded'
										? 'bg-orange-100 text-orange-800'
										: 'bg-green-100 text-green-800'
							}
						>
							{healthyServices}/{healthEndpoints.length} Services Healthy
						</Badge>
					</AlertDescription>
				</Alert>

				{/* Service Status Cards */}
				<div className='grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6'>
					{healthEndpoints.map((endpoint, index) => {
						const query = healthQueries[index]
						const status = query.isLoading
							? 'loading'
							: query.data?.status || 'unknown'

						return (
							<Card key={endpoint.name} className='relative'>
								<CardHeader className='pb-3'>
									<div className='flex items-center justify-between'>
										<CardTitle className='text-lg flex items-center space-x-2'>
											{getStatusIcon(status)}
											<span>{endpoint.name}</span>
										</CardTitle>
										{getStatusBadge(status, endpoint.critical)}
									</div>
									<p className='text-sm text-gray-600 dark:text-gray-400'>
										{endpoint.description}
									</p>
								</CardHeader>
								<CardContent className='pt-0'>
									<div className='space-y-2 text-sm'>
										<div className='flex justify-between'>
											<span className='text-gray-600 dark:text-gray-400'>
												Endpoint:
											</span>
											<span className='font-mono text-xs truncate ml-2'>
												{endpoint.url}
											</span>
										</div>

										{query.data?.responseTime && (
											<div className='flex justify-between'>
												<span className='text-gray-600 dark:text-gray-400'>
													Response Time:
												</span>
												<span
													className={`${
														query.data.responseTime < 1000
															? 'text-green-600'
															: query.data.responseTime < 3000
																? 'text-orange-600'
																: 'text-red-600'
													}`}
												>
													{query.data.responseTime}ms
												</span>
											</div>
										)}

										{query.data?.timestamp && (
											<div className='flex justify-between'>
												<span className='text-gray-600 dark:text-gray-400'>
													Last Check:
												</span>
												<span>
													{new Date(query.data.timestamp).toLocaleTimeString()}
												</span>
											</div>
										)}

										{endpoint.critical && (
											<div className='flex justify-between'>
												<span className='text-gray-600 dark:text-gray-400'>
													Service Type:
												</span>
												<Badge variant='destructive' className='text-xs'>
													Critical
												</Badge>
											</div>
										)}

										{query.data?.error && (
											<div className='mt-3 p-2 bg-red-50 dark:bg-red-950 rounded text-red-700 dark:text-red-300 text-xs'>
												<strong>Error:</strong> {query.data.error}
											</div>
										)}
									</div>

									<Button
										onClick={() => query.refetch()}
										variant='outline'
										size='sm'
										className='w-full mt-4'
										disabled={query.isFetching}
									>
										{query.isFetching ? (
											<>
												<Loader2 className='h-3 w-3 animate-spin mr-2' />
												Checking...
											</>
										) : (
											<>
												<RefreshCcw className='h-3 w-3 mr-2' />
												Recheck
											</>
										)}
									</Button>
								</CardContent>
							</Card>
						)
					})}
				</div>

				{/* Debug Information */}
				<Card className='mt-8'>
					<CardHeader>
						<CardTitle className='text-lg'>Debug Information</CardTitle>
					</CardHeader>
					<CardContent>
						<div className='grid grid-cols-1 md:grid-cols-3 gap-4 text-sm'>
							<div>
								<h4 className='font-semibold mb-2'>Network</h4>
								<p>Environment: {process.env.NODE_ENV}</p>
								<p>Host: {window.location.hostname}</p>
								<p>Port: {window.location.port}</p>
							</div>
							<div>
								<h4 className='font-semibold mb-2'>API Configuration</h4>
								<p>Base URL: https://api.frogedu.org</p>
								<p>Timeout: 10 seconds</p>
								<p>Auto-refresh: 30 seconds</p>
							</div>
							<div>
								<h4 className='font-semibold mb-2'>Browser</h4>
								<p>User Agent: {navigator.userAgent.split(' ')[0]}</p>
								<p>Language: {navigator.language}</p>
								<p>Online: {navigator.onLine ? 'Yes' : 'No'}</p>
							</div>
						</div>
					</CardContent>
				</Card>
			</div>
		</div>
	)
}

export default HealthPage
