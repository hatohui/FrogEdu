export interface AIUsageRecord {
	id: string
	userId: string
	actionType: string
	usedAt: string
	metadata: string | null
}

export interface AIUsageLimitInfo {
	userId: string
	plan: string
	usedCount: number
	maxAllowed: number | null
	remaining: number
	canUseAI: boolean
	isUnlimited: boolean
}
