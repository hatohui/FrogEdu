FROM python:3.13-slim

WORKDIR /app

# Copy requirements and install dependencies
COPY requirement.txt .
RUN pip install --no-cache-dir --upgrade -r requirement.txt

# Copy application code
COPY main.py .
COPY app ./app

# Run the application
EXPOSE 8080
CMD ["fastapi", "run", "main.py", "--host", "0.0.0.0", "--port", "8080"]