FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Services/Class/ Services/Class/
COPY Shared/ Shared/

WORKDIR /src/Services/Class
RUN dotnet restore Class.API/FrogEdu.Class.API.csproj
RUN dotnet publish Class.API/FrogEdu.Class.API.csproj -c Release -o /app/publish

FROM public.ecr.aws/lambda/dotnet:9 AS runtime
WORKDIR /var/task
COPY --from=build /app/publish .
CMD ["FrogEdu.Class.API"]