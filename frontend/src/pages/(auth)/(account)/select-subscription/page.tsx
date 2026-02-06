import React from 'react'
import { useNavigate } from 'react-router'
import { useSubscription } from '@/hooks/useSubscription'
import { useMe } from '@/hooks/auth/useMe'
import {
	Card,
	CardContent,
	CardDescription,
	CardFooter,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'
import { Skeleton } from '@/components/ui/skeleton'
import { Check, Sparkles, Crown, Zap, ArrowRight } from 'lucide-react'
import { useTranslation } from 'react-i18next'

const SelectSubscriptionPage = (): React.JSX.Element => {
	const navigate = useNavigate()
	const { t } = useTranslation()
	const { user, isAuthenticated, isLoading: isLoadingUser } = useMe()
	const { tiers, isLoadingTiers, subscribeToPro, isSubscribing, isPro } =
		useSubscription()

	// Redirect if not authenticated or no role
	React.useEffect(() => {
		if (!isLoadingUser && !isAuthenticated) {
			navigate('/login')
		}
	}, [isAuthenticated, isLoadingUser, navigate])

	// If already Pro, redirect to app
	React.useEffect(() => {
		if (isPro) {
			navigate('/app')
		}
	}, [isPro, navigate])

	const handleSelectFree = () => {
		navigate('/app')
	}

	const handleSelectPro = async () => {
		try {
			await subscribeToPro()
			navigate('/app')
		} catch {
			// Error is handled by the hook
		}
	}

	const handleSkip = () => {
		navigate('/app')
	}

	if (isLoadingUser || isLoadingTiers) {
		return (
			<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900 p-4'>
				<Card className='w-full max-w-4xl shadow-xl'>
					<CardHeader className='text-center'>
						<Skeleton className='h-20 w-20 rounded-full mx-auto mb-4' />
						<Skeleton className='h-8 w-64 mx-auto mb-2' />
						<Skeleton className='h-4 w-96 mx-auto' />
					</CardHeader>
					<CardContent>
						<div className='grid md:grid-cols-2 gap-6'>
							<Skeleton className='h-80' />
							<Skeleton className='h-80' />
						</div>
					</CardContent>
				</Card>
			</div>
		)
	}

	const freeTier = tiers?.find(t => t.name.toLowerCase() === 'free')
	const proTier = tiers?.find(t => t.name.toLowerCase() === 'pro')

	const freeFeatures = [
		'create_classes',
		'basic_exam_generation',
		'ai_exams_limit',
		'digital_textbooks',
		'community_support',
	]

	const proFeatures = [
		'unlimited_classes',
		'advanced_exam_generation',
		'unlimited_ai_exams',
		'priority_features',
		'advanced_analytics',
		'export_formats',
		'priority_support',
		'custom_branding',
	]

	return (
		<div className='flex min-h-screen items-center justify-center bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950 dark:to-gray-900 p-4'>
			<div className='w-full max-w-4xl space-y-6'>
				<Card className='shadow-xl'>
					<CardHeader className='text-center space-y-2'>
						<div className='flex justify-center mb-4'>
							<img
								src='/frog.png'
								alt={t('common.logo_alt')}
								className='w-20 h-20'
							/>
						</div>
						<CardTitle className='text-3xl font-bold'>
							{t('pages.auth.subscription.title')}
						</CardTitle>
						<CardDescription className='text-base max-w-lg mx-auto'>
							{t('pages.auth.subscription.subtitle', {
								name: user?.firstName || '',
							})}
						</CardDescription>
					</CardHeader>
					<CardContent className='space-y-6'>
						<div className='grid md:grid-cols-2 gap-6'>
							{/* Free Tier Card */}
							<Card
								className='relative border-2 hover:border-muted-foreground/50 transition-colors cursor-pointer'
								onClick={handleSelectFree}
							>
								<CardHeader>
									<div className='flex items-center justify-between'>
										<Badge variant='secondary'>
											<Zap className='w-3 h-3 mr-1' />
											{t('badges.free')}
										</Badge>
									</div>
									<CardTitle className='text-2xl'>
										{t('pages.auth.subscription.free_title')}
									</CardTitle>
									<CardDescription>
										{t('pages.auth.subscription.free_description')}
									</CardDescription>
								</CardHeader>
								<CardContent className='space-y-4'>
									<div className='text-3xl font-bold'>
										{freeTier?.currency === 'VND' ? '₫' : '$'}0
										<span className='text-base font-normal text-muted-foreground'>
											/{t('pages.auth.subscription.forever')}
										</span>
									</div>
									<Separator />
									<ul className='space-y-3'>
										{freeFeatures.map((feature, index) => (
											<li key={index} className='flex items-start gap-2'>
												<Check className='w-5 h-5 text-green-500 shrink-0 mt-0.5' />
												<span className='text-sm'>
													{t(
														`pages.auth.subscription.features.free.${feature}`
													)}
												</span>
											</li>
										))}
									</ul>
								</CardContent>
								<CardFooter className='pt-6'>
									<Button
										variant='outline'
										className='w-full'
										onClick={e => {
											e.stopPropagation()
											handleSelectFree()
										}}
									>
										{t('actions.continue_free')}
									</Button>
								</CardFooter>
							</Card>

							{/* Pro Tier Card */}
							<Card
								className='relative border-2 border-primary shadow-lg cursor-pointer'
								onClick={handleSelectPro}
							>
								<div className='absolute -top-3 left-1/2 -translate-x-1/2'>
									<Badge className='bg-primary text-primary-foreground'>
										<Sparkles className='w-3 h-3 mr-1' />
										{t('badges.recommended')}
									</Badge>
								</div>
								<CardHeader className='pt-6'>
									<div className='flex items-center justify-between'>
										<Badge variant='default'>
											<Crown className='w-3 h-3 mr-1' />
											{t('badges.pro')}
										</Badge>
									</div>
									<CardTitle className='text-2xl'>
										{t('pages.auth.subscription.pro_title')}
									</CardTitle>
									<CardDescription>
										{t('pages.auth.subscription.pro_description')}
									</CardDescription>
								</CardHeader>
								<CardContent className='space-y-4'>
									<div className='text-3xl font-bold'>
										{proTier?.currency === 'VND' ? '₫' : '$'}
										{proTier?.price?.toLocaleString() || '99,000'}
										<span className='text-base font-normal text-muted-foreground'>
											/{proTier?.durationInDays || 30} {t('common.days')}
										</span>
									</div>
									<Separator />
									<ul className='space-y-3'>
										{proFeatures.map((feature, index) => (
											<li key={index} className='flex items-start gap-2'>
												<Check className='w-5 h-5 text-primary shrink-0 mt-0.5' />
												<span className='text-sm'>
													{t(`pages.auth.subscription.features.pro.${feature}`)}
												</span>
											</li>
										))}
									</ul>
								</CardContent>
								<CardFooter className='pt-6'>
									<Button
										className='w-full'
										onClick={e => {
											e.stopPropagation()
											handleSelectPro()
										}}
										disabled={isSubscribing}
									>
										{isSubscribing ? (
											t('actions.processing')
										) : (
											<>
												{t('actions.upgrade_to_pro')}
												<ArrowRight className='w-4 h-4 ml-2' />
											</>
										)}
									</Button>
								</CardFooter>
							</Card>
						</div>

						<div className='text-center text-sm text-muted-foreground'>
							<p>{t('pages.auth.subscription.footer_note')}</p>
							<p className='mt-2'>
								<button
									type='button'
									onClick={handleSkip}
									className='text-primary hover:underline'
								>
									{t('actions.skip_for_now')}
								</button>
							</p>
						</div>
					</CardContent>
				</Card>
			</div>
		</div>
	)
}

export default SelectSubscriptionPage
