"""Lambda handler for AWS Lambda Web Adapter integration."""
from typing import Any

_ = __import__("main")


def lambda_handler(event: dict[str, Any], context: Any) -> dict[str, Any]: 
    """
    Lambda handler entry point.
    
    The Lambda Web Adapter extension intercepts Lambda invocations and proxies them 
    as HTTP requests to the local FastAPI application running on localhost:8000.
    
    Args:
        event: Lambda event dictionary (not used, intercepted by adapter)
        context: Lambda context object (not used, intercepted by adapter)
    
    Returns:
        Response from the FastAPI application (proxied by Lambda Web Adapter)
    """
    return {"statusCode": 200}
