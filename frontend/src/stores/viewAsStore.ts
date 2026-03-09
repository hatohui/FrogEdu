import { create } from 'zustand'

type ViewAsRole = 'Teacher' | 'Student' | null

interface ViewAsState {
	viewAs: ViewAsRole
	setViewAs: (role: ViewAsRole) => void
	clearViewAs: () => void
}

export const useViewAsStore = create<ViewAsState>()(set => ({
	viewAs: null,
	setViewAs: (role: ViewAsRole) => set({ viewAs: role }),
	clearViewAs: () => set({ viewAs: null }),
}))
