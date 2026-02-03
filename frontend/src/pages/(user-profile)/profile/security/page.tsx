import React from 'react'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Key, Smartphone, AlertTriangle } from 'lucide-react'

const SecurityPage = (): React.ReactElement => {
	return (
		<div className='space-y-6 max-w-4xl mx-auto'>
			{/* Password Section */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						<Key className='h-5 w-5' />
						Password
					</CardTitle>
					<CardDescription>Change your account password</CardDescription>
				</CardHeader>
				<CardContent>
					<div className='flex items-center justify-between'>
						<span className='text-sm text-muted-foreground'>
							Last changed: Never
						</span>
						<Button variant='outline'>Change Password</Button>
					</div>
				</CardContent>
			</Card>

			{/* Two-Factor Authentication */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						<Smartphone className='h-5 w-5' />
						Two-Factor Authentication
					</CardTitle>
					<CardDescription>
						Add an extra layer of security to your account
					</CardDescription>
				</CardHeader>
				<CardContent>
					<div className='flex items-center justify-between'>
						<span className='text-sm text-muted-foreground'>
							Authenticator app
						</span>
						<Button variant='outline' disabled>
							Coming Soon
						</Button>
					</div>
				</CardContent>
			</Card>

			{/* Danger Zone */}
			<Card className='border-destructive'>
				<CardHeader>
					<CardTitle className='flex items-center gap-2 text-destructive'>
						<AlertTriangle className='h-5 w-5' />
						Danger Zone
					</CardTitle>
					<CardDescription>Irreversible actions</CardDescription>
				</CardHeader>
				<CardContent>
					<div className='flex items-center justify-between'>
						<span className='text-sm text-muted-foreground'>
							Permanently delete your account
						</span>
						<Button variant='destructive' disabled>
							Delete Account
						</Button>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default SecurityPage
