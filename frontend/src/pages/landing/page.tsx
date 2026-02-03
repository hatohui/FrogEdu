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

const LandingPage = (): React.ReactElement => {
	return (
		<div className='min-h-screen'>
			{/* Hero Section - Light with gradient */}
			<section className='pt-32 pb-20 px-4 md:px-0 bg-gradient-to-br from-background via-background to-[#b8d282]/5'>
				<div className='container max-w-4xl mx-auto text-center space-y-8'>
					<div className='space-y-4'>
						<div className='flex justify-center gap-2 flex-wrap'>
							<Badge variant='secondary'>
								<Sparkles className='w-3 h-3 mr-1' />
								AI-Powered
							</Badge>
							<Badge variant='secondary'>
								<Heart className='w-3 h-3 mr-1' />
								Vietnamese Curriculum
							</Badge>
						</div>
						<h1 className='text-5xl md:text-6xl font-bold tracking-tight'>
							Transform Education with{' '}
							<span className='text-primary'>AI-Powered Learning</span>
						</h1>
						<p className='text-xl text-muted-foreground max-w-2xl mx-auto'>
							FrogEdu combines intelligent exam generation, personalized
							tutoring, and comprehensive digital textbooks to revolutionize
							Vietnamese primary education.
						</p>
					</div>

					<div className='flex flex-col sm:flex-row gap-4 justify-center pt-6'>
						<Link to='/register'>
							<Button size='lg' className='w-full sm:w-auto'>
								Start Teaching Today
							</Button>
						</Link>
						<Link to='/login'>
							<Button variant='outline' size='lg' className='w-full sm:w-auto'>
								I Already Have an Account
							</Button>
						</Link>
					</div>

					{/* Stats */}
					<div className='grid grid-cols-3 gap-4 pt-12 max-w-2xl mx-auto'>
						<div className='space-y-2'>
							<p className='text-3xl font-bold text-primary'>5</p>
							<p className='text-sm text-muted-foreground'>Grade Levels</p>
						</div>
						<div className='space-y-2'>
							<p className='text-3xl font-bold text-primary'>1000+</p>
							<p className='text-sm text-muted-foreground'>Questions</p>
						</div>
						<div className='space-y-2'>
							<p className='text-3xl font-bold text-primary'>AI</p>
							<p className='text-sm text-muted-foreground'>Powered</p>
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
						<h2 className='text-4xl font-bold'>Powerful Features</h2>
						<p className='text-lg text-[#b8d282]'>
							Everything you need to create, teach, and inspire
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
												<p>Balanced exam generation based on difficulty</p>
											</TooltipContent>
										</Tooltip>
									</TooltipProvider>
								</div>
								<CardTitle>Smart Exam Generator</CardTitle>
								<CardDescription className='text-[#b8d282]'>
									Define exam matrices and generate balanced assessments
								</CardDescription>
							</CardHeader>
							<CardContent className='space-y-2'>
								<p className='text-sm'>✓ Create exams by difficulty level</p>
								<p className='text-sm'>✓ Automatic question selection</p>
								<p className='text-sm'>✓ PDF export to S3</p>
								<p className='text-sm'>✓ Manual override options</p>
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
												<p>Socratic method with intelligent responses</p>
											</TooltipContent>
										</Tooltip>
									</TooltipProvider>
								</div>
								<CardTitle>AI Student Tutor</CardTitle>
								<CardDescription className='text-[#b8d282]'>
									Socratic dialogue with intelligent guidance
								</CardDescription>
							</CardHeader>
							<CardContent className='space-y-2'>
								<p className='text-sm'>✓ Real-time chat responses</p>
								<p className='text-sm'>✓ Socratic questioning method</p>
								<p className='text-sm'>✓ Textbook references</p>
								<p className='text-sm'>✓ Conversation history tracking</p>
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
												<p>Grade 1-5 Vietnamese textbooks</p>
											</TooltipContent>
										</Tooltip>
									</TooltipProvider>
								</div>
								<CardTitle>Digital Textbook Library</CardTitle>
								<CardDescription className='text-[#b8d282]'>
									Comprehensive Vietnamese curriculum content
								</CardDescription>
							</CardHeader>
							<CardContent className='space-y-2'>
								<p className='text-sm'>✓ Grade 1-5 textbooks</p>
								<p className='text-sm'>✓ Chapter-based navigation</p>
								<p className='text-sm'>✓ Supplementary assets</p>
								<p className='text-sm'>✓ Full-text search</p>
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
												<p>Manage students and track progress</p>
											</TooltipContent>
										</Tooltip>
									</TooltipProvider>
								</div>
								<CardTitle>Class Management</CardTitle>
								<CardDescription className='text-[#b8d282]'>
									Organize students and track progress
								</CardDescription>
							</CardHeader>
							<CardContent className='space-y-2'>
								<p className='text-sm'>✓ Create and manage classes</p>
								<p className='text-sm'>✓ Student enrollment codes</p>
								<p className='text-sm'>✓ Progress tracking</p>
								<p className='text-sm'>✓ Performance analytics</p>
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
						<h2 className='text-4xl font-bold'>Why Choose FrogEdu?</h2>
						<p className='text-lg text-muted-foreground'>
							Designed by educators, powered by AI
						</p>
					</div>

					<div className='space-y-4'>
						{[
							{
								title: 'Save Time',
								description:
									'Generate comprehensive exams in minutes, not hours',
							},
							{
								title: 'Improve Learning',
								description:
									'AI tutor provides personalized guidance using Socratic method',
							},
							{
								title: 'Better Assessment',
								description:
									'Balanced exams with automatic question selection based on difficulty',
							},
							{
								title: 'Student Engagement',
								description:
									'Interactive learning platform keeps students motivated and engaged',
							},
							{
								title: 'Data-Driven Decisions',
								description:
									'Track student progress and identify learning gaps',
							},
							{
								title: 'Vietnamese Curriculum',
								description:
									'Content designed for Vietnamese primary education standards',
							},
						].map((benefit, idx) => (
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
						Built for Vietnamese Education
					</h2>
					<p className='text-lg text-[#e8f3d8] max-w-2xl mx-auto'>
						FrogEdu is specifically designed for Vietnamese primary schools
						(Grades 1-5), combining cutting-edge AI technology with proven
						educational methods to create an intuitive platform that teachers
						and students love.
					</p>

					<div className='grid md:grid-cols-3 gap-8 pt-8'>
						<div className='bg-[#286147]/40 rounded-lg p-6 backdrop-blur-sm'>
							<p className='text-3xl font-bold text-white mb-2'>100%</p>
							<p className='text-[#e8f3d8]'>Vietnamese Curriculum</p>
						</div>
						<div className='bg-[#286147]/40 rounded-lg p-6 backdrop-blur-sm'>
							<p className='text-3xl font-bold text-white mb-2'>24/7</p>
							<p className='text-[#e8f3d8]'>AI Support Available</p>
						</div>
						<div className='bg-[#286147]/40 rounded-lg p-6 backdrop-blur-sm'>
							<p className='text-3xl font-bold text-white mb-2'>∞</p>
							<p className='text-[#e8f3d8]'>Scalable Platform</p>
						</div>
					</div>
				</div>
			</section>

			{/* CTA Section */}
			<section className='py-20 px-4 md:px-0'>
				<div className='container max-w-3xl mx-auto text-center space-y-8'>
					<h2 className='text-4xl font-bold'>
						Ready to Transform Your Classroom?
					</h2>
					<p className='text-lg text-muted-foreground'>
						Join teachers across Vietnam using FrogEdu to create smarter
						assessments and empower student learning
					</p>

					<div className='flex flex-col sm:flex-row gap-4 justify-center'>
						<Link to='/register'>
							<Button size='lg' className='w-full sm:w-auto'>
								Create Free Account
							</Button>
						</Link>
						<Link to='/login'>
							<Button variant='outline' size='lg' className='w-full sm:w-auto'>
								Sign In
							</Button>
						</Link>
					</div>
				</div>
			</section>

			{/* FAQ Section */}
			<section className='py-20 px-4 md:px-0 bg-[#286147] text-white'>
				<div className='container max-w-3xl mx-auto'>
					<div className='text-center space-y-4 mb-12'>
						<h2 className='text-4xl font-bold'>Frequently Asked Questions</h2>
						<p className='text-lg text-muted-foreground'>
							Get answers to common questions about FrogEdu
						</p>
					</div>

					<Accordion type='single' collapsible className='w-full'>
						<AccordionItem value='item-1'>
							<AccordionTrigger>
								How does the Smart Exam Generator work?
							</AccordionTrigger>
							<AccordionContent>
								The Smart Exam Generator uses AI to analyze your exam matrix
								(difficulty distribution and topic coverage) and automatically
								selects questions from our extensive question bank. You can
								always manually override selections if needed.
							</AccordionContent>
						</AccordionItem>
						<AccordionItem value='item-2'>
							<AccordionTrigger>
								Is FrogEdu suitable for all grade levels?
							</AccordionTrigger>
							<AccordionContent>
								Yes! FrogEdu covers Vietnamese primary education grades 1-5.
								Each grade has age-appropriate content and difficulty levels
								tailored to curriculum standards.
							</AccordionContent>
						</AccordionItem>
						<AccordionItem value='item-3'>
							<AccordionTrigger>
								Can I export exams in different formats?
							</AccordionTrigger>
							<AccordionContent>
								Currently, we support PDF and DOCX exports. All files are stored
								securely in AWS S3 and delivered via presigned URLs for safe
								access.
							</AccordionContent>
						</AccordionItem>
						<AccordionItem value='item-4'>
							<AccordionTrigger>
								How does the AI Tutor help students?
							</AccordionTrigger>
							<AccordionContent>
								The AI Tutor uses a Socratic method to guide students through
								problems without giving direct answers. It references relevant
								textbook content and maintains conversation history for teachers
								to review.
							</AccordionContent>
						</AccordionItem>
						<AccordionItem value='item-5'>
							<AccordionTrigger>
								Is there a free trial available?
							</AccordionTrigger>
							<AccordionContent>
								Yes! We offer a free trial so you can explore all features. No
								credit card required to get started. Sign up today to begin
								transforming your classroom.
							</AccordionContent>
						</AccordionItem>
					</Accordion>
				</div>
			</section>

			{/* Newsletter Section */}
			<section className='py-20 px-4 md:px-0 bg-[#8db376]'>
				<div className='container max-w-2xl mx-auto text-center space-y-8'>
					<div className='space-y-4'>
						<h2 className='text-4xl font-bold text-white'>Stay Updated</h2>
						<p className='text-lg text-[#e8f3d8]'>
							Subscribe to our newsletter for tips, updates, and exclusive
							educator resources
						</p>
					</div>

					<div className='flex gap-2 max-w-md mx-auto'>
						<Input
							type='email'
							placeholder='Enter your email'
							className='flex-1 bg-[#4d8f6d] border-[#b8d282] text-white placeholder:text-[#b8d282]'
						/>
						<Button className='bg-white text-[#286147] hover:bg-[#e8f3d8]'>
							Subscribe
						</Button>
					</div>
				</div>
			</section>

			{/* Footer */}
			<footer className='border-t py-12 px-4 bg-[#033a1e] text-white'>
				<div className='container max-w-6xl mx-auto px-4'>
					<div className='grid md:grid-cols-4 gap-8 mb-8'>
						<div>
							<h3 className='font-semibold mb-4 text-[#b8d282]'>Product</h3>
							<ul className='space-y-2 text-sm text-[#b8d282]'>
								<li>
									<a href='#features' className='hover:text-white'>
										Features
									</a>
								</li>
								<li>
									<a href='#benefits' className='hover:text-white'>
										Benefits
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										Pricing
									</a>
								</li>
							</ul>
						</div>
						<div>
							<h3 className='font-semibold mb-4 text-[#b8d282]'>Resources</h3>
							<ul className='space-y-2 text-sm text-[#b8d282]'>
								<li>
									<a href='#' className='hover:text-white'>
										Documentation
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										Blog
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										Support
									</a>
								</li>
							</ul>
						</div>
						<div>
							<h3 className='font-semibold mb-4 text-[#b8d282]'>Company</h3>
							<ul className='space-y-2 text-sm text-[#b8d282]'>
								<li>
									<a href='#about' className='hover:text-white'>
										About
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										Contact
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										Careers
									</a>
								</li>
							</ul>
						</div>
						<div>
							<h3 className='font-semibold mb-4 text-[#b8d282]'>Legal</h3>
							<ul className='space-y-2 text-sm text-[#b8d282]'>
								<li>
									<a href='#' className='hover:text-white'>
										Privacy
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										Terms
									</a>
								</li>
								<li>
									<a href='#' className='hover:text-white'>
										Cookies
									</a>
								</li>
							</ul>
						</div>
					</div>

					<div className='border-t border-[#4d8f6d] pt-8 flex flex-col md:flex-row justify-between items-center text-sm text-[#b8d282]'>
						<p>&copy; 2026 FrogEdu. All rights reserved.</p>
						<div className='flex space-x-4 mt-4 md:mt-0'>
							<a href='#' className='hover:text-white'>
								Twitter
							</a>
							<a href='#' className='hover:text-white'>
								Facebook
							</a>
							<a href='#' className='hover:text-white'>
								LinkedIn
							</a>
						</div>
					</div>
				</div>
			</footer>
		</div>
	)
}

export default LandingPage
