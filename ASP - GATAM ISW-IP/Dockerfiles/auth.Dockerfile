# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["../ASP - GATAM ISW-IP.sln ", "."]
COPY ../Presentation/Gatam.WebApi/Gatam.WebAPI.csproj Presentation/Gatam.WebAPI/
COPY ../ApplicationCore/Gatam.Domain/Gatam.Domain.csproj ApplicationCore/Gatam.Domain/

# Restore dependencies
RUN dotnet restore

# Copy everything else
COPY . .

# Build the application
WORKDIR /source/Presentation/Gatam.WebApi
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

EXPOSE 8080
EXPOSE 443

WORKDIR /app
COPY --from=build /app/publish ./

# Set the entry point
ENTRYPOINT ["dotnet", "Gatam.WebApi.dll"]