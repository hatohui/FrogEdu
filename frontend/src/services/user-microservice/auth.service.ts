import type {
	VerifyEmailRequest,
	ForgotPasswordRequest,
	ResetPasswordRequest,
} from '@/types/dtos/users/verify-email'
import { ApiService, type ApiResponse } from '../api.service'

class AuthService extends ApiService {
	private readonly baseUrl = '/users/auth'

	async sendVerificationEmail(
		userId: string
	): Promise<ApiResponse<{ message: string }>> {
		return this.post<{ message: string }>(
			`${this.baseUrl}/send-verification-email/${userId}`
		)
	}

	async verifyEmail(token: string): Promise<ApiResponse<{ message: string }>> {
		return this.post<{ message: string }, VerifyEmailRequest>(
			`${this.baseUrl}/verify-email`,
			{ token }
		)
	}

	async forgotPassword(
		email: string
	): Promise<ApiResponse<{ message: string }>> {
		return this.post<{ message: string }, ForgotPasswordRequest>(
			`${this.baseUrl}/forgot-password`,
			{ email }
		)
	}

	async resetPassword(
		token: string,
		newPassword: string
	): Promise<ApiResponse<{ message: string }>> {
		return this.post<{ message: string }, ResetPasswordRequest>(
			`${this.baseUrl}/reset-password`,
			{ token, newPassword }
		)
	}

	async resendVerificationCode(
		email: string
	): Promise<ApiResponse<{ message: string }>> {
		return this.post<{ message: string }>(
			`${this.baseUrl}/resend-verification-code`,
			{ email }
		)
	}
}

const authService = new AuthService()
export default authService
