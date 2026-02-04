import { useEffect, useCallback } from 'react'
import { useForm, useFieldArray } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import * as z from 'zod'
import {
	QuestionType,
	CognitiveLevel,
	QUESTION_TYPE_CONFIGS,
} from '@/types/model/exam-service'

// Answer schema
const answerSchema = z.object({
	content: z.string().min(1, 'Answer content is required'),
	isCorrect: z.boolean(),
	explanation: z.string().optional(),
})

// Base question schema
const questionSchema = z.object({
	content: z.string().min(10, 'Question must be at least 10 characters'),
	point: z.number().min(0.5).max(100),
	type: z.nativeEnum(QuestionType),
	cognitiveLevel: z.nativeEnum(CognitiveLevel),
	topicId: z.string().min(1, 'Topic is required'),
	isPublic: z.boolean(),
	mediaUrl: z.string().optional(),
	answers: z.array(answerSchema),
})

export type QuestionFormData = z.infer<typeof questionSchema>

interface UseQuestionFormOptions {
	defaultValues?: Partial<QuestionFormData>
}

/**
 * Get default answers for a question type
 */
function getDefaultAnswers(questionType: QuestionType) {
	switch (questionType) {
		case QuestionType.TrueFalse:
			return [
				{ content: 'A', isCorrect: false, explanation: '' },
				{ content: 'B', isCorrect: false, explanation: '' },
			]
		case QuestionType.Essay:
			return [{ content: '', isCorrect: true, explanation: '' }]
		case QuestionType.FillInTheBlank:
			return [{ content: '', isCorrect: true, explanation: '' }]
		case QuestionType.MultipleChoice:
		case QuestionType.MultipleAnswer:
		default:
			return [
				{ content: '', isCorrect: false, explanation: '' },
				{ content: '', isCorrect: false, explanation: '' },
			]
	}
}

/**
 * Custom hook for question form management
 * Handles type-specific validation and answer structure
 */
export function useQuestionForm(options: UseQuestionFormOptions = {}) {
	const form = useForm<QuestionFormData>({
		resolver: zodResolver(questionSchema),
		defaultValues: {
			content: '',
			point: 1,
			type: QuestionType.MultipleChoice,
			cognitiveLevel: CognitiveLevel.Remember,
			topicId: '',
			isPublic: true,
			mediaUrl: '',
			answers: getDefaultAnswers(QuestionType.MultipleChoice),
			...options.defaultValues,
		},
	})

	const { fields, append, remove, replace } = useFieldArray({
		control: form.control,
		name: 'answers',
	})

	const questionType = form.watch('type')

	// Update answers when question type changes
	useEffect(() => {
		const currentAnswers = form.getValues('answers')

		// Check if we need to reset answers for this type
		const needsReset = (() => {
			switch (questionType) {
				case QuestionType.TrueFalse:
					return (
						currentAnswers.length !== 2 || currentAnswers[0]?.content !== 'A'
					)
				case QuestionType.Essay:
					return (
						currentAnswers.length !== 1 || currentAnswers[0]?.content === 'A'
					)
				case QuestionType.FillInTheBlank:
					return currentAnswers.length < 1 || currentAnswers[0]?.content === 'A'
				case QuestionType.MultipleChoice:
				case QuestionType.MultipleAnswer:
					return currentAnswers.length < 2 || currentAnswers[0]?.content === 'A'
				default:
					return false
			}
		})()

		if (needsReset) {
			replace(getDefaultAnswers(questionType))
		}
	}, [questionType, form, replace])

	/**
	 * Handle correct answer change based on question type
	 */
	const handleCorrectAnswerChange = useCallback(
		(index: number, isChecked: boolean) => {
			const config = QUESTION_TYPE_CONFIGS[questionType]
			const currentAnswers = form.getValues('answers')

			if (!config.allowMultipleCorrect) {
				// Only one answer can be correct
				const updatedAnswers = currentAnswers.map((ans, i) => ({
					...ans,
					isCorrect: i === index ? isChecked : false,
				}))
				form.setValue('answers', updatedAnswers)
			} else {
				// Multiple answers can be correct
				form.setValue(`answers.${index}.isCorrect`, isChecked)
			}
		},
		[questionType, form]
	)

	/**
	 * Add a new answer option
	 */
	const addAnswer = useCallback(() => {
		const config = QUESTION_TYPE_CONFIGS[questionType]
		if (config.maxAnswers && fields.length >= config.maxAnswers) {
			return false
		}

		// For fill in blank, new answers are also correct
		const defaultCorrect = questionType === QuestionType.FillInTheBlank

		append({ content: '', isCorrect: defaultCorrect, explanation: '' })
		return true
	}, [questionType, fields.length, append])

	/**
	 * Remove an answer option
	 */
	const removeAnswer = useCallback(
		(index: number) => {
			const config = QUESTION_TYPE_CONFIGS[questionType]
			if (fields.length <= config.minAnswers) {
				return false
			}
			remove(index)
			return true
		},
		[questionType, fields.length, remove]
	)

	/**
	 * Check if more answers can be added
	 */
	const canAddAnswer = useCallback(() => {
		const config = QUESTION_TYPE_CONFIGS[questionType]
		return !config.maxAnswers || fields.length < config.maxAnswers
	}, [questionType, fields.length])

	/**
	 * Check if an answer can be removed
	 */
	const canRemoveAnswer = useCallback(() => {
		const config = QUESTION_TYPE_CONFIGS[questionType]
		return fields.length > config.minAnswers
	}, [questionType, fields.length])

	/**
	 * Load AI generated question into form
	 */
	const loadAIQuestion = useCallback(
		(question: {
			content: string
			questionType: QuestionType
			cognitiveLevel: CognitiveLevel
			point: number
			topicId?: string
			answers: Array<{
				content: string
				isCorrect: boolean
				explanation?: string
			}>
		}) => {
			form.setValue('content', question.content)
			form.setValue('type', question.questionType)
			form.setValue('cognitiveLevel', question.cognitiveLevel)
			form.setValue('point', question.point)
			form.setValue(
				'answers',
				question.answers.map(a => ({
					content: a.content,
					isCorrect: a.isCorrect,
					explanation: a.explanation ?? '',
				}))
			)
			if (question.topicId) {
				form.setValue('topicId', question.topicId)
			}
		},
		[form]
	)

	/**
	 * Reset form to default values
	 */
	const resetForm = useCallback(() => {
		form.reset({
			content: '',
			point: 1,
			type: QuestionType.MultipleChoice,
			cognitiveLevel: CognitiveLevel.Remember,
			topicId: '',
			isPublic: true,
			mediaUrl: '',
			answers: getDefaultAnswers(QuestionType.MultipleChoice),
		})
	}, [form])

	return {
		form,
		fields,
		questionType,
		handleCorrectAnswerChange,
		addAnswer,
		removeAnswer,
		canAddAnswer,
		canRemoveAnswer,
		loadAIQuestion,
		resetForm,
		append,
		remove,
	}
}

export default useQuestionForm
