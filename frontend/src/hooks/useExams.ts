import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import examService from '@/services/exam.service'
import type {
	CreateExamRequest,
	CreateMatrixRequest,
	CreateQuestionRequest,
} from '@/types/dtos/exams'
import type { CognitiveLevel } from '@/types/model/exams'
import { toast } from 'sonner'

// Query Keys
export const examKeys = {
	all: ['exams'] as const,
	lists: () => [...examKeys.all, 'list'] as const,
	list: (isDraft?: boolean) => [...examKeys.lists(), { isDraft }] as const,
	subjects: (grade?: number) => ['subjects', { grade }] as const,
	topics: (subjectId: string) => ['topics', subjectId] as const,
	questions: (params?: {
		topicId?: string
		cognitiveLevel?: CognitiveLevel
		isPublic?: boolean
	}) => ['questions', params] as const,
}

// ========== Subjects & Topics ==========
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

// ========== Exams ==========
export function useExams(isDraft?: boolean) {
	return useQuery({
		queryKey: examKeys.list(isDraft),
		queryFn: async () => {
			const response = await examService.getExams(isDraft)
			return response.data?.exams || []
		},
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
