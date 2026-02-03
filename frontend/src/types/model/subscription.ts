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

// Re-export response types for convenience
export type { SubscribeToProResponse } from '@/types/dtos/subscriptions'
