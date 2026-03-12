import React from 'react'
import { Link, useLocation } from 'react-router'
import { cn } from '@/utils/shadcn'
import {
	Home,
	FileText,
	User,
	LogOut,
	X,
	ChevronLeft,
	ChevronRight,
	LayoutDashboard,
	Grid3x3,
	Users,
	ClipboardList,
	CalendarDays,
	CreditCard,
	Eye,
	GraduationCap,
	PencilRuler,
	ShieldCheck,
} from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { Skeleton } from '@/components/ui/skeleton'
import { useMe } from '@/hooks/auth/useMe'
import { useEffectiveRole } from '@/hooks/useEffectiveRole'
import { useViewAsStore } from '@/stores/viewAsStore'
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
	adminOnly?: boolean
	teacherOnly?: boolean
	studentOnly?: boolean
	roles?: string[] // explicit roles that can see this item
}

const navItems: NavItem[] = [
	{
		labelKey: 'navigation.dashboard',
		href: '/dashboard',
		icon: LayoutDashboard,
		adminOnly: true,
	},
	{
		labelKey: 'navigation.app',
		href: '/app',
		icon: Home,
	},
	{
		labelKey: 'navigation.exams',
		href: '/app/exams',
		icon: FileText,
		roles: ['Admin', 'Teacher'],
	},
	{
		labelKey: 'navigation.classes',
		href: '/app/classes',
		icon: Users,
	},
	{
		labelKey: 'navigation.exam_sessions',
		href: '/app/exam-sessions',
		icon: ClipboardList,
	},
	{
		labelKey: 'navigation.calendar',
		href: '/app/calendar',
		icon: CalendarDays,
		studentOnly: true,
	},
	{
		labelKey: 'navigation.matrices',
		href: '/app/matrices',
		icon: Grid3x3,
		roles: ['Admin', 'Teacher'],
	},
	{
		labelKey: 'navigation.subscription',
		href: '/profile/subscription',
		icon: CreditCard,
	},
	{
		labelKey: 'navigation.profile',
		href: '/profile',
		icon: User,
	},
]

interface SidebarProps {
	className?: string
	onClose?: () => void
	collapsed?: boolean
	onToggleCollapse?: () => void
}

const Sidebar = ({
	className,
	onClose,
	collapsed = false,
	onToggleCollapse,
}: SidebarProps): React.ReactElement => {
	const location = useLocation()
	const { user: me, signOut, isLoading } = useMe()
	const { effectiveRole, isActualAdmin, isViewingAs, viewAs } =
		useEffectiveRole()
	const { setViewAs, clearViewAs } = useViewAsStore()
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
						to='/'
						className='flex items-center space-x-3 group'
						onClick={onClose}
					>
						<div className='w-10 h-10 rounded-lg flex items-center justify-center text-primary-foreground font-bold text-xl transition-transform group-hover:scale-105'>
							<img
								src='/frog.svg'
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
									{t('common.learning_platform')}
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

				{/* Navigation Links */}
				<nav className='flex-1 p-4 space-y-2 overflow-y-auto min-h-0'>
					{isLoading
						? Array.from({ length: 5 }).map((_, i) => (
								<div
									key={i}
									className={cn(
										'flex items-center px-4 py-3 rounded-lg',
										collapsed ? 'justify-center' : 'space-x-3'
									)}
								>
									<Skeleton className='h-5 w-5 rounded flex-shrink-0' />
									{!collapsed && <Skeleton className='h-4 flex-1' />}
								</div>
							))
						: navItems
								.filter(item => {
									const userRole = effectiveRole
									const isAdmin = userRole === 'Admin'
									const isTeacher = userRole === 'Teacher'
									const isStudent = userRole === 'Student'

									if (item.adminOnly) return isAdmin
									if (item.teacherOnly) return isTeacher || isAdmin
									if (item.studentOnly) return isStudent
									if (item.roles) return item.roles.includes(userRole ?? '')
									return true
								})
								.map(item => {
									const isActive =
										location.pathname === item.href ||
										(item.href !== '/app' &&
											item.href !== '/dashboard' &&
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
				</nav>

				{/* View As — admin only */}
				{isActualAdmin && (
					<>
						<Separator className='bg-sidebar-border flex-shrink-0' />
						<div className='p-4 flex-shrink-0 space-y-2'>
							{!collapsed && (
								<p className='text-xs font-semibold text-sidebar-foreground/50 uppercase tracking-wide px-1 flex items-center gap-1.5'>
									<Eye className='h-3 w-3' />
									{t('navigation.view_as', 'View As')}
								</p>
							)}
							<div
								className={cn(
									'flex gap-1',
									collapsed ? 'flex-col items-center' : 'flex-row'
								)}
							>
								{/* Admin button */}
								{collapsed ? (
									<Tooltip>
										<TooltipTrigger asChild>
											<Button
												size='icon'
												variant={!isViewingAs ? 'default' : 'ghost'}
												className='h-8 w-8'
												onClick={clearViewAs}
											>
												<ShieldCheck className='h-4 w-4' />
											</Button>
										</TooltipTrigger>
										<TooltipContent side='right'>
											<p>{t('navigation.view_as_admin', 'Admin')}</p>
										</TooltipContent>
									</Tooltip>
								) : (
									<Button
										size='sm'
										variant={!isViewingAs ? 'default' : 'ghost'}
										className='flex-1 h-8 text-xs gap-1'
										onClick={clearViewAs}
									>
										<ShieldCheck className='h-3.5 w-3.5' />
										{t('navigation.view_as_admin', 'Admin')}
									</Button>
								)}

								{/* Teacher button */}
								{collapsed ? (
									<Tooltip>
										<TooltipTrigger asChild>
											<Button
												size='icon'
												variant={viewAs === 'Teacher' ? 'default' : 'ghost'}
												className='h-8 w-8'
												onClick={() => setViewAs('Teacher')}
											>
												<PencilRuler className='h-4 w-4' />
											</Button>
										</TooltipTrigger>
										<TooltipContent side='right'>
											<p>{t('navigation.view_as_teacher', 'Teacher')}</p>
										</TooltipContent>
									</Tooltip>
								) : (
									<Button
										size='sm'
										variant={viewAs === 'Teacher' ? 'default' : 'ghost'}
										className='flex-1 h-8 text-xs gap-1'
										onClick={() => setViewAs('Teacher')}
									>
										<PencilRuler className='h-3.5 w-3.5' />
										{t('navigation.view_as_teacher', 'Teacher')}
									</Button>
								)}

								{/* Student button */}
								{collapsed ? (
									<Tooltip>
										<TooltipTrigger asChild>
											<Button
												size='icon'
												variant={viewAs === 'Student' ? 'default' : 'ghost'}
												className='h-8 w-8'
												onClick={() => setViewAs('Student')}
											>
												<GraduationCap className='h-4 w-4' />
											</Button>
										</TooltipTrigger>
										<TooltipContent side='right'>
											<p>{t('navigation.view_as_student', 'Student')}</p>
										</TooltipContent>
									</Tooltip>
								) : (
									<Button
										size='sm'
										variant={viewAs === 'Student' ? 'default' : 'ghost'}
										className='flex-1 h-8 text-xs gap-1'
										onClick={() => setViewAs('Student')}
									>
										<GraduationCap className='h-3.5 w-3.5' />
										{t('navigation.view_as_student', 'Student')}
									</Button>
								)}
							</div>
						</div>
					</>
				)}

				<Separator className='bg-sidebar-border flex-shrink-0' />

				{/* User Profile Section */}
				<div className='p-4 space-y-3 flex-shrink-0'>
					{!collapsed ? (
						<>
							<div className='flex items-center space-x-3 px-4 py-3 rounded-lg bg-sidebar-accent/50'>
								{isLoading ? (
									<Skeleton className='h-8 w-8 rounded-full flex-shrink-0' />
								) : (
									<UserAvatar user={me} />
								)}
								<div className='flex-1 min-w-0'>
									{isLoading ? (
										<div className='space-y-1.5'>
											<Skeleton className='h-3.5 w-3/4' />
											<Skeleton className='h-3 w-1/2' />
										</div>
									) : (
										<>
											<p className='text-sm font-medium text-sidebar-foreground truncate'>
												{me
													? [me.firstName, me.lastName]
															.filter(Boolean)
															.join(' ') || me.email
													: t('roles.user')}
											</p>
											<p className='text-xs text-sidebar-foreground/60 truncate'>
												{me?.role?.name || t('roles.user')}
											</p>
										</>
									)}
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
										{isLoading ? (
											<Skeleton className='h-8 w-8 rounded-full' />
										) : (
											<UserAvatar user={me} />
										)}
									</div>
								</TooltipTrigger>
								<TooltipContent side='right'>
									<p className='font-medium'>
										{me
											? [me.firstName, me.lastName].filter(Boolean).join(' ') ||
												me.email
											: t('roles.user')}
									</p>
									<p className='text-xs text-muted-foreground'>
										{me?.role?.name || t('roles.user')}
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

export default Sidebar
