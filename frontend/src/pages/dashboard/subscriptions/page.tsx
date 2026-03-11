import React, { useMemo, useState } from 'react'
import {
	CreditCard,
	Search,
	MoreVertical,
	Users,
	DollarSign,
	TrendingUp,
	CheckCircle2,
	XCircle,
	Pause,
	Play,
	RefreshCw,
	Loader2,
	User,
	Mail,
	Calendar,
	Zap,
	Clock,
} from 'lucide-react'
import { useTranslation } from 'react-i18next'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import { Skeleton } from '@/components/ui/skeleton'
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'
import { Separator } from '@/components/ui/separator'
import {
	Sheet,
	SheetContent,
	SheetHeader,
	SheetTitle,
} from '@/components/ui/sheet'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuSeparator,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogFooter,
	DialogHeader,
	DialogTitle,
} from '@/components/ui/dialog'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import {
	useAdminSubscriptions,
	useAdminTiers,
	useAdminTransactions,
	useSubscriptionDashboardStats,
} from '@/hooks/useSubscription'
import subscriptionService from '@/services/subscription.service'
import userService from '@/services/user-microservice/user.service'
import { useQueryClient, useQueries, useQuery } from '@tanstack/react-query'
import { userKeys } from '@/hooks/useUsers'
import { toast } from 'sonner'
import type { AdminSubscriptionDto } from '@/types/dtos/subscriptions'
import type { SubscriptionTier } from '@/types/model/subscription'
import type { GetMeResponse } from '@/types/dtos/users/user'

// ─── Stat Card Component ───

interface StatCardProps {
	title: string
	value: string | number
	icon: React.ComponentType<{ className?: string }>
	color: string
	bgColor: string
}

const StatCard = ({
	title,
	value,
	icon: Icon,
	color,
	bgColor,
}: StatCardProps) => (
	<Card>
		<CardContent className='p-6'>
			<div className='flex items-center justify-between'>
				<div className='space-y-1'>
					<p className='text-sm text-muted-foreground'>{title}</p>
					<p className='text-2xl font-bold'>{value}</p>
				</div>
				<div className={`p-3 rounded-full ${bgColor}`}>
					<Icon className={`h-5 w-5 ${color}`} />
				</div>
			</div>
		</CardContent>
	</Card>
)

// ─── User Cell Component ───

const UserCell = ({ user }: { user: GetMeResponse | undefined }) => {
	if (!user) {
		return (
			<span className='text-muted-foreground text-xs italic'>Loading…</span>
		)
	}
	const initials =
		`${user.firstName?.[0] ?? ''}${user.lastName?.[0] ?? ''}`.toUpperCase()
	return (
		<div className='flex items-center gap-2'>
			<Avatar className='h-7 w-7'>
				<AvatarImage src={user.avatarUrl ?? undefined} />
				<AvatarFallback className='text-xs'>{initials}</AvatarFallback>
			</Avatar>
			<div className='leading-tight'>
				<p className='text-sm font-medium'>
					{user.firstName} {user.lastName}
				</p>
				<p className='text-xs text-muted-foreground'>{user.email}</p>
			</div>
		</div>
	)
}

// ─── Subscription Detail Sheet ───

interface SubscriptionDetailSheetProps {
	sub: AdminSubscriptionDto | null
	user: GetMeResponse | undefined
	open: boolean
	onOpenChange: (open: boolean) => void
}

const SubscriptionDetailSheet = ({
	sub,
	user,
	open,
	onOpenChange,
}: SubscriptionDetailSheetProps) => {
	const { t } = useTranslation()
	const { data: aiUsage, isLoading: aiLoading } = useQuery({
		queryKey: ['subscription', 'ai-usage-admin', sub?.userId],
		queryFn: () => subscriptionService.getAIUsageLimitForUser(sub!.userId),
		enabled: open && !!sub?.userId,
		staleTime: 30 * 1000,
	})

	if (!sub) return null

	const startDate = new Date(sub.startDate)
	const endDate = new Date(sub.endDate)
	const now = new Date()
	const totalMs = endDate.getTime() - startDate.getTime()
	const elapsedMs = Math.min(now.getTime() - startDate.getTime(), totalMs)
	const monthsSubscribed = Math.max(
		0,
		Math.floor(elapsedMs / (1000 * 60 * 60 * 24 * 30))
	)
	const daysLeft = Math.max(
		0,
		Math.ceil((endDate.getTime() - now.getTime()) / (1000 * 60 * 60 * 24))
	)

	const initials = user
		? `${user.firstName?.[0] ?? ''}${user.lastName?.[0] ?? ''}`.toUpperCase()
		: '?'

	return (
		<Sheet open={open} onOpenChange={onOpenChange}>
			<SheetContent className='w-1/4 px-5 sm:w-[480px] overflow-y-auto'>
				<SheetHeader className='pb-4'>
					<SheetTitle>
						{t('admin.subscriptions.detail.title', 'Subscription Details')}
					</SheetTitle>
				</SheetHeader>

				{/* User Info */}
				<div className='space-y-4'>
					<div className='flex items-center gap-4 p-4 rounded-lg bg-muted/50'>
						<Avatar className='h-14 w-14'>
							<AvatarImage src={user?.avatarUrl ?? undefined} />
							<AvatarFallback className='text-lg font-semibold'>
								{initials}
							</AvatarFallback>
						</Avatar>
						<div>
							{user ? (
								<>
									<p className='text-base font-semibold'>
										{user.firstName} {user.lastName}
									</p>
									<div className='flex items-center gap-1 text-sm text-muted-foreground'>
										<Mail className='h-3 w-3' />
										{user.email}
									</div>
									<div className='flex items-center gap-1 text-xs text-muted-foreground mt-1'>
										<User className='h-3 w-3' />
										{user.id}
									</div>
								</>
							) : (
								<>
									<Skeleton className='h-4 w-32 mb-1' />
									<Skeleton className='h-3 w-40' />
								</>
							)}
						</div>
					</div>

					<Separator />

					{/* Subscription Info */}
					<div className='space-y-3'>
						<h3 className='text-sm font-semibold text-muted-foreground uppercase tracking-wide'>
							{t('admin.subscriptions.detail.subscription', 'Subscription')}
						</h3>
						<div className='grid grid-cols-2 gap-3'>
							<div className='space-y-1'>
								<p className='text-xs text-muted-foreground'>
									{t('admin.subscriptions.table.plan', 'Plan')}
								</p>
								<Badge variant='outline' className='font-medium'>
									{sub.planName}
								</Badge>
							</div>
							<div className='space-y-1'>
								<p className='text-xs text-muted-foreground'>
									{t('admin.subscriptions.table.status', 'Status')}
								</p>
								{getStatusBadge(sub.status)}
							</div>
							<div className='space-y-1'>
								<p className='text-xs text-muted-foreground flex items-center gap-1'>
									<Calendar className='h-3 w-3' />
									{t('admin.subscriptions.table.start_date', 'Start Date')}
								</p>
								<p className='text-sm font-medium'>
									{startDate.toLocaleDateString()}
								</p>
							</div>
							<div className='space-y-1'>
								<p className='text-xs text-muted-foreground flex items-center gap-1'>
									<Calendar className='h-3 w-3' />
									{t('admin.subscriptions.table.end_date', 'End Date')}
								</p>
								<p className='text-sm font-medium'>
									{endDate.toLocaleDateString()}
								</p>
							</div>
							<div className='space-y-1'>
								<p className='text-xs text-muted-foreground flex items-center gap-1'>
									<Clock className='h-3 w-3' />
									{t(
										'admin.subscriptions.detail.months_subscribed',
										'Months Subscribed'
									)}
								</p>
								<p className='text-sm font-medium'>{monthsSubscribed}</p>
							</div>
							<div className='space-y-1'>
								<p className='text-xs text-muted-foreground'>
									{t('admin.subscriptions.detail.days_left', 'Days Left')}
								</p>
								<p
									className={`text-sm font-medium ${daysLeft <= 7 ? 'text-destructive' : ''}`}
								>
									{daysLeft}
								</p>
							</div>
						</div>
					</div>

					<Separator />

					{/* AI Usage */}
					<div className='space-y-3'>
						<h3 className='text-sm font-semibold text-muted-foreground uppercase tracking-wide flex items-center gap-1'>
							<Zap className='h-3.5 w-3.5' />
							{t('admin.subscriptions.detail.ai_usage', 'AI Usage')}
						</h3>
						{aiLoading ? (
							<div className='space-y-2'>
								<Skeleton className='h-4 w-full' />
								<Skeleton className='h-4 w-3/4' />
							</div>
						) : aiUsage ? (
							<div className='grid grid-cols-2 gap-3'>
								<div className='space-y-1'>
									<p className='text-xs text-muted-foreground'>
										{t(
											'admin.subscriptions.detail.used_this_month',
											'Used This Month'
										)}
									</p>
									<p className='text-sm font-medium'>{aiUsage.usedCount}</p>
								</div>
								<div className='space-y-1'>
									<p className='text-xs text-muted-foreground'>
										{t('admin.subscriptions.detail.ai_limit', 'Limit')}
									</p>
									<p className='text-sm font-medium'>
										{aiUsage.isUnlimited ? '∞' : aiUsage.maxAllowed}
									</p>
								</div>
								{!aiUsage.isUnlimited && aiUsage.maxAllowed !== null && (
									<div className='col-span-2 space-y-1'>
										<div className='flex justify-between text-xs text-muted-foreground'>
											<span>
												{t('admin.subscriptions.detail.remaining', 'Remaining')}
											</span>
											<span>
												{aiUsage.remaining} / {aiUsage.maxAllowed}
											</span>
										</div>
										<div className='h-2 rounded-full bg-muted overflow-hidden'>
											<div
												className='h-full rounded-full bg-[var(--frog-700)]'
												style={{
													width: `${Math.min(100, (aiUsage.usedCount / aiUsage.maxAllowed) * 100)}%`,
												}}
											/>
										</div>
									</div>
								)}
							</div>
						) : (
							<p className='text-sm text-muted-foreground'>
								{t(
									'admin.subscriptions.detail.no_ai_data',
									'No usage data available'
								)}
							</p>
						)}
					</div>

					<Separator />

					{/* IDs */}
					<div className='space-y-2'>
						<h3 className='text-sm font-semibold text-muted-foreground uppercase tracking-wide'>
							{t('admin.subscriptions.detail.ids', 'Reference IDs')}
						</h3>
						<div className='space-y-1.5'>
							<div className='flex justify-between text-xs'>
								<span className='text-muted-foreground'>Subscription ID</span>
								<span className='font-mono'>{sub.id.slice(0, 8)}…</span>
							</div>
							<div className='flex justify-between text-xs'>
								<span className='text-muted-foreground'>User ID</span>
								<span className='font-mono'>{sub.userId.slice(0, 8)}…</span>
							</div>
						</div>
					</div>
				</div>
			</SheetContent>
		</Sheet>
	)
}

// ─── Status Badge Helper ───

const getStatusBadge = (status: string) => {
	switch (status?.toLowerCase()) {
		case 'active':
			return <Badge className='bg-green-600'>{status}</Badge>
		case 'expired':
			return <Badge variant='secondary'>{status}</Badge>
		case 'cancelled':
			return <Badge variant='destructive'>{status}</Badge>
		case 'suspended':
			return <Badge className='bg-amber-500 text-white'>{status}</Badge>
		default:
			return <Badge variant='outline'>{status}</Badge>
	}
}

const getPaymentStatusBadge = (status: string) => {
	switch (status?.toLowerCase()) {
		case 'completed':
		case 'success':
			return <Badge className='bg-green-600'>{status}</Badge>
		case 'pending':
			return <Badge className='bg-amber-500 text-white'>{status}</Badge>
		case 'failed':
			return <Badge variant='destructive'>{status}</Badge>
		case 'refunded':
			return <Badge variant='secondary'>{status}</Badge>
		default:
			return <Badge variant='outline'>{status}</Badge>
	}
}

// ─── Main Page Component ───

const SubscriptionsPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const queryClient = useQueryClient()

	// State
	const [statusFilter, setStatusFilter] = useState<string>('all')
	const [searchQuery, setSearchQuery] = useState('')
	const [actionLoading, setActionLoading] = useState<string | null>(null)
	const [selectedSub, setSelectedSub] = useState<AdminSubscriptionDto | null>(
		null
	)
	const [sheetOpen, setSheetOpen] = useState(false)
	const [confirmDialog, setConfirmDialog] = useState<{
		open: boolean
		title: string
		description: string
		action: () => Promise<void>
	}>({ open: false, title: '', description: '', action: async () => {} })

	// Data
	const { data: stats, isLoading: statsLoading } =
		useSubscriptionDashboardStats()
	const { data: subscriptions, isLoading: subsLoading } = useAdminSubscriptions(
		statusFilter === 'all' ? undefined : statusFilter
	)
	const { data: tiers, isLoading: tiersLoading } = useAdminTiers(true)
	const { data: transactions, isLoading: txLoading } = useAdminTransactions()

	// Fetch user data for all unique userIds in parallel
	const uniqueUserIds = useMemo(
		() => [...new Set(subscriptions?.map(s => s.userId) ?? [])],
		[subscriptions]
	)
	const userQueries = useQueries({
		queries: uniqueUserIds.map(userId => ({
			queryKey: userKeys.detail(userId),
			queryFn: () => userService.getUserById(userId),
			staleTime: 5 * 60 * 1000,
		})),
	})
	const userMap = useMemo(() => {
		const map: Record<string, GetMeResponse> = {}
		uniqueUserIds.forEach((id, i) => {
			const data = userQueries[i]?.data
			if (data) map[id] = data
		})
		return map
	}, [uniqueUserIds, userQueries])

	// Filter subscriptions by search (name, email, plan, id)
	const filteredSubscriptions = subscriptions?.filter(sub => {
		if (!searchQuery) return true
		const q = searchQuery.toLowerCase()
		const user = userMap[sub.userId]
		return (
			sub.userId.toLowerCase().includes(q) ||
			sub.planName.toLowerCase().includes(q) ||
			sub.id.toLowerCase().includes(q) ||
			(user &&
				(`${user.firstName} ${user.lastName}`.toLowerCase().includes(q) ||
					user.email.toLowerCase().includes(q)))
		)
	})

	// Stat cards
	const statCards = [
		{
			title: t('admin.subscriptions.stats.total_revenue'),
			value: stats ? `$${stats.totalRevenue.toLocaleString()}` : '$0',
			icon: DollarSign,
			color: 'text-green-600',
			bgColor: 'bg-green-100 dark:bg-green-950',
		},
		{
			title: t('admin.subscriptions.stats.active'),
			value: stats?.activeSubscriptions ?? 0,
			icon: CheckCircle2,
			color: 'text-blue-600',
			bgColor: 'bg-blue-100 dark:bg-blue-950',
		},
		{
			title: t('admin.subscriptions.stats.total'),
			value: stats?.totalSubscriptions ?? 0,
			icon: Users,
			color: 'text-purple-600',
			bgColor: 'bg-purple-100 dark:bg-purple-950',
		},
		{
			title: t('admin.subscriptions.stats.monthly_growth'),
			value: stats?.monthlyRevenue?.length
				? `$${stats.monthlyRevenue[stats.monthlyRevenue.length - 1].revenue.toLocaleString()}`
				: '$0',
			icon: TrendingUp,
			color: 'text-amber-600',
			bgColor: 'bg-amber-100 dark:bg-amber-950',
		},
	]

	// Actions
	const invalidateAll = () => {
		queryClient.invalidateQueries({ queryKey: ['admin'] })
		queryClient.invalidateQueries({
			queryKey: ['subscription', 'dashboard-stats'],
		})
	}

	const getUserLabel = (sub: AdminSubscriptionDto) => {
		const user = userMap[sub.userId]
		return user
			? `${user.firstName} ${user.lastName}`
			: sub.userId.slice(0, 8) + '...'
	}

	const handleSuspend = (sub: AdminSubscriptionDto) => {
		setConfirmDialog({
			open: true,
			title: t('admin.subscriptions.confirm_suspend'),
			description: t('admin.subscriptions.confirm_suspend_desc', {
				user: getUserLabel(sub),
			}),
			action: async () => {
				setActionLoading(sub.id)
				try {
					await subscriptionService.suspendSubscription(sub.id)
					toast.success(t('admin.subscriptions.suspended_success'))
					invalidateAll()
				} catch {
					toast.error(t('admin.subscriptions.action_failed'))
				} finally {
					setActionLoading(null)
				}
			},
		})
	}

	const handleActivate = (sub: AdminSubscriptionDto) => {
		setConfirmDialog({
			open: true,
			title: t('admin.subscriptions.confirm_activate'),
			description: t('admin.subscriptions.confirm_activate_desc', {
				user: getUserLabel(sub),
			}),
			action: async () => {
				setActionLoading(sub.id)
				try {
					await subscriptionService.activateSubscription(sub.id)
					toast.success(t('admin.subscriptions.activated_success'))
					invalidateAll()
				} catch {
					toast.error(t('admin.subscriptions.action_failed'))
				} finally {
					setActionLoading(null)
				}
			},
		})
	}

	const handleToggleTier = async (tier: SubscriptionTier) => {
		setActionLoading(tier.id)
		try {
			if (tier.isActive) {
				await subscriptionService.deactivateTier(tier.id)
				toast.success(t('admin.subscriptions.tier_deactivated'))
			} else {
				await subscriptionService.activateTier(tier.id)
				toast.success(t('admin.subscriptions.tier_activated'))
			}
			invalidateAll()
		} catch {
			toast.error(t('admin.subscriptions.action_failed'))
		} finally {
			setActionLoading(null)
		}
	}

	return (
		<div className='space-y-6 p-6 max-w-[1600px] mx-auto'>
			{/* Header */}
			<div className='space-y-1'>
				<h1 className='text-3xl font-bold tracking-tight flex items-center gap-2'>
					<CreditCard className='h-8 w-8' />
					{t('admin.subscriptions.title')}
				</h1>
				<p className='text-muted-foreground'>
					{t('admin.subscriptions.subtitle')}
				</p>
			</div>

			{/* Stats */}
			{statsLoading ? (
				<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-4'>
					{[1, 2, 3, 4].map(i => (
						<Skeleton key={i} className='h-24' />
					))}
				</div>
			) : (
				<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-4'>
					{statCards.map((stat, i) => (
						<StatCard key={i} {...stat} />
					))}
				</div>
			)}

			{/* Tabs */}
			<Tabs defaultValue='subscriptions'>
				<TabsList>
					<TabsTrigger value='subscriptions'>
						{t('admin.subscriptions.tabs.subscriptions')}
					</TabsTrigger>
					<TabsTrigger value='tiers'>
						{t('admin.subscriptions.tabs.tiers')}
					</TabsTrigger>
					<TabsTrigger value='transactions'>
						{t('admin.subscriptions.tabs.transactions')}
					</TabsTrigger>
				</TabsList>

				{/* Subscriptions Tab */}
				<TabsContent value='subscriptions'>
					<Card>
						<CardHeader>
							<div className='flex items-center justify-between'>
								<CardTitle>
									{t('admin.subscriptions.all_subscriptions')}
								</CardTitle>
								<div className='flex items-center gap-3'>
									<div className='relative'>
										<Search className='absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground' />
										<Input
											placeholder={t('admin.subscriptions.search_placeholder')}
											value={searchQuery}
											onChange={e => setSearchQuery(e.target.value)}
											className='pl-9 w-64'
										/>
									</div>
									<Select value={statusFilter} onValueChange={setStatusFilter}>
										<SelectTrigger className='w-40'>
											<SelectValue
												placeholder={t('admin.subscriptions.filter_status')}
											/>
										</SelectTrigger>
										<SelectContent>
											<SelectItem value='all'>{t('common.all')}</SelectItem>
											<SelectItem value='Active'>Active</SelectItem>
											<SelectItem value='Expired'>Expired</SelectItem>
											<SelectItem value='Cancelled'>Cancelled</SelectItem>
											<SelectItem value='Suspended'>Suspended</SelectItem>
										</SelectContent>
									</Select>
								</div>
							</div>
						</CardHeader>
						<CardContent>
							{subsLoading ? (
								<div className='space-y-3'>
									{[1, 2, 3, 4, 5].map(i => (
										<Skeleton key={i} className='h-12' />
									))}
								</div>
							) : (
								<Table>
									<TableHeader>
										<TableRow>
											<TableHead>
												{t('admin.subscriptions.table.user', 'User')}
											</TableHead>
											<TableHead>
												{t('admin.subscriptions.table.plan')}
											</TableHead>
											<TableHead>
												{t('admin.subscriptions.table.status')}
											</TableHead>
											<TableHead>
												{t('admin.subscriptions.table.start_date')}
											</TableHead>
											<TableHead>
												{t('admin.subscriptions.table.end_date')}
											</TableHead>
											<TableHead className='text-right'>
												{t('admin.subscriptions.table.actions')}
											</TableHead>
										</TableRow>
									</TableHeader>
									<TableBody>
										{filteredSubscriptions &&
										filteredSubscriptions.length > 0 ? (
											filteredSubscriptions.map(sub => (
												<TableRow
													key={sub.id}
													className='cursor-pointer hover:bg-muted/50'
													onClick={() => {
														setSelectedSub(sub)
														setSheetOpen(true)
													}}
												>
													<TableCell>
														<UserCell user={userMap[sub.userId]} />
													</TableCell>
													<TableCell>
														<Badge variant='outline'>{sub.planName}</Badge>
													</TableCell>
													<TableCell>{getStatusBadge(sub.status)}</TableCell>
													<TableCell className='text-sm'>
														{new Date(sub.startDate).toLocaleDateString()}
													</TableCell>
													<TableCell className='text-sm'>
														{new Date(sub.endDate).toLocaleDateString()}
													</TableCell>
													<TableCell
														className='text-right'
														onClick={e => e.stopPropagation()}
													>
														<DropdownMenu>
															<DropdownMenuTrigger asChild>
																<Button
																	variant='ghost'
																	size='icon'
																	disabled={actionLoading === sub.id}
																>
																	{actionLoading === sub.id ? (
																		<Loader2 className='h-4 w-4 animate-spin' />
																	) : (
																		<MoreVertical className='h-4 w-4' />
																	)}
																</Button>
															</DropdownMenuTrigger>
															<DropdownMenuContent align='end'>
																{sub.isActive && (
																	<DropdownMenuItem
																		onClick={() => handleSuspend(sub)}
																	>
																		<Pause className='h-4 w-4 mr-2' />
																		{t('admin.subscriptions.actions.suspend')}
																	</DropdownMenuItem>
																)}
																{(sub.status === 'Suspended' ||
																	sub.status === 'Expired') && (
																	<DropdownMenuItem
																		onClick={() => handleActivate(sub)}
																	>
																		<Play className='h-4 w-4 mr-2' />
																		{t('admin.subscriptions.actions.activate')}
																	</DropdownMenuItem>
																)}
																<DropdownMenuSeparator />
																<DropdownMenuItem>
																	<RefreshCw className='h-4 w-4 mr-2' />
																	{t('admin.subscriptions.actions.renew')}
																</DropdownMenuItem>
															</DropdownMenuContent>
														</DropdownMenu>
													</TableCell>
												</TableRow>
											))
										) : (
											<TableRow>
												<TableCell
													colSpan={6}
													className='text-center py-8 text-muted-foreground'
												>
													{t('admin.subscriptions.no_subscriptions')}
												</TableCell>
											</TableRow>
										)}
									</TableBody>
								</Table>
							)}
						</CardContent>
					</Card>
				</TabsContent>

				{/* Tiers Tab */}
				<TabsContent value='tiers'>
					<Card>
						<CardHeader>
							<CardTitle>
								{t('admin.subscriptions.subscription_tiers')}
							</CardTitle>
						</CardHeader>
						<CardContent>
							{tiersLoading ? (
								<div className='space-y-3'>
									{[1, 2, 3].map(i => (
										<Skeleton key={i} className='h-24' />
									))}
								</div>
							) : tiers && tiers.length > 0 ? (
								<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-3'>
									{tiers.map(tier => (
										<Card
											key={tier.id}
											className={!tier.isActive ? 'opacity-60' : undefined}
										>
											<CardHeader className='pb-3'>
												<div className='flex items-center justify-between'>
													<CardTitle className='text-lg'>{tier.name}</CardTitle>
													{tier.isActive ? (
														<Badge className='bg-green-600'>
															{t('common.active')}
														</Badge>
													) : (
														<Badge variant='secondary'>
															{t('common.inactive')}
														</Badge>
													)}
												</div>
											</CardHeader>
											<CardContent className='space-y-3'>
												<p className='text-sm text-muted-foreground'>
													{tier.description}
												</p>
												<div className='flex items-baseline gap-1'>
													<span className='text-3xl font-bold'>
														{tier.currency} {tier.price.toLocaleString()}
													</span>
													<span className='text-sm text-muted-foreground'>
														/{tier.durationInDays} {t('common.days')}
													</span>
												</div>
												<div className='flex items-center justify-between text-sm'>
													<span className='text-muted-foreground'>
														{t('admin.subscriptions.target_role')}:
													</span>
													<Badge variant='outline'>{tier.targetRole}</Badge>
												</div>
												<div className='flex gap-2 pt-2'>
													<Button
														size='sm'
														variant={tier.isActive ? 'destructive' : 'default'}
														onClick={() => handleToggleTier(tier)}
														disabled={actionLoading === tier.id}
													>
														{actionLoading === tier.id ? (
															<Loader2 className='h-4 w-4 animate-spin mr-1' />
														) : tier.isActive ? (
															<XCircle className='h-4 w-4 mr-1' />
														) : (
															<CheckCircle2 className='h-4 w-4 mr-1' />
														)}
														{tier.isActive
															? t('admin.subscriptions.actions.deactivate')
															: t('admin.subscriptions.actions.activate')}
													</Button>
												</div>
											</CardContent>
										</Card>
									))}
								</div>
							) : (
								<p className='text-center py-8 text-muted-foreground'>
									{t('admin.subscriptions.no_tiers')}
								</p>
							)}
						</CardContent>
					</Card>
				</TabsContent>

				{/* Transactions Tab */}
				<TabsContent value='transactions'>
					<Card>
						<CardHeader>
							<CardTitle>{t('admin.subscriptions.all_transactions')}</CardTitle>
						</CardHeader>
						<CardContent>
							{txLoading ? (
								<div className='space-y-3'>
									{[1, 2, 3, 4, 5].map(i => (
										<Skeleton key={i} className='h-12' />
									))}
								</div>
							) : (
								<Table>
									<TableHeader>
										<TableRow>
											<TableHead>
												{t('admin.subscriptions.tx_table.code')}
											</TableHead>
											<TableHead>
												{t('admin.subscriptions.tx_table.amount')}
											</TableHead>
											<TableHead>
												{t('admin.subscriptions.tx_table.provider')}
											</TableHead>
											<TableHead>
												{t('admin.subscriptions.tx_table.status')}
											</TableHead>
											<TableHead>
												{t('admin.subscriptions.tx_table.plan')}
											</TableHead>
											<TableHead>
												{t('admin.subscriptions.tx_table.date')}
											</TableHead>
										</TableRow>
									</TableHeader>
									<TableBody>
										{transactions && transactions.length > 0 ? (
											transactions.map(tx => (
												<TableRow key={tx.id}>
													<TableCell className='font-mono text-xs'>
														{tx.transactionCode}
													</TableCell>
													<TableCell className='font-medium'>
														{tx.currency} {tx.amount.toLocaleString()}
													</TableCell>
													<TableCell>
														<Badge variant='outline'>
															{tx.paymentProvider}
														</Badge>
													</TableCell>
													<TableCell>
														{getPaymentStatusBadge(tx.paymentStatus)}
													</TableCell>
													<TableCell>
														{tx.subscriptionPlanName || '-'}
													</TableCell>
													<TableCell className='text-sm'>
														{new Date(tx.createdAt).toLocaleDateString()}
													</TableCell>
												</TableRow>
											))
										) : (
											<TableRow>
												<TableCell
													colSpan={6}
													className='text-center py-8 text-muted-foreground'
												>
													{t('admin.subscriptions.no_transactions')}
												</TableCell>
											</TableRow>
										)}
									</TableBody>
								</Table>
							)}
						</CardContent>
					</Card>
				</TabsContent>
			</Tabs>

			{/* Confirm Dialog */}
			<Dialog
				open={confirmDialog.open}
				onOpenChange={open => setConfirmDialog(prev => ({ ...prev, open }))}
			>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>{confirmDialog.title}</DialogTitle>
						<DialogDescription>{confirmDialog.description}</DialogDescription>
					</DialogHeader>
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() =>
								setConfirmDialog(prev => ({
									...prev,
									open: false,
								}))
							}
						>
							{t('common.cancel')}
						</Button>
						<Button
							onClick={async () => {
								await confirmDialog.action()
								setConfirmDialog(prev => ({
									...prev,
									open: false,
								}))
							}}
						>
							{t('common.confirm')}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Subscription Detail Sheet */}
			<SubscriptionDetailSheet
				sub={selectedSub}
				user={selectedSub ? userMap[selectedSub.userId] : undefined}
				open={sheetOpen}
				onOpenChange={setSheetOpen}
			/>
		</div>
	)
}

export default SubscriptionsPage
