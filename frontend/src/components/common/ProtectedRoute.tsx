import React from 'react'
import { Navigate, useLocation } from 'react-router'
import { useAuthStore } from '@/stores/authStore'

interface ProtectedRouteProps {
	children: React.ReactNode
	requireAuth?: boolean
	redirectTo?: string
}

const ProtectedRoute = ({
	children,
	requireAuth = true,
	redirectTo = '/login',
}: ProtectedRouteProps): React.ReactElement => {
	const location = useLocation()
	const { isAuthenticated, isLoading } = useAuthStore()

	if (isLoading) {
		return (
			<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900'>
				<div className='text-center space-y-6'>
					{/* Logo */}
					<div className='flex justify-center mb-4'>
						<img
							src='/frog.png'
							alt='FrogEdu logo'
							className='w-20 h-20 animate-pulse'
						/>
					</div>

					{/* Spinner */}
					<div className='flex justify-center'>
						<div className='relative'>
							<div className='h-12 w-12 rounded-full border-4 border-green-200 dark:border-green-900' />
							<div className='absolute top-0 left-0 h-12 w-12 rounded-full border-4 border-transparent border-t-green-600 dark:border-t-green-400 animate-spin' />
						</div>
					</div>

					{/* Loading Text */}
					<div className='space-y-2'>
						<h2 className='text-xl font-semibold text-foreground'>
							Loading...
						</h2>
						<p className='text-sm text-muted-foreground'>
							Please wait a moment
						</p>
					</div>
				</div>
			</div>
		)
	}

	if (requireAuth && !isAuthenticated) {
		return <Navigate to={redirectTo} state={{ from: location }} replace />
	}

	if (!requireAuth && isAuthenticated) {
		// Let useMe determine the redirect location based on role
		return <Navigate to='/app' replace />
	}

	return <>{children}</>
}

export default ProtectedRoute
