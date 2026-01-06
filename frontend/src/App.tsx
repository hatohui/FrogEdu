import React, { Suspense } from 'react'
import registerGSAPPlugins from './config/registerGSAPPlugins'
import { RouterProvider } from 'react-router'
import router from './config/dynamicRouter'
import LanguageProvider from './config/i18n'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { TanStackDevtools } from '@tanstack/react-devtools'
import ThemeProvider from './config/theme'
import { Toaster } from './components/ui/sonner'
import tanstackConfig from './config/tanstack'
import { useTranslation } from 'react-i18next'
import { ReactQueryDevtoolsPanel } from '@tanstack/react-query-devtools'
import { GlobalLoading } from './components/common/GlobalLoading'

registerGSAPPlugins()

const AppContent = (): React.ReactNode => {
	const t = useTranslation()
	const config = tanstackConfig(t)
	const client = new QueryClient(config)

	return (
		<QueryClientProvider client={client}>
			<ThemeProvider>
				<RouterProvider router={router} />
				<Toaster richColors position='top-center' />
			</ThemeProvider>
			{import.meta.env.DEV && (
				<TanStackDevtools
					plugins={[
						{
							name: 'TanStack Query',
							render: <ReactQueryDevtoolsPanel />,
							defaultOpen: true,
						},
					]}
				/>
			)}
		</QueryClientProvider>
	)
}

const App = (): React.ReactNode => {
	return (
		<Suspense fallback={<GlobalLoading />}>
			<LanguageProvider>
				<AppContent />
			</LanguageProvider>
		</Suspense>
	)
}

export default App
