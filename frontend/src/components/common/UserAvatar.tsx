import { Avatar, AvatarFallback, AvatarImage } from '../ui/avatar'
import type { GetMeResponse } from '@/types/dtos/users/user'

const UserAvatar = ({
	user,
	avatarPreview,
	size = 'h-10 w-10',
}: {
	user?: GetMeResponse
	avatarPreview?: string | null
	size?: string
}) => {
	const getUserInitials = () => {
		if (!user) return ''
		const first = user.firstName ? user.firstName.charAt(0) : ''
		const last = user.lastName ? user.lastName.charAt(0) : ''
		const initials = (first + last).toUpperCase()
		if (initials) return initials
		if (user.email && user.email.length > 0)
			return user.email.charAt(0).toUpperCase()
		return ''
	}

	return (
		<Avatar className={size}>
			<AvatarImage
				src={
					avatarPreview !== null ? avatarPreview : user?.avatarUrl || undefined
				}
				alt={user?.firstName || user?.lastName || 'User'}
			/>
			<AvatarFallback
				className={`bg-primary text-primary-foreground flex items-center justify-center font-semibold ${size}`}
			>
				{getUserInitials()}
			</AvatarFallback>
		</Avatar>
	)
}

export default UserAvatar
