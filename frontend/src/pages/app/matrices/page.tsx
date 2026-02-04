import React from 'react'
import { useNavigate } from 'react-router'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { MatrixListSkeleton } from '@/components/common/skeletons'
import {
	Grid3x3,
	ArrowLeft,
	Eye,
	Download,
	Trash2,
	FileText,
	Plus,
	Edit,
} from 'lucide-react'
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import {
	useMatrices,
	useDeleteMatrix,
	useExportMatrixToPdf,
	useExportMatrixToExcel,
	useSubjects,
} from '@/hooks/useExams'
import { useConfirm } from '@/hooks/useConfirm'
import { ConfirmDialog } from '@/components/common/ConfirmDialog'
import type { Matrix } from '@/types/model/exam-service'

// Component to display a single matrix card
const MatrixCard = ({ matrix }: { matrix: Matrix }) => {
	const navigate = useNavigate()
	const deleteMatrixMutation = useDeleteMatrix()
	const exportMatrixToPdf = useExportMatrixToPdf()
	const exportMatrixToExcel = useExportMatrixToExcel()
	const { confirm } = useConfirm()
	const { data: subjects = [] } = useSubjects(matrix.grade)

	const subjectName = subjects.find(s => s.id === matrix.subjectId)?.name

	const handleDeleteMatrix = async () => {
		const confirmed = await confirm({
			title: 'Delete Matrix',
			description: `Are you sure you want to delete the matrix "${matrix.name}"? This action cannot be undone.`,
			confirmText: 'Delete',
			variant: 'destructive',
		})

		if (confirmed) {
			await deleteMatrixMutation.mutateAsync(matrix.id)
		}
	}

	const handleExportPdf = async () => {
		await exportMatrixToPdf.mutateAsync(matrix.id)
	}

	const handleExportExcel = async () => {
		await exportMatrixToExcel.mutateAsync(matrix.id)
	}

	return (
		<Card className='hover:shadow-md transition-shadow'>
			<CardHeader>
				<div className='flex items-start justify-between'>
					<div className='flex-1'>
						<CardTitle className='text-lg'>{matrix.name}</CardTitle>
						<CardDescription className='mt-1'>
							{subjectName || 'Unknown Subject'} • Grade {matrix.grade}
						</CardDescription>
					</div>
					<Badge variant='outline'>{matrix.totalQuestionCount} Questions</Badge>
				</div>
			</CardHeader>
			<CardContent className='space-y-3'>
				{matrix.description && (
					<p className='text-sm text-muted-foreground line-clamp-2'>
						{matrix.description}
					</p>
				)}

				<div className='flex items-center gap-2 text-sm text-muted-foreground'>
					<Grid3x3 className='h-4 w-4' />
					<span>{matrix.matrixTopics.length} Topics Configured</span>
				</div>

				<div className='flex flex-col gap-2'>
					<Button
						variant='outline'
						size='sm'
						className='w-full justify-start'
						onClick={() => navigate(`/app/matrices/${matrix.id}`)}
					>
						<Eye className='h-4 w-4 mr-2' />
						View Details
					</Button>

					<div className='flex gap-2'>
						<Button
							variant='outline'
							size='sm'
							className='flex-1'
							onClick={() => navigate(`/app/matrices/${matrix.id}/edit`)}
						>
							<Edit className='h-4 w-4 mr-2' />
							Edit
						</Button>

						<DropdownMenu>
							<DropdownMenuTrigger asChild>
								<Button variant='outline' size='sm'>
									<Download className='h-4 w-4' />
								</Button>
							</DropdownMenuTrigger>
							<DropdownMenuContent align='start'>
								<DropdownMenuItem
									onClick={handleExportPdf}
									disabled={exportMatrixToPdf.isPending}
								>
									<FileText className='h-4 w-4 mr-2' />
									{exportMatrixToPdf.isPending
										? 'Exporting PDF...'
										: 'Export as PDF'}
								</DropdownMenuItem>
								<DropdownMenuItem
									onClick={handleExportExcel}
									disabled={exportMatrixToExcel.isPending}
								>
									<FileText className='h-4 w-4 mr-2' />
									{exportMatrixToExcel.isPending
										? 'Exporting Excel...'
										: 'Export as Excel'}
								</DropdownMenuItem>
							</DropdownMenuContent>
						</DropdownMenu>

						<Button
							variant='outline'
							size='sm'
							onClick={handleDeleteMatrix}
							disabled={deleteMatrixMutation.isPending}
							className='text-destructive hover:text-destructive'
						>
							<Trash2 className='h-4 w-4' />
						</Button>
					</div>
				</div>
			</CardContent>
		</Card>
	)
}

const MatricesPage = (): React.ReactElement => {
	const navigate = useNavigate()
	const { data: matrices, isLoading } = useMatrices()
	const { confirmState, handleConfirm, handleCancel, handleOpenChange } =
		useConfirm()

	if (isLoading) {
		return <MatrixListSkeleton />
	}

	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header */}
			<div className='flex items-center justify-between'>
				<div className='flex items-center gap-4'>
					<Button
						variant='ghost'
						size='icon'
						onClick={() => navigate('/app/exams')}
					>
						<ArrowLeft className='h-5 w-5' />
					</Button>
					<div>
						<h1 className='text-3xl font-bold tracking-tight flex items-center gap-2'>
							<Grid3x3 className='h-8 w-8' />
							Exam Matrices
						</h1>
						<p className='text-muted-foreground'>
							Create and manage reusable exam blueprints
						</p>
					</div>
				</div>
				<Button onClick={() => navigate('/app/matrices/new')}>
					<Plus className='h-4 w-4 mr-2' />
					Create Matrix
				</Button>
			</div>

			{/* Matrices List */}
			{!matrices || matrices.length === 0 ? (
				<Card>
					<CardContent className='py-12 text-center'>
						<Grid3x3 className='h-12 w-12 mx-auto text-muted-foreground mb-4' />
						<h3 className='text-lg font-medium mb-2'>No Matrices Yet</h3>
						<p className='text-muted-foreground mb-4'>
							Create a matrix blueprint to define your exam structure
						</p>
						<Button onClick={() => navigate('/app/matrices/new')}>
							<Plus className='h-4 w-4 mr-2' />
							Create Your First Matrix
						</Button>
					</CardContent>
				</Card>
			) : (
				<div className='grid gap-4 md:grid-cols-2 lg:grid-cols-3'>
					{matrices.map(matrix => (
						<MatrixCard key={matrix.id} matrix={matrix} />
					))}
				</div>
			)}

			<ConfirmDialog
				open={confirmState.isOpen}
				onOpenChange={handleOpenChange}
				title={confirmState.title}
				description={confirmState.description}
				onConfirm={handleConfirm}
				onCancel={handleCancel}
				confirmText={confirmState.confirmText}
				cancelText={confirmState.cancelText}
				variant={confirmState.variant}
			/>
		</div>
	)
}

export default MatricesPage
