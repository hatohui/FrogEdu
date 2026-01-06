import gsap from 'gsap'
import { SplitText } from 'gsap/SplitText'
import { useGSAP } from '@gsap/react'
import { Physics2DPlugin } from 'gsap/all'

const registerGSAPPlugins = () => {
	gsap.registerPlugin(SplitText)
	gsap.registerPlugin(useGSAP)
	gsap.registerPlugin(Physics2DPlugin)
}

export default registerGSAPPlugins
