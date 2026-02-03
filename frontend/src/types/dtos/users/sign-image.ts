/**
 * Request parameters for getting a presigned image upload URL
 */
export interface GetPresignedImageUrlParams {
	/** Target folder/bucket for the upload (e.g., 'avatars', 'questions', 'media') */
	folder: string
	/** MIME type of the file to upload */
	contentType: string
}

export interface SignImageResponse {
	url: string
	key: string
}

export interface GetPresignedImageUrlResponse {
	uploadUrl: string
	publicUrl: string
	expiresAt: string
}
