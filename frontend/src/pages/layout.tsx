import React from 'react'
import Navigation from '@/components/common/Navigation'
import { useLocation } from 'react-router'
import { RoleGuard } from '@/components/common/RoleGuard'

const MainLayout = ({
	children,
}: {
	children: React.ReactNode
}): React.ReactElement => {
	const location = useLocation()

	const isAuthPage =
		location.pathname.startsWith('/login') ||
		location.pathname.startsWith('/register') ||
		location.pathname.startsWith('/forgot-password') ||
		location.pathname.startsWith('/select-role') ||
		location.pathname.startsWith('/auth/')

	const isAppPage = location.pathname.startsWith('/app')

	if (isAuthPage || isAppPage) {
		return <RoleGuard>{children}</RoleGuard>
	}

	return (
		<RoleGuard>
			<div className='bg-gray-50 dark:bg-gray-900'>
				<Navigation />
				<main className='flex-1'>{children}</main>
			</div>
		</RoleGuard>
	)
}

export default MainLayout
