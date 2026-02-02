import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import type {
	CreateExamRequest,
	CreateMatrixRequest,
	CreateQuestionRequest,
} from '@/types/dtos/exams'
import type { CognitiveLevel } from '@/types/model/exams'
import { toast } from 'sonner'
import examService from '@/services/exam-microservice/exam.service'

export const examKeys = {
	all: ['exams'] as const,
	lists: () => [...examKeys.all, 'list'] as const,
	list: (isDraft?: boolean) => [...examKeys.lists(), { isDraft }] as const,
	detail: (id: string) => [...examKeys.all, 'detail', id] as const,
	subjects: (grade?: number) => ['subjects', { grade }] as const,
	topics: (subjectId: string) => ['topics', subjectId] as const,
	questions: (params?: {
		topicId?: string
		cognitiveLevel?: CognitiveLevel
		isPublic?: boolean
	}) => ['questions', params] as const,
	questionDetail: (id: string) => ['questions', 'detail', id] as const,
	examQuestions: (examId: string) => ['exams', examId, 'questions'] as const,
}

export function useSubjects(grade?: number) {
	return useQuery({
		queryKey: examKeys.subjects(grade),
		queryFn: async () => {
			const response = await examService.getSubjects(grade)
			return response.data?.subjects || []
		},
	})
}

export function useTopics(subjectId: string) {
	return useQuery({
		queryKey: examKeys.topics(subjectId),
		queryFn: async () => {
			const response = await examService.getTopics(subjectId)
			return response.data?.topics || []
		},
		enabled: !!subjectId,
	})
}

export function useExams(isDraft?: boolean) {
	return useQuery({
		queryKey: examKeys.list(isDraft),
		queryFn: async () => {
			const response = await examService.getExams(isDraft)
			return response.data?.exams || []
		},
	})
}

export function useExam(examId: string) {
	return useQuery({
		queryKey: examKeys.detail(examId),
		queryFn: async () => {
			const response = await examService.getExamById(examId)
			return response.data
		},
		enabled: !!examId,
	})
}

export function useCreateExam() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (request: CreateExamRequest) => examService.createExam(request),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: examKeys.lists() })
			toast.success('Exam created successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to create exam: ${error.message}`)
		},
	})
}

export function useUpdateExam() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			examId,
			data,
		}: {
			examId: string
			data: Partial<CreateExamRequest>
		}) => examService.updateExam(examId, data),
		onSuccess: (_, { examId }) => {
			queryClient.invalidateQueries({ queryKey: examKeys.lists() })
			queryClient.invalidateQueries({ queryKey: examKeys.detail(examId) })
			toast.success('Exam updated successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to update exam: ${error.message}`)
		},
	})
}

export function usePublishExam() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (examId: string) => examService.publishExam(examId),
		onSuccess: (_, examId) => {
			queryClient.invalidateQueries({ queryKey: examKeys.lists() })
			queryClient.invalidateQueries({ queryKey: examKeys.detail(examId) })
			toast.success('Exam published successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to publish exam: ${error.message}`)
		},
	})
}

export function useDeleteExam() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (examId: string) => examService.deleteExam(examId),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: examKeys.lists() })
			toast.success('Exam deleted successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to delete exam: ${error.message}`)
		},
	})
}

// ========== Matrix ==========
export function useCreateMatrix() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (request: CreateMatrixRequest) =>
			examService.createMatrix(request),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: examKeys.lists() })
			toast.success('Matrix created successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to create matrix: ${error.message}`)
		},
	})
}

// ========== Questions ==========
export function useQuestions(params?: {
	topicId?: string
	cognitiveLevel?: CognitiveLevel
	isPublic?: boolean
}) {
	return useQuery({
		queryKey: examKeys.questions(params),
		queryFn: async () => {
			const response = await examService.getQuestions(params)
			return response.data?.questions || []
		},
	})
}

export function useQuestion(questionId: string) {
	return useQuery({
		queryKey: examKeys.questionDetail(questionId),
		queryFn: async () => {
			const response = await examService.getQuestionById(questionId)
			return response.data
		},
		enabled: !!questionId,
	})
}

export function useExamQuestions(examId: string) {
	return useQuery({
		queryKey: examKeys.examQuestions(examId),
		queryFn: async () => {
			const response = await examService.getExamQuestions(examId)
			return response.data?.questions || []
		},
		enabled: !!examId,
	})
}

export function useCreateQuestion() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (request: CreateQuestionRequest) =>
			examService.createQuestion(request),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ['questions'] })
			toast.success('Question created successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to create question: ${error.message}`)
		},
	})
}

export function useUpdateQuestion() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			questionId,
			data,
		}: {
			questionId: string
			data: Partial<CreateQuestionRequest>
		}) => examService.updateQuestion(questionId, data),
		onSuccess: (_, { questionId }) => {
			queryClient.invalidateQueries({ queryKey: ['questions'] })
			queryClient.invalidateQueries({
				queryKey: examKeys.questionDetail(questionId),
			})
			toast.success('Question updated successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to update question: ${error.message}`)
		},
	})
}

export function useDeleteQuestion() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (questionId: string) => examService.deleteQuestion(questionId),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ['questions'] })
			toast.success('Question deleted successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to delete question: ${error.message}`)
		},
	})
}

export function useAddQuestionsToExam() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			examId,
			questionIds,
		}: {
			examId: string
			questionIds: string[]
		}) => examService.addQuestionsToExam(examId, questionIds),
		onSuccess: (_, { examId }) => {
			queryClient.invalidateQueries({
				queryKey: examKeys.examQuestions(examId),
			})
			queryClient.invalidateQueries({ queryKey: examKeys.detail(examId) })
			toast.success('Questions added to exam successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to add questions: ${error.message}`)
		},
	})
}

export function useRemoveQuestionFromExam() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			examId,
			questionId,
		}: {
			examId: string
			questionId: string
		}) => examService.removeQuestionFromExam(examId, questionId),
		onSuccess: (_, { examId }) => {
			queryClient.invalidateQueries({
				queryKey: examKeys.examQuestions(examId),
			})
			queryClient.invalidateQueries({ queryKey: examKeys.detail(examId) })
			toast.success('Question removed from exam successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to remove question: ${error.message}`)
		},
	})
}
