# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY PortfolioCapararo/*.csproj ./PortfolioCapararo/
COPY Application/*.csproj ./Application/
COPY Domain/*.csproj ./Domain/
COPY Infraestructure/*.csproj ./Infraestructure/

# Restore
WORKDIR /src/PortfolioCapararo
RUN dotnet restore

# Copy everything else
WORKDIR /src
COPY . .

# Build and publish
WORKDIR /src/PortfolioCapararo
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Render sets PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "PortfolioCapararo.dll"]