import type { MatrixTopicDto } from '@/types/model/exam-service'

export interface CreateMatrixRequest {
	name: string
	description?: string
	subjectId: string
	grade: number
	matrixTopics: MatrixTopicDto[]
}

export interface UpdateMatrixRequest {
	name?: string
	description?: string
	matrixTopics: MatrixTopicDto[]
}

export interface AttachMatrixRequest {
	matrixId: string
}
