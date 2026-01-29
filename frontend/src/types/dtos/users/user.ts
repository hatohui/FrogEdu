import type { Role } from './role'

export interface GetMeResponse {
	id: string
	cognitoId: string
	email: string
	firstName: string
	lastName: string
	role: Role
	avatarUrl: string | null
	isEmailVerified: boolean
	lastLoginAt: string | null
	createdAt: string
	updatedAt: string
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

export interface UpdateProfileDto {
	firstName: string
	lastName: string
}
