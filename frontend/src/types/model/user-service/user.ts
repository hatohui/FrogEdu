import type { GetMeResponse } from '@/types/dtos/users/user'
import type { Role } from './role'

/**
 * User data extended with role information
 */
export interface UserWithRole extends GetMeResponse {
	role?: Role
}
