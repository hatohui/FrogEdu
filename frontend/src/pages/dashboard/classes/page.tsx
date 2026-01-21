import React, { useState } from 'react'
import { useAuthStore } from '@/stores/authStore'
import { useClasses } from '@/hooks/useClasses'
import { ClassCard, CreateClassModal, JoinClassForm } from '@/components/classes'
import { Button } from '@/components/ui/button'
import { Skeleton } from '@/components/ui/skeleton'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Plus, Users, Archive, UserPlus } from 'lucide-react'

const ClassesPage: React.FC = () => {
	const { userProfile } = useAuthStore()
	const isTeacher = userProfile?.['custom:role'] === 'Teacher'

	const [showCreateModal, setShowCreateModal] = useState(false)
	const [includeArchived, setIncludeArchived] = useState(false)

	const { data: classes, isLoading, error } = useClasses(includeArchived)

	const activeClasses = classes?.filter(c => !c.isArchived) || []
	const archivedClasses = classes?.filter(c => c.isArchived) || []

	if (isLoading) {
		return (
			<div className='p-6 space-y-6'>
				<div className='flex items-center justify-between'>
					<Skeleton className='h-8 w-48' />
					<Skeleton className='h-10 w-32' />
				</div>
				<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-3'>
					{[1, 2, 3, 4, 5, 6].map(i => (
						<Skeleton key={i} className='h-48' />
					))}
				</div>
			</div>
		)
	}

	if (error) {
		return (
			<div className='p-6'>
				<div className='text-center py-12'>
					<p className='text-destructive'>Failed to load classes</p>
					<p className='text-sm text-muted-foreground mt-2'>{error.message}</p>
				</div>
			</div>
		)
	}

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div>
					<h1 className='text-3xl font-bold tracking-tight'>My Classes</h1>
					<p className='text-muted-foreground mt-1'>
						{isTeacher
							? 'Manage your classes and students'
							: 'View your enrolled classes'}
					</p>
				</div>
				{isTeacher && (
					<Button onClick={() => setShowCreateModal(true)}>
						<Plus className='h-4 w-4 mr-2' />
						Create Class
					</Button>
				)}
			</div>

			{isTeacher ? (
				/* Teacher view with tabs for active/archived */
				<Tabs
					defaultValue='active'
					onValueChange={value => setIncludeArchived(value === 'archived')}
				>
					<TabsList>
						<TabsTrigger value='active' className='gap-2'>
							<Users className='h-4 w-4' />
							Active ({activeClasses.length})
						</TabsTrigger>
						<TabsTrigger value='archived' className='gap-2'>
							<Archive className='h-4 w-4' />
							Archived ({archivedClasses.length})
						</TabsTrigger>
					</TabsList>

					<TabsContent value='active' className='mt-6'>
						{activeClasses.length === 0 ? (
							<EmptyClassesState
								isTeacher={isTeacher}
								onCreateClick={() => setShowCreateModal(true)}
							/>
						) : (
							<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-3'>
								{activeClasses.map(classData => (
									<ClassCard
										key={classData.id}
										classData={classData}
										isTeacher={isTeacher}
									/>
								))}
							</div>
						)}
					</TabsContent>

					<TabsContent value='archived' className='mt-6'>
						{archivedClasses.length === 0 ? (
							<div className='text-center py-12 text-muted-foreground'>
								<Archive className='h-12 w-12 mx-auto mb-4 opacity-50' />
								<p>No archived classes</p>
							</div>
						) : (
							<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-3'>
								{archivedClasses.map(classData => (
									<ClassCard
										key={classData.id}
										classData={classData}
										isTeacher={isTeacher}
									/>
								))}
							</div>
						)}
					</TabsContent>
				</Tabs>
			) : (
				/* Student view */
				<div className='space-y-6'>
					{/* Join class section for students */}
					<div className='flex justify-center'>
						<JoinClassForm />
					</div>

					{/* Enrolled classes */}
					<div>
						<h2 className='text-lg font-semibold mb-4'>Enrolled Classes</h2>
						{activeClasses.length === 0 ? (
							<EmptyClassesState isTeacher={false} />
						) : (
							<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-3'>
								{activeClasses.map(classData => (
									<ClassCard
										key={classData.id}
										classData={classData}
										isTeacher={false}
									/>
								))}
							</div>
						)}
					</div>
				</div>
			)}

			{/* Create Class Modal */}
			<CreateClassModal
				open={showCreateModal}
				onOpenChange={setShowCreateModal}
			/>
		</div>
	)
}

interface EmptyClassesStateProps {
	isTeacher: boolean
	onCreateClick?: () => void
}

const EmptyClassesState: React.FC<EmptyClassesStateProps> = ({
	isTeacher,
	onCreateClick,
}) => (
	<div className='text-center py-12 border-2 border-dashed rounded-lg'>
		<div className='mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-muted'>
			{isTeacher ? (
				<Users className='h-8 w-8 text-muted-foreground' />
			) : (
				<UserPlus className='h-8 w-8 text-muted-foreground' />
			)}
		</div>
		<h3 className='text-lg font-semibold'>No classes yet</h3>
		<p className='text-muted-foreground mt-2 max-w-sm mx-auto'>
			{isTeacher
				? 'Create your first class to start organizing your students and content.'
				: 'Join a class using an invite code from your teacher.'}
		</p>
		{isTeacher && onCreateClick && (
			<Button onClick={onCreateClick} className='mt-4'>
				<Plus className='h-4 w-4 mr-2' />
				Create Your First Class
			</Button>
		)}
	</div>
)

export default ClassesPage
