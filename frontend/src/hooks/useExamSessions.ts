import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { toast } from 'sonner'
import { examSessionService } from '@/services/class-microservice/examSession.service'
import type {
	ExamSession,
	StudentExamAttempt,
	ExamSessionResults,
} from '@/types/model/class-service'
import type {
	CreateExamSessionRequest,
	UpdateExamSessionRequest,
	SubmitExamAttemptRequest,
} from '@/types/dtos/classes'

// ─── Query Keys ───

export const examSessionKeys = {
	all: ['exam-sessions'] as const,
	byClass: (classId: string) =>
		[...examSessionKeys.all, 'class', classId] as const,
	detail: (sessionId: string) =>
		[...examSessionKeys.all, 'detail', sessionId] as const,
	student: (upcomingOnly?: boolean) =>
		[...examSessionKeys.all, 'student', { upcomingOnly }] as const,
	attempts: (sessionId: string) =>
		[...examSessionKeys.all, 'attempts', sessionId] as const,
	attemptDetail: (attemptId: string) =>
		[...examSessionKeys.all, 'attempt', attemptId] as const,
	results: (sessionId: string) =>
		[...examSessionKeys.all, 'results', sessionId] as const,
}

// ─── Session Queries ───

/**
 * Fetch all exam sessions for a class
 */
export function useExamSessions(classId: string) {
	return useQuery<ExamSession[], Error>({
		queryKey: examSessionKeys.byClass(classId),
		queryFn: () => examSessionService.getExamSessions(classId),
		enabled: !!classId,
	})
}

/**
 * Fetch a single exam session detail
 */
export function useExamSessionDetail(sessionId: string) {
	return useQuery<ExamSession, Error>({
		queryKey: examSessionKeys.detail(sessionId),
		queryFn: () => examSessionService.getExamSessionDetail(sessionId),
		enabled: !!sessionId,
	})
}

/**
 * Fetch active/upcoming exam sessions for the current student
 */
export function useStudentExamSessions(upcomingOnly: boolean = false) {
	return useQuery<ExamSession[], Error>({
		queryKey: examSessionKeys.student(upcomingOnly),
		queryFn: () => examSessionService.getStudentExamSessions(upcomingOnly),
	})
}

// ─── Session Mutations ───

/**
 * Create a new exam session for a class (Teacher)
 */
export function useCreateExamSession() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			classId,
			data,
		}: {
			classId: string
			data: CreateExamSessionRequest
		}) => examSessionService.createExamSession(classId, data),
		onSuccess: (_, variables) => {
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.byClass(variables.classId),
			})
			toast.success('Exam session created successfully!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to create exam session')
		},
	})
}

/**
 * Update an exam session (Teacher)
 */
export function useUpdateExamSession() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			sessionId,
			data,
		}: {
			sessionId: string
			data: UpdateExamSessionRequest
		}) => examSessionService.updateExamSession(sessionId, data),
		onSuccess: result => {
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.detail(result.id),
			})
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.byClass(result.classId),
			})
			toast.success('Exam session updated successfully!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to update exam session')
		},
	})
}

/**
 * Delete an exam session (Teacher)
 */
export function useDeleteExamSession() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({ sessionId }: { sessionId: string; classId: string }) =>
			examSessionService.deleteExamSession(sessionId),
		onSuccess: (_, variables) => {
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.byClass(variables.classId),
			})
			toast.success('Exam session deleted successfully!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to delete exam session')
		},
	})
}

// ─── Attempt Queries ───

/**
 * Fetch all attempts for a session (Teacher)
 */
export function useSessionAttempts(sessionId: string) {
	return useQuery<StudentExamAttempt[], Error>({
		queryKey: examSessionKeys.attempts(sessionId),
		queryFn: () => examSessionService.getSessionAttempts(sessionId),
		enabled: !!sessionId,
	})
}

/**
 * Fetch a single attempt with answers
 */
export function useAttemptDetail(attemptId: string) {
	return useQuery<StudentExamAttempt, Error>({
		queryKey: examSessionKeys.attemptDetail(attemptId),
		queryFn: () => examSessionService.getAttemptDetail(attemptId),
		enabled: !!attemptId,
	})
}

/**
 * Fetch the current student's own attempts for a session (includes scores).
 */
export function useMySessionAttempts(sessionId: string) {
	return useQuery<StudentExamAttempt[], Error>({
		queryKey: [...examSessionKeys.all, 'my-attempts', sessionId] as const,
		queryFn: () => examSessionService.getMySessionAttempts(sessionId),
		enabled: !!sessionId,
	})
}

/**
 * Fetch session results with student scores (Teacher)
 */
export function useSessionResults(sessionId: string) {
	return useQuery<ExamSessionResults, Error>({
		queryKey: examSessionKeys.results(sessionId),
		queryFn: () => examSessionService.getSessionResults(sessionId),
		enabled: !!sessionId,
	})
}

// ─── Attempt Mutations ───

/**
 * Start an exam attempt (Student)
 */
export function useStartExamAttempt() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (sessionId: string) =>
			examSessionService.startExamAttempt(sessionId),
		onSuccess: (_, sessionId) => {
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.attempts(sessionId),
			})
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.detail(sessionId),
			})
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to start exam attempt')
		},
	})
}

/**
 * Submit an exam attempt with answers (Student)
 */
export function useSubmitExamAttempt() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			sessionId,
			attemptId,
			data,
		}: {
			sessionId: string
			attemptId: string
			data: SubmitExamAttemptRequest
		}) => examSessionService.submitExamAttempt(sessionId, attemptId, data),
		onSuccess: (_result, variables) => {
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.attempts(variables.sessionId),
			})
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.attemptDetail(variables.attemptId),
			})
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.detail(variables.sessionId),
			})
			queryClient.invalidateQueries({
				queryKey: [...examSessionKeys.all, 'my-attempts', variables.sessionId],
			})
			queryClient.invalidateQueries({
				queryKey: examSessionKeys.student(),
			})
			toast.success('Exam submitted successfully!')
		},
		onError: (error: Error) => {
			toast.error(error.message || 'Failed to submit exam')
		},
	})
}
