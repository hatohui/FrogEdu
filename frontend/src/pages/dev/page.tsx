import { useQuery } from '@tanstack/react-query'
import axios from 'axios'
import React from 'react'

const DevPage = () => {
	useQuery({
		queryKey: ['devData'],
		queryFn: () =>
			axios.get('http://localhost:5000/health').then(res => res.data),
	})

	return <div>Dev</div>
}

export default DevPage
