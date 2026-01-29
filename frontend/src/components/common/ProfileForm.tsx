import React, { useCallback, useState } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import * as z from 'zod'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'
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
import userService, {
	type UserDto,
	type UpdateProfileDto,
} from '@/services/user.service'

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
	user: UserDto
	onSuccess?: () => void
}

const ProfileForm = ({
	user,
	onSuccess,
}: ProfileFormProps): React.JSX.Element => {
	const queryClient = useQueryClient()
	const [avatarPreview, setAvatarPreview] = useState<string | null>(null)
	const [isUploadingAvatar, setIsUploadingAvatar] = useState(false)

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

	const handleAvatarChange = useCallback(
		async (event: React.ChangeEvent<HTMLInputElement>) => {
			const file = event.target.files?.[0]
			if (!file) return

			// Validate file type
			if (!file.type.startsWith('image/')) {
				toast.error('Please select an image file')
				return
			}

			// Validate file size (max 5MB)
			if (file.size > 5 * 1024 * 1024) {
				toast.error('Image must be less than 5MB')
				return
			}

			// Show preview
			const reader = new FileReader()
			reader.onload = e => {
				setAvatarPreview(e.target?.result as string)
			}
			reader.readAsDataURL(file)

			// Upload to S3
			setIsUploadingAvatar(true)
			try {
				await userService.uploadAvatar(file)
				queryClient.invalidateQueries({ queryKey: ['currentUser'] })
				toast.success('Avatar updated successfully')
			} catch (error) {
				toast.error('Failed to upload avatar')
				console.error('Avatar upload error:', error)
				setAvatarPreview(null)
			} finally {
				setIsUploadingAvatar(false)
			}
		},
		[queryClient]
	)

	const getUserInitials = () => {
		if (!user) return ''
		const first =
			user.firstName && typeof user.firstName === 'string'
				? user.firstName.charAt(0)
				: ''
		const last =
			user.lastName && typeof user.lastName === 'string'
				? user.lastName.charAt(0)
				: ''
		const initials = (first + last).toUpperCase()
		if (initials) return initials
		if (user.email && typeof user.email === 'string' && user.email.length > 0)
			return user.email.charAt(0).toUpperCase()
		return ''
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
								src={avatarPreview || user.avatarUrl}
								alt={`${user.firstName} ${user.lastName}`}
							/>
							<AvatarFallback className='bg-primary text-primary-foreground text-2xl'>
								{getUserInitials()}
							</AvatarFallback>
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
								<p className='text-xs text-muted-foreground'>
									Email cannot be changed
								</p>
							</div>
							<div className='space-y-2'>
								<label className='text-sm font-medium'>Role</label>
								<Input value={user.role} disabled />
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
