import React, { useState } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import {
	Form,
	FormControl,
	FormDescription,
	FormField,
	FormItem,
	FormMessage,
} from '@/components/ui/form'
import { Button } from '@/components/ui/button'
import {
	InputOTP,
	InputOTPGroup,
	InputOTPSlot,
} from '@/components/ui/input-otp'
import { Loader2, UserPlus, X } from 'lucide-react'
import { useJoinClass } from '@/hooks/useClasses'
import { useTranslation } from 'react-i18next'
import type { TFunction } from 'i18next'

const joinClassSchema = (t: TFunction) =>
	z.object({
		inviteCode: z
			.string()
			.length(6, t('forms.classes.validation.invite_length'))
			.regex(/^[A-Z0-9]+$/i, t('forms.classes.validation.invite_format')),
	})

type JoinClassFormData = z.infer<ReturnType<typeof joinClassSchema>>

interface JoinClassFormProps {
	onSuccess?: () => void
}

const JoinClassForm: React.FC<JoinClassFormProps> = ({ onSuccess }) => {
	const { t } = useTranslation()
	const joinClass = useJoinClass()
	const [isComplete, setIsComplete] = useState(false)
	const schema = React.useMemo(() => joinClassSchema(t), [t])

	const form = useForm<JoinClassFormData>({
		resolver: zodResolver(schema),
		defaultValues: {
			inviteCode: '',
		},
	})

	const onSubmit = async (data: JoinClassFormData) => {
		await joinClass.mutateAsync(data.inviteCode.toUpperCase())
		form.reset()
		setIsComplete(false)
		onSuccess?.()
	}

	return (
		<Card className='w-full max-w-md'>
			<CardHeader className='text-center'>
				<div className='flex pointer-events-auto justify-end w-full'>
					<Button size='sm' variant='outline' onClick={() => onSuccess?.()}>
						<X className='h-4 w-4' />
					</Button>
				</div>
				<div className='mx-auto mb-4 flex h-12 w-12 items-center justify-center rounded-full bg-primary/10'>
					<UserPlus className='h-6 w-6 text-primary' />
				</div>
				<CardTitle>{t('pages.classes.join.title')}</CardTitle>
				<CardDescription>{t('pages.classes.join.subtitle')}</CardDescription>
			</CardHeader>
			<CardContent>
				<Form {...form}>
					<form onSubmit={form.handleSubmit(onSubmit)} className='space-y-6'>
						<FormField
							control={form.control}
							name='inviteCode'
							render={({ field }) => (
								<FormItem className='flex flex-col items-center'>
									<FormControl>
										<InputOTP
											maxLength={6}
											value={field.value}
											onChange={(value: string) => {
												field.onChange(value.toUpperCase())
												setIsComplete(value.length === 6)
											}}
										>
											<InputOTPGroup>
												<InputOTPSlot index={0} className='uppercase' />
												<InputOTPSlot index={1} className='uppercase' />
												<InputOTPSlot index={2} className='uppercase' />
												<InputOTPSlot index={3} className='uppercase' />
												<InputOTPSlot index={4} className='uppercase' />
												<InputOTPSlot index={5} className='uppercase' />
											</InputOTPGroup>
										</InputOTP>
									</FormControl>
									<FormDescription className='text-center mt-2'>
										{t('pages.classes.join.case_insensitive')}
									</FormDescription>
									<FormMessage />
								</FormItem>
							)}
						/>

						<Button
							type='submit'
							className='w-full pointer-events-auto'
							disabled={!isComplete || joinClass.isPending}
						>
							{joinClass.isPending ? (
								<>
									<Loader2 className='mr-2 h-4 w-4 animate-spin' />
									{t('pages.classes.join.actions.joining')}
								</>
							) : (
								t('pages.classes.join.actions.join')
							)}
						</Button>
					</form>
				</Form>
			</CardContent>
		</Card>
	)
}

export default JoinClassForm
