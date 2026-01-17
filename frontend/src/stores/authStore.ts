import { create } from 'zustand'
import {
	getCurrentUser,
	signOut as amplifySignOut,
	signIn as amplifySignIn,
	signUp as amplifySignUp,
	fetchAuthSession,
	signInWithRedirect,
} from 'aws-amplify/auth'
import type { AuthUser } from 'aws-amplify/auth'

interface UserProfile {
	sub: string
	email?: string
	name?: string
	given_name?: string
	family_name?: string
	picture?: string
	username?: string
}

interface AuthState {
	isAuthenticated: boolean
	user: AuthUser | null
	userProfile: UserProfile | null
	isLoading: boolean
	error: string | null
	signIn: (email: string, password: string) => Promise<void>
	signUp: (email: string, password: string) => Promise<void>
	signInWithGoogle: () => Promise<void>
	signOut: () => Promise<void>
	refreshAuth: () => Promise<void>
	clearError: () => void
}

export const useAuthStore = create<AuthState>()((set, get) => ({
	isAuthenticated: false,
	user: null,
	userProfile: null,
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

	signUp: async (email: string, password: string) => {
		set({ isLoading: true, error: null })
		try {
			await amplifySignUp({
				username: email,
				password,
				options: {
					userAttributes: {
						email,
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
			throw error
		}
	},

	signInWithGoogle: async () => {
		set({ isLoading: true, error: null })
		try {
			// Check if user is already signed in
			try {
				const currentUser = await getCurrentUser()
				if (currentUser) {
					// User is already signed in, redirect to dashboard
					window.location.href = '/dashboard'
					return
				}
			} catch {
				// User is not signed in, proceed with Google sign-in
			}

			await signInWithRedirect({
				provider: 'Google',
				customState: 'oauth-login',
			})
			// Note: The user will be redirected to Google,
			// so this function won't complete here
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
				user: null,
				userProfile: null,
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
				// Get user profile from ID token payload instead of fetchUserAttributes
				// fetchUserAttributes requires aws.cognito.signin.user.admin scope which OAuth doesn't provide
				const idToken = session.tokens.idToken
				const userProfile: UserProfile = {
					sub: currentUser.userId,
					email: idToken?.payload.email as string | undefined,
					name: idToken?.payload.name as string | undefined,
					given_name: idToken?.payload.given_name as string | undefined,
					family_name: idToken?.payload.family_name as string | undefined,
					picture: idToken?.payload.picture as string | undefined,
					username: currentUser.username,
				}

				set({
					user: currentUser,
					userProfile,
					isAuthenticated: true,
					isLoading: false,
				})
			} else {
				set({
					user: null,
					userProfile: null,
					isAuthenticated: false,
					isLoading: false,
				})
			}
		} catch (error) {
			console.log('Not authenticated:', error)
			set({
				user: null,
				userProfile: null,
				isAuthenticated: false,
				isLoading: false,
			})
		}
	},

	clearError: () => set({ error: null }),
}))
