import { Card, CardContent, CardHeader } from '@/components/ui/card'
import { Skeleton } from '@/components/ui/skeleton'

export const MatrixDetailSkeleton = () => {
	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header Skeleton */}
			<div className='flex items-center justify-between'>
				<div className='flex items-center space-x-4'>
					<Skeleton className='h-10 w-10 rounded-md' />
					<div className='space-y-2'>
						<Skeleton className='h-8 w-64' />
						<Skeleton className='h-4 w-96' />
					</div>
				</div>
				<div className='flex gap-2'>
					<Skeleton className='h-10 w-24' />
					<Skeleton className='h-10 w-20' />
					<Skeleton className='h-10 w-24' />
				</div>
			</div>

			{/* Matrix Info Skeleton */}
			<Card>
				<CardHeader>
					<Skeleton className='h-6 w-48' />
				</CardHeader>
				<CardContent className='space-y-4'>
					<div className='space-y-2'>
						<Skeleton className='h-4 w-24' />
						<Skeleton className='h-5 w-full' />
					</div>
					<div className='grid grid-cols-3 gap-4'>
						<div className='space-y-2'>
							<Skeleton className='h-4 w-20' />
							<Skeleton className='h-5 w-32' />
						</div>
						<div className='space-y-2'>
							<Skeleton className='h-4 w-20' />
							<Skeleton className='h-5 w-24' />
						</div>
						<div className='space-y-2'>
							<Skeleton className='h-4 w-20' />
							<Skeleton className='h-5 w-32' />
						</div>
					</div>
					<Skeleton className='h-4 w-full' />
				</CardContent>
			</Card>

			{/* Topic Distribution Skeleton */}
			<Card>
				<CardHeader>
					<div className='flex items-center justify-between'>
						<Skeleton className='h-6 w-48' />
						<Skeleton className='h-6 w-32' />
					</div>
				</CardHeader>
				<CardContent>
					<div className='space-y-4'>
						{[1, 2, 3].map(i => (
							<div key={i} className='p-4 border rounded-lg space-y-3'>
								<div className='flex items-center justify-between'>
									<Skeleton className='h-5 w-48' />
									<Skeleton className='h-6 w-24' />
								</div>
								<div className='flex flex-wrap gap-2'>
									<Skeleton className='h-6 w-20' />
									<Skeleton className='h-6 w-24' />
									<Skeleton className='h-6 w-20' />
								</div>
							</div>
						))}
					</div>
				</CardContent>
			</Card>

			{/* Summary Skeleton */}
			<Card>
				<CardHeader>
					<Skeleton className='h-6 w-32' />
				</CardHeader>
				<CardContent>
					<div className='grid grid-cols-4 gap-4 text-center'>
						{[1, 2, 3, 4].map(i => (
							<div key={i} className='p-4 bg-muted/50 rounded-lg space-y-2'>
								<Skeleton className='h-8 w-12 mx-auto' />
								<Skeleton className='h-4 w-20 mx-auto' />
							</div>
						))}
					</div>
				</CardContent>
			</Card>
		</div>
	)
}
