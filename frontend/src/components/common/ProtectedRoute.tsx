import React from 'react'
import { Navigate, useLocation } from 'react-router'
import { useAuthStore } from '@/stores/authStore'
import { Skeleton } from '@/components/ui/skeleton'

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
			<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900 p-4'>
				<div className='w-full max-w-sm'>
					{/* Logo Skeleton */}
					<div className='flex justify-center mb-8'>
						<Skeleton className='h-16 w-16 rounded-lg' />
					</div>

					{/* Card Container */}
					<div className='bg-card rounded-lg border border-border shadow-lg p-6 space-y-6'>
						{/* Title Skeleton */}
						<div className='space-y-3 text-center'>
							<Skeleton className='h-8 w-3/4 mx-auto rounded' />
							<Skeleton className='h-4 w-full rounded' />
						</div>

						{/* Form Fields Skeletons */}
						<div className='space-y-4'>
							{/* Field 1 */}
							<div className='space-y-2'>
								<Skeleton className='h-4 w-12 rounded' />
								<Skeleton className='h-10 w-full rounded' />
							</div>

							{/* Field 2 */}
							<div className='space-y-2'>
								<Skeleton className='h-4 w-20 rounded' />
								<Skeleton className='h-10 w-full rounded' />
							</div>

							{/* Submit Button */}
							<Skeleton className='h-10 w-full rounded mt-6' />
						</div>

						{/* Divider */}
						<div className='flex items-center space-x-4'>
							<div className='flex-1'>
								<Skeleton className='h-px w-full' />
							</div>
							<Skeleton className='h-3 w-12 rounded' />
							<div className='flex-1'>
								<Skeleton className='h-px w-full' />
							</div>
						</div>

						{/* OAuth Button */}
						<Skeleton className='h-10 w-full rounded' />
					</div>

					{/* Footer Skeleton */}
					<div className='flex justify-center mt-6'>
						<Skeleton className='h-4 w-48 rounded' />
					</div>

					{/* Loading Indicator */}
					<div className='flex justify-center mt-8'>
						<div className='relative'>
							<div className='h-8 w-8 rounded-full border-4 border-border border-t-primary animate-spin' />
						</div>
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
