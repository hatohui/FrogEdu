import axiosInstance from './axios'

// DTOs matching backend
export interface ClassDto {
	id: string
	name: string
	subject?: string
	grade: number
	school?: string
	description?: string
	teacherId: string
	teacherName: string
	studentCount: number
	maxStudents?: number
	inviteCode?: string
	inviteCodeExpiresAt?: string
	isArchived: boolean
	createdAt: string
}

export interface CreateClassDto {
	name: string
	subject?: string
	grade: number
	school?: string
	description?: string
	maxStudents?: number
}

export interface JoinClassDto {
	inviteCode: string
}

export interface ClassMemberDto {
	userId: string
	name: string
	email: string
	role: 'Teacher' | 'Student'
	avatarUrl?: string
	joinedAt: string
}

export interface ClassDetailsDto {
	id: string
	name: string
	subject?: string
	grade: number
	school?: string
	description?: string
	teacherId: string
	teacherName: string
	studentCount: number
	maxStudents?: number
	inviteCode?: string
	inviteCodeExpiresAt?: string
	isArchived: boolean
	createdAt: string
	members: ClassMemberDto[]
}

export interface DashboardStatsDto {
	totalClasses: number
	totalStudents: number
	activeClasses: number
	archivedClasses: number
}

export interface CreateClassResponse {
	classId: string
	inviteCode: string
}

export interface JoinClassResponse {
	classId: string
	className: string
}

export interface RegenerateCodeResponse {
	inviteCode: string
	expiresAt: string
}

// Class Service API calls
export const classService = {
	getMyClasses: async (): Promise<ClassDto[]> => {
		const response = await axiosInstance.get<ClassDto[]>('/classes')
		return response.data
	},

	getClassDetails: async (id: string): Promise<ClassDetailsDto> => {
		const response = await axiosInstance.get<ClassDetailsDto>(`/classes/${id}`)
		return response.data
	},

	createClass: async (data: CreateClassDto): Promise<CreateClassResponse> => {
		const response = await axiosInstance.post<CreateClassResponse>(
			'/classes',
			data
		)
		return response.data
	},

	joinClass: async (data: JoinClassDto): Promise<JoinClassResponse> => {
		const response = await axiosInstance.post<JoinClassResponse>(
			'/classes/join',
			data
		)
		return response.data
	},

	regenerateInviteCode: async (
		classId: string,
		expiresInDays?: number
	): Promise<RegenerateCodeResponse> => {
		const response = await axiosInstance.post<RegenerateCodeResponse>(
			`/classes/${classId}/regenerate-code`,
			{ expiresInDays }
		)
		return response.data
	},

	getDashboardStats: async (): Promise<DashboardStatsDto> => {
		const response = await axiosInstance.get<DashboardStatsDto>(
			'/classes/dashboard/stats'
		)
		return response.data
	},
}
