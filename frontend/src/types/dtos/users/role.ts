export interface Role {
	id: string
	name: 'Teacher' | 'Student' | 'Admin'
	description: string
}

export type RoleType = 'Teacher' | 'Student' | 'Admin'
