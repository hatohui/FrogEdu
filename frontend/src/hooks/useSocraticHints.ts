import { useMutation } from '@tanstack/react-query'
import { aiService } from '@/services/ai.service'
import type {
	SocraticHintsRequest,
	SocraticHintsResponse,
} from '@/types/dtos/ai'

/**
 * Hook for the Socratic hints feature.
 * Teachers can generate guiding questions to help a student
 * who answered incorrectly, without revealing the answer directly.
 */
export function useSocraticHints() {
	return useMutation<SocraticHintsResponse, Error, SocraticHintsRequest>({
		mutationFn: (request: SocraticHintsRequest) =>
			aiService.getSocraticHints(request),
	})
}
