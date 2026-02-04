import React from 'react'
import { Brain, Lightbulb, Wrench, Search } from 'lucide-react'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import {
	CognitiveLevel,
	COGNITIVE_LEVEL_LABELS,
} from '@/types/model/exam-service'

interface CognitiveLevelSelectorProps {
	value: CognitiveLevel
	onValueChange: (value: CognitiveLevel) => void
	disabled?: boolean
}

/**
 * Get icon for cognitive level
 */
function getCognitiveLevelIcon(level: CognitiveLevel) {
	switch (level) {
		case CognitiveLevel.Remember:
			return <Brain className='h-4 w-4' />
		case CognitiveLevel.Understand:
			return <Lightbulb className='h-4 w-4' />
		case CognitiveLevel.Apply:
			return <Wrench className='h-4 w-4' />
		case CognitiveLevel.Analyze:
			return <Search className='h-4 w-4' />
		default:
			return <Brain className='h-4 w-4' />
	}
}

/**
 * Get Vietnamese label for cognitive level
 */
function getCognitiveLevelVietnamese(level: CognitiveLevel): string {
	switch (level) {
		case CognitiveLevel.Remember:
			return 'Nhận biết'
		case CognitiveLevel.Understand:
			return 'Thông hiểu'
		case CognitiveLevel.Apply:
			return 'Vận dụng'
		case CognitiveLevel.Analyze:
			return 'Vận dụng cao'
		default:
			return ''
	}
}

/**
 * Get description for cognitive level based on Bloom's Taxonomy
 */
function getCognitiveLevelDescription(level: CognitiveLevel): string {
	switch (level) {
		case CognitiveLevel.Remember:
			return 'Recall facts and basic concepts'
		case CognitiveLevel.Understand:
			return 'Explain ideas or concepts'
		case CognitiveLevel.Apply:
			return 'Use information in new situations'
		case CognitiveLevel.Analyze:
			return 'Draw connections among ideas'
		default:
			return ''
	}
}

/**
 * Cognitive level selector based on Bloom's Taxonomy (4 levels)
 */
export const CognitiveLevelSelector: React.FC<CognitiveLevelSelectorProps> = ({
	value,
	onValueChange,
	disabled = false,
}) => {
	return (
		<Select
			value={String(value)}
			onValueChange={v => onValueChange(Number(v) as CognitiveLevel)}
			disabled={disabled}
		>
			<SelectTrigger className='w-full'>
				<SelectValue>
					<div className='flex items-center gap-2'>
						{getCognitiveLevelIcon(value)}
						<span>{COGNITIVE_LEVEL_LABELS[value]}</span>
					</div>
				</SelectValue>
			</SelectTrigger>
			<SelectContent>
				{/* Remember */}
				<SelectItem value={String(CognitiveLevel.Remember)}>
					<div className='flex items-center gap-2'>
						<Brain className='h-4 w-4 text-blue-500' />
						<div>
							<div className='font-medium'>
								Remember{' '}
								<span className='text-muted-foreground'>
									({getCognitiveLevelVietnamese(CognitiveLevel.Remember)})
								</span>
							</div>
							<div className='text-xs text-muted-foreground'>
								{getCognitiveLevelDescription(CognitiveLevel.Remember)}
							</div>
						</div>
					</div>
				</SelectItem>

				{/* Understand */}
				<SelectItem value={String(CognitiveLevel.Understand)}>
					<div className='flex items-center gap-2'>
						<Lightbulb className='h-4 w-4 text-green-500' />
						<div>
							<div className='font-medium'>
								Understand{' '}
								<span className='text-muted-foreground'>
									({getCognitiveLevelVietnamese(CognitiveLevel.Understand)})
								</span>
							</div>
							<div className='text-xs text-muted-foreground'>
								{getCognitiveLevelDescription(CognitiveLevel.Understand)}
							</div>
						</div>
					</div>
				</SelectItem>

				{/* Apply */}
				<SelectItem value={String(CognitiveLevel.Apply)}>
					<div className='flex items-center gap-2'>
						<Wrench className='h-4 w-4 text-amber-500' />
						<div>
							<div className='font-medium'>
								Apply{' '}
								<span className='text-muted-foreground'>
									({getCognitiveLevelVietnamese(CognitiveLevel.Apply)})
								</span>
							</div>
							<div className='text-xs text-muted-foreground'>
								{getCognitiveLevelDescription(CognitiveLevel.Apply)}
							</div>
						</div>
					</div>
				</SelectItem>

				{/* Analyze */}
				<SelectItem value={String(CognitiveLevel.Analyze)}>
					<div className='flex items-center gap-2'>
						<Search className='h-4 w-4 text-purple-500' />
						<div>
							<div className='font-medium'>
								Analyze{' '}
								<span className='text-muted-foreground'>
									({getCognitiveLevelVietnamese(CognitiveLevel.Analyze)})
								</span>
							</div>
							<div className='text-xs text-muted-foreground'>
								{getCognitiveLevelDescription(CognitiveLevel.Analyze)}
							</div>
						</div>
					</div>
				</SelectItem>
			</SelectContent>
		</Select>
	)
}

export default CognitiveLevelSelector
