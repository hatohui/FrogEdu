import React from 'react'
import { Outlet } from 'react-router'
import AdminRoute from '@/components/common/AdminRoute'

/**
 * Dashboard layout for admin-only pages
 */
const DashboardLayout = (): React.ReactElement => {
	return (
		<AdminRoute>
			<Outlet />
		</AdminRoute>
	)
}

export default DashboardLayout
