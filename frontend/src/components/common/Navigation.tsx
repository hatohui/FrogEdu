import React from 'react'
import { Link, useLocation, useNavigate } from 'react-router'
import { Button } from '@/components/ui/button'
import {
	Home,
	Sun,
	Moon,
	LogOut,
	User,
	Settings,
	Shield,
	BookOpen,
	GraduationCap,
	Crown,
	CreditCard,
} from 'lucide-react'
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuLabel,
	DropdownMenuSeparator,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import { Badge } from '@/components/ui/badge'
import { useTheme } from '@/config/theme'
import UserAvatar from './UserAvatar'
import { useMe } from '@/hooks/auth/useMe'
import { useSubscription } from '@/hooks/useSubscription'
import { toast } from 'sonner'
import { useTranslation } from 'react-i18next'
import { useLanguage } from '@/config/i18n'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'

const Navigation = (): React.JSX.Element => {
	const location = useLocation()
	const navigate = useNavigate()
	const [theme, toggleTheme] = useTheme()
	const { user, signOutThenNavigate } = useMe()
	const { isPro } = useSubscription()
	const { t } = useTranslation()
	const { lang, setLanguage } = useLanguage()

	const isAdmin = user?.role?.name === 'Admin'
	const isTeacher = user?.role?.name === 'Teacher'
	const isStudent = user?.role?.name === 'Student'

	const handleSignOut = () => {
		signOutThenNavigate('/login')
		toast.success(t('messages.sign_out_success'))
	}

	const handleLanguageChange = (value: string) => {
		void setLanguage(value)
	}

	const staticNavItems = [
		{ path: '/', label: t('navigation.home'), icon: Home },
		{ path: '/about', label: t('navigation.about'), icon: Home },
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
								alt={t('common.logo_alt')}
								className='w-full h-full object-contain'
							/>
						</div>
						<span className='text-xl font-bold'>{t('common.app_name')}</span>
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

						{user ? (
							<>
								{/* Role Badge */}
								{isAdmin && (
									<Badge className='bg-gradient-to-r from-purple-600 to-purple-700 text-white hidden sm:flex'>
										<Shield className='h-3 w-3 mr-1' />
										{t('roles.admin')}
									</Badge>
								)}
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
								{!isAdmin && (
									<Badge
										className='hidden sm:flex cursor-pointer'
										variant={isPro ? 'default' : 'secondary'}
										onClick={() => navigate('/profile/subscription')}
									>
										{isPro ? (
											<>
												<Crown className='h-3 w-3 mr-1' />
												{t('badges.pro')}
											</>
										) : (
											<>
												<CreditCard className='h-3 w-3 mr-1' />
												{t('badges.free')}
											</>
										)}
									</Badge>
								)}

								<Button
									variant='outline'
									size='sm'
									onClick={() => navigate(isAdmin ? '/dashboard' : '/app')}
									className='hidden sm:flex items-center gap-2'
								>
									<Home className='h-4 w-4' />
									<span>
										{isAdmin ? t('navigation.dashboard') : t('navigation.app')}
									</span>
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
											<div className='flex items-center gap-2 flex-wrap'>
												<span className='text-sm font-medium'>
													{user?.firstName || user?.lastName || t('roles.user')}
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
							</>
						) : (
							<Button variant='outline' size='sm' asChild>
								<Link to='/login'>{t('actions.login')}</Link>
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
