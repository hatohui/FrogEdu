import React from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogFooter,
	DialogHeader,
	DialogTitle,
} from '@/components/ui/dialog'
import {
	Form,
	FormControl,
	FormDescription,
	FormField,
	FormItem,
	FormLabel,
	FormMessage,
} from '@/components/ui/form'
import { Input } from '@/components/ui/input'
import { Button } from '@/components/ui/button'
import { Textarea } from '@/components/ui/textarea'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { Loader2 } from 'lucide-react'
import { useCreateClass } from '@/hooks/useClasses'
import { useTranslation } from 'react-i18next'
import type { TFunction } from 'i18next'

const createClassSchema = (t: TFunction) =>
	z.object({
		name: z
			.string()
			.min(1, t('forms.classes.validation.name_required'))
			.max(200, t('forms.classes.validation.name_max')),
		grade: z.string().min(1, t('forms.classes.validation.grade_required')),
		description: z
			.string()
			.min(1, t('forms.classes.validation.description_required'))
			.max(1000, t('forms.classes.validation.description_max')),
		maxStudents: z
			.string()
			.min(1, t('forms.classes.validation.max_students_required'))
			.refine(
				val => !isNaN(Number(val)) && Number(val) > 0,
				t('forms.classes.validation.max_students_positive')
			)
			.refine(
				val => Number(val) <= 500,
				t('forms.classes.validation.max_students_limit')
			),
		bannerUrl: z
			.string()
			.url(t('forms.classes.validation.banner_url_invalid'))
			.optional()
			.or(z.literal('')),
	})

type CreateClassFormData = z.infer<ReturnType<typeof createClassSchema>>

interface CreateClassModalProps {
	open: boolean
	onOpenChange: (open: boolean) => void
}

const CreateClassModal: React.FC<CreateClassModalProps> = ({
	open,
	onOpenChange,
}) => {
	const { t } = useTranslation()
	const createClass = useCreateClass()
	const schema = React.useMemo(() => createClassSchema(t), [t])

	const form = useForm<CreateClassFormData>({
		resolver: zodResolver(schema),
		defaultValues: {
			name: '',
			grade: '10',
			description: '',
			maxStudents: '30',
			bannerUrl: '',
		},
	})

	const onSubmit = async (data: CreateClassFormData) => {
		await createClass.mutateAsync({
			name: data.name,
			grade: data.grade,
			description: data.description,
			maxStudents: parseInt(data.maxStudents, 10),
			bannerUrl: data.bannerUrl || undefined,
		})
		form.reset()
		onOpenChange(false)
	}

	return (
		<Dialog open={open} onOpenChange={onOpenChange}>
			<DialogContent className='sm:max-w-[500px]'>
				<DialogHeader>
					<DialogTitle>{t('pages.classes.create_modal.title')}</DialogTitle>
					<DialogDescription>
						{t('pages.classes.create_modal.description')}
					</DialogDescription>
				</DialogHeader>

				<Form {...form}>
					<form onSubmit={form.handleSubmit(onSubmit)} className='space-y-4'>
						<FormField
							control={form.control}
							name='name'
							render={({ field }) => (
								<FormItem>
									<FormLabel>
										{t('pages.classes.create_modal.fields.name')}
									</FormLabel>
									<FormControl>
										<Input
											placeholder={t(
												'pages.classes.create_modal.placeholders.name'
											)}
											{...field}
										/>
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>

						<div className='grid grid-cols-2 gap-4'>
							<FormField
								control={form.control}
								name='grade'
								render={({ field }) => (
									<FormItem>
										<FormLabel>
											{t('pages.classes.create_modal.fields.grade')}
										</FormLabel>
										<Select
											onValueChange={field.onChange}
											defaultValue={field.value}
										>
											<FormControl>
												<SelectTrigger>
													<SelectValue
														placeholder={t(
															'pages.classes.create_modal.placeholders.grade'
														)}
													/>
												</SelectTrigger>
											</FormControl>
											<SelectContent>
												{Array.from({ length: 12 }, (_, i) => i + 1).map(
													grade => (
														<SelectItem key={grade} value={grade.toString()}>
															{t('pages.classes.create_modal.grade_option', {
																grade,
															})}
														</SelectItem>
													)
												)}
											</SelectContent>
										</Select>
										<FormMessage />
									</FormItem>
								)}
							/>

							<FormField
								control={form.control}
								name='maxStudents'
								render={({ field }) => (
									<FormItem>
										<FormLabel>
											{t('pages.classes.create_modal.fields.max_students')}
										</FormLabel>
										<FormControl>
											<Input
												type='number'
												placeholder={t(
													'pages.classes.create_modal.placeholders.max_students'
												)}
												{...field}
											/>
										</FormControl>
										<FormDescription>
											{t('pages.classes.create_modal.help.max_students')}
										</FormDescription>
										<FormMessage />
									</FormItem>
								)}
							/>
						</div>

						<FormField
							control={form.control}
							name='description'
							render={({ field }) => (
								<FormItem>
									<FormLabel>
										{t('pages.classes.create_modal.fields.description')}
									</FormLabel>
									<FormControl>
										<Textarea
											placeholder={t(
												'pages.classes.create_modal.placeholders.description'
											)}
											className='resize-none'
											{...field}
										/>
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>

						<FormField
							control={form.control}
							name='bannerUrl'
							render={({ field }) => (
								<FormItem>
									<FormLabel>
										{t('pages.classes.create_modal.fields.banner_url')}
									</FormLabel>
									<FormControl>
										<Input
											type='url'
											placeholder={t(
												'pages.classes.create_modal.placeholders.banner_url'
											)}
											{...field}
										/>
									</FormControl>
									<FormDescription>
										{t('pages.classes.create_modal.help.banner_url')}
									</FormDescription>
									<FormMessage />
								</FormItem>
							)}
						/>

						<DialogFooter>
							<Button
								type='button'
								variant='outline'
								onClick={() => onOpenChange(false)}
							>
								{t('common.cancel')}
							</Button>
							<Button type='submit' disabled={createClass.isPending}>
								{createClass.isPending && (
									<Loader2 className='mr-2 h-4 w-4 animate-spin' />
								)}
								{t('pages.classes.create_modal.actions.create')}
							</Button>
						</DialogFooter>
					</form>
				</Form>
			</DialogContent>
		</Dialog>
	)
}

export default CreateClassModal
