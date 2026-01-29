import { useNavigate } from 'react-router'
import { useAuthStore } from '@/stores/authStore'
import { useCallback } from 'react'

export function useRegister() {
	const navigate = useNavigate()
	const signUp = useAuthStore(state => state.signUp)
	const isLoading = useAuthStore(state => state.isLoading)
	const error = useAuthStore(state => state.error)
	const clearError = useAuthStore(state => state.clearError)

	const register = useCallback(
		async (data: {
			email: string
			password: string
			firstName: string
			lastName: string
			role: string
		}) => {
			try {
				await signUp(
					data.email,
					data.password,
					data.firstName,
					data.lastName,
					data.role
				)
				navigate(`/confirm-email?email=${encodeURIComponent(data.email)}`)
			} catch {
				// Error handled in store
			}
		},
		[signUp, navigate]
	)

	return { register, isLoading, error, clearError }
}
