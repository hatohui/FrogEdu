import React from 'react'
import { useNavigate } from 'react-router'
import { useAuthStore } from '@/stores/authStore'
import userService from '@/services/user.service'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { GraduationCap, Users } from 'lucide-react'

const SelectRolePage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const user = useAuthStore(state => state.user)
	const userProfile = useAuthStore(state => state.userProfile)
	const [isSubmitting, setIsSubmitting] = React.useState(false)
	const [error, setError] = React.useState<string | null>(null)

	React.useEffect(() => {
		if (!user) {
			navigate('/login')
		} else if (userProfile?.['custom:role']) {
			navigate('/dashboard')
		}
	}, [user, userProfile, navigate])

	const handleRoleSelect = async (role: 'Teacher' | 'Student') => {
		if (!user || !userProfile) return

		setIsSubmitting(true)
		setError(null)

		try {
			await userService.createUserFromCognito({
				sub: user.userId,
				email: userProfile.email || '',
				givenName:
					userProfile.given_name || userProfile.name?.split(' ')[0] || 'User',
				familyName:
					userProfile.family_name ||
					userProfile.name?.split(' ').slice(1).join(' ') ||
					'',
				customRole: role,
			})

			// Navigate to dashboard
			navigate('/dashboard')
		} catch (err) {
			console.error('Error creating user profile:', err)
			setError('Failed to set up your profile. Please try again.')
			setIsSubmitting(false)
		}
	}

	return (
		<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900 p-4'>
			<Card className='w-full max-w-2xl shadow-xl'>
				<CardHeader className='text-center space-y-2'>
					<div className='flex justify-center mb-4'>
						<img src='/frog.png' alt='FrogEdu logo' className='w-20 h-20' />
					</div>
					<CardTitle className='text-3xl font-bold'>
						Welcome to FrogEdu!
					</CardTitle>
					<CardDescription className='text-base'>
						Please select your role to continue
					</CardDescription>
				</CardHeader>
				<CardContent className='space-y-6'>
					{error && (
						<div className='rounded-lg border border-destructive/30 bg-destructive/10 px-4 py-3 text-sm text-destructive'>
							{error}
						</div>
					)}

					<div className='grid md:grid-cols-2 gap-4'>
						<button
							onClick={() => handleRoleSelect('Teacher')}
							disabled={isSubmitting}
							className='relative group'
							type='button'
						>
							<Card className='cursor-pointer transition-all duration-200 h-full hover:border-green-400'>
								<CardContent className='flex flex-col items-center justify-center p-8 space-y-4'>
									<div className='w-20 h-20 rounded-full bg-green-100 dark:bg-green-900 flex items-center justify-center group-hover:scale-110 transition-transform'>
										<GraduationCap className='w-10 h-10 text-green-700 dark:text-green-400' />
									</div>
									<div className='text-center'>
										<h3 className='text-xl font-semibold mb-2'>Teacher</h3>
										<p className='text-sm text-muted-foreground'>
											Create and manage classes, assign homework, and track
											student progress
										</p>
									</div>
								</CardContent>
							</Card>
						</button>

						<button
							onClick={() => handleRoleSelect('Student')}
							disabled={isSubmitting}
							className='relative group'
							type='button'
						>
							<Card className='cursor-pointer transition-all duration-200 h-full hover:border-blue-400'>
								<CardContent className='flex flex-col items-center justify-center p-8 space-y-4'>
									<div className='w-20 h-20 rounded-full bg-blue-100 dark:bg-blue-900 flex items-center justify-center group-hover:scale-110 transition-transform'>
										<Users className='w-10 h-10 text-blue-700 dark:text-blue-400' />
									</div>
									<div className='text-center'>
										<h3 className='text-xl font-semibold mb-2'>Student</h3>
										<p className='text-sm text-muted-foreground'>
											Join classes, complete assignments, and learn with AI
											assistance
										</p>
									</div>
								</CardContent>
							</Card>
						</button>
					</div>

					<div className='text-center text-sm text-muted-foreground'>
						You can change your role later in settings
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default SelectRolePage
