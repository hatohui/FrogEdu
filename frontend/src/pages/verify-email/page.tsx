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
import authService from '@/services/auth.service'

const VerifyEmailPage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const [searchParams] = useSearchParams()
	const token = searchParams.get('token')
	const [isVerifying, setIsVerifying] = React.useState(true)
	const [isSuccess, setIsSuccess] = React.useState(false)
	const [error, setError] = React.useState<string | null>(null)

	React.useEffect(() => {
		if (!token) {
			setError('No verification token provided')
			setIsVerifying(false)
			return
		}

		const verifyEmail = async () => {
			try {
				const response = await authService.verifyEmail(token)
				if (response.success) {
					setIsSuccess(true)
					toast.success('Email verified successfully!')
				} else {
					const message = response.error?.detail || 'Failed to verify email'
					setError(message)
					toast.error(message)
				}
			} catch (err: unknown) {
				const message =
					err instanceof Error
						? err.message
						: 'Failed to verify email. The link may have expired.'
				setError(message)
				toast.error(message)
			} finally {
				setIsVerifying(false)
			}
		}

		verifyEmail()
	}, [token])

	if (isVerifying) {
		return (
			<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900'>
				<Card className='w-full max-w-md shadow-xl'>
					<CardHeader className='space-y-1 text-center'>
						<div className='flex justify-center mb-4'>
							<Loader2 className='h-16 w-16 animate-spin text-green-600' />
						</div>
						<CardTitle className='text-2xl font-bold'>
							Verifying Your Email
						</CardTitle>
						<CardDescription>Please wait...</CardDescription>
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
						{isSuccess ? 'Email Verified!' : 'Verification Failed'}
					</CardTitle>
					<CardDescription className='text-base'>
						{isSuccess
							? 'Your email has been successfully verified. You can now sign in to your account.'
							: error || 'Unable to verify your email. Please try again.'}
					</CardDescription>
				</CardHeader>
				<CardContent className='space-y-4'>
					{isSuccess ? (
						<Button
							className='w-full'
							onClick={() => navigate('/login')}
							size='lg'
						>
							Go to Login
						</Button>
					) : (
						<div className='space-y-2'>
							<Button
								className='w-full'
								onClick={() => navigate('/register')}
								size='lg'
							>
								Back to Register
							</Button>
							<Button
								variant='outline'
								className='w-full'
								onClick={() => navigate('/')}
								size='lg'
							>
								Go to Home
							</Button>
						</div>
					)}
				</CardContent>
			</Card>
		</div>
	)
}

export default VerifyEmailPage
