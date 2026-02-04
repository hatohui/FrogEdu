import { ApiService, type ApiResponse } from './api.service'

export interface AnalyticsMetrics {
	totalUsers: number
	activeUsers: number
	totalExams: number
	completionRate: number
	averageScore: number
}

export interface UserGrowthData {
	month: string
	users: number
	active: number
	newUsers: number
}

export interface ExamPerformanceData {
	month: string
	avgScore: number
	submissions: number
}

export interface ActivityData {
	day: string
	logins: number
	examsTaken: number
	questionsCreated: number
}

export interface RoleDistribution {
	role: string
	count: number
	percentage: number
}

class AnalyticsService extends ApiService {
	private readonly baseUrl = '/api/analytics'

	async getOverviewMetrics(): Promise<ApiResponse<AnalyticsMetrics>> {
		return this.get<AnalyticsMetrics>(`${this.baseUrl}/metrics/overview`)
	}

	async getUserGrowth(
		timeRange: '7d' | '30d' | '90d' | '1y' = '30d'
	): Promise<ApiResponse<UserGrowthData[]>> {
		return this.get<UserGrowthData[]>(`${this.baseUrl}/users/growth`, {
			timeRange,
		})
	}

	async getExamPerformance(
		timeRange: '7d' | '30d' | '90d' | '1y' = '30d'
	): Promise<ApiResponse<ExamPerformanceData[]>> {
		return this.get<ExamPerformanceData[]>(
			`${this.baseUrl}/exams/performance`,
			{
				timeRange,
			}
		)
	}

	async getActivityData(
		timeRange: '7d' | '30d' | '90d' | '1y' = '7d'
	): Promise<ApiResponse<ActivityData[]>> {
		return this.get<ActivityData[]>(`${this.baseUrl}/activity`, {
			timeRange,
		})
	}

	async getRoleDistribution(): Promise<ApiResponse<RoleDistribution[]>> {
		return this.get<RoleDistribution[]>(`${this.baseUrl}/users/distribution`)
	}
}

const analyticsService = new AnalyticsService()
export default analyticsService
