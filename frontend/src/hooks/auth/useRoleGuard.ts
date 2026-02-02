import { useEffect } from 'react'
import { useNavigate, useLocation } from 'react-router'
import { useMe } from './useMe'

/**
 * Hook to guard routes that require a user to have a role selected
 * Redirects to /select-role if authenticated user has no role
 */
export const useRoleGuard = () => {
	const navigate = useNavigate()
	const location = useLocation()
	const { user, isLoading, isAuthenticated } = useMe()

	useEffect(() => {
		// Don't check on auth-related pages
		const isAuthPage =
			location.pathname === '/select-role' ||
			location.pathname === '/login' ||
			location.pathname === '/register' ||
			location.pathname.startsWith('/confirm-email') ||
			location.pathname.startsWith('/auth/callback') ||
			location.pathname === '/'

		if (isAuthPage || isLoading || !isAuthenticated) {
			return
		}

		// If user is authenticated but has no role, redirect to select-role
		if (user && !user.roleId) {
			console.log('User has no role, redirecting to role selection')
			navigate('/select-role', { replace: true })
		}
	}, [user, isLoading, isAuthenticated, location.pathname, navigate])

	return { user, isLoading, isAuthenticated }
}
