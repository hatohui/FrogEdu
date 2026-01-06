import { create } from 'zustand'

interface AuthState {
	isAuthenticated: boolean
	user: string | null
	login: () => void
	logout: () => void
}

const useAuth = create<AuthState>(set => ({
	isAuthenticated: false,
	user: null,
	login: () => set({ isAuthenticated: true, user: 'John Doe' }),
	logout: () => set({ isAuthenticated: false, user: null }),
}))

export default useAuth
