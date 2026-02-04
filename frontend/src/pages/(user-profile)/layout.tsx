import React from 'react'
import { NavLink } from 'react-router'
import { User, CreditCard, Settings, Shield, Receipt } from 'lucide-react'
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
	{ to: '/profile/transactions', label: 'Transactions', icon: Receipt },
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
		<div className='min-h-[calc(100vh-4rem)] flex'>
			{/* Left Sidebar Navigation */}
			<aside className='w-64 border-r bg-card flex-shrink-0'>
				<nav className='p-4 space-y-1'>
					{navItems.map(item => (
						<NavLink
							key={item.to}
							to={item.to}
							end={item.to === '/profile'}
							className={({ isActive }) =>
								cn(
									'flex items-center gap-3 px-4 py-3 rounded-lg text-sm font-medium transition-colors',
									isActive
										? 'bg-primary text-primary-foreground shadow-sm'
										: 'text-muted-foreground hover:text-foreground hover:bg-accent'
								)
							}
						>
							<item.icon className='h-5 w-5' />
							<span className='flex-1'>{item.label}</span>
							{item.label === 'Subscription' && isPro && (
								<Badge className='bg-gradient-to-r from-amber-500 to-orange-500 text-white text-xs px-1.5 py-0'>
									Pro
								</Badge>
							)}
						</NavLink>
					))}
				</nav>
			</aside>

			{/* Main Content */}
			<main className='flex-1 p-6 overflow-auto'>{children}</main>
		</div>
	)
}

export default UserProfileLayout
