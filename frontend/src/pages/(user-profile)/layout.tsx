import React from 'react'
import { NavLink } from 'react-router'
import { User, CreditCard, Settings, Shield } from 'lucide-react'
import { cn } from '@/utils/shadcn'
import { Badge } from '@/components/ui/badge'
import { useSubscription } from '@/hooks/useSubscription'

interface NavItem {
	to: string
	label: string
	icon: React.ElementType
}

const navItems: NavItem[] = [
	{ to: '/profile', label: 'Profile', icon: User },
	{ to: '/profile/subscription', label: 'Subscription', icon: CreditCard },
	{ to: '/profile/security', label: 'Security', icon: Shield },
	{ to: '/profile/settings', label: 'Settings', icon: Settings },
]

const UserProfileLayout = ({
	children,
}: {
	children: React.ReactNode
}): React.ReactElement => {
	const { isPro } = useSubscription()

	return (
		<div className='min-h-[calc(100vh-4rem)]'>
			<div className='max-w-4xl mx-auto px-4 py-6'>
				{/* Inline Tab Navigation */}
				<nav className='flex gap-1 p-1 bg-muted/50 rounded-lg mb-6 overflow-x-auto'>
					{navItems.map(item => (
						<NavLink
							key={item.to}
							to={item.to}
							end={item.to === '/profile'}
							className={({ isActive }) =>
								cn(
									'flex items-center gap-2 px-4 py-2 rounded-md text-sm font-medium transition-colors whitespace-nowrap',
									isActive
										? 'bg-background text-foreground shadow-sm'
										: 'text-muted-foreground hover:text-foreground hover:bg-background/50'
								)
							}
						>
							<item.icon className='h-4 w-4' />
							<span>{item.label}</span>
							{item.label === 'Subscription' && isPro && (
								<Badge
									variant='secondary'
									className='ml-1 bg-gradient-to-r from-amber-500 to-orange-500 text-white text-xs px-1.5 py-0'
								>
									Pro
								</Badge>
							)}
						</NavLink>
					))}
				</nav>

				{/* Main Content */}
				<main>{children}</main>
			</div>
		</div>
	)
}

export default UserProfileLayout
