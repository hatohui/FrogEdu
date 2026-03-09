import { useMe } from '@/hooks/auth/useMe'
import { useViewAsStore } from '@/stores/viewAsStore'

/**
 * Returns the effective role to use for UI rendering.
 * Admins can override their role to preview the UI as Teacher or Student.
 */
export const useEffectiveRole = () => {
	const { user } = useMe()
	const viewAs = useViewAsStore(state => state.viewAs)

	const actualRole = user?.role?.name ?? null
	const isActualAdmin = actualRole === 'Admin'

	// Only apply override when the actual user is an Admin
	const effectiveRole = isActualAdmin && viewAs ? viewAs : actualRole

	return {
		effectiveRole,
		actualRole,
		isActualAdmin,
		isViewingAs: isActualAdmin && viewAs !== null,
		viewAs,
		isAdmin: effectiveRole === 'Admin',
		isTeacher: effectiveRole === 'Teacher',
		isStudent: effectiveRole === 'Student',
	}
}
