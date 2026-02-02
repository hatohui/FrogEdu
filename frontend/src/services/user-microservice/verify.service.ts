import axiosInstance from '../axios'

export async function resendVerificationEmail(userId: string): Promise<void> {
	await axiosInstance.post(`/users/auth/send-verification-email/${userId}`)
}
