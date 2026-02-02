import React from 'react'
import DashboardLayout from '@/components/layout/DashboardLayout'
import ProtectedRoute from '@/components/common/ProtectedRoute'

interface DashboardLayoutWrapperProps {
	children?: React.ReactNode
}

const DashboardLayoutWrapper = ({
	children,
}: DashboardLayoutWrapperProps): React.ReactElement => {
	return (
		<ProtectedRoute>
			<DashboardLayout>{children}</DashboardLayout>
		</ProtectedRoute>
	)
}

export default DashboardLayoutWrapper
