import { useState, useCallback } from 'react'
import { toast } from 'sonner'
import type {
	FileValidationResult,
	UploadResult,
	MediaUploadState,
	MediaUploadConfig,
} from '@/types/dtos/media/upload'
import { DEFAULT_MEDIA_UPLOAD_CONFIG } from '@/types/dtos/media/upload'

/**
 * Generic hook for media file upload with preview
 * Can be used for question media, profile avatars, etc.
 */
export const useMediaUpload = (config: MediaUploadConfig = {}) => {
	const mergedConfig = { ...DEFAULT_MEDIA_UPLOAD_CONFIG, ...config }

	const [state, setState] = useState<MediaUploadState>({
		isUploading: false,
		preview: null,
		error: null,
	})

	/**
	 * Validate file against configured constraints
	 */
	const validateFile = useCallback(
		(file: File): FileValidationResult => {
			// Check file type
			if (mergedConfig.allowedTypes.length > 0) {
				if (!mergedConfig.allowedTypes.includes(file.type)) {
					return {
						isValid: false,
						error: `File type ${file.type} not allowed. Allowed: ${mergedConfig.allowedTypes.join(', ')}`,
					}
				}
			}

			// Check file size
			if (file.size > mergedConfig.maxSizeBytes) {
				const maxSizeMB = Math.round(mergedConfig.maxSizeBytes / (1024 * 1024))
				return {
					isValid: false,
					error: `File size exceeds ${maxSizeMB}MB limit`,
				}
			}

			return { isValid: true }
		},
		[mergedConfig.allowedTypes, mergedConfig.maxSizeBytes]
	)

	/**
	 * Generate a preview data URL from a file
	 */
	const generatePreview = useCallback((file: File): Promise<string> => {
		return new Promise((resolve, reject) => {
			const reader = new FileReader()
			reader.onload = e => {
				const result = e.target?.result
				if (typeof result === 'string') {
					resolve(result)
				} else {
					reject(new Error('Failed to generate preview'))
				}
			}
			reader.onerror = () => reject(new Error('Failed to read file'))
			reader.readAsDataURL(file)
		})
	}, [])

	/**
	 * Upload a media file
	 * @param file - The file to upload
	 * @param uploadFn - Custom upload function that takes a file and returns the public URL
	 */
	const uploadMedia = useCallback(
		async (
			file: File,
			uploadFn?: (file: File) => Promise<string>
		): Promise<UploadResult> => {
			// Validate the file
			const validation = validateFile(file)
			if (!validation.isValid) {
				if (mergedConfig.showToasts) {
					toast.error(validation.error)
				}
				setState(prev => ({ ...prev, error: validation.error ?? null }))
				return { success: false, error: validation.error }
			}

			setState(prev => ({ ...prev, isUploading: true, error: null }))

			try {
				// Generate preview
				const preview = await generatePreview(file)
				setState(prev => ({ ...prev, preview }))

				// Upload if function provided
				let url: string | undefined
				if (uploadFn) {
					url = await uploadFn(file)
				} else {
					// TODO: Implement default upload via presigned URL when backend supports it
					// For now, just use the preview as URL (development mode)
					console.warn(
						'[useMediaUpload] No upload function provided. Using preview as placeholder.'
					)
					url = preview
				}

				if (mergedConfig.showToasts) {
					toast.success('File uploaded successfully')
				}

				setState(prev => ({ ...prev, isUploading: false }))
				return { success: true, url }
			} catch (error) {
				const errorMessage =
					error instanceof Error ? error.message : 'Upload failed'
				if (mergedConfig.showToasts) {
					toast.error(errorMessage)
				}
				setState(prev => ({
					...prev,
					isUploading: false,
					preview: null,
					error: errorMessage,
				}))
				return { success: false, error: errorMessage }
			}
		},
		[validateFile, generatePreview, mergedConfig.showToasts]
	)

	/**
	 * Handle file input change event
	 * @param uploadFn - Custom upload function
	 */
	const handleFileChange = useCallback(
		(uploadFn?: (file: File) => Promise<string>) => {
			return async (
				event: React.ChangeEvent<HTMLInputElement>
			): Promise<UploadResult> => {
				const file = event.target.files?.[0]
				if (!file) {
					return { success: false, error: 'No file selected' }
				}
				return uploadMedia(file, uploadFn)
			}
		},
		[uploadMedia]
	)

	/**
	 * Clear the current preview and error state
	 */
	const clearPreview = useCallback(() => {
		setState({
			isUploading: false,
			preview: null,
			error: null,
		})
	}, [])

	return {
		...state,
		uploadMedia,
		handleFileChange,
		validateFile,
		generatePreview,
		clearPreview,
	}
}
