#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Copiar el archivo de configuración Nginx al contenedor
COPY ../nginx.conf /etc/nginx/nginx.conf

# Copiar los certificados SSL al contenedor
COPY ../ssl/server.crt /etc/nginx/certs/server.crt
COPY ../ssl/server.key /etc/nginx/certs/server.key

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AdministrativoWebApi/AdministrativoWebApi.csproj", "AdministrativoWebApi/"]
RUN dotnet restore "./AdministrativoWebApi/AdministrativoWebApi.csproj"
COPY . .
WORKDIR "/src/AdministrativoWebApi"
RUN dotnet build "./AdministrativoWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AdministrativoWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdministrativoWebApi.dll"]