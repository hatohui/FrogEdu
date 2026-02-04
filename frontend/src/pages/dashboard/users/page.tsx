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
import { Avatar, AvatarFallback } from '@/components/ui/avatar'

// Mock data - will be replaced with actual API calls
const mockUsers = [
	{
		id: '1',
		email: 'john.doe@example.com',
		firstName: 'John',
		lastName: 'Doe',
		role: 'Admin',
		isEmailVerified: true,
		createdAt: '2026-01-15T10:30:00Z',
		lastLoginAt: '2026-02-03T14:20:00Z',
	},
	{
		id: '2',
		email: 'sarah.wilson@example.com',
		firstName: 'Sarah',
		lastName: 'Wilson',
		role: 'Teacher',
		isEmailVerified: true,
		createdAt: '2026-01-20T09:15:00Z',
		lastLoginAt: '2026-02-04T08:45:00Z',
	},
	{
		id: '3',
		email: 'mike.johnson@example.com',
		firstName: 'Mike',
		lastName: 'Johnson',
		role: 'Student',
		isEmailVerified: false,
		createdAt: '2026-02-01T11:00:00Z',
		lastLoginAt: '2026-02-03T16:30:00Z',
	},
	{
		id: '4',
		email: 'emily.brown@example.com',
		firstName: 'Emily',
		lastName: 'Brown',
		role: 'Teacher',
		isEmailVerified: true,
		createdAt: '2025-12-10T13:45:00Z',
		lastLoginAt: '2026-02-04T09:10:00Z',
	},
	{
		id: '5',
		email: 'david.lee@example.com',
		firstName: 'David',
		lastName: 'Lee',
		role: 'Student',
		isEmailVerified: true,
		createdAt: '2026-01-25T15:20:00Z',
		lastLoginAt: '2026-02-02T18:00:00Z',
	},
]

const stats = [
	{
		title: 'Total Users',
		value: '2,847',
		change: '+12.5%',
		icon: Users,
		color: 'text-blue-600',
		bgColor: 'bg-blue-100 dark:bg-blue-950',
	},
	{
		title: 'Active Today',
		value: '1,234',
		change: '+5.2%',
		icon: CheckCircle2,
		color: 'text-green-600',
		bgColor: 'bg-green-100 dark:bg-green-950',
	},
	{
		title: 'Teachers',
		value: '156',
		change: '+8.1%',
		icon: Shield,
		color: 'text-purple-600',
		bgColor: 'bg-purple-100 dark:bg-purple-950',
	},
	{
		title: 'Students',
		value: '2,691',
		change: '+13.2%',
		icon: Users,
		color: 'text-orange-600',
		bgColor: 'bg-orange-100 dark:bg-orange-950',
	},
]

const UsersPage = (): React.ReactElement => {
	const [searchQuery, setSearchQuery] = useState('')
	const [roleFilter, setRoleFilter] = useState<string>('all')
	const [selectedUser, setSelectedUser] = useState<
		(typeof mockUsers)[0] | null
	>(null)
	const [isEditDialogOpen, setIsEditDialogOpen] = useState(false)
	const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false)

	const filteredUsers = mockUsers.filter(user => {
		const matchesSearch =
			user.firstName.toLowerCase().includes(searchQuery.toLowerCase()) ||
			user.lastName.toLowerCase().includes(searchQuery.toLowerCase()) ||
			user.email.toLowerCase().includes(searchQuery.toLowerCase())

		const matchesRole = roleFilter === 'all' || user.role === roleFilter

		return matchesSearch && matchesRole
	})

	const getRoleBadgeVariant = (role: string) => {
		switch (role.toLowerCase()) {
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

	const getInitials = (firstName: string, lastName: string) => {
		return `${firstName[0]}${lastName[0]}`.toUpperCase()
	}

	const handleEditUser = (user: (typeof mockUsers)[0]) => {
		setSelectedUser(user)
		setIsEditDialogOpen(true)
	}

	const handleDeleteUser = (user: (typeof mockUsers)[0]) => {
		setSelectedUser(user)
		setIsDeleteDialogOpen(true)
	}

	const handleSendPasswordReset = (user: (typeof mockUsers)[0]) => {
		console.log('Send password reset to:', user.email)
		// TODO: Implement password reset email
	}

	return (
		<div className='space-y-6 p-6'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div>
					<h1 className='text-3xl font-bold tracking-tight'>User Management</h1>
					<p className='text-muted-foreground'>
						Manage users, roles, and permissions
					</p>
				</div>
				<Button>
					<UserPlus className='mr-2 h-4 w-4' />
					Add User
				</Button>
			</div>

			{/* Stats Cards */}
			<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-4'>
				{stats.map((stat, index) => {
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
									<span className='text-green-600'>{stat.change}</span> from
									last month
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
									placeholder='Search users...'
									value={searchQuery}
									onChange={e => setSearchQuery(e.target.value)}
									className='pl-8'
								/>
							</div>
							<Select value={roleFilter} onValueChange={setRoleFilter}>
								<SelectTrigger className='w-[180px]'>
									<Filter className='mr-2 h-4 w-4' />
									<SelectValue placeholder='Filter by role' />
								</SelectTrigger>
								<SelectContent>
									<SelectItem value='all'>All Roles</SelectItem>
									<SelectItem value='Admin'>Admin</SelectItem>
									<SelectItem value='Teacher'>Teacher</SelectItem>
									<SelectItem value='Student'>Student</SelectItem>
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
									<TableHead>User</TableHead>
									<TableHead>Email</TableHead>
									<TableHead>Role</TableHead>
									<TableHead>Status</TableHead>
									<TableHead>Joined</TableHead>
									<TableHead>Last Login</TableHead>
									<TableHead className='text-right'>Actions</TableHead>
								</TableRow>
							</TableHeader>
							<TableBody>
								{filteredUsers.length === 0 ? (
									<TableRow>
										<TableCell colSpan={7} className='text-center py-8'>
											<p className='text-muted-foreground'>No users found</p>
										</TableCell>
									</TableRow>
								) : (
									filteredUsers.map(user => (
										<TableRow key={user.id}>
											<TableCell>
												<div className='flex items-center gap-3'>
													<Avatar>
														<AvatarFallback>
															{getInitials(user.firstName, user.lastName)}
														</AvatarFallback>
													</Avatar>
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
												<Badge variant={getRoleBadgeVariant(user.role)}>
													{user.role}
												</Badge>
											</TableCell>
											<TableCell>
												{user.isEmailVerified ? (
													<div className='flex items-center gap-1 text-green-600'>
														<CheckCircle2 className='h-4 w-4' />
														<span className='text-sm'>Verified</span>
													</div>
												) : (
													<div className='flex items-center gap-1 text-orange-600'>
														<XCircle className='h-4 w-4' />
														<span className='text-sm'>Unverified</span>
													</div>
												)}
											</TableCell>
											<TableCell className='text-muted-foreground'>
												{formatDate(user.createdAt)}
											</TableCell>
											<TableCell className='text-muted-foreground'>
												{formatDate(user.lastLoginAt)}
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
															Edit User
														</DropdownMenuItem>
														<DropdownMenuItem
															onClick={() => handleSendPasswordReset(user)}
														>
															<Mail className='mr-2 h-4 w-4' />
															Send Password Reset
														</DropdownMenuItem>
														<DropdownMenuSeparator />
														<DropdownMenuItem
															onClick={() => handleDeleteUser(user)}
															className='text-destructive'
														>
															<Trash2 className='mr-2 h-4 w-4' />
															Delete User
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
				</CardContent>
			</Card>

			{/* Edit User Dialog */}
			<Dialog open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen}>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>Edit User</DialogTitle>
						<DialogDescription>
							Update user information and role assignments.
						</DialogDescription>
					</DialogHeader>
					{selectedUser && (
						<div className='grid gap-4 py-4'>
							<div className='grid gap-2'>
								<Label htmlFor='firstName'>First Name</Label>
								<Input id='firstName' defaultValue={selectedUser.firstName} />
							</div>
							<div className='grid gap-2'>
								<Label htmlFor='lastName'>Last Name</Label>
								<Input id='lastName' defaultValue={selectedUser.lastName} />
							</div>
							<div className='grid gap-2'>
								<Label htmlFor='email'>Email</Label>
								<Input
									id='email'
									type='email'
									defaultValue={selectedUser.email}
								/>
							</div>
							<div className='grid gap-2'>
								<Label htmlFor='role'>Role</Label>
								<Select defaultValue={selectedUser.role}>
									<SelectTrigger>
										<SelectValue />
									</SelectTrigger>
									<SelectContent>
										<SelectItem value='Admin'>Admin</SelectItem>
										<SelectItem value='Teacher'>Teacher</SelectItem>
										<SelectItem value='Student'>Student</SelectItem>
									</SelectContent>
								</Select>
							</div>
						</div>
					)}
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => setIsEditDialogOpen(false)}
						>
							Cancel
						</Button>
						<Button onClick={() => setIsEditDialogOpen(false)}>
							Save Changes
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			{/* Delete User Dialog */}
			<Dialog open={isDeleteDialogOpen} onOpenChange={setIsDeleteDialogOpen}>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>Delete User</DialogTitle>
						<DialogDescription>
							Are you sure you want to delete this user? This action cannot be
							undone.
						</DialogDescription>
					</DialogHeader>
					{selectedUser && (
						<div className='py-4'>
							<p className='text-sm'>
								<strong>User:</strong> {selectedUser.firstName}{' '}
								{selectedUser.lastName}
							</p>
							<p className='text-sm text-muted-foreground'>
								<strong>Email:</strong> {selectedUser.email}
							</p>
						</div>
					)}
					<DialogFooter>
						<Button
							variant='outline'
							onClick={() => setIsDeleteDialogOpen(false)}
						>
							Cancel
						</Button>
						<Button
							variant='destructive'
							onClick={() => setIsDeleteDialogOpen(false)}
						>
							Delete User
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>
		</div>
	)
}

export default UsersPage
