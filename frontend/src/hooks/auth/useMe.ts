import userService from '@/services/user-microservice/user.service'
import { useAuthStore } from '@/stores/authStore'
import { useQuery, useQueryClient } from '@tanstack/react-query'
import { useNavigate, useLocation } from 'react-router'
import { useEffect, useRef } from 'react'
import type { UserWithRole } from '@/types/model/user-service/user'

// Re-export for backward compatibility
export type { UserWithRole } from '@/types/model/user-service/user'

export const useMe = () => {
	const isAuthenticated = useAuthStore(state => state.isAuthenticated)
	const signOut = useAuthStore(state => state.signOut)
	const refreshAuth = useAuthStore(state => state.refreshAuth)
	const syncRoleAndRefreshToken = useAuthStore(
		state => state.syncRoleAndRefreshToken
	)
	const location = useLocation()
	const queryClient = useQueryClient()
	const hasSyncedRef = useRef(false)

	const shouldQuery =
		isAuthenticated &&
		!location.pathname.startsWith('/select-role') &&
		!location.pathname.startsWith('/auth/callback')

	const {
		data: userData,
		isLoading: isLoadingUser,
		isFetching,
		error,
		isError,
	} = useQuery({
		queryKey: ['currentUser'],
		queryFn: () => userService.getCurrentUser(),
		staleTime: 5 * 60 * 1000,
		retry: 1,
		enabled: shouldQuery,
	})

	const { data: role, isLoading: isLoadingRole } = useQuery({
		queryKey: ['role', userData?.roleId],
		queryFn: () => userService.getRoleById(userData!.roleId),
		staleTime: 10 * 60 * 1000,
		retry: 1,
		enabled: !!userData?.roleId,
	})

	// Auto-sync role on first load (self-healing)
	useEffect(() => {
		if (
			shouldQuery &&
			!hasSyncedRef.current &&
			!isLoadingUser &&
			!isLoadingRole
		) {
			hasSyncedRef.current = true
			// Trigger role sync in background - this will:
			// 1. Call GET /me to sync role to Cognito
			// 2. Force JWT token refresh
			// 3. Invalidate queries to refetch with new role
			syncRoleAndRefreshToken().then(newRole => {
				if (newRole) {
					// Invalidate queries to refetch user data with refreshed token
					queryClient.invalidateQueries({ queryKey: ['currentUser'] })
					console.log('🔄 Auto-synced role and refreshed token')
				}
			})
		}
	}, [
		shouldQuery,
		isLoadingUser,
		isLoadingRole,
		syncRoleAndRefreshToken,
		queryClient,
	])

	const user: UserWithRole | undefined = userData
		? { ...userData, role }
		: undefined

	const isLoading = isLoadingUser || isLoadingRole

	const navigate = useNavigate()

	const signOutThenNavigate = async (to: string) => {
		await signOut()
		navigate(to)
	}

	return {
		role,
		user,
		isLoading,
		isFetching,
		error,
		isError,
		signOutThenNavigate,
		signOut,
		isAuthenticated,
		refreshAuth,
		syncRoleAndRefreshToken,
	}
}
