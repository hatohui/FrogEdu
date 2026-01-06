import { useCallback } from 'react'
import { useSearchParams } from 'react-router'

export interface MenuSearchParams {
	page?: number
	count?: number
	category?: string
	lang?: string
}

export function useMenuSearchParams() {
	const [searchParams, setSearchParams] = useSearchParams()

	const page = Number(searchParams.get('page')) || 1
	const count = Number(searchParams.get('count')) || 20
	const category = searchParams.get('category') || 'all'
	const lang = searchParams.get('lang') || undefined

	const updateParams = useCallback(
		(updates: Partial<MenuSearchParams>) => {
			setSearchParams(params => {
				Object.entries(updates).forEach(([key, value]) => {
					if (value === undefined || value === null) {
						params.delete(key)
					} else {
						params.set(key, String(value))
					}
				})
				return params
			})
		},
		[setSearchParams]
	)

	const setPage = useCallback(
		(newPage: number) => {
			updateParams({ page: newPage })
		},
		[updateParams]
	)

	const setCount = useCallback(
		(newCount: number) => {
			updateParams({ count: newCount, page: 1 })
		},
		[updateParams]
	)

	const setCategory = useCallback(
		(newCategory: string) => {
			updateParams({ category: newCategory, page: 1 })
		},
		[updateParams]
	)

	const setLang = useCallback(
		(newLang: string | undefined) => {
			updateParams({ lang: newLang })
		},
		[updateParams]
	)

	const resetParams = useCallback(() => {
		setSearchParams({})
	}, [setSearchParams])

	return {
		page,
		count,
		category,
		lang,
		setPage,
		setCount,
		setCategory,
		setLang,
		updateParams,
		resetParams,
	}
}

export function buildSearchParams(params: MenuSearchParams): URLSearchParams {
	const searchParams = new URLSearchParams()

	Object.entries(params).forEach(([key, value]) => {
		if (value !== undefined && value !== null) {
			searchParams.set(key, String(value))
		}
	})

	return searchParams
}

export function parseMenuSearchParams(
	searchParams: URLSearchParams
): MenuSearchParams {
	return {
		page: Number(searchParams.get('page')) || 1,
		count: Number(searchParams.get('count')) || 20,
		category: searchParams.get('category') || 'all',
		lang: searchParams.get('lang') || undefined,
	}
}
