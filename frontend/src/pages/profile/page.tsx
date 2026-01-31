import React from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { User, Shield, Clock, Mail } from 'lucide-react'
import { Skeleton } from '@/components/ui/skeleton'
import ProfileForm from '@/components/common/ProfileForm'
import { Badge } from '@/components/ui/badge'
import { useMe } from '@/hooks/auth/useMe'

const ProfilePage = (): React.ReactElement => {
	const { user, isLoading, error } = useMe()

	const formatDate = (dateString?: string) => {
		if (!dateString) return 'Never'
		return new Date(dateString).toLocaleDateString('vi-VN')
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
						<span>My Profile</span>
					</h1>
					<p className='text-muted-foreground'>
						Manage your account settings and preferences.
					</p>
				</div>

				<Card>
					<CardHeader>
						<CardTitle>Profile Information</CardTitle>
					</CardHeader>
					<CardContent className='space-y-4'>
						<div className='flex items-center space-x-4'>
							<div>
								<h3 className='text-xl font-semibold'>User</h3>
								<p className='text-sm text-muted-foreground'>
									Unable to load profile from server
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
					<span>My Profile</span>
				</h1>
			</div>

			{/* Profile Form Card */}
			<ProfileForm user={user} />

			{/* Account Info Card */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						<Shield className='h-5 w-5' />
						Account Information
					</CardTitle>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='grid gap-4 sm:grid-cols-2'>
						<div className='flex items-center gap-3'>
							<Mail className='h-5 w-5 text-muted-foreground' />
							<div>
								<p className='text-sm text-muted-foreground'>Email Status</p>
								<div className='flex items-center gap-2'>
									<span>{user.email}</span>
									{user.isEmailVerified ? (
										<Badge variant='default' className='bg-green-600'>
											Verified
										</Badge>
									) : (
										<Badge variant='destructive'>Not Verified</Badge>
									)}
								</div>
							</div>
						</div>
						<div className='flex items-center gap-3'>
							<Clock className='h-5 w-5 text-muted-foreground' />
							<div>
								<p className='text-sm text-muted-foreground'>Last Login</p>
								<p>
									{user.lastLoginAt ? formatDate(user.lastLoginAt) : 'Never'}
								</p>
							</div>
						</div>
					</div>
					<div className='grid gap-4 sm:grid-cols-2'>
						<div>
							<p className='text-sm text-muted-foreground'>Account Created</p>
							<p>{formatDate(user.createdAt)}</p>
						</div>
						<div>
							<p className='text-sm text-muted-foreground'>Last Updated</p>
							<p>{formatDate(user.updatedAt)}</p>
						</div>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default ProfilePage
