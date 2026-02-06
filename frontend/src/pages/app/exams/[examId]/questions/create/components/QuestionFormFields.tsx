import React from 'react'
import { Upload, X } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import {
	FormControl,
	FormDescription,
	FormField,
	FormItem,
	FormLabel,
	FormMessage,
} from '@/components/ui/form'
import { Input } from '@/components/ui/input'
import { Textarea } from '@/components/ui/textarea'
import { Checkbox } from '@/components/ui/checkbox'
import { TopicSelector } from '@/components/exams/topic-selector'
import { QuestionTypeSelector } from '@/components/exams/QuestionTypeSelector'
import { CognitiveLevelSelector } from '@/components/exams/CognitiveLevelSelector'
import { useMediaUpload } from '@/hooks/image/useMediaUpload'
import type { UseFormReturn } from 'react-hook-form'
import type { QuestionFormData } from '@/hooks/useQuestionForm'
import type { Topic } from '@/types/model/exam-service'
import { useTranslation } from 'react-i18next'

interface QuestionFormFieldsProps {
	form: UseFormReturn<QuestionFormData>
	topics: Topic[]
}

/**
 * Form fields for question details (content, type, cognitive level, etc.)
 */
export const QuestionFormFields: React.FC<QuestionFormFieldsProps> = ({
	form,
	topics,
}) => {
	const { t } = useTranslation()
	const {
		isUploading,
		preview: mediaPreview,
		handleFileChange,
		clearPreview,
	} = useMediaUpload()

	const handleMediaUpload = handleFileChange(async (file: File) => {
		// TODO: Replace with actual S3 presigned URL upload
		return `https://cdn.frogedu.com/questions/media/${encodeURIComponent(file.name)}`
	})

	const onMediaFileChange = async (
		event: React.ChangeEvent<HTMLInputElement>
	) => {
		const result = await handleMediaUpload(event)
		if (result.success && result.url) {
			form.setValue('mediaUrl', result.url)
		}
	}

	return (
		<Card>
			<CardHeader>
				<CardTitle>{t('pages.exams.questions.form.title')}</CardTitle>
			</CardHeader>
			<CardContent className='space-y-4'>
				{/* Question Content */}
				<FormField
					control={form.control}
					name='content'
					render={({ field }) => (
						<FormItem>
							<FormLabel>
								{t('pages.exams.questions.form.fields.content')}
							</FormLabel>
							<FormControl>
								<Textarea
									placeholder={t(
										'pages.exams.questions.form.placeholders.content'
									)}
									rows={4}
									{...field}
								/>
							</FormControl>
							<FormMessage />
						</FormItem>
					)}
				/>

				{/* Type and Cognitive Level */}
				<div className='grid grid-cols-2 gap-4'>
					<FormField
						control={form.control}
						name='type'
						render={({ field }) => (
							<FormItem>
								<FormLabel>
									{t('pages.exams.questions.form.fields.type')}
								</FormLabel>
								<FormControl>
									<QuestionTypeSelector
										value={field.value}
										onValueChange={field.onChange}
									/>
								</FormControl>
								<FormMessage />
							</FormItem>
						)}
					/>

					<FormField
						control={form.control}
						name='cognitiveLevel'
						render={({ field }) => (
							<FormItem>
								<FormLabel>
									{t('pages.exams.questions.form.fields.cognitive_level')}
								</FormLabel>
								<FormControl>
									<CognitiveLevelSelector
										value={field.value}
										onValueChange={field.onChange}
									/>
								</FormControl>
								<FormMessage />
							</FormItem>
						)}
					/>
				</div>

				{/* Points and Topic */}
				<div className='grid grid-cols-2 gap-4 items-start'>
					<FormField
						control={form.control}
						name='point'
						render={({ field }) => (
							<FormItem>
								<FormLabel>
									{t('pages.exams.questions.form.fields.points')}
								</FormLabel>
								<FormControl>
									<Input
										type='number'
										step='0.5'
										{...field}
										onChange={e => field.onChange(Number(e.target.value))}
									/>
								</FormControl>
								<FormMessage />
							</FormItem>
						)}
					/>

					<FormField
						control={form.control}
						name='topicId'
						render={({ field }) => (
							<FormItem>
								<FormLabel>
									{t('pages.exams.questions.form.fields.topic')}
								</FormLabel>
								<FormControl>
									<TopicSelector
										topics={topics}
										value={field.value}
										onValueChange={field.onChange}
										placeholder={t(
											'pages.exams.questions.form.placeholders.topic'
										)}
									/>
								</FormControl>
								<FormDescription>
									{t('pages.exams.questions.form.help.topic')}
								</FormDescription>
								<FormMessage />
							</FormItem>
						)}
					/>
				</div>

				{/* Public Checkbox */}
				<FormField
					control={form.control}
					name='isPublic'
					render={({ field }) => (
						<FormItem className='flex flex-row items-start space-x-3 space-y-0'>
							<FormControl>
								<Checkbox
									checked={field.value}
									onCheckedChange={field.onChange}
								/>
							</FormControl>
							<div className='space-y-1 leading-none'>
								<FormLabel>
									{t('pages.exams.questions.form.fields.public')}
								</FormLabel>
								<FormDescription>
									{t('pages.exams.questions.form.help.public')}
								</FormDescription>
							</div>
						</FormItem>
					)}
				/>

				{/* Media Upload */}
				<div className='space-y-2'>
					<FormLabel>{t('pages.exams.questions.form.fields.media')}</FormLabel>
					<div className='flex gap-2'>
						<Input
							type='file'
							accept='image/*,video/*'
							onChange={onMediaFileChange}
							disabled={isUploading}
						/>
						<Button type='button' variant='outline' disabled={isUploading}>
							<Upload className='h-4 w-4 mr-2' />
							{isUploading
								? t('pages.exams.questions.form.actions.uploading')
								: t('pages.exams.questions.form.actions.upload')}
						</Button>
					</div>
					{mediaPreview && (
						<div className='mt-2 relative'>
							<img
								src={mediaPreview}
								alt='Preview'
								className='max-h-32 rounded border'
							/>
							<Button
								type='button'
								variant='ghost'
								size='icon'
								className='absolute top-1 right-1 h-6 w-6'
								onClick={() => {
									clearPreview()
									form.setValue('mediaUrl', '')
								}}
							>
								<X className='h-4 w-4' />
							</Button>
						</div>
					)}
					{form.watch('mediaUrl') && !mediaPreview && (
						<p className='text-sm text-muted-foreground'>
							{t('pages.exams.questions.form.media_uploaded', {
								url: form.watch('mediaUrl'),
							})}
						</p>
					)}
				</div>
			</CardContent>
		</Card>
	)
}

export default QuestionFormFields
