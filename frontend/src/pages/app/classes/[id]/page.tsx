import React, { useState } from 'react'
import { useParams, Link, useNavigate } from 'react-router'
import {
	useClassDetailTyped,
	useRegenerateInviteCode,
	useAssignExam,
} from '@/hooks/useClasses'
import { useMe } from '@/hooks/auth/useMe'
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
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogFooter,
	DialogHeader,
	DialogTitle,
} from '@/components/ui/dialog'
import {
	ArrowLeft,
	Calendar,
	CheckCircle2,
	ClipboardPlus,
	Copy,
	FileText,
	AlertTriangle,
	RefreshCw,
	Users,
} from 'lucide-react'
import { format } from 'date-fns'
import { toast } from 'sonner'
import { useTranslation } from 'react-i18next'
import type { AssignExamRequest } from '@/types/dtos/classes'

const ClassDetailPage: React.FC = () => {
	const { t } = useTranslation()
	const { id } = useParams<{ id: string }>()
	const navigate = useNavigate()
	const { user } = useMe()

	const { data: classDetail, isLoading, error } = useClassDetailTyped(id || '')
	const regenerateCode = useRegenerateInviteCode()
	const assignExamMutation = useAssignExam()

	// Assign exam dialog state
	const [assignDialogOpen, setAssignDialogOpen] = useState(false)
	const [examId, setExamId] = useState('')
	const [startDate, setStartDate] = useState('')
	const [dueDate, setDueDate] = useState('')
	const [isMandatory, setIsMandatory] = useState(true)
	const [weight, setWeight] = useState('100')

	const isTeacher =
		user?.role?.name === 'Teacher' && classDetail?.teacherId === user?.cognitoId

	const isAdmin = user?.role?.name === 'Admin'

	const canManage = isTeacher || isAdmin

	const copyInviteCode = () => {
		if (classDetail?.inviteCode) {
			navigator.clipboard.writeText(classDetail.inviteCode)
			toast.success(t('pages.classes.detail.invite_copied'))
		}
	}

	const handleRegenerateCode = async () => {
		if (!id) return
		await regenerateCode.mutateAsync({ classId: id })
	}

	const handleOpenAssignDialog = () => {
		setExamId('')
		setStartDate('')
		setDueDate('')
		setIsMandatory(true)
		setWeight('100')
		setAssignDialogOpen(true)
	}

	const handleAssignExam = () => {
		if (!id || !examId || !startDate || !dueDate) return

		const data: AssignExamRequest = {
			examId,
			startDate: new Date(startDate).toISOString(),
			dueDate: new Date(dueDate).toISOString(),
			isMandatory,
			weight: Number(weight) || 100,
		}

		assignExamMutation.mutate(
			{ classId: id, data },
			{
				onSuccess: () => {
					setAssignDialogOpen(false)
				},
			}
		)
	}

	const getEnrollmentStatusBadge = (status: string) => {
		switch (status) {
			case 'Active':
				return (
					<Badge variant='default'>
						{t('pages.dashboard.classes.filter_active')}
					</Badge>
				)
			case 'Inactive':
				return (
					<Badge variant='secondary'>
						{t('pages.dashboard.classes.filter_inactive')}
					</Badge>
				)
			case 'Kicked':
				return <Badge variant='destructive'>{status}</Badge>
			case 'Withdrawn':
				return <Badge variant='outline'>{status}</Badge>
			default:
				return <Badge variant='outline'>{status}</Badge>
		}
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

	if (error || !classDetail) {
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

	const activeEnrollments = classDetail.enrollments.filter(
		e => e.status === 'Active'
	)

	return (
		<div className='p-6 space-y-6 max-w-5xl mx-auto'>
			{/* Back button */}
			<Link to='/app/classes'>
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
							{classDetail.name}
						</h1>
						{!classDetail.isActive && (
							<Badge variant='secondary'>
								{t('pages.dashboard.classes.filter_inactive')}
							</Badge>
						)}
					</div>
					<p className='text-muted-foreground mt-1'>
						{t('pages.classes.detail.grade_label', {
							grade: classDetail.grade,
						})}
					</p>
				</div>
				{canManage && (
					<Button onClick={handleOpenAssignDialog}>
						<ClipboardPlus className='h-4 w-4 mr-2' />
						{t('pages.dashboard.classes.actions.assign_exam')}
					</Button>
				)}
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
									<Users className='h-5 w-5 text-primary' />
								</div>
								<div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.classes.detail.grade_level')}
									</p>
									<p className='font-medium'>
										{t('pages.classes.detail.grade_label', {
											grade: classDetail.grade,
										})}
									</p>
								</div>
							</div>

							<div className='flex items-center gap-3'>
								<div className='flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10'>
									<Calendar className='h-5 w-5 text-primary' />
								</div>
								<div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.classes.detail.created')}
									</p>
									<p className='font-medium'>
										{format(new Date(classDetail.createdAt), 'MMM d, yyyy')}
									</p>
								</div>
							</div>
						</div>

						<Separator />

						<div className='flex items-center justify-between'>
							<div>
								<p className='text-sm text-muted-foreground'>
									{t('pages.dashboard.classes.table.students')}
								</p>
								<p className='font-medium'>
									{classDetail.studentCount} / {classDetail.maxStudents}
								</p>
							</div>
							<div className='text-right'>
								<p className='text-sm text-muted-foreground'>
									{t('pages.dashboard.classes.table.assignments')}
								</p>
								<p className='font-medium'>{classDetail.assignments.length}</p>
							</div>
						</div>
					</CardContent>
				</Card>

				{/* Invite code card - for teachers and admins */}
				{canManage && classDetail.inviteCode && (
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
									{classDetail.inviteCode}
								</code>
								<Button variant='ghost' size='icon' onClick={copyInviteCode}>
									<Copy className='h-5 w-5' />
								</Button>
							</div>

							{isTeacher && (
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
							)}
						</CardContent>
					</Card>
				)}
			</div>

			{/* Enrolled Students */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						<Users className='h-5 w-5' />
						{t('pages.dashboard.classes.detail.enrollments_tab')}
						<Badge variant='secondary' className='ml-2'>
							{activeEnrollments.length}
						</Badge>
					</CardTitle>
					<CardDescription>
						{t('pages.classes.detail.roster_description')}
					</CardDescription>
				</CardHeader>
				<CardContent>
					{classDetail.enrollments.length === 0 ? (
						<div className='flex flex-col items-center justify-center py-8 gap-2'>
							<Users className='h-8 w-8 text-muted-foreground' />
							<p className='text-muted-foreground'>
								{t('pages.dashboard.classes.detail.no_students')}
							</p>
						</div>
					) : (
						<div className='rounded-md border'>
							<Table>
								<TableHeader>
									<TableRow>
										<TableHead>
											{t('pages.dashboard.classes.detail.student_id')}
										</TableHead>
										<TableHead>
											{t('pages.dashboard.classes.detail.joined_at')}
										</TableHead>
										<TableHead>
											{t('pages.dashboard.classes.detail.status')}
										</TableHead>
									</TableRow>
								</TableHeader>
								<TableBody>
									{classDetail.enrollments.map(enrollment => (
										<TableRow key={enrollment.id}>
											<TableCell className='font-mono text-sm'>
												{enrollment.studentId}
											</TableCell>
											<TableCell className='text-muted-foreground'>
												{format(new Date(enrollment.joinedAt), 'MMM d, yyyy')}
											</TableCell>
											<TableCell>
												{getEnrollmentStatusBadge(enrollment.status)}
											</TableCell>
										</TableRow>
									))}
								</TableBody>
							</Table>
						</div>
					)}
				</CardContent>
			</Card>

			{/* Assignments & Upcoming Exams */}
			<Card>
				<CardHeader>
					<div className='flex items-center justify-between'>
						<div>
							<CardTitle className='flex items-center gap-2'>
								<FileText className='h-5 w-5' />
								{t('pages.classes.detail.assignments_title')}
								<Badge variant='secondary' className='ml-2'>
									{classDetail.assignments.length}
								</Badge>
							</CardTitle>
							<CardDescription>
								{t('pages.classes.detail.assignments_description')}
							</CardDescription>
						</div>
						{canManage && (
							<Button size='sm' onClick={handleOpenAssignDialog}>
								<ClipboardPlus className='mr-2 h-4 w-4' />
								{t('pages.dashboard.classes.actions.assign_exam')}
							</Button>
						)}
					</div>
				</CardHeader>
				<CardContent>
					{classDetail.assignments.length === 0 ? (
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
									{classDetail.assignments.map(assignment => (
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

			{/* Assign Exam Dialog */}
			<Dialog open={assignDialogOpen} onOpenChange={setAssignDialogOpen}>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>
							{t('pages.dashboard.classes.assign_dialog.title')}
						</DialogTitle>
						<DialogDescription>
							{t('pages.dashboard.classes.assign_dialog.description')}
						</DialogDescription>
					</DialogHeader>
					<div className='grid gap-4 py-4'>
						<div className='grid gap-2'>
							<Label htmlFor='examId'>
								{t('pages.dashboard.classes.assign_dialog.exam_id')}
							</Label>
							<Input
								id='examId'
								value={examId}
								onChange={e => setExamId(e.target.value)}
								placeholder={t(
									'pages.dashboard.classes.assign_dialog.exam_id_placeholder'
								)}
							/>
						</div>
						<div className='grid grid-cols-2 gap-4'>
							<div className='grid gap-2'>
								<Label htmlFor='startDate'>
									{t('pages.dashboard.classes.assign_dialog.start_date')}
								</Label>
								<Input
									id='startDate'
									type='datetime-local'
									value={startDate}
									onChange={e => setStartDate(e.target.value)}
								/>
							</div>
							<div className='grid gap-2'>
								<Label htmlFor='dueDate'>
									{t('pages.dashboard.classes.assign_dialog.due_date')}
								</Label>
								<Input
									id='dueDate'
									type='datetime-local'
									value={dueDate}
									onChange={e => setDueDate(e.target.value)}
								/>
							</div>
						</div>
						<div className='flex items-center gap-4'>
							<div className='flex items-center gap-2'>
								<input
									type='checkbox'
									id='mandatory-class'
									checked={isMandatory}
									onChange={e => setIsMandatory(e.target.checked)}
									className='h-4 w-4 rounded border-gray-300'
								/>
								<Label htmlFor='mandatory-class'>
									{t('pages.dashboard.classes.assign_dialog.mandatory')}
								</Label>
							</div>
							<div className='grid gap-2 flex-1'>
								<Label htmlFor='weight-class'>
									{t('pages.dashboard.classes.assign_dialog.weight')}
								</Label>
								<Input
									id='weight-class'
									type='number'
									min={0}
									max={100}
									value={weight}
									onChange={e => setWeight(e.target.value)}
									placeholder={t(
										'pages.dashboard.classes.assign_dialog.weight_placeholder'
									)}
								/>
							</div>
						</div>
					</div>
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => setAssignDialogOpen(false)}
						>
							{t('common.cancel')}
						</Button>
						<Button
							onClick={handleAssignExam}
							disabled={
								!examId ||
								!startDate ||
								!dueDate ||
								assignExamMutation.isPending
							}
						>
							{assignExamMutation.isPending
								? t('pages.dashboard.classes.assign_dialog.submitting')
								: t('pages.dashboard.classes.assign_dialog.submit')}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>
		</div>
	)
}

export default ClassDetailPage
