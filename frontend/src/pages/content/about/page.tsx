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

const AboutPage = (): React.ReactElement => {
	return (
		<div className='min-h-screen bg-background'>
			{/* Hero Section */}
			<section className='pt-20 pb-16 px-4 md:px-0 bg-gradient-to-br from-background via-background to-[#b8d282]/5'>
				<div className='container max-w-4xl mx-auto text-center space-y-6'>
					<div className='flex justify-center mb-6'>
						<img
							src='/frog.png'
							alt='FrogEdu Logo'
							className='w-24 h-24 drop-shadow-lg'
						/>
					</div>
					<div className='flex justify-center gap-2 flex-wrap'>
						<Badge variant='secondary'>
							<Sparkles className='w-3 h-3 mr-1' />
							AI-Powered
						</Badge>
						<Badge variant='secondary'>
							<Heart className='w-3 h-3 mr-1' />
							Made in Vietnam
						</Badge>
					</div>
					<h1 className='text-4xl md:text-5xl font-bold tracking-tight'>
						About <span className='text-primary'>FrogEdu</span>
					</h1>
					<p className='text-lg text-muted-foreground max-w-2xl mx-auto'>
						FrogEdu is an innovative AI-powered educational platform designed
						specifically for Vietnamese primary education, combining
						cutting-edge technology with pedagogical excellence.
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
								<h2 className='text-3xl font-bold'>Our Mission</h2>
							</div>
							<p className='text-muted-foreground leading-relaxed'>
								We believe every child deserves access to quality education. Our
								mission is to democratize learning by providing teachers and
								students with powerful AI tools that make education more
								engaging, personalized, and effective.
							</p>
							<p className='text-muted-foreground leading-relaxed'>
								By focusing on the Vietnamese primary education curriculum
								(Grades 1-5), we ensure our platform aligns perfectly with
								national educational standards while incorporating innovative
								teaching methodologies.
							</p>
						</div>
						<Card className='bg-gradient-to-br from-[#286147]/5 to-[#b8d282]/10'>
							<CardContent className='p-8 space-y-4'>
								<div className='grid grid-cols-2 gap-4 text-center'>
									<div className='space-y-2'>
										<p className='text-3xl font-bold text-primary'>5</p>
										<p className='text-sm text-muted-foreground'>
											Grade Levels
										</p>
									</div>
									<div className='space-y-2'>
										<p className='text-3xl font-bold text-primary'>1000+</p>
										<p className='text-sm text-muted-foreground'>Questions</p>
									</div>
									<div className='space-y-2'>
										<p className='text-3xl font-bold text-primary'>AI</p>
										<p className='text-sm text-muted-foreground'>Powered</p>
									</div>
									<div className='space-y-2'>
										<p className='text-3xl font-bold text-primary'>24/7</p>
										<p className='text-sm text-muted-foreground'>Available</p>
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
						<h2 className='text-3xl font-bold mb-4'>What We Offer</h2>
						<p className='text-white/80 max-w-2xl mx-auto'>
							Our platform provides comprehensive tools for both teachers and
							students
						</p>
					</div>
					<div className='grid md:grid-cols-3 gap-6'>
						<Card className='bg-white/10 border-white/20 text-white'>
							<CardHeader>
								<div className='w-12 h-12 rounded-full bg-white/20 flex items-center justify-center mb-2'>
									<Brain className='w-6 h-6' />
								</div>
								<CardTitle className='text-white'>AI Exam Generation</CardTitle>
							</CardHeader>
							<CardContent>
								<CardDescription className='text-white/80'>
									Generate customized exams aligned with Vietnamese curriculum
									using advanced AI. Create multiple choice, fill-in-the-blank,
									and essay questions instantly.
								</CardDescription>
							</CardContent>
						</Card>

						<Card className='bg-white/10 border-white/20 text-white'>
							<CardHeader>
								<div className='w-12 h-12 rounded-full bg-white/20 flex items-center justify-center mb-2'>
									<GraduationCap className='w-6 h-6' />
								</div>
								<CardTitle className='text-white'>Class Management</CardTitle>
							</CardHeader>
							<CardContent>
								<CardDescription className='text-white/80'>
									Create and manage classes, assign homework, track student
									progress, and communicate with students all in one place.
								</CardDescription>
							</CardContent>
						</Card>

						<Card className='bg-white/10 border-white/20 text-white'>
							<CardHeader>
								<div className='w-12 h-12 rounded-full bg-white/20 flex items-center justify-center mb-2'>
									<BookOpen className='w-6 h-6' />
								</div>
								<CardTitle className='text-white'>Digital Textbooks</CardTitle>
							</CardHeader>
							<CardContent>
								<CardDescription className='text-white/80'>
									Access comprehensive digital textbooks for Vietnamese primary
									education with interactive content and learning materials.
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
						<h2 className='text-3xl font-bold mb-4'>Our Team</h2>
						<p className='text-muted-foreground max-w-2xl mx-auto'>
							FrogEdu is built by a passionate team of educators, engineers, and
							designers who believe in the power of technology to transform
							education.
						</p>
					</div>
					<div className='grid md:grid-cols-2 gap-6 max-w-2xl mx-auto'>
						<Card>
							<CardContent className='p-6 flex items-center gap-4'>
								<div className='w-16 h-16 rounded-full bg-primary/10 flex items-center justify-center'>
									<Users className='w-8 h-8 text-primary' />
								</div>
								<div>
									<h3 className='font-semibold'>Dedicated Team</h3>
									<p className='text-sm text-muted-foreground'>
										Educators & Engineers working together
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
									<h3 className='font-semibold'>Made in Vietnam</h3>
									<p className='text-sm text-muted-foreground'>
										Built with Vietnamese education in mind
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
							<CardTitle className='text-2xl'>Get in Touch</CardTitle>
							<CardDescription>
								Have questions or want to learn more about FrogEdu?
							</CardDescription>
						</CardHeader>
						<CardContent className='space-y-6'>
							<div className='grid md:grid-cols-2 gap-4'>
								<div className='flex items-center gap-3 p-4 rounded-lg bg-muted'>
									<Mail className='w-5 h-5 text-primary' />
									<div>
										<p className='text-sm text-muted-foreground'>Email</p>
										<p className='font-medium'>contact@frogedu.vn</p>
									</div>
								</div>
								<div className='flex items-center gap-3 p-4 rounded-lg bg-muted'>
									<MapPin className='w-5 h-5 text-primary' />
									<div>
										<p className='text-sm text-muted-foreground'>Location</p>
										<p className='font-medium'>Ho Chi Minh City, Vietnam</p>
									</div>
								</div>
							</div>
							<div className='flex flex-col sm:flex-row gap-4 justify-center pt-4'>
								<Link to='/register'>
									<Button size='lg' className='w-full sm:w-auto'>
										Start Teaching Today
									</Button>
								</Link>
								<Link to='/'>
									<Button
										variant='outline'
										size='lg'
										className='w-full sm:w-auto'
									>
										Back to Home
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
