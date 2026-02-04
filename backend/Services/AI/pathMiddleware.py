from fastapi import Request
from starlette.middleware.base import BaseHTTPMiddleware
from starlette.types import ASGIApp


class PathPrefixMiddleware(BaseHTTPMiddleware):
    """
    Middleware to ensure all requests have proper leading slashes.
    
    API Gateway strips the /api/ai prefix and may pass paths without leading slashes.
    FastAPI's root_path="/api/ai" handles the prefix, so we just need to ensure
    paths start with /.
    """
    
    def __init__(self, app: ASGIApp):
        super().__init__(app)
    
    async def dispatch(self, request: Request, call_next):
        path = request.url.path
        
        # Ensure path starts with /
        if not path.startswith("/"):
            path = "/" + path
            request.scope["path"] = path
        
        response = await call_next(request)
        return response
