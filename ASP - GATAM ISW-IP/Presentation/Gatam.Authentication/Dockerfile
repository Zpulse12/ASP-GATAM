#
# STAGE 1 Alles builden van projecten en dependencies.
#
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
EXPOSE 8080
EXPOSE 443
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
ARG APP="Gatam.Authentication"
# Alles overkopieren wat we nodig hebben.
COPY *.sln .
COPY ApplicationCore/Gatam.Application/*.csproj ./ApplicationCore/Gatam.Application/
COPY ApplicationCore/Gatam.Domain/*.csproj ./ApplicationCore/Gatam.Domain/
COPY Infrastructure/Gatam.Infrastructure/*.csproj ./Infrastructure/Gatam.Infrastructure/
COPY Presentation/Gatam.Authentication/*.csproj ./Presentation/Gatam.Authentication/
COPY Presentation/${APP}/*.csproj ./Presentation/${APP}/
RUN dotnet restore ./Presentation/${APP}/${APP}.csproj # Restore dependencies voor het project dat gaat runnen.
# copy code
COPY . .
RUN dotnet build ./Presentation/${APP}/${APP}.csproj --configuration Release # Build project in release mode
RUN dotnet publish Presentation/${APP}/${APP}.csproj -c Release -o /app --no-restore # Publish the Gatam.WebAPI project
#
# STAGE 2 De app omzetten naar een runtime en kleinere image.
#
FROM base AS runtime
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
# alles voor .dll moet zelfde zijn als APP argument bij Stage 1
ENTRYPOINT ["dotnet" , "Gatam.Authentication.dll"]
