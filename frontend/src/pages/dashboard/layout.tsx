import React from 'react'
import AdminRoute from '@/components/common/AdminRoute'

const DashboardLayout = ({
	children,
}: {
	children: React.ReactNode
}): React.ReactElement => {
	return <AdminRoute>{children}</AdminRoute>
}

export default DashboardLayout
