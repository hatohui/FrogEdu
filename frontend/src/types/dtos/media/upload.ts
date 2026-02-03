/**
 * Result of file validation check
 */
export interface FileValidationResult {
	isValid: boolean
	error?: string
}

/**
 * Result of an upload operation
 */
export interface UploadResult {
	success: boolean
	url?: string
	error?: string
}

/**
 * State for media upload operations
 */
export interface MediaUploadState {
	isUploading: boolean
	preview: string | null
	error: string | null
}

/**
 * Configuration options for media upload
 */
export interface MediaUploadConfig {
	/** Maximum file size in bytes (default: 5MB) */
	maxSizeBytes?: number
	/** Allowed MIME types (default: common image types) */
	allowedTypes?: string[]
	/** Show toast notifications (default: true) */
	showToasts?: boolean
}

/**
 * Default media upload configuration
 */
export const DEFAULT_MEDIA_UPLOAD_CONFIG: Required<MediaUploadConfig> = {
	maxSizeBytes: 5 * 1024 * 1024, // 5MB
	allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
	showToasts: true,
}
