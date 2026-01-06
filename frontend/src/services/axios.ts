import axios from 'axios'

const ApiUrl = import.meta.env.VITE_API_URL

const axiosInstance = axios.create({
	baseURL: ApiUrl,
	timeout: 10000,
	headers: {
		'Content-Type': 'application/json',
	},
})

export default axiosInstance
