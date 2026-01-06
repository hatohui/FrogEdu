import en from './en/en'

const resources = {
	en,
}

export { resources }
export type Translation = { translation: { [key: string]: string } }
export type Language = keyof typeof resources
