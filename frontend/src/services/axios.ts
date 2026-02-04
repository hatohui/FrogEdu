import axios from 'axios'
import { fetchAuthSession } from 'aws-amplify/auth'

const ApiUrl = import.meta.env.VITE_API_URL

// Create axios instance with /api prefix
// Frontend calls: /users/health -> axios makes request to: {ApiUrl}/api/users/health
// This matches the API Gateway routing pattern
const axiosInstance = axios.create({
	baseURL: `${ApiUrl}/api`,
	timeout: 30000,
	headers: {
		'Content-Type': 'application/json',
	},
})

// Add request interceptor to attach Cognito JWT token
axiosInstance.interceptors.request.use(
	async config => {
		try {
			const session = await fetchAuthSession()
			const token = session.tokens?.idToken?.toString()

			if (token) {
				config.headers.Authorization = `Bearer ${token}`
			}
		} catch (error) {
			console.error('Error fetching auth session:', error)
		}
		return config
	},
	error => {
		return Promise.reject(error)
	}
)

axiosInstance.interceptors.response.use(
	response => response,
	error => {
		if (error.response?.status === 401) {
			console.error('Unauthorized - redirecting to login')
		}
		return Promise.reject(error)
	}
)

export const publicAxios = axios.create({
	baseURL: `${ApiUrl}/api`,
	timeout: 10000,
	headers: {
		'Content-Type': 'application/json',
	},
})

export default axiosInstance
