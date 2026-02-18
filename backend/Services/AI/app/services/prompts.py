"""Prompt templates for Gemini AI service."""
from typing import Optional
from app.schemas import GenerateQuestionsRequest, GenerateSingleQuestionRequest
from app.models.enums import QuestionType


# Question type specific instructions
QUESTION_TYPE_INSTRUCTIONS = {
    QuestionType.TRUE_FALSE: """
**TRUE/FALSE Question Format:**
- Create a clear statement that is either definitely TRUE or definitely FALSE
- The statement should be unambiguous and fact-based
- Generate exactly 2 answers: "True" and "False"
- Mark exactly one as correct (is_correct: true)
- Provide an explanation for why the statement is true or false

Example output:
{
    "content": "Water boils at 100 degrees Celsius at sea level.",
    "question_type": "true_false",
    "answers": [
        {"content": "True", "is_correct": true, "explanation": "At standard atmospheric pressure (sea level), pure water boils at exactly 100°C."},
        {"content": "False", "is_correct": false, "explanation": "This would be incorrect as the statement is factually accurate."}
    ]
}
""",
    
    QuestionType.SELECT: """
**SELECT (Single Choice) Question Format:**
- Create a question with ONE correct answer
- Generate 4 answer options (A, B, C, D)
- Mark exactly ONE answer as correct (is_correct: true)
- All other answers must have is_correct: false
- Provide explanations for each answer

Example output:
{
    "content": "What is the capital of France?",
    "question_type": "select",
    "answers": [
        {"content": "London", "is_correct": false, "explanation": "London is the capital of the United Kingdom."},
        {"content": "Paris", "is_correct": true, "explanation": "Paris is the capital and largest city of France."},
        {"content": "Berlin", "is_correct": false, "explanation": "Berlin is the capital of Germany."},
        {"content": "Madrid", "is_correct": false, "explanation": "Madrid is the capital of Spain."}
    ]
}
""",

    QuestionType.MULTIPLE_CHOICE: """
**MULTIPLE CHOICE (Multiple Answers) Question Format:**
- Create a question where ONE OR MORE answers can be correct
- Generate 4-6 answer options
- Mark ALL correct answers with is_correct: true (can be 1 to all options)
- The question should clearly indicate multiple answers may be correct
- Use phrases like "Select all that apply" or "Which of the following are..."
- Provide explanations for each answer

Example output:
{
    "content": "Which of the following are prime numbers? (Select all that apply)",
    "question_type": "multiple_choice",
    "answers": [
        {"content": "2", "is_correct": true, "explanation": "2 is the only even prime number."},
        {"content": "4", "is_correct": false, "explanation": "4 is divisible by 2, so it's not prime."},
        {"content": "7", "is_correct": true, "explanation": "7 is only divisible by 1 and itself."},
        {"content": "9", "is_correct": false, "explanation": "9 is divisible by 3, so it's not prime."},
        {"content": "11", "is_correct": true, "explanation": "11 is only divisible by 1 and itself."}
    ]
}
""",

    QuestionType.ESSAY: """
**ESSAY Question Format:**
- Create an open-ended question that requires a detailed written response
- There is NO correct answer - the response will be graded based on content and accuracy
- Generate exactly 1 answer entry with grading guidelines/rubric
- The answer content should describe what a good response should include
- Set is_correct: true for the grading rubric entry

Example output:
{
    "content": "Explain the causes and effects of climate change on global ecosystems.",
    "question_type": "essay",
    "answers": [
        {
            "content": "A comprehensive answer should include: (1) Main causes: greenhouse gas emissions, deforestation, industrial activities; (2) Effects: rising temperatures, sea level rise, biodiversity loss; (3) Specific ecosystem impacts: coral bleaching, habitat destruction, species migration",
            "is_correct": true,
            "explanation": "Grading rubric: Content accuracy (40%), Logical structure (30%), Supporting examples (20%), Language clarity (10%)"
        }
    ]
}
""",

    QuestionType.FILL_IN_BLANK: """
**FILL IN THE BLANK Question Format:**
- Create a statement with a blank (use underscores: _____) for the missing word/phrase
- The user must type the EXACT word/phrase to be correct
- Generate 1-3 acceptable answers (alternative correct spellings or equivalent terms)
- All answers should have is_correct: true (they are all acceptable answers)
- Keep the expected answer as a single word or short phrase

Example output:
{
    "content": "The chemical symbol for water is _____.",
    "question_type": "fill_in_blank",
    "answers": [
        {"content": "H2O", "is_correct": true, "explanation": "H2O is the standard chemical formula for water."},
        {"content": "H₂O", "is_correct": true, "explanation": "Alternative notation with subscript."}
    ]
}
"""
}


def get_question_type_instruction(question_type: QuestionType) -> str:
    """Get the instruction for a specific question type."""
    return QUESTION_TYPE_INSTRUCTIONS.get(
        question_type,
        QUESTION_TYPE_INSTRUCTIONS[QuestionType.SELECT]
    )


def build_matrix_prompt(request: GenerateQuestionsRequest) -> str:
    """Build prompt for matrix-based question generation."""
    # Build per-topic details, including question_type instruction
    matrix_lines = []
    type_instructions_used: set[QuestionType] = set()
    
    for topic in request.matrix_topics:
        if topic.question_type:
            type_label = topic.question_type.value
            type_instructions_used.add(topic.question_type)
        else:
            type_label = "random (vary across select, true_false, multiple_choice)"
        
        matrix_lines.append(
            f"- Topic: {topic.topic_name}, "
            f"Cognitive Level: {topic.cognitive_level.value}, "
            f"Quantity: {topic.quantity}, "
            f"Question Type: {type_label}"
        )
    
    matrix_details = "\n".join(matrix_lines)
    
    language_instruction = (
        "Generate ALL content in Vietnamese (questions, answers, and explanations)."
        if request.language == "vi"
        else "Generate ALL content in English (questions, answers, and explanations)."
    )
    
    # Include specific type instructions only for types that are explicitly requested
    type_specific_section = ""
    if type_instructions_used:
        type_specific_section = "\n**Question Type Instructions:**\n"
        for qt in type_instructions_used:
            type_specific_section += get_question_type_instruction(qt) + "\n"
    
    curriculum_note = (
        "Follow Vietnamese curriculum standards and context."
        if request.language == "vi"
        else "Follow international curriculum standards appropriate for this grade level."
    )

    return f"""You are an expert education content creator.

{language_instruction}

Generate exam questions with the following specifications:

**Subject**: {request.subject}
**Grade Level**: {request.grade}
**Output Language**: {request.language.upper()} — YOU MUST write ALL question content, ALL answer content, and ALL explanations in {"Vietnamese" if request.language == "vi" else "English"}. Do NOT mix languages.

**Matrix Requirements** (generate exactly the specified quantity for each row):
{matrix_details}

{type_specific_section}

**General Rules for ALL questions:**
1. Content must be appropriate for grade {request.grade} students
2. {curriculum_note}
3. Strictly match the specified cognitive level (Bloom's Taxonomy):
   - remember: Recall facts, definitions, basic concepts
   - understand: Explain, summarize, interpret ideas
   - apply: Use knowledge in new situations, solve problems
   - analyze: Break down, compare, draw connections
4. For "random" question types: vary question types naturally across select, true_false, and multiple_choice
5. Always include 4 answer options for select/multiple_choice questions
6. Mark the correct answer(s) accurately
7. Provide clear, educational explanations for each answer

Return ONLY the questions in the specified JSON format with no additional text."""


def build_single_question_prompt(request: GenerateSingleQuestionRequest) -> str:
    """Build prompt for single question generation."""
    language_instruction = (
        "Generate ALL content in Vietnamese (question, answers, and explanations)."
        if request.language == "vi"
        else "Generate ALL content in English (question, answers, and explanations)."
    )
    
    topic_context = ""
    if request.topic_description:
        topic_context = f"\n**Topic Description**: {request.topic_description}"
    
    # Get type-specific instructions
    type_instruction = get_question_type_instruction(request.question_type)
    
    return f"""You are an expert education content creator.

{language_instruction}

Generate ONE exam question with these specifications:

**Subject**: {request.subject}
**Grade Level**: {request.grade}
**Topic**: {request.topic_name}{topic_context}
**Cognitive Level**: {request.cognitive_level.value}
**Question Type**: {request.question_type.value}
**Output Language**: {request.language.upper()} — YOU MUST write ALL question content, ALL answer content, and ALL explanations in {"Vietnamese" if request.language == "vi" else "English"}. Do NOT mix languages.

{type_instruction}

Requirements:
1. Content appropriate for grade {request.grade}
2. Follow the specified cognitive level (Bloom's Taxonomy):
   - Remember: Recall facts and basic concepts
   - Understand: Explain ideas or concepts
   - Apply: Use information in new situations
   - Analyze: Draw connections among ideas

3. STRICTLY follow the question type format specified above
4. Provide clear explanations for each answer

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
