from pydantic import BaseModel, Field
from typing import Optional, Any
from app.models.enums import CognitiveLevel, QuestionType


class Answer(BaseModel):
    """Schema for an answer to a question."""
    content: str = Field(..., description="The answer text")
    is_correct: bool = Field(..., description="Whether this is the correct answer")
    explanation: Optional[str] = Field(None, description="Explanation for the answer")
    point: float = Field(default=1.0, ge=0, description="Points awarded for this answer")


class Question(BaseModel):
    """Schema for a generated question."""
    content: str = Field(..., description="The question text")
    question_type: QuestionType = Field(..., description="Type of question")
    cognitive_level: CognitiveLevel = Field(..., description="Bloom's taxonomy level")
    point: float = Field(default=1.0, ge=0, description="Total points for the question")
    media_url: Optional[str] = Field(None, description="URL to associated media")
    answers: list[Answer] = Field(..., min_length=1, description="List of possible answers")
    topic_id: Optional[str] = Field(None, description="Associated topic ID")


class MatrixTopicConfig(BaseModel):
    """Configuration for a topic in the exam matrix."""
    topic_id: str = Field(..., description="ID of the topic")
    topic_name: str = Field(..., description="Name of the topic")
    cognitive_level: CognitiveLevel = Field(..., description="Cognitive level")
    quantity: int = Field(..., gt=0, description="Number of questions to generate")
    question_type: Optional[QuestionType] = Field(
        None,
        description="Specific question type to generate. If null/not provided, question type is randomized per question."
    )


class GenerateQuestionsRequest(BaseModel):
    """Request to generate questions based on a matrix."""
    subject: str = Field(..., description="Subject name (e.g., 'Mathematics', 'Physics')")
    grade: int = Field(..., ge=1, le=12, description="Grade level (1-12)")
    matrix_topics: list[MatrixTopicConfig] = Field(
        ..., 
        min_length=1,
        description="Matrix configuration defining topics and quantities"
    )
    language: str = Field(default="vi", description="Language for generated content (vi/en)")


class GenerateSingleQuestionRequest(BaseModel):
    """Request to generate a single question."""
    subject: str = Field(..., description="Subject name")
    grade: int = Field(..., ge=1, le=12, description="Grade level")
    topic_name: str = Field(..., description="Topic name")
    topic_id: Optional[str] = Field(None, description="Topic ID to associate with the question")
    cognitive_level: CognitiveLevel = Field(..., description="Cognitive level")
    question_type: QuestionType = Field(..., description="Type of question")
    language: str = Field(default="vi", description="Language (vi/en)")
    topic_description:str = Field(
        default="", 
        description="Brief description of the topic for context"
    )


class GenerateQuestionsResponse(BaseModel):
    """Response containing generated questions."""
    questions: list[Question] = Field(..., description="List of generated questions")
    total_count: int = Field(..., description="Total number of questions generated")


class TutorChatRequest(BaseModel):
    """Request for tutoring conversation."""
    message: str = Field(..., description="Student's question or message")
    subject: str = Field(..., description="Subject context")
    grade: int = Field(..., ge=1, le=12, description="Student's grade level")
    topic: Optional[str] = Field(None, description="Specific topic if applicable")
    conversation_history: Optional[list[dict[str, Any]]] = Field(
        None, 
        description="Previous conversation messages"
    )


class TutorChatResponse(BaseModel):
    """Response from tutoring conversation."""
    message: str = Field(..., description="Tutor's response")
    suggestions: Optional[list[str]] = Field(None, description="Follow-up suggestions")


class HealthResponse(BaseModel):
    """Health check response."""
    status: str = Field(..., description="Service status")
    service_name: str = Field(..., description="Name of the service")
    gemini_connected: bool = Field(..., description="Whether Gemini API is accessible")
