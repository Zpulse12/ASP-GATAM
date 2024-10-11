FROM node:18 AS styling
WORKDIR /style
RUN ["npx", "tailwindcss", "init"]
RUN ["mkdir", "out"]
WORKDIR /style/out
RUN ["touch", "tailwind.css"]
WORKDIR /style
COPY tailwind.config.js ./tailwind.config.js
COPY global.tailwind.css ./global.tailwind.css
RUN ["npx", "tailwindcss", "-i", "./global.tailwind.css", "-o", "./out/tailwind.css"]
#
# STAGE 1 Alles builden van projecten en dependencies.
#
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
ARG APP="Gatam.WebAppBegeleider"
# Alles overkopieren wat we nodig hebben.
COPY *.sln .
COPY ApplicationCore/Gatam.Application/*.csproj ./ApplicationCore/Gatam.Application/
COPY ApplicationCore/Gatam.Domain/*.csproj ./ApplicationCore/Gatam.Domain/
COPY Infrastructure/Gatam.Infrastructure/*.csproj ./Infrastructure/Gatam.Infrastructure/
COPY Presentation/${APP}/*.csproj ./Presentation/${APP}/
RUN dotnet restore ./Presentation/${APP}/${APP}.csproj # Restore dependencies voor het project dat gaat runnen.
# copy code
COPY . .
COPY --from=styling /style/out/tailwind.css ./Presentation/${APP}/wwwroot/
RUN dotnet build ./Presentation/${APP}/${APP}.csproj --configuration Release # Build project in release mode
RUN dotnet publish Presentation/${APP}/${APP}.csproj -c Release -o /app --no-restore # Publish the Gatam.WebAPI project
RUN ls -la /app
#
# STAGE 2 De app omzetten naar een runtime en kleinere image.
#
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
EXPOSE 443
# alles voor .dll moet zelfde zijn als APP argument bij Stage 1
ENTRYPOINT ["dotnet", "Gatam.WebAppBegeleider.dll"]
