import { AxiosError } from 'axios'
import axiosInstance from './axios'

/**
 * API Error Response Type
 * Follows RFC 7807 Problem Details standard
 */
export interface ApiErrorResponse {
	type?: string
	title?: string
	status?: number
	detail?: string
	instance?: string
	errors?: Record<string, string[]>
}

/**
 * Generic API Response Wrapper
 */
export interface ApiResponse<T> {
	success: boolean
	data?: T
	error?: ApiErrorResponse
	statusCode: number
}

/**
 * Pagination Metadata
 */
export interface PaginationMeta {
	total: number
	page: number
	pageSize: number
	totalPages: number
	hasNextPage: boolean
	hasPreviousPage: boolean
}

/**
 * Paginated Response
 */
export interface PaginatedResponse<T> {
	items: T[]
	meta: PaginationMeta
}

/**
 * API Error with response metadata
 */
interface ApiError extends Error {
	response?: ApiErrorResponse
}

/**
 * Base API service class with common operations
 */
export class ApiService {
	/**
	 * GET request
	 */
	async get<T>(
		url: string,
		params?: Record<string, unknown>
	): Promise<ApiResponse<T>> {
		try {
			const response = await axiosInstance.get<T>(url, { params })
			return {
				success: true,
				data: response.data,
				statusCode: response.status,
			}
		} catch (error) {
			return this.handleError(error)
		}
	}

	/**
	 * POST request
	 */
	async post<T, D = unknown>(
		url: string,
		data?: D,
		params?: Record<string, unknown>
	): Promise<ApiResponse<T>> {
		try {
			const response = await axiosInstance.post<T>(url, data, { params })
			return {
				success: true,
				data: response.data,
				statusCode: response.status,
			}
		} catch (error) {
			return this.handleError(error)
		}
	}

	/**
	 * PUT request
	 */
	async put<T, D = unknown>(url: string, data?: D): Promise<ApiResponse<T>> {
		try {
			const response = await axiosInstance.put<T>(url, data)
			return {
				success: true,
				data: response.data,
				statusCode: response.status,
			}
		} catch (error) {
			return this.handleError(error)
		}
	}

	/**
	 * PATCH request
	 */
	async patch<T, D = unknown>(url: string, data?: D): Promise<ApiResponse<T>> {
		try {
			const response = await axiosInstance.patch<T>(url, data)
			return {
				success: true,
				data: response.data,
				statusCode: response.status,
			}
		} catch (error) {
			return this.handleError(error)
		}
	}

	/**
	 * DELETE request
	 */
	async delete<T>(url: string): Promise<ApiResponse<T>> {
		try {
			const response = await axiosInstance.delete<T>(url)
			return {
				success: true,
				data: response.data,
				statusCode: response.status,
			}
		} catch (error) {
			return this.handleError(error)
		}
	}

	/**
	 * Handle errors consistently
	 */
	private handleError(error: unknown): ApiResponse<never> {
		if (error instanceof AxiosError) {
			const status = error.response?.status || 500
			const errorData = error.response?.data as ApiErrorResponse | undefined

			return {
				success: false,
				error: errorData || {
					title: error.message,
					status,
					detail: error.response?.statusText,
				},
				statusCode: status,
			}
		}

		return {
			success: false,
			error: {
				title: 'Unknown Error',
				status: 500,
				detail:
					error instanceof Error
						? error.message
						: 'An unexpected error occurred',
			},
			statusCode: 500,
		}
	}

	/**
	 * Helper to throw on error (for use with try-catch)
	 */
	throwIfError<T>(response: ApiResponse<T>): T {
		if (!response.success) {
			const error = new Error(
				response.error?.detail || response.error?.title || 'API Error'
			) as ApiError
			error.response = response.error
			throw error
		}
		return response.data as T
	}
}

export default new ApiService()
