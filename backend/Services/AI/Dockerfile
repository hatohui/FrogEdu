FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Services/AI/ Services/AI/
COPY Shared/ Shared/

WORKDIR /src/Services/AI
RUN dotnet restore AI.API/FrogEdu.AI.API.csproj
RUN dotnet publish AI.API/FrogEdu.AI.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FrogEdu.AI.API.dll"]