import React from 'react'
import { Link, useLocation } from 'react-router'
import { cn } from '@/utils/shadcn'
import {
	Home,
	BookOpen,
	FileText,
	User,
	LogOut,
	X,
	Users,
	ChevronLeft,
	ChevronRight,
	LayoutDashboard,
	GraduationCap,
	Settings,
	BarChart3,
	Shield,
	Grid3x3,
	Clock,
} from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { useMe } from '@/hooks/auth/useMe'
import UserAvatar from '../common/UserAvatar'
import {
	Tooltip,
	TooltipContent,
	TooltipProvider,
	TooltipTrigger,
} from '@/components/ui/tooltip'
import { useTranslation } from 'react-i18next'

interface NavItem {
	labelKey: string
	href: string
	icon: React.ComponentType<{ className?: string }>
}

const adminNavItems: NavItem[] = [
	{
		labelKey: 'navigation.overview',
		href: '/dashboard',
		icon: LayoutDashboard,
	},
	{
		labelKey: 'navigation.subjects',
		href: '/dashboard/subjects',
		icon: GraduationCap,
	},
	{
		labelKey: 'navigation.users',
		href: '/dashboard/users',
		icon: Users,
	},
	{
		labelKey: 'navigation.classes',
		href: '/dashboard/classes',
		icon: GraduationCap,
	},
	{
		labelKey: 'navigation.exam_sessions',
		href: '/dashboard/exam-sessions',
		icon: Clock,
	},
	{
		labelKey: 'navigation.analytics',
		href: '/dashboard/analytics',
		icon: BarChart3,
	},
	{
		labelKey: 'navigation.settings',
		href: '/dashboard/settings',
		icon: Settings,
	},
]

const appNavItems: NavItem[] = [
	{
		labelKey: 'navigation.app_home',
		href: '/app',
		icon: Home,
	},
	{
		labelKey: 'navigation.my_classes',
		href: '/app/classes',
		icon: Users,
	},
	{
		labelKey: 'navigation.content_library',
		href: '/app/content',
		icon: BookOpen,
	},
	{
		labelKey: 'navigation.exams',
		href: '/app/exams',
		icon: FileText,
	},
	{
		labelKey: 'navigation.matrices',
		href: '/app/matrices',
		icon: Grid3x3,
	},
	{
		labelKey: 'navigation.profile',
		href: '/profile',
		icon: User,
	},
]

interface AdminSidebarProps {
	className?: string
	onClose?: () => void
	collapsed?: boolean
	onToggleCollapse?: () => void
}

const AdminSidebar = ({
	className,
	onClose,
	collapsed = false,
	onToggleCollapse,
}: AdminSidebarProps): React.ReactElement => {
	const location = useLocation()
	const { user: me, signOut } = useMe()
	const { t } = useTranslation()

	const handleSignOut = async () => {
		await signOut()
		if (onClose) onClose()
	}

	const handleWheel = (e: React.WheelEvent) => {
		e.stopPropagation()
	}

	return (
		<TooltipProvider>
			<aside
				className={cn(
					'flex flex-col h-full bg-sidebar border-r border-sidebar-border',
					className
				)}
				onWheel={handleWheel}
			>
				{/* Close button for mobile */}
				{onClose && (
					<div className='flex justify-end p-4 lg:hidden flex-shrink-0'>
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
				<div className={cn('p-6 space-y-2 flex-shrink-0', collapsed && 'p-4')}>
					<Link
						to='/dashboard'
						className='flex items-center space-x-3 group'
						onClick={onClose}
					>
						<div className='w-10 h-10 rounded-lg bg-primary flex items-center justify-center text-primary-foreground font-bold text-xl transition-transform group-hover:scale-105'>
							<img
								src='/frog.png'
								alt={t('common.logo_alt')}
								className='w-full h-full object-contain'
							/>
						</div>
						{!collapsed && (
							<div className='flex flex-col'>
								<span className='font-bold text-lg text-sidebar-foreground'>
									{t('common.app_name')}
								</span>
								<span className='text-xs text-sidebar-foreground/60'>
									{t('common.admin_dashboard')}
								</span>
							</div>
						)}
					</Link>
				</div>

				<Separator className='bg-sidebar-border flex-shrink-0' />

				{/* Collapse Toggle Button */}
				{onToggleCollapse && (
					<div className='px-4 py-2 hidden lg:block flex-shrink-0'>
						<Button
							variant='ghost'
							size='sm'
							onClick={onToggleCollapse}
							className='w-full justify-center'
						>
							{collapsed ? (
								<ChevronRight className='h-4 w-4' />
							) : (
								<ChevronLeft className='h-4 w-4' />
							)}
							{!collapsed && (
								<span className='ml-2'>{t('actions.collapse')}</span>
							)}
						</Button>
					</div>
				)}

				{/* Admin Navigation Section */}
				<div className='flex-1 overflow-y-auto min-h-0'>
					<div className='p-4 space-y-2'>
						{!collapsed && (
							<div className='px-4 mb-2'>
								<div className='flex items-center gap-2 text-xs font-semibold text-muted-foreground uppercase tracking-wide'>
									<Shield className='h-3 w-3' />
									<span>{t('navigation.admin_panel')}</span>
								</div>
							</div>
						)}

						{adminNavItems.map(item => {
							const isActive =
								location.pathname === item.href ||
								(item.href !== '/dashboard' &&
									location.pathname.startsWith(item.href))
							const Icon = item.icon
							const label = t(item.labelKey)

							const linkContent = (
								<Link
									key={item.href}
									to={item.href}
									onClick={onClose}
									className={cn(
										'flex items-center px-4 py-3 rounded-lg transition-all',
										'hover:bg-sidebar-accent text-sidebar-foreground hover:text-sidebar-accent-foreground',
										isActive &&
											'bg-sidebar-primary text-sidebar-primary-foreground font-medium shadow-sm',
										collapsed ? 'justify-center' : 'space-x-3'
									)}
								>
									<Icon className='h-5 w-5 flex-shrink-0' />
									{!collapsed && <span>{label}</span>}
								</Link>
							)

							return collapsed ? (
								<Tooltip key={item.href}>
									<TooltipTrigger asChild>{linkContent}</TooltipTrigger>
									<TooltipContent side='right'>
										<p>{label}</p>
									</TooltipContent>
								</Tooltip>
							) : (
								linkContent
							)
						})}
					</div>

					<Separator className='my-4 bg-sidebar-border' />

					{/* App Navigation Section */}
					<div className='p-4 space-y-2'>
						{!collapsed && (
							<div className='px-4 mb-2'>
								<div className='flex items-center gap-2 text-xs font-semibold text-muted-foreground uppercase tracking-wide'>
									<Home className='h-3 w-3' />
									<span>{t('navigation.quick_access')}</span>
								</div>
							</div>
						)}

						{appNavItems.map(item => {
							const isActive =
								location.pathname === item.href ||
								(item.href !== '/app' &&
									location.pathname.startsWith(item.href))
							const Icon = item.icon
							const label = t(item.labelKey)

							const linkContent = (
								<Link
									key={item.href}
									to={item.href}
									onClick={onClose}
									className={cn(
										'flex items-center px-4 py-3 rounded-lg transition-all',
										'hover:bg-sidebar-accent text-sidebar-foreground hover:text-sidebar-accent-foreground',
										isActive &&
											'bg-sidebar-primary text-sidebar-primary-foreground font-medium shadow-sm',
										collapsed ? 'justify-center' : 'space-x-3'
									)}
								>
									<Icon className='h-5 w-5 flex-shrink-0' />
									{!collapsed && <span>{label}</span>}
								</Link>
							)

							return collapsed ? (
								<Tooltip key={item.href}>
									<TooltipTrigger asChild>{linkContent}</TooltipTrigger>
									<TooltipContent side='right'>
										<p>{label}</p>
									</TooltipContent>
								</Tooltip>
							) : (
								linkContent
							)
						})}
					</div>
				</div>

				<Separator className='bg-sidebar-border flex-shrink-0' />

				{/* User Profile Section */}
				<div className='p-4 space-y-3 flex-shrink-0'>
					{!collapsed ? (
						<>
							<div className='flex items-center space-x-3 px-4 py-3 rounded-lg bg-sidebar-accent/50'>
								<UserAvatar user={me} />
								<div className='flex-1 min-w-0'>
									<p className='text-sm font-medium text-sidebar-foreground truncate'>
										{me
											? [me.firstName, me.lastName].filter(Boolean).join(' ') ||
												me.email
											: t('roles.administrator')}
									</p>
									<p className='text-xs text-sidebar-foreground/60 truncate'>
										{me?.role?.name || t('roles.administrator')}
									</p>
								</div>
							</div>

							<Button
								variant='outline'
								className='w-full justify-start space-x-3'
								onClick={handleSignOut}
							>
								<LogOut className='h-4 w-4' />
								<span>{t('actions.sign_out')}</span>
							</Button>
						</>
					) : (
						<>
							<Tooltip>
								<TooltipTrigger asChild>
									<div className='flex justify-center'>
										<UserAvatar user={me} />
									</div>
								</TooltipTrigger>
								<TooltipContent side='right'>
									<p className='font-medium'>
										{me
											? [me.firstName, me.lastName].filter(Boolean).join(' ') ||
												me.email
											: t('roles.administrator')}
									</p>
									<p className='text-xs text-muted-foreground'>
										{me?.role?.name || t('roles.administrator')}
									</p>
								</TooltipContent>
							</Tooltip>

							<Tooltip>
								<TooltipTrigger asChild>
									<Button
										variant='outline'
										className='w-full justify-center'
										onClick={handleSignOut}
									>
										<LogOut className='h-4 w-4' />
									</Button>
								</TooltipTrigger>
								<TooltipContent side='right'>
									<p>{t('actions.sign_out')}</p>
								</TooltipContent>
							</Tooltip>
						</>
					)}
				</div>
			</aside>
		</TooltipProvider>
	)
}

export default AdminSidebar
