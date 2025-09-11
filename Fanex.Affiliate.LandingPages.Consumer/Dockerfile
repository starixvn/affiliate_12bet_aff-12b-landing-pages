FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder
WORKDIR /src

COPY *.sln .
COPY Fanex.Affiliate.LandingPages.Consumer/Fanex.Affiliate.LandingPages.Consumer.csproj ./Fanex.Affiliate.LandingPages.Consumer/
COPY Fanex.Affiliate.LandingPages.Core/Fanex.Affiliate.LandingPages.Core.csproj ./Fanex.Affiliate.LandingPages.Core/

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2

RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet restore -v n ./Fanex.Affiliate.LandingPages.Consumer/Fanex.Affiliate.LandingPages.Consumer.csproj

COPY Fanex.Affiliate.LandingPages.Consumer/. ./Fanex.Affiliate.LandingPages.Consumer/
COPY Fanex.Affiliate.LandingPages.Core/. ./Fanex.Affiliate.LandingPages.Core/

WORKDIR /src/Fanex.Affiliate.LandingPages.Consumer

RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet publish -v n -r linux-x64 --self-contained false -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine AS base

RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app

USER dotnetuser

ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000


WORKDIR /app
COPY --from=builder /app .

ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Fanex.Affiliate.LandingPages.Consumer.dll"]