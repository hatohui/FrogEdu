import React, { useState } from 'react'
import { useNavigate, useParams } from 'react-router'
import { useTranslation } from 'react-i18next'
import {
	ArrowLeft,
	Users,
	FileText,
	Info,
	ClipboardPlus,
	GraduationCap,
	Calendar,
	CheckCircle2,
	XCircle,
	AlertTriangle,
	UserPlus,
} from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from '@/components/ui/table'
import { Badge } from '@/components/ui/badge'
import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogFooter,
	DialogHeader,
	DialogTitle,
} from '@/components/ui/dialog'
import { Label } from '@/components/ui/label'
import { Skeleton } from '@/components/ui/skeleton'
import {
	useClassDetail,
	useAdminAssignExam,
	useJoinClass,
} from '@/hooks/useClasses'
import { useMe } from '@/hooks/auth/useMe'
import { useUser } from '@/hooks/useUsers'
import type { AssignExamRequest } from '@/types/dtos/classes'

type Tab = 'info' | 'students' | 'assignments'

const ClassDetailPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const { id } = useParams<{ id: string }>()
	const { user } = useMe()
	// Use regular useClassDetail - backend already handles authorization
	const { data: classDetail, isLoading } = useClassDetail(id ?? '')

	// Fetch teacher data
	const { data: teacher } = useUser(classDetail?.teacherId ?? '')

	const [activeTab, setActiveTab] = useState<Tab>('info')
	const [assignDialogOpen, setAssignDialogOpen] = useState(false)

	// Assign exam form state
	const [examId, setExamId] = useState('')
	const [startDate, setStartDate] = useState('')
	const [dueDate, setDueDate] = useState('')
	const [isMandatory, setIsMandatory] = useState(true)
	const [weight, setWeight] = useState('100')

	const assignExamMutation = useAdminAssignExam()
	const joinClassMutation = useJoinClass()

	const canEnroll = user?.cognitoId !== classDetail?.teacherId

	const formatDate = (dateString: string) => {
		return new Date(dateString).toLocaleDateString('en-US', {
			year: 'numeric',
			month: 'short',
			day: 'numeric',
		})
	}

	const formatDateTime = (dateString: string) => {
		return new Date(dateString).toLocaleString('en-US', {
			year: 'numeric',
			month: 'short',
			day: 'numeric',
			hour: '2-digit',
			minute: '2-digit',
		})
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

	const handleJoinClass = () => {
		if (classDetail?.inviteCode) {
			joinClassMutation.mutate(classDetail.inviteCode)
		}
	}

	const getEnrollmentStatusBadge = (status: string) => {
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

	const tabs: { key: Tab; labelKey: string; icon: React.ElementType }[] = [
		{
			key: 'info',
			labelKey: 'pages.dashboard.classes.detail.info_tab',
			icon: Info,
		},
		{
			key: 'students',
			labelKey: 'pages.dashboard.classes.detail.enrollments_tab',
			icon: Users,
		},
		{
			key: 'assignments',
			labelKey: 'pages.dashboard.classes.detail.assignments_tab',
			icon: FileText,
		},
	]

	if (isLoading) {
		return (
			<div className='space-y-6 p-6'>
				<div className='flex items-center gap-4'>
					<Skeleton className='h-9 w-9' />
					<div>
						<Skeleton className='h-8 w-48' />
						<Skeleton className='h-4 w-32 mt-1' />
					</div>
				</div>
				<Card>
					<CardContent className='pt-6'>
						<Skeleton className='h-64 w-full' />
					</CardContent>
				</Card>
			</div>
		)
	}

	if (!classDetail) {
		return (
			<div className='flex flex-col items-center justify-center p-12 gap-4'>
				<GraduationCap className='h-12 w-12 text-muted-foreground' />
				<p className='text-lg font-medium text-muted-foreground'>
					{t('pages.dashboard.classes.empty.title')}
				</p>
				<Button
					variant='outline'
					onClick={() => navigate('/dashboard/classes')}
				>
					<ArrowLeft className='mr-2 h-4 w-4' />
					{t('pages.dashboard.classes.detail.back')}
				</Button>
			</div>
		)
	}

	return (
		<div className='space-y-6 p-6'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div className='flex items-center gap-4'>
					<Button
						variant='ghost'
						size='icon'
						onClick={() => navigate('/dashboard/classes')}
					>
						<ArrowLeft className='h-4 w-4' />
					</Button>
					<div>
						<h1 className='text-3xl font-bold tracking-tight'>
							{classDetail.name}
						</h1>
						<p className='text-muted-foreground'>
							{t('pages.dashboard.classes.detail.title')} &middot;{' '}
							<Badge variant='outline'>{classDetail.grade}</Badge>
						</p>
					</div>
				</div>
				<div className='flex items-center gap-2'>
					{classDetail.isActive ? (
						<Badge variant='default'>
							{t('pages.dashboard.classes.filter_active')}
						</Badge>
					) : (
						<Badge variant='secondary'>
							{t('pages.dashboard.classes.filter_inactive')}
						</Badge>
					)}
					{canEnroll && classDetail.isActive && (
						<Button onClick={handleJoinClass}>
							<UserPlus className='h-4 w-4 mr-2' />
							{t('pages.classes.actions.join')}
						</Button>
					)}
				</div>
			</div>

			{/* Tab Navigation */}
			<div className='flex gap-1 border-b'>
				{tabs.map(tab => {
					const Icon = tab.icon
					return (
						<button
							key={tab.key}
							onClick={() => setActiveTab(tab.key)}
							className={`flex items-center gap-2 px-4 py-2 text-sm font-medium border-b-2 transition-colors ${
								activeTab === tab.key
									? 'border-primary text-primary'
									: 'border-transparent text-muted-foreground hover:text-foreground'
							}`}
						>
							<Icon className='h-4 w-4' />
							{t(tab.labelKey)}
						</button>
					)
				})}
			</div>

			{/* Tab Content */}
			{activeTab === 'info' && (
				<Card>
					<CardHeader>
						<CardTitle className='flex items-center gap-2'>
							<Info className='h-5 w-5' />
							{t('pages.dashboard.classes.detail.info_tab')}
						</CardTitle>
					</CardHeader>
					<CardContent>
						<div className='grid gap-4 md:grid-cols-2'>
							<div className='space-y-3'>
								<div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.dashboard.classes.table.name')}
									</p>
									<p className='font-medium'>{classDetail.name}</p>
								</div>
								<div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.dashboard.classes.table.grade')}
									</p>
									<p className='font-medium'>{classDetail.grade}</p>
								</div>
								<div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.dashboard.classes.table.invite_code')}
									</p>
									<code className='text-sm bg-muted px-2 py-1 rounded'>
										{classDetail.inviteCode}
									</code>
								</div>
							</div>
							<div className='space-y-3'>
								<div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.dashboard.classes.table.teacher')}
									</p>
									<p className='font-medium'>
										{teacher
											? `${teacher.firstName} ${teacher.lastName}`
											: classDetail.teacherId}
									</p>
								</div>
								<div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.dashboard.classes.table.students')}
									</p>
									<p className='font-medium'>
										{classDetail.studentCount} / {classDetail.maxStudents}
									</p>
								</div>
								<div>
									<p className='text-sm text-muted-foreground'>
										{t('pages.dashboard.classes.table.created')}
									</p>
									<p className='font-medium'>
										{formatDate(classDetail.createdAt)}
									</p>
								</div>
							</div>
						</div>
					</CardContent>
				</Card>
			)}

			{activeTab === 'students' && (
				<Card>
					<CardHeader>
						<CardTitle className='flex items-center gap-2'>
							<Users className='h-5 w-5' />
							{t('pages.dashboard.classes.detail.enrollments_tab')}
							<Badge variant='secondary' className='ml-2'>
								{classDetail.enrollments.length}
							</Badge>
						</CardTitle>
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
												{t('pages.dashboard.classes.detail.student')}
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
												<TableCell>
													<div className='flex items-center gap-3'>
														{enrollment.studentAvatarUrl ? (
															<img
																src={enrollment.studentAvatarUrl}
																alt=''
																className='h-8 w-8 rounded-full object-cover'
															/>
														) : (
															<div className='h-8 w-8 rounded-full bg-muted flex items-center justify-center text-xs font-medium text-muted-foreground'>
																{enrollment.studentFirstName?.[0]}
																{enrollment.studentLastName?.[0]}
															</div>
														)}
														<span className='font-medium'>
															{enrollment.studentFirstName}{' '}
															{enrollment.studentLastName}
														</span>
													</div>
												</TableCell>
												<TableCell className='text-muted-foreground'>
													{formatDate(enrollment.joinedAt)}
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
			)}

			{activeTab === 'assignments' && (
				<Card>
					<CardHeader>
						<div className='flex items-center justify-between'>
							<CardTitle className='flex items-center gap-2'>
								<FileText className='h-5 w-5' />
								{t('pages.dashboard.classes.detail.assignments_tab')}
								<Badge variant='secondary' className='ml-2'>
									{classDetail.assignments.length}
								</Badge>
							</CardTitle>
							<Button size='sm' onClick={handleOpenAssignDialog}>
								<ClipboardPlus className='mr-2 h-4 w-4' />
								{t('pages.dashboard.classes.actions.assign_exam')}
							</Button>
						</div>
					</CardHeader>
					<CardContent>
						{classDetail.assignments.length === 0 ? (
							<div className='flex flex-col items-center justify-center py-8 gap-2'>
								<FileText className='h-8 w-8 text-muted-foreground' />
								<p className='text-muted-foreground'>
									{t('pages.dashboard.classes.detail.no_assignments')}
								</p>
								<Button
									variant='outline'
									size='sm'
									onClick={handleOpenAssignDialog}
								>
									<ClipboardPlus className='mr-2 h-4 w-4' />
									{t('pages.dashboard.classes.actions.assign_exam')}
								</Button>
							</div>
						) : (
							<div className='rounded-md border'>
								<Table>
									<TableHeader>
										<TableRow>
											<TableHead>Exam ID</TableHead>
											<TableHead>
												<div className='flex items-center gap-1'>
													<Calendar className='h-3.5 w-3.5' />
													Start
												</div>
											</TableHead>
											<TableHead>
												<div className='flex items-center gap-1'>
													<Calendar className='h-3.5 w-3.5' />
													Due
												</div>
											</TableHead>
											<TableHead>Weight</TableHead>
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
													{formatDateTime(assignment.startDate)}
												</TableCell>
												<TableCell className='text-muted-foreground'>
													{formatDateTime(assignment.dueDate)}
												</TableCell>
												<TableCell>
													<div className='flex items-center gap-1'>
														{assignment.weight}%
														{assignment.isMandatory && (
															<Badge variant='outline' className='ml-1 text-xs'>
																Required
															</Badge>
														)}
													</div>
												</TableCell>
												<TableCell>
													{assignment.isOverdue ? (
														<div className='flex items-center gap-1 text-orange-600'>
															<AlertTriangle className='h-3.5 w-3.5' />
															Overdue
														</div>
													) : assignment.isActive ? (
														<div className='flex items-center gap-1 text-green-600'>
															<CheckCircle2 className='h-3.5 w-3.5' />
															Active
														</div>
													) : (
														<div className='flex items-center gap-1 text-muted-foreground'>
															<XCircle className='h-3.5 w-3.5' />
															Inactive
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
			)}

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
									id='mandatory-detail'
									checked={isMandatory}
									onChange={e => setIsMandatory(e.target.checked)}
									className='h-4 w-4 rounded border-gray-300'
								/>
								<Label htmlFor='mandatory-detail'>
									{t('pages.dashboard.classes.assign_dialog.mandatory')}
								</Label>
							</div>
							<div className='grid gap-2 flex-1'>
								<Label htmlFor='weight-detail'>
									{t('pages.dashboard.classes.assign_dialog.weight')}
								</Label>
								<Input
									id='weight-detail'
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
