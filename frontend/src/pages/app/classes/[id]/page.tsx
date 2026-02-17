import React from 'react'
import { useParams, Link, useNavigate } from 'react-router'
import {
	useClassDetails,
	useRegenerateInviteCode,
	useClassAssignments,
} from '@/hooks/useClasses'
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
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import {
	ArrowLeft,
	BookOpen,
	Building,
	Calendar,
	CheckCircle2,
	Clock,
	Copy,
	FileText,
	AlertTriangle,
	RefreshCw,
	Users,
} from 'lucide-react'
import { format } from 'date-fns'
import { toast } from 'sonner'
import { useTranslation } from 'react-i18next'

const ClassDetailPage: React.FC = () => {
	const { t } = useTranslation()
	const { id } = useParams<{ id: string }>()
	const navigate = useNavigate()
	const { user } = useMe()

	const { data: classDetails, isLoading, error } = useClassDetails(id || '')
	const { data: assignments, isLoading: assignmentsLoading } =
		useClassAssignments(id || '')
	const regenerateCode = useRegenerateInviteCode()

	const isTeacher =
		user?.role?.name === 'Teacher' &&
		classDetails?.homeroomTeacherId === user?.cognitoId

	const copyInviteCode = () => {
		if (classDetails?.inviteCode) {
			navigator.clipboard.writeText(classDetails.inviteCode)
			toast.success(t('pages.classes.detail.invite_copied'))
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
					{t('pages.classes.detail.back')}
				</Button>
				<div className='text-center py-12'>
					<p className='text-destructive'>
						{error?.message || t('pages.classes.detail.not_found')}
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
					{t('pages.classes.detail.back_to_classes')}
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
							<Badge variant='secondary'>
								{t('pages.classes.detail.archived')}
							</Badge>
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
						<CardTitle>{t('pages.classes.detail.info_title')}</CardTitle>
					</CardHeader>
					<CardContent className='space-y-4'>
						<div className='grid gap-4 sm:grid-cols-2'>
							<div className='flex items-center gap-3'>
								<div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10'>
									<BookOpen className='h-5 w-5 text-primary' />
								</div>
								<div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.classes.detail.subject')}
									</p>
									<p className='font-medium'>
										{classDetails.subject ||
											t('pages.classes.detail.not_specified')}
									</p>
								</div>
							</div>

							<div className='flex items-center gap-3'>
								<div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10'>
									<Users className='h-5 w-5 text-primary' />
								</div>
								<div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.classes.detail.grade_level')}
									</p>
									<p className='font-medium'>
										{t('pages.classes.detail.grade_label', {
											grade: classDetails.grade,
										})}
									</p>
								</div>
							</div>

							{classDetails.school && (
								<div className='flex items-center gap-3'>
									<div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10'>
										<Building className='h-5 w-5 text-primary' />
									</div>
									<div>
										<p className='text-sm text-muted-foreground'>
											{t('pages.classes.detail.school')}
										</p>
										<p className='font-medium'>{classDetails.school}</p>
									</div>
								</div>
							)}

							<div className='flex items-center gap-3'>
								<div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10'>
									<Calendar className='h-5 w-5 text-primary' />
								</div>
								<div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.classes.detail.created')}
									</p>
									<p className='font-medium'>
										{format(new Date(classDetails.createdAt), 'MMM d, yyyy')}
									</p>
								</div>
							</div>
						</div>

						<Separator />

						<div className='flex items-center justify-between'>
							<div>
								<p className='text-sm text-muted-foreground'>
									{t('pages.classes.detail.teacher')}
								</p>
								<p className='font-medium'>
									{classDetails.teacherName ||
										t('pages.classes.detail.teacher_unknown')}
								</p>
							</div>
							<div className='text-right'>
								<p className='text-sm text-muted-foreground'>
									{t('pages.classes.detail.students')}
								</p>
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
							<CardTitle className='text-lg'>
								{t('pages.classes.detail.invite_title')}
							</CardTitle>
							<CardDescription>
								{t('pages.classes.detail.invite_description')}
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
									<span className='text-destructive'>
										{t('pages.classes.detail.code_expired')}
									</span>
								) : (
									<span className='text-muted-foreground'>
										{t('pages.classes.detail.expires')}{' '}
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
								{t('pages.classes.detail.regenerate')}
							</Button>
						</CardContent>
					</Card>
				)}
			</div>

			{/* Class roster */}
			<Card>
				<CardHeader>
					<CardTitle>{t('pages.classes.detail.roster_title')}</CardTitle>
					<CardDescription>
						{t('pages.classes.detail.roster_description')}
					</CardDescription>
				</CardHeader>
				<CardContent>
					<StudentList members={classDetails.members} showRole={isTeacher} />
				</CardContent>
			</Card>

			{/* Assignments */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						<FileText className='h-5 w-5' />
						{t('pages.classes.detail.assignments_title')}
					</CardTitle>
					<CardDescription>
						{t('pages.classes.detail.assignments_description')}
					</CardDescription>
				</CardHeader>
				<CardContent>
					{assignmentsLoading ? (
						<Skeleton className='h-32 w-full' />
					) : !assignments || assignments.length === 0 ? (
						<div className='flex flex-col items-center justify-center py-8 gap-2'>
							<FileText className='h-8 w-8 text-muted-foreground' />
							<p className='font-medium'>
								{t('pages.classes.detail.no_assignments')}
							</p>
							<p className='text-sm text-muted-foreground'>
								{t('pages.classes.detail.no_assignments_description')}
							</p>
						</div>
					) : (
						<div className='rounded-md border'>
							<Table>
								<TableHeader>
									<TableRow>
										<TableHead>
											{t('pages.classes.detail.exam_label')}
										</TableHead>
										<TableHead>
											{t('pages.classes.detail.start_date')}
										</TableHead>
										<TableHead>{t('pages.classes.detail.due_date')}</TableHead>
										<TableHead>{t('pages.classes.detail.weight')}</TableHead>
										<TableHead>{t('pages.classes.detail.mandatory')}</TableHead>
										<TableHead>
											{t('pages.dashboard.classes.detail.status')}
										</TableHead>
									</TableRow>
								</TableHeader>
								<TableBody>
									{assignments.map(assignment => (
										<TableRow key={assignment.id}>
											<TableCell className='font-mono text-sm'>
												{assignment.examId}
											</TableCell>
											<TableCell className='text-muted-foreground'>
												{format(
													new Date(assignment.startDate),
													'MMM d, yyyy h:mm a'
												)}
											</TableCell>
											<TableCell className='text-muted-foreground'>
												{format(
													new Date(assignment.dueDate),
													'MMM d, yyyy h:mm a'
												)}
											</TableCell>
											<TableCell>{assignment.weight}%</TableCell>
											<TableCell>
												<Badge
													variant={
														assignment.isMandatory ? 'default' : 'outline'
													}
												>
													{assignment.isMandatory
														? t('pages.classes.detail.mandatory')
														: t('pages.classes.detail.optional')}
												</Badge>
											</TableCell>
											<TableCell>
												{assignment.isOverdue ? (
													<div className='flex items-center gap-1 text-orange-600'>
														<AlertTriangle className='h-3.5 w-3.5' />
														{t('pages.classes.detail.status_overdue')}
													</div>
												) : assignment.isActive ? (
													<div className='flex items-center gap-1 text-green-600'>
														<CheckCircle2 className='h-3.5 w-3.5' />
														{t('pages.classes.detail.status_active')}
													</div>
												) : (
													<div className='flex items-center gap-1 text-blue-600'>
														<Calendar className='h-3.5 w-3.5' />
														{t('pages.classes.detail.status_upcoming')}
													</div>
												)}
											</TableCell>
										</TableRow>
									))}
								</TableBody>
							</Table>
						</div>
					)}
				</CardContent>
			</Card>
		</div>
	)
}

export default ClassDetailPage
