import { Card, CardContent, CardHeader } from '@/components/ui/card'
import { Skeleton } from '@/components/ui/skeleton'

export const ExamDetailSkeleton = () => {
	return (
		<div className='p-6 space-y-6 max-w-7xl mx-auto'>
			{/* Header Skeleton */}
			<div className='flex items-center justify-between'>
				<div className='flex items-center space-x-4'>
					<Skeleton className='h-10 w-10 rounded-md' />
					<div className='space-y-2'>
						<Skeleton className='h-8 w-80' />
						<Skeleton className='h-4 w-96' />
					</div>
				</div>
				<div className='flex gap-2'>
					<Skeleton className='h-10 w-28' />
					<Skeleton className='h-10 w-20' />
					<Skeleton className='h-10 w-24' />
				</div>
			</div>

			{/* Status and Info Skeleton */}
			<div className='grid grid-cols-1 md:grid-cols-2 gap-6'>
				<Card>
					<CardHeader>
						<Skeleton className='h-6 w-32' />
					</CardHeader>
					<CardContent className='space-y-4'>
						{[1, 2, 3, 4].map(i => (
							<div key={i} className='flex justify-between items-center'>
								<Skeleton className='h-4 w-32' />
								<Skeleton className='h-5 w-40' />
							</div>
						))}
					</CardContent>
				</Card>

				<Card>
					<CardHeader>
						<Skeleton className='h-6 w-32' />
					</CardHeader>
					<CardContent className='space-y-4'>
						{[1, 2, 3, 4].map(i => (
							<div key={i} className='flex justify-between items-center'>
								<Skeleton className='h-4 w-32' />
								<Skeleton className='h-5 w-40' />
							</div>
						))}
					</CardContent>
				</Card>
			</div>

			{/* Questions Skeleton */}
			<Card>
				<CardHeader>
					<div className='flex items-center justify-between'>
						<Skeleton className='h-6 w-32' />
						<Skeleton className='h-10 w-40' />
					</div>
				</CardHeader>
				<CardContent>
					<div className='space-y-4'>
						{[1, 2, 3].map(i => (
							<div key={i} className='p-4 border rounded-lg space-y-3'>
								<div className='flex items-center justify-between'>
									<Skeleton className='h-5 w-full max-w-2xl' />
									<Skeleton className='h-8 w-20' />
								</div>
								<div className='space-y-2 pl-4'>
									<Skeleton className='h-4 w-full max-w-xl' />
									<Skeleton className='h-4 w-full max-w-lg' />
									<Skeleton className='h-4 w-full max-w-md' />
								</div>
							</div>
						))}
					</div>
				</CardContent>
			</Card>
		</div>
	)
}
