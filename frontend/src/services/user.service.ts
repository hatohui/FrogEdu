import type {
	ChangePasswordRequest,
	GetMeResponse,
	UpdateProfileDto,
} from '@/types/dtos/users/user'
import apiService, { type ApiResponse } from './api.service'
import axiosInstance from './axios'
import type { AvatarPresignedUrlResponse } from '@/types/dtos/users/sign-image'

/**
 * User Service
 * Handles user profile and account management
 */
class UserService {
	private readonly baseUrl = '/users'

	/**
	 * Get current user profile from backend
	 */
	async getCurrentUser(): Promise<GetMeResponse> {
		const response = await axiosInstance.get<GetMeResponse>(
			`${this.baseUrl}/me`
		)
		return response.data
	}

	/**
	 * Update user profile
	 */
	async updateProfile(updates: UpdateProfileDto): Promise<void> {
		await axiosInstance.put(`${this.baseUrl}/me`, updates)
	}

	/**
	 * Get presigned URL for avatar upload
	 */
	async getAvatarUploadUrl(
		contentType: string
	): Promise<AvatarPresignedUrlResponse> {
		const response = await axiosInstance.post<AvatarPresignedUrlResponse>(
			`${this.baseUrl}/me/avatar`,
			{ contentType }
		)
		return response.data
	}

	/**
	 * Confirm avatar upload (after uploading to S3)
	 */
	async confirmAvatarUpload(avatarUrl: string): Promise<void> {
		await axiosInstance.put(`${this.baseUrl}/me/avatar`, { avatarUrl })
	}

	/**
	 * Upload avatar file to S3 using presigned URL
	 */
	async uploadAvatar(file: File): Promise<string> {
		// Get presigned URL
		const { uploadUrl, avatarUrl } = await this.getAvatarUploadUrl(file.type)

		// Upload to S3
		await fetch(uploadUrl, {
			method: 'PUT',
			body: file,
			headers: {
				'Content-Type': file.type,
			},
		})

		// Confirm upload
		await this.confirmAvatarUpload(avatarUrl)

		return avatarUrl
	}

	/**
	 * Get current user profile (legacy)
	 */
	async getProfile(): Promise<ApiResponse<GetMeResponse>> {
		return apiService.get<GetMeResponse>(`${this.baseUrl}/profile`)
	}

	/**
	 * Create user in database after Cognito signup
	 * Used for webhook-like functionality from frontend
	 */
	async createUserFromCognito(userData: {
		sub: string
		email: string
		givenName: string
		familyName: string
		customRole: string
	}): Promise<void> {
		// Note: This endpoint doesn't require auth token (AllowAnonymous)
		// So we use axiosInstance directly but the interceptor won't add auth
		await axiosInstance.post('/users/auth/webhook', {
			Request: {
				UserAttributes: {
					Sub: userData.sub,
					Email: userData.email,
					GivenName: userData.givenName,
					FamilyName: userData.familyName,
					CustomRole: userData.customRole,
				},
			},
		})
	}

	/**
	 * Upload profile picture (legacy)
	 * Returns presigned S3 URL for upload
	 */
	async getProfilePictureUploadUrl(
		fileName: string
	): Promise<ApiResponse<{ uploadUrl: string; downloadUrl: string }>> {
		return apiService.post<{ uploadUrl: string; downloadUrl: string }>(
			`${this.baseUrl}/profile/picture/upload-url`,
			{ fileName }
		)
	}

	/**
	 * Change password
	 */
	async changePassword(
		request: ChangePasswordRequest
	): Promise<ApiResponse<void>> {
		return apiService.post<void, ChangePasswordRequest>(
			`${this.baseUrl}/change-password`,
			request
		)
	}

	/**
	 * Delete account
	 */
	async deleteAccount(): Promise<ApiResponse<void>> {
		return apiService.delete<void>(`${this.baseUrl}/account`)
	}

	/**
	 * Get user statistics (for dashboard)
	 */
	async getStatistics(): Promise<
		ApiResponse<{
			totalClasses: number
			totalExams: number
			totalStudents: number
			recentActivity: Array<{
				id: string
				action: string
				timestamp: string
			}>
		}>
	> {
		return apiService.get(`${this.baseUrl}/statistics`)
	}

	/**
	 * Refresh token (if needed)
	 */
	async refreshToken(): Promise<ApiResponse<{ token: string }>> {
		return apiService.post<{ token: string }>(`${this.baseUrl}/refresh-token`)
	}
}

export default new UserService()
