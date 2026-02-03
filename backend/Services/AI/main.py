import logging
import asyncio
from contextlib import asynccontextmanager
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import RedirectResponse

from app.api import router
from app.config import get_settings

logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s - %(name)s - %(levelname)s - %(message)s"
)
logger = logging.getLogger(__name__)

# Global uvicorn server instance for Lambda
_uvicorn_server = None
_server_task = None


@asynccontextmanager
async def lifespan(app: FastAPI):
    """Lifespan context manager for startup and shutdown events."""
    settings = get_settings()
    logger.info(f"Starting {settings.app_name}")
    logger.info(f"Debug mode: {settings.debug}")
    yield
    logger.info("Shutting down FrogEdu AI Service")


app = FastAPI(
    title="FrogEdu AI Service",
    description="AI-powered question generation and tutoring service",
    version="1.0.0",
    lifespan=lifespan,
    docs_url="/docs",
    redoc_url="/redoc",
)

settings = get_settings()
app.add_middleware(
    CORSMiddleware,
    allow_origins=settings.cors_origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Include API router
app.include_router(router)


@app.get("/", include_in_schema=False)
async def root():
    """Redirect root to API docs."""
    return RedirectResponse(url="/docs")


async def start_uvicorn_server():
    """Start uvicorn server for Lambda Web Adapter."""
    global _uvicorn_server
    import uvicorn
    
    config = uvicorn.Config(
        app,
        host="127.0.0.1",
        port=8000,
        log_level="info"
    )
    _uvicorn_server = uvicorn.Server(config)
    await _uvicorn_server.serve()


# Start the server when module is imported
if __name__ != "__main__":
    # Running in Lambda context
    try:
        asyncio.run(start_uvicorn_server())
    except Exception as e:
        logger.error(f"Failed to start uvicorn server: {e}")
        raise


if __name__ == "__main__":
    # Local development
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
