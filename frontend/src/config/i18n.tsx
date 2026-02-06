import i18n from 'i18next'
import { initReactI18next } from 'react-i18next'
import I18nextBrowserLanguageDetector from 'i18next-browser-languagedetector'
import { createContext, useContext, useState, useEffect } from 'react'

const loadLanguage = async (lng: string) => {
	const response = await fetch(`/languages/${lng}.json`)
	return response.json()
}

export const initI18n = async () => {
	const supportedLngs = ['en', 'vi']

	const urlParams = new URLSearchParams(window.location.search)
	const langFromQuery = urlParams.get('lang')
	const langFromStorage = localStorage.getItem('i18nextLng')
	const browserLang = navigator.language.split('-')[0]

	let detectedLang = 'en'

	if (langFromQuery && supportedLngs.includes(langFromQuery)) {
		detectedLang = langFromQuery
	} else if (langFromStorage && supportedLngs.includes(langFromStorage)) {
		detectedLang = langFromStorage
	} else if (supportedLngs.includes(browserLang)) {
		detectedLang = browserLang
	}

	if (!langFromQuery) {
		urlParams.set('lang', detectedLang)
		const newUrl = `${window.location.pathname}?${urlParams.toString()}`
		window.history.replaceState({}, '', newUrl)
	}

	const translation = await loadLanguage(detectedLang)

	await i18n
		.use(I18nextBrowserLanguageDetector)
		.use(initReactI18next)
		.init({
			resources: {
				[detectedLang]: { translation },
			},
			lng: detectedLang,
			supportedLngs,
			fallbackLng: 'en',
			detection: {
				order: ['querystring', 'localStorage', 'navigator'],
				caches: ['localStorage'],
				lookupQuerystring: 'lang',
				lookupLocalStorage: 'i18nextLng',
			},
			interpolation: {
				escapeValue: false,
			},
			react: {
				useSuspense: false,
			},
		})

	return detectedLang
}

export const changeLanguage = async (lng: string) => {
	const supportedLngs = ['en', 'vi']
	if (!supportedLngs.includes(lng)) return

	const translation = await loadLanguage(lng)
	i18n.addResourceBundle(lng, 'translation', translation)
	await i18n.changeLanguage(lng)

	const urlParams = new URLSearchParams(window.location.search)
	urlParams.set('lang', lng)
	const newUrl = `${window.location.pathname}?${urlParams.toString()}`
	window.history.replaceState({}, '', newUrl)
}

type LanguageContextType = {
	lang: string
	setLanguage: (lang: string) => Promise<void>
}

const LanguageContext = createContext<LanguageContextType>({
	lang: 'en',
	setLanguage: async () => {},
})

export const useLanguage = () => useContext(LanguageContext)

const LanguageProvider = ({ children }: { children: React.ReactNode }) => {
	const [lang, setLang] = useState('en')
	const [isInitialized, setIsInitialized] = useState(false)

	useEffect(() => {
		initI18n().then(detectedLang => {
			setLang(detectedLang)
			setIsInitialized(true)
		})
	}, [])

	const setLanguage = async (newLang: string) => {
		await changeLanguage(newLang)
		setLang(newLang)
	}

	if (!isInitialized) {
		return null
	}

	return (
		<LanguageContext.Provider value={{ lang, setLanguage }}>
			{children}
		</LanguageContext.Provider>
	)
}

export default LanguageProvider
