FROM public.ecr.aws/lambda/python:3.13

# Copy Lambda Web Adapter for running web applications on Lambda
COPY --from=public.ecr.aws/awsguru/aws-lambda-adapter:0.9.1 /lambda-adapter /opt/extensions/lambda-adapter

# Set working directory
WORKDIR ${LAMBDA_TASK_ROOT}

# Copy requirements and install dependencies
COPY requirement.txt .
RUN python -m pip install --no-cache-dir --upgrade -r requirement.txt

# Copy application code
COPY main.py .
COPY app ./app

# Set environment variables
ENV PORT=8000

# Use Lambda Web Adapter with FastAPI/Uvicorn
# This allows the container to run as a web application on Lambda
CMD exec uvicorn --port ${PORT} main:app