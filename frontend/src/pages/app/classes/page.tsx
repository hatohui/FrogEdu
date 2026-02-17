import React, { useState } from 'react'
import { useMe } from '@/hooks/auth/useMe'
import { useClasses } from '@/hooks/useClasses'
import { ClassCard, CreateClassModal } from '@/components/classes'
import { Button } from '@/components/ui/button'
import { Skeleton } from '@/components/ui/skeleton'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Plus, Users, Archive, UserPlus } from 'lucide-react'
import JoinClassModal from '@/components/classes/JoinClassModal'
import { useTranslation } from 'react-i18next'

const ClassesPage: React.FC = () => {
	const { t } = useTranslation()
	const { user } = useMe()
	const isTeacher = user?.role?.name === 'Teacher'
	const isAdmin = user?.role?.name === 'Admin'

	const [showCreateModal, setShowCreateModal] = useState(false)
	const [showJoinModal, setShowJoinModal] = useState(false)

	const { data: classes, isLoading } = useClasses()

	const activeClasses = classes?.filter(c => c.isActive) || []
	const inactiveClasses = classes?.filter(c => !c.isActive) || []

	if (isLoading) {
		return (
			<div className='p-6 space-y-6 max-w-7xl mx-auto'>
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

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div>
					<h1 className='text-3xl font-bold tracking-tight'>
						{t('pages.classes.title')}
					</h1>
					<p className='text-muted-foreground mt-1'>
						{isTeacher
							? t('pages.classes.subtitle_teacher')
							: isAdmin
								? t('pages.classes.subtitle_teacher')
								: t('pages.classes.subtitle_student')}
					</p>
				</div>
				{(isTeacher || isAdmin) && (
					<div className='flex gap-2'>
						<Button
							variant='outline'
							onClick={() => setShowJoinModal(prev => !prev)}
						>
							<UserPlus className='h-4 w-4 mr-2' />
							{t('pages.classes.actions.join')}
						</Button>
						<Button onClick={() => setShowCreateModal(true)}>
							<Plus className='h-4 w-4 mr-2' />
							{t('pages.classes.actions.create')}
						</Button>
					</div>
				)}
				{!isTeacher && !isAdmin && (
					<Button onClick={() => setShowJoinModal(prev => !prev)}>
						<UserPlus className='h-4 w-4 mr-2' />
						{t('pages.classes.actions.join')}
					</Button>
				)}
			</div>

			{isTeacher || isAdmin ? (
				<Tabs defaultValue='active'>
					<TabsList>
						<TabsTrigger value='active' className='gap-2'>
							<Users className='h-4 w-4' />
							{t('pages.classes.tabs.active', {
								count: activeClasses.length,
							})}
						</TabsTrigger>
						<TabsTrigger value='inactive' className='gap-2'>
							<Archive className='h-4 w-4' />
							{t('pages.classes.tabs.archived', {
								count: inactiveClasses.length,
							})}
						</TabsTrigger>
					</TabsList>

					<TabsContent value='active' className='mt-6'>
						{activeClasses.length === 0 ? (
							<EmptyClassesState
								isTeacher={isTeacher || isAdmin}
								onCreateClick={() => setShowCreateModal(true)}
							/>
						) : (
							<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-3'>
								{activeClasses.map(classData => (
									<ClassCard
										key={classData.id}
										classData={classData}
										isTeacher={isTeacher || isAdmin}
									/>
								))}
							</div>
						)}
					</TabsContent>

					<TabsContent value='inactive' className='mt-6'>
						{inactiveClasses.length === 0 ? (
							<div className='text-center py-12 text-muted-foreground'>
								<Archive className='h-12 w-12 mx-auto mb-4 opacity-50' />
								<p>{t('pages.classes.archived_empty')}</p>
							</div>
						) : (
							<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-3'>
								{inactiveClasses.map(classData => (
									<ClassCard
										key={classData.id}
										classData={classData}
										isTeacher={isTeacher || isAdmin}
									/>
								))}
							</div>
						)}
					</TabsContent>
				</Tabs>
			) : (
				/* Student view */
				<div className='space-y-6'>
					{/* Enrolled classes */}
					<div>
						<h2 className='text-lg font-semibold mb-4'>
							{t('pages.classes.sections.enrolled')}
						</h2>
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
			{/* Join Class Modal */}
			<JoinClassModal open={showJoinModal} onOpenChange={setShowJoinModal} />
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
}) => {
	const { t } = useTranslation()

	return (
		<div className='text-center py-12 border-2 border-dashed rounded-lg'>
			<div className='mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-muted'>
				{isTeacher ? (
					<Users className='h-8 w-8 text-muted-foreground' />
				) : (
					<UserPlus className='h-8 w-8 text-muted-foreground' />
				)}
			</div>
			<h3 className='text-lg font-semibold'>
				{t('pages.classes.empty.title')}
			</h3>
			<p className='text-muted-foreground mt-2 max-w-sm mx-auto'>
				{isTeacher
					? t('pages.classes.empty.teacher_description')
					: t('pages.classes.empty.student_description')}
			</p>
			{isTeacher && onCreateClick && (
				<Button onClick={onCreateClick} className='mt-4'>
					<Plus className='h-4 w-4 mr-2' />
					{t('pages.classes.empty.create_first')}
				</Button>
			)}
		</div>
	)
}

export default ClassesPage
