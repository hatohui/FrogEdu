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
from app.services.prompts import (
    build_matrix_prompt,
    build_single_question_prompt,
    build_tutor_system_instruction,
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
            prompt = build_matrix_prompt(request)
            
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
            prompt = build_single_question_prompt(request)
            
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
            system_instruction = build_tutor_system_instruction(
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
