import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { toast } from 'sonner'
import {
	classService,
	type ClassDetailsDto,
	type ClassDto,
	type CreateClassDto,
	type DashboardStatsDto,
} from '@/services/class-microservice/class.service'

// Query keys
export const classKeys = {
	all: ['classes'] as const,
	list: (includeArchived?: boolean) =>
		[...classKeys.all, 'list', { includeArchived }] as const,
	details: (id: string) => [...classKeys.all, 'details', id] as const,
	dashboardStats: () => [...classKeys.all, 'dashboard-stats'] as const,
}

/**
 * Hook to fetch all classes for the current user
 */
export function useClasses(includeArchived?: boolean) {
	return useQuery<ClassDto[], Error>({
		queryKey: classKeys.list(includeArchived),
		queryFn: () => classService.getMyClasses(),
	})
}

/**
 * Hook to fetch class details
 */
export function useClassDetails(classId: string) {
	return useQuery<ClassDetailsDto, Error>({
		queryKey: classKeys.details(classId),
		queryFn: () => classService.getClassDetails(classId),
		enabled: !!classId,
	})
}

/**
 * Hook to fetch dashboard statistics
 */
export function useDashboardStats() {
	return useQuery<DashboardStatsDto, Error>({
		queryKey: classKeys.dashboardStats(),
		queryFn: () => classService.getDashboardStats(),
	})
}

/**
 * Hook to create a new class
 */
export function useCreateClass() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (data: CreateClassDto) => classService.createClass(data),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: classKeys.all })
			toast.success('Class created successfully!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to create class')
		},
	})
}

/**
 * Hook to join a class via invite code
 */
export function useJoinClass() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (inviteCode: string) => classService.joinClass({ inviteCode }),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: classKeys.all })
			toast.success('Successfully joined the class!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to join class')
		},
	})
}

/**
 * Hook to regenerate invite code
 */
export function useRegenerateInviteCode() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			classId,
			expiresInDays = 7,
		}: {
			classId: string
			expiresInDays?: number
		}) => classService.regenerateInviteCode(classId, expiresInDays),
		onSuccess: (_, variables) => {
			queryClient.invalidateQueries({
				queryKey: classKeys.details(variables.classId),
			})
			toast.success('Invite code regenerated!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to regenerate invite code')
		},
	})
}
