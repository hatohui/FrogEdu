import React from 'react'
import DashboardLayout from '@/components/layout/DashboardLayout'
import ProtectedRoute from '@/components/common/ProtectedRoute'

const DashboardLayoutWrapper = (): React.ReactElement => {
	return (
		<ProtectedRoute>
			<DashboardLayout />
		</ProtectedRoute>
	)
}

export default DashboardLayoutWrapper
