import React from 'react'
import { Link } from 'react-router'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Users, ChevronRight, Copy } from 'lucide-react'
import { toast } from 'sonner'
import type { ClassRoom } from '@/types/model/class-service'
import { useTranslation } from 'react-i18next'

interface ClassCardProps {
	classData: ClassRoom
	isTeacher?: boolean
}

const ClassCard: React.FC<ClassCardProps> = ({
	classData,
	isTeacher = false,
}) => {
	const { t } = useTranslation()

	const copyInviteCode = (e: React.MouseEvent) => {
		e.preventDefault()
		e.stopPropagation()
		if (classData.inviteCode) {
			navigator.clipboard.writeText(classData.inviteCode)
			toast.success(t('pages.classes.detail.invite_copied'))
		}
	}

	return (
		<Link to={`/app/classes/${classData.id}`}>
			<Card className='hover:shadow-md transition-all duration-200 cursor-pointer group'>
				<CardHeader className='pb-3'>
					<div className='flex items-start justify-between'>
						<div className='flex-1'>
							<CardTitle className='text-lg group-hover:text-primary transition-colors'>
								{classData.name}
							</CardTitle>
							<CardDescription className='mt-1'>
								{classData.assignmentCount > 0 && (
									<span className='text-xs'>
										{classData.assignmentCount}{' '}
										{t('pages.dashboard.classes.table.assignments')}
									</span>
								)}
							</CardDescription>
						</div>
						<Badge
							variant={classData.isActive ? 'default' : 'secondary'}
							className='ml-2'
						>
							{t('pages.classes.card.grade_badge', {
								grade: classData.grade,
							})}
						</Badge>
					</div>
				</CardHeader>
				<CardContent className='space-y-3'>
					{/* Student count */}
					<div className='flex items-center text-sm text-muted-foreground'>
						<Users className='h-4 w-4 mr-2' />
						<span>
							{classData.studentCount} {t('pages.classes.card.students_label')}
							{classData.maxStudents &&
								t('pages.classes.card.max_suffix', {
									max: classData.maxStudents,
								})}
						</span>
					</div>

					{/* Invite code for teachers */}
					{isTeacher && classData.inviteCode && (
						<div className='flex items-center justify-between bg-muted rounded-md p-2'>
							<div className='flex items-center gap-2'>
								<span className='text-xs text-muted-foreground'>
									{t('pages.classes.card.invite_label')}
								</span>
								<code className='font-mono font-semibold tracking-widest'>
									{classData.inviteCode}
								</code>
							</div>
							<Button
								variant='ghost'
								size='sm'
								onClick={copyInviteCode}
								className='h-7 w-7 p-0'
							>
								<Copy className='h-4 w-4' />
							</Button>
						</div>
					)}

					{/* View details button */}
					<div className='flex justify-end pt-2'>
						<Button
							variant='ghost'
							size='sm'
							className='text-muted-foreground group-hover:text-primary'
						>
							{t('pages.classes.card.view_details')}
							<ChevronRight className='h-4 w-4 ml-1' />
						</Button>
					</div>
				</CardContent>
			</Card>
		</Link>
	)
}

export default ClassCard
