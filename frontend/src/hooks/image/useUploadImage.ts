import userService from '@/services/user-microservice/user.service'
import { useQueryClient } from '@tanstack/react-query'
import { useState, useCallback } from 'react'
import { toast } from 'sonner'

export const useUploadImage = () => {
	const queryClient = useQueryClient()

	const [avatarPreview, setAvatarPreview] = useState<string | null>(null)
	const [isUploadingAvatar, setIsUploadingAvatar] = useState(false)

	const handleAvatarChange = useCallback(
		async (event: React.ChangeEvent<HTMLInputElement>) => {
			const file = event.target.files?.[0]
			if (!file) return

			if (!file.type.startsWith('image/')) {
				toast.error('Please select an image file')
				return
			}

			if (file.size > 5 * 1024 * 1024) {
				toast.error('Image must be less than 5MB')
				return
			}

			const reader = new FileReader()
			reader.onload = e => {
				setAvatarPreview(e.target?.result as string)
			}
			reader.readAsDataURL(file)

			setIsUploadingAvatar(true)
			try {
				await userService.uploadAvatar(file)
				queryClient.invalidateQueries({ queryKey: ['currentUser'] })
				toast.success('Avatar updated successfully')
			} catch (error) {
				toast.error('Failed to upload avatar')
				console.error('Avatar upload error:', error)
				setAvatarPreview(null)
			} finally {
				setIsUploadingAvatar(false)
			}
		},
		[queryClient]
	)

	return {
		avatarPreview,
		isUploadingAvatar,
		handleAvatarChange,
	}
}
