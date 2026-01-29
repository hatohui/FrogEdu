FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Services/Exam/ Services/Exam/
COPY Shared/ Shared/

WORKDIR /src/Services/Exam
RUN dotnet restore Exam.API/FrogEdu.Exam.API.csproj
RUN dotnet publish Exam.API/FrogEdu.Exam.API.csproj -c Release -o /app/publish

FROM public.ecr.aws/lambda/dotnet:9 AS runtime
WORKDIR /var/task
COPY --from=build /app/publish .
CMD ["FrogEdu.Exam.API"]