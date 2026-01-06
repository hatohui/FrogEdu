import js from '@eslint/js'
import globals from 'globals'
import reactHooks from 'eslint-plugin-react-hooks'
import reactRefresh from 'eslint-plugin-react-refresh'
import tseslint from 'typescript-eslint'
import { globalIgnores } from 'eslint/config'
import prettier from 'eslint-config-prettier'
import eslintPluginPrettier from 'eslint-plugin-prettier'
import reactPlugin from 'eslint-plugin-react'

export default tseslint.config([
	globalIgnores(['dist']),
	{
		files: ['**/*.{ts,tsx}'],
		extends: [
			js.configs.recommended,
			tseslint.configs.recommended,
			reactHooks.configs['recommended-latest'],
			reactRefresh.configs.vite,
			prettier,
		],
		plugins: {
			prettier: eslintPluginPrettier,
			react: reactPlugin.configs.recommended.plugins,
		},
		rules: {
			'prettier/prettier': 'error',
			'react/react-in-jsx-scope': 'off',
			'react-refresh/only-export-components': 'off',
		},
		languageOptions: {
			ecmaVersion: 2020,
			globals: globals.browser,
		},
	},
])
