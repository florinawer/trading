#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TradingApp.Web.Api/TradingApp.Web.Api.csproj", "TradingApp.Web.Api/"]
COPY ["TradingApp.Web.Dto/TradingApp.Web.Dto.csproj", "TradingApp.Web.Dto/"]
COPY ["TradingApp.Domain.Entities/TradingApp.Domain.Entities.csproj", "TradingApp.Domain.Entities/"]
COPY ["TradingApp.Infrastructure.Data.Model/TradingApp.Infrastructure.Data.Model.csproj", "TradingApp.Infrastructure.Data.Model/"]
COPY ["TradingApp.Application.Services/TradingApp.Application.Services.csproj", "TradingApp.Application.Services/"]
COPY ["TradingApp.Infrastructure.Repository/TradingApp.Infrastructure.Repository.csproj", "TradingApp.Infrastructure.Repository/"]
COPY ["TradingApp.Infrastructure.Persistence/TradingApp.Infrastructure.Persistence.csproj", "TradingApp.Infrastructure.Persistence/"]
RUN dotnet restore "TradingApp.Web.Api/TradingApp.Web.Api.csproj"
COPY . .
WORKDIR "/src/TradingApp.Web.Api"
RUN dotnet build "TradingApp.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradingApp.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradingApp.Web.Api.dll"]