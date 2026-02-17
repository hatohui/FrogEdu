import React, { useState } from 'react'
import {
	Users,
	Search,
	Filter,
	MoreVertical,
	UserPlus,
	Mail,
	Shield,
	CheckCircle2,
	XCircle,
	Edit,
	Trash2,
	Loader2,
} from 'lucide-react'
import { useTranslation } from 'react-i18next'
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
	useUsers,
	useUserStatistics,
	useUpdateUser,
	useDeleteUser,
	useRoles,
} from '@/hooks/useUsers'
import type { GetMeResponse, UpdateUserRequest } from '@/types/dtos/users'
import UserAvatar from '@/components/common/UserAvatar'

const UsersPage = (): React.ReactElement => {
	const { t } = useTranslation()

	// Fetch roles from backend
	const { data: roles } = useRoles()

	// Helper function to get role name by ID
	const getRoleName = (roleId: string): string => {
		if (!roles) return 'Unknown'
		const role = roles.find(r => r.id === roleId)
		return role?.name || 'Unknown'
	}

	// Helper function to get role ID by name
	const getRoleId = (roleName: string): string => {
		if (!roles) return ''
		const role = roles.find(r => r.name === roleName)
		return role?.id || ''
	}

	// Pagination state
	const [page, setPage] = useState(1)
	const [pageSize] = useState(10)
	const [searchQuery, setSearchQuery] = useState('')
	const [roleFilter, setRoleFilter] = useState<string>('all')

	// Fetch users with pagination
	const { data: usersData, isLoading: isLoadingUsers } = useUsers({
		page,
		pageSize,
		search: searchQuery || undefined,
		role: roleFilter === 'all' ? undefined : getRoleId(roleFilter),
		sortBy: 'createdAt',
		sortOrder: 'desc',
	})

	// Fetch user statistics
	const { data: statistics, isLoading: isLoadingStats } = useUserStatistics()

	// Mutations
	const updateUserMutation = useUpdateUser()
	const deleteUserMutation = useDeleteUser()

	// Dialog state
	const [selectedUser, setSelectedUser] = useState<GetMeResponse | null>(null)
	const [isEditDialogOpen, setIsEditDialogOpen] = useState(false)
	const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false)

	// Edit form state
	const [editFirstName, setEditFirstName] = useState('')
	const [editLastName, setEditLastName] = useState('')
	const [editRole, setEditRole] = useState('')

	const users = usersData?.items || []
	const totalUsers = usersData?.total || 0
	const totalPages = usersData?.totalPages || 1

	// Stats cards data
	const stats = [
		{
			title: t('user_management.total_users'),
			value: statistics?.totalUsers?.toString() || '0',
			change: `+${statistics?.usersCreatedLast30Days || 0}`,
			icon: Users,
			color: 'text-blue-600',
			bgColor: 'bg-blue-100 dark:bg-blue-950',
		},
		{
			title: t('badges.verified'),
			value: statistics?.verifiedUsers?.toString() || '0',
			change: `${statistics ? Math.round((statistics.verifiedUsers / statistics.totalUsers) * 100) : 0}%`,
			icon: CheckCircle2,
			color: 'text-green-600',
			bgColor: 'bg-green-100 dark:bg-green-950',
		},
		{
			title: t('user_management.teachers'),
			value: statistics?.totalTeachers?.toString() || '0',
			change: `${statistics ? Math.round((statistics.totalTeachers / statistics.totalUsers) * 100) : 0}%`,
			icon: Shield,
			color: 'text-purple-600',
			bgColor: 'bg-purple-100 dark:bg-purple-950',
		},
		{
			title: t('user_management.students'),
			value: statistics?.totalStudents?.toString() || '0',
			change: `${statistics ? Math.round((statistics.totalStudents / statistics.totalUsers) * 100) : 0}%`,
			icon: Users,
			color: 'text-orange-600',
			bgColor: 'bg-orange-100 dark:bg-orange-950',
		},
	]

	const getRoleBadgeVariant = (roleId: string) => {
		const roleName = getRoleName(roleId)
		switch (roleName.toLowerCase()) {
			case 'admin':
				return 'destructive'
			case 'teacher':
				return 'default'
			case 'student':
				return 'secondary'
			default:
				return 'outline'
		}
	}

	const formatDate = (dateString: string) => {
		return new Date(dateString).toLocaleDateString('en-US', {
			year: 'numeric',
			month: 'short',
			day: 'numeric',
		})
	}

	const handleEditUser = (user: GetMeResponse) => {
		setSelectedUser(user)
		setEditFirstName(user.firstName)
		setEditLastName(user.lastName)
		setEditRole(getRoleName(user.roleId))
		setIsEditDialogOpen(true)
	}

	const handleDeleteUser = (user: GetMeResponse) => {
		setSelectedUser(user)
		setIsDeleteDialogOpen(true)
	}

	const handleSaveUser = async () => {
		if (!selectedUser) return

		const updates: UpdateUserRequest = {
			firstName:
				editFirstName !== selectedUser.firstName ? editFirstName : undefined,
			lastName:
				editLastName !== selectedUser.lastName ? editLastName : undefined,
			roleId:
				getRoleId(editRole) !== selectedUser.roleId
					? getRoleId(editRole)
					: undefined,
		}

		// Remove undefined fields
		const cleanedUpdates = Object.fromEntries(
			Object.entries(updates).filter(([, v]) => v !== undefined)
		) as UpdateUserRequest

		if (Object.keys(cleanedUpdates).length === 0) {
			setIsEditDialogOpen(false)
			return
		}

		await updateUserMutation.mutateAsync({
			userId: selectedUser.id,
			updates: cleanedUpdates,
		})

		setIsEditDialogOpen(false)
	}

	const handleConfirmDelete = async () => {
		if (!selectedUser) return

		await deleteUserMutation.mutateAsync(selectedUser.id)
		setIsDeleteDialogOpen(false)
	}

	const handleSendPasswordReset = (user: GetMeResponse) => {
		console.log('Send password reset to:', user.email)
	}

	const handlePreviousPage = () => {
		setPage(p => Math.max(1, p - 1))
	}

	const handleNextPage = () => {
		setPage(p => Math.min(totalPages, p + 1))
	}

	return (
		<div className='space-y-6 p-6'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div>
					<h1 className='text-3xl font-bold tracking-tight'>
						{t('user_management.title')}
					</h1>
					<p className='text-muted-foreground'>
						{t('user_management.description')}
					</p>
				</div>
				<Button>
					<UserPlus className='mr-2 h-4 w-4' />
					{t('action.add_user')}
				</Button>
			</div>

			{/* Stats Cards */}
			<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-4'>
				{isLoadingStats
					? Array.from({ length: 4 }).map((_, index) => (
							<Card key={index}>
								<CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
									<Skeleton className='h-4 w-24' />
									<Skeleton className='h-8 w-8 rounded-full' />
								</CardHeader>
								<CardContent>
									<Skeleton className='h-8 w-16 mb-2' />
									<Skeleton className='h-3 w-32' />
								</CardContent>
							</Card>
						))
					: stats.map((stat, index) => {
							const Icon = stat.icon
							return (
								<Card key={index}>
									<CardHeader className='flex flex-row items-center justify-between space-y-0 pb-2'>
										<CardTitle className='text-sm font-medium'>
											{stat.title}
										</CardTitle>
										<div className={`rounded-full p-2 ${stat.bgColor}`}>
											<Icon className={`h-4 w-4 ${stat.color}`} />
										</div>
									</CardHeader>
									<CardContent>
										<div className='text-2xl font-bold'>{stat.value}</div>
										<p className='text-xs text-muted-foreground mt-1'>
											<span className='text-green-600'>{stat.change}</span>{' '}
											{t('label.from_last_month', 'from last month')}
										</p>
									</CardContent>
								</Card>
							)
						})}
			</div>

			{/* Filters */}
			<Card>
				<CardHeader>
					<div className='flex flex-col gap-4 md:flex-row md:items-center md:justify-between'>
						<div className='flex flex-1 items-center gap-2'>
							<div className='relative flex-1 max-w-sm'>
								<Search className='absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground' />
								<Input
									placeholder={t('user_management.search_placeholder')}
									value={searchQuery}
									onChange={e => setSearchQuery(e.target.value)}
									className='pl-8'
								/>
							</div>
							<Select value={roleFilter} onValueChange={setRoleFilter}>
								<SelectTrigger className='w-[180px]'>
									<Filter className='mr-2 h-4 w-4' />
									<SelectValue
										placeholder={t('user_management.filter_by_role')}
									/>
								</SelectTrigger>
								<SelectContent>
									<SelectItem value='all'>
										{t('user_management.all_roles')}
									</SelectItem>
									{roles?.map(role => (
										<SelectItem key={role.id} value={role.name}>
											{t(`roles.${role.name.toLowerCase()}`)}
										</SelectItem>
									))}
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
									<TableHead>{t('labels.user')}</TableHead>
									<TableHead>{t('labels.email')}</TableHead>
									<TableHead>{t('labels.role')}</TableHead>
									<TableHead>{t('labels.status')}</TableHead>
									<TableHead>{t('labels.created_at')}</TableHead>
									<TableHead>{t('labels.last_login')}</TableHead>
									<TableHead className='text-right'>
										{t('labels.actions')}
									</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{isLoadingUsers ? (
									// Loading skeleton
									Array.from({ length: pageSize }).map((_, index) => (
										<TableRow key={index}>
											<TableCell>
												<div className='flex items-center gap-3'>
													<Skeleton className='h-10 w-10 rounded-full' />
													<Skeleton className='h-4 w-32' />
												</div>
											</TableCell>
											<TableCell>
												<Skeleton className='h-4 w-48' />
											</TableCell>
											<TableCell>
												<Skeleton className='h-5 w-16' />
											</TableCell>
											<TableCell>
												<Skeleton className='h-4 w-20' />
											</TableCell>
											<TableCell>
												<Skeleton className='h-4 w-24' />
											</TableCell>
											<TableCell>
												<Skeleton className='h-4 w-24' />
											</TableCell>
											<TableCell>
												<Skeleton className='h-8 w-8 ml-auto' />
											</TableCell>
										</TableRow>
									))
								) : users.length === 0 ? (
									<TableRow>
										<TableCell colSpan={7} className='text-center py-8'>
											<p className='text-muted-foreground'>
												{t('user_management.no_users_found')}
											</p>
										</TableCell>
									</TableRow>
								) : (
									users.map(user => (
										<TableRow key={user.id}>
											<TableCell>
												<div className='flex items-center gap-3'>
													<UserAvatar user={user} />
													<div>
														<div className='font-medium'>
															{user.firstName} {user.lastName}
														</div>
													</div>
												</div>
											</TableCell>
											<TableCell className='text-muted-foreground'>
												{user.email}
											</TableCell>
											<TableCell>
												<Badge variant={getRoleBadgeVariant(user.roleId)}>
													{t(`role.${getRoleName(user.roleId).toLowerCase()}`)}
												</Badge>
											</TableCell>
											<TableCell>
												{user.isEmailVerified ? (
													<div className='flex items-center gap-1 text-green-600'>
														<CheckCircle2 className='h-4 w-4' />
														<span className='text-sm'>
															{t('badges.verified')}
														</span>
													</div>
												) : (
													<div className='flex items-center gap-1 text-orange-600'>
														<XCircle className='h-4 w-4' />
														<span className='text-sm'>
															{t('badges.unverified')}
														</span>
													</div>
												)}
											</TableCell>
											<TableCell className='text-muted-foreground'>
												{formatDate(user.createdAt)}
											</TableCell>
											<TableCell className='text-muted-foreground'>
												{user.updatedAt ? formatDate(user.updatedAt) : '-'}
											</TableCell>
											<TableCell className='text-right'>
												<DropdownMenu>
													<DropdownMenuTrigger asChild>
														<Button variant='ghost' size='icon'>
															<MoreVertical className='h-4 w-4' />
														</Button>
													</DropdownMenuTrigger>
													<DropdownMenuContent align='end'>
														<DropdownMenuItem
															onClick={() => handleEditUser(user)}
														>
															<Edit className='mr-2 h-4 w-4' />
															{t('user_management.edit_user')}
														</DropdownMenuItem>
														<DropdownMenuItem
															onClick={() => handleSendPasswordReset(user)}
														>
															<Mail className='mr-2 h-4 w-4' />
															{t(
																'action.send_password_reset',
																'Send Password Reset'
															)}
														</DropdownMenuItem>
														<DropdownMenuSeparator />
														<DropdownMenuItem
															onClick={() => handleDeleteUser(user)}
															className='text-destructive'
														>
															<Trash2 className='mr-2 h-4 w-4' />
															{t('user_management.delete_user')}
														</DropdownMenuItem>
													</DropdownMenuContent>
												</DropdownMenu>
											</TableCell>
										</TableRow>
									))
								)}
							</TableBody>
						</Table>
					</div>

					{/* Pagination */}
					{!isLoadingUsers && users.length > 0 && (
						<div className='flex items-center justify-between px-2 py-4'>
							<div className='text-sm text-muted-foreground'>
								{t('user_management.showing_results', {
									from: (page - 1) * pageSize + 1,
									to: Math.min(page * pageSize, totalUsers),
									total: totalUsers,
								})}
							</div>
							<div className='flex items-center gap-2'>
								<Button
									variant='outline'
									size='sm'
									onClick={handlePreviousPage}
									disabled={page === 1}
								>
									{t('user_management.previous_page')}
								</Button>
								<div className='text-sm font-medium'>
									{page} / {totalPages}
								</div>
								<Button
									variant='outline'
									size='sm'
									onClick={handleNextPage}
									disabled={page === totalPages}
								>
									{t('user_management.next_page')}
								</Button>
							</div>
						</div>
					)}
				</CardContent>
			</Card>

			{/* Edit User Dialog */}
			<Dialog open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen}>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>{t('user_management.edit_user')}</DialogTitle>
						<DialogDescription>
							{t(
								'user_management.edit_description',
								'Update user information and role assignments.'
							)}
						</DialogDescription>
					</DialogHeader>
					{selectedUser && (
						<div className='grid gap-4 py-4'>
							<div className='grid gap-2'>
								<Label htmlFor='firstName'>{t('labels.first_name')}</Label>
								<Input
									id='firstName'
									value={editFirstName}
									onChange={e => setEditFirstName(e.target.value)}
								/>
							</div>
							<div className='grid gap-2'>
								<Label htmlFor='lastName'>{t('labels.last_name')}</Label>
								<Input
									id='lastName'
									value={editLastName}
									onChange={e => setEditLastName(e.target.value)}
								/>
							</div>
							<div className='grid gap-2'>
								<Label htmlFor='email'>{t('labels.email')}</Label>
								<Input
									id='email'
									type='email'
									value={selectedUser.email}
									disabled
								/>
							</div>
							<div className='grid gap-2'>
								<Label htmlFor='role'>{t('labels.role')}</Label>
								<Select value={editRole} onValueChange={setEditRole}>
									<SelectTrigger>
										<SelectValue />
									</SelectTrigger>
									<SelectContent>
										{roles?.map(role => (
											<SelectItem key={role.id} value={role.name}>
												{t(`roles.${role.name.toLowerCase()}`)}
											</SelectItem>
										))}
									</SelectContent>
								</Select>
							</div>
						</div>
					)}
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => setIsEditDialogOpen(false)}
							disabled={updateUserMutation.isPending}
						>
							{t('action.cancel')}
						</Button>
						<Button
							onClick={handleSaveUser}
							disabled={updateUserMutation.isPending}
						>
							{updateUserMutation.isPending && (
								<Loader2 className='mr-2 h-4 w-4 animate-spin' />
							)}
							{t('action.save')}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Delete User Dialog */}
			<Dialog open={isDeleteDialogOpen} onOpenChange={setIsDeleteDialogOpen}>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>
							{t('user_management.delete_confirm_title')}
						</DialogTitle>
						<DialogDescription>
							{t('user_management.delete_confirm_message')}
						</DialogDescription>
					</DialogHeader>
					{selectedUser && (
						<div className='py-4'>
							<p className='text-sm'>
								<strong>{t('labels.user')}:</strong> {selectedUser.firstName}{' '}
								{selectedUser.lastName}
							</p>
							<p className='text-sm text-muted-foreground'>
								<strong>{t('labels.email')}:</strong> {selectedUser.email}
							</p>
						</div>
					)}
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => setIsDeleteDialogOpen(false)}
							disabled={deleteUserMutation.isPending}
						>
							{t('action.cancel')}
						</Button>
						<Button
							variant='destructive'
							onClick={handleConfirmDelete}
							disabled={deleteUserMutation.isPending}
						>
							{deleteUserMutation.isPending && (
								<Loader2 className='mr-2 h-4 w-4 animate-spin' />
							)}
							{t('action.delete')}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>
		</div>
	)
}

export default UsersPage
