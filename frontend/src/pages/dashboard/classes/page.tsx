import React, { useState, useMemo } from 'react'
import { useNavigate } from 'react-router'
import { useTranslation } from 'react-i18next'
import {
	Search,
	GraduationCap,
	Users,
	FileText,
	CheckCircle2,
	MoreVertical,
	Eye,
	ClipboardPlus,
	BookOpen,
	Plus,
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
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuSeparator,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
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
	useClasses,
	useAdminAssignExam,
	useJoinClass,
} from '@/hooks/useClasses'
import { useMe } from '@/hooks/auth/useMe'
import { CreateClassModal } from '@/components/classes'
import type { ClassRoom } from '@/types/model/class-service'
import type { AssignExamRequest } from '@/types/dtos/classes'
import { ExamSelector } from '@/components/classes/ExamSelector'

const ClassesPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const { user } = useMe()
	// Use regular useClasses - backend already filters by role (Admin gets all)
	const { data: classes, isLoading } = useClasses()

	const [searchQuery, setSearchQuery] = useState('')
	const [statusFilter, setStatusFilter] = useState<string>('all')
	const [assignDialogOpen, setAssignDialogOpen] = useState(false)
	const [selectedClassId, setSelectedClassId] = useState<string | null>(null)
	const [showCreateModal, setShowCreateModal] = useState(false)

	// Assign exam form state
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

	const assignExamMutation = useAdminAssignExam()
	const joinClassMutation = useJoinClass()

	const filteredClasses = useMemo(() => {
		if (!classes) return []
		return classes.filter(cls => {
			const matchesSearch =
				cls.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
				cls.grade.toLowerCase().includes(searchQuery.toLowerCase())

			const matchesStatus =
				statusFilter === 'all' ||
				(statusFilter === 'active' && cls.isActive) ||
				(statusFilter === 'inactive' && !cls.isActive)

			return matchesSearch && matchesStatus
		})
	}, [classes, searchQuery, statusFilter])

	const stats = useMemo(() => {
		if (!classes) return { total: 0, active: 0, students: 0, assignments: 0 }
		return {
			total: classes.length,
			active: classes.filter(c => c.isActive).length,
			students: classes.reduce((sum, c) => sum + c.studentCount, 0),
			assignments: classes.reduce((sum, c) => sum + c.assignmentCount, 0),
		}
	}, [classes])

	const formatDate = (dateString: string) => {
		return new Date(dateString).toLocaleDateString('en-US', {
			year: 'numeric',
			month: 'short',
			day: 'numeric',
		})
	}

	const handleViewDetails = (cls: ClassRoom) => {
		navigate(`/dashboard/classes/${cls.id}`)
	}

	const handleOpenAssignDialog = (cls: ClassRoom) => {
		setSelectedClassId(cls.id)
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

	const handleJoinClass = (cls: ClassRoom) => {
		if (cls.inviteCode) {
			joinClassMutation.mutate(cls.inviteCode)
		}
	}

	const handleAssignExam = () => {
		if (!selectedClassId || !examId || !startDate || !dueDate) return

		const data: AssignExamRequest = {
			examId,
			startDate: new Date(startDate).toISOString(),
			dueDate: new Date(dueDate).toISOString(),
			isMandatory,
			weight: Number(weight) || 100,
			retryTimes: Number(retryTimes) || 0,
			isRetryable,
			shouldShuffleQuestions,
			shouldShuffleAnswers,
			allowPartialScoring,
		}

		assignExamMutation.mutate(
			{ classId: selectedClassId, data },
			{
				onSuccess: () => {
					setAssignDialogOpen(false)
				},
			}
		)
	}

	const statCards = [
		{
			titleKey: 'pages.dashboard.classes.stats.total_classes',
			value: stats.total,
			icon: GraduationCap,
			color: 'text-blue-600',
			bgColor: 'bg-blue-100 dark:bg-blue-950',
		},
		{
			titleKey: 'pages.dashboard.classes.stats.active_classes',
			value: stats.active,
			icon: CheckCircle2,
			color: 'text-green-600',
			bgColor: 'bg-green-100 dark:bg-green-950',
		},
		{
			titleKey: 'pages.dashboard.classes.stats.total_students',
			value: stats.students,
			icon: Users,
			color: 'text-purple-600',
			bgColor: 'bg-purple-100 dark:bg-purple-950',
		},
		{
			titleKey: 'pages.dashboard.classes.stats.total_assignments',
			value: stats.assignments,
			icon: FileText,
			color: 'text-orange-600',
			bgColor: 'bg-orange-100 dark:bg-orange-950',
		},
	]

	if (isLoading) {
		return (
			<div className='space-y-6 p-6'>
				<div>
					<Skeleton className='h-9 w-64' />
					<Skeleton className='h-5 w-96 mt-2' />
				</div>
				<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-4'>
					{Array.from({ length: 4 }).map((_, i) => (
						<Card key={i}>
							<CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
								<Skeleton className='h-4 w-24' />
								<Skeleton className='h-8 w-8 rounded-full' />
							</CardHeader>
							<CardContent>
								<Skeleton className='h-7 w-16' />
							</CardContent>
						</Card>
					))}
				</div>
				<Card>
					<CardContent className='pt-6'>
						<Skeleton className='h-64 w-full' />
					</CardContent>
				</Card>
			</div>
		)
	}

	return (
		<div className='space-y-6 p-6'>
			{/* Header */}
			<div>
				<div className='flex items-center justify-between'>
					<div>
						<h1 className='text-3xl font-bold tracking-tight'>
							{t('pages.dashboard.classes.title')}
						</h1>
						<p className='text-muted-foreground'>
							{t('pages.dashboard.classes.subtitle')}
						</p>
					</div>
					<div className='flex gap-2'>
						<Button onClick={() => setShowCreateModal(true)}>
							<Plus className='h-4 w-4 mr-2' />
							{t('pages.classes.actions.create')}
						</Button>
					</div>
				</div>
			</div>

			{/* Stats Cards */}
			<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-4'>
				{statCards.map((stat, index) => {
					const Icon = stat.icon
					return (
						<Card key={index}>
							<CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
								<CardTitle className='text-sm font-medium'>
									{t(stat.titleKey)}
								</CardTitle>
								<div className={`rounded-full p-2 ${stat.bgColor}`}>
									<Icon className={`h-4 w-4 ${stat.color}`} />
								</div>
							</CardHeader>
							<CardContent>
								<div className='text-2xl font-bold'>{stat.value}</div>
							</CardContent>
						</Card>
					)
				})}
			</div>

			{/* Filters & Table */}
			<Card>
				<CardHeader>
					<div className='flex flex-col gap-4 md:flex-row md:items-center md:justify-between'>
						<div className='flex flex-1 items-center gap-2'>
							<div className='relative flex-1 max-w-sm'>
								<Search className='absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground' />
								<Input
									placeholder={t('pages.dashboard.classes.search_placeholder')}
									value={searchQuery}
									onChange={e => setSearchQuery(e.target.value)}
									className='pl-8'
								/>
							</div>
							<Select value={statusFilter} onValueChange={setStatusFilter}>
								<SelectTrigger className='w-[150px]'>
									<SelectValue />
								</SelectTrigger>
								<SelectContent>
									<SelectItem value='all'>
										{t('pages.dashboard.classes.filter_all')}
									</SelectItem>
									<SelectItem value='active'>
										{t('pages.dashboard.classes.filter_active')}
									</SelectItem>
									<SelectItem value='inactive'>
										{t('pages.dashboard.classes.filter_inactive')}
									</SelectItem>
								</SelectContent>
							</Select>
						</div>
					</div>
				</CardHeader>
				<CardContent>
					<div className='rounded-md border'>
						<Table>
							<TableHeader>
								<TableRow>
									<TableHead>
										{t('pages.dashboard.classes.table.name')}
									</TableHead>
									<TableHead>
										{t('pages.dashboard.classes.table.grade')}
									</TableHead>
									<TableHead>
										{t('pages.dashboard.classes.table.invite_code')}
									</TableHead>
									<TableHead>
										{t('pages.dashboard.classes.table.students')}
									</TableHead>
									<TableHead>
										{t('pages.dashboard.classes.table.assignments')}
									</TableHead>
									<TableHead>
										{t('pages.dashboard.classes.table.status')}
									</TableHead>
									<TableHead>
										{t('pages.dashboard.classes.table.created')}
									</TableHead>
									<TableHead className='text-right'>
										{t('pages.dashboard.classes.table.actions')}
									</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{filteredClasses.length === 0 ? (
									<TableRow>
										<TableCell colSpan={8} className='text-center py-8'>
											<div className='flex flex-col items-center gap-2'>
												<BookOpen className='h-8 w-8 text-muted-foreground' />
												<p className='font-medium'>
													{t('pages.dashboard.classes.empty.title')}
												</p>
												<p className='text-sm text-muted-foreground'>
													{t('pages.dashboard.classes.empty.description')}
												</p>
											</div>
										</TableCell>
									</TableRow>
								) : (
									filteredClasses.map(cls => (
										<TableRow
											key={cls.id}
											className='cursor-pointer hover:bg-muted/50'
											onClick={() => navigate(`/dashboard/classes/${cls.id}`)}
										>
											<TableCell className='font-medium'>{cls.name}</TableCell>
											<TableCell>
												<Badge variant='outline'>{cls.grade}</Badge>
											</TableCell>
											<TableCell>
												<code className='text-xs bg-muted px-1.5 py-0.5 rounded'>
													{cls.inviteCode}
												</code>
											</TableCell>
											<TableCell>
												<div className='flex items-center gap-1'>
													<Users className='h-3.5 w-3.5 text-muted-foreground' />
													{cls.studentCount}
													<span className='text-muted-foreground text-xs'>
														/ {cls.maxStudents}
													</span>
												</div>
											</TableCell>
											<TableCell>{cls.assignmentCount}</TableCell>
											<TableCell>
												{cls.isActive ? (
													<Badge variant='default'>
														{t('pages.dashboard.classes.filter_active')}
													</Badge>
												) : (
													<Badge variant='secondary'>
														{t('pages.dashboard.classes.filter_inactive')}
													</Badge>
												)}
											</TableCell>
											<TableCell className='text-muted-foreground'>
												{formatDate(cls.createdAt)}
											</TableCell>
											<TableCell
												className='text-right'
												onClick={e => e.stopPropagation()}
											>
												<DropdownMenu>
													<DropdownMenuTrigger asChild>
														<Button variant='ghost' size='icon'>
															<MoreVertical className='h-4 w-4' />
														</Button>
													</DropdownMenuTrigger>
													<DropdownMenuContent align='end'>
														<DropdownMenuItem
															onClick={() => handleViewDetails(cls)}
														>
															<Eye className='mr-2 h-4 w-4' />
															{t(
																'pages.dashboard.classes.actions.view_details'
															)}
														</DropdownMenuItem>
														<DropdownMenuSeparator />
														<DropdownMenuItem
															onClick={() => handleOpenAssignDialog(cls)}
														>
															<ClipboardPlus className='mr-2 h-4 w-4' />
															{t('pages.dashboard.classes.actions.assign_exam')}
														</DropdownMenuItem>
														{user?.cognitoId !== cls.teacherId && (
															<>
																<DropdownMenuSeparator />
																<DropdownMenuItem
																	onClick={() => handleJoinClass(cls)}
																>
																	<UserPlus className='mr-2 h-4 w-4' />
																	{t('pages.classes.actions.join')}
																</DropdownMenuItem>
															</>
														)}
													</DropdownMenuContent>
												</DropdownMenu>
											</TableCell>
										</TableRow>
									))
								)}
							</TableBody>
						</Table>
					</div>
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
									id='mandatory'
									checked={isMandatory}
									onChange={e => setIsMandatory(e.target.checked)}
									className='h-4 w-4 rounded border-gray-300'
								/>
								<Label htmlFor='mandatory'>
									{t('pages.dashboard.classes.assign_dialog.mandatory')}
								</Label>
							</div>
							<div className='grid gap-2 flex-1'>
								<Label htmlFor='weight'>
									{t('pages.dashboard.classes.assign_dialog.weight')}
								</Label>
								<Input
									id='weight'
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
								id='retryable-d'
								checked={isRetryable}
								onChange={e => setIsRetryable(e.target.checked)}
								className='h-4 w-4 rounded border-gray-300'
							/>
							<Label htmlFor='retryable-d'>Allow retries</Label>
						</div>
						{isRetryable && (
							<div className='grid gap-1'>
								<Label htmlFor='retrytimes-d'>Max retries</Label>
								<Input
									id='retrytimes-d'
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
								id='shuffle-q-d'
								checked={shouldShuffleQuestions}
								onChange={e => setShouldShuffleQuestions(e.target.checked)}
								className='h-4 w-4 rounded border-gray-300'
							/>
							<Label htmlFor='shuffle-q-d'>Shuffle questions</Label>
						</div>
						<div className='flex items-center gap-2'>
							<input
								type='checkbox'
								id='shuffle-a-d'
								checked={shouldShuffleAnswers}
								onChange={e => setShouldShuffleAnswers(e.target.checked)}
								className='h-4 w-4 rounded border-gray-300'
							/>
							<Label htmlFor='shuffle-a-d'>Shuffle answers</Label>
						</div>
						<div className='flex items-center gap-2'>
							<input
								type='checkbox'
								id='partial-d'
								checked={allowPartialScoring}
								onChange={e => setAllowPartialScoring(e.target.checked)}
								className='h-4 w-4 rounded border-gray-300'
							/>
							<Label htmlFor='partial-d'>Partial scoring</Label>
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

			{/* Create Class Modal */}
			<CreateClassModal
				open={showCreateModal}
				onOpenChange={setShowCreateModal}
			/>
		</div>
	)
}

export default ClassesPage
