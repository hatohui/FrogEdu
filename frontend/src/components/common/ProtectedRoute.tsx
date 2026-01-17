import React from 'react'
import { Navigate, useLocation } from 'react-router'
import { useAuthStore } from '@/stores/authStore'
import { Skeleton } from '@/components/ui/skeleton'

interface ProtectedRouteProps {
	children: React.ReactNode
	requireAuth?: boolean
	redirectTo?: string
}

/**
 * ProtectedRoute component to guard routes that require authentication
 *
 * @param children - The component to render if authenticated
 * @param requireAuth - Whether authentication is required (default: true)
 * @param redirectTo - Where to redirect if not authenticated (default: /login)
 */
const ProtectedRoute = ({
	children,
	requireAuth = true,
	redirectTo = '/login',
}: ProtectedRouteProps): React.ReactElement => {
	const location = useLocation()
	const { isAuthenticated, isLoading } = useAuthStore()

	// Show loading state while checking authentication
	if (isLoading) {
		return (
			<div className='flex min-h-screen items-center justify-center'>
				<div className='space-y-4 w-full max-w-md p-8'>
					<Skeleton className='h-12 w-full' />
					<Skeleton className='h-32 w-full' />
					<Skeleton className='h-32 w-full' />
					<Skeleton className='h-12 w-full' />
				</div>
			</div>
		)
	}

	// If authentication is required but user is not authenticated, redirect to login
	if (requireAuth && !isAuthenticated) {
		return <Navigate to={redirectTo} state={{ from: location }} replace />
	}

	// If route is public only (like login page) and user is authenticated, redirect to dashboard
	if (!requireAuth && isAuthenticated) {
		return <Navigate to='/dashboard' replace />
	}

	return <>{children}</>
}

export default ProtectedRoute
