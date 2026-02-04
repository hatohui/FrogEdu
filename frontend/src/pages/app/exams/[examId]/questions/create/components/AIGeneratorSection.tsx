import React, { useState } from 'react'
import { Sparkles, Loader2, Zap, SaveAll } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Label } from '@/components/ui/label'
import { Slider } from '@/components/ui/slider'
import { TopicSelector } from '@/components/exams/topic-selector'
import { QuestionTypeSelector } from '@/components/exams/QuestionTypeSelector'
import { CognitiveLevelSelector } from '@/components/exams/CognitiveLevelSelector'
import { AIQuestionPreview } from '@/components/exams/AIQuestionPreview'
import { QuestionType, CognitiveLevel } from '@/types/model/exam-service'
import type { AIGeneratedQuestion } from '@/types/model/ai-service'
import type { Topic, Exam, Matrix } from '@/types/model/exam-service'

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
				<Card className='border-amber-200 bg-gradient-to-br from-amber-50 to-orange-50'>
					<CardHeader>
						<CardTitle className='flex items-center gap-2'>
							<Zap className='h-5 w-5 text-amber-600' />
							Generate Entire Matrix
						</CardTitle>
					</CardHeader>
					<CardContent>
						<p className='text-sm text-muted-foreground mb-4'>
							Generate all remaining questions needed to complete the exam
							matrix in one go.
						</p>
						<Button
							onClick={onGenerateMatrix}
							disabled={isGenerating}
							className='w-full bg-gradient-to-r from-amber-600 to-orange-600 hover:from-amber-700 hover:to-orange-700'
							size='lg'
						>
							{isGenerating ? (
								<>
									<Loader2 className='h-4 w-4 mr-2 animate-spin' />
									Generating Matrix...
								</>
							) : (
								<>
									<Zap className='h-4 w-4 mr-2' />
									Generate All Matrix Questions
								</>
							)}
						</Button>
					</CardContent>
				</Card>
			)}

			{/* AI Generation Card */}
			<Card>
				<CardHeader>
					<CardTitle className='flex items-center gap-2'>
						<Sparkles className='h-5 w-5 text-amber-500' />
						AI Question Generator
					</CardTitle>
				</CardHeader>
				<CardContent className='space-y-4'>
					{/* Topic and Question Type */}
					<div className='grid grid-cols-2 gap-4'>
						<div className='space-y-2'>
							<Label>Topic *</Label>
							<TopicSelector
								topics={topics}
								value={selectedTopic}
								onValueChange={onTopicChange}
								placeholder='Select a topic...'
							/>
						</div>
						<div className='space-y-2'>
							<Label>Question Type</Label>
							<QuestionTypeSelector
								value={questionType}
								onValueChange={onQuestionTypeChange}
							/>
						</div>
					</div>

					{/* Cognitive Level and Quantity */}
					<div className='grid grid-cols-2 gap-4'>
						<div className='space-y-2'>
							<Label>Cognitive Level</Label>
							<CognitiveLevelSelector
								value={cognitiveLevel}
								onValueChange={onCognitiveLevelChange}
							/>
						</div>
						<div className='space-y-2'>
							<Label>Quantity: {aiQuantity}</Label>
							<Slider
								value={[aiQuantity]}
								onValueChange={([v]) => setAiQuantity(v)}
								min={1}
								max={5}
								step={1}
								className='mt-2'
							/>
						</div>
					</div>

					<Button
						onClick={handleGenerate}
						disabled={isGenerating || !selectedTopic}
						className='w-full bg-gradient-to-r from-amber-500 to-orange-500 hover:from-amber-600 hover:to-orange-600'
					>
						{isGenerating ? (
							<>
								<Loader2 className='h-4 w-4 mr-2 animate-spin' />
								Generating...
							</>
						) : (
							<>
								<Sparkles className='h-4 w-4 mr-2' />
								Generate {aiQuantity} Question{aiQuantity > 1 ? 's' : ''}
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
							Generated Questions ({generatedQuestions.length})
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
											Saving All...
										</>
									) : (
										<>
											<SaveAll className='h-4 w-4 mr-2' />
											Save All ({generatedQuestions.length})
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
								Clear All
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
