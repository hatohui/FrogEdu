import React from 'react'
import {
	AlertDialog,
	AlertDialogAction,
	AlertDialogCancel,
	AlertDialogContent,
	AlertDialogDescription,
	AlertDialogFooter,
	AlertDialogHeader,
	AlertDialogTitle,
} from '@/components/ui/alert-dialog'

interface ConfirmDialogProps {
	open: boolean
	onOpenChange: (open: boolean) => void
	title: string
	description: string
	onConfirm: () => void
	onCancel?: () => void
	confirmText?: string
	cancelText?: string
	variant?: 'default' | 'destructive'
}

export const ConfirmDialog: React.FC<ConfirmDialogProps> = ({
	open,
	onOpenChange,
	title,
	description,
	onConfirm,
	onCancel,
	confirmText = 'Continue',
	cancelText = 'Cancel',
	variant = 'default',
}) => {
	const handleConfirm = () => {
		onConfirm()
		onOpenChange(false)
	}

	const handleCancel = () => {
		onCancel?.()
		onOpenChange(false)
	}

	return (
		<AlertDialog open={open} onOpenChange={onOpenChange}>
			<AlertDialogContent>
				<AlertDialogHeader>
					<AlertDialogTitle>{title}</AlertDialogTitle>
					<AlertDialogDescription>{description}</AlertDialogDescription>
				</AlertDialogHeader>
				<AlertDialogFooter>
					<AlertDialogCancel onClick={handleCancel}>
						{cancelText}
					</AlertDialogCancel>
					<AlertDialogAction
						onClick={handleConfirm}
						className={
							variant === 'destructive'
								? 'bg-destructive text-destructive-foreground hover:bg-destructive/90'
								: ''
						}
					>
						{confirmText}
					</AlertDialogAction>
				</AlertDialogFooter>
			</AlertDialogContent>
		</AlertDialog>
	)
}
