import { useGSAP } from '@gsap/react'
import React, { useRef } from 'react'

interface ContainerProps {
	children: React.ReactNode
	className?: string
	animation?:
		| ((container: HTMLDivElement) => void)
		| ((container: HTMLDivElement) => void)[]
	revertOnUpdate?: boolean
	dependencies?: unknown[]
}

const Container = ({
	children,
	className,
	animation,
	revertOnUpdate = false,
	dependencies = [],
}: ContainerProps) => {
	const container = useRef<HTMLDivElement | null>(null)

	useGSAP(
		() => {
			if (animation && container.current) {
				const animations = Array.isArray(animation) ? animation : [animation]
				animations.forEach(fn => fn(container.current!))
			}
		},
		{ scope: container, revertOnUpdate, dependencies }
	)

	return (
		<div ref={container} className={className ?? ''}>
			{children}
		</div>
	)
}

export default Container
