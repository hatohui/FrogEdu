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
	AlertDialog,
	AlertDialogAction,
	AlertDialogCancel,
	AlertDialogContent,
	AlertDialogDescription,
	AlertDialogFooter,
	AlertDialogHeader,
	AlertDialogTitle,
	AlertDialogTrigger,
} from '@/components/ui/alert-dialog'
import {
	Check,
	Crown,
	Zap,
	BookOpen,
	Brain,
	Clock,
	Sparkles,
	XCircle,
	AlertTriangle,
} from 'lucide-react'
import { useSubscription } from '@/hooks/useSubscription'

const SubscriptionPage = (): React.ReactElement => {
	const {
		subscription,
		tiers,
		isLoading,
		subscribeToPro,
		isSubscribing,
		cancelSubscription,
		isCancelling,
		isPro,
		isFree,
		isCancelled,
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

	const handleCancelSubscription = async () => {
		try {
			await cancelSubscription()
		} catch (error) {
			console.error('Failed to cancel subscription:', error)
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
		<div className='space-y-6 max-w-4xl mx-auto'>
			{/* Current Plan Card */}
			<Card
				className={
					isPro
						? 'border-2 border-amber-500 bg-gradient-to-br from-amber-50 to-orange-50 dark:from-amber-950/20 dark:to-orange-950/20'
						: isCancelled
							? 'border-2 border-red-300 bg-gradient-to-br from-red-50 to-orange-50 dark:from-red-950/20 dark:to-orange-950/20'
							: ''
				}
			>
				<CardHeader>
					<div className='flex items-center justify-between'>
						<div>
							<CardTitle className='flex items-center gap-2'>
								Current Plan
								{isPro && !isCancelled && (
									<Badge className='bg-gradient-to-r from-amber-500 to-orange-500 text-white'>
										<Crown className='h-3 w-3 mr-1' />
										Pro
									</Badge>
								)}
								{isCancelled && (
									<Badge variant='destructive'>
										<XCircle className='h-3 w-3 mr-1' />
										Cancelled
									</Badge>
								)}
							</CardTitle>
							<CardDescription>
								{isPro && !isCancelled
									? 'You have access to all premium features'
									: isCancelled
										? 'Your subscription has been cancelled'
										: 'Upgrade to unlock premium features'}
							</CardDescription>
						</div>
						{isPro && !isCancelled && (
							<Crown className='h-12 w-12 text-amber-500' />
						)}
						{isCancelled && (
							<AlertTriangle className='h-12 w-12 text-red-500' />
						)}
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
									variant={
										isCancelled
											? 'destructive'
											: subscription?.isActive
												? 'default'
												: 'secondary'
									}
									className={
										subscription?.isActive && !isCancelled ? 'bg-green-600' : ''
									}
								>
									{isCancelled
										? 'Cancelled'
										: subscription?.isActive
											? 'Active'
											: 'Inactive'}
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
									<p className='text-sm text-muted-foreground'>
										{isCancelled ? 'Access Until' : 'Expires'}
									</p>
									<p>{formatDate(subscription.endDate)}</p>
								</div>
							</>
						)}
					</div>

					{/* Cancel Subscription Button */}
					{isPro && !isCancelled && (
						<div className='pt-4 border-t'>
							<AlertDialog>
								<AlertDialogTrigger asChild>
									<Button
										variant='outline'
										className='text-red-600 hover:text-red-700 hover:bg-red-50'
									>
										<XCircle className='h-4 w-4 mr-2' />
										Cancel Subscription
									</Button>
								</AlertDialogTrigger>
								<AlertDialogContent>
									<AlertDialogHeader>
										<AlertDialogTitle>
											Cancel your subscription?
										</AlertDialogTitle>
										<AlertDialogDescription className='space-y-2'>
											<p>
												Are you sure you want to cancel your Pro subscription?
											</p>
											<p>
												You will continue to have access to Pro features until{' '}
												<strong>{formatDate(subscription?.endDate)}</strong>.
												After that, your account will be downgraded to the Free
												plan.
											</p>
										</AlertDialogDescription>
									</AlertDialogHeader>
									<AlertDialogFooter>
										<AlertDialogCancel>Keep Subscription</AlertDialogCancel>
										<AlertDialogAction
											onClick={handleCancelSubscription}
											disabled={isCancelling}
											className='bg-red-600 hover:bg-red-700'
										>
											{isCancelling ? 'Cancelling...' : 'Yes, Cancel'}
										</AlertDialogAction>
									</AlertDialogFooter>
								</AlertDialogContent>
							</AlertDialog>
						</div>
					)}

					{/* Resubscribe option for cancelled subscriptions */}
					{isCancelled && (
						<div className='pt-4 border-t'>
							<p className='text-sm text-muted-foreground mb-3'>
								Changed your mind? You can resubscribe anytime.
							</p>
							<Button
								onClick={handleSubscribe}
								disabled={isSubscribing}
								className='bg-gradient-to-r from-amber-500 to-orange-500 hover:from-amber-600 hover:to-orange-600'
							>
								{isSubscribing ? (
									<>
										<span className='animate-spin mr-2'>⏳</span>
										Resubscribing...
									</>
								) : (
									<>
										<Crown className='h-4 w-4 mr-2' />
										Resubscribe to Pro
									</>
								)}
							</Button>
						</div>
					)}
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
				<CardContent>
					<div className='flex items-start gap-3'>
						<Zap className='h-5 w-5 text-blue-600 mt-0.5' />
						<div>
							<p className='font-medium text-blue-900 dark:text-blue-100'>
								{isPro && !isCancelled
									? 'You can cancel your subscription at any time. Your access will continue until the end of your billing period.'
									: isCancelled
										? 'Your subscription has been cancelled but you still have access until the end of your billing period.'
										: 'Upgrade to Pro to unlock all premium features including AI-powered exam generation and tutoring.'}
							</p>
						</div>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default SubscriptionPage
