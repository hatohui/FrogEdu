import type {
	SubscriptionTier,
	UserSubscription,
} from '@/types/model/subscription'
import type { SubscribeToProResponse } from '@/types/dtos/subscriptions'
import axiosInstance from './axios'

class SubscriptionService {
	private readonly baseUrl = '/subscriptions'

	/**
	 * Get all available subscription tiers
	 */
	async getSubscriptionTiers(): Promise<SubscriptionTier[]> {
		const response = await axiosInstance.get<SubscriptionTier[]>(
			`${this.baseUrl}/tiers`
		)
		return response.data
	}

	/**
	 * Get current user's subscription
	 */
	async getMySubscription(): Promise<UserSubscription> {
		const response = await axiosInstance.get<UserSubscription>(
			`${this.baseUrl}/me`
		)
		return response.data
	}

	/**
	 * Subscribe to Pro tier (mock - no payment required)
	 */
	async subscribeToPro(): Promise<SubscribeToProResponse> {
		const response = await axiosInstance.post<SubscribeToProResponse>(
			`${this.baseUrl}/subscribe/pro`
		)
		return response.data
	}
}

const subscriptionService = new SubscriptionService()
export default subscriptionService
