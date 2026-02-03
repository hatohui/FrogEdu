import React from 'react'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { Label } from '@/components/ui/label'
import { Switch } from '@/components/ui/switch'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { Bell, Globe, Palette } from 'lucide-react'
import { useTheme } from '@/config/theme'

const SettingsPage = (): React.ReactElement => {
	const [theme, toggleTheme] = useTheme()

	return (
		<div className='space-y-6 max-w-4xl mx-auto'>
			{/* Appearance */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						<Palette className='h-5 w-5' />
						Appearance
					</CardTitle>
					<CardDescription>Customize how the app looks</CardDescription>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='flex items-center justify-between'>
						<Label htmlFor='theme-select'>Theme</Label>
						<Select value={theme} onValueChange={() => toggleTheme()}>
							<SelectTrigger id='theme-select' className='w-32'>
								<SelectValue />
							</SelectTrigger>
							<SelectContent>
								<SelectItem value='light'>Light</SelectItem>
								<SelectItem value='dark'>Dark</SelectItem>
							</SelectContent>
						</Select>
					</div>
				</CardContent>
			</Card>

			{/* Notifications */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						<Bell className='h-5 w-5' />
						Notifications
					</CardTitle>
					<CardDescription>Configure notification preferences</CardDescription>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='flex items-center justify-between'>
						<Label htmlFor='email-notifications'>Email notifications</Label>
						<Switch id='email-notifications' defaultChecked />
					</div>
					<div className='flex items-center justify-between'>
						<Label htmlFor='push-notifications'>Push notifications</Label>
						<Switch id='push-notifications' />
					</div>
				</CardContent>
			</Card>

			{/* Language */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						<Globe className='h-5 w-5' />
						Language
					</CardTitle>
					<CardDescription>Set your preferred language</CardDescription>
				</CardHeader>
				<CardContent>
					<div className='flex items-center justify-between'>
						<Label htmlFor='language-select'>Language</Label>
						<Select defaultValue='vi'>
							<SelectTrigger id='language-select' className='w-40'>
								<SelectValue />
							</SelectTrigger>
							<SelectContent>
								<SelectItem value='vi'>Tiếng Việt</SelectItem>
								<SelectItem value='en'>English</SelectItem>
							</SelectContent>
						</Select>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default SettingsPage
