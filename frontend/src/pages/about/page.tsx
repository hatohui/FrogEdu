import React from 'react'
import { Link } from 'react-router'
import { Button } from '@/components/ui/button'
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import {
	BookOpen,
	Brain,
	Users,
	Sparkles,
	MapPin,
	Heart,
	Target,
	Zap,
} from 'lucide-react'
import { useTranslation } from 'react-i18next'

const AboutPage = (): React.ReactElement => {
	const { t } = useTranslation()

	const features = [
		{
			icon: Brain,
			title: t('pages.about.features.ai_exam.title'),
			description: t('pages.about.features.ai_exam.description'),
		},
		{
			icon: Users,
			title: t('pages.about.features.class_management.title'),
			description: t('pages.about.features.class_management.description'),
		},
		{
			icon: BookOpen,
			title: t('pages.about.features.textbooks.title'),
			description: t('pages.about.features.textbooks.description'),
		},
	]

	return (
		<div className='min-h-screen'>
			{/* Hero Section */}
			<section className='pt-32 pb-20 px-4 md:px-0 bg-gradient-to-br from-background via-background to-[#b8d282]/5'>
				<div className='container max-w-4xl mx-auto text-center space-y-8'>
					<div className='space-y-4'>
						<div className='flex justify-center gap-2 flex-wrap'>
							<Badge variant='secondary'>
								<Sparkles className='w-3 h-3 mr-1' />
								{t('pages.about.hero.badge_ai')}
							</Badge>
							<Badge variant='secondary'>
								<Heart className='w-3 h-3 mr-1' />
								{t('pages.about.hero.badge_made_in')}
							</Badge>
						</div>
						<h1 className='text-5xl md:text-6xl font-bold tracking-tight'>
							{t('pages.about.hero.title_prefix')}{' '}
							<span className='text-primary'>FrogEdu</span>
						</h1>
						<p className='text-xl text-muted-foreground max-w-2xl mx-auto'>
							{t('pages.about.hero.subtitle')}
						</p>
					</div>

					{/* Stats */}
					<div className='grid grid-cols-2 sm:grid-cols-4 gap-4 pt-12 max-w-2xl mx-auto'>
						<div className='space-y-2'>
							<p className='text-3xl font-bold text-primary'>5</p>
							<p className='text-sm text-muted-foreground'>
								{t('pages.about.stats.grade_levels')}
							</p>
						</div>
						<div className='space-y-2'>
							<p className='text-3xl font-bold text-primary'>1000+</p>
							<p className='text-sm text-muted-foreground'>
								{t('pages.about.stats.questions')}
							</p>
						</div>
						<div className='space-y-2'>
							<p className='text-3xl font-bold text-primary'>AI</p>
							<p className='text-sm text-muted-foreground'>
								{t('pages.about.stats.powered')}
							</p>
						</div>
						<div className='space-y-2'>
							<p className='text-3xl font-bold text-primary'>24/7</p>
							<p className='text-sm text-muted-foreground'>
								{t('pages.about.stats.available')}
							</p>
						</div>
					</div>
				</div>
			</section>

			{/* Mission Section - Dark frog green */}
			<section className='py-20 px-4 md:px-0 bg-[#286147] text-white'>
				<div className='container max-w-4xl mx-auto'>
					<div className='grid md:grid-cols-2 gap-12 items-center'>
						<div className='space-y-6'>
							<div className='w-14 h-14 rounded-2xl bg-[#8db376] flex items-center justify-center'>
								<Target className='w-7 h-7 text-[#033a1e]' />
							</div>
							<h2 className='text-4xl font-bold'>
								{t('pages.about.mission.title')}
							</h2>
							<p className='text-[#b8d282] text-lg leading-relaxed'>
								{t('pages.about.mission.paragraph_one')}
							</p>
							<p className='text-white/80 leading-relaxed'>
								{t('pages.about.mission.paragraph_two')}
							</p>
						</div>
						<div className='grid grid-cols-2 gap-4'>
							<Card className='bg-[#4d8f6d] border-[#8db376] text-white'>
								<CardContent className='pt-6 space-y-2'>
									<p className='text-3xl font-bold text-[#b8d282]'>5</p>
									<p className='text-sm text-white/80'>
										{t('pages.about.stats.grade_levels')}
									</p>
								</CardContent>
							</Card>
							<Card className='bg-[#4d8f6d] border-[#8db376] text-white'>
								<CardContent className='pt-6 space-y-2'>
									<p className='text-3xl font-bold text-[#b8d282]'>1000+</p>
									<p className='text-sm text-white/80'>
										{t('pages.about.stats.questions')}
									</p>
								</CardContent>
							</Card>
							<Card className='bg-[#4d8f6d] border-[#8db376] text-white col-span-2'>
								<CardContent className='pt-6 space-y-2'>
									<p className='text-3xl font-bold text-[#b8d282]'>AI</p>
									<p className='text-sm text-white/80'>
										{t('pages.about.stats.powered')}
									</p>
								</CardContent>
							</Card>
						</div>
					</div>
				</div>
			</section>

			{/* Features Section */}
			<section className='py-20 px-4 md:px-0 bg-gradient-to-b from-background to-[#b8d282]/10'>
				<div className='container max-w-5xl mx-auto'>
					<div className='text-center space-y-4 mb-16'>
						<h2 className='text-4xl font-bold'>
							{t('pages.about.features.title')}
						</h2>
						<p className='text-lg text-muted-foreground max-w-2xl mx-auto'>
							{t('pages.about.features.subtitle')}
						</p>
					</div>

					<div className='grid md:grid-cols-3 gap-6'>
						{features.map(feature => (
							<Card
								key={feature.title}
								className='border-border hover:border-primary/50 transition-colors duration-300'
							>
								<CardHeader>
									<div className='w-12 h-12 rounded-lg bg-primary/10 flex items-center justify-center mb-4'>
										<feature.icon className='w-6 h-6 text-primary' />
									</div>
									<CardTitle>{feature.title}</CardTitle>
									<CardDescription>{feature.description}</CardDescription>
								</CardHeader>
							</Card>
						))}
					</div>
				</div>
			</section>

			{/* Team Section - Medium frog green */}
			<section className='py-20 px-4 md:px-0 bg-[#8db376]'>
				<div className='container max-w-4xl mx-auto text-center space-y-12'>
					<div className='space-y-4'>
						<h2 className='text-4xl font-bold text-[#033a1e]'>
							{t('pages.about.team.title')}
						</h2>
						<p className='text-lg text-[#033a1e]/80 max-w-2xl mx-auto'>
							{t('pages.about.team.subtitle')}
						</p>
					</div>

					<div className='grid sm:grid-cols-2 gap-6 max-w-2xl mx-auto'>
						<Card className='bg-white/20 border-white/30 text-[#033a1e]'>
							<CardHeader className='text-center'>
								<div className='w-14 h-14 rounded-2xl bg-[#033a1e] flex items-center justify-center mx-auto mb-4'>
									<Users className='w-7 h-7 text-[#b8d282]' />
								</div>
								<CardTitle className='text-[#033a1e]'>
									{t('pages.about.team.dedicated_title')}
								</CardTitle>
								<CardDescription className='text-[#033a1e]/70'>
									{t('pages.about.team.dedicated_description')}
								</CardDescription>
							</CardHeader>
						</Card>
						<Card className='bg-white/20 border-white/30 text-[#033a1e]'>
							<CardHeader className='text-center'>
								<div className='w-14 h-14 rounded-2xl bg-[#033a1e] flex items-center justify-center mx-auto mb-4'>
									<Zap className='w-7 h-7 text-[#b8d282]' />
								</div>
								<CardTitle className='text-[#033a1e]'>
									{t('pages.about.team.made_in_title')}
								</CardTitle>
								<CardDescription className='text-[#033a1e]/70'>
									{t('pages.about.team.made_in_description')}
								</CardDescription>
							</CardHeader>
						</Card>
					</div>
				</div>
			</section>

			{/* Contact / CTA Section */}
			<section className='py-20 px-4 md:px-0 bg-gradient-to-br from-background via-background to-[#b8d282]/5'>
				<div className='container max-w-3xl mx-auto text-center space-y-8'>
					<h2 className='text-4xl font-bold'>
						{t('pages.about.contact.title')}
					</h2>
					<p className='text-lg text-muted-foreground'>
						{t('pages.about.contact.subtitle')}
					</p>

					<div className='flex items-center justify-center gap-2 text-muted-foreground'>
						<MapPin className='w-4 h-4 text-primary' />
						<span className='text-sm'>
							{t('pages.about.contact.location_label')}:{' '}
							<span className='font-medium text-foreground'>
								{t('pages.about.contact.location_value')}
							</span>
						</span>
					</div>

					<div className='flex flex-col sm:flex-row gap-4 justify-center pt-4'>
						<Link to='/register'>
							<Button size='lg' className='w-full sm:w-auto'>
								{t('pages.about.contact.primary_cta')}
							</Button>
						</Link>
						<Link to='/'>
							<Button variant='outline' size='lg' className='w-full sm:w-auto'>
								{t('buttons.back_to_home')}
							</Button>
						</Link>
					</div>
				</div>
			</section>
		</div>
	)
}

export default AboutPage
