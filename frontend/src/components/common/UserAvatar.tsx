import { Avatar, AvatarFallback, AvatarImage } from '../ui/avatar'
import type { GetMeResponse } from '@/types/dtos/users/user'

const UserAvatar = ({ user }: { user?: GetMeResponse }) => {
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
		<Avatar className='h-10 w-10'>
			<AvatarImage
				src={user?.avatarUrl || undefined}
				alt={user?.firstName || user?.lastName || 'User'}
			/>
			<AvatarFallback className='bg-primary text-primary-foreground'>
				{getUserInitials()}
			</AvatarFallback>
		</Avatar>
	)
}

export default UserAvatar
