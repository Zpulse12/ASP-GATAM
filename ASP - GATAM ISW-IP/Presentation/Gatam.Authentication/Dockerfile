FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG APP_NAME=Gatam.Authentication

WORKDIR /src
COPY ["${APP_NAME}.csproj", "."]
RUN dotnet restore "${APP_NAME}.csproj"
COPY . .
RUN dotnet build "${APP_NAME}.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "${APP_NAME}.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gatam.Authentication.dll"]