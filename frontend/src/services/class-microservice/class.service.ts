import type {
	ClassRoom,
	ClassDetail,
	ClassAssignment,
} from '@/types/model/class-service'
import type {
	AssignExamRequest,
	CreateClassResponse,
	JoinClassResponse,
	AssignmentResponse,
} from '@/types/dtos/classes'
import axiosInstance from '../axios'

// Legacy DTOs kept for backward compatibility with existing components
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
	description: string
	grade: string
	maxStudents: number
	bannerUrl?: string
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
	homeroomTeacherId?: string
	studentCount: number
	maxStudents?: number
	inviteCode?: string
	inviteCodeExpiresAt?: string
	isArchived: boolean
	createdAt: string
	members: ClassMemberDto[]
}

export interface DashboardStatsDto {
	classCount: number
	examCount: number
	studentCount: number
	contentItemCount: number
	totalClasses: number
	totalStudents: number
	activeClasses: number
	archivedClasses: number
}

export {
	type CreateClassResponse,
	type JoinClassResponse,
	type AssignmentResponse,
}

export interface RegenerateCodeResponse {
	inviteCode: string
	expiresAt: string
}

// Class Service API calls
export const classService = {
	// ─── User-facing endpoints ───

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

	// ─── Typed endpoints (new) ───

	getMyClassesTyped: async (): Promise<ClassRoom[]> => {
		const response = await axiosInstance.get<ClassRoom[]>('/classes')
		return response.data
	},

	getClassDetailTyped: async (id: string): Promise<ClassDetail> => {
		const response = await axiosInstance.get<ClassDetail>(`/classes/${id}`)
		return response.data
	},

	assignExam: async (
		classId: string,
		data: AssignExamRequest
	): Promise<AssignmentResponse> => {
		const response = await axiosInstance.post<AssignmentResponse>(
			`/classes/${classId}/assignments`,
			data
		)
		return response.data
	},

	getClassAssignments: async (classId: string): Promise<ClassAssignment[]> => {
		const response = await axiosInstance.get<ClassAssignment[]>(
			`/classes/${classId}/assignments`
		)
		return response.data
	},

	// ─── Admin endpoints ───

	adminGetAllClasses: async (): Promise<ClassRoom[]> => {
		const response = await axiosInstance.get<ClassRoom[]>('/classes/admin/all')
		return response.data
	},

	adminGetClassDetail: async (id: string): Promise<ClassDetail> => {
		const response = await axiosInstance.get<ClassDetail>(
			`/classes/admin/${id}`
		)
		return response.data
	},

	adminAssignExam: async (
		classId: string,
		data: AssignExamRequest
	): Promise<AssignmentResponse> => {
		const response = await axiosInstance.post<AssignmentResponse>(
			`/classes/admin/${classId}/assignments`,
			data
		)
		return response.data
	},
}
