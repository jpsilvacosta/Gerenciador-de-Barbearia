# Imagem base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Imagem para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar os csproj primeiro (cache eficiente)
COPY ./src/BarberBoss.Api/BarberBoss.Api.csproj ./BarberBoss.Api/
COPY ./src/BarberBoss.Application/BarberBoss.Application.csproj ./BarberBoss.Application/
COPY ./src/BarberBoss.Domain/BarberBoss.Domain.csproj ./BarberBoss.Domain/
COPY ./src/BarberBoss.Infrastructure/BarberBoss.Infrastructure.csproj ./BarberBoss.Infrastructure/
COPY ./src/BarberBoss.Communication/BarberBoss.Communication.csproj ./BarberBoss.Communication/
COPY ./src/BarberBoss.Exception/BarberBoss.Exception.csproj ./BarberBoss.Exception/

# Restaurar dependências
RUN dotnet restore ./BarberBoss.Api/BarberBoss.Api.csproj

# Copiar o restante do código (inclui appsettings)
COPY ./src ./ 

WORKDIR /src/BarberBoss.Api
RUN dotnet build "BarberBoss.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BarberBoss.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BarberBoss.Api.dll"]
