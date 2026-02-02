import React from 'react'
import { Link, useLocation, useNavigate } from 'react-router'
import { Button } from '@/components/ui/button'
import { Home, Sun, Moon, LogOut, User, Settings } from 'lucide-react'
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuLabel,
	DropdownMenuSeparator,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import { useTheme } from '@/config/theme'
import UserAvatar from './UserAvatar'
import { useMe } from '@/hooks/auth/useMe'
import { toast } from 'sonner'

const Navigation = (): React.JSX.Element => {
	const location = useLocation()
	const navigate = useNavigate()
	const [theme, toggleTheme] = useTheme()
	const { user, signOutThenNavigate } = useMe()

	const handleSignOut = () => {
		signOutThenNavigate('/login')
		toast.success('Successfully signed out')
	}

	const staticNavItems = [
		{ path: '/', label: 'Home', icon: Home },
		{ path: '/about', label: 'About', icon: Home },
	]

	return (
		<nav className='bg-card border-b border-border'>
			<div className='max-w-7xl mx-auto px-4 sm:px-6 lg:px-8'>
				<div className='flex justify-between items-center h-16'>
					<Link
						to='/'
						className='flex items-center space-x-2 hover:opacity-80 transition-opacity flex-shrink-0'
					>
						<div className='w-8 h-8 rounded-lg bg-primary flex items-center justify-center text-primary-foreground font-bold text-lg'>
							<img
								src='/frog.png'
								alt='FrogEdu Logo'
								className='w-full h-full object-contain'
							/>
						</div>
						<span className='text-xl font-bold'>FrogEdu</span>
					</Link>

					<div className='hidden md:flex items-center space-x-4 absolute left-1/2 transform -translate-x-1/2'>
						{staticNavItems.map(item => {
							const isActive = location.pathname === item.path

							return (
								<Button
									key={item.path}
									variant={isActive ? 'default' : 'ghost'}
									size='sm'
									asChild
								>
									<Link to={item.path}>
										<span>{item.label}</span>
									</Link>
								</Button>
							)
						})}
					</div>

					<div className='flex items-center space-x-2 flex-shrink-0'>
						<Button
							variant='ghost'
							size='icon'
							onClick={toggleTheme}
							className='h-10 w-10'
							title={`Switch to ${theme === 'light' ? 'dark' : 'light'} mode`}
						>
							{theme === 'light' ? (
								<Moon className='h-5 w-5' />
							) : (
								<Sun className='h-5 w-5' />
							)}
						</Button>

						{user ? (
							<>
								<Button
									variant='outline'
									size='sm'
									onClick={() => navigate('/app')}
									className='hidden sm:flex items-center gap-2'
								>
									<Home className='h-4 w-4' />
									<span>App</span>
								</Button>

								<DropdownMenu>
									<DropdownMenuTrigger asChild>
										<Button
											variant='ghost'
											className='relative h-10 w-10 rounded-full'
										>
											<UserAvatar user={user} />
										</Button>
									</DropdownMenuTrigger>

									<DropdownMenuContent align='end' className='w-56'>
										<DropdownMenuLabel className='flex flex-col space-y-1'>
											<span className='text-sm font-medium'>
												{user?.firstName || user?.lastName || 'User'}
											</span>
											<span className='text-xs text-muted-foreground'>
												{user?.email || ''}
											</span>
										</DropdownMenuLabel>

										<DropdownMenuSeparator />

										<DropdownMenuItem asChild>
											<Link to='/profile' className='cursor-pointer'>
												<User className='mr-2 h-4 w-4' />
												<span>Profile</span>
											</Link>
										</DropdownMenuItem>

										<DropdownMenuItem asChild>
											<Link to='/settings' className='cursor-pointer'>
												<Settings className='mr-2 h-4 w-4' />
												<span>Settings</span>
											</Link>
										</DropdownMenuItem>

										<DropdownMenuSeparator />

										<DropdownMenuItem onClick={handleSignOut}>
											<LogOut className='mr-2 h-4 w-4' />
											<span>Logout</span>
										</DropdownMenuItem>
									</DropdownMenuContent>
								</DropdownMenu>
							</>
						) : (
							<Button variant='outline' size='sm' asChild>
								<Link to='/login'>Login</Link>
							</Button>
						)}
					</div>
				</div>

				<div className='md:hidden pb-3'>
					<div className='flex flex-wrap gap-2'>
						{staticNavItems.map(item => {
							const isActive = location.pathname === item.path

							return (
								<Button
									key={item.path}
									variant={isActive ? 'default' : 'ghost'}
									size='sm'
									asChild
								>
									<Link to={item.path}>
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
