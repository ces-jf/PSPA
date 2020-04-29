#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["nuget.config", ""]
COPY ["BlazorMVC/BlazorSite.csproj", "BlazorMVC/"]
COPY ["Infra/Infra.csproj", "Infra/"]
COPY ["SystemHelper/SystemHelper.csproj", "SystemHelper/"]
COPY ["IoC/IoC.csproj", "IoC/"]
COPY ["Data/Data.csproj", "Data/"]
RUN dotnet restore "BlazorMVC/BlazorSite.csproj"
COPY . .
WORKDIR "/src/BlazorMVC"
RUN dotnet build "BlazorSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorSite.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorSite.dll"]
