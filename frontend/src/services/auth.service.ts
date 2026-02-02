import { ApiService, type ApiResponse } from './api.service'

/**
 * Request DTOs
 */
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

/**
 * Authentication Service
 * Handles email verification and password reset operations
 */
class AuthService extends ApiService {
	private readonly baseUrl = '/auth'

	/**
	 * Send verification email to a user
	 */
	async sendVerificationEmail(
		userId: string
	): Promise<ApiResponse<{ message: string }>> {
		return this.post<{ message: string }>(
			`${this.baseUrl}/send-verification-email/${userId}`
		)
	}

	/**
	 * Verify email with token
	 */
	async verifyEmail(token: string): Promise<ApiResponse<{ message: string }>> {
		return this.post<{ message: string }, VerifyEmailRequest>(
			`${this.baseUrl}/verify-email`,
			{ token }
		)
	}

	/**
	 * Request password reset email
	 */
	async forgotPassword(
		email: string
	): Promise<ApiResponse<{ message: string }>> {
		return this.post<{ message: string }, ForgotPasswordRequest>(
			`${this.baseUrl}/forgot-password`,
			{ email }
		)
	}

	/**
	 * Reset password with token
	 */
	async resetPassword(
		token: string,
		newPassword: string
	): Promise<ApiResponse<{ message: string }>> {
		return this.post<{ message: string }, ResetPasswordRequest>(
			`${this.baseUrl}/reset-password`,
			{ token, newPassword }
		)
	}

	/**
	 * Resend verification code (for Amplify-based confirmation)
	 */
	async resendVerificationCode(
		email: string
	): Promise<ApiResponse<{ message: string }>> {
		return this.post<{ message: string }>(
			`${this.baseUrl}/resend-verification-code`,
			{ email }
		)
	}
}

// Export singleton instance
const authService = new AuthService()
export default authService
