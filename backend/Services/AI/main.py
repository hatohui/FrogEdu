import logging
from contextlib import asynccontextmanager
from fastapi import FastAPI, Request
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import RedirectResponse
from mangum import Mangum

from app.api import router
from app.config import get_settings

logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s - %(name)s - %(levelname)s - %(message)s"
)
logger = logging.getLogger(__name__)


@asynccontextmanager
async def lifespan(app: FastAPI):
    """Lifespan context manager for startup and shutdown events."""
    settings = get_settings()
    logger.info(f"Starting {settings.app_name}")
    logger.info(f"Debug mode: {settings.debug}")
    logger.info("🔍 Registered routes:")
    for route in app.routes:
        logger.info(f"   {route}")
    yield
    logger.info("Shutting down FrogEdu AI Service")


# Initialize FastAPI application
app = FastAPI(
    title="FrogEdu AI Service",
    description="AI-powered question generation and tutoring service",
    version="1.0.0",
    lifespan=lifespan,
    docs_url="/api/ai/docs",
    redoc_url="/api/ai/redoc",
    openapi_url="/api/ai/openapi.json",
)

# Configure CORS
settings = get_settings()
app.add_middleware(
    CORSMiddleware,
    allow_origins=settings.cors_origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)


# Add request logging middleware
@app.middleware("http")
async def log_requests(request: Request, call_next):
    logger.info(f"📥 Incoming request: {request.method} {request.url.path}")
    logger.info(f"   Headers: {dict(request.headers)}")
    logger.info(f"   Query params: {dict(request.query_params)}")
    
    response = await call_next(request)
    
    logger.info(f"📤 Response status: {response.status_code}")
    return response

# Include API router (already has /api/ai prefix in routes.py)
app.include_router(router)


@app.get("/", include_in_schema=False)
async def root():
    """Redirect root to API docs."""
    return RedirectResponse(url="/api/ai/docs")


# Lambda handler with Mangum
handler = Mangum(app, lifespan="off")
