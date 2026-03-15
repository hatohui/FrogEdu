export interface BadgeDto {
	id: string
	name: string
	description: string
	iconUrl: string
	badgeType: string
	requiredScore: number
	isActive: boolean
}

export interface StudentBadgeDto {
	id: string
	studentId: string
	badgeId: string
	classId: string
	examSessionId: string | null
	awardedByTeacherId: string | null
	customPraise: string | null
	awardedAt: string
	badgeName: string
	badgeDescription: string
	badgeIconUrl: string
	badgeType: string
}

export interface AwardBadgeRequest {
	studentId: string
	badgeId: string
	classId: string
	examSessionId?: string
	customPraise?: string
}
