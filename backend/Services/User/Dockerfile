FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Services/User/ Services/User/
COPY Shared/ Shared/

WORKDIR /src/Services/User
RUN dotnet restore User.API/FrogEdu.User.API.csproj
RUN dotnet publish User.API/FrogEdu.User.API.csproj -c Release -o /app/publish

FROM public.ecr.aws/lambda/dotnet:9 AS runtime
WORKDIR /var/task
COPY --from=build /app/publish .
CMD ["FrogEdu.User.API"]