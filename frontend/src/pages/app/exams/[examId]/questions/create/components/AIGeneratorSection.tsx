import React, { useState } from 'react'
import {
	Sparkles,
	Loader2,
	Zap,
	SaveAll,
	ChevronDown,
	ChevronUp,
	Globe,
	Info,
	CheckCircle2,
} from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Label } from '@/components/ui/label'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { TopicSelector } from '@/components/exams/topic-selector'
import { QuestionTypeSelector } from '@/components/exams/QuestionTypeSelector'
import { CognitiveLevelSelector } from '@/components/exams/CognitiveLevelSelector'
import { AIQuestionPreview } from '@/components/exams/AIQuestionPreview'
import {
	QuestionType,
	CognitiveLevel,
	QUESTION_TYPE_CONFIGS,
} from '@/types/model/exam-service'
import type { AIGeneratedQuestion } from '@/types/model/ai-service'
import type { Topic, Exam, Matrix } from '@/types/model/exam-service'
import { useTranslation } from 'react-i18next'

// Sentinel value representing "randomize" question type
const RANDOM_TYPE = 'random' as const
type QuestionTypeOrRandom = QuestionType | typeof RANDOM_TYPE

/** Composite key for a matrix row: topicId-cognitiveLevel */
export function matrixRowKey(topicId: string, level: CognitiveLevel): string {
	return `${topicId}-${level}`
}

function getCognitiveLevelColor(level: CognitiveLevel): string {
	switch (level) {
		case CognitiveLevel.Remember:
			return 'bg-blue-500/10 text-blue-700 dark:text-blue-400 border-blue-200 dark:border-blue-800'
		case CognitiveLevel.Understand:
			return 'bg-cyan-500/10 text-cyan-700 dark:text-cyan-400 border-cyan-200 dark:border-cyan-800'
		case CognitiveLevel.Apply:
			return 'bg-green-500/10 text-green-700 dark:text-green-400 border-green-200 dark:border-green-800'
		case CognitiveLevel.Analyze:
			return 'bg-amber-500/10 text-amber-700 dark:text-amber-400 border-amber-200 dark:border-amber-800'
		default:
			return ''
	}
}

interface AIGeneratorSectionProps {
	exam: Exam | undefined
	topics: Topic[]
	matrix?: Matrix
	/** Remaining questions needed per matrix row (key = matrixRowKey) */
	matrixRemainingCounts?: Record<string, number>
	/** Per-row question type override. null/undefined = random */
	matrixTopicQuestionTypes: Record<string, QuestionType | null>
	onMatrixTopicTypeChange: (key: string, type: QuestionType | null) => void
	selectedTopic: string
	onTopicChange: (topicId: string) => void
	cognitiveLevel: CognitiveLevel
	onCognitiveLevelChange: (level: CognitiveLevel) => void
	questionType: QuestionType
	onQuestionTypeChange: (type: QuestionType) => void
	isGenerating: boolean
	generatedQuestions: AIGeneratedQuestion[]
	onGenerate: (
		subject: string,
		grade: number,
		topicName: string,
		cognitiveLevel: CognitiveLevel,
		questionType: QuestionType,
		language?: 'vi' | 'en',
		topicDescription?: string
	) => Promise<AIGeneratedQuestion | undefined>
	onGenerateMatrix?: () => Promise<void>
	onEditQuestion: (question: AIGeneratedQuestion) => void
	onSaveQuestion: (question: AIGeneratedQuestion, index: number) => void
	onSaveAllQuestions?: () => Promise<void>
	onRemoveQuestion: (index: number) => void
	onClearAll: () => void
	isSaving: boolean
	isSavingAll?: boolean
}

/**
 * Redesigned AI question generation section.
 *
 * Layout:
 * 1. PRIMARY card: "Generate from Matrix Blueprint" – matrix rows with per-row question-type selector.
 * 2. SECONDARY collapsible: "Generate single question" for one-off generation.
 * 3. Generated questions preview with Save / Save All / Clear controls.
 */
export const AIGeneratorSection: React.FC<AIGeneratorSectionProps> = ({
	exam,
	topics,
	matrix,
	matrixRemainingCounts = {},
	matrixTopicQuestionTypes,
	onMatrixTopicTypeChange,
	selectedTopic,
	onTopicChange,
	cognitiveLevel,
	onCognitiveLevelChange,
	questionType,
	onQuestionTypeChange,
	isGenerating,
	generatedQuestions,
	onGenerate,
	onGenerateMatrix,
	onEditQuestion,
	onSaveQuestion,
	onSaveAllQuestions,
	onRemoveQuestion,
	onClearAll,
	isSaving,
	isSavingAll = false,
}) => {
	const { t, i18n } = useTranslation()
	const [singleOpen, setSingleOpen] = useState(false)
	const [aiQuantity, setAiQuantity] = useState(1)

	const currentLang: 'vi' | 'en' = i18n.language?.startsWith('vi') ? 'vi' : 'en'
	const langLabel =
		currentLang === 'vi'
			? t('pages.exams.questions.ai.language_vietnamese')
			: t('pages.exams.questions.ai.language_english')

	const allFulfilled =
		!!matrix &&
		matrix.matrixTopics.every(row => {
			const key = matrixRowKey(row.topicId, row.cognitiveLevel)
			const remaining = matrixRemainingCounts[key]
			return remaining !== undefined && remaining <= 0
		})

	const handleSingleGenerate = async () => {
		if (!exam || !selectedTopic) return
		const selectedTopicData = topics.find(tp => tp.id === selectedTopic)
		if (!selectedTopicData) return
		for (let i = 0; i < aiQuantity; i++) {
			await onGenerate(
				exam.name ?? 'General',
				exam.grade ?? 10,
				selectedTopicData.title,
				cognitiveLevel,
				questionType,
				currentLang,
				selectedTopicData.description ?? ''
			)
		}
	}

	const typeSelectValue = (key: string): QuestionTypeOrRandom => {
		const stored = matrixTopicQuestionTypes[key]
		return stored == null ? RANDOM_TYPE : stored
	}

	const handleTypeChange = (key: string, val: string) => {
		if (val === RANDOM_TYPE) {
			onMatrixTopicTypeChange(key, null)
		} else {
			onMatrixTopicTypeChange(key, Number(val) as QuestionType)
		}
	}

	return (
		<>
			{/* ── PRIMARY: Matrix Generation ─────────────────────────── */}
			<Card className='border-2 border-primary/20 bg-gradient-to-br from-primary/5 to-transparent'>
				<CardHeader className='pb-3'>
					<div className='flex items-start justify-between gap-4'>
						<div className='flex items-center gap-3'>
							<div className='p-2 bg-primary/10 rounded-lg shrink-0'>
								<Zap className='h-5 w-5 text-primary' />
							</div>
							<div>
								<CardTitle className='text-base'>
									{t('pages.exams.questions.ai.matrix_hero_title')}
								</CardTitle>
								<p className='text-xs text-muted-foreground mt-0.5 max-w-lg'>
									{t('pages.exams.questions.ai.matrix_hero_description')}
								</p>
							</div>
						</div>
						<Badge variant='outline' className='flex items-center gap-1 shrink-0 self-start'>
							<Globe className='h-3 w-3' />
							{langLabel}
						</Badge>
					</div>
				</CardHeader>

				<CardContent className='space-y-4'>
					{!matrix ? (
						<div className='flex items-center gap-3 p-4 bg-muted/50 rounded-lg border border-dashed'>
							<Info className='h-4 w-4 text-muted-foreground shrink-0' />
							<p className='text-sm text-muted-foreground'>
								{t('pages.exams.questions.ai.matrix_empty')}
							</p>
						</div>
					) : allFulfilled ? (
						<div className='flex items-center gap-3 p-4 bg-green-500/10 rounded-lg border border-green-200 dark:border-green-800'>
							<CheckCircle2 className='h-4 w-4 text-green-600 dark:text-green-400 shrink-0' />
							<p className='text-sm text-green-700 dark:text-green-400 font-medium'>
								{t('pages.exams.questions.ai.matrix_complete')}
							</p>
						</div>
					) : (
						<>
							{/* Matrix topic table */}
							<div className='rounded-md border overflow-hidden'>
								{/* Header */}
								<div className='grid grid-cols-[1fr_auto_auto_180px] bg-muted/50 border-b'>
									<div className='px-3 py-2 text-xs font-medium text-muted-foreground'>
										{t('pages.exams.questions.ai.matrix_topic_table_topic')}
									</div>
									<div className='px-3 py-2 text-xs font-medium text-muted-foreground text-center'>
										{t('pages.exams.questions.ai.matrix_topic_table_level')}
									</div>
									<div className='px-3 py-2 text-xs font-medium text-muted-foreground text-center min-w-[80px]'>
										{t('pages.exams.questions.ai.matrix_topic_table_count')}
									</div>
									<div className='px-3 py-2 text-xs font-medium text-muted-foreground'>
										{t('pages.exams.questions.ai.matrix_topic_table_type')}
									</div>
								</div>

								{/* Rows */}
								{matrix.matrixTopics.map((row, idx) => {
									const topic = topics.find(tp => tp.id === row.topicId)
									const key = matrixRowKey(row.topicId, row.cognitiveLevel)
									const remaining = matrixRemainingCounts[key]
									const isFulfilled = remaining !== undefined && remaining <= 0
									const typeVal = typeSelectValue(key)

									return (
										<div
											key={key}
											className={`grid grid-cols-[1fr_auto_auto_180px] border-b last:border-b-0 transition-colors ${
												isFulfilled
													? 'opacity-50'
													: idx % 2 === 0
													? 'bg-background'
													: 'bg-muted/20'
											}`}
										>
											<div className='px-3 py-2.5 flex items-center'>
												<span className='text-sm font-medium truncate'>
													{topic?.title ?? row.topicId}
												</span>
											</div>
											<div className='px-3 py-2.5 flex items-center justify-center'>
												<Badge
													variant='outline'
													className={`text-xs whitespace-nowrap ${getCognitiveLevelColor(row.cognitiveLevel)}`}
												>
													{t(
														`exams.cognitive_levels.${CognitiveLevel[row.cognitiveLevel].toLowerCase()}`
													)}
												</Badge>
											</div>
											<div className='px-3 py-2.5 flex items-center justify-center min-w-[80px]'>
												{isFulfilled ? (
													<CheckCircle2 className='h-4 w-4 text-green-500' />
												) : (
													<Badge variant='secondary' className='text-xs'>
														{t('pages.exams.questions.ai.matrix_questions_needed', {
															count: remaining ?? row.quantity,
														})}
													</Badge>
												)}
											</div>
											<div className='px-3 py-2 flex items-center'>
												<Select
													value={String(typeVal)}
													onValueChange={val => handleTypeChange(key, val)}
													disabled={isFulfilled || isGenerating}
												>
													<SelectTrigger className='h-8 text-xs w-full'>
														<SelectValue />
													</SelectTrigger>
													<SelectContent>
														<SelectItem value={RANDOM_TYPE} className='text-xs'>
															<span className='flex items-center gap-1.5'>
																<Sparkles className='h-3 w-3 text-muted-foreground' />
																{t(
																	'pages.exams.questions.ai.matrix_question_type_random'
																)}
															</span>
														</SelectItem>
														<Separator className='my-1' />
														{Object.values(QuestionType)
															.filter(v => typeof v === 'number')
															.map(v => {
																const cfg =
																	QUESTION_TYPE_CONFIGS[v as QuestionType]
																return (
																	<SelectItem
																		key={v}
																		value={String(v)}
																		className='text-xs'
																	>
																		{cfg?.label ?? String(v)}
																	</SelectItem>
																)
															})}
													</SelectContent>
												</Select>
											</div>
										</div>
									)
								})}
							</div>

							<Button
								onClick={onGenerateMatrix}
								disabled={isGenerating || !onGenerateMatrix}
								className='w-full'
								size='lg'
							>
								{isGenerating ? (
									<>
										<Loader2 className='h-4 w-4 mr-2 animate-spin' />
										{t('pages.exams.questions.ai.matrix_generating')}
									</>
								) : (
									<>
										<Zap className='h-4 w-4 mr-2' />
										{t('pages.exams.questions.ai.matrix_generate')}
									</>
								)}
							</Button>
						</>
					)}
				</CardContent>
			</Card>

			{/* ── SECONDARY: Single Question Generator (collapsible) ── */}
			<div className='rounded-lg border bg-card overflow-hidden'>
				<button
					type='button'
					onClick={() => setSingleOpen(v => !v)}
					className='w-full flex items-center justify-between px-4 py-3 hover:bg-muted/40 transition-colors text-left'
				>
					<div className='flex items-center gap-2'>
						<Sparkles className='h-4 w-4 text-muted-foreground' />
						<span className='text-sm font-medium'>
							{t('pages.exams.questions.ai.single_section_title')}
						</span>
						<span className='text-xs text-muted-foreground hidden sm:inline'>
							–{' '}{t('pages.exams.questions.ai.single_section_description')}
						</span>
					</div>
					{singleOpen ? (
						<ChevronUp className='h-4 w-4 text-muted-foreground shrink-0' />
					) : (
						<ChevronDown className='h-4 w-4 text-muted-foreground shrink-0' />
					)}
				</button>

				{singleOpen && (
					<div className='px-4 pb-4 pt-1 space-y-4 border-t'>
						<div className='grid grid-cols-2 gap-4'>
							<div className='space-y-2'>
								<Label className='text-xs'>
									{t('pages.exams.questions.ai.fields.topic')}
								</Label>
								<TopicSelector
									topics={topics}
									value={selectedTopic}
									onValueChange={onTopicChange}
									placeholder={t('pages.exams.questions.ai.placeholders.topic')}
								/>
							</div>
							<div className='space-y-2'>
								<Label className='text-xs'>
									{t('pages.exams.questions.ai.fields.type')}
								</Label>
								<QuestionTypeSelector
									value={questionType}
									onValueChange={onQuestionTypeChange}
								/>
							</div>
						</div>
						<div className='grid grid-cols-2 gap-4'>
							<div className='space-y-2'>
								<Label className='text-xs'>
									{t('pages.exams.questions.ai.fields.cognitive_level')}
								</Label>
								<CognitiveLevelSelector
									value={cognitiveLevel}
									onValueChange={onCognitiveLevelChange}
								/>
							</div>
							<div className='space-y-2'>
								<Label className='text-xs'>
									{t('pages.exams.questions.ai.fields.quantity')}
								</Label>
								<Input
									type='number'
									value={aiQuantity}
									onChange={e =>
										setAiQuantity(
											Math.max(1, Math.min(10, Number(e.target.value) || 1))
										)
									}
									min={1}
									max={10}
									className='h-9'
								/>
							</div>
						</div>
						<Button
							onClick={handleSingleGenerate}
							disabled={isGenerating || !selectedTopic}
							className='w-full'
							variant='secondary'
						>
							{isGenerating ? (
								<>
									<Loader2 className='h-4 w-4 mr-2 animate-spin' />
									{t('pages.exams.questions.ai.generating')}
								</>
							) : (
								<>
									<Sparkles className='h-4 w-4 mr-2' />
									{t('pages.exams.questions.ai.generate', { count: aiQuantity })}
								</>
							)}
						</Button>
					</div>
				)}
			</div>

			{/* ── Generated Questions Preview ──────────────────────── */}
			{generatedQuestions.length > 0 && (
				<Card>
					<CardHeader className='flex flex-row items-center justify-between pb-3'>
						<CardTitle className='text-sm'>
							{t('pages.exams.questions.ai.generated_title', {
								count: generatedQuestions.length,
							})}
						</CardTitle>
						<div className='flex gap-2'>
							{onSaveAllQuestions && generatedQuestions.length > 1 && (
								<Button
									size='sm'
									onClick={onSaveAllQuestions}
									disabled={isSavingAll || isSaving}
									className='bg-gradient-to-r from-green-500 to-emerald-500 hover:from-green-600 hover:to-emerald-600 text-white'
								>
									{isSavingAll ? (
										<>
											<Loader2 className='h-3.5 w-3.5 mr-1.5 animate-spin' />
											{t('pages.exams.questions.ai.saving_all')}
										</>
									) : (
										<>
											<SaveAll className='h-3.5 w-3.5 mr-1.5' />
											{t('pages.exams.questions.ai.save_all', {
												count: generatedQuestions.length,
											})}
										</>
									)}
								</Button>
							)}
							<Button
								variant='outline'
								size='sm'
								onClick={onClearAll}
								disabled={isSavingAll}
							>
								{t('pages.exams.questions.ai.clear_all')}
							</Button>
						</div>
					</CardHeader>
					<CardContent className='space-y-3 pt-0'>
						{generatedQuestions.map((question, index) => (
							<AIQuestionPreview
								key={index}
								question={question}
								index={index}
								onEdit={onEditQuestion}
								onSave={onSaveQuestion}
								onRemove={onRemoveQuestion}
								isSaving={isSaving || isSavingAll}
								topicName={topics.find(tp => tp.id === question.topicId)?.title}
							/>
						))}
					</CardContent>
				</Card>
			)}
		</>
	)
}

export default AIGeneratorSection

