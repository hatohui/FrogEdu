import type { SignImageResponse } from '@/types/dtos/users/sign-image'
import axios from './axios'

export const ImageService = {
	getSignedUrl: (folder: string) =>
		axios
			.get<SignImageResponse>('/images/sign-url', { params: { folder } })
			.then(res => res.data),
}
