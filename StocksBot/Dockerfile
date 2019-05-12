FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY StocksBot/StocksBot.csproj StocksBot/
RUN dotnet restore StocksBot/StocksBot.csproj
COPY . .
WORKDIR /src/StocksBot
RUN dotnet build StocksBot.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish StocksBot.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "StocksBot.dll"]
