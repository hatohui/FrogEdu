FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Services/AI/ Services/AI/
COPY Shared/ Shared/

WORKDIR /src/Services/AI
RUN dotnet restore AI.API/FrogEdu.AI.API.csproj
RUN dotnet publish AI.API/FrogEdu.AI.API.csproj -c Release -o /app/publish

FROM public.ecr.aws/lambda/dotnet:9 AS runtime
WORKDIR /var/task
COPY --from=build /app/publish .
CMD ["FrogEdu.AI.API"]