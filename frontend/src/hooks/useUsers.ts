import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import userService from '@/services/user-microservice/user.service'
import type {
	GetAllUsersParams,
	UpdateUserRequest,
} from '@/types/dtos/users/admin'
import { toast } from 'sonner'

// Query keys factory
export const userKeys = {
	all: ['users'] as const,
	lists: () => [...userKeys.all, 'list'] as const,
	list: (params: GetAllUsersParams) => [...userKeys.lists(), params] as const,
	statistics: () => [...userKeys.all, 'statistics'] as const,
	dashboardStats: () => [...userKeys.all, 'dashboard-stats'] as const,
	details: () => [...userKeys.all, 'detail'] as const,
	detail: (id: string) => [...userKeys.details(), id] as const,
	roles: () => ['roles'] as const,
}

// Get all roles
export function useRoles() {
	return useQuery({
		queryKey: userKeys.roles(),
		queryFn: () => userService.getRoles(),
		staleTime: Infinity, // Roles rarely change, cache indefinitely
	})
}

// Get all users with pagination
export function useUsers(params: GetAllUsersParams = {}) {
	return useQuery({
		queryKey: userKeys.list(params),
		queryFn: () => userService.getAllUsers(params),
	})
}

// Get user statistics
export function useUserStatistics() {
	return useQuery({
		queryKey: userKeys.statistics(),
		queryFn: () => userService.getUserStatistics(),
	})
}

// Get user dashboard stats (growth chart, role distribution, verification status)
export function useUserDashboardStats() {
	return useQuery({
		queryKey: userKeys.dashboardStats(),
		queryFn: () => userService.getUserDashboardStats(),
		staleTime: 5 * 60 * 1000,
	})
}

// Get user by ID
export function useUser(userId: string) {
	return useQuery({
		queryKey: userKeys.detail(userId),
		queryFn: () => userService.getUserById(userId),
		enabled: !!userId,
	})
}

// Update user mutation
export function useUpdateUser() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			userId,
			updates,
		}: {
			userId: string
			updates: UpdateUserRequest
		}) => userService.updateUser(userId, updates),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: userKeys.lists() })
			queryClient.invalidateQueries({ queryKey: userKeys.statistics() })
			toast.success('User updated successfully')
		},
		onError: error => {
			console.error('Failed to update user:', error)
			toast.error('Failed to update user')
		},
	})
}

// Delete user mutation
export function useDeleteUser() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (userId: string) => userService.deleteUser(userId),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: userKeys.lists() })
			queryClient.invalidateQueries({ queryKey: userKeys.statistics() })
			toast.success('User deleted successfully')
		},
		onError: error => {
			console.error('Failed to delete user:', error)
			toast.error('Failed to delete user')
		},
	})
}
