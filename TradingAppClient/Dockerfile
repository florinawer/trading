#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TradingAppClient.Presentation.Website/TradingAppClient.Presentation.Website.csproj", "TradingAppClient.Presentation.Website/"]
COPY ["TradingAppClient.Common.Dto/TradingAppClient.Common.Dto.csproj", "TradingAppClient.Common.Dto/"]
COPY ["TradingAppClient.Business.Logic/TradingAppClient.Business.Logic.csproj", "TradingAppClient.Business.Logic/"]
RUN dotnet restore "TradingAppClient.Presentation.Website/TradingAppClient.Presentation.Website.csproj"
COPY . .
WORKDIR "/src/TradingAppClient.Presentation.Website"
RUN dotnet build "TradingAppClient.Presentation.Website.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradingAppClient.Presentation.Website.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradingAppClient.Presentation.Website.dll"]