export interface PaginatedResponse<T> {
	items: T[]
	total: number
	page: number
	pageSize: number
	totalPages: number
}

export interface UserStatistics {
	totalUsers: number
	totalAdmins: number
	totalTeachers: number
	totalStudents: number
	verifiedUsers: number
	unverifiedUsers: number
	usersCreatedLast30Days: number
	usersCreatedLast7Days: number
}

export interface UpdateUserRequest {
	firstName?: string
	lastName?: string
	roleId?: string
}

export interface GetAllUsersParams {
	page?: number
	pageSize?: number
	search?: string
	role?: string
	sortBy?: string
	sortOrder?: 'asc' | 'desc'
}
