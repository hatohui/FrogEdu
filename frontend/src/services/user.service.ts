import apiService, { type ApiResponse } from './api.service'

/**
 * User DTOs
 */
export interface UserProfile {
	id: string
	email: string
	name: string
	role: 'Teacher' | 'Student' | 'Admin'
	picture?: string
	bio?: string
	phoneNumber?: string
	school?: string
	grade?: number
	subject?: string
	createdAt: string
	updatedAt: string
	lastLoginAt?: string
}

export interface UpdateProfileRequest {
	name?: string
	picture?: string
	bio?: string
	phoneNumber?: string
	school?: string
	subject?: string
}

export interface ChangePasswordRequest {
	currentPassword: string
	newPassword: string
	confirmPassword: string
}

/**
 * User Service
 * Handles user profile and account management
 */
class UserService {
	private readonly baseUrl = '/api/user'

	/**
	 * Get current user profile
	 */
	async getProfile(): Promise<ApiResponse<UserProfile>> {
		return apiService.get<UserProfile>(`${this.baseUrl}/profile`)
	}

	/**
	 * Update user profile
	 */
	async updateProfile(
		updates: UpdateProfileRequest
	): Promise<ApiResponse<UserProfile>> {
		return apiService.put<UserProfile, UpdateProfileRequest>(
			`${this.baseUrl}/profile`,
			updates
		)
	}

	/**
	 * Upload profile picture
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
