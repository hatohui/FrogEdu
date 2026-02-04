import React from 'react'
import { useNavigate, useLocation } from 'react-router'
import { useMe } from '@/hooks/auth/useMe'
import { useSubscription } from '@/hooks/useSubscription'

/**
 * RoleGuard component checks if authenticated user has a role set
 * If not, redirects them to the select-role page
 * After role selection, redirects to subscription selection if no subscription
 */
export const RoleGuard = ({
	children,
}: {
	children: React.ReactNode
}): React.ReactElement => {
	const navigate = useNavigate()
	const location = useLocation()
	const { user, isAuthenticated, isLoading, isError } = useMe()
	const { subscription, isLoadingSubscription } = useSubscription()

	React.useEffect(() => {
		// Skip if still loading or on auth/select-role/select-subscription pages
		if (
			isLoading ||
			isLoadingSubscription ||
			location.pathname.startsWith('/login') ||
			location.pathname.startsWith('/register') ||
			location.pathname.startsWith('/select-role') ||
			location.pathname.startsWith('/select-subscription') ||
			location.pathname.startsWith('/forgot-password') ||
			location.pathname.startsWith('/confirm-email') ||
			location.pathname.startsWith('/auth/callback') ||
			location.pathname.startsWith('/about') ||
			location.pathname === '/'
		) {
			return
		}

		// If authenticated but user doesn't exist in backend or has no role
		if (isAuthenticated && !isLoading) {
			// User doesn't exist in backend (404 error)
			if (isError && !user) {
				console.log(
					'RoleGuard: User not found in backend, redirecting to select-role'
				)
				navigate('/select-role', { replace: true })
				return
			}

			// User exists but has no role
			if (user && !user.roleId) {
				console.log('RoleGuard: User has no role, redirecting to select-role')
				navigate('/select-role', { replace: true })
				return
			}

			// User has role but check if they've been shown subscription page
			// Only show subscription page if they haven't explicitly chosen yet
			// We track this by checking if subscription status is 'None' (never subscribed)
			if (user && user.roleId && !isLoadingSubscription) {
				const hasSeenSubscriptionPage = sessionStorage.getItem(
					'hasSeenSubscriptionPage'
				)
				if (
					!hasSeenSubscriptionPage &&
					subscription?.status === 'None' &&
					!subscription?.isActive
				) {
					console.log(
						'RoleGuard: User has no subscription, redirecting to select-subscription'
					)
					sessionStorage.setItem('hasSeenSubscriptionPage', 'true')
					navigate('/select-subscription', { replace: true })
				}
			}
		}
	}, [
		isAuthenticated,
		user,
		isLoading,
		isError,
		isLoadingSubscription,
		subscription,
		location.pathname,
		navigate,
	])

	return <>{children}</>
}
