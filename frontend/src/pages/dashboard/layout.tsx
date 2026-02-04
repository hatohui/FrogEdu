import React from 'react'
import AdminRoute from '@/components/common/AdminRoute'
import AdminLayout from '@/components/layout/AdminLayout'

const DashboardLayout = ({
	children,
}: {
	children: React.ReactNode
}): React.ReactElement => {
	return (
		<AdminRoute>
			<AdminLayout>{children}</AdminLayout>
		</AdminRoute>
	)
}

export default DashboardLayout
