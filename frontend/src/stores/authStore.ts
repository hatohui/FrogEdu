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
	cachedRoleId: string | null // Cache roleId to avoid repeated /me calls
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
	syncRoleAndRefreshToken: () => Promise<string | null>
	clearError: () => void
}

export const useAuthStore = create<AuthState>()((set, get) => ({
	isAuthenticated: false,
	isLoading: true,
	error: null,
	cachedRoleId: null,

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
				cachedRoleId: null, // Clear cached role on sign out
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

				// If role not in JWT, check cache first before calling backend
				if (!customRole) {
					const cachedRole = get().cachedRoleId
					if (cachedRole) {
						customRole = cachedRole
					} else {
						// Only fetch from backend if not cached
						try {
							const backendUser = await userService.getCurrentUser()
							customRole = backendUser.roleId
							// Cache the roleId to avoid repeated API calls
							set({ cachedRoleId: customRole })
						} catch (error) {
							console.log('Could not fetch user from backend:', error)
						}
					}
				} else {
					// Update cache if role is in JWT
					set({ cachedRoleId: customRole })
				}

				set({
					isAuthenticated: true,
					isLoading: false,
				})
			} else {
				set({
					isAuthenticated: false,
					isLoading: false,
					cachedRoleId: null,
				})
			}
		} catch (error) {
			console.log('Not authenticated:', error)
			set({
				isAuthenticated: false,
				isLoading: false,
				cachedRoleId: null,
			})
		}
	},

	syncRoleAndRefreshToken: async () => {
		try {
			await userService.getCurrentUser()

			const session = await fetchAuthSession({ forceRefresh: true })
			const newRole = session.tokens?.idToken?.payload['custom:role'] as
				| string
				| undefined

			if (newRole) {
				return newRole
			} else {
				return null
			}
		} catch (error) {
			console.error('Failed to sync role and refresh token:', error)
			return null
		}
	},

	clearError: () => set({ error: null }),
}))
