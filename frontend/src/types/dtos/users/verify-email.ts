export interface SendVerificationEmailRequest {
	userId: string
}

export interface VerifyEmailRequest {
	token: string
}

export interface ForgotPasswordRequest {
	email: string
}

export interface ResetPasswordRequest {
	token: string
	newPassword: string
}
