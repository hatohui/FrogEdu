import React, { useMemo, useState } from 'react'
import { Check, ChevronsUpDown, BookOpen } from 'lucide-react'
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
import { Badge } from '@/components/ui/badge'
import { Skeleton } from '@/components/ui/skeleton'
import { cn } from '@/utils/cn'
import { useExams } from '@/hooks/useExams'
import { useSubjects } from '@/hooks/useExams'
import { useMe } from '@/hooks/auth/useMe'
import type { Exam } from '@/types/model/exam-service'

interface ExamSelectorProps {
	value: string
	onValueChange: (examId: string) => void
	disabled?: boolean
}

export const ExamSelector: React.FC<ExamSelectorProps> = ({
	value,
	onValueChange,
	disabled = false,
}) => {
	const [open, setOpen] = useState(false)

	const { user } = useMe()
	const isAdmin = user?.role?.name === 'Admin'

	// Admins see all active exams (no creator filter); teachers see only their own
	const { data: exams = [], isLoading: loadingExams } = useExams(
		isAdmin ? false : undefined
	)
	const { data: subjects = [], isLoading: loadingSubjects } = useSubjects()

	const subjectMap = useMemo(
		() => new Map(subjects.map(s => [s.id, s])),
		[subjects]
	)

	// Group exams by subject
	const bySubject = useMemo(() => {
		const map = new Map<string, Exam[]>()
		for (const exam of exams) {
			const group = exam.subjectId || '__none__'
			if (!map.has(group)) map.set(group, [])
			map.get(group)!.push(exam)
		}
		return map
	}, [exams])

	const selectedExam = exams.find(e => e.id === value)

	const isLoading = loadingExams || loadingSubjects

	if (isLoading) return <Skeleton className='h-9 w-full' />

	return (
		<Popover open={open} onOpenChange={setOpen}>
			<PopoverTrigger asChild>
				<Button
					variant='outline'
					role='combobox'
					aria-expanded={open}
					className='w-full justify-between font-normal'
					disabled={disabled}
				>
					{selectedExam ? (
						<div className='flex items-center gap-2 overflow-hidden'>
							<BookOpen className='h-4 w-4 shrink-0 text-muted-foreground' />
							<span className='truncate'>{selectedExam.name}</span>
							{selectedExam.grade && (
								<Badge variant='secondary' className='ml-auto shrink-0'>
									Grade {selectedExam.grade}
								</Badge>
							)}
						</div>
					) : (
						<span className='text-muted-foreground'>Select an exam…</span>
					)}
					<ChevronsUpDown className='ml-2 h-4 w-4 shrink-0 opacity-50' />
				</Button>
			</PopoverTrigger>
			<PopoverContent className='w-[500px] p-0' align='start'>
				<Command
					filter={(itemValue, search) => {
						// itemValue is set to the serialized JSON string, search on name
						try {
							const exam: Exam = JSON.parse(itemValue)
							const subjectName = subjectMap.get(exam.subjectId)?.name ?? ''
							const haystack =
								`${exam.name} ${exam.description} ${subjectName}`.toLowerCase()
							return haystack.includes(search.toLowerCase()) ? 1 : 0
						} catch {
							return 0
						}
					}}
				>
					<CommandInput placeholder='Search by name or subject…' />
					<CommandList>
						<CommandEmpty>No exams found.</CommandEmpty>

						{Array.from(bySubject.entries()).map(
							([subjectId, groupExams], idx) => {
								const subject = subjectMap.get(subjectId)
								const heading = subject
									? `${subject.name} · Grade ${subject.grade}`
									: 'Other'

								return (
									<React.Fragment key={subjectId}>
										{idx > 0 && <CommandSeparator />}
										<CommandGroup heading={heading}>
											{groupExams.map(exam => (
												<CommandItem
													key={exam.id}
													value={JSON.stringify(exam)}
													onSelect={() => {
														onValueChange(exam.id)
														setOpen(false)
													}}
												>
													<Check
														className={cn(
															'mr-2 h-4 w-4 shrink-0',
															value === exam.id ? 'opacity-100' : 'opacity-0'
														)}
													/>
													<div className='flex-1 overflow-hidden'>
														<div className='flex items-center gap-2'>
															<span className='font-medium truncate'>
																{exam.name}
															</span>
															<Badge
																variant='outline'
																className='ml-auto shrink-0 text-xs'
															>
																{exam.questionCount} Qs
															</Badge>
														</div>
														{exam.description && (
															<p className='text-xs text-muted-foreground line-clamp-1 mt-0.5'>
																{exam.description}
															</p>
														)}
													</div>
												</CommandItem>
											))}
										</CommandGroup>
									</React.Fragment>
								)
							}
						)}
					</CommandList>
				</Command>
			</PopoverContent>
		</Popover>
	)
}
