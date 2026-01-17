import React from 'react'
import { Menu, LogOut, User, Settings } from 'lucide-react'
import { Button } from '@/components/ui/button'
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuLabel,
	DropdownMenuSeparator,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'
import { Link } from 'react-router'
import { useAuthStore } from '@/stores/authStore'

interface HeaderProps {
	onMenuClick?: () => void
	className?: string
}

const Header = ({
	onMenuClick,
	className = '',
}: HeaderProps): React.ReactElement => {
	const { user, userProfile, signOut } = useAuthStore()

	const handleSignOut = async () => {
		await signOut()
	}

	const getUserInitials = () => {
		if (!userProfile && !user) return 'U'
		const name = userProfile?.name || userProfile?.email || user?.username || ''
		const parts = name.split(' ')
		if (parts.length >= 2) {
			return `${parts[0][0]}${parts[1][0]}`.toUpperCase()
		}
		return name.substring(0, 2).toUpperCase()
	}

	return (
		<header
			className={`flex items-center justify-between px-6 py-4 border-b bg-card ${className}`}
		>
			{/* Left side - Logo and Menu Button */}
			<div className='flex items-center space-x-4'>
				<Button
					variant='ghost'
					size='icon'
					onClick={onMenuClick}
					className='lg:hidden'
				>
					<Menu className='h-6 w-6' />
				</Button>

				<div className='hidden lg:flex items-center space-x-2'>
					<div className='w-8 h-8 rounded-lg bg-primary flex items-center justify-center text-primary-foreground font-bold text-lg'>
						<img
							src='/frog.png'
							alt='FrogEdu Logo'
							className='w-full h-full object-contain'
						/>
					</div>
					<span className='font-bold text-lg'>FrogEdu</span>
				</div>
			</div>

			{/* Center - Page Title (optional, can be passed via context or props) */}
			<div className='flex-1 px-4'>
				<h1 className='text-lg font-semibold text-foreground hidden sm:block'>
					{typeof document !== 'undefined'
						? document.title.split(' | ')[0]
						: 'Dashboard'}
				</h1>
			</div>

			{/* Right side - User Menu */}
			<div className='flex items-center space-x-4'>
				{/* Notification bell (placeholder) */}
				{/* Future: Add notification badge here */}

				{/* User Avatar & Dropdown */}
				<DropdownMenu>
					<DropdownMenuTrigger asChild>
						<Button variant='ghost' className='relative h-10 w-10 rounded-full'>
							<Avatar className='h-10 w-10'>
								<AvatarImage
									src={userProfile?.picture}
									alt={userProfile?.name || userProfile?.email || 'User'}
								/>
								<AvatarFallback className='bg-primary text-primary-foreground'>
									{getUserInitials()}
								</AvatarFallback>
							</Avatar>
						</Button>
					</DropdownMenuTrigger>

					<DropdownMenuContent align='end' className='w-56'>
						<DropdownMenuLabel className='flex flex-col space-y-1'>
							<span className='text-sm font-medium'>
								{userProfile?.name ||
									userProfile?.email ||
									user?.username ||
									'User'}
							</span>
							<span className='text-xs text-muted-foreground'>
								{userProfile?.email || user?.username || ''}
							</span>
						</DropdownMenuLabel>

						<DropdownMenuSeparator />

						<DropdownMenuItem asChild>
							<Link to='/profile' className='cursor-pointer'>
								<User className='mr-2 h-4 w-4' />
								<span>Profile Settings</span>
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
							<span>Sign Out</span>
						</DropdownMenuItem>
					</DropdownMenuContent>
				</DropdownMenu>
			</div>
		</header>
	)
}

export default Header
