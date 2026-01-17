import React, { useEffect } from 'react'
import { useNavigate } from 'react-router'
import { useAuthStore } from '@/stores/authStore'
import { Skeleton } from '@/components/ui/skeleton'

const LandingPage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const { isAuthenticated, isLoading } = useAuthStore()

	useEffect(() => {
		if (!isLoading) {
			if (isAuthenticated) {
				navigate('/dashboard', { replace: true })
			} else {
				navigate('/login', { replace: true })
			}
		}
	}, [isAuthenticated, isLoading, navigate])

	return (
		<div className='flex min-h-screen items-center justify-center'>
			<div className='space-y-4 w-full max-w-md p-8'>
				<Skeleton className='h-12 w-full' />
				<Skeleton className='h-32 w-full' />
				<Skeleton className='h-12 w-full' />
			</div>
		</div>
	)
}

export default LandingPage
