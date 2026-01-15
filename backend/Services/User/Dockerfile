FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Services/User/ Services/User/
COPY Shared/ Shared/

WORKDIR /src/Services/User
RUN dotnet restore User.API/FrogEdu.User.API.csproj
RUN dotnet publish User.API/FrogEdu.User.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FrogEdu.User.API.dll"]