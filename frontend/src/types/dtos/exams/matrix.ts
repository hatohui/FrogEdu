import type { MatrixTopicDto } from '@/types/model/exams'

export interface CreateMatrixRequest {
	examId: string
	matrixTopics: MatrixTopicDto[]
}
