# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./PLPlayersAPI/PLPlayersAPI.csproj" --disable-parallel
RUN dotnet publish "./PLPlayersAPI/PLPlayersAPI.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./

# Define default values for environment variables
ENV ASPNETCORE_HTTP_PORT=https://+:5001
ENV ASPNETCORE_URLS=http://+:5000

ARG ConnectionStrings__PremierLeagueDatabase
ARG Jwt__Key
ARG Jwt__Issuer
ARG Jwt__Audience

ENV ConnectionStrings__PremierLeagueDatabase=$ConnectionStrings__PremierLeagueDatabase
ENV Jwt__Key=$Jwt__Key
ENV Jwt__Issuer=$Jwt__Issuer
ENV Jwt__Audience=$Jwt__Audience


EXPOSE 5000

ENTRYPOINT ["dotnet", "PLPlayersAPI.dll"]
