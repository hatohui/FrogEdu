import React from 'react'
import { useParams, Link, useNavigate } from 'react-router'
import { useClassDetails, useRegenerateInviteCode } from '@/hooks/useClasses'
import { useMe } from '@/hooks/auth/useMe'
import { StudentList } from '@/components/classes'
import { Button } from '@/components/ui/button'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Skeleton } from '@/components/ui/skeleton'
import { Separator } from '@/components/ui/separator'
import {
	ArrowLeft,
	BookOpen,
	Building,
	Calendar,
	Clock,
	Copy,
	RefreshCw,
	Users,
} from 'lucide-react'
import { format } from 'date-fns'
import { toast } from 'sonner'

const ClassDetailPage: React.FC = () => {
	const { id } = useParams<{ id: string }>()
	const navigate = useNavigate()
	const { user } = useMe()

	const { data: classDetails, isLoading, error } = useClassDetails(id || '')
	const regenerateCode = useRegenerateInviteCode()

	const isTeacher =
		user?.role?.name === 'Teacher' &&
		classDetails?.homeroomTeacherId === user?.cognitoId

	const copyInviteCode = () => {
		if (classDetails?.inviteCode) {
			navigator.clipboard.writeText(classDetails.inviteCode)
			toast.success('Invite code copied to clipboard!')
		}
	}

	const handleRegenerateCode = async () => {
		if (!id) return
		await regenerateCode.mutateAsync({ classId: id })
	}

	if (isLoading) {
		return (
			<div className='p-6 space-y-6 max-w-5xl mx-auto'>
				<Skeleton className='h-8 w-24' />
				<div className='grid gap-6 lg:grid-cols-3'>
					<div className='lg:col-span-2'>
						<Skeleton className='h-64' />
					</div>
					<div>
						<Skeleton className='h-48' />
					</div>
				</div>
				<Skeleton className='h-96' />
			</div>
		)
	}

	if (error || !classDetails) {
		return (
			<div className='p-6'>
				<Button variant='ghost' onClick={() => navigate(-1)} className='mb-4'>
					<ArrowLeft className='h-4 w-4 mr-2' />
					Back
				</Button>
				<div className='text-center py-12'>
					<p className='text-destructive'>
						{error?.message || 'Class not found'}
					</p>
				</div>
			</div>
		)
	}

	const isCodeExpired =
		classDetails.inviteCodeExpiresAt &&
		new Date(classDetails.inviteCodeExpiresAt) < new Date()

	const studentCount = classDetails.members.filter(
		m => m.role === 'Student'
	).length

	return (
		<div className='p-6 space-y-6 max-w-5xl mx-auto'>
			{/* Back button */}
			<Link to='/dashboard/classes'>
				<Button variant='ghost' className='gap-2'>
					<ArrowLeft className='h-4 w-4' />
					Back to Classes
				</Button>
			</Link>

			{/* Header */}
			<div className='flex items-start justify-between'>
				<div>
					<div className='flex items-center gap-3'>
						<h1 className='text-3xl font-bold tracking-tight'>
							{classDetails.name}
						</h1>
						{classDetails.isArchived && (
							<Badge variant='secondary'>Archived</Badge>
						)}
					</div>
					{classDetails.description && (
						<p className='text-muted-foreground mt-2 max-w-2xl'>
							{classDetails.description}
						</p>
					)}
				</div>
			</div>

			<div className='grid gap-6 lg:grid-cols-3'>
				{/* Class info */}
				<Card className='lg:col-span-2'>
					<CardHeader>
						<CardTitle>Class Information</CardTitle>
					</CardHeader>
					<CardContent className='space-y-4'>
						<div className='grid gap-4 sm:grid-cols-2'>
							<div className='flex items-center gap-3'>
								<div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10'>
									<BookOpen className='h-5 w-5 text-primary' />
								</div>
								<div>
									<p className='text-sm text-muted-foreground'>Subject</p>
									<p className='font-medium'>
										{classDetails.subject || 'Not specified'}
									</p>
								</div>
							</div>

							<div className='flex items-center gap-3'>
								<div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10'>
									<Users className='h-5 w-5 text-primary' />
								</div>
								<div>
									<p className='text-sm text-muted-foreground'>Grade Level</p>
									<p className='font-medium'>Grade {classDetails.grade}</p>
								</div>
							</div>

							{classDetails.school && (
								<div className='flex items-center gap-3'>
									<div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10'>
										<Building className='h-5 w-5 text-primary' />
									</div>
									<div>
										<p className='text-sm text-muted-foreground'>School</p>
										<p className='font-medium'>{classDetails.school}</p>
									</div>
								</div>
							)}

							<div className='flex items-center gap-3'>
								<div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10'>
									<Calendar className='h-5 w-5 text-primary' />
								</div>
								<div>
									<p className='text-sm text-muted-foreground'>Created</p>
									<p className='font-medium'>
										{format(new Date(classDetails.createdAt), 'MMM d, yyyy')}
									</p>
								</div>
							</div>
						</div>

						<Separator />

						<div className='flex items-center justify-between'>
							<div>
								<p className='text-sm text-muted-foreground'>Teacher</p>
								<p className='font-medium'>
									{classDetails.teacherName || 'Unknown'}
								</p>
							</div>
							<div className='text-right'>
								<p className='text-sm text-muted-foreground'>Students</p>
								<p className='font-medium'>
									{studentCount}
									{classDetails.maxStudents && ` / ${classDetails.maxStudents}`}
								</p>
							</div>
						</div>
					</CardContent>
				</Card>

				{/* Invite code card - only for teachers */}
				{isTeacher && classDetails.inviteCode && (
					<Card>
						<CardHeader>
							<CardTitle className='text-lg'>Invite Code</CardTitle>
							<CardDescription>
								Share this code with students to join
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-4'>
							<div className='flex items-center justify-between bg-muted rounded-lg p-4'>
								<code className='font-mono text-2xl font-bold tracking-[0.3em]'>
									{classDetails.inviteCode}
								</code>
								<Button variant='ghost' size='icon' onClick={copyInviteCode}>
									<Copy className='h-5 w-5' />
								</Button>
							</div>

							<div className='flex items-center gap-2 text-sm'>
								<Clock className='h-4 w-4 text-muted-foreground' />
								{isCodeExpired ? (
									<span className='text-destructive'>Code has expired</span>
								) : (
									<span className='text-muted-foreground'>
										Expires{' '}
										{format(
											new Date(classDetails.inviteCodeExpiresAt!),
											'MMM d, yyyy'
										)}
									</span>
								)}
							</div>

							<Button
								variant='outline'
								className='w-full'
								onClick={handleRegenerateCode}
								disabled={regenerateCode.isPending}
							>
								{regenerateCode.isPending ? (
									<RefreshCw className='h-4 w-4 mr-2 animate-spin' />
								) : (
									<RefreshCw className='h-4 w-4 mr-2' />
								)}
								Regenerate Code
							</Button>
						</CardContent>
					</Card>
				)}
			</div>

			{/* Class roster */}
			<Card>
				<CardHeader>
					<CardTitle>Class Roster</CardTitle>
					<CardDescription>All members enrolled in this class</CardDescription>
				</CardHeader>
				<CardContent>
					<StudentList members={classDetails.members} showRole={isTeacher} />
				</CardContent>
			</Card>
		</div>
	)
}

export default ClassDetailPage
