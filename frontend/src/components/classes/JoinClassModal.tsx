import JoinClassForm from './JoinClassForm'

interface JoinClassModalProps {
	open: boolean
	onOpenChange: (open: boolean) => void
}

const JoinClassModal = ({
	open,
	onOpenChange,
}: JoinClassModalProps): React.JSX.Element => {
	return open ? (
		<div className='absolute inset-0 pointer-events-none flex items-center justify-center backdrop-blur-sm'>
			<JoinClassForm onSuccess={() => onOpenChange(false)} />
		</div>
	) : (
		<></>
	)
}

export default JoinClassModal
