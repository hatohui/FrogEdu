"""Prompt templates for Gemini AI service."""
from typing import Optional
from app.schemas import GenerateQuestionsRequest, GenerateSingleQuestionRequest


def build_matrix_prompt(request: GenerateQuestionsRequest) -> str:
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


def build_single_question_prompt(request: GenerateSingleQuestionRequest) -> str:
    """Build prompt for single question generation."""
    language_instruction = (
        "Generate all content in Vietnamese."
        if request.language == "vi"
        else "Generate all content in English."
    )
    
    topic_context = ""
    if request.topic_description:
        topic_context = f"\n**Topic Description**: {request.topic_description}"
    
    return f"""You are an expert education content creator.

Generate ONE exam question with these specifications:

**Subject**: {request.subject}
**Grade Level**: {request.grade}
**Topic**: {request.topic_name}{topic_context}
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


def build_tutor_system_instruction(
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
