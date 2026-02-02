import userService from '@/services/user.service'
import { useAuthStore } from '@/stores/authStore'
import { useQuery } from '@tanstack/react-query'
import { useNavigate } from 'react-router'
import type { GetMeResponse } from '@/types/dtos/users/user'
import type { Role } from '@/types/dtos/users/role'

interface UserWithRole extends GetMeResponse {
	role?: Role
}

export const useMe = () => {
	const isAuthenticated = useAuthStore(state => state.isAuthenticated)
	const signOut = useAuthStore(state => state.signOut)
	const refreshAuth = useAuthStore(state => state.refreshAuth)

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
		enabled: isAuthenticated,
	})

	const { data: role, isLoading: isLoadingRole } = useQuery({
		queryKey: ['role', userData?.roleId],
		queryFn: () => userService.getRoleById(userData!.roleId),
		staleTime: 10 * 60 * 1000,
		retry: 1,
		enabled: !!userData?.roleId,
	})

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
		user,
		isLoading,
		isFetching,
		error,
		isError,
		signOutThenNavigate,
		signOut,
		isAuthenticated,
		refreshAuth,
	}
}
