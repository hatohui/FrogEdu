import React from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { BookOpen } from 'lucide-react'

const ContentPage = (): React.ReactElement => {
	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			<div className='space-y-2'>
				<h1 className='text-3xl font-bold tracking-tight flex items-center space-x-2'>
					<BookOpen className='h-8 w-8' />
					<span>Content Library</span>
				</h1>
				<p className='text-muted-foreground'>
					Browse and manage your educational content, textbooks, and learning
					materials.
				</p>
			</div>

			<Card>
				<CardHeader>
					<CardTitle>Coming Soon</CardTitle>
				</CardHeader>
				<CardContent>
					<p className='text-muted-foreground'>
						The Content Library feature is currently under development. You'll
						be able to browse textbooks, chapters, and lessons here.
					</p>
				</CardContent>
			</Card>
		</div>
	)
}

export default ContentPage
