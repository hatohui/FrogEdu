import React, { useState } from 'react'
import {
	User,
	Bell,
	Shield,
	Palette,
	Globe,
	Key,
	Mail,
	Smartphone,
	Lock,
	Eye,
	EyeOff,
	Save,
	RefreshCw,
} from 'lucide-react'
import {
	Card,
	CardContent,
	CardHeader,
	CardTitle,
	CardDescription,
} from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Switch } from '@/components/ui/switch'
import { Separator } from '@/components/ui/separator'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { useMe } from '@/hooks/auth/useMe'
import { Textarea } from '@/components/ui/textarea'
import UserAvatar from '@/components/common/UserAvatar'
import { useTranslation } from 'react-i18next'
import { useLanguage } from '@/config/i18n'

const SettingsPage = (): React.ReactElement => {
	const { user } = useMe()
	const { t } = useTranslation()
	const { lang, setLanguage } = useLanguage()
	const [showPassword, setShowPassword] = useState(false)
	const [showNewPassword, setShowNewPassword] = useState(false)
	const [showConfirmPassword, setShowConfirmPassword] = useState(false)

	// Settings states
	const [emailNotifications, setEmailNotifications] = useState(true)
	const [pushNotifications, setPushNotifications] = useState(true)
	const [examReminders, setExamReminders] = useState(true)
	const [classUpdates, setClassUpdates] = useState(true)
	const [marketingEmails, setMarketingEmails] = useState(false)
	const [twoFactorEnabled, setTwoFactorEnabled] = useState(false)
	const [theme, setTheme] = useState('system')
	const [timezone, setTimezone] = useState('UTC')

	const handleLanguageChange = (value: string) => {
		void setLanguage(value)
	}

	const handleSaveProfile = () => {
		console.log('Saving profile...')
		// TODO: Implement profile update API call
	}

	const handleChangePassword = () => {
		console.log('Changing password...')
		// TODO: Implement password change API call
	}

	const handleSaveNotifications = () => {
		console.log('Saving notification preferences...')
		// TODO: Implement notification preferences update
	}

	const handleSaveAppearance = () => {
		console.log('Saving appearance settings...')
		// TODO: Implement appearance settings update
	}

	const handleEnable2FA = () => {
		console.log('Enabling 2FA...')
		// TODO: Implement 2FA setup
	}

	return (
		<div className='space-y-6 p-6 max-w-3xl mx-auto'>
			{/* Header */}
			<div>
				<h1 className='text-3xl font-bold tracking-tight'>
					{t('pages.settings.title')}
				</h1>
				<p className='text-muted-foreground'>{t('pages.settings.subtitle')}</p>
			</div>

			<Tabs defaultValue='profile' className='space-y-4'>
				<TabsList>
					<TabsTrigger value='profile'>
						<User className='mr-2 h-4 w-4' />
						{t('pages.settings.tabs.profile')}
					</TabsTrigger>
					<TabsTrigger value='security'>
						<Shield className='mr-2 h-4 w-4' />
						{t('pages.settings.tabs.security')}
					</TabsTrigger>
					<TabsTrigger value='notifications'>
						<Bell className='mr-2 h-4 w-4' />
						{t('pages.settings.tabs.notifications')}
					</TabsTrigger>
					<TabsTrigger value='appearance'>
						<Palette className='mr-2 h-4 w-4' />
						{t('pages.settings.tabs.appearance')}
					</TabsTrigger>
				</TabsList>

				{/* Profile Tab */}
				<TabsContent value='profile' className='space-y-4'>
					<Card>
						<CardHeader>
							<CardTitle>{t('pages.settings.profile_information')}</CardTitle>
							<CardDescription>
								{t('pages.settings.profile_description')}
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-6'>
							{/* Avatar Section */}
							<div className='flex items-center gap-4'>
								<UserAvatar user={user} size='h-16 w-16' />
								<div>
									<Button variant='outline' size='sm'>
										{t('actions.change_avatar')}
									</Button>
									<p className='text-sm text-muted-foreground mt-2'>
										{t('pages.settings.avatar_limit')}
									</p>
								</div>
							</div>

							<Separator />

							{/* Personal Information */}
							<div className='grid gap-4 md:grid-cols-2'>
								<div className='space-y-2'>
									<Label htmlFor='firstName'>{t('labels.first_name')}</Label>
									<Input
										id='firstName'
										defaultValue={user?.firstName}
										placeholder={t('placeholders.first_name_full')}
									/>
								</div>
								<div className='space-y-2'>
									<Label htmlFor='lastName'>{t('labels.last_name')}</Label>
									<Input
										id='lastName'
										defaultValue={user?.lastName}
										placeholder={t('placeholders.last_name_full')}
									/>
								</div>
							</div>

							<div className='space-y-2'>
								<Label htmlFor='email'>{t('labels.email_address')}</Label>
								<Input
									id='email'
									type='email'
									defaultValue={user?.email}
									placeholder={t('placeholders.email')}
								/>
							</div>

							<div className='space-y-2'>
								<Label htmlFor='phone'>{t('labels.phone_number')}</Label>
								<Input
									id='phone'
									type='tel'
									placeholder={t('placeholders.phone_number')}
								/>
							</div>

							<div className='space-y-2'>
								<Label htmlFor='bio'>{t('labels.bio')}</Label>
								<Textarea
									id='bio'
									placeholder={t('placeholders.bio')}
									rows={4}
								/>
							</div>

							<div className='flex justify-end'>
								<Button onClick={handleSaveProfile}>
									<Save className='mr-2 h-4 w-4' />
									{t('actions.save_changes')}
								</Button>
							</div>
						</CardContent>
					</Card>

					<Card>
						<CardHeader>
							<CardTitle>{t('pages.settings.preferences')}</CardTitle>
							<CardDescription>
								{t('pages.settings.preferences_description')}
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='space-y-2'>
								<Label htmlFor='language'>{t('labels.language')}</Label>
								<Select value={lang} onValueChange={handleLanguageChange}>
									<SelectTrigger>
										<Globe className='mr-2 h-4 w-4' />
										<SelectValue />
									</SelectTrigger>
									<SelectContent>
										<SelectItem value='en'>{t('languages.english')}</SelectItem>
										<SelectItem value='vi'>
											{t('languages.vietnamese')}
										</SelectItem>
									</SelectContent>
								</Select>
							</div>

							<div className='space-y-2'>
								<Label htmlFor='timezone'>{t('labels.timezone')}</Label>
								<Select value={timezone} onValueChange={setTimezone}>
									<SelectTrigger>
										<SelectValue />
									</SelectTrigger>
									<SelectContent>
										<SelectItem value='UTC'>{t('timezones.utc')}</SelectItem>
										<SelectItem value='America/New_York'>
											{t('timezones.eastern')}
										</SelectItem>
										<SelectItem value='America/Los_Angeles'>
											{t('timezones.pacific')}
										</SelectItem>
										<SelectItem value='Asia/Ho_Chi_Minh'>
											{t('timezones.hcm')}
										</SelectItem>
										<SelectItem value='Europe/London'>
											{t('timezones.london')}
										</SelectItem>
									</SelectContent>
								</Select>
							</div>

							<div className='flex justify-end'>
								<Button onClick={handleSaveProfile}>
									<Save className='mr-2 h-4 w-4' />
									{t('actions.save_preferences')}
								</Button>
							</div>
						</CardContent>
					</Card>
				</TabsContent>

				{/* Security Tab */}
				<TabsContent value='security' className='space-y-4'>
					<Card>
						<CardHeader>
							<CardTitle>{t('pages.settings.change_password')}</CardTitle>
							<CardDescription>
								{t('pages.settings.change_password_description')}
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='space-y-2'>
								<Label htmlFor='currentPassword'>
									{t('labels.current_password')}
								</Label>
								<div className='relative'>
									<Input
										id='currentPassword'
										type={showPassword ? 'text' : 'password'}
										placeholder={t('placeholders.current_password')}
									/>
									<Button
										variant='ghost'
										size='icon'
										className='absolute right-0 top-0'
										onClick={() => setShowPassword(!showPassword)}
									>
										{showPassword ? (
											<EyeOff className='h-4 w-4' />
										) : (
											<Eye className='h-4 w-4' />
										)}
									</Button>
								</div>
							</div>

							<div className='space-y-2'>
								<Label htmlFor='newPassword'>{t('labels.new_password')}</Label>
								<div className='relative'>
									<Input
										id='newPassword'
										type={showNewPassword ? 'text' : 'password'}
										placeholder={t('placeholders.new_password')}
									/>
									<Button
										variant='ghost'
										size='icon'
										className='absolute right-0 top-0'
										onClick={() => setShowNewPassword(!showNewPassword)}
									>
										{showNewPassword ? (
											<EyeOff className='h-4 w-4' />
										) : (
											<Eye className='h-4 w-4' />
										)}
									</Button>
								</div>
							</div>

							<div className='space-y-2'>
								<Label htmlFor='confirmPassword'>
									{t('labels.confirm_new_password')}
								</Label>
								<div className='relative'>
									<Input
										id='confirmPassword'
										type={showConfirmPassword ? 'text' : 'password'}
										placeholder={t('placeholders.confirm_new_password')}
									/>
									<Button
										variant='ghost'
										size='icon'
										className='absolute right-0 top-0'
										onClick={() => setShowConfirmPassword(!showConfirmPassword)}
									>
										{showConfirmPassword ? (
											<EyeOff className='h-4 w-4' />
										) : (
											<Eye className='h-4 w-4' />
										)}
									</Button>
								</div>
							</div>

							<div className='flex justify-end'>
								<Button onClick={handleChangePassword}>
									<Lock className='mr-2 h-4 w-4' />
									{t('actions.change_password')}
								</Button>
							</div>
						</CardContent>
					</Card>

					<Card>
						<CardHeader>
							<CardTitle>{t('pages.settings.two_factor')}</CardTitle>
							<CardDescription>
								{t('pages.settings.two_factor_description')}
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='flex items-center justify-between'>
								<div className='space-y-1'>
									<div className='flex items-center gap-2'>
										<Smartphone className='h-4 w-4' />
										<p className='font-medium'>
											{t('labels.authenticator_app')}
										</p>
									</div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.settings.authenticator_description')}
									</p>
								</div>
								<Switch
									checked={twoFactorEnabled}
									onCheckedChange={setTwoFactorEnabled}
								/>
							</div>

							{twoFactorEnabled && (
								<div className='rounded-lg border p-4 space-y-3'>
									<p className='text-sm font-medium'>
										{t('pages.settings.authenticator_setup_title')}
									</p>
									<p className='text-sm text-muted-foreground'>
										{t('pages.settings.authenticator_setup_description')}
									</p>
									<div className='flex justify-center py-4'>
										<div className='h-48 w-48 bg-muted rounded-lg flex items-center justify-center'>
											<p className='text-sm text-muted-foreground'>
												{t('pages.settings.qr_placeholder')}
											</p>
										</div>
									</div>
									<div className='space-y-2'>
										<Label htmlFor='verificationCode'>
											{t('labels.verification_code')}
										</Label>
										<Input
											id='verificationCode'
											placeholder={t('placeholders.verification_code')}
											maxLength={6}
										/>
									</div>
									<Button className='w-full' onClick={handleEnable2FA}>
										<Key className='mr-2 h-4 w-4' />
										{t('actions.enable_two_factor')}
									</Button>
								</div>
							)}
						</CardContent>
					</Card>

					<Card>
						<CardHeader>
							<CardTitle>{t('pages.settings.active_sessions')}</CardTitle>
							<CardDescription>
								{t('pages.settings.active_sessions_description')}
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='rounded-lg border p-4'>
								<div className='flex items-center justify-between'>
									<div className='space-y-1'>
										<p className='font-medium'>{t('labels.current_session')}</p>
										<p className='text-sm text-muted-foreground'>
											{t('pages.settings.session_location')}
										</p>
										<p className='text-xs text-muted-foreground'>
											{t('pages.settings.active_now')}
										</p>
									</div>
									<Badge variant='outline' className='text-green-600'>
										{t('badges.active')}
									</Badge>
								</div>
							</div>

							<div className='flex justify-end'>
								<Button variant='outline'>
									<RefreshCw className='mr-2 h-4 w-4' />
									{t('actions.revoke_sessions')}
								</Button>
							</div>
						</CardContent>
					</Card>
				</TabsContent>

				{/* Notifications Tab */}
				<TabsContent value='notifications' className='space-y-4'>
					<Card>
						<CardHeader>
							<CardTitle>{t('pages.settings.email_notifications')}</CardTitle>
							<CardDescription>
								{t('pages.settings.email_notifications_description')}
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='flex items-center justify-between'>
								<div className='space-y-1'>
									<div className='flex items-center gap-2'>
										<Mail className='h-4 w-4' />
										<p className='font-medium'>
											{t('labels.email_notifications')}
										</p>
									</div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.settings.email_activity_updates')}
									</p>
								</div>
								<Switch
									checked={emailNotifications}
									onCheckedChange={setEmailNotifications}
								/>
							</div>

							<Separator />

							<div className='flex items-center justify-between'>
								<div className='space-y-1'>
									<p className='font-medium'>{t('labels.exam_reminders')}</p>
									<p className='text-sm text-muted-foreground'>
										{t('pages.settings.exam_reminders_description')}
									</p>
								</div>
								<Switch
									checked={examReminders}
									onCheckedChange={setExamReminders}
									disabled={!emailNotifications}
								/>
							</div>

							<div className='flex items-center justify-between'>
								<div className='space-y-1'>
									<p className='font-medium'>{t('labels.class_updates')}</p>
									<p className='text-sm text-muted-foreground'>
										{t('pages.settings.class_updates_description')}
									</p>
								</div>
								<Switch
									checked={classUpdates}
									onCheckedChange={setClassUpdates}
									disabled={!emailNotifications}
								/>
							</div>

							<div className='flex items-center justify-between'>
								<div className='space-y-1'>
									<p className='font-medium'>{t('labels.marketing_emails')}</p>
									<p className='text-sm text-muted-foreground'>
										{t('pages.settings.marketing_emails_description')}
									</p>
								</div>
								<Switch
									checked={marketingEmails}
									onCheckedChange={setMarketingEmails}
									disabled={!emailNotifications}
								/>
							</div>
						</CardContent>
					</Card>

					<Card>
						<CardHeader>
							<CardTitle>{t('pages.settings.push_notifications')}</CardTitle>
							<CardDescription>
								{t('pages.settings.push_notifications_description')}
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='flex items-center justify-between'>
								<div className='space-y-1'>
									<div className='flex items-center gap-2'>
										<Bell className='h-4 w-4' />
										<p className='font-medium'>
											{t('labels.push_notifications')}
										</p>
									</div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.settings.push_notifications_devices')}
									</p>
								</div>
								<Switch
									checked={pushNotifications}
									onCheckedChange={setPushNotifications}
								/>
							</div>
						</CardContent>
					</Card>

					<div className='flex justify-end'>
						<Button onClick={handleSaveNotifications}>
							<Save className='mr-2 h-4 w-4' />
							{t('actions.save_preferences')}
						</Button>
					</div>
				</TabsContent>

				{/* Appearance Tab */}
				<TabsContent value='appearance' className='space-y-4'>
					<Card>
						<CardHeader>
							<CardTitle>{t('pages.settings.theme')}</CardTitle>
							<CardDescription>
								{t('pages.settings.theme_description')}
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='space-y-2'>
								<Label>{t('labels.theme_mode')}</Label>
								<Select value={theme} onValueChange={setTheme}>
									<SelectTrigger>
										<Palette className='mr-2 h-4 w-4' />
										<SelectValue />
									</SelectTrigger>
									<SelectContent>
										<SelectItem value='light'>{t('options.light')}</SelectItem>
										<SelectItem value='dark'>{t('options.dark')}</SelectItem>
										<SelectItem value='system'>
											{t('options.system')}
										</SelectItem>
									</SelectContent>
								</Select>
								<p className='text-sm text-muted-foreground'>
									{t('pages.settings.theme_hint')}
								</p>
							</div>
						</CardContent>
					</Card>

					<Card>
						<CardHeader>
							<CardTitle>{t('pages.settings.display_preferences')}</CardTitle>
							<CardDescription>
								{t('pages.settings.display_preferences_description')}
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='flex items-center justify-between'>
								<div className='space-y-1'>
									<p className='font-medium'>{t('labels.compact_mode')}</p>
									<p className='text-sm text-muted-foreground'>
										{t('pages.settings.compact_mode_description')}
									</p>
								</div>
								<Switch />
							</div>

							<div className='flex items-center justify-between'>
								<div className='space-y-1'>
									<p className='font-medium'>{t('labels.show_animations')}</p>
									<p className='text-sm text-muted-foreground'>
										{t('pages.settings.show_animations_description')}
									</p>
								</div>
								<Switch defaultChecked />
							</div>
						</CardContent>
					</Card>

					<div className='flex justify-end'>
						<Button onClick={handleSaveAppearance}>
							<Save className='mr-2 h-4 w-4' />
							{t('actions.save_preferences')}
						</Button>
					</div>
				</TabsContent>
			</Tabs>
		</div>
	)
}

// Badge component for active session
const Badge = ({
	children,
	variant = 'default',
	className = '',
}: {
	children: React.ReactNode
	variant?: 'default' | 'outline'
	className?: string
}) => {
	return (
		<span
			className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-semibold ${
				variant === 'outline'
					? 'border border-current'
					: 'bg-primary text-primary-foreground'
			} ${className}`}
		>
			{children}
		</span>
	)
}

export default SettingsPage
