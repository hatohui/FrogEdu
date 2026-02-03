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
COPY handler.py .
COPY app ./app

# Set environment variables
ENV PORT=8000

# Lambda handler entry point - Lambda Web Adapter proxies HTTP to local FastAPI on :8000
CMD ["handler.lambda_handler"]