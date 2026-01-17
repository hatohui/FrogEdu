FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /src

# Install dotnet-watch globally for hot reload
RUN dotnet tool install --global dotnet-watch

# Copy source files
COPY Services/AI/ Services/AI/
COPY Shared/ Shared/

WORKDIR /src/Services/AI

# Restore dependencies
RUN dotnet restore AI.API/FrogEdu.AI.API.csproj

# Set environment variables for polling and development
ENV DOTNET_WATCH_POLLING=1
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=1

# Run with dotnet watch for hot reload on file changes
ENTRYPOINT ["dotnet", "watch", "run", "--project", "AI.API/FrogEdu.AI.API.csproj", "--urls", "http://0.0.0.0:8080"]