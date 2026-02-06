import React from 'react'
import { useNavigate } from 'react-router'
import { useAuthStore } from '@/stores/authStore'
import { Loader2 } from 'lucide-react'
import userService from '@/services/user-microservice/user.service'
import { useTranslation } from 'react-i18next'

const AuthCallbackPage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const { t } = useTranslation()
	const refreshAuth = useAuthStore(state => state.refreshAuth)
	const isLoading = useAuthStore(state => state.isLoading)
	const error = useAuthStore(state => state.error)

	React.useEffect(() => {
		const handleOAuthCallback = async () => {
			try {
				await refreshAuth()

				try {
					const backendUser = await userService.getCurrentUser()

					if (!backendUser.roleId) {
						navigate('/select-role', { replace: true })
						return
					}

					navigate('/app', { replace: true })
				} catch (error) {
					console.log(error)
					navigate('/select-role', { replace: true })
				}
			} catch (error) {
				console.error('OAuth callback error:', error)
				navigate('/login')
			}
		}

		handleOAuthCallback()
	}, [refreshAuth, navigate])

	if (error) {
		return (
			<div className='flex min-h-screen items-center justify-center'>
				<div className='text-center'>
					<h2 className='text-xl font-semibold text-destructive mb-2'>
						{t('pages.auth.callback.error_title')}
					</h2>
					<p className='text-muted-foreground'>{error}</p>
					<button
						onClick={() => navigate('/login')}
						className='mt-4 px-4 py-2 bg-primary text-primary-foreground rounded-md'
					>
						{t('actions.back_to_login')}
					</button>
				</div>
			</div>
		)
	}

	return (
		<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900'>
			<div className='text-center'>
				<div className='flex justify-center mb-4'>
					<img
						src='/frog.png'
						alt={t('common.logo_alt')}
						className='w-16 h-16'
					/>
				</div>
				<div className='flex items-center justify-center space-x-2'>
					<Loader2 className='h-6 w-6 animate-spin text-green-700' />
					<p className='text-lg text-muted-foreground'>
						{isLoading
							? t('pages.auth.callback.completing_sign_in')
							: t('pages.auth.callback.redirecting')}
					</p>
				</div>
			</div>
		</div>
	)
}

export default AuthCallbackPage
