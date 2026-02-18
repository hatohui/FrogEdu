import React, { useState } from 'react'
import { useParams, Link, useNavigate } from 'react-router'
import {
	useClassDetail,
	useAssignExam,
	useAdminAssignExam,
	useRemoveStudent,
	useUpdateAssignment,
	useDeleteAssignment,
} from '@/hooks/useClasses'
import { useExamNames } from '@/hooks/useExams'
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
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuSeparator,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import {
	ArrowLeft,
	Calendar,
	CheckCircle2,
	ClipboardPlus,
	Copy,
	FileText,
	AlertTriangle,
	Users,
	MoreVertical,
	UserMinus,
	Edit,
	Trash2,
} from 'lucide-react'
import { format } from 'date-fns'
import { toast } from 'sonner'
import { useTranslation } from 'react-i18next'
import type {
	AssignExamRequest,
	UpdateAssignmentRequest,
} from '@/types/dtos/classes'
import type { ClassAssignment } from '@/types/model/class-service'
import type { GetMeResponse } from '@/types/dtos/users/user'
import UserAvatar from '@/components/common/UserAvatar'
import { ExamSelector } from '@/components/classes/ExamSelector'

const ClassDetailPage: React.FC = () => {
	const { t } = useTranslation()
	const { id } = useParams<{ id: string }>()
	const navigate = useNavigate()
	const { user } = useMe()

	const { data: classDetail, isLoading, error } = useClassDetail(id || '')

	const assignmentExamIds = classDetail?.assignments.map(a => a.examId) ?? []
	const { data: examNames = {} } = useExamNames(assignmentExamIds)
	const assignExamMutation = useAssignExam()
	const adminAssignExamMutation = useAdminAssignExam()
	const removeStudentMutation = useRemoveStudent()
	const updateAssignmentMutation = useUpdateAssignment()
	const deleteAssignmentMutation = useDeleteAssignment()

	// Assign exam dialog state
	const [assignDialogOpen, setAssignDialogOpen] = useState(false)
	const [examId, setExamId] = useState('')
	const [startDate, setStartDate] = useState('')
	const [dueDate, setDueDate] = useState('')
	const [isMandatory, setIsMandatory] = useState(true)
	const [weight, setWeight] = useState('100')
	const [retryTimes, setRetryTimes] = useState('0')
	const [isRetryable, setIsRetryable] = useState(false)
	const [shouldShuffleQuestions, setShouldShuffleQuestions] = useState(false)
	const [shouldShuffleAnswers, setShouldShuffleAnswers] = useState(false)
	const [allowPartialScoring, setAllowPartialScoring] = useState(true)

	// Remove student confirmation dialog
	const [removeDialogOpen, setRemoveDialogOpen] = useState(false)
	const [studentToRemove, setStudentToRemove] = useState<{
		id: string
		name: string
	} | null>(null)

	// Edit assignment dialog state
	const [editAssignDialogOpen, setEditAssignDialogOpen] = useState(false)
	const [editingAssignment, setEditingAssignment] =
		useState<ClassAssignment | null>(null)

	// Delete assignment confirmation dialog state
	const [deleteAssignDialogOpen, setDeleteAssignDialogOpen] = useState(false)
	const [assignmentToDelete, setAssignmentToDelete] =
		useState<ClassAssignment | null>(null)

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

	const handleOpenAssignDialog = () => {
		setExamId('')
		setStartDate('')
		setDueDate('')
		setIsMandatory(true)
		setWeight('100')
		setRetryTimes('0')
		setIsRetryable(false)
		setShouldShuffleQuestions(false)
		setShouldShuffleAnswers(false)
		setAllowPartialScoring(true)
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
			retryTimes: isRetryable ? Number(retryTimes) || 0 : 0,
			isRetryable,
			shouldShuffleQuestions,
			shouldShuffleAnswers,
			allowPartialScoring,
		}

		const mutation = isAdmin ? adminAssignExamMutation : assignExamMutation
		mutation.mutate(
			{ classId: id, data },
			{
				onSuccess: () => {
					setAssignDialogOpen(false)
				},
			}
		)
	}

	const handleRemoveStudent = () => {
		if (!id || !studentToRemove) return

		removeStudentMutation.mutate(
			{ classId: id, studentId: studentToRemove.id },
			{
				onSuccess: () => {
					setRemoveDialogOpen(false)
					setStudentToRemove(null)
				},
			}
		)
	}

	const openRemoveDialog = (studentId: string, studentName: string) => {
		setStudentToRemove({ id: studentId, name: studentName })
		setRemoveDialogOpen(true)
	}

	const handleOpenEditAssignDialog = (assignment: ClassAssignment) => {
		setEditingAssignment(assignment)
		setStartDate(format(new Date(assignment.startDate), "yyyy-MM-dd'T'HH:mm"))
		setDueDate(format(new Date(assignment.dueDate), "yyyy-MM-dd'T'HH:mm"))
		setIsMandatory(assignment.isMandatory)
		setWeight(String(assignment.weight))
		setRetryTimes('0')
		setIsRetryable(false)
		setShouldShuffleQuestions(false)
		setShouldShuffleAnswers(false)
		setAllowPartialScoring(true)
		setEditAssignDialogOpen(true)
	}

	const handleUpdateAssignment = () => {
		if (!id || !editingAssignment || !startDate || !dueDate) return
		const data: UpdateAssignmentRequest = {
			startDate: new Date(startDate).toISOString(),
			dueDate: new Date(dueDate).toISOString(),
			isMandatory,
			weight: Number(weight) || 100,
			retryTimes: isRetryable ? Number(retryTimes) || 0 : 0,
			isRetryable,
			shouldShuffleQuestions,
			shouldShuffleAnswers,
			allowPartialScoring,
		}
		updateAssignmentMutation.mutate(
			{ classId: id, assignmentId: editingAssignment.id, data },
			{
				onSuccess: () => {
					setEditAssignDialogOpen(false)
					setEditingAssignment(null)
				},
			}
		)
	}

	const handleDeleteAssignment = () => {
		if (!id || !assignmentToDelete) return
		deleteAssignmentMutation.mutate(
			{ classId: id, assignmentId: assignmentToDelete.id },
			{
				onSuccess: () => {
					setDeleteAssignDialogOpen(false)
					setAssignmentToDelete(null)
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
				<div className='flex gap-2'>
					{canManage && (
						<>
							<Button onClick={handleOpenAssignDialog}>
								<ClipboardPlus className='h-4 w-4 mr-2' />
								{t('pages.dashboard.classes.actions.assign_exam')}
							</Button>
							<Button variant='outline'>
								<Edit className='h-4 w-4 mr-2' />
								{t('pages.classes.detail.edit_details')}
							</Button>
						</>
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

				{/* Invite code card - for teachers and admins only */}
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
										<TableHead>{t('pages.classes.detail.student')}</TableHead>
										<TableHead>
											{t('pages.dashboard.classes.detail.joined_at')}
										</TableHead>
										<TableHead>
											{t('pages.dashboard.classes.detail.status')}
										</TableHead>
										{canManage && <TableHead className='w-[50px]'></TableHead>}
									</TableRow>
								</TableHeader>
								<TableBody>
									{classDetail.enrollments.map(enrollment => {
										const studentName = `${enrollment.studentFirstName} ${enrollment.studentLastName}`
										const studentUser: GetMeResponse = {
											id: enrollment.studentId,
											cognitoId: enrollment.studentId,
											email: '',
											firstName: enrollment.studentFirstName,
											lastName: enrollment.studentLastName,
											roleId: '',
											avatarUrl: enrollment.studentAvatarUrl,
											isEmailVerified: false,
											createdAt: enrollment.joinedAt,
											updatedAt: null,
										}
										return (
											<TableRow key={enrollment.id}>
												<TableCell>
													<div className='flex items-center gap-3'>
														<UserAvatar user={studentUser} />
														<div className='font-medium'>{studentName}</div>
													</div>
												</TableCell>
												<TableCell className='text-muted-foreground'>
													{format(new Date(enrollment.joinedAt), 'MMM d, yyyy')}
												</TableCell>
												<TableCell>
													{getEnrollmentStatusBadge(enrollment.status)}
												</TableCell>
												{canManage && (
													<TableCell>
														<DropdownMenu>
															<DropdownMenuTrigger asChild>
																<Button variant='ghost' size='icon'>
																	<MoreVertical className='h-4 w-4' />
																</Button>
															</DropdownMenuTrigger>
															<DropdownMenuContent align='end'>
																<DropdownMenuItem
																	onClick={() => navigate(`/app/classes/${id}`)}
																>
																	{t('pages.classes.detail.view_profile')}
																</DropdownMenuItem>
																<DropdownMenuSeparator />
																<DropdownMenuItem
																	className='text-destructive'
																	onClick={() =>
																		openRemoveDialog(
																			enrollment.studentId,
																			studentName
																		)
																	}
																>
																	<UserMinus className='h-4 w-4 mr-2' />
																	{t('pages.classes.detail.remove_from_class')}
																</DropdownMenuItem>
															</DropdownMenuContent>
														</DropdownMenu>
													</TableCell>
												)}
											</TableRow>
										)
									})}
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
										{canManage && <TableHead className='w-[50px]' />}
									</TableRow>
								</TableHeader>
								<TableBody>
									{classDetail.assignments.map(assignment => (
										<TableRow key={assignment.id}>
											<TableCell className='font-medium'>
												{canManage ? (
													<Link
														to={`/app/exams/${assignment.examId}`}
														className='hover:underline text-primary'
													>
														{examNames[assignment.examId] ?? assignment.examId}
													</Link>
												) : (
													<span>
														{examNames[assignment.examId] ?? assignment.examId}
													</span>
												)}
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
											{canManage && (
												<TableCell>
													<DropdownMenu>
														<DropdownMenuTrigger asChild>
															<Button
																variant='ghost'
																size='icon'
																className='h-8 w-8'
															>
																<MoreVertical className='h-4 w-4' />
															</Button>
														</DropdownMenuTrigger>
														<DropdownMenuContent align='end'>
															<DropdownMenuItem
																onClick={() =>
																	handleOpenEditAssignDialog(assignment)
																}
															>
																<Edit className='mr-2 h-4 w-4' />
																Edit
															</DropdownMenuItem>
															<DropdownMenuSeparator />
															<DropdownMenuItem
																className='text-destructive focus:text-destructive'
																onClick={() => {
																	setAssignmentToDelete(assignment)
																	setDeleteAssignDialogOpen(true)
																}}
															>
																<Trash2 className='mr-2 h-4 w-4' />
																Delete
															</DropdownMenuItem>
														</DropdownMenuContent>
													</DropdownMenu>
												</TableCell>
											)}
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
							<Label>
								{t('pages.dashboard.classes.assign_dialog.exam_id')}
							</Label>
							<ExamSelector value={examId} onValueChange={setExamId} />
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
					{/* Session settings */}
					<div className='grid grid-cols-2 gap-3'>
						<div className='flex items-center gap-2'>
							<input
								type='checkbox'
								id='retryable-c'
								checked={isRetryable}
								onChange={e => setIsRetryable(e.target.checked)}
								className='h-4 w-4 rounded border-gray-300'
							/>
							<Label htmlFor='retryable-c'>Allow retries</Label>
						</div>
						{isRetryable && (
							<div className='grid gap-1'>
								<Label htmlFor='retrytimes-c'>Max retries</Label>
								<Input
									id='retrytimes-c'
									type='number'
									min={1}
									value={retryTimes}
									onChange={e => setRetryTimes(e.target.value)}
								/>
							</div>
						)}
						<div className='flex items-center gap-2'>
							<input
								type='checkbox'
								id='shuffle-q-c'
								checked={shouldShuffleQuestions}
								onChange={e => setShouldShuffleQuestions(e.target.checked)}
								className='h-4 w-4 rounded border-gray-300'
							/>
							<Label htmlFor='shuffle-q-c'>Shuffle questions</Label>
						</div>
						<div className='flex items-center gap-2'>
							<input
								type='checkbox'
								id='shuffle-a-c'
								checked={shouldShuffleAnswers}
								onChange={e => setShouldShuffleAnswers(e.target.checked)}
								className='h-4 w-4 rounded border-gray-300'
							/>
							<Label htmlFor='shuffle-a-c'>Shuffle answers</Label>
						</div>
						<div className='flex items-center gap-2'>
							<input
								type='checkbox'
								id='partial-c'
								checked={allowPartialScoring}
								onChange={e => setAllowPartialScoring(e.target.checked)}
								className='h-4 w-4 rounded border-gray-300'
							/>
							<Label htmlFor='partial-c'>Partial scoring</Label>
						</div>
					</div>
					<DialogFooter>
						<Button
							onClick={handleAssignExam}
							disabled={assignExamMutation.isPending}
						>
							{assignExamMutation.isPending
								? t('pages.dashboard.classes.assign_dialog.submitting')
								: t('pages.dashboard.classes.assign_dialog.submit')}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Edit Assignment Dialog */}
			<Dialog
				open={editAssignDialogOpen}
				onOpenChange={setEditAssignDialogOpen}
			>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>Edit Assignment</DialogTitle>
						<DialogDescription>
							Update the assignment settings for this exam.
						</DialogDescription>
					</DialogHeader>
					<div className='grid gap-4 py-4'>
						<div className='grid grid-cols-2 gap-4'>
							<div className='grid gap-2'>
								<Label htmlFor='edit-startDate'>
									{t('pages.dashboard.classes.assign_dialog.start_date')}
								</Label>
								<Input
									id='edit-startDate'
									type='datetime-local'
									value={startDate}
									onChange={e => setStartDate(e.target.value)}
								/>
							</div>
							<div className='grid gap-2'>
								<Label htmlFor='edit-dueDate'>
									{t('pages.dashboard.classes.assign_dialog.due_date')}
								</Label>
								<Input
									id='edit-dueDate'
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
									id='edit-mandatory'
									checked={isMandatory}
									onChange={e => setIsMandatory(e.target.checked)}
									className='h-4 w-4 rounded border-gray-300'
								/>
								<Label htmlFor='edit-mandatory'>
									{t('pages.dashboard.classes.assign_dialog.mandatory')}
								</Label>
							</div>
							<div className='grid gap-2 flex-1'>
								<Label htmlFor='edit-weight'>
									{t('pages.dashboard.classes.assign_dialog.weight')}
								</Label>
								<Input
									id='edit-weight'
									type='number'
									min={0}
									max={100}
									value={weight}
									onChange={e => setWeight(e.target.value)}
								/>
							</div>
						</div>
					</div>
					{/* Session settings */}
					<div className='grid grid-cols-2 gap-3'>
						<div className='flex items-center gap-2'>
							<input
								type='checkbox'
								id='edit-retryable'
								checked={isRetryable}
								onChange={e => setIsRetryable(e.target.checked)}
								className='h-4 w-4 rounded border-gray-300'
							/>
							<Label htmlFor='edit-retryable'>Allow retries</Label>
						</div>
						{isRetryable && (
							<div className='grid gap-1'>
								<Label htmlFor='edit-retrytimes'>Max retries</Label>
								<Input
									id='edit-retrytimes'
									type='number'
									min={1}
									value={retryTimes}
									onChange={e => setRetryTimes(e.target.value)}
								/>
							</div>
						)}
						<div className='flex items-center gap-2'>
							<input
								type='checkbox'
								id='edit-shuffle-q'
								checked={shouldShuffleQuestions}
								onChange={e => setShouldShuffleQuestions(e.target.checked)}
								className='h-4 w-4 rounded border-gray-300'
							/>
							<Label htmlFor='edit-shuffle-q'>Shuffle questions</Label>
						</div>
						<div className='flex items-center gap-2'>
							<input
								type='checkbox'
								id='edit-shuffle-a'
								checked={shouldShuffleAnswers}
								onChange={e => setShouldShuffleAnswers(e.target.checked)}
								className='h-4 w-4 rounded border-gray-300'
							/>
							<Label htmlFor='edit-shuffle-a'>Shuffle answers</Label>
						</div>
						<div className='flex items-center gap-2'>
							<input
								type='checkbox'
								id='edit-partial'
								checked={allowPartialScoring}
								onChange={e => setAllowPartialScoring(e.target.checked)}
								className='h-4 w-4 rounded border-gray-300'
							/>
							<Label htmlFor='edit-partial'>Partial scoring</Label>
						</div>
					</div>
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => setEditAssignDialogOpen(false)}
						>
							{t('common.cancel')}
						</Button>
						<Button
							onClick={handleUpdateAssignment}
							disabled={
								!startDate || !dueDate || updateAssignmentMutation.isPending
							}
						>
							{updateAssignmentMutation.isPending
								? t('pages.dashboard.classes.assign_dialog.submitting')
								: 'Save changes'}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Delete Assignment Confirmation Dialog */}
			<Dialog
				open={deleteAssignDialogOpen}
				onOpenChange={setDeleteAssignDialogOpen}
			>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>Delete Assignment</DialogTitle>
						<DialogDescription>
							Are you sure you want to remove this assignment? This will also
							delete the linked exam session.
						</DialogDescription>
					</DialogHeader>
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => {
								setDeleteAssignDialogOpen(false)
								setAssignmentToDelete(null)
							}}
						>
							{t('common.cancel')}
						</Button>
						<Button
							variant='destructive'
							onClick={handleDeleteAssignment}
							disabled={deleteAssignmentMutation.isPending}
						>
							{deleteAssignmentMutation.isPending ? 'Deleting…' : 'Delete'}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Remove Student Confirmation Dialog */}
			<Dialog open={removeDialogOpen} onOpenChange={setRemoveDialogOpen}>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>
							{t('pages.classes.detail.remove_student_title')}
						</DialogTitle>
						<DialogDescription>
							{t('pages.classes.detail.remove_student_description', {
								name: studentToRemove?.name,
							})}
						</DialogDescription>
					</DialogHeader>
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => {
								setRemoveDialogOpen(false)
								setStudentToRemove(null)
							}}
						>
							{t('common.cancel')}
						</Button>
						<Button
							variant='destructive'
							onClick={handleRemoveStudent}
							disabled={removeStudentMutation.isPending}
						>
							{removeStudentMutation.isPending
								? t('pages.classes.detail.removing')
								: t('pages.classes.detail.remove_student')}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>
		</div>
	)
}

export default ClassDetailPage
