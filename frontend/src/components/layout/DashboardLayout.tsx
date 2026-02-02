import React, { useState } from 'react'
import { Outlet } from 'react-router'
import Sidebar from './Sidebar'
import Header from './Header'

const AppLayout = (): React.ReactElement => {
	const [sidebarOpen, setSidebarOpen] = useState(false)

	return (
		<div className='flex h-screen overflow-hidden bg-background'>
			{sidebarOpen && (
				<div
					className='fixed inset-0 bg-black/50 z-40 lg:hidden'
					onClick={() => setSidebarOpen(false)}
				/>
			)}

			<div
				className={`fixed inset-y-0 left-0 z-50 w-72 transform transition-transform duration-300 ease-in-out lg:relative lg:translate-x-0 ${
					sidebarOpen ? 'translate-x-0' : '-translate-x-full'
				}`}
			>
				<Sidebar onClose={() => setSidebarOpen(false)} />
			</div>

			<div className='flex-1 flex flex-col overflow-hidden'>
				<Header onMenuClick={() => setSidebarOpen(!sidebarOpen)} />

				<main className='flex-1 overflow-y-auto bg-background'>
					<Outlet />
				</main>
			</div>
		</div>
	)
}

export default AppLayout
