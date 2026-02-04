import React, { useState } from 'react'
import AdminSidebar from './AdminSidebar'
import Header from './Header'

interface AdminLayoutProps {
	children?: React.ReactNode
}

const AdminLayout = ({ children }: AdminLayoutProps): React.ReactElement => {
	const [sidebarOpen, setSidebarOpen] = useState(false)
	const [sidebarCollapsed, setSidebarCollapsed] = useState(false)

	return (
		<div className='flex h-screen overflow-hidden bg-background'>
			{sidebarOpen && (
				<div
					className='fixed inset-0 bg-black/50 z-40 lg:hidden'
					onClick={() => setSidebarOpen(false)}
				/>
			)}

			<div
				className={`fixed inset-y-0 left-0 z-50 transform transition-all duration-300 ease-in-out lg:relative lg:translate-x-0 ${
					sidebarOpen ? 'translate-x-0' : '-translate-x-full'
				} ${sidebarCollapsed ? 'w-20' : 'w-72'}`}
			>
				<AdminSidebar
					onClose={() => setSidebarOpen(false)}
					collapsed={sidebarCollapsed}
					onToggleCollapse={() => setSidebarCollapsed(!sidebarCollapsed)}
				/>
			</div>

			<div className='flex-1 flex flex-col overflow-hidden'>
				<Header onMenuClick={() => setSidebarOpen(!sidebarOpen)} />

				<main className='flex-1 overflow-y-auto bg-background'>{children}</main>
			</div>
		</div>
	)
}

export default AdminLayout
