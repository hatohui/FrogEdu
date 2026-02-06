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
import { Input } from '@/components/ui/input'
import {
	Accordion,
	AccordionContent,
	AccordionItem,
	AccordionTrigger,
} from '@/components/ui/accordion'
import {
	Tooltip,
	TooltipContent,
	TooltipProvider,
	TooltipTrigger,
} from '@/components/ui/tooltip'
import {
	BookOpen,
	Brain,
	BarChart3,
	Users,
	Sparkles,
	Heart,
} from 'lucide-react'
import { useTranslation } from 'react-i18next'

const LandingPage = (): React.ReactElement => {
	const { t } = useTranslation()
	const benefits = [
		{
			title: t('pages.landing.benefits.save_time.title'),
			description: t('pages.landing.benefits.save_time.description'),
		},
		{
			title: t('pages.landing.benefits.improve_learning.title'),
			description: t('pages.landing.benefits.improve_learning.description'),
		},
		{
			title: t('pages.landing.benefits.better_assessment.title'),
			description: t('pages.landing.benefits.better_assessment.description'),
		},
		{
			title: t('pages.landing.benefits.student_engagement.title'),
			description: t('pages.landing.benefits.student_engagement.description'),
		},
		{
			title: t('pages.landing.benefits.data_driven.title'),
			description: t('pages.landing.benefits.data_driven.description'),
		},
		{
			title: t('pages.landing.benefits.vietnamese_curriculum.title'),
			description: t(
				'pages.landing.benefits.vietnamese_curriculum.description'
			),
		},
	]
	const faqs = [
		{
			question: t('pages.landing.faq.q1.question'),
			answer: t('pages.landing.faq.q1.answer'),
		},
		{
			question: t('pages.landing.faq.q2.question'),
			answer: t('pages.landing.faq.q2.answer'),
		},
		{
			question: t('pages.landing.faq.q3.question'),
			answer: t('pages.landing.faq.q3.answer'),
		},
		{
			question: t('pages.landing.faq.q4.question'),
			answer: t('pages.landing.faq.q4.answer'),
		},
		{
			question: t('pages.landing.faq.q5.question'),
			answer: t('pages.landing.faq.q5.answer'),
		},
	]
	return (
		<div className='min-h-screen'>
			{/* Hero Section - Light with gradient */}
			<section className='pt-32 pb-20 px-4 md:px-0 bg-gradient-to-br from-background via-background to-[#b8d282]/5'>
				<div className='container max-w-4xl mx-auto text-center space-y-8'>
					<div className='space-y-4'>
						<div className='flex justify-center gap-2 flex-wrap'>
							<Badge variant='secondary'>
								<Sparkles className='w-3 h-3 mr-1' />
								{t('pages.landing.hero.badge_ai')}
							</Badge>
							<Badge variant='secondary'>
								<Heart className='w-3 h-3 mr-1' />
								{t('pages.landing.hero.badge_curriculum')}
							</Badge>
						</div>
						<h1 className='text-5xl md:text-6xl font-bold tracking-tight'>
							{t('pages.landing.hero.title_prefix')}{' '}
							<span className='text-primary'>
								{t('pages.landing.hero.title_emphasis')}
							</span>
						</h1>
						<p className='text-xl text-muted-foreground max-w-2xl mx-auto'>
							{t('pages.landing.hero.subtitle')}
						</p>
					</div>

					<div className='flex flex-col sm:flex-row gap-4 justify-center pt-6'>
						<Link to='/register'>
							<Button size='lg' className='w-full sm:w-auto'>
								{t('pages.landing.hero.primary_cta')}
							</Button>
						</Link>
						<Link to='/login'>
							<Button variant='outline' size='lg' className='w-full sm:w-auto'>
								{t('pages.landing.hero.secondary_cta')}
							</Button>
						</Link>
					</div>

					{/* Stats */}
					<div className='grid grid-cols-3 gap-4 pt-12 max-w-2xl mx-auto'>
						<div className='space-y-2'>
							<p className='text-3xl font-bold text-primary'>5</p>
							<p className='text-sm text-muted-foreground'>
								{t('pages.landing.hero.stats.grade_levels')}
							</p>
						</div>
						<div className='space-y-2'>
							<p className='text-3xl font-bold text-primary'>1000+</p>
							<p className='text-sm text-muted-foreground'>
								{t('pages.landing.hero.stats.questions')}
							</p>
						</div>
						<div className='space-y-2'>
							<p className='text-3xl font-bold text-primary'>AI</p>
							<p className='text-sm text-muted-foreground'>
								{t('pages.landing.hero.stats.powered')}
							</p>
						</div>
					</div>
				</div>
			</section>

			{/* Features Section - Dark frog green background */}
			<section
				id='features'
				className='py-20 px-4 md:px-0 bg-[#286147] text-white'
			>
				<div className='container max-w-5xl mx-auto'>
					<div className='text-center space-y-4 mb-16'>
						<h2 className='text-4xl font-bold'>
							{t('pages.landing.features.title')}
						</h2>
						<p className='text-lg text-[#b8d282]'>
							{t('pages.landing.features.subtitle')}
						</p>
					</div>

					<div className='grid md:grid-cols-2 gap-6'>
						{/* Smart Exam Generator */}
						<Card className='bg-[#4d8f6d] border-[#8db376] text-white'>
							<CardHeader>
								<div className='w-12 h-12 rounded-lg bg-[#8db376] flex items-center justify-center mb-4'>
									<TooltipProvider>
										<Tooltip>
											<TooltipTrigger asChild>
												<BarChart3 className='w-6 h-6 text-[#033a1e] cursor-help' />
											</TooltipTrigger>
											<TooltipContent>
												<p>{t('pages.landing.features.smart_exam.tooltip')}</p>
											</TooltipContent>
										</Tooltip>
									</TooltipProvider>
								</div>
								<CardTitle>
									{t('pages.landing.features.smart_exam.title')}
								</CardTitle>
								<CardDescription className='text-[#b8d282]'>
									{t('pages.landing.features.smart_exam.description')}
								</CardDescription>
							</CardHeader>
							<CardContent className='space-y-2'>
								<p className='text-sm'>
									✓{' '}
									{t('pages.landing.features.smart_exam.bullets.by_difficulty')}
								</p>
								<p className='text-sm'>
									✓{' '}
									{t(
										'pages.landing.features.smart_exam.bullets.auto_selection'
									)}
								</p>
								<p className='text-sm'>
									✓ {t('pages.landing.features.smart_exam.bullets.pdf_export')}
								</p>
								<p className='text-sm'>
									✓{' '}
									{t(
										'pages.landing.features.smart_exam.bullets.manual_override'
									)}
								</p>
							</CardContent>
						</Card>

						{/* AI Student Tutor */}
						<Card className='bg-[#4d8f6d] border-[#8db376] text-white'>
							<CardHeader>
								<div className='w-12 h-12 rounded-lg bg-[#8db376] flex items-center justify-center mb-4'>
									<TooltipProvider>
										<Tooltip>
											<TooltipTrigger asChild>
												<Brain className='w-6 h-6 text-[#033a1e] cursor-help' />
											</TooltipTrigger>
											<TooltipContent>
												<p>{t('pages.landing.features.ai_tutor.tooltip')}</p>
											</TooltipContent>
										</Tooltip>
									</TooltipProvider>
								</div>
								<CardTitle>
									{t('pages.landing.features.ai_tutor.title')}
								</CardTitle>
								<CardDescription className='text-[#b8d282]'>
									{t('pages.landing.features.ai_tutor.description')}
								</CardDescription>
							</CardHeader>
							<CardContent className='space-y-2'>
								<p className='text-sm'>
									✓ {t('pages.landing.features.ai_tutor.bullets.realtime_chat')}
								</p>
								<p className='text-sm'>
									✓ {t('pages.landing.features.ai_tutor.bullets.socratic')}
								</p>
								<p className='text-sm'>
									✓ {t('pages.landing.features.ai_tutor.bullets.textbook_refs')}
								</p>
								<p className='text-sm'>
									✓ {t('pages.landing.features.ai_tutor.bullets.history')}
								</p>
							</CardContent>
						</Card>

						{/* Content Library */}
						<Card className='bg-[#4d8f6d] border-[#8db376] text-white'>
							<CardHeader>
								<div className='w-12 h-12 rounded-lg bg-[#8db376] flex items-center justify-center mb-4'>
									<TooltipProvider>
										<Tooltip>
											<TooltipTrigger asChild>
												<BookOpen className='w-6 h-6 text-[#033a1e] cursor-help' />
											</TooltipTrigger>
											<TooltipContent>
												<p>{t('pages.landing.features.textbook.tooltip')}</p>
											</TooltipContent>
										</Tooltip>
									</TooltipProvider>
								</div>
								<CardTitle>
									{t('pages.landing.features.textbook.title')}
								</CardTitle>
								<CardDescription className='text-[#b8d282]'>
									{t('pages.landing.features.textbook.description')}
								</CardDescription>
							</CardHeader>
							<CardContent className='space-y-2'>
								<p className='text-sm'>
									✓{' '}
									{t('pages.landing.features.textbook.bullets.grade_textbooks')}
								</p>
								<p className='text-sm'>
									✓ {t('pages.landing.features.textbook.bullets.navigation')}
								</p>
								<p className='text-sm'>
									✓ {t('pages.landing.features.textbook.bullets.assets')}
								</p>
								<p className='text-sm'>
									✓ {t('pages.landing.features.textbook.bullets.search')}
								</p>
							</CardContent>
						</Card>

						{/* Class Management */}
						<Card className='bg-[#4d8f6d] border-[#8db376] text-white'>
							<CardHeader>
								<div className='w-12 h-12 rounded-lg bg-[#8db376] flex items-center justify-center mb-4'>
									<TooltipProvider>
										<Tooltip>
											<TooltipTrigger asChild>
												<Users className='w-6 h-6 text-[#033a1e] cursor-help' />
											</TooltipTrigger>
											<TooltipContent>
												<p>
													{t('pages.landing.features.class_management.tooltip')}
												</p>
											</TooltipContent>
										</Tooltip>
									</TooltipProvider>
								</div>
								<CardTitle>
									{t('pages.landing.features.class_management.title')}
								</CardTitle>
								<CardDescription className='text-[#b8d282]'>
									{t('pages.landing.features.class_management.description')}
								</CardDescription>
							</CardHeader>
							<CardContent className='space-y-2'>
								<p className='text-sm'>
									✓{' '}
									{t('pages.landing.features.class_management.bullets.create')}
								</p>
								<p className='text-sm'>
									✓{' '}
									{t(
										'pages.landing.features.class_management.bullets.enrollment'
									)}
								</p>
								<p className='text-sm'>
									✓{' '}
									{t(
										'pages.landing.features.class_management.bullets.progress'
									)}
								</p>
								<p className='text-sm'>
									✓{' '}
									{t(
										'pages.landing.features.class_management.bullets.analytics'
									)}
								</p>
							</CardContent>
						</Card>
					</div>
				</div>
			</section>

			{/* Benefits Section - Light background */}
			<section
				id='benefits'
				className='py-20 px-4 md:px-0 bg-gradient-to-b from-background to-[#b8d282]/10'
			>
				<div className='container max-w-4xl mx-auto'>
					<div className='text-center space-y-4 mb-16'>
						<h2 className='text-4xl font-bold'>
							{t('pages.landing.benefits.title')}
						</h2>
						<p className='text-lg text-muted-foreground'>
							{t('pages.landing.benefits.subtitle')}
						</p>
					</div>

					<div className='space-y-4'>
						{benefits.map((benefit, idx) => (
							<Card key={idx}>
								<CardContent className='pt-6'>
									<h3 className='font-semibold mb-2'>{benefit.title}</h3>
									<p className='text-muted-foreground text-sm'>
										{benefit.description}
									</p>
								</CardContent>
							</Card>
						))}
					</div>
				</div>
			</section>

			{/* About Section - Medium frog green background */}
			<section id='about' className='py-20 px-4 md:px-0 bg-[#8db376]'>
				<div className='container max-w-4xl mx-auto text-center space-y-8'>
					<h2 className='text-4xl font-bold text-white'>
						{t('pages.landing.about.title')}
					</h2>
					<p className='text-lg text-[#e8f3d8] max-w-2xl mx-auto'>
						{t('pages.landing.about.subtitle')}
					</p>

					<div className='grid md:grid-cols-3 gap-8 pt-8'>
						<div className='bg-[#286147]/40 rounded-lg p-6 backdrop-blur-sm'>
							<p className='text-3xl font-bold text-white mb-2'>100%</p>
							<p className='text-[#e8f3d8]'>
								{t('pages.landing.about.stats.curriculum')}
							</p>
						</div>
						<div className='bg-[#286147]/40 rounded-lg p-6 backdrop-blur-sm'>
							<p className='text-3xl font-bold text-white mb-2'>24/7</p>
							<p className='text-[#e8f3d8]'>
								{t('pages.landing.about.stats.support')}
							</p>
						</div>
						<div className='bg-[#286147]/40 rounded-lg p-6 backdrop-blur-sm'>
							<p className='text-3xl font-bold text-white mb-2'>∞</p>
							<p className='text-[#e8f3d8]'>
								{t('pages.landing.about.stats.scalable')}
							</p>
						</div>
					</div>
				</div>
			</section>

			{/* CTA Section */}
			<section className='py-20 px-4 md:px-0'>
				<div className='container max-w-3xl mx-auto text-center space-y-8'>
					<h2 className='text-4xl font-bold'>{t('pages.landing.cta.title')}</h2>
					<p className='text-lg text-muted-foreground'>
						{t('pages.landing.cta.subtitle')}
					</p>

					<div className='flex flex-col sm:flex-row gap-4 justify-center'>
						<Link to='/register'>
							<Button size='lg' className='w-full sm:w-auto'>
								{t('pages.landing.cta.primary_cta')}
							</Button>
						</Link>
						<Link to='/login'>
							<Button variant='outline' size='lg' className='w-full sm:w-auto'>
								{t('actions.sign_in')}
							</Button>
						</Link>
					</div>
				</div>
			</section>

			{/* FAQ Section */}
			<section className='py-20 px-4 md:px-0 bg-[#286147] text-white'>
				<div className='container max-w-3xl mx-auto'>
					<div className='text-center space-y-4 mb-12'>
						<h2 className='text-4xl font-bold'>
							{t('pages.landing.faq.title')}
						</h2>
						<p className='text-lg text-muted-foreground'>
							{t('pages.landing.faq.subtitle')}
						</p>
					</div>

					<Accordion type='single' collapsible className='w-full'>
						{faqs.map((item, index) => (
							<AccordionItem key={item.question} value={`item-${index + 1}`}>
								<AccordionTrigger>{item.question}</AccordionTrigger>
								<AccordionContent>{item.answer}</AccordionContent>
							</AccordionItem>
						))}
					</Accordion>
				</div>
			</section>

			{/* Newsletter Section */}
			<section className='py-20 px-4 md:px-0 bg-[#8db376]'>
				<div className='container max-w-2xl mx-auto text-center space-y-8'>
					<div className='space-y-4'>
						<h2 className='text-4xl font-bold text-white'>
							{t('pages.landing.newsletter.title')}
						</h2>
						<p className='text-lg text-[#e8f3d8]'>
							{t('pages.landing.newsletter.subtitle')}
						</p>
					</div>

					<div className='flex gap-2 max-w-md mx-auto'>
						<Input
							type='email'
							placeholder={t('placeholders.email')}
							className='flex-1 bg-[#4d8f6d] border-[#b8d282] text-white placeholder:text-[#b8d282]'
						/>
						<Button className='bg-white text-[#286147] hover:bg-[#e8f3d8]'>
							{t('actions.subscribe')}
						</Button>
					</div>
				</div>
			</section>

			{/* Footer */}
			<footer className='border-t py-12 px-4 bg-[#033a1e] text-white'>
				<div className='container max-w-6xl mx-auto px-4'>
					<div className='grid md:grid-cols-4 gap-8 mb-8'>
						<div>
							<h3 className='font-semibold mb-4 text-[#b8d282]'>
								{t('pages.landing.footer.product')}
							</h3>
							<ul className='space-y-2 text-sm text-[#b8d282]'>
								<li>
									<a href='#features' className='hover:text-white'>
										{t('pages.landing.footer.features')}
									</a>
								</li>
								<li>
									<a href='#benefits' className='hover:text-white'>
										{t('pages.landing.footer.benefits')}
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										{t('pages.landing.footer.pricing')}
									</a>
								</li>
							</ul>
						</div>
						<div>
							<h3 className='font-semibold mb-4 text-[#b8d282]'>
								{t('pages.landing.footer.resources')}
							</h3>
							<ul className='space-y-2 text-sm text-[#b8d282]'>
								<li>
									<a href='#' className='hover:text-white'>
										{t('pages.landing.footer.documentation')}
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										{t('pages.landing.footer.blog')}
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										{t('pages.landing.footer.support')}
									</a>
								</li>
							</ul>
						</div>
						<div>
							<h3 className='font-semibold mb-4 text-[#b8d282]'>
								{t('pages.landing.footer.company')}
							</h3>
							<ul className='space-y-2 text-sm text-[#b8d282]'>
								<li>
									<a href='#about' className='hover:text-white'>
										{t('pages.landing.footer.about')}
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										{t('pages.landing.footer.contact')}
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										{t('pages.landing.footer.careers')}
									</a>
								</li>
							</ul>
						</div>
						<div>
							<h3 className='font-semibold mb-4 text-[#b8d282]'>
								{t('pages.landing.footer.legal')}
							</h3>
							<ul className='space-y-2 text-sm text-[#b8d282]'>
								<li>
									<a href='#' className='hover:text-white'>
										{t('pages.landing.footer.privacy')}
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										{t('pages.landing.footer.terms')}
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										{t('pages.landing.footer.cookies')}
									</a>
								</li>
							</ul>
						</div>
					</div>

					<div className='border-t border-[#4d8f6d] pt-8 flex flex-col md:flex-row justify-between items-center text-sm text-[#b8d282]'>
						<p>{t('pages.landing.footer.copyright')}</p>
						<div className='flex space-x-4 mt-4 md:mt-0'>
							<a href='#' className='hover:text-white'>
								{t('pages.landing.footer.twitter')}
							</a>
							<a href='#' className='hover:text-white'>
								{t('pages.landing.footer.facebook')}
							</a>
							<a href='#' className='hover:text-white'>
								{t('pages.landing.footer.linkedin')}
							</a>
						</div>
					</div>
				</div>
			</footer>
		</div>
	)
}

export default LandingPage
