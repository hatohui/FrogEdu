import React from 'react'
import { useNavigate } from 'react-router'
import { useMe } from '@/hooks/auth/useMe'
import userService from '@/services/user.service'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { GraduationCap, Users } from 'lucide-react'
import { getCurrentUser, fetchAuthSession } from 'aws-amplify/auth'

const SelectRolePage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const { user, isAuthenticated } = useMe()
	const [isSubmitting, setIsSubmitting] = React.useState(false)
	const [error, setError] = React.useState<string | null>(null)
	const [cognitoUser, setCognitoUser] = React.useState<{
		sub: string
		email: string
		givenName?: string
		familyName?: string
		picture?: string
	} | null>(null)

	// Fetch Cognito user data if backend user doesn't exist
	React.useEffect(() => {
		const fetchCognitoUser = async () => {
			try {
				const currentUser = await getCurrentUser()
				const session = await fetchAuthSession()
				const idToken = session.tokens?.idToken

				if (idToken) {
					setCognitoUser({
						sub: currentUser.userId,
						email: (idToken.payload.email as string) || '',
						givenName: (idToken.payload.given_name as string) || undefined,
						familyName: (idToken.payload.family_name as string) || undefined,
						picture: (idToken.payload.picture as string) || undefined,
					})
				}
			} catch (err) {
				console.error('Failed to fetch Cognito user:', err)
			}
		}

		if (isAuthenticated && !user) {
			fetchCognitoUser()
		}
	}, [isAuthenticated, user])

	React.useEffect(() => {
		if (!isAuthenticated) {
			navigate('/login')
		} else if (user?.roleId) {
			navigate('/dashboard')
		}
	}, [user, isAuthenticated, navigate])

	const handleRoleSelect = async (role: 'Teacher' | 'Student') => {
		const userData = user || cognitoUser
		if (!userData) return

		setIsSubmitting(true)
		setError(null)

		try {
			await userService.createUserFromCognito({
				sub: 'cognitoId' in userData ? userData.cognitoId : userData.sub,
				email: userData.email || '',
				givenName:
					('firstName' in userData ? userData.firstName : userData.givenName) ||
					'User',
				familyName:
					('lastName' in userData ? userData.lastName : userData.familyName) ||
					'',
				customRole: role,
				picture:
					'avatarUrl' in userData
						? userData.avatarUrl || undefined
						: 'picture' in userData
							? userData.picture
							: undefined,
			})

			// Refresh auth to update user session with new role
			window.location.href = '/dashboard'
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
