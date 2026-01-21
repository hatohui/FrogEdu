import React from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { User, Shield, Clock, Mail } from 'lucide-react'
import { useAuthStore } from '@/stores/authStore'
import { Avatar, AvatarFallback } from '@/components/ui/avatar'
import { useQuery } from '@tanstack/react-query'
import { Skeleton } from '@/components/ui/skeleton'
import ProfileForm from '@/components/common/ProfileForm'
import userService from '@/services/user.service'
import { Badge } from '@/components/ui/badge'

const ProfilePage = (): React.ReactElement => {
	const { user: authUser } = useAuthStore()

	// Fetch user profile from backend
	const {
		data: userProfile,
		isLoading,
		error,
	} = useQuery({
		queryKey: ['currentUser'],
		queryFn: () => userService.getCurrentUser(),
		staleTime: 5 * 60 * 1000, // 5 minutes
		retry: 1,
	})

	const formatDate = (dateString?: string) => {
		if (!dateString) return 'Never'
		return new Date(dateString).toLocaleDateString('en-US', {
			year: 'numeric',
			month: 'long',
			day: 'numeric',
			hour: '2-digit',
			minute: '2-digit',
		})
	}

	if (isLoading) {
		return (
			<div className='p-6 space-y-6 max-w-4xl mx-auto'>
				<Skeleton className='h-10 w-64' />
				<Skeleton className='h-[400px]' />
			</div>
		)
	}

	// Show basic profile from auth store if backend fetch fails
	if (error || !userProfile) {
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
							<Avatar className='h-20 w-20'>
								<AvatarFallback className='bg-primary text-primary-foreground text-2xl'>
									{authUser?.username?.charAt(0).toUpperCase() || 'U'}
								</AvatarFallback>
							</Avatar>
							<div>
								<h3 className='text-xl font-semibold'>
									{authUser?.username || 'User'}
								</h3>
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
				<p className='text-muted-foreground'>
					Manage your account settings and preferences.
				</p>
			</div>

			{/* Profile Form Card */}
			<ProfileForm user={userProfile} />

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
									<span>{userProfile.email}</span>
									{userProfile.isEmailVerified ? (
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
								<p>{formatDate(userProfile.lastLoginAt)}</p>
							</div>
						</div>
					</div>
					<div className='grid gap-4 sm:grid-cols-2'>
						<div>
							<p className='text-sm text-muted-foreground'>Account Created</p>
							<p>{formatDate(userProfile.createdAt)}</p>
						</div>
						<div>
							<p className='text-sm text-muted-foreground'>Last Updated</p>
							<p>{formatDate(userProfile.updatedAt)}</p>
						</div>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default ProfilePage
