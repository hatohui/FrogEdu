import React from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { User } from 'lucide-react'
import { useAuthStore } from '@/stores/authStore'
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'

const ProfilePage = (): React.ReactElement => {
	const { user } = useAuthStore()

	const getUserInitials = () => {
		if (!user) return 'U'
		const name = user.username || ''
		const parts = name.split(' ')
		if (parts.length >= 2) {
			return `${parts[0][0]}${parts[1][0]}`.toUpperCase()
		}
		return name.substring(0, 2).toUpperCase()
	}

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
							<AvatarImage
								src={user?.username}
								alt={user?.username || 'User'}
							/>
							<AvatarFallback className='bg-primary text-primary-foreground text-2xl'>
								{getUserInitials()}
							</AvatarFallback>
						</Avatar>
						<div>
							<h3 className='text-xl font-semibold'>
								{user?.username || 'User'}
							</h3>
							<p className='text-sm text-muted-foreground'>Teacher Account</p>
						</div>
					</div>

					<div className='pt-4'>
						<p className='text-muted-foreground'>
							Profile editing feature is coming soon. You'll be able to update
							your name, bio, avatar, and other preferences here.
						</p>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default ProfilePage
