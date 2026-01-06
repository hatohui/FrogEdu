import 'react-i18next'

declare module 'react-i18next' {
	interface CustomTypeOptions {
		defaultNS: 'translation'
		resources: {
			translation: {
				welcome: string
				login: string
				logout: string
				signup: string
				username: string
				password: string
				email: string
				submit: string
				cancel: string
				profile: string
				settings: string
				home: string
			}
		}
	}
}
