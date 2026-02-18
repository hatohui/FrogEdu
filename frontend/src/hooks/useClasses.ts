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
	UpdateAssignmentRequest,
} from '@/types/dtos/classes'
import { examSessionKeys } from './useExamSessions'

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
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.student(),
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
			queryClient.invalidateQueries({
				queryKey: classKeys.detail(variables.classId),
			})
			queryClient.invalidateQueries({ queryKey: classKeys.all() })
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.student(),
			})
			toast.success('Exam assigned successfully!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to assign exam')
		},
	})
}

/**
 * Hook to update an assignment and its linked exam session (Teacher/Admin)
 */
export function useUpdateAssignment() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			classId,
			assignmentId,
			data,
		}: {
			classId: string
			assignmentId: string
			data: UpdateAssignmentRequest
		}) => classService.updateAssignment(classId, assignmentId, data),
		onSuccess: (_, variables) => {
			queryClient.invalidateQueries({
				queryKey: classKeys.detail(variables.classId),
			})
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.student(),
			})
			toast.success('Assignment updated successfully!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to update assignment')
		},
	})
}

/**
 * Hook to delete an assignment and its linked exam session (Teacher/Admin)
 */
export function useDeleteAssignment() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			classId,
			assignmentId,
		}: {
			classId: string
			assignmentId: string
		}) => classService.deleteAssignment(classId, assignmentId),
		onSuccess: (_, variables) => {
			queryClient.invalidateQueries({
				queryKey: classKeys.detail(variables.classId),
			})
			queryClient.invalidateQueries({ queryKey: classKeys.all() })
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.student(),
			})
			toast.success('Assignment deleted successfully!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to delete assignment')
		},
	})
}

/**
 * Hook to remove a student from a class (Teacher/Admin)
 */
export function useRemoveStudent() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			classId,
			studentId,
		}: {
			classId: string
			studentId: string
		}) => classService.removeStudent(classId, studentId),
		onSuccess: (_, variables) => {
			queryClient.invalidateQueries({
				queryKey: classKeys.detail(variables.classId),
			})
			queryClient.invalidateQueries({ queryKey: classKeys.all() })
			toast.success('Student removed successfully')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to remove student')
		},
	})
}
