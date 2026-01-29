import React from 'react'
import { useNavigate, useLocation } from 'react-router'
import { useAuthStore } from '@/stores/authStore'

/**
 * RoleGuard component checks if authenticated user has a role set
 * If not, redirects them to the select-role page
 */
export const RoleGuard = ({
	children,
}: {
	children: React.ReactNode
}): React.ReactElement => {
	const navigate = useNavigate()
	const location = useLocation()
	const isAuthenticated = useAuthStore(state => state.isAuthenticated)
	const userProfile = useAuthStore(state => state.userProfile)
	const isLoading = useAuthStore(state => state.isLoading)

	React.useEffect(() => {
		// Skip if still loading or on auth/select-role pages
		if (
			isLoading ||
			location.pathname.startsWith('/login') ||
			location.pathname.startsWith('/register') ||
			location.pathname.startsWith('/select-role') ||
			location.pathname.startsWith('/forgot-password') ||
			location.pathname === '/'
		) {
			return
		}

		// If authenticated but no role, redirect to role selection
		if (isAuthenticated && userProfile && !userProfile['custom:role']) {
			navigate('/select-role', { replace: true })
		}
	}, [isAuthenticated, userProfile, isLoading, location.pathname, navigate])

	return <>{children}</>
}
