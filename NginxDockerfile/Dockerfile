FROM nginx:latest

# Copiar el archivo de configuración Nginx al contenedor
COPY ../nginx.conf /etc/nginx/nginx.conf

# Copiar los certificados SSL al contenedor
COPY ../ssl/server.crt /etc/nginx/certs/server.crt
COPY ../ssl/server.key /etc/nginx/certs/server.key

# Exponer el puerto 443 para HTTPS y 80 para HTTP
EXPOSE 443
EXPOSE 80