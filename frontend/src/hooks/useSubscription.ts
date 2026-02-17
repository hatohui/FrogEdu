import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import subscriptionService from '@/services/subscription.service'
import { toast } from 'sonner'
import { useAuthStore } from '@/stores/authStore'

export const useSubscription = () => {
	const queryClient = useQueryClient()
	const isAuthenticated = useAuthStore(state => state.isAuthenticated)

	// Query for user's current subscription
	const {
		data: subscription,
		isLoading: isLoadingSubscription,
		error: subscriptionError,
		refetch: refetchSubscription,
	} = useQuery({
		queryKey: ['subscription', 'me'],
		queryFn: () => subscriptionService.getMySubscription(),
		staleTime: 5 * 60 * 1000, // 5 minutes
		retry: 1,
		enabled: isAuthenticated,
	})

	// Query for available subscription tiers
	const {
		data: tiers,
		isLoading: isLoadingTiers,
		error: tiersError,
	} = useQuery({
		queryKey: ['subscription', 'tiers'],
		queryFn: () => subscriptionService.getSubscriptionTiers(),
		staleTime: 30 * 60 * 1000, // 30 minutes
		enabled: isAuthenticated,
	})

	// Mutation for subscribing to Pro
	const subscribeToProMutation = useMutation({
		mutationFn: () => subscriptionService.subscribeToPro(),
		onSuccess: data => {
			toast.success(data.message || 'Successfully subscribed to Pro!')
			// Invalidate subscription query to refetch
			queryClient.invalidateQueries({ queryKey: ['subscription', 'me'] })
			// Invalidate transactions to show new transaction
			queryClient.invalidateQueries({ queryKey: ['transactions', 'me'] })
			// Also invalidate user query to update subscription info
			queryClient.invalidateQueries({ queryKey: ['currentUser'] })
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to subscribe to Pro')
		},
	})

	// Mutation for cancelling subscription
	const cancelSubscriptionMutation = useMutation({
		mutationFn: () => subscriptionService.cancelSubscription(),
		onSuccess: data => {
			toast.success(data.message || 'Subscription cancelled successfully')
			// Invalidate subscription query to refetch
			queryClient.invalidateQueries({ queryKey: ['subscription', 'me'] })
			// Also invalidate user query to update subscription info
			queryClient.invalidateQueries({ queryKey: ['currentUser'] })
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to cancel subscription')
		},
	})

	const subscribeToPro = async () => {
		return subscribeToProMutation.mutateAsync()
	}

	const cancelSubscription = async () => {
		return cancelSubscriptionMutation.mutateAsync()
	}

	const isPro =
		subscription?.isActive && subscription?.planName?.toLowerCase() === 'pro'
	const isFree =
		!subscription?.isActive || subscription?.planName?.toLowerCase() === 'free'
	const isCancelled = subscription?.status?.toLowerCase() === 'cancelled'

	return {
		subscription,
		tiers,
		isLoading: isLoadingSubscription || isLoadingTiers,
		isLoadingSubscription,
		isLoadingTiers,
		subscriptionError,
		tiersError,
		subscribeToPro,
		isSubscribing: subscribeToProMutation.isPending,
		cancelSubscription,
		isCancelling: cancelSubscriptionMutation.isPending,
		isPro,
		isFree,
		isCancelled,
		refetchSubscription,
	}
}

/**
 * Hook for fetching user's transaction history
 */
export const useTransactions = () => {
	const isAuthenticated = useAuthStore(state => state.isAuthenticated)

	const {
		data: transactions,
		isLoading,
		error,
		refetch,
	} = useQuery({
		queryKey: ['transactions', 'me'],
		queryFn: () => subscriptionService.getMyTransactions(),
		staleTime: 5 * 60 * 1000, // 5 minutes
		retry: 1,
		enabled: isAuthenticated,
	})

	return {
		transactions: transactions ?? [],
		isLoading,
		error,
		refetch,
	}
}

/**
 * Hook for fetching subscription dashboard statistics (Admin only)
 */
export const useSubscriptionDashboardStats = () => {
	return useQuery({
		queryKey: ['subscription', 'dashboard-stats'],
		queryFn: () => subscriptionService.getDashboardStats(),
		staleTime: 5 * 60 * 1000,
	})
}
