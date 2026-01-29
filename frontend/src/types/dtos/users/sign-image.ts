export interface SignImageResponse {
	url: string
	key: string
}

export interface GetPresignedImageUrlResponse {
	uploadUrl: string
	publicUrl: string
	expiresAt: string
}
