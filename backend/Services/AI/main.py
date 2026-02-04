import logging
import sys
from contextlib import asynccontextmanager
from fastapi import FastAPI, Request
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import RedirectResponse, JSONResponse
from mangum import Mangum

from app.api import router
from app.config import get_settings

# Configure logging for Lambda
logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s - %(name)s - %(levelname)s - %(message)s",
    stream=sys.stdout,
    force=True
)
logger = logging.getLogger(__name__)

# Initialize FastAPI application
app = FastAPI(
    title="FrogEdu AI Service",
    description="AI-powered question generation and tutoring service",
    version="1.0.0",
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


@app.middleware("http")
async def log_requests(request: Request, call_next):
    logger.info("=" * 80)
    logger.info(f"📥 INCOMING REQUEST: {request.method} {request.url.path}")
    logger.info(f"   Full URL: {request.url}")
    logger.info(f"   Headers: {dict(request.headers)}")
    logger.info(f"   Query params: {dict(request.query_params)}")
    logger.info("=" * 80)
    
    try:
        response = await call_next(request)
        logger.info("=" * 80)
        logger.info(f"📤 RESPONSE: status={response.status_code}")
        logger.info("=" * 80)
        return response
    except Exception as e:
        logger.error("=" * 80)
        logger.error(f"❌ EXCEPTION IN REQUEST PROCESSING: {e}", exc_info=True)
        logger.error("=" * 80)
        raise


@app.exception_handler(404)
async def not_found_handler(request: Request, exc):
    logger.error("=" * 80)
    logger.error(f"❌ 404 NOT FOUND: {request.method} {request.url.path}")
    logger.error(f"   Available routes:")
    for route in app.routes:
        if hasattr(route, 'methods') and hasattr(route, 'path'):
            logger.error(f"      {list(route.methods)} {route.path}") # type: ignore
    logger.error("=" * 80)
    return JSONResponse(
        status_code=404,
        content={"detail": "Not Found"}
    )

app.include_router(router)

@app.get("/", include_in_schema=False)
async def root():
    """Redirect root to API docs."""
    logger.info("🏠 Root endpoint hit, redirecting to docs")
    return RedirectResponse(url="/api/ai/docs")


handler = Mangum(app, lifespan="off", api_gateway_base_path="/api/ai")
logger.info("✅ Mangum handler created successfully")
