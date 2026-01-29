import React from 'react'
import { Link, useLocation } from 'react-router'
import { cn } from '@/utils/shadcn'
import { Home, BookOpen, FileText, User, LogOut, X, Users } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import useAuth from '@/hooks/auth/useAuth'
import { useMe } from '@/hooks/auth/useMe'
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'

interface NavItem {
	name: string
	href: string
	icon: React.ComponentType<{ className?: string }>
}

const navItems: NavItem[] = [
	{
		name: 'Dashboard',
		href: '/dashboard',
		icon: Home,
	},
	{
		name: 'My Classes',
		href: '/dashboard/classes',
		icon: Users,
	},
	{
		name: 'Content Library',
		href: '/content',
		icon: BookOpen,
	},
	{
		name: 'Exam Generator',
		href: '/assessment',
		icon: FileText,
	},
	{
		name: 'Profile',
		href: '/profile',
		icon: User,
	},
]

interface SidebarProps {
	className?: string
	onClose?: () => void
}

const Sidebar = ({ className, onClose }: SidebarProps): React.ReactElement => {
	const location = useLocation()
	const { signOut } = useAuth()
	const { data: me } = useMe()

	const handleSignOut = async () => {
		await signOut()
		if (onClose) onClose()
	}

	const getUserInitials = () => {
		if (!me) return 'U'
		const name =
			me.firstName && me.lastName
				? `${me.firstName} ${me.lastName}`
				: me.email || ''
		const parts = name.split(' ')
		if (parts.length >= 2) {
			return `${parts[0][0]}${parts[1][0]}`.toUpperCase()
		}
		return name.substring(0, 2).toUpperCase()
	}

	return (
		<aside
			className={cn(
				'flex flex-col h-full bg-sidebar border-r border-sidebar-border',
				className
			)}
		>
			{/* Close button for mobile */}
			{onClose && (
				<div className='flex justify-end p-4 lg:hidden'>
					<Button
						variant='ghost'
						size='icon'
						onClick={onClose}
						className='text-sidebar-foreground'
					>
						<X className='h-5 w-5' />
					</Button>
				</div>
			)}

			{/* Logo and Brand */}
			<div className='p-6 space-y-2'>
				<Link
					to='/dashboard'
					className='flex items-center space-x-3 group'
					onClick={onClose}
				>
					<div className='w-10 h-10 rounded-lg bg-primary flex items-center justify-center text-primary-foreground font-bold text-xl transition-transform group-hover:scale-105'>
						<img
							src='/frog.png'
							alt='FrogEdu Logo'
							className='w-full h-full object-contain'
						/>
					</div>
					<div className='flex flex-col'>
						<span className='font-bold text-lg text-sidebar-foreground'>
							FrogEdu
						</span>
						<span className='text-xs text-sidebar-foreground/60'>
							Learning Platform
						</span>
					</div>
				</Link>
			</div>

			<Separator className='bg-sidebar-border' />

			{/* Navigation Links */}
			<nav className='flex-1 p-4 space-y-2'>
				{navItems.map(item => {
					const isActive =
						location.pathname === item.href ||
						(item.href !== '/dashboard' &&
							location.pathname.startsWith(item.href))
					const Icon = item.icon

					return (
						<Link
							key={item.href}
							to={item.href}
							onClick={onClose}
							className={cn(
								'flex items-center space-x-3 px-4 py-3 rounded-lg transition-all',
								'hover:bg-sidebar-accent text-sidebar-foreground hover:text-sidebar-accent-foreground',
								isActive &&
									'bg-sidebar-primary text-sidebar-primary-foreground font-medium shadow-sm'
							)}
						>
							<Icon className='h-5 w-5 flex-shrink-0' />
							<span>{item.name}</span>
						</Link>
					)
				})}
			</nav>

			<Separator className='bg-sidebar-border' />

			{/* User Profile Section */}
			<div className='p-4 space-y-3'>
				<div className='flex items-center space-x-3 px-4 py-3 rounded-lg bg-sidebar-accent/50'>
					<Avatar className='h-10 w-10'>
						<AvatarImage
							src={me?.avatarUrl || undefined}
							alt={me ? `${me.firstName} ${me.lastName}` : 'User'}
						/>
						<AvatarFallback className='bg-primary text-primary-foreground'>
							{getUserInitials()}
						</AvatarFallback>
					</Avatar>
					<div className='flex-1 min-w-0'>
						<p className='text-sm font-medium text-sidebar-foreground truncate'>
							{me ? `${me.firstName} ${me.lastName}` : 'User'}
						</p>
						<p className='text-xs text-sidebar-foreground/60 truncate'>
							{me?.role}
						</p>
					</div>
				</div>

				<Button
					variant='outline'
					className='w-full justify-start space-x-3'
					onClick={handleSignOut}
				>
					<LogOut className='h-4 w-4' />
					<span>Sign Out</span>
				</Button>
			</div>
		</aside>
	)
}

export default Sidebar
