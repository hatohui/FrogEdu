import React from 'react'
import {
	Menu,
	LogOut,
	User,
	Settings,
	Sun,
	Moon,
	Home,
	CreditCard,
	Crown,
} from 'lucide-react'
import { Button } from '@/components/ui/button'
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuLabel,
	DropdownMenuSeparator,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import { Badge } from '@/components/ui/badge'
import { Link, useNavigate } from 'react-router'
import { useTheme } from '@/config/theme'
import { useMe } from '@/hooks/auth/useMe'
import { useSubscription } from '@/hooks/useSubscription'
import UserAvatar from '../common/UserAvatar'

interface HeaderProps {
	onMenuClick?: () => void
	className?: string
}

const Header = ({
	onMenuClick,
	className = '',
}: HeaderProps): React.ReactElement => {
	const { user, signOut } = useMe()
	const { isPro } = useSubscription()
	const [theme, toggleTheme] = useTheme()
	const navigate = useNavigate()

	const handleSignOut = async () => {
		await signOut()
	}

	return (
		<header
			className={`flex items-center justify-between px-6 py-4 border-b bg-card ${className}`}
		>
			{/* Left side - Menu Button */}
			<div className='flex items-center space-x-4'>
				<Button
					variant='ghost'
					size='icon'
					onClick={onMenuClick}
					className='lg:hidden'
				>
					<Menu className='h-6 w-6' />
				</Button>
			</div>

			{/* Center - Page Title (optional, can be passed via context or props) */}
			<div className='flex-1 px-4'>
				<h1 className='text-lg font-semibold text-foreground hidden sm:block'>
					{typeof document !== 'undefined'
						? document.title.split(' | ')[0]
						: 'App'}
				</h1>
			</div>

			{/* Right side - Actions & User Menu */}
			<div className='flex items-center space-x-2'>
				{/* Subscription Status Badge */}
				{user && (
					<Button
						variant='ghost'
						size='sm'
						onClick={() => navigate('/profile/subscription')}
						className='hidden sm:flex items-center gap-2'
					>
						{isPro ? (
							<Badge className='bg-gradient-to-r from-amber-500 to-orange-500 text-white'>
								<Crown className='h-3 w-3 mr-1' />
								Pro
							</Badge>
						) : (
							<Badge variant='secondary'>
								<CreditCard className='h-3 w-3 mr-1' />
								Free
							</Badge>
						)}
					</Button>
				)}

				{/* Dark Mode Toggle */}
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

				{user && (
					<Button
						variant='outline'
						size='sm'
						onClick={() => navigate('/app')}
						className='hidden sm:flex items-center gap-2'
					>
						<Home className='h-4 w-4' />
						<span>App</span>
					</Button>
				)}

				{/* User Avatar & Dropdown */}
				<DropdownMenu>
					<DropdownMenuTrigger asChild>
						<Button variant='ghost' className='relative h-10 w-10 rounded-full'>
							<UserAvatar user={user} />
						</Button>
					</DropdownMenuTrigger>

					<DropdownMenuContent align='end' className='w-56'>
						<DropdownMenuLabel className='flex flex-col space-y-1'>
							<div className='flex items-center gap-2'>
								<span className='text-sm font-medium'>
									{user?.firstName || user?.email || 'User'}
								</span>
								{isPro && (
									<Badge className='bg-gradient-to-r from-amber-500 to-orange-500 text-white text-xs px-1.5 py-0'>
										Pro
									</Badge>
								)}
							</div>
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
							<Link to='/profile/subscription' className='cursor-pointer'>
								<CreditCard className='mr-2 h-4 w-4' />
								<span>Subscription</span>
								{!isPro && (
									<Badge
										variant='outline'
										className='ml-auto text-xs border-amber-500 text-amber-600'
									>
										Upgrade
									</Badge>
								)}
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
			</div>
		</header>
	)
}

export default Header
