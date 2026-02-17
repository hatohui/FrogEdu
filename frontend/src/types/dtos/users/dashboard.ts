export interface DailyUserCount {
	date: string
	count: number
}

export interface RoleDistributionItem {
	role: string
	count: number
	percentage: number
}

export interface VerificationStatusItem {
	status: string
	count: number
	percentage: number
}

export interface UserDashboardStatsResponse {
	userGrowthLast30Days: DailyUserCount[]
	roleDistribution: RoleDistributionItem[]
	verificationStatus: VerificationStatusItem[]
}
