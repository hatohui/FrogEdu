export interface MonthlyRevenueItem {
	month: string
	revenue: number
	transactionCount: number
}

export interface SubscriptionStatusItem {
	status: string
	count: number
	percentage: number
}

export interface SubscriptionDashboardStatsResponse {
	totalRevenue: number
	totalSubscriptions: number
	activeSubscriptions: number
	expiredSubscriptions: number
	cancelledSubscriptions: number
	suspendedSubscriptions: number
	monthlyRevenue: MonthlyRevenueItem[]
	statusDistribution: SubscriptionStatusItem[]
}
