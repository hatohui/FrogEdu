import React from 'react'
import { useNavigate } from 'react-router'
import { useAuthStore } from '@/stores/authStore'
import { Loader2 } from 'lucide-react'

const AuthCallbackPage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const refreshAuth = useAuthStore(state => state.refreshAuth)
	const isLoading = useAuthStore(state => state.isLoading)
	const error = useAuthStore(state => state.error)

	React.useEffect(() => {
		const handleOAuthCallback = async () => {
			try {
				// Refresh auth state to get the new user session
				await refreshAuth()
				// Redirect to dashboard
				navigate('/dashboard', { replace: true })
			} catch (error) {
				console.error('OAuth callback error:', error)
				// Redirect to login with error
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
						Authentication Error
					</h2>
					<p className='text-muted-foreground'>{error}</p>
					<button
						onClick={() => navigate('/login')}
						className='mt-4 px-4 py-2 bg-primary text-primary-foreground rounded-md'
					>
						Back to Login
					</button>
				</div>
			</div>
		)
	}

	return (
		<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900'>
			<div className='text-center'>
				<div className='flex justify-center mb-4'>
					<img src='/frog.png' alt='FrogEdu logo' className='w-16 h-16' />
				</div>
				<div className='flex items-center justify-center space-x-2'>
					<Loader2 className='h-6 w-6 animate-spin text-green-700' />
					<p className='text-lg text-muted-foreground'>
						{isLoading ? 'Completing sign in...' : 'Redirecting...'}
					</p>
				</div>
			</div>
		</div>
	)
}

export default AuthCallbackPage
