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
	Shield,
	BookOpen,
	GraduationCap,
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
import { useTranslation } from 'react-i18next'
import { useLanguage } from '@/config/i18n'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'

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
	const { t } = useTranslation()
	const { lang, setLanguage } = useLanguage()

	const isAdmin = user?.role?.name === 'Admin'
	const isTeacher = user?.role?.name === 'Teacher'
	const isStudent = user?.role?.name === 'Student'

	const handleSignOut = async () => {
		await signOut()
	}

	const handleLanguageChange = (value: string) => {
		void setLanguage(value)
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
						: t('navigation.app')}
				</h1>
			</div>

			{/* Right side - Actions & User Menu */}
			<div className='flex items-center space-x-2'>
				{/* Role/Subscription Badge */}
				{user && (
					<>
						{isAdmin ? (
							<Badge className='bg-gradient-to-r from-purple-600 to-purple-700 text-white hidden sm:flex'>
								<Shield className='h-3 w-3 mr-1' />
								{t('roles.admin')}
							</Badge>
						) : (
							<>
								{/* Role Badge for Teacher/Student */}
								{isTeacher && (
									<Badge className='bg-gradient-to-r from-blue-600 to-blue-700 text-white hidden sm:flex'>
										<BookOpen className='h-3 w-3 mr-1' />
										{t('roles.teacher')}
									</Badge>
								)}
								{isStudent && (
									<Badge className='bg-gradient-to-r from-green-600 to-green-700 text-white hidden sm:flex'>
										<GraduationCap className='h-3 w-3 mr-1' />
										{t('roles.student')}
									</Badge>
								)}

								{/* Subscription Badge for non-admins */}
								<Button
									variant='ghost'
									size='sm'
									onClick={() => navigate('/profile/subscription')}
									className='hidden sm:flex items-center gap-2'
								>
									{isPro ? (
										<Badge className='bg-gradient-to-r from-amber-500 to-orange-500 text-white'>
											<Crown className='h-3 w-3 mr-1' />
											{t('badges.pro')}
										</Badge>
									) : (
										<Badge variant='secondary'>
											<CreditCard className='h-3 w-3 mr-1' />
											{t('badges.free')}
										</Badge>
									)}
								</Button>
							</>
						)}
					</>
				)}

				{/* Dark Mode Toggle */}
				<Button
					variant='ghost'
					size='icon'
					onClick={toggleTheme}
					className='h-10 w-10'
					title={
						theme === 'light'
							? t('actions.switch_to_dark')
							: t('actions.switch_to_light')
					}
				>
					{theme === 'light' ? (
						<Moon className='h-5 w-5' />
					) : (
						<Sun className='h-5 w-5' />
					)}
				</Button>

				<Select value={lang} onValueChange={handleLanguageChange}>
					<SelectTrigger className='h-10 w-[110px]'>
						<SelectValue placeholder={t('config.select_language')} />
					</SelectTrigger>
					<SelectContent>
						<SelectItem value='en'>{t('languages.english')}</SelectItem>
						<SelectItem value='vi'>{t('languages.vietnamese')}</SelectItem>
					</SelectContent>
				</Select>

				{/* Admin Navigation Buttons */}
				{user?.role?.name === 'Admin' && (
					<>
						<Button
							variant='outline'
							size='sm'
							onClick={() => navigate('/dashboard')}
							className='hidden md:flex items-center gap-2'
						>
							<Home className='h-4 w-4' />
							<span>{t('navigation.dashboard')}</span>
						</Button>
						<Button
							variant='outline'
							size='sm'
							onClick={() => navigate('/app')}
							className='hidden md:flex items-center gap-2'
						>
							<Home className='h-4 w-4' />
							<span>{t('navigation.app')}</span>
						</Button>
					</>
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
									{user?.firstName || user?.email || t('roles.user')}
								</span>
								{/* Role Badge */}
								{isAdmin && (
									<Badge className='bg-gradient-to-r from-purple-600 to-purple-700 text-white text-xs px-1.5 py-0'>
										{t('roles.admin')}
									</Badge>
								)}
								{isTeacher && (
									<Badge className='bg-gradient-to-r from-blue-600 to-blue-700 text-white text-xs px-1.5 py-0'>
										{t('roles.teacher')}
									</Badge>
								)}
								{isStudent && (
									<Badge className='bg-gradient-to-r from-green-600 to-green-700 text-white text-xs px-1.5 py-0'>
										{t('roles.student')}
									</Badge>
								)}
								{/* Subscription Badge for non-admins */}
								{!isAdmin && isPro && (
									<Badge className='bg-gradient-to-r from-amber-500 to-orange-500 text-white text-xs px-1.5 py-0'>
										{t('badges.pro')}
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
								<span>{t('navigation.profile')}</span>
							</Link>
						</DropdownMenuItem>

						<DropdownMenuItem asChild>
							<Link to='/profile/subscription' className='cursor-pointer'>
								<CreditCard className='mr-2 h-4 w-4' />
								<span>{t('navigation.subscription')}</span>
								{!isAdmin && !isPro && (
									<Badge
										variant='outline'
										className='ml-auto text-xs border-amber-500 text-amber-600'
									>
										{t('badges.upgrade')}
									</Badge>
								)}
							</Link>
						</DropdownMenuItem>

						<DropdownMenuItem asChild>
							<Link to='/settings' className='cursor-pointer'>
								<Settings className='mr-2 h-4 w-4' />
								<span>{t('navigation.settings')}</span>
							</Link>
						</DropdownMenuItem>

						<DropdownMenuSeparator />

						<DropdownMenuItem onClick={handleSignOut}>
							<LogOut className='mr-2 h-4 w-4' />
							<span>{t('actions.logout')}</span>
						</DropdownMenuItem>
					</DropdownMenuContent>
				</DropdownMenu>
			</div>
		</header>
	)
}

export default Header
