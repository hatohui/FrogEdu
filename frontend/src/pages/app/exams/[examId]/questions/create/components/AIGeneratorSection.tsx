import React, { useState } from 'react'
import { Sparkles, Loader2 } from 'lucide-react'
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
import type { Topic, Exam } from '@/types/model/exam-service'

interface AIGeneratorSectionProps {
	exam: Exam | undefined
	topics: Topic[]
	selectedTopic: string
	onTopicChange: (topicId: string) => void
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
	onEditQuestion: (question: AIGeneratedQuestion) => void
	onSaveQuestion: (question: AIGeneratedQuestion, index: number) => void
	onRemoveQuestion: (index: number) => void
	onClearAll: () => void
	isSaving: boolean
}

/**
 * AI question generation section
 */
export const AIGeneratorSection: React.FC<AIGeneratorSectionProps> = ({
	exam,
	topics,
	selectedTopic,
	onTopicChange,
	isGenerating,
	generatedQuestions,
	onGenerate,
	onEditQuestion,
	onSaveQuestion,
	onRemoveQuestion,
	onClearAll,
	isSaving,
}) => {
	const [aiQuantity, setAiQuantity] = useState(1)
	const [aiQuestionType, setAiQuestionType] = useState<QuestionType>(
		QuestionType.MultipleChoice
	)
	const [aiCognitiveLevel, setAiCognitiveLevel] = useState<CognitiveLevel>(
		CognitiveLevel.Remember
	)

	const handleGenerate = async () => {
		if (!exam || !selectedTopic) return

		const selectedTopicData = topics.find(t => t.id === selectedTopic)
		if (!selectedTopicData) return

		for (let i = 0; i < aiQuantity; i++) {
			await onGenerate(
				exam.name ?? 'General',
				exam.grade ?? 10,
				selectedTopicData.title,
				aiCognitiveLevel,
				aiQuestionType,
				'vi',
				selectedTopicData.description
			)
		}
	}

	return (
		<>
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
								value={aiQuestionType}
								onValueChange={setAiQuestionType}
							/>
						</div>
					</div>

					{/* Cognitive Level and Quantity */}
					<div className='grid grid-cols-2 gap-4'>
						<div className='space-y-2'>
							<Label>Cognitive Level</Label>
							<CognitiveLevelSelector
								value={aiCognitiveLevel}
								onValueChange={setAiCognitiveLevel}
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
						<Button variant='outline' size='sm' onClick={onClearAll}>
							Clear All
						</Button>
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
								isSaving={isSaving}
							/>
						))}
					</CardContent>
				</Card>
			)}
		</>
	)
}

export default AIGeneratorSection
