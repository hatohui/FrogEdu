import React, { useState } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import * as z from 'zod'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Avatar, AvatarImage } from '@/components/ui/avatar'
import {
	Form,
	FormControl,
	FormField,
	FormItem,
	FormLabel,
	FormMessage,
} from '@/components/ui/form'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { Loader2, Camera, Check } from 'lucide-react'
import { toast } from 'sonner'
import userService from '@/services/user.service'
import { resendVerificationEmail } from '@/services/verify.service'
import type { GetMeResponse, UpdateProfileDto } from '@/types/dtos/users/user'
import { useUploadImage } from '@/hooks/image/useUploadImage'
import FallBackUserAvatar from './UserAvatar'

const profileSchema = z.object({
	firstName: z
		.string()
		.min(1, { message: 'First name is required' })
		.max(100, { message: 'First name must not exceed 100 characters' }),
	lastName: z
		.string()
		.min(1, { message: 'Last name is required' })
		.max(100, { message: 'Last name must not exceed 100 characters' }),
})

type ProfileFormValues = z.infer<typeof profileSchema>

interface ProfileFormProps {
	user: GetMeResponse
	onSuccess?: () => void
}

const ProfileForm = ({
	user,
	onSuccess,
}: ProfileFormProps): React.JSX.Element => {
	const queryClient = useQueryClient()
	const [isVerifying, setIsVerifying] = useState(false)
	const { avatarPreview, isUploadingAvatar, handleAvatarChange } =
		useUploadImage()

	const form = useForm<ProfileFormValues>({
		resolver: zodResolver(profileSchema),
		defaultValues: {
			firstName: user.firstName || '',
			lastName: user.lastName || '',
		},
	})

	const updateProfileMutation = useMutation({
		mutationFn: (data: UpdateProfileDto) => userService.updateProfile(data),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ['currentUser'] })
			toast.success('Profile updated successfully')
			onSuccess?.()
		},
		onError: error => {
			toast.error('Failed to update profile')
			console.error('Profile update error:', error)
		},
	})

	const onSubmit = async (data: ProfileFormValues) => {
		await updateProfileMutation.mutateAsync(data)
	}

	const handleResendVerification = async () => {
		setIsVerifying(true)
		try {
			await resendVerificationEmail(user.email)
			toast.success('Verification email sent!')
		} catch (err) {
			toast.error('Failed to send verification email')
			console.error(err)
		} finally {
			setIsVerifying(false)
		}
	}

	return (
		<Card>
			<CardHeader>
				<CardTitle>Profile Information</CardTitle>
				<CardDescription>
					Update your personal information and avatar.
				</CardDescription>
			</CardHeader>
			<CardContent className='space-y-6'>
				{/* Avatar Section */}
				<div className='flex items-center gap-6'>
					<div className='relative'>
						<Avatar className='h-24 w-24'>
							<AvatarImage
								src={
									avatarPreview ? avatarPreview : user?.avatarUrl || undefined
								}
								alt={`${user?.firstName} ${user?.lastName}`}
							/>
							<FallBackUserAvatar user={user} />
						</Avatar>
						<label
							htmlFor='avatar-upload'
							className='absolute bottom-0 right-0 flex h-8 w-8 cursor-pointer items-center justify-center rounded-full bg-primary text-primary-foreground shadow-md hover:bg-primary/90 transition-colors'
						>
							{isUploadingAvatar ? (
								<Loader2 className='h-4 w-4 animate-spin' />
							) : (
								<Camera className='h-4 w-4' />
							)}
						</label>
						<input
							id='avatar-upload'
							type='file'
							accept='image/*'
							className='hidden'
							onChange={handleAvatarChange}
							disabled={isUploadingAvatar}
						/>
					</div>
					<div>
						<h3 className='font-medium'>Profile Picture</h3>
						<p className='text-sm text-muted-foreground'>
							Click the camera icon to upload a new photo.
						</p>
						<p className='text-xs text-muted-foreground mt-1'>
							JPG, PNG, GIF up to 5MB
						</p>
					</div>
				</div>

				{/* Profile Form */}
				<Form {...form}>
					<form onSubmit={form.handleSubmit(onSubmit)} className='space-y-4'>
						<div className='grid gap-4 sm:grid-cols-2'>
							<FormField
								control={form.control}
								name='firstName'
								render={({ field }) => (
									<FormItem>
										<FormLabel>First Name</FormLabel>
										<FormControl>
											<Input placeholder='John' {...field} />
										</FormControl>
										<FormMessage />
									</FormItem>
								)}
							/>
							<FormField
								control={form.control}
								name='lastName'
								render={({ field }) => (
									<FormItem>
										<FormLabel>Last Name</FormLabel>
										<FormControl>
											<Input placeholder='Doe' {...field} />
										</FormControl>
										<FormMessage />
									</FormItem>
								)}
							/>
						</div>

						{/* Read-only fields */}
						<div className='grid gap-4 sm:grid-cols-2'>
							<div className='space-y-2'>
								<label className='text-sm font-medium'>Email</label>
								<Input value={user.email} disabled />
								<div className='flex items-center gap-2'>
									{!user.isEmailVerified && (
										<>
											<span className='text-xs text-destructive font-semibold'>
												Unverified
											</span>
											<Button
												type='button'
												size='sm'
												variant='outline'
												disabled={isVerifying}
												onClick={handleResendVerification}
											>
												{isVerifying ? (
													<Loader2 className='h-3 w-3 animate-spin mr-1' />
												) : null}
												Verify Email
											</Button>
										</>
									)}
									{user.isEmailVerified && (
										<span className='text-xs text-green-600 font-semibold'>
											Verified
										</span>
									)}
								</div>
								<p className='text-xs text-muted-foreground'>
									Email cannot be changed
								</p>
							</div>
							<div className='space-y-2'>
								<label className='text-sm font-medium'>Role</label>
								<Input value={user.roleId} disabled />
							</div>
						</div>

						<div className='flex justify-end'>
							<Button
								type='submit'
								disabled={
									updateProfileMutation.isPending || !form.formState.isDirty
								}
							>
								{updateProfileMutation.isPending ? (
									<>
										<Loader2 className='mr-2 h-4 w-4 animate-spin' />
										Saving...
									</>
								) : (
									<>
										<Check className='mr-2 h-4 w-4' />
										Save Changes
									</>
								)}
							</Button>
						</div>
					</form>
				</Form>
			</CardContent>
		</Card>
	)
}

export default ProfileForm
