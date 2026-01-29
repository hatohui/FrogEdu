import { resendSignUpCode } from 'aws-amplify/auth'

export async function resendVerificationEmail(email: string): Promise<void> {
	await resendSignUpCode({ username: email })
}
