import React from 'react'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import { Badge } from '@/components/ui/badge'
import { format } from 'date-fns'
import type { ClassEnrollment } from '@/types/model/class-service'
import { useTranslation } from 'react-i18next'

interface StudentListProps {
	enrollments: ClassEnrollment[]
}

const StudentList: React.FC<StudentListProps> = ({ enrollments }) => {
	const { t } = useTranslation()

	const getStatusBadge = (status: string) => {
		switch (status) {
			case 'Active':
				return <Badge variant='default'>{status}</Badge>
			case 'Inactive':
				return <Badge variant='secondary'>{status}</Badge>
			case 'Kicked':
				return <Badge variant='destructive'>{status}</Badge>
			case 'Withdrawn':
				return <Badge variant='outline'>{status}</Badge>
			default:
				return <Badge variant='outline'>{status}</Badge>
		}
	}

	return (
		<div className='space-y-6'>
			<div>
				<h3 className='text-sm font-medium text-muted-foreground mb-3'>
					{t('pages.classes.roster.students', {
						count: enrollments.length,
					})}
				</h3>
				{enrollments.length === 0 ? (
					<div className='text-center py-8 text-muted-foreground'>
						<p>{t('pages.classes.roster.empty_title')}</p>
						<p className='text-sm'>
							{t('pages.classes.roster.empty_subtitle')}
						</p>
					</div>
				) : (
					<div className='rounded-md border'>
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead>
										{t('pages.classes.roster.table.student_id')}
									</TableHead>
									<TableHead>
										{t('pages.classes.roster.table.joined')}
									</TableHead>
									<TableHead>
										{t('pages.classes.roster.table.status')}
									</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{enrollments.map(enrollment => (
									<TableRow key={enrollment.id}>
										<TableCell className='font-mono text-sm'>
											{enrollment.studentId}
										</TableCell>
										<TableCell className='text-muted-foreground'>
											{format(new Date(enrollment.joinedAt), 'MMM d, yyyy')}
										</TableCell>
										<TableCell>{getStatusBadge(enrollment.status)}</TableCell>
									</TableRow>
								))}
							</TableBody>
						</Table>
					</div>
				)}
			</div>
		</div>
	)
}

export default StudentList
