import React from 'react'
import { useNavigate, useLocation } from 'react-router'
import { useMe } from '@/hooks/auth/useMe'

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
	const { user, isAuthenticated, isLoading, isError } = useMe()

	React.useEffect(() => {
		// Skip if still loading or on auth/select-role pages
		if (
			isLoading ||
			location.pathname.startsWith('/login') ||
			location.pathname.startsWith('/register') ||
			location.pathname.startsWith('/select-role') ||
			location.pathname.startsWith('/forgot-password') ||
			location.pathname.startsWith('/confirm-email') ||
			location.pathname.startsWith('/auth/callback') ||
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
			}
		}
	}, [isAuthenticated, user, isLoading, isError, location.pathname, navigate])

	return <>{children}</>
}
