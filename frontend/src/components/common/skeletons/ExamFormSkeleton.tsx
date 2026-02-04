import { Card, CardContent, CardHeader } from '@/components/ui/card'
import { Skeleton } from '@/components/ui/skeleton'

export const ExamFormSkeleton = () => {
	return (
		<div className='p-6 space-y-6 max-w-5xl mx-auto'>
			{/* Header Skeleton */}
			<div className='flex items-center justify-between'>
				<div className='flex items-center space-x-4'>
					<Skeleton className='h-10 w-10 rounded-md' />
					<Skeleton className='h-8 w-64' />
				</div>
				<div className='flex gap-2'>
					<Skeleton className='h-10 w-24' />
					<Skeleton className='h-10 w-20' />
				</div>
			</div>

			{/* Form Skeleton */}
			<Card>
				<CardHeader>
					<Skeleton className='h-6 w-48' />
				</CardHeader>
				<CardContent className='space-y-6'>
					{/* Form fields */}
					{[1, 2, 3, 4, 5].map(i => (
						<div key={i} className='space-y-2'>
							<Skeleton className='h-4 w-32' />
							<Skeleton className='h-10 w-full' />
						</div>
					))}

					{/* Large text area */}
					<div className='space-y-2'>
						<Skeleton className='h-4 w-32' />
						<Skeleton className='h-32 w-full' />
					</div>

					{/* Buttons */}
					<div className='flex gap-2'>
						<Skeleton className='h-10 w-24' />
						<Skeleton className='h-10 w-24' />
					</div>
				</CardContent>
			</Card>
		</div>
	)
}
