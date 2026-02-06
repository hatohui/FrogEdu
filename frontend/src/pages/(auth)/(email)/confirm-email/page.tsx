import React, { useMemo } from 'react'
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
import { useTranslation } from 'react-i18next'

type ConfirmEmailFormValues = {
	code: string
}

const ConfirmEmailPage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const { t } = useTranslation()
	const [searchParams] = useSearchParams()
	const email = searchParams.get('email')
	const [isLoading, setIsLoading] = React.useState(false)
	const [error, setError] = React.useState<string | null>(null)
	const [isResending, setIsResending] = React.useState(false)

	const confirmEmailSchema = useMemo(
		() =>
			z.object({
				code: z
					.string()
					.min(6, { message: t('forms.auth.validation.code_length') }),
			}),
		[t]
	)

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
			const result = await confirmSignUp({
				username: email,
				confirmationCode: data.code,
			})

			console.log(result)

			toast.success(t('messages.email_confirmed_success'))
			navigate('/login')
		} catch (err) {
			const message =
				err instanceof Error
					? err.message
					: t('messages.email_confirmed_failed')
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
			toast.success(t('messages.verification_code_resent'))
		} catch (err) {
			const message =
				err instanceof Error
					? err.message
					: t('messages.verification_code_resend_failed')
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
						{t('pages.auth.confirm.title')}
					</CardTitle>
					<CardDescription className='text-base'>
						{t('pages.auth.confirm.subtitle')}{' '}
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
										<FormLabel>{t('labels.verification_code')}</FormLabel>
										<FormControl>
											<Input
												type='text'
												placeholder={t('placeholders.verification_code')}
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
										{t('actions.confirming')}
									</>
								) : (
									t('actions.confirm_email')
								)}
							</Button>
						</form>
					</Form>
				</CardContent>
				<CardFooter className='flex flex-col space-y-2'>
					<div className='text-sm text-gray-600 dark:text-gray-400 text-center'>
						{t('pages.auth.confirm.no_code')}
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
								{t('actions.resending')}
							</>
						) : (
							t('actions.resend_code')
						)}
					</Button>
				</CardFooter>
			</Card>
		</div>
	)
}

export default ConfirmEmailPage
