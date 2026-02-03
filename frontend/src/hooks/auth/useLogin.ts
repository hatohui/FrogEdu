import { useNavigate } from 'react-router'
import { useAuthStore } from '@/stores/authStore'
import { useCallback } from 'react'
import { toast } from 'sonner'
import userService from '@/services/user-microservice/user.service'

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
				setTimeout(async () => {
					try {
						const user = await userService.getCurrentUser()
						if (user.roleId) {
							const role = await userService.getRoleById(user.roleId)
							if (role?.name === 'Admin') {
								navigate('/dashboard')
								return
							}
						}
					} catch (err) {
						console.error('Failed to get user role:', err)
					}
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
