import React from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { User, Shield, Clock, Mail } from 'lucide-react'
import { Skeleton } from '@/components/ui/skeleton'
import ProfileForm from '@/components/common/ProfileForm'
import { Badge } from '@/components/ui/badge'
import { useMe } from '@/hooks/auth/useMe'
import { useTranslation } from 'react-i18next'

const ProfilePage = (): React.ReactElement => {
	const { user, isLoading, error } = useMe()
	const { t, i18n } = useTranslation()
	const locale = i18n.language === 'vi' ? 'vi-VN' : 'en-US'

	const formatDate = (dateString?: string) => {
		if (!dateString) return t('common.never')
		return new Date(dateString).toLocaleDateString(locale)
	}

	if (isLoading) {
		return (
			<div className='p-6 space-y-6 max-w-4xl mx-auto'>
				<Skeleton className='h-10 w-64' />
				<Skeleton className='h-[400px]' />
			</div>
		)
	}

	if (!user || error) {
		return (
			<div className='p-6 space-y-6 max-w-4xl mx-auto'>
				<div className='space-y-2'>
					<h1 className='text-3xl font-bold tracking-tight flex items-center space-x-2'>
						<User className='h-8 w-8' />
						<span>{t('pages.profile.title')}</span>
					</h1>
					<p className='text-muted-foreground'>{t('pages.profile.subtitle')}</p>
				</div>

				<Card>
					<CardHeader>
						<CardTitle>{t('pages.profile.profile_information')}</CardTitle>
					</CardHeader>
					<CardContent className='space-y-4'>
						<div className='flex items-center space-x-4'>
							<div>
								<h3 className='text-xl font-semibold'>{t('roles.user')}</h3>
								<p className='text-sm text-muted-foreground'>
									{t('pages.profile.unable_to_load')}
								</p>
							</div>
						</div>
					</CardContent>
				</Card>
			</div>
		)
	}

	return (
		<div className='p-6 space-y-6 max-w-4xl mx-auto'>
			{/* Page Header */}
			<div className='space-y-2'>
				<h1 className='text-3xl font-bold tracking-tight flex items-center space-x-2'>
					<User className='h-8 w-8' />
					<span>{t('pages.profile.title')}</span>
				</h1>
			</div>

			{/* Profile Form Card */}
			<ProfileForm user={user} />

			{/* Account Info Card */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						<Shield className='h-5 w-5' />
						{t('pages.profile.account_information')}
					</CardTitle>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='grid gap-4 sm:grid-cols-2'>
						<div className='flex items-center gap-3'>
							<Mail className='h-5 w-5 text-muted-foreground' />
							<div>
								<p className='text-sm text-muted-foreground'>
									{t('pages.profile.email_status')}
								</p>
								<div className='flex items-center gap-2'>
									<span>{user.email}</span>
									{user.isEmailVerified ? (
										<Badge variant='default' className='bg-green-600'>
											{t('badges.verified')}
										</Badge>
									) : (
										<Badge variant='destructive'>
											{t('badges.not_verified')}
										</Badge>
									)}
								</div>
							</div>
						</div>
						<div className='flex items-center gap-3'>
							<Clock className='h-5 w-5 text-muted-foreground' />
							<div>
								<p className='text-sm text-muted-foreground'>
									{t('pages.profile.member_since')}
								</p>
								<p>{formatDate(user.createdAt)}</p>
							</div>
						</div>
					</div>
					<div className='grid gap-4 sm:grid-cols-2'>
						<div>
							<p className='text-sm text-muted-foreground'>
								{t('pages.profile.account_created')}
							</p>
							<p>{formatDate(user.createdAt)}</p>
						</div>
						<div>
							<p className='text-sm text-muted-foreground'>
								{t('pages.profile.last_updated')}
							</p>
							<p>
								{user.updatedAt
									? formatDate(user.updatedAt)
									: t('common.never')}
							</p>
						</div>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default ProfilePage
