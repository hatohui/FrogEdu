export interface CreateClassResponse {
	classId: string
	name: string
	grade: string
	inviteCode: string
	maxStudents: number
	bannerUrl: string | null
	isActive: boolean
	teacherId: string
	createdAt: string
}

export interface JoinClassResponse {
	classId: string
	className: string
}

export interface AssignmentResponse {
	id: string
	classId: string
	examId: string
	startDate: string
	dueDate: string
	isMandatory: boolean
	weight: number
	isActive: boolean
	isOverdue: boolean
}
