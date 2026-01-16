import React from 'react'
import Navigation from '@/components/common/Navigation'

const MainLayout = ({
	children,
}: {
	children: React.ReactNode
}): React.ReactElement => {
	return (
		<div className='min-h-screen bg-gray-50 dark:bg-gray-900'>
			<Navigation />
			<main className='flex-1'>{children}</main>
		</div>
	)
}

export default MainLayout
