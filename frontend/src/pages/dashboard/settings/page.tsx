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
import { Avatar, AvatarFallback } from '@/components/ui/avatar'
import { useMe } from '@/hooks/auth/useMe'
import { Textarea } from '@/components/ui/textarea'

const SettingsPage = (): React.ReactElement => {
	const { user } = useMe()
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
	const [language, setLanguage] = useState('en')
	const [theme, setTheme] = useState('system')
	const [timezone, setTimezone] = useState('UTC')

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
				<h1 className='text-3xl font-bold tracking-tight'>Settings</h1>
				<p className='text-muted-foreground'>
					Manage your account settings and preferences
				</p>
			</div>

			<Tabs defaultValue='profile' className='space-y-4'>
				<TabsList>
					<TabsTrigger value='profile'>
						<User className='mr-2 h-4 w-4' />
						Profile
					</TabsTrigger>
					<TabsTrigger value='security'>
						<Shield className='mr-2 h-4 w-4' />
						Security
					</TabsTrigger>
					<TabsTrigger value='notifications'>
						<Bell className='mr-2 h-4 w-4' />
						Notifications
					</TabsTrigger>
					<TabsTrigger value='appearance'>
						<Palette className='mr-2 h-4 w-4' />
						Appearance
					</TabsTrigger>
				</TabsList>

				{/* Profile Tab */}
				<TabsContent value='profile' className='space-y-4'>
					<Card>
						<CardHeader>
							<CardTitle>Profile Information</CardTitle>
							<CardDescription>
								Update your personal information and public profile
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-6'>
							{/* Avatar Section */}
							<div className='flex items-center gap-4'>
								<Avatar className='h-20 w-20'>
									<AvatarFallback className='text-2xl'>
										{user?.firstName?.[0]}
										{user?.lastName?.[0]}
									</AvatarFallback>
								</Avatar>
								<div>
									<Button variant='outline' size='sm'>
										Change Avatar
									</Button>
									<p className='text-sm text-muted-foreground mt-2'>
										JPG, PNG or GIF. Max size 2MB.
									</p>
								</div>
							</div>

							<Separator />

							{/* Personal Information */}
							<div className='grid gap-4 md:grid-cols-2'>
								<div className='space-y-2'>
									<Label htmlFor='firstName'>First Name</Label>
									<Input
										id='firstName'
										defaultValue={user?.firstName}
										placeholder='Enter your first name'
									/>
								</div>
								<div className='space-y-2'>
									<Label htmlFor='lastName'>Last Name</Label>
									<Input
										id='lastName'
										defaultValue={user?.lastName}
										placeholder='Enter your last name'
									/>
								</div>
							</div>

							<div className='space-y-2'>
								<Label htmlFor='email'>Email Address</Label>
								<Input
									id='email'
									type='email'
									defaultValue={user?.email}
									placeholder='Enter your email'
								/>
							</div>

							<div className='space-y-2'>
								<Label htmlFor='phone'>Phone Number</Label>
								<Input id='phone' type='tel' placeholder='+1 (555) 123-4567' />
							</div>

							<div className='space-y-2'>
								<Label htmlFor='bio'>Bio</Label>
								<Textarea
									id='bio'
									placeholder='Tell us about yourself'
									rows={4}
								/>
							</div>

							<div className='flex justify-end'>
								<Button onClick={handleSaveProfile}>
									<Save className='mr-2 h-4 w-4' />
									Save Changes
								</Button>
							</div>
						</CardContent>
					</Card>

					<Card>
						<CardHeader>
							<CardTitle>Preferences</CardTitle>
							<CardDescription>
								Configure your language and timezone preferences
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='space-y-2'>
								<Label htmlFor='language'>Language</Label>
								<Select value={language} onValueChange={setLanguage}>
									<SelectTrigger>
										<Globe className='mr-2 h-4 w-4' />
										<SelectValue />
									</SelectTrigger>
									<SelectContent>
										<SelectItem value='en'>English</SelectItem>
										<SelectItem value='vi'>Tiếng Việt</SelectItem>
										<SelectItem value='es'>Español</SelectItem>
										<SelectItem value='fr'>Français</SelectItem>
									</SelectContent>
								</Select>
							</div>

							<div className='space-y-2'>
								<Label htmlFor='timezone'>Timezone</Label>
								<Select value={timezone} onValueChange={setTimezone}>
									<SelectTrigger>
										<SelectValue />
									</SelectTrigger>
									<SelectContent>
										<SelectItem value='UTC'>UTC (GMT+0)</SelectItem>
										<SelectItem value='America/New_York'>
											Eastern Time (GMT-5)
										</SelectItem>
										<SelectItem value='America/Los_Angeles'>
											Pacific Time (GMT-8)
										</SelectItem>
										<SelectItem value='Asia/Ho_Chi_Minh'>
											Ho Chi Minh (GMT+7)
										</SelectItem>
										<SelectItem value='Europe/London'>
											London (GMT+0)
										</SelectItem>
									</SelectContent>
								</Select>
							</div>

							<div className='flex justify-end'>
								<Button onClick={handleSaveProfile}>
									<Save className='mr-2 h-4 w-4' />
									Save Preferences
								</Button>
							</div>
						</CardContent>
					</Card>
				</TabsContent>

				{/* Security Tab */}
				<TabsContent value='security' className='space-y-4'>
					<Card>
						<CardHeader>
							<CardTitle>Change Password</CardTitle>
							<CardDescription>
								Ensure your account is using a strong password
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='space-y-2'>
								<Label htmlFor='currentPassword'>Current Password</Label>
								<div className='relative'>
									<Input
										id='currentPassword'
										type={showPassword ? 'text' : 'password'}
										placeholder='Enter current password'
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
								<Label htmlFor='newPassword'>New Password</Label>
								<div className='relative'>
									<Input
										id='newPassword'
										type={showNewPassword ? 'text' : 'password'}
										placeholder='Enter new password'
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
								<Label htmlFor='confirmPassword'>Confirm New Password</Label>
								<div className='relative'>
									<Input
										id='confirmPassword'
										type={showConfirmPassword ? 'text' : 'password'}
										placeholder='Confirm new password'
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
									Change Password
								</Button>
							</div>
						</CardContent>
					</Card>

					<Card>
						<CardHeader>
							<CardTitle>Two-Factor Authentication</CardTitle>
							<CardDescription>
								Add an extra layer of security to your account
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='flex items-center justify-between'>
								<div className='space-y-1'>
									<div className='flex items-center gap-2'>
										<Smartphone className='h-4 w-4' />
										<p className='font-medium'>Authenticator App</p>
									</div>
									<p className='text-sm text-muted-foreground'>
										Use an authenticator app to generate verification codes
									</p>
								</div>
								<Switch
									checked={twoFactorEnabled}
									onCheckedChange={setTwoFactorEnabled}
								/>
							</div>

							{twoFactorEnabled && (
								<div className='rounded-lg border p-4 space-y-3'>
									<p className='text-sm font-medium'>Setup Authenticator App</p>
									<p className='text-sm text-muted-foreground'>
										Scan the QR code below with your authenticator app
									</p>
									<div className='flex justify-center py-4'>
										<div className='h-48 w-48 bg-muted rounded-lg flex items-center justify-center'>
											<p className='text-sm text-muted-foreground'>
												QR Code Placeholder
											</p>
										</div>
									</div>
									<div className='space-y-2'>
										<Label htmlFor='verificationCode'>Verification Code</Label>
										<Input
											id='verificationCode'
											placeholder='Enter 6-digit code'
											maxLength={6}
										/>
									</div>
									<Button className='w-full' onClick={handleEnable2FA}>
										<Key className='mr-2 h-4 w-4' />
										Enable Two-Factor Authentication
									</Button>
								</div>
							)}
						</CardContent>
					</Card>

					<Card>
						<CardHeader>
							<CardTitle>Active Sessions</CardTitle>
							<CardDescription>
								Manage your active sessions across devices
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='rounded-lg border p-4'>
								<div className='flex items-center justify-between'>
									<div className='space-y-1'>
										<p className='font-medium'>Current Session</p>
										<p className='text-sm text-muted-foreground'>
											Windows • Chrome • Ho Chi Minh, Vietnam
										</p>
										<p className='text-xs text-muted-foreground'>Active now</p>
									</div>
									<Badge variant='outline' className='text-green-600'>
										Active
									</Badge>
								</div>
							</div>

							<div className='flex justify-end'>
								<Button variant='outline'>
									<RefreshCw className='mr-2 h-4 w-4' />
									Revoke All Other Sessions
								</Button>
							</div>
						</CardContent>
					</Card>
				</TabsContent>

				{/* Notifications Tab */}
				<TabsContent value='notifications' className='space-y-4'>
					<Card>
						<CardHeader>
							<CardTitle>Email Notifications</CardTitle>
							<CardDescription>
								Choose what emails you want to receive
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='flex items-center justify-between'>
								<div className='space-y-1'>
									<div className='flex items-center gap-2'>
										<Mail className='h-4 w-4' />
										<p className='font-medium'>Email Notifications</p>
									</div>
									<p className='text-sm text-muted-foreground'>
										Receive email updates about your activity
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
									<p className='font-medium'>Exam Reminders</p>
									<p className='text-sm text-muted-foreground'>
										Get notified about upcoming exams
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
									<p className='font-medium'>Class Updates</p>
									<p className='text-sm text-muted-foreground'>
										Updates about classes you're enrolled in
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
									<p className='font-medium'>Marketing Emails</p>
									<p className='text-sm text-muted-foreground'>
										News, updates, and special offers
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
							<CardTitle>Push Notifications</CardTitle>
							<CardDescription>
								Manage your push notification preferences
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='flex items-center justify-between'>
								<div className='space-y-1'>
									<div className='flex items-center gap-2'>
										<Bell className='h-4 w-4' />
										<p className='font-medium'>Push Notifications</p>
									</div>
									<p className='text-sm text-muted-foreground'>
										Receive push notifications on your devices
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
							Save Preferences
						</Button>
					</div>
				</TabsContent>

				{/* Appearance Tab */}
				<TabsContent value='appearance' className='space-y-4'>
					<Card>
						<CardHeader>
							<CardTitle>Theme</CardTitle>
							<CardDescription>Choose how FrogEdu looks to you</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='space-y-2'>
								<Label>Theme Mode</Label>
								<Select value={theme} onValueChange={setTheme}>
									<SelectTrigger>
										<Palette className='mr-2 h-4 w-4' />
										<SelectValue />
									</SelectTrigger>
									<SelectContent>
										<SelectItem value='light'>Light</SelectItem>
										<SelectItem value='dark'>Dark</SelectItem>
										<SelectItem value='system'>System</SelectItem>
									</SelectContent>
								</Select>
								<p className='text-sm text-muted-foreground'>
									Select the theme for the dashboard
								</p>
							</div>
						</CardContent>
					</Card>

					<Card>
						<CardHeader>
							<CardTitle>Display Preferences</CardTitle>
							<CardDescription>
								Customize your viewing experience
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='flex items-center justify-between'>
								<div className='space-y-1'>
									<p className='font-medium'>Compact Mode</p>
									<p className='text-sm text-muted-foreground'>
										Display more content in less space
									</p>
								</div>
								<Switch />
							</div>

							<div className='flex items-center justify-between'>
								<div className='space-y-1'>
									<p className='font-medium'>Show Animations</p>
									<p className='text-sm text-muted-foreground'>
										Enable smooth transitions and animations
									</p>
								</div>
								<Switch defaultChecked />
							</div>
						</CardContent>
					</Card>

					<div className='flex justify-end'>
						<Button onClick={handleSaveAppearance}>
							<Save className='mr-2 h-4 w-4' />
							Save Preferences
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
