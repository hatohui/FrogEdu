import React, { useState } from 'react'
import { Plus, FileText, BookOpen, CheckCircle2 } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import { useExams } from '@/hooks/useExams'
import { Badge } from '@/components/ui/badge'
import { useNavigate } from 'react-router'

const ExamsPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const [filter, setFilter] = useState<'all' | 'draft' | 'published'>('all')

	const isDraft =
		filter === 'draft' ? true : filter === 'published' ? false : undefined
	const { data: exams, isLoading } = useExams(isDraft)

	const handleCreateExam = () => {
		navigate('/app/exams/create')
	}

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div className='space-y-2'>
					<h1 className='text-3xl font-bold tracking-tight flex items-center space-x-2'>
						<FileText className='h-8 w-8' />
						<span>Exams</span>
					</h1>
					<p className='text-muted-foreground'>
						Create and manage your assessments
					</p>
				</div>
				<Button onClick={handleCreateExam} size='lg'>
					<Plus className='h-5 w-5 mr-2' />
					New Exam
				</Button>
			</div>

			{/* Filter Tabs */}
			<div className='flex space-x-2'>
				<Button
					variant={filter === 'all' ? 'default' : 'outline'}
					onClick={() => setFilter('all')}
				>
					All Exams
				</Button>
				<Button
					variant={filter === 'draft' ? 'default' : 'outline'}
					onClick={() => setFilter('draft')}
				>
					Drafts
				</Button>
				<Button
					variant={filter === 'published' ? 'default' : 'outline'}
					onClick={() => setFilter('published')}
				>
					Published
				</Button>
			</div>

			{/* Exams Table */}
			<Card>
				<CardHeader>
					<CardTitle>Your Exams</CardTitle>
				</CardHeader>
				<CardContent>
					{isLoading ? (
						<div className='text-center py-8 text-muted-foreground'>
							Loading exams...
						</div>
					) : exams && exams.length > 0 ? (
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead>Name</TableHead>
									<TableHead>Subject</TableHead>
									<TableHead>Grade</TableHead>
									<TableHead>Status</TableHead>
									<TableHead>Created</TableHead>
									<TableHead className='text-right'>Actions</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{exams.map(exam => (
									<TableRow key={exam.id}>
										<TableCell className='font-medium'>{exam.name}</TableCell>
										<TableCell>-</TableCell>
										<TableCell>{exam.grade}</TableCell>
										<TableCell>
											{exam.isDraft ? (
												<Badge variant='secondary'>
													<BookOpen className='h-3 w-3 mr-1' />
													Draft
												</Badge>
											) : (
												<Badge variant='default'>
													<CheckCircle2 className='h-3 w-3 mr-1' />
													Active
												</Badge>
											)}
										</TableCell>
										<TableCell>
											{new Date(exam.createdAt).toLocaleDateString()}
										</TableCell>
										<TableCell className='text-right'>
											<Button
												variant='ghost'
												size='sm'
												onClick={() => navigate(`/app/exams/${exam.id}`)}
											>
												View
											</Button>
										</TableCell>
									</TableRow>
								))}
							</TableBody>
						</Table>
					) : (
						<div className='text-center py-12'>
							<FileText className='h-12 w-12 mx-auto text-muted-foreground mb-4' />
							<p className='text-muted-foreground mb-4'>No exams found</p>
							<Button onClick={handleCreateExam}>
								<Plus className='h-4 w-4 mr-2' />
								Create Your First Exam
							</Button>
						</div>
					)}
				</CardContent>
			</Card>
		</div>
	)
}

export default ExamsPage
