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
	Heart,
	GraduationCap,
	Target,
	Globe,
	Mail,
	MapPin,
} from 'lucide-react'
import { useTranslation } from 'react-i18next'

const AboutPage = (): React.ReactElement => {
	const { t } = useTranslation()
	return (
		<div className='min-h-screen bg-background'>
			{/* Hero Section */}
			<section className='pt-20 pb-16 px-4 md:px-0 bg-gradient-to-br from-background via-background to-[#b8d282]/5'>
				<div className='container max-w-4xl mx-auto text-center space-y-6'>
					<div className='flex justify-center mb-6'>
						<img
							src='/frog.png'
							alt={t('common.logo_alt')}
							className='w-24 h-24 drop-shadow-lg'
						/>
					</div>
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
					<h1 className='text-4xl md:text-5xl font-bold tracking-tight'>
						{t('pages.about.hero.title_prefix')}{' '}
						<span className='text-primary'>{t('common.app_name')}</span>
					</h1>
					<p className='text-lg text-muted-foreground max-w-2xl mx-auto'>
						{t('pages.about.hero.subtitle')}
					</p>
				</div>
			</section>

			{/* Mission Section */}
			<section className='py-16 px-4 md:px-0'>
				<div className='container max-w-5xl mx-auto'>
					<div className='grid md:grid-cols-2 gap-8 items-center'>
						<div className='space-y-6'>
							<div className='flex items-center gap-3'>
								<div className='w-12 h-12 rounded-full bg-primary/10 flex items-center justify-center'>
									<Target className='w-6 h-6 text-primary' />
								</div>
								<h2 className='text-3xl font-bold'>
									{t('pages.about.mission.title')}
								</h2>
							</div>
							<p className='text-muted-foreground leading-relaxed'>
								{t('pages.about.mission.paragraph_one')}
							</p>
							<p className='text-muted-foreground leading-relaxed'>
								{t('pages.about.mission.paragraph_two')}
							</p>
						</div>
						<Card className='bg-gradient-to-br from-[#286147]/5 to-[#b8d282]/10'>
							<CardContent className='p-8 space-y-4'>
								<div className='grid grid-cols-2 gap-4 text-center'>
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
							</CardContent>
						</Card>
					</div>
				</div>
			</section>

			{/* Features Section */}
			<section className='py-16 px-4 md:px-0 bg-[#286147] text-white'>
				<div className='container max-w-5xl mx-auto'>
					<div className='text-center mb-12'>
						<h2 className='text-3xl font-bold mb-4'>
							{t('pages.about.features.title')}
						</h2>
						<p className='text-white/80 max-w-2xl mx-auto'>
							{t('pages.about.features.subtitle')}
						</p>
					</div>
					<div className='grid md:grid-cols-3 gap-6'>
						<Card className='bg-white/10 border-white/20 text-white'>
							<CardHeader>
								<div className='w-12 h-12 rounded-full bg-white/20 flex items-center justify-center mb-2'>
									<Brain className='w-6 h-6' />
								</div>
								<CardTitle className='text-white'>
									{t('pages.about.features.ai_exam.title')}
								</CardTitle>
							</CardHeader>
							<CardContent>
								<CardDescription className='text-white/80'>
									{t('pages.about.features.ai_exam.description')}
								</CardDescription>
							</CardContent>
						</Card>

						<Card className='bg-white/10 border-white/20 text-white'>
							<CardHeader>
								<div className='w-12 h-12 rounded-full bg-white/20 flex items-center justify-center mb-2'>
									<GraduationCap className='w-6 h-6' />
								</div>
								<CardTitle className='text-white'>
									{t('pages.about.features.class_management.title')}
								</CardTitle>
							</CardHeader>
							<CardContent>
								<CardDescription className='text-white/80'>
									{t('pages.about.features.class_management.description')}
								</CardDescription>
							</CardContent>
						</Card>

						<Card className='bg-white/10 border-white/20 text-white'>
							<CardHeader>
								<div className='w-12 h-12 rounded-full bg-white/20 flex items-center justify-center mb-2'>
									<BookOpen className='w-6 h-6' />
								</div>
								<CardTitle className='text-white'>
									{t('pages.about.features.textbooks.title')}
								</CardTitle>
							</CardHeader>
							<CardContent>
								<CardDescription className='text-white/80'>
									{t('pages.about.features.textbooks.description')}
								</CardDescription>
							</CardContent>
						</Card>
					</div>
				</div>
			</section>

			{/* Team Section */}
			<section className='py-16 px-4 md:px-0'>
				<div className='container max-w-5xl mx-auto'>
					<div className='text-center mb-12'>
						<h2 className='text-3xl font-bold mb-4'>
							{t('pages.about.team.title')}
						</h2>
						<p className='text-muted-foreground max-w-2xl mx-auto'>
							{t('pages.about.team.subtitle')}
						</p>
					</div>
					<div className='grid md:grid-cols-2 gap-6 max-w-2xl mx-auto'>
						<Card>
							<CardContent className='p-6 flex items-center gap-4'>
								<div className='w-16 h-16 rounded-full bg-primary/10 flex items-center justify-center'>
									<Users className='w-8 h-8 text-primary' />
								</div>
								<div>
									<h3 className='font-semibold'>
										{t('pages.about.team.dedicated_title')}
									</h3>
									<p className='text-sm text-muted-foreground'>
										{t('pages.about.team.dedicated_description')}
									</p>
								</div>
							</CardContent>
						</Card>
						<Card>
							<CardContent className='p-6 flex items-center gap-4'>
								<div className='w-16 h-16 rounded-full bg-primary/10 flex items-center justify-center'>
									<Globe className='w-8 h-8 text-primary' />
								</div>
								<div>
									<h3 className='font-semibold'>
										{t('pages.about.team.made_in_title')}
									</h3>
									<p className='text-sm text-muted-foreground'>
										{t('pages.about.team.made_in_description')}
									</p>
								</div>
							</CardContent>
						</Card>
					</div>
				</div>
			</section>

			{/* Contact Section */}
			<section className='py-16 px-4 md:px-0 bg-muted/50'>
				<div className='container max-w-3xl mx-auto'>
					<Card>
						<CardHeader className='text-center'>
							<CardTitle className='text-2xl'>
								{t('pages.about.contact.title')}
							</CardTitle>
							<CardDescription>
								{t('pages.about.contact.subtitle')}
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-6'>
							<div className='grid md:grid-cols-2 gap-4'>
								<div className='flex items-center gap-3 p-4 rounded-lg bg-muted'>
									<Mail className='w-5 h-5 text-primary' />
									<div>
										<p className='text-sm text-muted-foreground'>
											{t('labels.email')}
										</p>
										<p className='font-medium'>contact@frogedu.vn</p>
									</div>
								</div>
								<div className='flex items-center gap-3 p-4 rounded-lg bg-muted'>
									<MapPin className='w-5 h-5 text-primary' />
									<div>
										<p className='text-sm text-muted-foreground'>
											{t('pages.about.contact.location_label')}
										</p>
										<p className='font-medium'>
											{t('pages.about.contact.location_value')}
										</p>
									</div>
								</div>
							</div>
							<div className='flex flex-col sm:flex-row gap-4 justify-center pt-4'>
								<Link to='/register'>
									<Button size='lg' className='w-full sm:w-auto'>
										{t('pages.about.contact.primary_cta')}
									</Button>
								</Link>
								<Link to='/'>
									<Button
										variant='outline'
										size='lg'
										className='w-full sm:w-auto'
									>
										{t('actions.back_to_home')}
									</Button>
								</Link>
							</div>
						</CardContent>
					</Card>
				</div>
			</section>
		</div>
	)
}

export default AboutPage
