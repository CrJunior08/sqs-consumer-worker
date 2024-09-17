FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["sqs-consumer-worker.csproj", "./"]
RUN dotnet restore "./sqs-consumer-worker.csproj"

COPY . .
WORKDIR "/src"
RUN dotnet build -c Release -o /app/build

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "sqs-consumer-worker.dll"]
