import React from 'react'
import { Link, useLocation } from 'react-router'
import { Button } from '@/components/ui/button'
import {
	Activity,
	Home,
	User,
	BookOpen,
	Brain,
	ClipboardCheck,
	Monitor,
} from 'lucide-react'

const Navigation = (): React.JSX.Element => {
	const location = useLocation()

	const navItems = [
		{ path: '/', label: 'Home', icon: Home },
		{ path: '/health', label: 'Health Check', icon: Activity },
		{ path: '/dashboard', label: 'Dashboard', icon: Monitor },
		{ path: '/content', label: 'Content', icon: BookOpen },
		{ path: '/assessments', label: 'Assessments', icon: ClipboardCheck },
		{ path: '/ai', label: 'AI Tutor', icon: Brain },
		{ path: '/profile', label: 'Profile', icon: User },
	]

	return (
		<nav className='bg-white dark:bg-gray-900 border-b border-gray-200 dark:border-gray-700'>
			<div className='max-w-7xl mx-auto px-4 sm:px-6 lg:px-8'>
				<div className='flex justify-between h-16'>
					<div className='flex items-center space-x-8'>
						<div className='flex items-center space-x-2'>
							<img src='/frog.png' alt='FrogEdu Logo' width={32} height={32} />
							<span className='text-xl font-bold text-gray-900 dark:text-white'>
								FrogEdu
							</span>
						</div>

						<div className='hidden md:flex items-center space-x-4'>
							{navItems.map(item => {
								const Icon = item.icon
								const isActive = location.pathname === item.path

								return (
									<Button
										key={item.path}
										variant={isActive ? 'default' : 'ghost'}
										size='sm'
										asChild
									>
										<Link
											to={item.path}
											className='flex items-center space-x-2'
										>
											<Icon className='h-4 w-4' />
											<span>{item.label}</span>
										</Link>
									</Button>
								)
							})}
						</div>
					</div>

					<div className='flex items-center space-x-4'>
						<Button variant='outline' size='sm' asChild>
							<Link to='/login'>Login</Link>
						</Button>
					</div>
				</div>

				{/* Mobile Navigation */}
				<div className='md:hidden pb-3'>
					<div className='flex flex-wrap gap-2'>
						{navItems.slice(0, 4).map(item => {
							const Icon = item.icon
							const isActive = location.pathname === item.path

							return (
								<Button
									key={item.path}
									variant={isActive ? 'default' : 'ghost'}
									size='sm'
									asChild
								>
									<Link to={item.path} className='flex items-center space-x-2'>
										<Icon className='h-4 w-4' />
										<span className='text-xs'>{item.label}</span>
									</Link>
								</Button>
							)
						})}
					</div>
				</div>
			</div>
		</nav>
	)
}

export default Navigation
