# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Copiar el archivo de configuración Nginx al contenedor
COPY ../nginx.conf /etc/nginx/nginx.conf

# Copiar los certificados SSL al contenedor
COPY ../ssl/server.crt /etc/nginx/certs/server.crt
COPY ../ssl/server.key /etc/nginx/certs/server.key

WORKDIR /src
COPY ["CitaMedicaWebApi/CitaMedicaWebApi.csproj", "CitaMedicaWebApi/"]
RUN dotnet restore "./CitaMedicaWebApi/CitaMedicaWebApi.csproj"
COPY . .
WORKDIR "/src/CitaMedicaWebApi"
RUN dotnet build "./CitaMedicaWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CitaMedicaWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CitaMedicaWebApi.dll"]