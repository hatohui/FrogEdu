import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import tailwindcss from '@tailwindcss/vite'
import { devtools } from '@tanstack/devtools-vite'

export default defineConfig({
	plugins: [react(), tailwindcss(), devtools()],
	resolve: {
		alias: {
			'@': '/src',
		},
	},
})
