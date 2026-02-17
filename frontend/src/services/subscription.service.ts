import type {
	SubscriptionTier,
	UserSubscription,
	Transaction,
} from '@/types/model/subscription'
import type {
	SubscribeToProResponse,
	CancelSubscriptionResponse,
} from '@/types/dtos/subscriptions'
import type { SubscriptionDashboardStatsResponse } from '@/types/dtos/subscriptions/dashboard'
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
	 * Get current user's transaction history
	 */
	async getMyTransactions(): Promise<Transaction[]> {
		const response = await axiosInstance.get<Transaction[]>(
			`${this.baseUrl}/transactions/me`
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

	/**
	 * Cancel current subscription
	 */
	async cancelSubscription(): Promise<CancelSubscriptionResponse> {
		const response = await axiosInstance.post<CancelSubscriptionResponse>(
			`${this.baseUrl}/cancel`
		)
		return response.data
	}

	/**
	 * Get subscription dashboard statistics (Admin only)
	 */
	async getDashboardStats(): Promise<SubscriptionDashboardStatsResponse> {
		const response =
			await axiosInstance.get<SubscriptionDashboardStatsResponse>(
				`${this.baseUrl}/admin/dashboard-stats`
			)
		return response.data
	}
}

const subscriptionService = new SubscriptionService()
export default subscriptionService
