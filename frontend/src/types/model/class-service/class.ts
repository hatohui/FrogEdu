export interface ClassRoom {
	id: string
	name: string
	grade: string
	inviteCode: string
	maxStudents: number
	bannerUrl: string | null
	isActive: boolean
	teacherId: string
	createdAt: string
	studentCount: number
	assignmentCount: number
}

export interface ClassEnrollment {
	id: string
	studentId: string
	studentFirstName: string
	studentLastName: string
	studentAvatarUrl: string | null
	joinedAt: string
	status: EnrollmentStatus
}

export interface ClassAssignment {
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

export type EnrollmentStatus = 'Active' | 'Inactive' | 'Kicked' | 'Withdrawn'

export interface ClassDetail {
	id: string
	name: string
	grade: string
	inviteCode: string
	maxStudents: number
	bannerUrl: string | null
	isActive: boolean
	teacherId: string
	createdAt: string
	studentCount: number
	enrollments: ClassEnrollment[]
	assignments: ClassAssignment[]
}
