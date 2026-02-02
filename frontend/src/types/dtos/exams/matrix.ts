import type { MatrixTopicDto } from '@/types/model/exam-service'

export interface CreateMatrixRequest {
	examId: string
	matrixTopics: MatrixTopicDto[]
}
