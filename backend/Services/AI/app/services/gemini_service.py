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
    GradeEssayRequest,
    GradeEssayResponse,
)
from app.models.enums import QuestionType
from app.services.prompts import (
    build_matrix_prompt,
    build_single_question_prompt,
    build_tutor_system_instruction,
    build_socratic_hints_prompt,
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

    async def explain_question(
        self,
        question_content: str,
        correct_answer: str,
        grade: int,
        subject: str,
        student_answer: Optional[str] = None,
        language: str = "vi",
    ) -> str:
        """Generate a child-friendly explanation for a question and its correct answer.

        Designed for primary school students (grades 1-5) who want to understand
        why their answer was wrong and what the correct answer means.
        """
        try:
            if student_answer:
                context = (
                    f"The student answered: \"{student_answer}\" — this is WRONG.\n"
                    f"Correct answer: {correct_answer}\n\n"
                    f"Explain clearly:\n"
                    f"1. Why the student's answer is incorrect.\n"
                    f"2. Why the correct answer is right, with a simple reason or example.\n"
                )
            else:
                context = (
                    f"Correct answer: {correct_answer}\n\n"
                    f"Explain why this answer is correct with a simple reason or example.\n"
                )

            prompt = (
                f"You are a {subject} teacher explaining a question to a grade {grade} primary school student.\n"
                f"Respond in {'Vietnamese' if language == 'vi' else 'English'}.\n"
                f"Be direct and educational. Do NOT compliment the student or add encouragement — "
                f"focus only on the factual explanation. Use simple words a grade {grade} student can understand. "
                f"Keep it to 2-4 sentences.\n\n"
                f"Question: {question_content}\n"
                f"{context}"
            )

            config = types.GenerateContentConfig(
                temperature=0.3,
            )

            response = self.client.models.generate_content(
                model=self.model_name,
                contents=prompt,
                config=config,
            )

            if response.text is None:
                raise ValueError("No response text received from Gemini API")

            logger.info(f"Explanation generated for grade {grade} {subject} question")
            return response.text

        except Exception as e:
            logger.error(f"Error generating explanation: {str(e)}")
            raise

    async def grade_essay(self, request: GradeEssayRequest) -> GradeEssayResponse:
        """Grade a student's essay answer using AI.

        Evaluates the student's free-text response against the grading rubric and
        returns a score (0..max_points) plus constructive feedback.
        """
        try:
            lang = "Vietnamese" if request.language == "vi" else "English"
            prompt = (
                f"You are a strict but fair {request.subject} teacher grading a "
                f"grade {request.grade} student's essay answer.\n"
                f"Respond in {lang}.\n\n"
                f"QUESTION:\n{request.question_content}\n\n"
                f"GRADING RUBRIC / EXPECTED ANSWER GUIDELINES:\n{request.grading_rubric}\n\n"
                f"STUDENT'S ANSWER:\n{request.student_answer}\n\n"
                f"MAXIMUM POINTS: {request.max_points}\n\n"
                f"Instructions:\n"
                f"1. Compare the student's answer to the rubric.\n"
                f"2. Assign a score between 0 and {request.max_points} (decimals allowed).\n"
                f"3. Write 2-4 sentences of constructive feedback explaining the score.\n"
                f"4. Be factual and direct — do NOT add generic encouragement.\n\n"
                f"Respond ONLY with a JSON object in this exact format:\n"
                f'{{"score": <number>, "feedback": "<string>"}}'
            )

            grading_schema = {
                "type": "OBJECT",
                "properties": {
                    "score": {"type": "NUMBER"},
                    "feedback": {"type": "STRING"},
                },
                "required": ["score", "feedback"],
            }

            config = types.GenerateContentConfig(
                response_mime_type="application/json",
                response_schema=grading_schema,
                temperature=0.2,
            )

            response = self.client.models.generate_content(
                model=self.model_name,
                contents=prompt,
                config=config,
            )

            if response.text is None:
                raise ValueError("No response text received from Gemini API")

            result = json.loads(response.text)
            raw_score = float(result.get("score", 0))
            # Clamp to valid range
            score = max(0.0, min(raw_score, request.max_points))
            percentage = round((score / request.max_points) * 100, 2) if request.max_points > 0 else 0.0

            logger.info(
                f"Essay graded: score={score}/{request.max_points} ({percentage}%) "
                f"for grade {request.grade} {request.subject}"
            )
            return GradeEssayResponse(
                score=score,
                feedback=result.get("feedback", ""),
                score_percentage=percentage,
            )

        except Exception as e:
            logger.error(f"Error grading essay: {str(e)}")
            raise

    async def generate_socratic_hints(
        self,
        question_content: str,
        student_answer: str,
        correct_answer: str,
        subject: str,
        grade: int,
        language: str = "vi",
    ) -> dict[str, Any]:
        """Generate Socratic method guiding questions for a teacher."""
        try:
            prompt = build_socratic_hints_prompt(
                question_content=question_content,
                student_answer=student_answer,
                correct_answer=correct_answer,
                subject=subject,
                grade=grade,
                language=language,
            )

            hints_schema = {
                "type": "OBJECT",
                "properties": {
                    "hints": {
                        "type": "ARRAY",
                        "items": {"type": "STRING"},
                    },
                    "teaching_note": {"type": "STRING"},
                },
                "required": ["hints", "teaching_note"],
            }

            config = types.GenerateContentConfig(
                response_mime_type="application/json",
                response_schema=hints_schema,
                temperature=0.6,
            )

            response = self.client.models.generate_content(
                model=self.model_name,
                contents=prompt,
                config=config,
            )

            if response.text is None:
                raise ValueError("No response text received from Gemini API")

            result = json.loads(response.text)
            logger.info(
                f"Generated {len(result.get('hints', []))} Socratic hints "
                f"for grade {grade} {subject}"
            )
            return result

        except Exception as e:
            logger.error(f"Error generating Socratic hints: {str(e)}")
            raise
