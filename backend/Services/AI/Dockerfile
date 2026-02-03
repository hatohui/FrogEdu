FROM python:3.13-slim

WORKDIR /app

# Copy requirements and install dependencies
COPY requirement.txt .
RUN pip install --no-cache-dir --upgrade -r requirement.txt

# Copy application code
COPY main.py .
COPY app ./app

# Set Python to run in unbuffered mode
ENV PYTHONUNBUFFERED=1

# Run the application
EXPOSE 8080
ENTRYPOINT ["python", "-m", "awslambdaric"]
CMD ["main.handler"]