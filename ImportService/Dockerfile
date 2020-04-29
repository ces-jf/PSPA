#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["nuget.config", ""]
COPY ["ImportService/ImportService.csproj", "ImportService/"]
RUN dotnet restore "ImportService/ImportService.csproj"
COPY . .
WORKDIR "/src/ImportService"
RUN dotnet build "ImportService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImportService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImportService.dll"]