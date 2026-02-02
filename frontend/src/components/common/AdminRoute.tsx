import React from 'react'
import { Navigate } from 'react-router'
import { useMe } from '@/hooks/auth/useMe'
import { Skeleton } from '@/components/ui/skeleton'

interface AdminRouteProps {
	children: React.ReactNode
}

const AdminRoute = ({ children }: AdminRouteProps): React.ReactElement => {
	const { user, isLoading } = useMe()

	if (isLoading) {
		return (
			<div className='flex min-h-screen items-center justify-center p-4'>
				<div className='w-full max-w-md space-y-4'>
					<Skeleton className='h-12 w-full rounded' />
					<Skeleton className='h-64 w-full rounded' />
					<Skeleton className='h-12 w-full rounded' />
				</div>
			</div>
		)
	}

	const isAdmin = user?.role?.name === 'Admin'

	if (!isAdmin) {
		return <Navigate to='/app' replace />
	}

	return <>{children}</>
}

export default AdminRoute
