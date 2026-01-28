FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Services/Assessment/ Services/Assessment/
COPY Shared/ Shared/

WORKDIR /src/Services/Assessment
RUN dotnet restore Assessment.API/FrogEdu.Assessment.API.csproj
RUN dotnet publish Assessment.API/FrogEdu.Assessment.API.csproj -c Release -o /app/publish

FROM public.ecr.aws/lambda/dotnet:9 AS runtime
WORKDIR /var/task
COPY --from=build /app/publish .
CMD ["FrogEdu.Assessment.API"]