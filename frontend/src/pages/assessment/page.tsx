import React from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { FileText } from 'lucide-react'

const AssessmentPage = (): React.ReactElement => {
	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			<div className='space-y-2'>
				<h1 className='text-3xl font-bold tracking-tight flex items-center space-x-2'>
					<FileText className='h-8 w-8' />
					<span>Exam Generator</span>
				</h1>
				<p className='text-muted-foreground'>
					Create and manage assessments with our intelligent exam builder.
				</p>
			</div>

			<Card>
				<CardHeader>
					<CardTitle>Coming Soon</CardTitle>
				</CardHeader>
				<CardContent>
					<p className='text-muted-foreground'>
						The Assessment feature is currently under development. You'll be
						able to create exams, build question matrices, and export PDFs here.
					</p>
				</CardContent>
			</Card>
		</div>
	)
}

export default AssessmentPage
