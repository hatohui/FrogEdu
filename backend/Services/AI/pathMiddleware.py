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
        path = request.scope.get("path", "")
        
        # Ensure path starts with /
        if not path.startswith("/"):
            path = "/" + path
            # Update both path and raw_path for proper URL reconstruction
            request.scope["path"] = path
            if "raw_path" in request.scope:
                raw_path = request.scope["raw_path"]
                if isinstance(raw_path, bytes):
                    if not raw_path.startswith(b"/"):
                        request.scope["raw_path"] = b"/" + raw_path
                elif isinstance(raw_path, str):
                    if not raw_path.startswith("/"):
                        request.scope["raw_path"] = "/" + raw_path
        
        response = await call_next(request)
        return response
