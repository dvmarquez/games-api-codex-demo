FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file first for better layer caching
COPY src/GamesApi/GamesApi.csproj src/GamesApi/
RUN dotnet restore src/GamesApi/GamesApi.csproj

# Copy source and publish
COPY . .
RUN dotnet build src/GamesApi/GamesApi.csproj -c Release --no-restore
RUN dotnet publish src/GamesApi/GamesApi.csproj -c Release -o /app/publish --no-build

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "GamesApi.dll"]
