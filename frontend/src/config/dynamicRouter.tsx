import { createBrowserRouter } from 'react-router'
import type { ComponentType } from 'react'

type PageModule = { default: ComponentType<Record<string, unknown>> }
type GlobImport = Record<string, () => Promise<PageModule>>

const pageLoaders = import.meta.glob('../pages/**/page.tsx') as GlobImport
const layoutLoaders = import.meta.glob('../pages/**/layout.tsx') as GlobImport
const errorLoaders = import.meta.glob(
	'../pages/**/{not-found,error}.tsx'
) as GlobImport

const getRoutePath = (filename: string) => {
	let name = filename.replace(/^\.\.\/pages\//, '').replace(/\.tsx$/, '')
	name = name.replace(/\[(.+?)\]/g, ':$1')
	name = name.replace(/\/?\(([^)]+)\)/g, '')
	name = name.replace(/^\/+/, '')
	if (name === 'page') return '/'
	if (name.endsWith('/page'))
		return `/${name.replace('/page', '')}`.length
			? `/${name.replace('/page', '')}`
			: '/'
	return `/${name}`
}

const routes = Object.keys(pageLoaders).map(pageKey => {
	const path = getRoutePath(pageKey)

	return {
		path,
		lazy: async () => {
			const pageMod = await pageLoaders[pageKey]()
			let PageComponent = pageMod.default

			const matches: string[] = []
			const normalizedPageKey = pageKey.replace(/\\/g, '/')

			for (const layoutKey of Object.keys(layoutLoaders)) {
				const normalizedLayoutKey = layoutKey.replace(/\\/g, '/')
				const layoutDir = normalizedLayoutKey
					.replace(/\/layout\.tsx$/, '/')
					.replace(/\\/g, '/')
				if (normalizedPageKey.startsWith(layoutDir)) {
					matches.push(layoutKey)
				}
			}

			matches.sort((a, b) => b.length - a.length)

			const Layouts: React.ComponentType<Record<string, unknown>>[] = []

			for (const layoutKey of matches) {
				const layoutMod = await layoutLoaders[layoutKey]()
				const Layout = layoutMod.default
				Layouts.push(Layout)
				const Prev = PageComponent
				PageComponent = () => (
					<Layout>
						<Prev />
					</Layout>
				)
			}

			const errorMatches: string[] = []
			for (const errorKey of Object.keys(errorLoaders)) {
				const normalizedErrorKey = errorKey.replace(/\\/g, '/')
				const errorDir = normalizedErrorKey
					.replace(/\/(not-found|error)\.tsx$/, '/')
					.replace(/\\/g, '/')
				if (normalizedPageKey.startsWith(errorDir)) {
					errorMatches.push(errorKey)
				}
			}

			errorMatches.sort((a, b) => b.length - a.length)

			if (errorMatches.length > 0) {
				const errorMod = await errorLoaders[errorMatches[0]]()
				const ErrorComponent = errorMod.default
				let ErrorElement: React.ReactNode = <ErrorComponent />

				for (const Layout of Layouts) {
					ErrorElement = <Layout>{ErrorElement}</Layout>
				}

				return { Component: PageComponent, errorElement: ErrorElement }
			}

			return { Component: PageComponent }
		},
	}
})

const globalNotFoundKey = Object.keys(errorLoaders).find(
	k =>
		k.replace(/\\/g, '/') === '../pages/not-found.tsx' ||
		k.replace(/\\/g, '/') === '../pages/error.tsx'
)

if (globalNotFoundKey) {
	routes.push({
		path: '*',
		lazy: async () => {
			const mod = await errorLoaders[globalNotFoundKey]()
			let Component = mod.default

			const normalizedKey = globalNotFoundKey.replace(/\\/g, '/')
			const matches: string[] = []
			for (const layoutKey of Object.keys(layoutLoaders)) {
				const normalizedLayoutKey = layoutKey.replace(/\\/g, '/')
				const layoutDir = normalizedLayoutKey
					.replace(/\/layout\.tsx$/, '/')
					.replace(/\\/g, '/')
				if (normalizedKey.startsWith(layoutDir)) {
					matches.push(layoutKey)
				}
			}
			matches.sort((a, b) => b.length - a.length)

			for (const layoutKey of matches) {
				const layoutMod = await layoutLoaders[layoutKey]()
				const Layout = layoutMod.default
				const Prev = Component
				Component = () => (
					<Layout>
						<Prev />
					</Layout>
				)
			}

			return { Component }
		},
	})
}

const router = createBrowserRouter(routes, {
	future: { v7_startTransition: true },
})

export default router
