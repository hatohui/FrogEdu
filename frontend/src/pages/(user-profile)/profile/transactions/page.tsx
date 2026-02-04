import React from 'react'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Skeleton } from '@/components/ui/skeleton'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import {
	Receipt,
	CheckCircle,
	XCircle,
	Clock,
	AlertCircle,
	RefreshCw,
	CreditCard,
} from 'lucide-react'
import { useTransactions } from '@/hooks/useSubscription'
import type { Transaction } from '@/types/model/subscription'

const TransactionsPage = (): React.ReactElement => {
	const { transactions, isLoading, error } = useTransactions()

	const formatDate = (dateString: string) => {
		return new Date(dateString).toLocaleDateString('vi-VN', {
			year: 'numeric',
			month: 'long',
			day: 'numeric',
			hour: '2-digit',
			minute: '2-digit',
		})
	}

	const formatAmount = (amount: number, currency: string) => {
		return new Intl.NumberFormat('vi-VN', {
			style: 'currency',
			currency: currency,
		}).format(amount)
	}

	const getStatusBadge = (status: string) => {
		const statusLower = status.toLowerCase()
		switch (statusLower) {
			case 'paid':
				return (
					<Badge className='bg-green-600'>
						<CheckCircle className='h-3 w-3 mr-1' />
						Paid
					</Badge>
				)
			case 'pending':
				return (
					<Badge variant='secondary'>
						<Clock className='h-3 w-3 mr-1' />
						Pending
					</Badge>
				)
			case 'failed':
				return (
					<Badge variant='destructive'>
						<XCircle className='h-3 w-3 mr-1' />
						Failed
					</Badge>
				)
			case 'cancelled':
				return (
					<Badge variant='outline'>
						<AlertCircle className='h-3 w-3 mr-1' />
						Cancelled
					</Badge>
				)
			case 'refunded':
				return (
					<Badge variant='outline' className='border-blue-500 text-blue-600'>
						<RefreshCw className='h-3 w-3 mr-1' />
						Refunded
					</Badge>
				)
			default:
				return <Badge variant='outline'>{status}</Badge>
		}
	}

	const getProviderIcon = (_provider: string) => {
		// Could add specific icons for Momo, VNPay, etc.
		return <CreditCard className='h-4 w-4' />
	}

	if (isLoading) {
		return (
			<div className='space-y-6 max-w-4xl mx-auto'>
				<Skeleton className='h-10 w-64' />
				<Skeleton className='h-[400px]' />
			</div>
		)
	}

	if (error) {
		return (
			<div className='space-y-6 max-w-4xl mx-auto'>
				<Card className='border-destructive'>
					<CardContent className='p-6 text-center'>
						<AlertCircle className='h-12 w-12 mx-auto text-destructive mb-4' />
						<h3 className='text-lg font-semibold'>
							Failed to load transactions
						</h3>
						<p className='text-muted-foreground'>
							Please try again later or contact support.
						</p>
					</CardContent>
				</Card>
			</div>
		)
	}

	return (
		<div className='space-y-6 max-w-4xl mx-auto'>
			{/* Header */}
			<div>
				<h1 className='text-3xl font-bold flex items-center gap-2'>
					<Receipt className='h-8 w-8' />
					Transaction History
				</h1>
				<p className='text-muted-foreground mt-1'>
					View all your payment transactions and subscription history
				</p>
			</div>

			{/* Transactions Table */}
			<Card>
				<CardHeader>
					<CardTitle>All Transactions</CardTitle>
					<CardDescription>
						{transactions.length === 0
							? 'No transactions found'
							: `Showing ${transactions.length} transaction${transactions.length === 1 ? '' : 's'}`}
					</CardDescription>
				</CardHeader>
				<CardContent>
					{transactions.length === 0 ? (
						<div className='text-center py-12'>
							<Receipt className='h-16 w-16 mx-auto text-muted-foreground/50 mb-4' />
							<h3 className='text-lg font-medium mb-2'>No transactions yet</h3>
							<p className='text-muted-foreground'>
								When you subscribe to a plan, your transactions will appear
								here.
							</p>
						</div>
					) : (
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead>Date</TableHead>
									<TableHead>Transaction Code</TableHead>
									<TableHead>Plan</TableHead>
									<TableHead>Provider</TableHead>
									<TableHead>Amount</TableHead>
									<TableHead>Status</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{transactions.map((transaction: Transaction) => (
									<TableRow key={transaction.id}>
										<TableCell className='whitespace-nowrap'>
											{formatDate(transaction.createdAt)}
										</TableCell>
										<TableCell>
											<code className='text-xs bg-muted px-2 py-1 rounded'>
												{transaction.transactionCode}
											</code>
										</TableCell>
										<TableCell>
											<Badge variant='outline'>
												{transaction.subscriptionPlanName || 'Unknown'}
											</Badge>
										</TableCell>
										<TableCell>
											<div className='flex items-center gap-2'>
												{getProviderIcon(transaction.paymentProvider)}
												<span>{transaction.paymentProvider}</span>
											</div>
										</TableCell>
										<TableCell className='font-medium'>
											{formatAmount(transaction.amount, transaction.currency)}
										</TableCell>
										<TableCell>
											{getStatusBadge(transaction.paymentStatus)}
										</TableCell>
									</TableRow>
								))}
							</TableBody>
						</Table>
					)}
				</CardContent>
			</Card>

			{/* Info Note */}
			<Card className='bg-blue-50 dark:bg-blue-950/20 border-blue-200 dark:border-blue-800'>
				<CardContent className='p-4'>
					<div className='flex items-start gap-3'>
						<AlertCircle className='h-5 w-5 text-blue-600 mt-0.5' />
						<div>
							<p className='font-medium text-blue-900 dark:text-blue-100'>
								Need help with a transaction?
							</p>
							<p className='text-sm text-blue-800 dark:text-blue-200 mt-1'>
								If you have any questions about your transactions or need a
								refund, please contact our support team at{' '}
								<a
									href='mailto:support@frogedu.vn'
									className='underline font-medium'
								>
									support@frogedu.vn
								</a>
							</p>
						</div>
					</div>
				</CardContent>
			</Card>
		</div>
	)
}

export default TransactionsPage
