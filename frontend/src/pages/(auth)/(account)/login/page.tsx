import React, { useMemo } from 'react'
import { useForm, type SubmitHandler } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import * as z from 'zod'
import { useLogin } from '@/hooks/auth/useLogin'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Eye, EyeOff, Loader2, ArrowLeft } from 'lucide-react'
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
import ProtectedRoute from '@/components/common/ProtectedRoute'
import { useNavigate } from 'react-router'
import { useTranslation } from 'react-i18next'

type LoginFormValues = {
	email: string
	password: string
}

const LoginPage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const { t } = useTranslation()
	const signInWithGoogle = useLogin().signInWithGoogle
	const { login, isLoading, error, clearError } = useLogin()
	const [showPassword, setShowPassword] = React.useState(false)

	const loginSchema = useMemo(
		() =>
			z.object({
				email: z
					.string()
					.email({ message: t('forms.auth.validation.email_invalid') }),
				password: z
					.string()
					.min(8, { message: t('forms.auth.validation.password_min') }),
			}),
		[t]
	)

	const form = useForm<LoginFormValues>({
		resolver: zodResolver(loginSchema),
		defaultValues: {
			email: '',
			password: '',
		},
	})

	React.useEffect(() => {
		return () => clearError()
	}, [clearError])

	const onSubmit: SubmitHandler<LoginFormValues> = async data => {
		await login(data.email, data.password)
	}

	return (
		<ProtectedRoute requireAuth={false}>
			<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900 p-4'>
				<Button
					variant='ghost'
					size='icon'
					className='absolute top-4 left-4'
					onClick={() => navigate('/')}
				>
					<ArrowLeft className='h-6 w-6' />
				</Button>
				<Card className='w-full max-w-md shadow-xl'>
					<CardHeader className='space-y-1 text-center'>
						<div className='flex justify-center mb-4'>
							<img
								src='/frog.png'
								alt={t('common.logo_alt')}
								className='w-20 h-20'
							/>
						</div>
						<CardTitle className='text-3xl font-bold'>
							{t('common.app_name')}
						</CardTitle>
						<CardDescription className='text-base'>
							{t('pages.auth.login.subtitle')}
						</CardDescription>
					</CardHeader>
					<CardContent>
						<Form {...form}>
							<form
								onSubmit={form.handleSubmit(onSubmit)}
								className='space-y-4'
							>
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

								<FormField
									control={form.control}
									name='password'
									render={({ field }) => (
										<FormItem>
											<FormLabel>{t('labels.password')}</FormLabel>
											<FormControl>
												<div className='relative'>
													<Input
														type={showPassword ? 'text' : 'password'}
														placeholder={t('placeholders.password')}
														{...field}
														className='pr-10'
													/>
													<Button
														type='button'
														variant='ghost'
														size='sm'
														className='absolute right-0 top-0 h-full px-3 py-2 hover:bg-transparent'
														onClick={() => setShowPassword(!showPassword)}
													>
														{showPassword ? (
															<EyeOff className='h-4 w-4 text-muted-foreground' />
														) : (
															<Eye className='h-4 w-4 text-muted-foreground' />
														)}
													</Button>
												</div>
											</FormControl>
											<FormMessage />
										</FormItem>
									)}
								/>

								{error && (
									<div className='rounded-lg border border-destructive/30 bg-destructive/10 px-4 py-3 text-sm text-destructive animate-in fade-in slide-in-from-top-2'>
										{error}
									</div>
								)}

								<Button
									type='submit'
									className='w-full h-10 bg-primary hover:bg-primary/90 dark:bg-green-700 dark:hover:bg-green-800 transition-all duration-200'
									disabled={isLoading}
								>
									{isLoading ? (
										<div className='flex items-center justify-center gap-2'>
											<Loader2 className='h-4 w-4 animate-spin' />
											<span>{t('actions.signing_in')}</span>
										</div>
									) : (
										<span className='font-medium'>{t('actions.sign_in')}</span>
									)}
								</Button>

								<div className='relative'>
									<div className='absolute inset-0 flex items-center'>
										<span className='w-full border-t' />
									</div>
									<div className='relative flex justify-center text-xs uppercase'>
										<span className='bg-background px-2 text-muted-foreground'>
											{t('pages.auth.login.or_continue_with')}
										</span>
									</div>
								</div>

								<Button
									type='button'
									variant='outline'
									className='w-full h-10 transition-all duration-200'
									disabled={isLoading}
									onClick={() => signInWithGoogle()}
								>
									{!isLoading && (
										<svg
											className='mr-2 h-4 w-4 flex-shrink-0'
											viewBox='0 0 24 24'
										>
											<path
												fill='#4285f4'
												d='M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z'
											/>
											<path
												fill='#34a853'
												d='M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z'
											/>
											<path
												fill='#fbbc05'
												d='M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z'
											/>
											<path
												fill='#ea4335'
												d='M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z'
											/>
										</svg>
									)}
									<span className='font-medium'>
										{isLoading
											? t('actions.signing_in')
											: t('actions.sign_in_google')}
									</span>
								</Button>

								<div className='text-center'>
									<Button
										type='button'
										variant='link'
										className='text-green-700 dark:text-green-400'
										onClick={() => navigate('/forgot-password')}
									>
										{t('pages.auth.login.forgot_password')}
									</Button>
								</div>
							</form>
						</Form>
					</CardContent>
					<CardFooter className='flex flex-col space-y-2'>
						<div className='text-sm text-center text-muted-foreground'>
							{t('pages.auth.login.no_account')}{' '}
							<Button
								variant='link'
								className='p-0 h-auto font-semibold text-green-700 dark:text-green-400'
								onClick={() => navigate('/register')}
							>
								{t('actions.create_account')}
							</Button>
						</div>
						<p className='text-xs text-center text-muted-foreground'>
							{t('pages.auth.login.footer', {
								year: new Date().getFullYear(),
							})}
						</p>
					</CardFooter>
				</Card>
			</div>
		</ProtectedRoute>
	)
}

export default LoginPage
