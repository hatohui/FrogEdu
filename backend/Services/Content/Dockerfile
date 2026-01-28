FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Services/Content/ Services/Content/
COPY Shared/ Shared/

WORKDIR /src/Services/Content
RUN dotnet restore Content.API/FrogEdu.Content.API.csproj
RUN dotnet publish Content.API/FrogEdu.Content.API.csproj -c Release -o /app/publish

FROM public.ecr.aws/lambda/dotnet:9 AS runtime
WORKDIR /var/task
COPY --from=build /app/publish .
CMD ["FrogEdu.Content.API"]