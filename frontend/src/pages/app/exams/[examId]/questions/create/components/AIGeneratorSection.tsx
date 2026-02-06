import React, { useState } from 'react'
import { Sparkles, Loader2, Zap, SaveAll } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Label } from '@/components/ui/label'
import { Input } from '@/components/ui/input'
import { TopicSelector } from '@/components/exams/topic-selector'
import { QuestionTypeSelector } from '@/components/exams/QuestionTypeSelector'
import { CognitiveLevelSelector } from '@/components/exams/CognitiveLevelSelector'
import { AIQuestionPreview } from '@/components/exams/AIQuestionPreview'
import { QuestionType, CognitiveLevel } from '@/types/model/exam-service'
import type { AIGeneratedQuestion } from '@/types/model/ai-service'
import type { Topic, Exam, Matrix } from '@/types/model/exam-service'
import { useTranslation } from 'react-i18next'

interface AIGeneratorSectionProps {
	exam: Exam | undefined
	topics: Topic[]
	matrix?: Matrix
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
 * AI question generation section
 */
export const AIGeneratorSection: React.FC<AIGeneratorSectionProps> = ({
	exam,
	topics,
	matrix,
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
	const { t } = useTranslation()
	const [aiQuantity, setAiQuantity] = useState(1)

	const handleGenerate = async () => {
		if (!exam || !selectedTopic) return

		const selectedTopicData = topics.find(t => t.id === selectedTopic)
		if (!selectedTopicData) return

		for (let i = 0; i < aiQuantity; i++) {
			await onGenerate(
				exam.name ?? 'General',
				exam.grade ?? 10,
				selectedTopicData.title,
				cognitiveLevel,
				questionType,
				'vi',
				selectedTopicData.description
			)
		}
	}

	return (
		<>
			{/* Matrix Generation Card */}
			{matrix && onGenerateMatrix && (
				<Card>
					<CardHeader>
						<CardTitle className='flex items-center gap-2 text-sm'>
							<Zap className='h-4 w-4' />
							{t('pages.exams.questions.ai.matrix_title')}
						</CardTitle>
					</CardHeader>
					<CardContent>
						<p className='text-sm text-muted-foreground mb-4'>
							{t('pages.exams.questions.ai.matrix_description')}
						</p>
						<Button
							onClick={onGenerateMatrix}
							disabled={isGenerating}
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
					</CardContent>
				</Card>
			)}

			{/* AI Generation Card */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2 text-sm'>
						<Sparkles className='h-4 w-4' />
						{t('pages.exams.questions.ai.title')}
					</CardTitle>
				</CardHeader>
				<CardContent className='space-y-4'>
					{/* Topic and Question Type */}
					<div className='grid grid-cols-2 gap-4'>
						<div className='space-y-2'>
							<Label>{t('pages.exams.questions.ai.fields.topic')}</Label>
							<TopicSelector
								topics={topics}
								value={selectedTopic}
								onValueChange={onTopicChange}
								placeholder={t('pages.exams.questions.ai.placeholders.topic')}
							/>
						</div>
						<div className='space-y-2'>
							<Label>{t('pages.exams.questions.ai.fields.type')}</Label>
							<QuestionTypeSelector
								value={questionType}
								onValueChange={onQuestionTypeChange}
							/>
						</div>
					</div>

					{/* Cognitive Level and Quantity */}
					<div className='grid grid-cols-2 gap-4'>
						<div className='space-y-2'>
							<Label>
								{t('pages.exams.questions.ai.fields.cognitive_level')}
							</Label>
							<CognitiveLevelSelector
								value={cognitiveLevel}
								onValueChange={onCognitiveLevelChange}
							/>
						</div>
						<div className='space-y-2'>
							<Label>{t('pages.exams.questions.ai.fields.quantity')}</Label>
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
							/>
						</div>
					</div>

					<Button
						onClick={handleGenerate}
						disabled={isGenerating || !selectedTopic}
						className='w-full'
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
				</CardContent>
			</Card>

			{/* Generated Questions Preview */}
			{generatedQuestions.length > 0 && (
				<Card>
					<CardHeader className='flex flex-row items-center justify-between'>
						<CardTitle>
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
									className='bg-gradient-to-r from-green-500 to-emerald-500 hover:from-green-600 hover:to-emerald-600'
								>
									{isSavingAll ? (
										<>
											<Loader2 className='h-4 w-4 mr-2 animate-spin' />
											{t('pages.exams.questions.ai.saving_all')}
										</>
									) : (
										<>
											<SaveAll className='h-4 w-4 mr-2' />
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
					<CardContent className='space-y-4'>
						{generatedQuestions.map((question, index) => (
							<AIQuestionPreview
								key={index}
								question={question}
								index={index}
								onEdit={onEditQuestion}
								onSave={onSaveQuestion}
								onRemove={onRemoveQuestion}
								isSaving={isSaving || isSavingAll}
								topicName={topics.find(t => t.id === question.topicId)?.title}
							/>
						))}
					</CardContent>
				</Card>
			)}
		</>
	)
}

export default AIGeneratorSection
