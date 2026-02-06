import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import type {
	CreateExamRequest,
	CreateMatrixRequest,
	UpdateMatrixRequest,
	CreateQuestionRequest,
	CreateSubjectRequest,
	UpdateSubjectRequest,
	CreateTopicRequest,
	UpdateTopicRequest,
} from '@/types/dtos/exams'
import type { CognitiveLevel } from '@/types/model/exam-service'
import { toast } from 'sonner'
import examService from '@/services/exam-microservice/exam.service'

export const examKeys = {
	all: ['exams'] as const,
	lists: () => [...examKeys.all, 'list'] as const,
	list: (isDraft?: boolean) => [...examKeys.lists(), { isDraft }] as const,
	detail: (id: string) => [...examKeys.all, 'detail', id] as const,
	subjects: (grade?: number) => ['subjects', { grade }] as const,
	topics: (subjectId: string) => ['topics', subjectId] as const,
	matrices: () => ['matrices'] as const,
	matrix: (matrixId: string) => ['matrices', matrixId] as const,
	matrixByExam: (examId: string) => ['matrices', 'exam', examId] as const,
	questions: (params?: {
		topicId?: string
		cognitiveLevel?: CognitiveLevel
		isPublic?: boolean
	}) => ['questions', params] as const,
	questionDetail: (id: string) => ['questions', 'detail', id] as const,
	examQuestions: (examId: string) => ['exams', examId, 'questions'] as const,
}

export function useSubjects(grade?: number, enabled: boolean = true) {
	return useQuery({
		queryKey: examKeys.subjects(grade),
		enabled: enabled,
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
export function useMatrices() {
	return useQuery({
		queryKey: examKeys.matrices(),
		queryFn: async () => {
			const response = await examService.getMatrices()
			return response.data?.matrices || []
		},
	})
}

export function useMatrixById(matrixId: string) {
	return useQuery({
		queryKey: examKeys.matrix(matrixId),
		queryFn: async () => {
			const response = await examService.getMatrixById(matrixId)
			return response.data
		},
		enabled: !!matrixId,
	})
}

export function useMatrixByExamId(examId: string) {
	return useQuery({
		queryKey: examKeys.matrixByExam(examId),
		queryFn: async () => {
			try {
				const response = await examService.getMatrixByExamId(examId)
				return response.data
			} catch (error) {
				// If matrix doesn't exist (404), return null instead of throwing
				if (
					(error as { response?: { status?: number } })?.response?.status ===
					404
				) {
					return null
				}
				throw error
			}
		},
		enabled: !!examId,
		retry: (failureCount, error) => {
			// Don't retry on 404 (matrix not found)
			if (
				(error as { response?: { status?: number } })?.response?.status === 404
			) {
				return false
			}
			// Retry other errors up to 3 times
			return failureCount < 3
		},
	})
}

export function useCreateMatrix() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (request: CreateMatrixRequest) =>
			examService.createMatrix(request),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: examKeys.matrices() })
			toast.success('Matrix created successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to create matrix: ${error.message}`)
		},
	})
}

export function useUpdateMatrix() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (params: { matrixId: string; data: UpdateMatrixRequest }) =>
			examService.updateMatrix(params.matrixId, params.data),
		onSuccess: (_, variables) => {
			queryClient.invalidateQueries({ queryKey: examKeys.matrices() })
			queryClient.invalidateQueries({
				queryKey: examKeys.matrix(variables.matrixId),
			})
			toast.success('Matrix updated successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to update matrix: ${error.message}`)
		},
	})
}

export function useDeleteMatrix() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (matrixId: string) => examService.deleteMatrix(matrixId),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: examKeys.matrices() })
			toast.success('Matrix deleted successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to delete matrix: ${error.message}`)
		},
	})
}

export function useAttachMatrixToExam() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (params: { examId: string; matrixId: string }) =>
			examService.attachMatrixToExam(params.examId, {
				matrixId: params.matrixId,
			}),
		onSuccess: (_, variables) => {
			queryClient.invalidateQueries({ queryKey: examKeys.lists() })
			queryClient.invalidateQueries({
				queryKey: examKeys.detail(variables.examId),
			})
			queryClient.invalidateQueries({
				queryKey: examKeys.matrixByExam(variables.examId),
			})
			toast.success('Matrix attached to exam successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to attach matrix: ${error.message}`)
		},
	})
}

export function useDetachMatrixFromExam() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (examId: string) => examService.detachMatrixFromExam(examId),
		onSuccess: (_, examId) => {
			queryClient.invalidateQueries({ queryKey: examKeys.lists() })
			queryClient.invalidateQueries({ queryKey: examKeys.detail(examId) })
			queryClient.invalidateQueries({
				queryKey: examKeys.matrixByExam(examId),
			})
			toast.success('Matrix detached from exam successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to detach matrix: ${error.message}`)
		},
	})
}

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

export function useCreateQuestion(examId?: string) {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (request: CreateQuestionRequest) =>
			examService.createQuestion(request),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ['questions'] })
			// Also invalidate exam questions if we're creating in context of an exam
			if (examId) {
				queryClient.invalidateQueries({
					queryKey: examKeys.examQuestions(examId),
				})
				queryClient.invalidateQueries({ queryKey: examKeys.matrix(examId) })
			}
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

// ========== Subjects ==========
export function useCreateSubject() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (request: CreateSubjectRequest) =>
			examService.createSubject(request),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ['subjects'] })
			toast.success('Subject created successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to create subject: ${error.message}`)
		},
	})
}

export function useUpdateSubject() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			subjectId,
			data,
		}: {
			subjectId: string
			data: UpdateSubjectRequest
		}) => examService.updateSubject(subjectId, data),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ['subjects'] })
			toast.success('Subject updated successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to update subject: ${error.message}`)
		},
	})
}

export function useDeleteSubject() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (subjectId: string) => examService.deleteSubject(subjectId),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ['subjects'] })
			toast.success('Subject deleted successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to delete subject: ${error.message}`)
		},
	})
}

// ========== Topics ==========
export function useCreateTopic() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (request: CreateTopicRequest) =>
			examService.createTopic(request),
		onSuccess: (_, { subjectId }) => {
			queryClient.invalidateQueries({ queryKey: examKeys.topics(subjectId) })
			toast.success('Topic created successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to create topic: ${error.message}`)
		},
	})
}

export function useUpdateTopic() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			subjectId,
			topicId,
			data,
		}: {
			subjectId: string
			topicId: string
			data: UpdateTopicRequest
		}) => examService.updateTopic(subjectId, topicId, data),
		onSuccess: (_, { subjectId }) => {
			queryClient.invalidateQueries({ queryKey: examKeys.topics(subjectId) })
			toast.success('Topic updated successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to update topic: ${error.message}`)
		},
	})
}

export function useDeleteTopic() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: ({
			subjectId,
			topicId,
		}: {
			subjectId: string
			topicId: string
		}) => examService.deleteTopic(subjectId, topicId),
		onSuccess: (_, { subjectId }) => {
			queryClient.invalidateQueries({ queryKey: examKeys.topics(subjectId) })
			toast.success('Topic deleted successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to delete topic: ${error.message}`)
		},
	})
}

// ========== Preview & Export ==========

export function useExamPreview(examId: string) {
	return useQuery({
		queryKey: [...examKeys.detail(examId), 'preview'],
		queryFn: async () => {
			const response = await examService.getExamPreview(examId)
			return response.data
		},
		enabled: !!examId,
	})
}

export function useExportExamToPdf() {
	return useMutation({
		mutationFn: async (examId: string) => {
			const blob = await examService.exportExamToPdf(examId)
			const url = window.URL.createObjectURL(blob)
			const link = document.createElement('a')
			link.href = url
			link.download = `exam-${examId}.pdf`
			document.body.appendChild(link)
			link.click()
			document.body.removeChild(link)
			window.URL.revokeObjectURL(url)
		},
		onSuccess: () => {
			toast.success('Exam exported to PDF successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to export PDF: ${error.message}`)
		},
	})
}

export function useExportExamToExcel() {
	return useMutation({
		mutationFn: async (examId: string) => {
			const blob = await examService.exportExamToExcel(examId)
			const url = window.URL.createObjectURL(blob)
			const link = document.createElement('a')
			link.href = url
			link.download = `exam-${examId}.xlsx`
			document.body.appendChild(link)
			link.click()
			document.body.removeChild(link)
			window.URL.revokeObjectURL(url)
		},
		onSuccess: () => {
			toast.success('Exam exported to Excel successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to export Excel: ${error.message}`)
		},
	})
}

// ========== Matrix Export ==========
export function useExportMatrixToPdf() {
	return useMutation({
		mutationFn: async (matrixId: string) => {
			const blob = await examService.exportMatrixToPdf(matrixId)
			const url = window.URL.createObjectURL(blob)
			const link = document.createElement('a')
			link.href = url
			link.download = `matrix-${matrixId}.pdf`
			document.body.appendChild(link)
			link.click()
			document.body.removeChild(link)
			window.URL.revokeObjectURL(url)
		},
		onSuccess: () => {
			toast.success('Matrix exported to PDF successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to export PDF: ${error.message}`)
		},
	})
}

export function useExportMatrixToExcel() {
	return useMutation({
		mutationFn: async (matrixId: string) => {
			const blob = await examService.exportMatrixToExcel(matrixId)
			const url = window.URL.createObjectURL(blob)
			const link = document.createElement('a')
			link.href = url
			link.download = `matrix-${matrixId}.xlsx`
			document.body.appendChild(link)
			link.click()
			document.body.removeChild(link)
			window.URL.revokeObjectURL(url)
		},
		onSuccess: () => {
			toast.success('Matrix exported to Excel successfully!')
		},
		onError: (error: Error) => {
			toast.error(`Failed to export Excel: ${error.message}`)
		},
	})
}
