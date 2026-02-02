import React from 'react'
import { useForm, type SubmitHandler } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import * as z from 'zod'
import { useNavigate, useSearchParams } from 'react-router'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Eye, EyeOff, Loader2, CheckCircle } from 'lucide-react'
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

const resetPasswordSchema = z
	.object({
		password: z
			.string()
			.min(8, { message: 'Password must be at least 8 characters' })
			.regex(/[A-Z]/, {
				message: 'Password must contain at least one uppercase letter',
			})
			.regex(/[a-z]/, {
				message: 'Password must contain at least one lowercase letter',
			})
			.regex(/[0-9]/, { message: 'Password must contain at least one number' })
			.regex(/[^A-Za-z0-9]/, {
				message: 'Password must contain at least one special character',
			}),
		confirmPassword: z.string(),
	})
	.refine(data => data.password === data.confirmPassword, {
		message: 'Passwords do not match',
		path: ['confirmPassword'],
	})

type ResetPasswordFormValues = z.infer<typeof resetPasswordSchema>

const ResetPasswordPage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const [searchParams] = useSearchParams()
	const token = searchParams.get('token')
	const [isLoading, setIsLoading] = React.useState(false)
	const [isSuccess, setIsSuccess] = React.useState(false)
	const [showPassword, setShowPassword] = React.useState(false)
	const [showConfirmPassword, setShowConfirmPassword] = React.useState(false)

	const form = useForm<ResetPasswordFormValues>({
		resolver: zodResolver(resetPasswordSchema),
		defaultValues: {
			password: '',
			confirmPassword: '',
		},
	})

	React.useEffect(() => {
		if (!token) {
			toast.error('Invalid or missing reset token')
			navigate('/forgot-password')
		}
	}, [token, navigate])

	const onSubmit: SubmitHandler<ResetPasswordFormValues> = async data => {
		if (!token) return

		setIsLoading(true)

		try {
			const response = await authService.resetPassword(token, data.password)
			if (response.success) {
				setIsSuccess(true)
				toast.success('Password reset successfully!')
			} else {
				const message =
					response.error?.detail ||
					'Failed to reset password. The link may have expired.'
				toast.error(message)
			}
		} catch (err: unknown) {
			const message =
				err instanceof Error
					? err.message
					: 'Failed to reset password. The link may have expired.'
			toast.error(message)
		} finally {
			setIsLoading(false)
		}
	}

	if (!token) {
		return <></>
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
							Password Reset Successful!
						</CardTitle>
						<CardDescription className='text-base'>
							Your password has been successfully reset. You can now sign in
							with your new password.
						</CardDescription>
					</CardHeader>
					<CardContent>
						<Button
							className='w-full'
							onClick={() => navigate('/login')}
							size='lg'
						>
							Go to Login
						</Button>
					</CardContent>
				</Card>
			</div>
		)
	}

	return (
		<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900 p-4'>
			<Card className='w-full max-w-md shadow-xl'>
				<CardHeader className='space-y-1 text-center'>
					<div className='flex justify-center mb-4'>
						<img src='/frog.png' alt='FrogEdu logo' className='w-20 h-20' />
					</div>
					<CardTitle className='text-3xl font-bold'>Reset Password</CardTitle>
					<CardDescription className='text-base'>
						Enter your new password below
					</CardDescription>
				</CardHeader>
				<CardContent>
					<Form {...form}>
						<form onSubmit={form.handleSubmit(onSubmit)} className='space-y-4'>
							<FormField
								control={form.control}
								name='password'
								render={({ field }) => (
									<FormItem>
										<FormLabel>New Password</FormLabel>
										<FormControl>
											<div className='relative'>
												<Input
													type={showPassword ? 'text' : 'password'}
													placeholder='Enter your new password'
													{...field}
												/>
												<Button
													type='button'
													variant='ghost'
													size='icon'
													className='absolute right-0 top-0 h-full px-3 py-2 hover:bg-transparent'
													onClick={() => setShowPassword(!showPassword)}
													tabIndex={-1}
												>
													{showPassword ? (
														<EyeOff className='h-4 w-4 text-gray-400' />
													) : (
														<Eye className='h-4 w-4 text-gray-400' />
													)}
												</Button>
											</div>
										</FormControl>
										<FormMessage />
									</FormItem>
								)}
							/>

							<FormField
								control={form.control}
								name='confirmPassword'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Confirm Password</FormLabel>
										<FormControl>
											<div className='relative'>
												<Input
													type={showConfirmPassword ? 'text' : 'password'}
													placeholder='Confirm your new password'
													{...field}
												/>
												<Button
													type='button'
													variant='ghost'
													size='icon'
													className='absolute right-0 top-0 h-full px-3 py-2 hover:bg-transparent'
													onClick={() =>
														setShowConfirmPassword(!showConfirmPassword)
													}
													tabIndex={-1}
												>
													{showConfirmPassword ? (
														<EyeOff className='h-4 w-4 text-gray-400' />
													) : (
														<Eye className='h-4 w-4 text-gray-400' />
													)}
												</Button>
											</div>
										</FormControl>
										<FormMessage />
									</FormItem>
								)}
							/>

							<Button type='submit' className='w-full' disabled={isLoading}>
								{isLoading ? (
									<>
										<Loader2 className='mr-2 h-4 w-4 animate-spin' />
										Resetting Password...
									</>
								) : (
									'Reset Password'
								)}
							</Button>
						</form>
					</Form>
				</CardContent>
			</Card>
		</div>
	)
}

export default ResetPasswordPage
