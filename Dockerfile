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
ENV ASPNETCORE_HTTP_PORT=5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ConnectionStrings__PremierLeagueDatabase = #
ENV Jwt__Key = #
ENV Jwt__Issuer = #
ENV Jwt__Audience = #


EXPOSE 5000

ENTRYPOINT ["dotnet", "PLPlayersAPI.dll"]
