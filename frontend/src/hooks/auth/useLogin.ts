import { useNavigate } from 'react-router'
import { useAuthStore } from '@/stores/authStore'
import { useCallback } from 'react'
import { toast } from 'sonner'

export function useLogin() {
	const navigate = useNavigate()
	const signIn = useAuthStore(state => state.signIn)
	const signInWithGoogle = useAuthStore(state => state.signInWithGoogle)
	const isLoading = useAuthStore(state => state.isLoading)
	const error = useAuthStore(state => state.error)
	const clearError = useAuthStore(state => state.clearError)

	const login = useCallback(
		async (email: string, password: string) => {
			try {
				await signIn(email, password)
				setTimeout(() => {
					navigate('/app')
				}, 100)
			} catch {
				toast.error('Failed to sign in. Please check your credentials.')
			}
		},
		[signIn, navigate]
	)

	return { login, signInWithGoogle, isLoading, error, clearError }
}
