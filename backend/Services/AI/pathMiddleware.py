from fastapi import Request
from starlette.middleware.base import BaseHTTPMiddleware
from starlette.types import ASGIApp


class PathPrefixMiddleware(BaseHTTPMiddleware):
    """
    Middleware to ensure all requests have the /api/ai/ prefix.
    
    This is necessary because API Gateway may strip the path prefix
    when routing to Lambda, but FastAPI expects the full path.
    """
    
    def __init__(self, app: ASGIApp, prefix: str = "/api/ai"):
        super().__init__(app)
        self.prefix = prefix
    
    async def dispatch(self, request: Request, call_next):
        path = request.url.path
        
        # If path doesn't start with the prefix and doesn't start with /api/ai/
        if not path.startswith(self.prefix):
            # Ensure path starts with /
            if not path.startswith("/"):
                path = "/" + path
            
            # Prepend the prefix
            path = self.prefix + path
            
            # Modify the request scope to use the new path
            request.scope["path"] = path
        
        response = await call_next(request)
        return response
