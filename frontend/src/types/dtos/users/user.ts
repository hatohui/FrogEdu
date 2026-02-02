export interface GetMeResponse {
	id: string
	cognitoId: string
	email: string
	firstName: string
	lastName: string
	roleId: string
	avatarUrl: string | null
	isEmailVerified: boolean
	createdAt: string
	updatedAt: string | null
}

export interface UserSummaryDto {
	id: string
	email: string
	fullName: string
	roleId: string
	avatarUrl: string | null
}

export interface UpdateProfileRequest {
	firstName: string
	lastName: string
}

export interface ChangePasswordRequest {
	currentPassword: string
	newPassword: string
	confirmPassword: string
}

export interface UpdateProfileDto {
	firstName: string
	lastName: string
}
