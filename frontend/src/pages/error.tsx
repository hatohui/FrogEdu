import React from 'react'
import { Link, useRouteError, isRouteErrorResponse } from 'react-router'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Alert, AlertDescription } from '@/components/ui/alert'
import { Home, RefreshCcw, AlertTriangle } from 'lucide-react'

const ErrorPage = (): React.JSX.Element => {
	const error = useRouteError()

	let errorMessage = 'An unexpected error occurred'
	let errorDetails = ''
	let statusCode = 500

	if (isRouteErrorResponse(error)) {
		statusCode = error.status
		errorMessage = error.statusText || 'Error'
		errorDetails = error.data?.message || ''
	} else if (error instanceof Error) {
		errorMessage = error.message
		errorDetails = error.stack || ''
	}

	const handleReload = () => {
		window.location.reload()
	}

	return (
		<div className='min-h-screen bg-gradient-to-br from-red-50 to-orange-100 dark:from-red-950 dark:to-gray-900 flex items-center justify-center p-4'>
			<Card className='max-w-2xl w-full'>
				<CardHeader className='text-center'>
					<div className='flex justify-center mb-4'>
						<AlertTriangle className='h-20 w-20 text-red-600 dark:text-red-400' />
					</div>
					<CardTitle className='text-3xl'>
						{statusCode !== 500
							? `Error ${statusCode}`
							: 'Something Went Wrong'}
					</CardTitle>
				</CardHeader>
				<CardContent className='space-y-6'>
					<Alert className='border-red-500 bg-red-50 dark:bg-red-950'>
						<AlertTriangle className='h-4 w-4' />
						<AlertDescription>
							<strong className='font-semibold'>{errorMessage}</strong>
							{errorDetails && (
								<p className='mt-2 text-sm text-gray-600 dark:text-gray-400'>
									{errorDetails}
								</p>
							)}
						</AlertDescription>
					</Alert>

					<div className='flex flex-col sm:flex-row gap-4 justify-center'>
						<Button
							onClick={handleReload}
							variant='outline'
							className='flex items-center space-x-2'
						>
							<RefreshCcw className='h-4 w-4' />
							<span>Reload Page</span>
						</Button>

						<Button asChild className='flex items-center space-x-2'>
							<Link to='/'>
								<Home className='h-4 w-4' />
								<span>Go Home</span>
							</Link>
						</Button>
					</div>

					{process.env.NODE_ENV === 'development' && errorDetails && (
						<div className='mt-8 p-4 bg-gray-50 dark:bg-gray-800 rounded-lg overflow-auto'>
							<h3 className='font-semibold mb-2 text-sm'>
								Stack Trace (Development Only)
							</h3>
							<pre className='text-xs text-gray-600 dark:text-gray-400 whitespace-pre-wrap'>
								{errorDetails}
							</pre>
						</div>
					)}
				</CardContent>
			</Card>
		</div>
	)
}

export default ErrorPage
