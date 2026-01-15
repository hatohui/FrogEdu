FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Services/Content/ Services/Content/
COPY Shared/ Shared/

WORKDIR /src/Services/Content
RUN dotnet restore Content.API/FrogEdu.Content.API.csproj
RUN dotnet publish Content.API/FrogEdu.Content.API.csproj -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FrogEdu.Content.API.dll"]