FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ImportacaoService/ContainerServices.csproj", "ImportacaoService/"]
COPY ["IoC/IoC.csproj", "IoC/"]
COPY ["Infra/Infra.csproj", "Infra/"]
COPY ["SystemHelper/SystemHelper.csproj", "SystemHelper/"]
COPY ["Data/Data.csproj", "Data/"]
RUN dotnet restore "ImportacaoService/ContainerServices.csproj"
COPY . .
WORKDIR "/src/ImportacaoService"
RUN dotnet build "ContainerServices.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ContainerServices.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ContainerServices.dll"]
