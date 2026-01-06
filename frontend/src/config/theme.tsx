import { createContext, useContext, useEffect, useState } from 'react'
import { THEME_STORAGE_KEY } from '../common/constants'
import { getUserPreferredTheme } from '../utils/theme'
import { localStorage } from '../utils/localstorage'

type ThemeContextType = [theme: string, toggleTheme: () => void]

const ThemeContext = createContext<ThemeContextType>(['light', () => {}])

const ThemeProvider = ({
	children,
}: {
	children: React.ReactNode
}): React.JSX.Element => {
	const [theme, setTheme] = useState('light')

	useEffect(() => {
		const savedTheme = localStorage.load(THEME_STORAGE_KEY)

		if (savedTheme) {
			setTheme(savedTheme)
		} else {
			const userPreferredTheme = getUserPreferredTheme()
			setTheme(userPreferredTheme)
			localStorage.save(THEME_STORAGE_KEY, userPreferredTheme)
		}
	}, [])

	const toggleTheme = () => {
		const newTheme = theme === 'light' ? 'dark' : 'light'
		setTheme(newTheme)
		localStorage.save(THEME_STORAGE_KEY, newTheme)
	}

	return (
		<ThemeContext.Provider value={[theme, toggleTheme]}>
			<div data-theme={theme}>{children}</div>
		</ThemeContext.Provider>
	)
}

export const useTheme = (): ThemeContextType => {
	const context = useContext(ThemeContext)
	if (!context) {
		throw new Error('useTheme must be used within a ThemeProvider')
	}
	return context
}

export default ThemeProvider
