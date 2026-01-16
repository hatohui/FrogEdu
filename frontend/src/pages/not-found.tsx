import React from 'react'
import { Link, useNavigate } from 'react-router'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Home, ArrowLeft, Search } from 'lucide-react'

const NotFoundPage = (): React.JSX.Element => {
	const navigate = useNavigate()

	return (
		<div className='min-h-screen bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900 flex items-center justify-center p-4'>
			<Card className='max-w-2xl w-full'>
				<CardHeader className='text-center'>
					<div className='flex justify-center mb-4'>
						<div className='text-9xl font-bold text-green-600 dark:text-green-400'>
							404
						</div>
					</div>
					<CardTitle className='text-3xl'>Page Not Found</CardTitle>
				</CardHeader>
				<CardContent className='space-y-6'>
					<p className='text-center text-gray-600 dark:text-gray-400'>
						Oops! The page you're looking for doesn't exist. It might have been
						moved or deleted.
					</p>

					<div className='flex flex-col sm:flex-row gap-4 justify-center'>
						<Button
							onClick={() => navigate(-1)}
							variant='outline'
							className='flex items-center space-x-2'
						>
							<ArrowLeft className='h-4 w-4' />
							<span>Go Back</span>
						</Button>

						<Button asChild className='flex items-center space-x-2'>
							<Link to='/'>
								<Home className='h-4 w-4' />
								<span>Go Home</span>
							</Link>
						</Button>
					</div>

					<div className='mt-8 p-4 bg-gray-50 dark:bg-gray-800 rounded-lg'>
						<h3 className='font-semibold mb-2 flex items-center space-x-2'>
							<Search className='h-4 w-4' />
							<span>Quick Links</span>
						</h3>
						<ul className='space-y-2 text-sm'>
							<li>
								<Link
									to='/health'
									className='text-green-600 dark:text-green-400 hover:underline'
								>
									System Health Check
								</Link>
							</li>
							<li>
								<Link
									to='/login'
									className='text-green-600 dark:text-green-400 hover:underline'
								>
									Login
								</Link>
							</li>
							<li>
								<Link
									to='/register'
									className='text-green-600 dark:text-green-400 hover:underline'
								>
									Register
								</Link>
							</li>
						</ul>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default NotFoundPage
