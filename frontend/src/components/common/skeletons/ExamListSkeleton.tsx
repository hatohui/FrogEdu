import { Card, CardContent, CardHeader } from '@/components/ui/card'
import { Skeleton } from '@/components/ui/skeleton'

export const ExamListSkeleton = () => {
	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header Skeleton */}
			<div className='flex items-center justify-between'>
				<div className='space-y-2'>
					<Skeleton className='h-8 w-48' />
					<Skeleton className='h-4 w-96' />
				</div>
				<div className='flex gap-2'>
					<Skeleton className='h-10 w-32' />
					<Skeleton className='h-10 w-40' />
				</div>
			</div>

			{/* Tabs Skeleton */}
			<div className='flex gap-4'>
				<Skeleton className='h-10 w-32' />
				<Skeleton className='h-10 w-32' />
			</div>

			{/* Card with Table Skeleton */}
			<Card>
				<CardHeader>
					<Skeleton className='h-6 w-48' />
				</CardHeader>
				<CardContent>
					{/* Table Header */}
					<div className='grid grid-cols-6 gap-4 pb-4 border-b'>
						<Skeleton className='h-4 w-20' />
						<Skeleton className='h-4 w-24' />
						<Skeleton className='h-4 w-16' />
						<Skeleton className='h-4 w-20' />
						<Skeleton className='h-4 w-20' />
						<Skeleton className='h-4 w-20' />
					</div>

					{/* Table Rows */}
					{[1, 2, 3, 4, 5].map(i => (
						<div key={i} className='grid grid-cols-6 gap-4 py-4 border-b'>
							<Skeleton className='h-5 w-full' />
							<Skeleton className='h-5 w-full' />
							<Skeleton className='h-5 w-16' />
							<Skeleton className='h-6 w-20' />
							<Skeleton className='h-5 w-24' />
							<Skeleton className='h-8 w-24' />
						</div>
					))}
				</CardContent>
			</Card>
		</div>
	)
}
