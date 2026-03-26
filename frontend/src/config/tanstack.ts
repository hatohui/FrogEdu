import {
	QueryCache,
	MutationCache,
	type QueryClientConfig,
} from '@tanstack/react-query'
import { toast } from 'sonner'
import { AxiosError } from 'axios'

const useTanstackConfig = (
	translation: ReturnType<typeof import('react-i18next').useTranslation>
): QueryClientConfig => {
	const { t } = translation

	// Helper to determine if error should be displayed
	const shouldShowError = (error: Error): boolean => {
		if (error instanceof AxiosError) {
			const status = error.response?.status
			if (status === 404 || status === 401) {
				return false
			}
		}
		return true
	}

	return {
		queryCache: new QueryCache({
			onError: error => {
				if (shouldShowError(error)) {
					console.error('Query error:', error)
				}
			},
		}),
		mutationCache: new MutationCache({
			onError: error => {
				// Only show toast for mutation errors (user actions)
				if (shouldShowError(error)) {
					const errorMessage = error.message || 'UNKNOWN_ERROR'
					const translatedMessage = t(`errors.${errorMessage}`, {
						defaultValue: errorMessage.toLowerCase().replace(/_/g, ' '),
					})
					toast.error(t('errors.title'), {
						description: translatedMessage,
					})
				}
			},
		}),
		defaultOptions: {
			queries: {
				retry: 1,
				refetchOnWindowFocus: false,
			},
		},
	}
}

export default useTanstackConfig
