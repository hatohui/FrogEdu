import React, { useMemo } from 'react'
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
import authService from '@/services/user-microservice/auth.service'
import { useTranslation } from 'react-i18next'

type ForgotPasswordFormValues = {
	email: string
}

const ForgotPasswordPage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const { t } = useTranslation()
	const [isLoading, setIsLoading] = React.useState(false)
	const [isSuccess, setIsSuccess] = React.useState(false)
	const [submittedEmail, setSubmittedEmail] = React.useState('')

	const forgotPasswordSchema = useMemo(
		() =>
			z.object({
				email: z
					.string()
					.email({ message: t('forms.auth.validation.email_invalid') }),
			}),
		[t]
	)

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
				toast.success(t('messages.reset_email_sent'))
			} else {
				const message =
					response.error?.detail || t('messages.reset_email_failed')
				toast.error(message)
			}
		} catch (err: unknown) {
			const message =
				err instanceof Error
					? err.message
					: t('messages.reset_email_failed_generic')
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
							{t('pages.auth.forgot.check_email_title')}
						</CardTitle>
						<CardDescription className='text-base'>
							{t('pages.auth.forgot.check_email_description')}{' '}
							<span className='font-semibold'>{submittedEmail}</span>
						</CardDescription>
					</CardHeader>
					<CardContent className='space-y-4'>
						<p className='text-sm text-gray-600 dark:text-gray-400 text-center'>
							{t('pages.auth.forgot.check_email_note')}
						</p>
						<div className='space-y-2'>
							<Button
								className='w-full'
								onClick={() => navigate('/login')}
								size='lg'
							>
								{t('actions.back_to_login')}
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
								{t('actions.resend_email')}
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
					<CardTitle className='text-3xl font-bold'>
						{t('pages.auth.forgot.title')}
					</CardTitle>
					<CardDescription className='text-base'>
						{t('pages.auth.forgot.subtitle')}
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
										<FormLabel>{t('labels.email')}</FormLabel>
										<FormControl>
											<Input
												type='email'
												placeholder={t('placeholders.email')}
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
										{t('actions.sending')}
									</>
								) : (
									t('actions.send_reset_link')
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
							{t('actions.back_to_login')}
						</Button>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default ForgotPasswordPage
