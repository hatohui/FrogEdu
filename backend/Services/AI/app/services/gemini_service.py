from google import genai
from google.genai import types
from typing import Optional, Any
import json
import logging

from app.config import Settings
from app.schemas import (
    Question,
    GenerateQuestionsRequest,
    GenerateSingleQuestionRequest,
)

logger = logging.getLogger(__name__)


class GeminiService:
    """Service for interacting with Google Gemini API."""
    
    def __init__(self, settings: Settings):
        """Initialize Gemini client with settings."""
        self.settings = settings
        self.client = genai.Client(api_key=settings.gemini_api_key)
        self.model_name = "gemini-3-flash-preview"
        logger.info(f"Initialized GeminiService with model: {self.model_name}")
    
    def _create_question_schema(self) -> dict[str, Any]:
        """Create JSON schema for question generation."""
        return {
            "type": "OBJECT",
            "properties": {
                "questions": {
                    "type": "ARRAY",
                    "items": {
                        "type": "OBJECT",
                        "properties": {
                            "content": {"type": "STRING"},
                            "question_type": {"type": "STRING"},
                            "cognitive_level": {"type": "STRING"},
                            "point": {"type": "NUMBER"},
                            "media_url": {"type": "STRING"},
                            "answers": {
                                "type": "ARRAY",
                                "items": {
                                    "type": "OBJECT",
                                    "properties": {
                                        "content": {"type": "STRING"},
                                        "is_correct": {"type": "BOOLEAN"},
                                        "explanation": {"type": "STRING"},
                                        "point": {"type": "NUMBER"},
                                    },
                                    "required": ["content", "is_correct"]
                                }
                            }
                        },
                        "required": ["content", "question_type", "cognitive_level", "answers"]
                    }
                }
            },
            "required": ["questions"]
        }
    
    async def generate_questions(
        self, 
        request: GenerateQuestionsRequest
    ) -> list[Question]:
        """Generate questions based on exam matrix."""
        try:
            # Build the prompt
            prompt = self._build_matrix_prompt(request)
            
            # Configure with JSON schema
            config = types.GenerateContentConfig(
                response_mime_type="application/json",
                response_schema=self._create_question_schema(),
                temperature=0.7,
            )
            
            # Generate content
            response = self.client.models.generate_content(
                model=self.model_name,
                contents=prompt,
                config=config,
            )
            
            # Parse response
            if response.text is None:
                raise ValueError("No response text received from Gemini API")
            result = json.loads(response.text)
            questions = [Question(**q) for q in result.get("questions", [])]
            
            logger.info(f"Generated {len(questions)} questions successfully")
            return questions
            
        except Exception as e:
            logger.error(f"Error generating questions: {str(e)}")
            raise
    
    async def generate_single_question(
        self,
        request: GenerateSingleQuestionRequest
    ) -> Question:
        """Generate a single question."""
        try:
            prompt = self._build_single_question_prompt(request)
            
            config = types.GenerateContentConfig(
                response_mime_type="application/json",
                response_schema=self._create_question_schema(),
                temperature=0.7,
            )
            
            response = self.client.models.generate_content(
                model=self.model_name,
                contents=prompt,
                config=config,
            )
            
            if response.text is None:
                raise ValueError("No response text received from Gemini API")
            result = json.loads(response.text)
            questions = result.get("questions", [])
            
            if not questions:
                raise ValueError("No question generated")
            
            question = Question(**questions[0])
            logger.info("Generated single question successfully")
            return question
            
        except Exception as e:
            logger.error(f"Error generating single question: {str(e)}")
            raise
    
    async def tutor_chat(
        self,
        message: str,
        subject: str,
        grade: int,
        topic: Optional[str] = None,
        conversation_history: Optional[list[dict[str, Any]]] = None
    ) -> str:
        """Conduct tutoring conversation with student."""
        try:
            # Create system instruction for tutoring
            system_instruction = self._build_tutor_system_instruction(
                subject=subject,
                grade=grade,
                topic=topic
            )
            
            config = types.GenerateContentConfig(
                system_instruction=system_instruction,
                temperature=0.8,
            )
            
            # For now, use simple generation (can be extended to chat sessions)
            prompt = f"Student asks: {message}"
            
            response = self.client.models.generate_content(
                model=self.model_name,
                contents=prompt,
                config=config,
            )
            
            if response.text is None:
                raise ValueError("No response text received from Gemini API")
            
            logger.info("Tutor response generated successfully")
            return response.text
            
        except Exception as e:
            logger.error(f"Error in tutor chat: {str(e)}")
            raise
    
    def _build_matrix_prompt(self, request: GenerateQuestionsRequest) -> str:
        """Build prompt for matrix-based question generation."""
        matrix_details = "\n".join([
            f"- Topic: {topic.topic_name}, "
            f"Cognitive Level: {topic.cognitive_level.value}, "
            f"Quantity: {topic.quantity}"
            for topic in request.matrix_topics
        ])
        
        language_instruction = (
            "Generate all content in Vietnamese."
            if request.language == "vi"
            else "Generate all content in English."
        )
        
        return f"""You are an expert education content creator for Vietnamese curriculum.

Generate exam questions with the following specifications:

**Subject**: {request.subject}
**Grade Level**: {request.grade}
**Language**: {request.language}

**Matrix Requirements**:
{matrix_details}

{language_instruction}

For each question:
1. Content must be appropriate for grade {request.grade} students
2. Follow Vietnamese curriculum standards (if applicable)
3. Match the specified cognitive level (Bloom's Taxonomy)
4. Include 4 answer options for multiple choice questions
5. Mark the correct answer
6. Provide clear explanations for answers

Return the questions in the specified JSON format."""
    
    def _build_single_question_prompt(
        self, 
        request: GenerateSingleQuestionRequest
    ) -> str:
        """Build prompt for single question generation."""
        language_instruction = (
            "Generate all content in Vietnamese."
            if request.language == "vi"
            else "Generate all content in English."
        )
        
        return f"""You are an expert education content creator.

Generate ONE exam question with these specifications:

**Subject**: {request.subject}
**Grade Level**: {request.grade}
**Topic**: {request.topic_name}
**Cognitive Level**: {request.cognitive_level.value}
**Question Type**: {request.question_type.value}

{language_instruction}

Requirements:
1. Content appropriate for grade {request.grade}
2. Follow the specified cognitive level
3. Include 4 options for multiple choice
4. Mark correct answer(s)
5. Provide explanations

Return in JSON format with a "questions" array containing one question."""
    
    def _build_tutor_system_instruction(
        self,
        subject: str,
        grade: int,
        topic: Optional[str] = None
    ) -> str:
        """Build system instruction for tutoring mode."""
        topic_context = f" focusing on {topic}" if topic else ""
        
        return f"""You are a friendly and patient Vietnamese teacher specializing in {subject} 
for grade {grade} students{topic_context}.

Your role:
- Explain concepts in a simple, age-appropriate manner
- Use Vietnamese language naturally
- Encourage critical thinking through guided questions
- Provide step-by-step explanations
- Use examples relevant to Vietnamese students
- Be encouraging and supportive
- Never give direct answers to homework - guide students to discover solutions

Teaching style:
- Start with what the student knows
- Break down complex topics
- Check understanding frequently
- Use analogies and real-world examples"""
    
    def health_check(self) -> bool:
        """Check if Gemini API is accessible."""
        try:
            response = self.client.models.generate_content(
                model=self.model_name,
                contents="Say 'OK' if you can read this.",
            )
            return bool(response.text)
        except Exception as e:
            logger.error(f"Health check failed: {str(e)}")
            return False
