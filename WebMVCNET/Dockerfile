FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["WebMVCNET/WebMVCNET.csproj", "WebMVCNET/"]
RUN dotnet restore "WebMVCNET/WebMVCNET.csproj"
COPY . .
WORKDIR "/src/WebMVCNET"
RUN dotnet build "WebMVCNET.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebMVCNET.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebMVCNET.dll"]