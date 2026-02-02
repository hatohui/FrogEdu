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
import { Users, BookOpen, ChevronRight, Copy, Clock } from 'lucide-react'
import { toast } from 'sonner'
import type { ClassDto } from '@/services/class-microservice/class.service'

interface ClassCardProps {
	classData: ClassDto
	isTeacher?: boolean
}

const ClassCard: React.FC<ClassCardProps> = ({
	classData,
	isTeacher = false,
}) => {
	const copyInviteCode = (e: React.MouseEvent) => {
		e.preventDefault()
		e.stopPropagation()
		if (classData.inviteCode) {
			navigator.clipboard.writeText(classData.inviteCode)
			toast.success('Invite code copied to clipboard!')
		}
	}

	const isCodeExpired =
		classData.inviteCodeExpiresAt &&
		new Date(classData.inviteCodeExpiresAt) < new Date()

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
								{classData.subject && (
									<span className='inline-flex items-center gap-1'>
										<BookOpen className='h-3 w-3' />
										{classData.subject}
									</span>
								)}
							</CardDescription>
						</div>
						<Badge
							variant={classData.isArchived ? 'secondary' : 'default'}
							className='ml-2'
						>
							Grade {classData.grade}
						</Badge>
					</div>
				</CardHeader>
				<CardContent className='space-y-3'>
					{/* Student count */}
					<div className='flex items-center text-sm text-muted-foreground'>
						<Users className='h-4 w-4 mr-2' />
						<span>
							{classData.studentCount} student
							{classData.studentCount !== 1 ? 's' : ''}
							{classData.maxStudents && ` / ${classData.maxStudents} max`}
						</span>
					</div>

					{/* Teacher info */}
					{!isTeacher && classData.teacherName && (
						<div className='text-sm text-muted-foreground'>
							Teacher: {classData.teacherName}
						</div>
					)}

					{/* Invite code for teachers */}
					{isTeacher && classData.inviteCode && (
						<div className='flex items-center justify-between bg-muted rounded-md p-2'>
							<div className='flex items-center gap-2'>
								<span className='text-xs text-muted-foreground'>
									Invite Code:
								</span>
								<code className='font-mono font-semibold tracking-widest'>
									{classData.inviteCode}
								</code>
								{isCodeExpired && (
									<Badge variant='destructive' className='text-xs'>
										<Clock className='h-3 w-3 mr-1' />
										Expired
									</Badge>
								)}
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

					{/* School info */}
					{classData.school && (
						<div className='text-xs text-muted-foreground truncate'>
							{classData.school}
						</div>
					)}

					{/* View details button */}
					<div className='flex justify-end pt-2'>
						<Button
							variant='ghost'
							size='sm'
							className='text-muted-foreground group-hover:text-primary'
						>
							View Details
							<ChevronRight className='h-4 w-4 ml-1' />
						</Button>
					</div>
				</CardContent>
			</Card>
		</Link>
	)
}

export default ClassCard
