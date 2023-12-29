# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

RUN ln -sf /usr/share/zoneinfo/posix/Asia/Taipei /etc/localtime

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
RUN dotnet nuget Add source "/PackageLocal" -n "MyPackage1"
COPY ["SP.Hotel.Service/src/SP.Hotel.API/SP.Hotel.API.csproj", "src/SP.Hotel.API/"]
COPY ["SP.Hotel.Service/src/SP.Hotel.Infrastructure/SP.Hotel.Infrastructure.csproj", "src/SP.Hotel.Infrastructure/"]
COPY ["SP.Hotel.Service/src/SP.Hotel.Domain/SP.Hotel.Domain.csproj", "src/SP.Hotel.Domain/"]
COPY ["/PackageLocal/SPCorePackage.1.0.0.nupkg", "/PackageLocal/"]
RUN dotnet restore "src/SP.Hotel.API/SP.Hotel.API.csproj"

COPY . .
WORKDIR /src/src/SP.Hotel.API

RUN dotnet build "SP.Hotel.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SP.Hotel.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SP.Hotel.API.dll"]


