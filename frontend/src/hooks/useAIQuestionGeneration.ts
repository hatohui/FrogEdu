import { useState, useCallback } from 'react'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { toast } from 'sonner'
import { aiService } from '@/services/ai.service'
import { useSubscription } from './useSubscription'
import type {
	GenerateQuestionsRequest,
	GenerateSingleQuestionRequest,
	AIGeneratedQuestion,
	AIGenerationState,
	AIMatrixTopicConfig,
} from '@/types/model/ai-service'
import type { CognitiveLevel, QuestionType } from '@/types/model/exam-service'

/**
 * Hook for AI question generation functionality
 * Requires Pro subscription to use AI features
 */
export const useAIQuestionGeneration = () => {
	const queryClient = useQueryClient()
	const { isPro } = useSubscription()

	const [generationState, setGenerationState] = useState<AIGenerationState>({
		isGenerating: false,
		generatedQuestions: [],
	})

	// Generate multiple questions from matrix
	const generateQuestionsMutation = useMutation({
		mutationFn: (request: GenerateQuestionsRequest) =>
			aiService.generateQuestions(request),
		onMutate: () => {
			setGenerationState(prev => ({
				...prev,
				isGenerating: true,
				error: undefined,
			}))
		},
		onSuccess: data => {
			setGenerationState(prev => ({
				...prev,
				isGenerating: false,
				generatedQuestions: [...prev.generatedQuestions, ...data.questions],
			}))
			toast.success(`Generated ${data.totalCount} questions successfully!`)
		},
		onError: error => {
			const errorMessage =
				error instanceof Error ? error.message : 'Failed to generate questions'
			setGenerationState(prev => ({
				...prev,
				isGenerating: false,
				error: errorMessage,
			}))
			toast.error(errorMessage)
		},
	})

	// Generate a single question
	const generateSingleQuestionMutation = useMutation({
		mutationFn: (request: GenerateSingleQuestionRequest) =>
			aiService.generateSingleQuestion(request),
		onMutate: () => {
			setGenerationState(prev => ({
				...prev,
				isGenerating: true,
				error: undefined,
			}))
		},
		onSuccess: question => {
			setGenerationState(prev => ({
				...prev,
				isGenerating: false,
				generatedQuestions: [...prev.generatedQuestions, question],
			}))
			toast.success('Question generated successfully!')
		},
		onError: error => {
			const errorMessage =
				error instanceof Error ? error.message : 'Failed to generate question'
			setGenerationState(prev => ({
				...prev,
				isGenerating: false,
				error: errorMessage,
			}))
			toast.error(errorMessage)
		},
	})

	/**
	 * Generate questions from matrix configuration
	 */
	const generateFromMatrix = useCallback(
		async (
			subject: string,
			grade: number,
			matrixTopics: AIMatrixTopicConfig[],
			language: 'vi' | 'en' = 'vi'
		) => {
			if (!isPro) {
				toast.error('Pro subscription required for AI question generation')
				return
			}

			return generateQuestionsMutation.mutateAsync({
				subject,
				grade,
				matrixTopics,
				language,
			})
		},
		[isPro, generateQuestionsMutation]
	)

	/**
	 * Generate a single question with specific parameters
	 */
	const generateSingle = useCallback(
		async (
			subject: string,
			grade: number,
			topicName: string,
			cognitiveLevel: CognitiveLevel,
			questionType: QuestionType,
			language: 'vi' | 'en' = 'vi',
			topicDescription?: string
		) => {
			if (!isPro) {
				toast.error('Pro subscription required for AI question generation')
				return
			}

			return generateSingleQuestionMutation.mutateAsync({
				subject,
				grade,
				topicName,
				cognitiveLevel,
				questionType,
				language,
				topicDescription,
			})
		},
		[isPro, generateSingleQuestionMutation]
	)

	/**
	 * Clear all generated questions
	 */
	const clearGeneratedQuestions = useCallback(() => {
		setGenerationState({
			isGenerating: false,
			generatedQuestions: [],
		})
	}, [])

	/**
	 * Remove a specific generated question by index
	 */
	const removeGeneratedQuestion = useCallback((index: number) => {
		setGenerationState(prev => ({
			...prev,
			generatedQuestions: prev.generatedQuestions.filter((_, i) => i !== index),
		}))
	}, [])

	/**
	 * Select a generated question for use
	 */
	const selectQuestion = useCallback(
		(question: AIGeneratedQuestion) => {
			// Invalidate questions cache to trigger refetch after adding
			queryClient.invalidateQueries({ queryKey: ['questions'] })
			return question
		},
		[queryClient]
	)

	return {
		// State
		isPro,
		isGenerating: generationState.isGenerating,
		generatedQuestions: generationState.generatedQuestions,
		error: generationState.error,

		// Actions
		generateFromMatrix,
		generateSingle,
		clearGeneratedQuestions,
		removeGeneratedQuestion,
		selectQuestion,

		// Mutation states
		isGeneratingBatch: generateQuestionsMutation.isPending,
		isGeneratingSingle: generateSingleQuestionMutation.isPending,
	}
}

export default useAIQuestionGeneration
