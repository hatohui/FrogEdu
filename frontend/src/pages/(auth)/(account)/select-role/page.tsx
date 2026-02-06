import React from 'react'
import { useNavigate } from 'react-router'
import { useMe } from '@/hooks/auth/useMe'
import userService from '@/services/user-microservice/user.service'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { GraduationCap, Users } from 'lucide-react'
import { getCurrentUser, fetchAuthSession } from 'aws-amplify/auth'
import { useTranslation } from 'react-i18next'

const SelectRolePage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const { t } = useTranslation()
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
			navigate('/app')
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

			window.location.href = '/app'
		} catch (err) {
			console.error('Error creating user profile:', err)
			setError(t('pages.auth.select_role.profile_setup_failed'))
			setIsSubmitting(false)
		}
	}

	return (
		<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900 p-4'>
			<Card className='w-full max-w-2xl shadow-xl'>
				<CardHeader className='text-center space-y-2'>
					<div className='flex justify-center mb-4'>
						<img
							src='/frog.png'
							alt={t('common.logo_alt')}
							className='w-20 h-20'
						/>
					</div>
					<CardTitle className='text-3xl font-bold'>
						{t('pages.auth.select_role.title')}
					</CardTitle>
					<CardDescription className='text-base'>
						{t('pages.auth.select_role.subtitle')}
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
										<h3 className='text-xl font-semibold mb-2'>
											{t('roles.teacher')}
										</h3>
										<p className='text-sm text-muted-foreground'>
											{t('pages.auth.select_role.teacher_description')}
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
										<h3 className='text-xl font-semibold mb-2'>
											{t('roles.student')}
										</h3>
										<p className='text-sm text-muted-foreground'>
											{t('pages.auth.select_role.student_description')}
										</p>
									</div>
								</CardContent>
							</Card>
						</button>
					</div>

					<div className='text-center text-sm text-muted-foreground'>
						{t('pages.auth.select_role.footer_note')}
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default SelectRolePage
