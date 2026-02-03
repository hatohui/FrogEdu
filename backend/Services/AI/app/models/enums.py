from enum import Enum


class CognitiveLevel(str, Enum):
    """Cognitive levels based on Bloom's Taxonomy."""
    REMEMBER = "remember"
    UNDERSTAND = "understand"
    APPLY = "apply"
    ANALYZE = "analyze"

class QuestionType(str, Enum):
    """Types of questions that can be generated."""
    MULTIPLE_CHOICE = "multiple_choice"
    TRUE_FALSE = "true_false"
    SHORT_ANSWER = "short_answer"
    ESSAY = "essay"


class QuestionSource(str, Enum):
    """Source of the question."""
    AI_GENERATED = "ai_generated"
    MANUAL = "manual"
    IMPORTED = "imported"
