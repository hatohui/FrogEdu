import React from 'react'
import { useAuthStore } from '@/stores/authStore'
import { Skeleton } from '@/components/ui/skeleton'
import LandingPage from './landing'

const RootPage = (): React.JSX.Element => {
	const { isLoading } = useAuthStore()

	if (isLoading) {
		return (
			<div className='min-h-screen flex items-center justify-center'>
				<div className='space-y-4 w-full max-w-md px-4'>
					<Skeleton className='h-12 w-3/4' />
					<Skeleton className='h-4 w-full' />
					<Skeleton className='h-4 w-5/6' />
				</div>
			</div>
		)
	}

	return <LandingPage />
}

export default RootPage
