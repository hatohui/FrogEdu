import React, { useState, useEffect } from 'react'
import Sidebar from './Sidebar'
import Header from './Header'
import { useViewAsStore } from '@/stores/viewAsStore'
import { useMe } from '@/hooks/auth/useMe'
import { Eye, X } from 'lucide-react'
import { Button } from '@/components/ui/button'

interface AppLayoutProps {
	children?: React.ReactNode
}

const AppLayout = ({ children }: AppLayoutProps): React.ReactElement => {
	const [sidebarOpen, setSidebarOpen] = useState(false)
	const [sidebarCollapsed, setSidebarCollapsed] = useState(false)
	const { viewAs, clearViewAs } = useViewAsStore()
	const { user } = useMe()
	const isActualAdmin = user?.role?.name === 'Admin'
	const isViewingAs = isActualAdmin && viewAs !== null

	useEffect(() => {
		document.body.classList.add('app-layout-active')
		return () => {
			document.body.classList.remove('app-layout-active')
		}
	}, [])

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
				<Sidebar
					onClose={() => setSidebarOpen(false)}
					collapsed={sidebarCollapsed}
					onToggleCollapse={() => setSidebarCollapsed(!sidebarCollapsed)}
				/>
			</div>

			<div className='flex-1 flex flex-col overflow-hidden'>
				<Header onMenuClick={() => setSidebarOpen(!sidebarOpen)} />
				{/* View-As Banner */}
				{isViewingAs && (
					<div className='flex items-center justify-between px-6 py-2 bg-amber-500 text-amber-950 text-sm font-medium flex-shrink-0'>
						<div className='flex items-center gap-2'>
							<Eye className='h-4 w-4' />
							<span>
								Previewing UI as <strong>{viewAs}</strong> — this is a read-only
								view, no data is changed.
							</span>
						</div>
						<Button
							variant='ghost'
							size='sm'
							onClick={clearViewAs}
							className='h-7 px-2 hover:bg-amber-600 hover:text-white'
						>
							<X className='h-4 w-4 mr-1' />
							Exit Preview
						</Button>
					</div>
				)}
				<main className='flex-1 overflow-y-auto relative bg-background'>
					{children}
				</main>
			</div>
		</div>
	)
}

export default AppLayout
