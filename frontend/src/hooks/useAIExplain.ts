import { useMutation } from '@tanstack/react-query'
import { aiService } from '@/services/ai.service'
import type { ExplainQuestionRequest } from '@/types/dtos/ai'

/**
 * Hook for the AI explain-a-question feature.
 * Returns a mutation that sends a question and correct answer to the AI service
 * and gets back a child-friendly explanation.
 */
export function useExplainQuestion() {
	return useMutation<string, Error, ExplainQuestionRequest>({
		mutationFn: (request: ExplainQuestionRequest) =>
			aiService.explainQuestion(request),
	})
}
