import React from 'react'
import { useForm, type SubmitHandler } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import * as z from 'zod'
import { useNavigate, useSearchParams } from 'react-router'
import { confirmSignUp, resendSignUpCode } from 'aws-amplify/auth'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Loader2, ArrowLeft, Mail } from 'lucide-react'
import {
	Card,
	CardContent,
	CardDescription,
	CardFooter,
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

const confirmEmailSchema = z.object({
	code: z.string().min(6, { message: 'Verification code must be 6 digits' }),
})

type ConfirmEmailFormValues = z.infer<typeof confirmEmailSchema>

const ConfirmEmailPage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const [searchParams] = useSearchParams()
	const email = searchParams.get('email')
	const [isLoading, setIsLoading] = React.useState(false)
	const [error, setError] = React.useState<string | null>(null)
	const [isResending, setIsResending] = React.useState(false)

	const form = useForm<ConfirmEmailFormValues>({
		resolver: zodResolver(confirmEmailSchema),
		defaultValues: {
			code: '',
		},
	})

	React.useEffect(() => {
		if (!email) {
			navigate('/register', { replace: true })
		}
	}, [email, navigate])

	const onSubmit: SubmitHandler<ConfirmEmailFormValues> = async data => {
		if (!email) return

		setIsLoading(true)
		setError(null)

		try {
			await confirmSignUp({
				username: email,
				confirmationCode: data.code,
			})

			toast.success('Email confirmed successfully!')
			navigate('/login')
		} catch (err) {
			const message =
				err instanceof Error
					? err.message
					: 'Failed to confirm email. Please try again.'
			setError(message)
			toast.error(message)
		} finally {
			setIsLoading(false)
		}
	}

	const handleResendCode = async () => {
		if (!email) return

		setIsResending(true)
		setError(null)

		try {
			await resendSignUpCode({ username: email })
			toast.success('Verification code resent to your email')
		} catch (err) {
			const message =
				err instanceof Error
					? err.message
					: 'Failed to resend code. Please try again.'
			setError(message)
			toast.error(message)
		} finally {
			setIsResending(false)
		}
	}

	if (!email) {
		return (
			<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900'>
				<Loader2 className='h-8 w-8 animate-spin text-green-600' />
			</div>
		)
	}

	return (
		<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900 p-4'>
			<Button
				variant='ghost'
				size='icon'
				className='absolute top-4 left-4'
				onClick={() => navigate('/register')}
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
					<CardTitle className='text-3xl font-bold'>
						Confirm Your Email
					</CardTitle>
					<CardDescription className='text-base'>
						We've sent a verification code to{' '}
						<span className='font-semibold'>{email}</span>
					</CardDescription>
				</CardHeader>
				<CardContent>
					<Form {...form}>
						<form onSubmit={form.handleSubmit(onSubmit)} className='space-y-4'>
							<FormField
								control={form.control}
								name='code'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Verification Code</FormLabel>
										<FormControl>
											<Input
												type='text'
												placeholder='Enter 6-digit code'
												maxLength={6}
												{...field}
											/>
										</FormControl>
										<FormMessage />
									</FormItem>
								)}
							/>

							{error && (
								<div className='text-sm text-red-500 text-center'>{error}</div>
							)}

							<Button type='submit' className='w-full' disabled={isLoading}>
								{isLoading ? (
									<>
										<Loader2 className='mr-2 h-4 w-4 animate-spin' />
										Confirming...
									</>
								) : (
									'Confirm Email'
								)}
							</Button>
						</form>
					</Form>
				</CardContent>
				<CardFooter className='flex flex-col space-y-2'>
					<div className='text-sm text-gray-600 dark:text-gray-400 text-center'>
						Didn't receive the code?
					</div>
					<Button
						variant='outline'
						className='w-full'
						onClick={handleResendCode}
						disabled={isResending}
					>
						{isResending ? (
							<>
								<Loader2 className='mr-2 h-4 w-4 animate-spin' />
								Resending...
							</>
						) : (
							'Resend Code'
						)}
					</Button>
				</CardFooter>
			</Card>
		</div>
	)
}

export default ConfirmEmailPage
