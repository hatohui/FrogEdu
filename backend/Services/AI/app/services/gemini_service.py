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
from app.models.enums import QuestionType
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
        self.model_name = "gemini-2.5-flash"
        logger.info(f"Initialized GeminiService with model: {self.model_name}")
            
    def _create_question_schema(self, question_type: Optional[QuestionType] = None) -> dict[str, Any]:
        """
        Create JSON schema for question generation.
        Schema adapts based on question type for better AI output.
        """
        # Base answer schema
        answer_schema = {
            "type": "OBJECT",
            "properties": {
                "content": {"type": "STRING"},
                "is_correct": {"type": "BOOLEAN"},
                "explanation": {"type": "STRING"},
                "point": {"type": "NUMBER"},
            },
            "required": ["content", "is_correct"]
        }
        
        # Question type enum values
        question_type_enum = ["select", "multiple_choice", "true_false", "essay", "fill_in_blank"]
        
        return {
            "type": "OBJECT",
            "properties": {
                "questions": {
                    "type": "ARRAY",
                    "items": {
                        "type": "OBJECT",
                        "properties": {
                            "content": {"type": "STRING"},
                            "question_type": {
                                "type": "STRING",
                                "enum": question_type_enum
                            },
                            "cognitive_level": {
                                "type": "STRING",
                                "enum": ["remember", "understand", "apply", "analyze"]
                            },
                            "point": {"type": "NUMBER"},
                            "media_url": {"type": "STRING"},
                            "answers": {
                                "type": "ARRAY",
                                "items": answer_schema
                            }
                        },
                        "required": ["content", "question_type", "cognitive_level", "answers"]
                    }
                }
            },
            "required": ["questions"]
        }
    
    def _validate_question(self, question_data: dict, expected_type: Optional[QuestionType] = None) -> dict:
        """
        Validate and fix question data based on question type rules.
        """
        question_type_str = question_data.get("question_type", "select")
        answers = question_data.get("answers", [])
        
        # Ensure question_type is set correctly
        if expected_type:
            question_data["question_type"] = expected_type.value
            question_type_str = expected_type.value
        
        # Validate based on question type
        if question_type_str == "true_false":
            # True/False must have exactly 2 answers: True and False
            if len(answers) != 2:
                question_data["answers"] = [
                    {"content": "True", "is_correct": answers[0].get("is_correct", False) if answers else False, "explanation": answers[0].get("explanation", "") if answers else ""},
                    {"content": "False", "is_correct": not (answers[0].get("is_correct", False) if answers else False), "explanation": answers[1].get("explanation", "") if len(answers) > 1 else ""}
                ]
            else:
                # Ensure content is exactly "True" and "False"
                question_data["answers"][0]["content"] = "True"
                question_data["answers"][1]["content"] = "False"
        
        elif question_type_str == "select":
            # Select must have exactly one correct answer
            correct_count = sum(1 for a in answers if a.get("is_correct", False))
            if correct_count != 1 and answers:
                # Set only the first correct one as correct
                found_correct = False
                for answer in question_data["answers"]:
                    if answer.get("is_correct", False) and not found_correct:
                        found_correct = True
                    else:
                        answer["is_correct"] = False
                if not found_correct and question_data["answers"]:
                    question_data["answers"][0]["is_correct"] = True
        
        elif question_type_str == "essay":
            # Essay should have 0 or 1 answer (grading rubric)
            if len(answers) > 1:
                question_data["answers"] = [answers[0]]
            if question_data["answers"]:
                question_data["answers"][0]["is_correct"] = True
        
        elif question_type_str == "fill_in_blank":
            # Fill in blank - all answers are correct (acceptable answers)
            for answer in question_data.get("answers", []):
                answer["is_correct"] = True
        
        return question_data
    
    async def generate_questions(
        self, 
        request: GenerateQuestionsRequest
    ) -> list[Question]:
        """Generate questions based on exam matrix.
        
        Generates questions for each matrix topic configuration and associates
        the topic_id with each generated question.
        """
        try:
            all_validated_questions: list[Question] = []
            
            # Process each matrix topic separately to maintain topic_id association
            for topic_config in request.matrix_topics:
                # Build a single-topic request for this batch
                single_topic_request = GenerateQuestionsRequest(
                    subject=request.subject,
                    grade=request.grade,
                    matrix_topics=[topic_config],
                    language=request.language
                )
                
                # Build the prompt for this topic
                prompt = build_matrix_prompt(single_topic_request)
                
                # Configure with JSON schema - constrain to specific type if provided
                config = types.GenerateContentConfig(
                    response_mime_type="application/json",
                    response_schema=self._create_question_schema(topic_config.question_type),
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
                    logger.warning(f"No response for topic {topic_config.topic_name}, skipping")
                    continue
                    
                result = json.loads(response.text)
                
                # Validate and fix each question, attaching the topic_id
                for q in result.get("questions", []):
                    validated_q = self._validate_question(q, topic_config.question_type)
                    # Attach the topic_id from the matrix configuration
                    validated_q["topic_id"] = topic_config.topic_id
                    all_validated_questions.append(Question(**validated_q))
                
                logger.info(f"Generated {len(result.get('questions', []))} questions for topic {topic_config.topic_name}")
            
            logger.info(f"Generated {len(all_validated_questions)} total questions successfully")
            return all_validated_questions
            
        except Exception as e:
            logger.error(f"Error generating questions: {str(e)}")
            raise
    
    async def generate_single_question(
        self,
        request: GenerateSingleQuestionRequest
    ) -> Question:
        """Generate a single question with type-specific validation.
        
        If topic_id is provided in the request, it will be associated with
        the generated question.
        """
        try:
            prompt = build_single_question_prompt(request)
            
            config = types.GenerateContentConfig(
                response_mime_type="application/json",
                response_schema=self._create_question_schema(request.question_type),
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
            
            # Validate and fix the question based on expected type
            validated_q = self._validate_question(questions[0], request.question_type)
            
            # Attach the topic_id if provided in the request
            if request.topic_id:
                validated_q["topic_id"] = request.topic_id
            
            question = Question(**validated_q)
            logger.info(f"Generated single {request.question_type.value} question successfully")
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
