# Etapa 1: Construcción
FROM node:20 AS build-step

RUN mkdir -p /app

WORKDIR /app

COPY package.json /app

RUN npm install

COPY . /app

RUN npm run build --prod

# Etapa 2: Despliegue
FROM nginx:1.21.3-alpine

COPY --from=build-step /app/dist/front-medico/browser /usr/share/nginx/html

# Copiar certificados SSL
COPY ssl/server.crt /etc/nginx/certs/server.crt
COPY ssl/server.key /etc/nginx/certs/server.key

# Copiar configuración de Nginx
COPY ssl/nginx-default.conf /etc/nginx/conf.d/default.conf


