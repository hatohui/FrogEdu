import React from 'react'

const MainLayout = ({
	children,
}: {
	children: React.ReactNode
}): React.ReactElement => {
	return <div>{children}</div>
}

export default MainLayout
