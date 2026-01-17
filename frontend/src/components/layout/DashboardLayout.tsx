import React, { useState } from 'react'
import { Outlet } from 'react-router'
import Sidebar from './Sidebar'
import { Button } from '@/components/ui/button'
import { Menu } from 'lucide-react'

const DashboardLayout = (): React.ReactElement => {
	const [sidebarOpen, setSidebarOpen] = useState(false)

	return (
		<div className='flex h-screen overflow-hidden bg-background'>
			{/* Mobile Sidebar Overlay */}
			{sidebarOpen && (
				<div
					className='fixed inset-0 bg-black/50 z-40 lg:hidden'
					onClick={() => setSidebarOpen(false)}
				/>
			)}

			{/* Sidebar - Desktop: Always visible, Mobile: Slide in from left */}
			<div
				className={`fixed inset-y-0 left-0 z-50 w-72 transform transition-transform duration-300 ease-in-out lg:relative lg:translate-x-0 ${
					sidebarOpen ? 'translate-x-0' : '-translate-x-full'
				}`}
			>
				<Sidebar onClose={() => setSidebarOpen(false)} />
			</div>

			{/* Main Content Area */}
			<div className='flex-1 flex flex-col overflow-hidden'>
				{/* Mobile Header */}
				<header className='lg:hidden flex items-center justify-between p-4 border-b bg-card'>
					<Button
						variant='ghost'
						size='icon'
						onClick={() => setSidebarOpen(true)}
					>
						<Menu className='h-6 w-6' />
					</Button>
					<div className='flex items-center space-x-2'>
						<span className='font-bold text-lg'>🐸 FrogEdu</span>
					</div>
					<div className='w-10' /> {/* Spacer for centering */}
				</header>

				{/* Page Content */}
				<main className='flex-1 overflow-y-auto bg-background'>
					<Outlet />
				</main>
			</div>
		</div>
	)
}

export default DashboardLayout
