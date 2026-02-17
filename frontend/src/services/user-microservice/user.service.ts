import type {
	ChangePasswordRequest,
	GetMeResponse,
	UpdateProfileDto,
} from '@/types/dtos/users/user'
import type {
	GetAllUsersParams,
	PaginatedResponse,
	UpdateUserRequest,
	UserStatistics,
} from '@/types/dtos/users/admin'
import type { UserDashboardStatsResponse } from '@/types/dtos/users/dashboard'
import type { Role } from '@/types/model/user-service/role'
import apiService, { type ApiResponse } from '../api.service'
import axiosInstance, { publicAxios } from '../axios'
import type {
	GetPresignedImageUrlResponse,
	GetPresignedImageUrlParams,
} from '@/types/dtos/users/sign-image'
import { AVATAR_BUCKET } from '@/common/constants'

class UserService {
	private readonly baseUrl = '/users'

	async getCurrentUser(): Promise<GetMeResponse> {
		const response = await axiosInstance.get<GetMeResponse>(
			`${this.baseUrl}/me`
		)
		return response.data
	}

	async updateProfile(updates: UpdateProfileDto): Promise<void> {
		await axiosInstance.put(`${this.baseUrl}/me`, updates)
	}

	/**
	 * Get a presigned URL for uploading an image
	 * @param params - Upload parameters including folder and content type
	 */
	async getPresignedUploadUrl(
		params: GetPresignedImageUrlParams
	): Promise<GetPresignedImageUrlResponse> {
		const query = new URLSearchParams({
			folder: params.folder,
			contentType: params.contentType,
		}).toString()

		const response = await axiosInstance.get<GetPresignedImageUrlResponse>(
			`${this.baseUrl}/assets/sign-url?${query}`
		)

		return response.data
	}

	/**
	 * Get a presigned URL specifically for avatar upload
	 * @deprecated Use getPresignedUploadUrl with folder param instead
	 */
	async getAvatarUploadUrl(
		contentType: string
	): Promise<GetPresignedImageUrlResponse> {
		return this.getPresignedUploadUrl({
			folder: AVATAR_BUCKET,
			contentType,
		})
	}

	async confirmAvatarUpload(publicUrl: string): Promise<void> {
		await axiosInstance.put(`${this.baseUrl}/me/avatar`, {
			avatarUrl: publicUrl,
		})
	}

	async uploadAvatar(file: File): Promise<string> {
		const { uploadUrl, publicUrl } = await this.getAvatarUploadUrl(file.type)

		await publicAxios.put(uploadUrl, file, {
			headers: {
				'Content-Type': file.type,
			},
		})

		await this.confirmAvatarUpload(publicUrl)
		return publicUrl
	}

	async deleteAvatar(): Promise<void> {
		await axiosInstance.delete(`${this.baseUrl}/me/avatar`)
	}

	async getProfile(): Promise<ApiResponse<GetMeResponse>> {
		return apiService.get<GetMeResponse>(`${this.baseUrl}/profile`)
	}

	async createUserFromCognito(userData: {
		sub: string
		email: string
		givenName: string
		familyName: string
		customRole: string
		picture?: string
	}): Promise<void> {
		await publicAxios.post('/users/auth/webhook', {
			Request: {
				UserAttributes: {
					Sub: userData.sub,
					Email: userData.email,
					GivenName: userData.givenName,
					FamilyName: userData.familyName,
					CustomRole: userData.customRole,
					Picture: userData.picture,
				},
			},
		})
	}

	async changePassword(
		request: ChangePasswordRequest
	): Promise<ApiResponse<void>> {
		return apiService.post<void, ChangePasswordRequest>(
			`${this.baseUrl}/change-password`,
			request
		)
	}

	async deleteAccount(): Promise<ApiResponse<void>> {
		return apiService.delete<void>(`${this.baseUrl}/account`)
	}

	async sendVerificationEmail(userId: string): Promise<void> {
		await axiosInstance.post(
			`${this.baseUrl}/auth/send-verification-email/${userId}`
		)
	}

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

	async refreshToken(): Promise<ApiResponse<{ token: string }>> {
		return apiService.post<{ token: string }>(`${this.baseUrl}/refresh-token`)
	}

	async getRoles(): Promise<Role[]> {
		const response = await axiosInstance.get<Role[]>(`${this.baseUrl}/roles`)
		return response.data
	}

	async getRoleById(roleId: string): Promise<Role> {
		const response = await axiosInstance.get<Role>(
			`${this.baseUrl}/roles/${roleId}`
		)
		return response.data
	}

	// User management endpoints for admin dashboard
	async getAllUsers(
		params?: GetAllUsersParams
	): Promise<PaginatedResponse<GetMeResponse>> {
		const query = new URLSearchParams()
		if (params?.page) query.append('page', params.page.toString())
		if (params?.pageSize) query.append('pageSize', params.pageSize.toString())
		if (params?.search) query.append('search', params.search)
		if (params?.role) query.append('role', params.role)
		if (params?.sortBy) query.append('sortBy', params.sortBy)
		if (params?.sortOrder) query.append('sortOrder', params.sortOrder)

		const response = await axiosInstance.get<PaginatedResponse<GetMeResponse>>(
			`${this.baseUrl}/users?${query.toString()}`
		)
		return response.data
	}

	async getUserStatistics(): Promise<UserStatistics> {
		const response = await axiosInstance.get<UserStatistics>(
			`${this.baseUrl}/users/statistics`
		)
		return response.data
	}

	async getUserDashboardStats(): Promise<UserDashboardStatsResponse> {
		const response = await axiosInstance.get<UserDashboardStatsResponse>(
			`${this.baseUrl}/users/dashboard-stats`
		)
		return response.data
	}

	async getUserById(userId: string): Promise<GetMeResponse> {
		const response = await axiosInstance.get<GetMeResponse>(
			`${this.baseUrl}/users/${userId}`
		)
		return response.data
	}

	async updateUser(userId: string, updates: UpdateUserRequest): Promise<void> {
		await axiosInstance.put(`${this.baseUrl}/users/${userId}`, updates)
	}

	async deleteUser(userId: string): Promise<void> {
		await axiosInstance.delete(`${this.baseUrl}/users/${userId}`)
	}

	async sendPasswordResetEmail(email: string): Promise<ApiResponse<void>> {
		return apiService.post(`${this.baseUrl}/auth/send-password-reset`, {
			email,
		})
	}
}

export default new UserService()
