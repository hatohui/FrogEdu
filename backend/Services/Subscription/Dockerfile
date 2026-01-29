FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Services/Subscription/ Services/Subscription/
COPY Shared/ Shared/

WORKDIR /src/Services/Subscription
RUN dotnet restore Subscription.API/FrogEdu.Subscription.API.csproj
RUN dotnet publish Subscription.API/FrogEdu.Subscription.API.csproj -c Release -o /app/publish

FROM public.ecr.aws/lambda/dotnet:9 AS runtime
WORKDIR /var/task
COPY --from=build /app/publish .
CMD ["FrogEdu.Subscription.API"]