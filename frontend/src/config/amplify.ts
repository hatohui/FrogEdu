import { Amplify } from 'aws-amplify'
import type { ResourcesConfig } from 'aws-amplify'
import { useAuthStore } from '@/stores/authStore'

// Configure Amplify with Cognito settings
const amplifyConfig: ResourcesConfig = {
	Auth: {
		Cognito: {
			userPoolId: import.meta.env.VITE_COGNITO_USER_POOL_ID || '',
			userPoolClientId: import.meta.env.VITE_COGNITO_CLIENT_ID || '',
			identityPoolId: import.meta.env.VITE_COGNITO_IDENTITY_POOL_ID,
			loginWith: {
				email: true,
				oauth: {
					domain: import.meta.env.VITE_COGNITO_DOMAIN || '',
					scopes: ['openid', 'email', 'profile'],
					redirectSignIn: [
						import.meta.env.VITE_OAUTH_REDIRECT_SIGN_IN ||
							window.location.origin,
					],
					redirectSignOut: [
						import.meta.env.VITE_OAUTH_REDIRECT_SIGN_OUT ||
							window.location.origin,
					],
					responseType: 'code',
				},
			},
			signUpVerificationMethod: 'code' as const,
			userAttributes: {
				email: {
					required: true,
				},
			},
			passwordFormat: {
				minLength: 8,
				requireUppercase: true,
				requireLowercase: true,
				requireNumbers: true,
				requireSpecialCharacters: false,
			},
		},
	},
}

Amplify.configure(amplifyConfig)

// Initialize auth state after Amplify is configured
useAuthStore.getState().refreshAuth()

export default amplifyConfig
