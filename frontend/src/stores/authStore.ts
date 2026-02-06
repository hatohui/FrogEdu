import { create } from 'zustand'
import {
	getCurrentUser,
	signOut as amplifySignOut,
	signIn as amplifySignIn,
	signUp as amplifySignUp,
	fetchAuthSession,
	signInWithRedirect,
} from 'aws-amplify/auth'
import userService from '@/services/user-microservice/user.service'

interface AuthState {
	isAuthenticated: boolean
	isLoading: boolean
	error: string | null
	signIn: (email: string, password: string) => Promise<void>
	signUp: (
		email: string,
		password: string,
		firstName: string,
		lastName: string,
		role: string
	) => Promise<void>
	signInWithGoogle: () => Promise<void>
	signOut: () => Promise<void>
	refreshAuth: () => Promise<void>
	clearError: () => void
}

export const useAuthStore = create<AuthState>()((set, get) => ({
	isAuthenticated: false,
	isLoading: true,
	error: null,

	signIn: async (email: string, password: string) => {
		set({ isLoading: true, error: null })
		try {
			await amplifySignIn({ username: email, password })
			await get().refreshAuth()
		} catch (error) {
			const message =
				error instanceof Error
					? error.message
					: 'Failed to sign in. Please check your credentials.'
			set({ error: message, isLoading: false })
			throw error
		}
	},

	signUp: async (
		email: string,
		password: string,
		firstName: string,
		lastName: string,
		role: string
	) => {
		set({ isLoading: true, error: null })
		try {
			await amplifySignUp({
				username: email,
				password,
				options: {
					userAttributes: {
						email,
						given_name: firstName,
						family_name: lastName,
						'custom:role': role,
					},
				},
			})

			set({ isLoading: false })
		} catch (error) {
			const message =
				error instanceof Error
					? error.message
					: 'Failed to create account. Please try again.'
			set({ error: message, isLoading: false })
		}
	},

	signInWithGoogle: async () => {
		set({ isLoading: true, error: null })
		try {
			try {
				const currentUser = await getCurrentUser()
				if (currentUser) {
					window.location.href = '/app'
					return
				}
			} catch {
				console.log('no user')
			}

			await signInWithRedirect({
				provider: 'Google',
				customState: 'oauth-login',
			})
		} catch (error) {
			const message =
				error instanceof Error
					? error.message
					: 'Failed to sign in with Google. Please try again.'
			set({ error: message, isLoading: false })
			throw error
		}
	},

	signOut: async () => {
		try {
			await amplifySignOut()
			set({
				isAuthenticated: false,
				error: null,
			})
		} catch (error) {
			console.error('Error signing out:', error)
			const message =
				error instanceof Error ? error.message : 'Failed to sign out'
			set({ error: message })
		}
	},

	refreshAuth: async () => {
		set({ isLoading: true, error: null })
		try {
			const currentUser = await getCurrentUser()
			const session = await fetchAuthSession()

			if (currentUser && session.tokens) {
				const idToken = session.tokens.idToken
				let customRole = idToken?.payload['custom:role'] as string | undefined

				if (!customRole) {
					try {
						const backendUser = await userService.getCurrentUser()
						customRole = backendUser.roleId
					} catch (error) {
						console.log('Could not fetch user from backend:', error)
					}
				}

				set({
					isAuthenticated: true,
					isLoading: false,
				})
			} else {
				set({
					isAuthenticated: false,
					isLoading: false,
				})
			}
		} catch (error) {
			console.log('Not authenticated:', error)
			set({
				isAuthenticated: false,
				isLoading: false,
			})
		}
	},

	clearError: () => set({ error: null }),
}))
