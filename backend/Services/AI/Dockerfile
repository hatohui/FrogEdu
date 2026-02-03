FROM public.ecr.aws/lambda/python:3.13

# Copy requirements and install dependencies
COPY requirement.txt ${LAMBDA_TASK_ROOT}/
RUN pip install --no-cache-dir -r ${LAMBDA_TASK_ROOT}/requirement.txt

# Copy application code
COPY main.py ${LAMBDA_TASK_ROOT}/
COPY app ${LAMBDA_TASK_ROOT}/app

# Set the handler
CMD ["main.handler"]