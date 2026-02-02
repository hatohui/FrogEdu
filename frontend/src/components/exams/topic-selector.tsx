import React, { useMemo, useState } from 'react'
import { Check, ChevronsUpDown } from 'lucide-react'
import { Button } from '@/components/ui/button'
import {
	Command,
	CommandEmpty,
	CommandGroup,
	CommandInput,
	CommandItem,
	CommandList,
	CommandSeparator,
} from '@/components/ui/command'
import {
	Popover,
	PopoverContent,
	PopoverTrigger,
} from '@/components/ui/popover'
import { cn } from '@/utils/cn'
import { Badge } from '@/components/ui/badge'
import type { Topic } from '@/types/model/exam-service'

interface TopicSelectorProps {
	topics: Topic[]
	value: string
	onValueChange: (value: string) => void
	placeholder?: string
	disabled?: boolean
}

export const TopicSelector: React.FC<TopicSelectorProps> = ({
	topics,
	value,
	onValueChange,
	placeholder = 'Select topic',
	disabled = false,
}) => {
	const [open, setOpen] = useState(false)

	const { curriculumTopics, userTopics } = useMemo(() => {
		const curriculum = topics.filter(topic => topic.isCurriculum)
		const user = topics.filter(topic => !topic.isCurriculum)
		return {
			curriculumTopics: curriculum,
			userTopics: user,
		}
	}, [topics])

	const selectedTopic = topics.find(topic => topic.id === value)

	return (
		<Popover open={open} onOpenChange={setOpen}>
			<PopoverTrigger asChild>
				<Button
					variant='outline'
					role='combobox'
					aria-expanded={open}
					className='w-full justify-between'
					disabled={disabled}
				>
					{selectedTopic ? (
						<div className='flex items-center gap-2 overflow-hidden'>
							<span className='truncate'>{selectedTopic.title}</span>
							{selectedTopic.isCurriculum && (
								<Badge variant='secondary' className='ml-auto shrink-0'>
									Curriculum
								</Badge>
							)}
						</div>
					) : (
						placeholder
					)}
					<ChevronsUpDown className='ml-2 h-4 w-4 shrink-0 opacity-50' />
				</Button>
			</PopoverTrigger>
			<PopoverContent className='w-[500px] p-0' align='start'>
				<Command>
					<CommandInput placeholder='Search topics...' />
					<CommandList>
						<CommandEmpty>No topic found.</CommandEmpty>

						{/* Curriculum Topics */}
						{curriculumTopics.length > 0 && (
							<>
								<CommandGroup heading='In Curriculum'>
									{curriculumTopics.map(topic => (
										<CommandItem
											key={topic.id}
											value={topic.title}
											onSelect={() => {
												onValueChange(topic.id)
												setOpen(false)
											}}
										>
											<Check
												className={cn(
													'mr-2 h-4 w-4',
													value === topic.id ? 'opacity-100' : 'opacity-0'
												)}
											/>
											<div className='flex-1'>
												<div className='font-medium'>{topic.title}</div>
												{topic.description && (
													<div className='text-sm text-muted-foreground line-clamp-1'>
														{topic.description}
													</div>
												)}
											</div>
										</CommandItem>
									))}
								</CommandGroup>
								{userTopics.length > 0 && <CommandSeparator />}
							</>
						)}

						{/* User-Created Topics */}
						{userTopics.length > 0 && (
							<CommandGroup heading='Created By Users'>
								{userTopics.map(topic => (
									<CommandItem
										key={topic.id}
										value={topic.title}
										onSelect={() => {
											onValueChange(topic.id)
											setOpen(false)
										}}
									>
										<Check
											className={cn(
												'mr-2 h-4 w-4',
												value === topic.id ? 'opacity-100' : 'opacity-0'
											)}
										/>
										<div className='flex-1'>
											<div className='font-medium'>{topic.title}</div>
											{topic.description && (
												<div className='text-sm text-muted-foreground line-clamp-1'>
													{topic.description}
												</div>
											)}
										</div>
										<Badge variant='outline' className='ml-2'>
											Custom
										</Badge>
									</CommandItem>
								))}
							</CommandGroup>
						)}
					</CommandList>
				</Command>
			</PopoverContent>
		</Popover>
	)
}
