import React from 'react'
import AppLayout from '@/components/layout/AppLayout'
import ProtectedRoute from '@/components/common/ProtectedRoute'

interface AppLayoutWrapperProps {
	children?: React.ReactNode
}

const AppLayoutWrapper = ({
	children,
}: AppLayoutWrapperProps): React.ReactElement => {
	return (
		<ProtectedRoute>
			<AppLayout>{children}</AppLayout>
		</ProtectedRoute>
	)
}

export default AppLayoutWrapper
