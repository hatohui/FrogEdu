import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { toast } from 'sonner'
import { classService } from '@/services/class-microservice/class.service'
import type {
	ClassRoom,
	ClassDetail,
	ClassAssignment,
} from '@/types/model/class-service'
import type {
	AssignExamRequest,
	CreateClassRequest,
} from '@/types/dtos/classes'

// Query keys
export const classKeys = {
	all: () => ['classes'] as const,
	detail: (id: string) => [...classKeys.all(), 'detail', id] as const,
	assignments: (classId: string) =>
		[...classKeys.all(), 'assignments', classId] as const,
}

/**
 * Hook to fetch classes for the current user (role-based: Admin→all, Teacher→own, Student→enrolled)
 */
export function useClasses() {
	return useQuery<ClassRoom[], Error>({
		queryKey: classKeys.all(),
		queryFn: () => classService.getMyClasses(),
	})
}

/**
 * Hook to fetch class details
 */
export function useClassDetail(classId: string) {
	return useQuery<ClassDetail, Error>({
		queryKey: classKeys.detail(classId),
		queryFn: () => classService.getClassDetail(classId),
		enabled: !!classId,
	})
}

/**
 * Hook to create a new class (Teacher)
 */
export function useCreateClass() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (data: CreateClassRequest) => classService.createClass(data),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: classKeys.all() })
			toast.success('Class created successfully!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to create class')
		},
	})
}

/**
 * Hook to join a class via invite code (Student)
 */
export function useJoinClass() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (inviteCode: string) => classService.joinClass({ inviteCode }),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: classKeys.all() })
			toast.success('Successfully joined the class!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to join class')
		},
	})
}

/**
 * Hook to fetch assignments for a class
 */
export function useClassAssignments(classId: string) {
	return useQuery<ClassAssignment[], Error>({
		queryKey: classKeys.assignments(classId),
		queryFn: () => classService.getClassAssignments(classId),
		enabled: !!classId,
	})
}

/**
 * Hook to assign an exam to a class (Teacher)
 */
export function useAssignExam() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			classId,
			data,
		}: {
			classId: string
			data: AssignExamRequest
		}) => classService.assignExam(classId, data),
		onSuccess: (_, variables) => {
			queryClient.invalidateQueries({
				queryKey: classKeys.assignments(variables.classId),
			})
			queryClient.invalidateQueries({
				queryKey: classKeys.detail(variables.classId),
			})
			toast.success('Exam assigned successfully!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to assign exam')
		},
	})
}

// ─── Admin hooks ───
// Note: Admin users can use regular useClasses/useClassDetail since backend filters by role.
// Only admin-specific assign is needed to bypass ownership checks.

/**
 * Hook to assign an exam to a class (Admin — bypasses ownership)
 */
export function useAdminAssignExam() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			classId,
			data,
		}: {
			classId: string
			data: AssignExamRequest
		}) => classService.adminAssignExam(classId, data),
		onSuccess: (_, variables) => {
			// Invalidate regular class queries (used by admin dashboard now)
			queryClient.invalidateQueries({
				queryKey: classKeys.detail(variables.classId),
			})
			queryClient.invalidateQueries({ queryKey: classKeys.all() })
			toast.success('Exam assigned successfully!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to assign exam')
		},
	})
}
