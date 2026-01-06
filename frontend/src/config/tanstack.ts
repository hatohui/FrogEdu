import {
	QueryCache,
	MutationCache,
	type QueryClientConfig,
} from '@tanstack/react-query'
import { toast } from 'sonner'

const useTanstackConfig = (
	translation: ReturnType<typeof import('react-i18next').useTranslation>
): QueryClientConfig => {
	const { t } = translation

	return {
		queryCache: new QueryCache({
			onError: error => {
				const errorMessage = error.message || 'UNKNOWN_ERROR'
				const translatedMessage = t(`errors.${errorMessage}`, {
					defaultValue: errorMessage.toLowerCase().replace(/_/g, ' '),
				})
				toast.error(t('errors.title'), {
					description: translatedMessage,
				})
			},
		}),
		mutationCache: new MutationCache({
			onError: error => {
				const errorMessage = error.message || 'UNKNOWN_ERROR'
				const translatedMessage = t(`errors.${errorMessage}`, {
					defaultValue: errorMessage.toLowerCase().replace(/_/g, ' '),
				})
				toast.error(t('errors.title'), {
					description: translatedMessage,
				})
			},
		}),
		defaultOptions: {
			queries: {
				retry: 1,
			},
		},
	}
}

export default useTanstackConfig
