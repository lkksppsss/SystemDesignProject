FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN ln -sf /usr/share/zoneinfo/posix/Asia/Taipei /etc/localtime

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["nuget.config", "src/SP.SPU.API/"]

COPY ["src/SP.SPU.API/SP.SPU.API.csproj", "src/SP.SPU.API/"]
COPY ["src/SP.SPU.Domain/SP.SPU.Domain.csproj", "src/SP.SPU.Domain/"]
COPY ["src/SP.SPU.Infrastructure/SP.SPU.Infrastructure.csproj", "src/SP.SPU.Infrastructure/"]
RUN dotnet restore "src/SP.SPU.API/SP.SPU.API.csproj"
COPY . .
WORKDIR "/src/src/SP.SPU.API"
RUN dotnet build "SP.SPU.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SP.SPU.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SP.SPU.API.dll"]

# docker build --no-cache -t SP.SPU.service:tag . 