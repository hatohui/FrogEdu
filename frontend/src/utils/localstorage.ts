const save = (key: string, value: string): void => {
	return globalThis.localStorage.setItem(key, value)
}

const load = (key: string): string | null => {
	return globalThis.localStorage.getItem(key)
}

const have = (key: string): boolean => {
	return globalThis.localStorage.getItem(key) !== null
}

const preload = (key: string, defaultValue: string): string => {
	let value = load(key)
	if (value === null) {
		save(key, defaultValue)
		value = defaultValue
	}
	return value
}

export const localStorage = {
	save,
	load,
	have,
	preload,
}
