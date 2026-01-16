import { useAuthStore } from '@/stores/authStore'
import type { AuthUser } from 'aws-amplify/auth'

interface UseAuthReturn {
	isAuthenticated: boolean
	user: AuthUser | null
	isLoading: boolean
	signOut: () => Promise<void>
	refreshAuth: () => Promise<void>
}

const useAuth = (): UseAuthReturn => {
	const isAuthenticated = useAuthStore(state => state.isAuthenticated)
	const user = useAuthStore(state => state.user)
	const isLoading = useAuthStore(state => state.isLoading)
	const signOut = useAuthStore(state => state.signOut)
	const refreshAuth = useAuthStore(state => state.refreshAuth)

	return {
		isAuthenticated,
		user,
		isLoading,
		signOut,
		refreshAuth,
	}
}

export default useAuth
