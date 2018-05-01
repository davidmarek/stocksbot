FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 14841
EXPOSE 44353

FROM microsoft/dotnet:2.1-sdk AS build
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
