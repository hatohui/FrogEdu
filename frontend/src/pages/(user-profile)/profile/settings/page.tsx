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
import { useTranslation } from 'react-i18next'
import { useLanguage } from '@/config/i18n'

const SettingsPage = (): React.ReactElement => {
	const [theme, toggleTheme] = useTheme()
	const { t } = useTranslation()
	const { lang, setLanguage } = useLanguage()

	const handleLanguageChange = (value: string) => {
		void setLanguage(value)
	}

	return (
		<div className='space-y-6 max-w-4xl mx-auto'>
			{/* Appearance */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						<Palette className='h-5 w-5' />
						{t('pages.settings.appearance')}
					</CardTitle>
					<CardDescription>
						{t('pages.settings.appearance_description')}
					</CardDescription>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='flex items-center justify-between'>
						<Label htmlFor='theme-select'>{t('labels.theme')}</Label>
						<Select value={theme} onValueChange={() => toggleTheme()}>
							<SelectTrigger id='theme-select' className='w-32'>
								<SelectValue />
							</SelectTrigger>
							<SelectContent>
								<SelectItem value='light'>{t('options.light')}</SelectItem>
								<SelectItem value='dark'>{t('options.dark')}</SelectItem>
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
						{t('pages.settings.notifications')}
					</CardTitle>
					<CardDescription>
						{t('pages.settings.notifications_description')}
					</CardDescription>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='flex items-center justify-between'>
						<Label htmlFor='email-notifications'>
							{t('labels.email_notifications')}
						</Label>
						<Switch id='email-notifications' defaultChecked />
					</div>
					<div className='flex items-center justify-between'>
						<Label htmlFor='push-notifications'>
							{t('labels.push_notifications')}
						</Label>
						<Switch id='push-notifications' />
					</div>
				</CardContent>
			</Card>

			{/* Language */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						<Globe className='h-5 w-5' />
						{t('pages.settings.language')}
					</CardTitle>
					<CardDescription>
						{t('pages.settings.language_description')}
					</CardDescription>
				</CardHeader>
				<CardContent>
					<div className='flex items-center justify-between'>
						<Label htmlFor='language-select'>{t('labels.language')}</Label>
						<Select value={lang} onValueChange={handleLanguageChange}>
							<SelectTrigger id='language-select' className='w-40'>
								<SelectValue />
							</SelectTrigger>
							<SelectContent>
								<SelectItem value='vi'>{t('languages.vietnamese')}</SelectItem>
								<SelectItem value='en'>{t('languages.english')}</SelectItem>
							</SelectContent>
						</Select>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default SettingsPage
