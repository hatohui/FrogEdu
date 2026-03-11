import type {
	SubscriptionTier,
	UserSubscription,
	Transaction,
} from '@/types/model/subscription'
import type {
	SubscribeToProResponse,
	CancelSubscriptionResponse,
	AIUsageRecord,
	AIUsageLimitInfo,
	AdminSubscriptionDto,
	AdminTransactionDto,
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

	// ==================== AI Usage ====================

	/**
	 * Get current user's AI usage limit info
	 */
	async getAIUsageLimit(): Promise<AIUsageLimitInfo> {
		const response = await axiosInstance.get<AIUsageLimitInfo>(
			`${this.baseUrl}/ai-usage/limit`
		)
		return response.data
	}

	/**
	 * Get AI usage limit info for a specific user (admin/internal use)
	 */
	async getAIUsageLimitForUser(userId: string): Promise<AIUsageLimitInfo> {
		const response = await axiosInstance.get<AIUsageLimitInfo>(
			`${this.baseUrl}/ai-usage/check/${userId}`
		)
		return response.data
	}

	/**
	 * Get current user's AI usage history
	 */
	async getAIUsageHistory(): Promise<AIUsageRecord[]> {
		const response = await axiosInstance.get<AIUsageRecord[]>(
			`${this.baseUrl}/ai-usage/history`
		)
		return response.data
	}

	/**
	 * Record AI usage
	 */
	async recordAIUsage(
		actionType: string,
		metadata?: string
	): Promise<{ recordId: string; message: string }> {
		const response = await axiosInstance.post<{
			recordId: string
			message: string
		}>(`${this.baseUrl}/ai-usage/record`, { actionType, metadata })
		return response.data
	}

	// ==================== Admin ====================

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

	/**
	 * Get all subscriptions (Admin only)
	 */
	async getAllSubscriptions(status?: string): Promise<AdminSubscriptionDto[]> {
		const params = status ? { status } : {}
		const response = await axiosInstance.get<AdminSubscriptionDto[]>(
			`${this.baseUrl}/admin/subscriptions`,
			{ params }
		)
		return response.data
	}

	/**
	 * Get all tiers including inactive (Admin only)
	 */
	async getAllTiers(includeInactive = true): Promise<SubscriptionTier[]> {
		const response = await axiosInstance.get<SubscriptionTier[]>(
			`${this.baseUrl}/admin/tiers`,
			{ params: { includeInactive } }
		)
		return response.data
	}

	/**
	 * Create subscription tier (Admin only)
	 */
	async createTier(data: {
		name: string
		description: string
		price: number
		currency: string
		durationInDays: number
		targetRole: string
		imageUrl?: string
	}): Promise<{ id: string }> {
		const response = await axiosInstance.post<{ id: string }>(
			`${this.baseUrl}/admin/tiers`,
			data
		)
		return response.data
	}

	/**
	 * Update subscription tier (Admin only)
	 */
	async updateTier(
		id: string,
		data: {
			name: string
			description: string
			price: number
			currency: string
			durationInDays: number
			imageUrl?: string
		}
	): Promise<void> {
		await axiosInstance.put(`${this.baseUrl}/admin/tiers/${id}`, data)
	}

	/**
	 * Activate/Deactivate tier (Admin only)
	 */
	async activateTier(id: string): Promise<void> {
		await axiosInstance.post(`${this.baseUrl}/admin/tiers/${id}/activate`)
	}

	async deactivateTier(id: string): Promise<void> {
		await axiosInstance.post(`${this.baseUrl}/admin/tiers/${id}/deactivate`)
	}

	/**
	 * Delete tier (Admin only)
	 */
	async deleteTier(id: string): Promise<void> {
		await axiosInstance.delete(`${this.baseUrl}/admin/tiers/${id}`)
	}

	/**
	 * Suspend/Activate/Renew subscription (Admin only)
	 */
	async suspendSubscription(id: string): Promise<void> {
		await axiosInstance.post(
			`${this.baseUrl}/admin/subscriptions/${id}/suspend`
		)
	}

	async activateSubscription(id: string): Promise<void> {
		await axiosInstance.post(
			`${this.baseUrl}/admin/subscriptions/${id}/activate`
		)
	}

	async renewSubscription(id: string, newEndDate: string): Promise<void> {
		await axiosInstance.post(
			`${this.baseUrl}/admin/subscriptions/${id}/renew`,
			{ newEndDate }
		)
	}

	/**
	 * Get all transactions (Admin only)
	 */
	async getAllTransactions(
		paymentStatus?: string,
		paymentProvider?: string
	): Promise<AdminTransactionDto[]> {
		const params: Record<string, string> = {}
		if (paymentStatus) params.paymentStatus = paymentStatus
		if (paymentProvider) params.paymentProvider = paymentProvider
		const response = await axiosInstance.get<AdminTransactionDto[]>(
			`${this.baseUrl}/admin/transactions`,
			{ params }
		)
		return response.data
	}
}

const subscriptionService = new SubscriptionService()
export default subscriptionService
