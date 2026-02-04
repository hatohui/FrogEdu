export interface SubscriptionTier {
	id: string
	name: string
	imageUrl: string | null
	description: string
	price: number
	currency: string
	durationInDays: number
	targetRole: string
	isActive: boolean
}

export interface UserSubscription {
	id: string
	userId: string
	planName: string
	startDate: string
	endDate: string
	status: string
	isActive: boolean
	isExpired: boolean
	expiresAtTimestamp: number
}

export interface SubscriptionInfo {
	plan: string
	expiresAt: number
	hasActiveSubscription: boolean
}

export interface Transaction {
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

// Re-export response types for convenience
export type {
	SubscribeToProResponse,
	CancelSubscriptionResponse,
} from '@/types/dtos/subscriptions'
