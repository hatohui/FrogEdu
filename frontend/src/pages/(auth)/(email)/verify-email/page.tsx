import React from 'react'
import { useNavigate, useSearchParams } from 'react-router'
import { Button } from '@/components/ui/button'
import { Loader2, CheckCircle, XCircle } from 'lucide-react'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { toast } from 'sonner'
import authService from '@/services/user-microservice/auth.service'
import { useAuthStore } from '@/stores/authStore'
import { useTranslation } from 'react-i18next'

const VerifyEmailPage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const { t } = useTranslation()
	const [searchParams] = useSearchParams()
	const token = searchParams.get('token')
	const [isVerifying, setIsVerifying] = React.useState(true)
	const [isSuccess, setIsSuccess] = React.useState(false)
	const [error, setError] = React.useState<string | null>(null)

	const authLoading = useAuthStore(state => state.isLoading)

	React.useEffect(() => {
		// Wait for auth store to finish hydrating before verifying
		if (authLoading) return

		if (!token) {
			setError(t('pages.auth.verify.no_token'))
			setIsVerifying(false)
			return
		}

		// Debug: Log the token received from URL
		console.log('Token from URL:', token)
		console.log('Token length:', token.length)

		const verifyEmail = async () => {
			try {
				const response = await authService.verifyEmail(token)
				if (response.success) {
					setIsSuccess(true)
					toast.success(t('messages.email_verified_success'))
				} else {
					const message =
						response.error?.detail || t('messages.email_verified_failed')
					setError(message)
					toast.error(message)
				}
			} catch (err: unknown) {
				const message =
					err instanceof Error
						? err.message
						: t('messages.email_verified_failed_generic')
				setError(message)
				toast.error(message)
			} finally {
				setIsVerifying(false)
			}
		}

		verifyEmail()
	}, [token, authLoading])

	if (isVerifying) {
		return (
			<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900'>
				<Card className='w-full max-w-md shadow-xl'>
					<CardHeader className='space-y-1 text-center'>
						<div className='flex justify-center mb-4'>
							<Loader2 className='h-16 w-16 animate-spin text-green-600' />
						</div>
						<CardTitle className='text-2xl font-bold'>
							{t('pages.auth.verify.verifying_title')}
						</CardTitle>
						<CardDescription>
							{t('pages.auth.verify.verifying_note')}
						</CardDescription>
					</CardHeader>
				</Card>
			</div>
		)
	}

	return (
		<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900 p-4'>
			<Card className='w-full max-w-md shadow-xl'>
				<CardHeader className='space-y-1 text-center'>
					<div className='flex justify-center mb-4'>
						{isSuccess ? (
							<div className='rounded-full bg-green-100 dark:bg-green-900 p-4'>
								<CheckCircle className='w-16 h-16 text-green-600 dark:text-green-400' />
							</div>
						) : (
							<div className='rounded-full bg-red-100 dark:bg-red-900 p-4'>
								<XCircle className='w-16 h-16 text-red-600 dark:text-red-400' />
							</div>
						)}
					</div>
					<CardTitle className='text-2xl font-bold'>
						{isSuccess
							? t('pages.auth.verify.success_title')
							: t('pages.auth.verify.failed_title')}
					</CardTitle>
					<CardDescription className='text-base'>
						{isSuccess
							? t('pages.auth.verify.success_description')
							: error || t('pages.auth.verify.failed_description')}
					</CardDescription>
				</CardHeader>
				<CardContent className='space-y-4'>
					{isSuccess ? (
						<Button
							className='w-full'
							onClick={() => navigate('/login')}
							size='lg'
						>
							{t('actions.go_to_login')}
						</Button>
					) : (
						<div className='space-y-2'>
							<Button
								className='w-full'
								onClick={() => navigate('/register')}
								size='lg'
							>
								{t('actions.back_to_register')}
							</Button>
							<Button
								variant='outline'
								className='w-full'
								onClick={() => navigate('/')}
								size='lg'
							>
								{t('actions.go_to_home')}
							</Button>
						</div>
					)}
				</CardContent>
			</Card>
		</div>
	)
}

export default VerifyEmailPage
