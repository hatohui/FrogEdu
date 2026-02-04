/**
 * Subscription response types (DTOs)
 */

/**
 * Response from subscribing to Pro plan
 */
export interface SubscribeToProResponse {
	subscriptionId: string
	message: string
}

/**
 * Response from cancelling a subscription
 */
export interface CancelSubscriptionResponse {
	subscriptionId: string
	message: string
}
