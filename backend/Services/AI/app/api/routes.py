from fastapi import APIRouter, Depends, HTTPException, status
from typing import Annotated
import logging

from app.schemas import (
    GenerateQuestionsRequest,
    GenerateQuestionsResponse,
    GenerateSingleQuestionRequest,
    Question,
    TutorChatRequest,
    TutorChatResponse,
    HealthResponse,
)
from app.services import GeminiService
from app.config import get_settings, Settings
from app.auth import TokenUser, get_current_user, get_subscribed_user

logger = logging.getLogger(__name__)

router = APIRouter(tags=["AI"])


def get_gemini_service(
    settings: Annotated[Settings, Depends(get_settings)]
) -> GeminiService:
    """Dependency to get GeminiService instance."""
    return GeminiService(settings)


@router.get("/health", response_model=HealthResponse)
async def health_check(
    service: Annotated[GeminiService, Depends(get_gemini_service)],
    settings: Annotated[Settings, Depends(get_settings)]
):
    """
    Health check endpoint to verify service and Gemini API connectivity.
    """
    gemini_connected = service.health_check()
    
    return HealthResponse(
        status="healthy" if gemini_connected else "degraded",
        service_name=settings.app_name,
        gemini_connected=gemini_connected
    )


@router.post(
    "/questions/generate",
    response_model=GenerateQuestionsResponse,
    status_code=status.HTTP_201_CREATED
)
async def generate_questions(
    request: GenerateQuestionsRequest,
    service: Annotated[GeminiService, Depends(get_gemini_service)],
    user: Annotated[TokenUser, Depends(get_subscribed_user)]
):
    """
    Generate multiple questions based on an exam matrix.
    
    This endpoint takes a matrix configuration specifying topics, cognitive levels,
    and quantities to generate a set of exam questions.
    
    **Requirements:**
    - Teacher role with active Pro subscription
    - Valid matrix configuration
    
    **Returns:**
    - List of generated questions with answers
    - Total count of questions
    """
    # Verify user has Teacher role
    if user.role and user.role.lower() not in ["teacher", "admin"]:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN,
            detail="Only teachers can generate exam questions"
        )
    
    try:
        logger.info(f"User {user.sub} generating questions with plan: {user.subscription.plan}")
        questions = await service.generate_questions(request)
        
        return GenerateQuestionsResponse(
            questions=questions,
            total_count=len(questions)
        )
    except Exception as e:
        logger.error(f"Failed to generate questions: {str(e)}")
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to generate questions: {str(e)}"
        )


@router.post(
    "/questions/generate-single",
    response_model=Question,
    status_code=status.HTTP_201_CREATED
)
async def generate_single_question(
    request: GenerateSingleQuestionRequest,
    service: Annotated[GeminiService, Depends(get_gemini_service)],
    user: Annotated[TokenUser, Depends(get_subscribed_user)]
):
    """
    Generate a single question with specified parameters.
    
    **Requirements:**
    - Teacher role with active Pro subscription
    
    **Returns:**
    - A single generated question with answers
    """
    logger.info("🎯 generate_single_question endpoint hit!")
    logger.info(f"   Request: topic={request.topic}, cognitive_level={request.cognitive_level}")
    logger.info(f"   User: {user.sub}, role={user.role}, plan={user.subscription.plan if user.subscription else 'None'}")
    
    # Verify user has Teacher role
    if user.role and user.role.lower() not in ["teacher", "admin"]:
        logger.warning(f"❌ User {user.sub} with role {user.role} attempted to generate question (forbidden)")
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN,
            detail="Only teachers can generate exam questions"
        )
    
    try:
        logger.info(f"✅ User {user.sub} authorized, generating single question with plan: {user.subscription.plan}")
        question = await service.generate_single_question(request)
        logger.info(f"✅ Successfully generated question for user {user.sub}")
        return question
    except Exception as e:
        logger.error(f"❌ Failed to generate single question: {str(e)}", exc_info=True)
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to generate question: {str(e)}"
        )


@router.post("/tutor/chat", response_model=TutorChatResponse)
async def tutor_chat(
    request: TutorChatRequest,
    service: Annotated[GeminiService, Depends(get_gemini_service)],
    user: Annotated[TokenUser, Depends(get_subscribed_user)]
):
    """
    Interactive tutoring chat endpoint.
    
    Students can ask questions about specific subjects and topics,
    and receive guided explanations appropriate for their grade level.
    
    **Requirements:**
    - Student role with active Pro subscription
    
    **Returns:**
    - Tutor's response message
    - Optional follow-up suggestions
    """
    try:
        logger.info(f"User {user.sub} using tutor chat with plan: {user.subscription.plan}")
        response_message = await service.tutor_chat( # type: ignore
            message=request.message,
            subject=request.subject,
            grade=request.grade,
            topic=request.topic,
            conversation_history=request.conversation_history # pyright: ignore[reportUnknownMemberType]
        )
        
        return TutorChatResponse(
            message=response_message,
            suggestions=None  # Can be enhanced to generate suggestions
        )
    except Exception as e:
        logger.error(f"Failed in tutor chat: {str(e)}")
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to process chat: {str(e)}"
        )
