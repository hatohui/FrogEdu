import React from 'react'
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
import { Skeleton } from '@/components/ui/skeleton'
import {
	Check,
	Crown,
	Zap,
	BookOpen,
	Brain,
	Clock,
	Sparkles,
} from 'lucide-react'
import { useSubscription } from '@/hooks/useSubscription'

const SubscriptionPage = (): React.ReactElement => {
	const {
		subscription,
		tiers,
		isLoading,
		subscribeToPro,
		isSubscribing,
		isPro,
		isFree,
	} = useSubscription()

	const formatDate = (dateString?: string) => {
		if (!dateString) return 'N/A'
		return new Date(dateString).toLocaleDateString('vi-VN', {
			year: 'numeric',
			month: 'long',
			day: 'numeric',
		})
	}

	const formatPrice = (price: number, currency: string) => {
		return new Intl.NumberFormat('vi-VN', {
			style: 'currency',
			currency: currency,
		}).format(price)
	}

	const handleSubscribe = async () => {
		try {
			await subscribeToPro()
		} catch (error) {
			console.error('Failed to subscribe:', error)
		}
	}

	if (isLoading) {
		return (
			<div className='space-y-6'>
				<Skeleton className='h-10 w-64' />
				<Skeleton className='h-[200px]' />
				<Skeleton className='h-[400px]' />
			</div>
		)
	}

	const proFeatures = [
		{ icon: Brain, text: 'AI-powered question generation' },
		{ icon: BookOpen, text: 'Unlimited exam creation' },
		{ icon: Sparkles, text: 'AI tutoring assistant' },
		{ icon: Zap, text: 'Priority support' },
		{ icon: Clock, text: 'Extended exam history' },
	]

	const freeFeatures = [
		{ text: 'Basic exam taking', included: true },
		{ text: 'Limited question bank access', included: true },
		{ text: 'AI question generation', included: false },
		{ text: 'AI tutoring', included: false },
		{ text: 'Priority support', included: false },
	]

	return (
		<div className='space-y-6'>
			{/* Current Plan Card */}
			<Card
				className={
					isPro
						? 'border-2 border-amber-500 bg-gradient-to-br from-amber-50 to-orange-50 dark:from-amber-950/20 dark:to-orange-950/20'
						: ''
				}
			>
				<CardHeader>
					<div className='flex items-center justify-between'>
						<div>
							<CardTitle className='flex items-center gap-2'>
								Current Plan
								{isPro && (
									<Badge className='bg-gradient-to-r from-amber-500 to-orange-500 text-white'>
										<Crown className='h-3 w-3 mr-1' />
										Pro
									</Badge>
								)}
							</CardTitle>
							<CardDescription>
								{isPro
									? 'You have access to all premium features'
									: 'Upgrade to unlock premium features'}
							</CardDescription>
						</div>
						{isPro && <Crown className='h-12 w-12 text-amber-500' />}
					</div>
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='grid gap-4 sm:grid-cols-2'>
						<div>
							<p className='text-sm text-muted-foreground'>Plan</p>
							<p className='text-lg font-semibold'>
								{subscription?.planName || 'Free'}
							</p>
						</div>
						<div>
							<p className='text-sm text-muted-foreground'>Status</p>
							<div className='flex items-center gap-2'>
								<Badge
									variant={subscription?.isActive ? 'default' : 'secondary'}
									className={subscription?.isActive ? 'bg-green-600' : ''}
								>
									{subscription?.isActive ? 'Active' : 'Inactive'}
								</Badge>
							</div>
						</div>
						{subscription?.isActive && (
							<>
								<div>
									<p className='text-sm text-muted-foreground'>Start Date</p>
									<p>{formatDate(subscription.startDate)}</p>
								</div>
								<div>
									<p className='text-sm text-muted-foreground'>Expires</p>
									<p>{formatDate(subscription.endDate)}</p>
								</div>
							</>
						)}
					</div>
				</CardContent>
			</Card>

			{/* Subscription Plans */}
			<div className='space-y-4'>
				<h2 className='text-xl font-semibold'>Available Plans</h2>
				<div className='grid gap-6 md:grid-cols-2'>
					{/* Free Plan */}
					<Card className={isFree ? 'border-2 border-primary' : ''}>
						<CardHeader>
							<CardTitle>Free</CardTitle>
							<CardDescription>
								For students just getting started
							</CardDescription>
							<div className='text-3xl font-bold'>
								{formatPrice(0, 'VND')}
								<span className='text-sm font-normal text-muted-foreground'>
									/month
								</span>
							</div>
						</CardHeader>
						<CardContent>
							<ul className='space-y-3'>
								{freeFeatures.map((feature, index) => (
									<li key={index} className='flex items-center gap-2'>
										{feature.included ? (
											<Check className='h-4 w-4 text-green-600' />
										) : (
											<span className='h-4 w-4 text-muted-foreground'>—</span>
										)}
										<span
											className={
												feature.included ? '' : 'text-muted-foreground'
											}
										>
											{feature.text}
										</span>
									</li>
								))}
							</ul>
						</CardContent>
						<CardFooter>
							{isFree && (
								<Button variant='outline' className='w-full' disabled>
									Current Plan
								</Button>
							)}
						</CardFooter>
					</Card>

					{/* Pro Plan */}
					<Card
						className={
							isPro
								? 'border-2 border-amber-500'
								: 'border-2 border-dashed border-amber-300'
						}
					>
						<CardHeader>
							<div className='flex items-center gap-2'>
								<CardTitle>Pro</CardTitle>
								<Badge className='bg-gradient-to-r from-amber-500 to-orange-500 text-white'>
									<Sparkles className='h-3 w-3 mr-1' />
									Recommended
								</Badge>
							</div>
							<CardDescription>
								For teachers and serious learners
							</CardDescription>
							<div className='text-3xl font-bold'>
								{tiers?.find(t => t.name.toLowerCase() === 'pro')
									? formatPrice(
											tiers.find(t => t.name.toLowerCase() === 'pro')!.price,
											tiers.find(t => t.name.toLowerCase() === 'pro')!.currency
										)
									: formatPrice(99000, 'VND')}
								<span className='text-sm font-normal text-muted-foreground'>
									/month
								</span>
							</div>
						</CardHeader>
						<CardContent>
							<ul className='space-y-3'>
								{proFeatures.map((feature, index) => (
									<li key={index} className='flex items-center gap-2'>
										<feature.icon className='h-4 w-4 text-amber-500' />
										<span>{feature.text}</span>
									</li>
								))}
							</ul>
						</CardContent>
						<CardFooter>
							{isPro ? (
								<Button variant='outline' className='w-full' disabled>
									<Crown className='h-4 w-4 mr-2' />
									Current Plan
								</Button>
							) : (
								<Button
									className='w-full bg-gradient-to-r from-amber-500 to-orange-500 hover:from-amber-600 hover:to-orange-600'
									onClick={handleSubscribe}
									disabled={isSubscribing}
								>
									{isSubscribing ? (
										<>
											<span className='animate-spin mr-2'>⏳</span>
											Subscribing...
										</>
									) : (
										<>
											<Crown className='h-4 w-4 mr-2' />
											Upgrade to Pro
										</>
									)}
								</Button>
							)}
						</CardFooter>
					</Card>
				</div>
			</div>

			{/* Info Note */}
			<Card className='bg-blue-50 dark:bg-blue-950/20 border-blue-200 dark:border-blue-800'>
				<CardContent className='pt-6'>
					<div className='flex items-start gap-3'>
						<Zap className='h-5 w-5 text-blue-600 mt-0.5' />
						<div>
							<p className='font-medium text-blue-900 dark:text-blue-100'>
								Demo Mode
							</p>
							<p className='text-sm text-blue-700 dark:text-blue-300'>
								This is a demo subscription. In production, this will be
								integrated with payment providers like VNPay, Stripe, or PayOS.
								Click "Upgrade to Pro" to instantly activate Pro features for
								testing.
							</p>
						</div>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default SubscriptionPage
