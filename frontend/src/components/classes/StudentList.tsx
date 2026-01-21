import React from 'react'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'
import { Badge } from '@/components/ui/badge'
import { format } from 'date-fns'
import type { ClassMemberDto } from '@/services/class.service'

interface StudentListProps {
	members: ClassMemberDto[]
	showRole?: boolean
}

const StudentList: React.FC<StudentListProps> = ({
	members,
	showRole = true,
}) => {
	const getInitials = (name: string) => {
		return name
			.split(' ')
			.map(n => n[0])
			.join('')
			.toUpperCase()
			.slice(0, 2)
	}

	const students = members.filter(m => m.role === 'Student')
	const teachers = members.filter(m => m.role === 'Teacher')

	return (
		<div className='space-y-6'>
			{/* Teachers section */}
			{teachers.length > 0 && (
				<div>
					<h3 className='text-sm font-medium text-muted-foreground mb-3'>
						Teachers ({teachers.length})
					</h3>
					<div className='flex flex-wrap gap-3'>
						{teachers.map(teacher => (
							<div
								key={teacher.userId}
								className='flex items-center gap-2 rounded-lg border p-2 pr-4'
							>
								<Avatar className='h-8 w-8'>
									<AvatarImage src={teacher.avatarUrl} alt={teacher.name} />
									<AvatarFallback className='text-xs'>
										{getInitials(teacher.name)}
									</AvatarFallback>
								</Avatar>
								<div>
									<p className='text-sm font-medium'>{teacher.name}</p>
									<Badge variant='secondary' className='text-xs'>
										Teacher
									</Badge>
								</div>
							</div>
						))}
					</div>
				</div>
			)}

			{/* Students section */}
			<div>
				<h3 className='text-sm font-medium text-muted-foreground mb-3'>
					Students ({students.length})
				</h3>
				{students.length === 0 ? (
					<div className='text-center py-8 text-muted-foreground'>
						<p>No students enrolled yet.</p>
						<p className='text-sm'>Share the invite code to add students.</p>
					</div>
				) : (
					<div className='rounded-md border'>
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead className='w-[50px]'></TableHead>
									<TableHead>Name</TableHead>
									{showRole && <TableHead>Role</TableHead>}
									<TableHead>Joined</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{students.map(student => (
									<TableRow key={student.userId}>
										<TableCell>
											<Avatar className='h-8 w-8'>
												<AvatarImage
													src={student.avatarUrl}
													alt={student.name}
												/>
												<AvatarFallback className='text-xs'>
													{getInitials(student.name)}
												</AvatarFallback>
											</Avatar>
										</TableCell>
										<TableCell className='font-medium'>
											{student.name}
										</TableCell>
										{showRole && (
											<TableCell>
												<Badge variant='outline'>{student.role}</Badge>
											</TableCell>
										)}
										<TableCell className='text-muted-foreground'>
											{format(new Date(student.joinedAt), 'MMM d, yyyy')}
										</TableCell>
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
