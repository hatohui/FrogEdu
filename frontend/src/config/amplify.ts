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
