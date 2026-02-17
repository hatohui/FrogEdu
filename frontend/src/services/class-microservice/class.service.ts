import type {
	ClassRoom,
	ClassDetail,
	ClassAssignment,
} from '@/types/model/class-service'
import type {
	AssignExamRequest,
	CreateClassRequest,
	CreateClassResponse,
	JoinClassRequest,
	JoinClassResponse,
	AssignmentResponse,
} from '@/types/dtos/classes'
import axiosInstance from '../axios'

const baseUrl = '/classes'

export const classService = {
	// ─── Student / Teacher / Admin (role-based) ───

	getMyClasses: async (): Promise<ClassRoom[]> => {
		const response = await axiosInstance.get<ClassRoom[]>(`${baseUrl}/classes`)
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

	// ─── Assignments ───

	assignExam: async (
		classId: string,
		data: AssignExamRequest
	): Promise<AssignmentResponse> => {
		const response = await axiosInstance.post<AssignmentResponse>(
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

	// ─── Admin endpoints ───

	adminAssignExam: async (
		classId: string,
		data: AssignExamRequest
	): Promise<AssignmentResponse> => {
		const response = await axiosInstance.post<AssignmentResponse>(
			`${baseUrl}/classes/admin/${classId}/assignments`,
			data
		)
		return response.data
	},
}
