import type {
	ChangePasswordRequest,
	GetMeResponse,
	UpdateProfileDto,
} from '@/types/dtos/users/user'
import type { Role } from '@/types/dtos/users/role'
import apiService, { type ApiResponse } from '../api.service'
import axiosInstance, { publicAxios } from '../axios'
import type { GetPresignedImageUrlResponse } from '@/types/dtos/users/sign-image'
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

	async getAvatarUploadUrl(
		contentType: string
	): Promise<GetPresignedImageUrlResponse> {
		const query = new URLSearchParams({
			folder: AVATAR_BUCKET,
			contentType: contentType,
		}).toString()

		const response = await axiosInstance.get<GetPresignedImageUrlResponse>(
			`${this.baseUrl}/assets/sign-url?${query}`
		)

		return response.data
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
}

export default new UserService()
