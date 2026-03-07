export interface AdminSubscriptionDto {
	id: string
	userId: string
	subscriptionTierId: string
	planName: string
	startDate: string
	endDate: string
	status: string
	isActive: boolean
	isExpired: boolean
	expiresAtTimestamp: number
}

export interface AdminTransactionDto {
	id: string
	transactionCode: string
	amount: number
	currency: string
	paymentProvider: string
	paymentStatus: string
	providerTransactionId: string | null
	createdAt: string
	userSubscriptionId: string
	subscriptionPlanName: string | null
}
