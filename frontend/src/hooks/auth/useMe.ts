import userService from '@/services/user.service'
import { useQuery } from '@tanstack/react-query'

export const useMe = () =>
	useQuery({
		queryKey: ['currentUser'],
		queryFn: () => userService.getCurrentUser(),
		staleTime: 5 * 60 * 1000,
		retry: 1,
	})
