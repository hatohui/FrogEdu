from enum import Enum


class CognitiveLevel(str, Enum):
    """Cognitive levels based on Bloom's Taxonomy (4 levels only)."""
    REMEMBER = "remember"
    UNDERSTAND = "understand"
    APPLY = "apply"
    ANALYZE = "analyze"


class QuestionType(str, Enum):
    """
    Types of questions that can be generated.
    
    1. TRUE_FALSE: A statement is correct or wrong (True/False only)
    2. SELECT: Select ONE correct answer from multiple options (A, B, C, D)
    3. MULTIPLE_CHOICE: Select ONE or MORE correct answers from multiple options
    4. ESSAY: Open-ended, no correct answer, AI or self graded
    5. FILL_IN_BLANK: User types the exact word/phrase to match
    """
    SELECT = "select"  # Single choice (MultipleChoice in backend C#)
    MULTIPLE_CHOICE = "multiple_choice"  # Multiple correct answers (MultipleAnswer in backend C#)
    TRUE_FALSE = "true_false"
    ESSAY = "essay"
    FILL_IN_BLANK = "fill_in_blank"


class QuestionSource(str, Enum):
    """Source of the question."""
    AI_GENERATED = "ai_generated"
    MANUAL = "manual"
    IMPORTED = "imported"
