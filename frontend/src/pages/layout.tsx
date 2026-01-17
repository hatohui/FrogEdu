import React from 'react'
import Navigation from '@/components/common/Navigation'
import { useLocation } from 'react-router'

const MainLayout = ({
	children,
}: {
	children: React.ReactNode
}): React.ReactElement => {
	const location = useLocation()

	// Exclude navigation from auth pages and dashboard pages (they have their own Header)
	const isAuthPage =
		location.pathname.startsWith('/login') ||
		location.pathname.startsWith('/register') ||
		location.pathname.startsWith('/forgot-password') ||
		location.pathname.startsWith('/auth/')

	const isDashboardPage = location.pathname.startsWith('/dashboard')

	if (isAuthPage || isDashboardPage) {
		return <>{children}</>
	}

	return (
		<div className='min-h-screen bg-gray-50 dark:bg-gray-900'>
			<Navigation />
			<main className='flex-1'>{children}</main>
		</div>
	)
}

export default MainLayout
