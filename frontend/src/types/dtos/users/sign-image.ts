export interface SignImageResponse {
	url: string
	key: string
}

export interface AvatarPresignedUrlResponse {
	uploadUrl: string
	avatarUrl: string
	expiresAt: string
}
