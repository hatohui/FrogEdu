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
import { useTranslation } from 'react-i18next'

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
 * Cognitive level selector based on Bloom's Taxonomy (4 levels)
 */
export const CognitiveLevelSelector: React.FC<CognitiveLevelSelectorProps> = ({
	value,
	onValueChange,
	disabled = false,
}) => {
	const { t } = useTranslation()
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
						<span>
							{t(`exams.cognitive_levels.${COGNITIVE_LEVEL_LABELS[value]}`)}
						</span>
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
								{t('exams.cognitive_levels.remember')}{' '}
								<span className='text-muted-foreground'>
									({t('exams.cognitive_levels_vi.remember')})
								</span>
							</div>
							<div className='text-xs text-muted-foreground'>
								{t('components.exams.cognitive_level.descriptions.remember')}
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
								{t('exams.cognitive_levels.understand')}{' '}
								<span className='text-muted-foreground'>
									({t('exams.cognitive_levels_vi.understand')})
								</span>
							</div>
							<div className='text-xs text-muted-foreground'>
								{t('components.exams.cognitive_level.descriptions.understand')}
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
								{t('exams.cognitive_levels.apply')}{' '}
								<span className='text-muted-foreground'>
									({t('exams.cognitive_levels_vi.apply')})
								</span>
							</div>
							<div className='text-xs text-muted-foreground'>
								{t('components.exams.cognitive_level.descriptions.apply')}
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
								{t('exams.cognitive_levels.analyze')}{' '}
								<span className='text-muted-foreground'>
									({t('exams.cognitive_levels_vi.analyze')})
								</span>
							</div>
							<div className='text-xs text-muted-foreground'>
								{t('components.exams.cognitive_level.descriptions.analyze')}
							</div>
						</div>
					</div>
				</SelectItem>
			</SelectContent>
		</Select>
	)
}

export default CognitiveLevelSelector
