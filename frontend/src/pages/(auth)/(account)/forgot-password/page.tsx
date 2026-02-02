import React from 'react'
import { useForm, type SubmitHandler } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import * as z from 'zod'
import { useNavigate } from 'react-router'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Loader2, ArrowLeft, Mail, CheckCircle } from 'lucide-react'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import {
	Form,
	FormControl,
	FormField,
	FormItem,
	FormLabel,
	FormMessage,
} from '@/components/ui/form'
import { toast } from 'sonner'
import authService from '@/services/auth.service'

const forgotPasswordSchema = z.object({
	email: z.string().email({ message: 'Please enter a valid email address' }),
})

type ForgotPasswordFormValues = z.infer<typeof forgotPasswordSchema>

const ForgotPasswordPage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const [isLoading, setIsLoading] = React.useState(false)
	const [isSuccess, setIsSuccess] = React.useState(false)
	const [submittedEmail, setSubmittedEmail] = React.useState('')

	const form = useForm<ForgotPasswordFormValues>({
		resolver: zodResolver(forgotPasswordSchema),
		defaultValues: {
			email: '',
		},
	})

	const onSubmit: SubmitHandler<ForgotPasswordFormValues> = async data => {
		setIsLoading(true)

		try {
			const response = await authService.forgotPassword(data.email)
			if (response.success) {
				setSubmittedEmail(data.email)
				setIsSuccess(true)
				toast.success('Password reset email sent successfully')
			} else {
				const message =
					response.error?.detail || 'Failed to send password reset email'
				toast.error(message)
			}
		} catch (err: unknown) {
			const message =
				err instanceof Error
					? err.message
					: 'Failed to send password reset email. Please try again.'
			toast.error(message)
		} finally {
			setIsLoading(false)
		}
	}

	if (isSuccess) {
		return (
			<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900 p-4'>
				<Card className='w-full max-w-md shadow-xl'>
					<CardHeader className='space-y-1 text-center'>
						<div className='flex justify-center mb-4'>
							<div className='rounded-full bg-green-100 dark:bg-green-900 p-4'>
								<CheckCircle className='w-16 h-16 text-green-600 dark:text-green-400' />
							</div>
						</div>
						<CardTitle className='text-2xl font-bold'>
							Check Your Email
						</CardTitle>
						<CardDescription className='text-base'>
							We've sent a password reset link to{' '}
							<span className='font-semibold'>{submittedEmail}</span>
						</CardDescription>
					</CardHeader>
					<CardContent className='space-y-4'>
						<p className='text-sm text-gray-600 dark:text-gray-400 text-center'>
							Please check your email and click on the link to reset your
							password. The link will expire in 1 hour.
						</p>
						<div className='space-y-2'>
							<Button
								className='w-full'
								onClick={() => navigate('/login')}
								size='lg'
							>
								Back to Login
							</Button>
							<Button
								variant='outline'
								className='w-full'
								onClick={() => {
									setIsSuccess(false)
									form.reset()
								}}
								size='lg'
							>
								Resend Email
							</Button>
						</div>
					</CardContent>
				</Card>
			</div>
		)
	}

	return (
		<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900 p-4'>
			<Button
				variant='ghost'
				size='icon'
				className='absolute top-4 left-4'
				onClick={() => navigate('/login')}
			>
				<ArrowLeft className='h-6 w-6' />
			</Button>
			<Card className='w-full max-w-md shadow-xl'>
				<CardHeader className='space-y-1 text-center'>
					<div className='flex justify-center mb-4'>
						<div className='rounded-full bg-green-100 dark:bg-green-900 p-4'>
							<Mail className='w-12 h-12 text-green-600 dark:text-green-400' />
						</div>
					</div>
					<CardTitle className='text-3xl font-bold'>Forgot Password?</CardTitle>
					<CardDescription className='text-base'>
						No worries! Enter your email and we'll send you reset instructions.
					</CardDescription>
				</CardHeader>
				<CardContent>
					<Form {...form}>
						<form onSubmit={form.handleSubmit(onSubmit)} className='space-y-4'>
							<FormField
								control={form.control}
								name='email'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Email</FormLabel>
										<FormControl>
											<Input
												type='email'
												placeholder='Enter your email address'
												{...field}
											/>
										</FormControl>
										<FormMessage />
									</FormItem>
								)}
							/>

							<Button type='submit' className='w-full' disabled={isLoading}>
								{isLoading ? (
									<>
										<Loader2 className='mr-2 h-4 w-4 animate-spin' />
										Sending...
									</>
								) : (
									'Send Reset Link'
								)}
							</Button>
						</form>
					</Form>

					<div className='mt-6 text-center'>
						<Button
							variant='link'
							className='text-sm'
							onClick={() => navigate('/login')}
						>
							Back to Login
						</Button>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default ForgotPasswordPage
