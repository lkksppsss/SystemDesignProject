FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN ln -sf /usr/share/zoneinfo/posix/Asia/Taipei /etc/localtime

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["nuget.config", "src/SP.Hotel.API/"]

COPY ["src/SP.Hotel.API/SP.Hotel.API.csproj", "src/SP.Hotel.API/"]
COPY ["src/SP.Hotel.Domain/SP.Hotel.Domain.csproj", "src/SP.Hotel.Domain/"]
COPY ["src/SP.Hotel.Infrastructure/SP.Hotel.Infrastructure.csproj", "src/SP.Hotel.Infrastructure/"]
RUN dotnet restore "src/SP.Hotel.API/SP.Hotel.API.csproj"
COPY . .
WORKDIR "/src/src/SP.Hotel.API"
RUN dotnet build "SP.Hotel.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SP.Hotel.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SP.Hotel.API.dll"]