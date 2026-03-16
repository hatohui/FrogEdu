import type {
	ClassRoom,
	ClassDetail,
	ClassAssignment,
} from '@/types/model/class-service'
import type { ExamSession } from '@/types/model/class-service'
import type {
	AssignExamRequest,
	ClassDashboardStatsResponse,
	CreateClassRequest,
	UpdateClassRequest,
	CreateClassResponse,
	JoinClassRequest,
	JoinClassResponse,
	BadgeDto,
	StudentBadgeDto,
	AwardBadgeRequest,
} from '@/types/dtos/classes'
import axiosInstance from '../axios'

const baseUrl = '/classes'

export const classService = {
	// ─── Student / Teacher / Admin (role-based) ───

	getMyClasses: async (role?: string): Promise<ClassRoom[]> => {
		const response = await axiosInstance.get<ClassRoom[]>(
			`${baseUrl}/classes`,
			{
				params: role ? { role } : undefined,
			}
		)
		return response.data
	},

	getClassDetail: async (id: string): Promise<ClassDetail> => {
		const response = await axiosInstance.get<ClassDetail>(
			`${baseUrl}/classes/${id}`
		)
		return response.data
	},

	createClass: async (
		data: CreateClassRequest
	): Promise<CreateClassResponse> => {
		const response = await axiosInstance.post<CreateClassResponse>(
			`${baseUrl}/classes`,
			data
		)
		return response.data
	},

	updateClass: async (
		classId: string,
		data: UpdateClassRequest
	): Promise<void> => {
		await axiosInstance.put(`${baseUrl}/classes/${classId}`, data)
	},

	joinClass: async (data: JoinClassRequest): Promise<JoinClassResponse> => {
		const response = await axiosInstance.post<JoinClassResponse>(
			`${baseUrl}/classes/join`,
			data
		)
		return response.data
	},

	removeStudent: async (classId: string, studentId: string): Promise<void> => {
		await axiosInstance.delete(
			`${baseUrl}/classes/${classId}/students/${studentId}`
		)
	},

	reinviteStudent: async (
		classId: string,
		studentId: string
	): Promise<void> => {
		await axiosInstance.post(
			`${baseUrl}/classes/${classId}/students/${studentId}/reinvite`
		)
	},

	acceptReinvite: async (classId: string): Promise<void> => {
		await axiosInstance.post(`${baseUrl}/classes/${classId}/reinvite/accept`)
	},

	// ─── Assignments ───

	assignExam: async (
		classId: string,
		data: AssignExamRequest
	): Promise<ExamSession> => {
		const response = await axiosInstance.post<ExamSession>(
			`${baseUrl}/classes/${classId}/assignments`,
			data
		)
		return response.data
	},

	getClassAssignments: async (classId: string): Promise<ClassAssignment[]> => {
		const response = await axiosInstance.get<ClassAssignment[]>(
			`${baseUrl}/classes/${classId}/assignments`
		)
		return response.data
	},

	updateAssignment: async (
		classId: string,
		assignmentId: string,
		data: import('@/types/dtos/classes').UpdateAssignmentRequest
	): Promise<import('@/types/model/class-service').ClassAssignment> => {
		const response = await axiosInstance.put(
			`${baseUrl}/classes/${classId}/assignments/${assignmentId}`,
			data
		)
		return response.data
	},

	deleteAssignment: async (
		classId: string,
		assignmentId: string
	): Promise<void> => {
		await axiosInstance.delete(
			`${baseUrl}/classes/${classId}/assignments/${assignmentId}`
		)
	},

	// ─── Admin endpoints ───

	adminAssignExam: async (
		classId: string,
		data: AssignExamRequest
	): Promise<ExamSession> => {
		const response = await axiosInstance.post<ExamSession>(
			`${baseUrl}/classes/admin/${classId}/assignments`,
			data
		)
		return response.data
	},

	getClassDashboardStats: async (): Promise<ClassDashboardStatsResponse> => {
		const response = await axiosInstance.get<ClassDashboardStatsResponse>(
			`${baseUrl}/classes/dashboard/stats`
		)
		return response.data
	},

	// ─── Gamification / Badges ───

	getBadges: async (activeOnly = true): Promise<BadgeDto[]> => {
		const response = await axiosInstance.get<BadgeDto[]>(`${baseUrl}/badges`, {
			params: { activeOnly },
		})
		return response.data
	},

	getStudentBadges: async (
		studentId: string,
		classId?: string
	): Promise<StudentBadgeDto[]> => {
		const response = await axiosInstance.get<StudentBadgeDto[]>(
			`${baseUrl}/students/${studentId}/badges`,
			{ params: classId ? { classId } : undefined }
		)
		return response.data
	},

	getMyBadges: async (classId?: string): Promise<StudentBadgeDto[]> => {
		const response = await axiosInstance.get<StudentBadgeDto[]>(
			`${baseUrl}/badges/me`,
			{ params: classId ? { classId } : undefined }
		)
		return response.data
	},

	awardBadge: async (data: AwardBadgeRequest): Promise<void> => {
		await axiosInstance.post(`${baseUrl}/badges/award`, data)
	},
}
