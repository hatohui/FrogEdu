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
	const { user, isAuthenticated, isLoading } = useMe()

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

		// If authenticated but no role, redirect to role selection
		if (isAuthenticated && user && !user.roleId) {
			console.log('RoleGuard: User has no role, redirecting to select-role')
			navigate('/select-role', { replace: true })
		}
	}, [isAuthenticated, user, isLoading, location.pathname, navigate])

	return <>{children}</>
}
