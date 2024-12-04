# Proyecto HCE

## Introducción

Este proyecto implementa un sistema basado en microservicios para gestionar una aplicación de historia clínica electrónica (HCE). Utiliza contenedores Docker y orquestación con Docker Swarm para desplegar y ejecutar tanto el backend como el frontend.

La aplicación consta de tres módulos principales:
- **Cliente**: Disponible en el puerto `4200/cliente`.
- **Médico**: Disponible en el puerto `4201/medico`.
- **Administración**: Disponible en el puerto `4202/administracion`.

Este README detalla las instrucciones necesarias para configurar, desplegar y ejecutar la aplicación.

---

## Tabla de Contenidos

- [Requisitos Previos](#requisitos-previos)
- [Instalación](#instalación)
- [Uso](#uso)
- [Solución de Problemas](#solución-de-problemas)
- [Contribuidores](#contribuidores)

---

## Requisitos Previos

Antes de comenzar, asegúrate de cumplir con los siguientes requisitos:
1. **Docker** instalado en tu máquina.
2. Acceso a una terminal **PowerShell**.
3. Credenciales de una cuenta de Docker Hub válida, en este caso: `Mangas330`.
  - De necesitar utilizar una cuenta diferente de Docker Hub, se puede cambiar el usuario utilizado en los archivos docker-compose.yml, docker-swarm-vital.yml, docker-swarm-servicios.yml y docker-swarm-nginx.yml, cambiando "mangas330/" en las imágenes por el nuevo nombre de usuario.
4. Permisos administrativos para modificar políticas de ejecución en PowerShell.

---

## Instalación

### Primera Ejecución

1. Abre una terminal **PowerShell** en la raíz del proyecto.
2. Inicializa Docker Swarm con el siguiente comando:
   ```docker swarm init```
3. Inicia sesión en Docker Hub con tus credenciales:
   ```docker login```
   Utiliza las credenciales de la cuenta `Mangas330`.
4. Construye las imágenes del proyecto:
   ```docker compose build```
5. Publica las imágenes en Docker Hub:
   ```docker compose push```

---

## Uso

### Ejecución de la Aplicación

Si es la primera vez que ejecutas la aplicación o si no ha habido cambios en el código:
1. Establece la política de ejecución de PowerShell:
   ```Set-ExecutionPolicy -Scope Process -ExecutionPolicy RemoteSigned```
2. Ejecuta el script de inicio:
   ```./Correr_Aplicacion.ps1```
   Este script se encargará de levantar los contenedores de Docker Swarm y los módulos frontend en sus respectivos puertos:
   - Cliente: `4200/cliente`
   - Médico: `4201/medico`
   - Administración: `4202/administracion`

---

## Solución de Problemas

### Contenedores no se levantan correctamente

1. Elimina la pila actual:
   ```docker stack rm HCE```
2. Despliega los servicios en el siguiente orden:
   - Primero, despliega los servicios esenciales:
     ```docker stack deploy -c docker-swarm-vital.yml HCE```
     Asegúrate de que SQL y RabbitMQ estén funcionando correctamente.
   - Luego, despliega los microservicios:
     ```docker stack deploy -c docker-swarm-servicios.yml HCE```
     Verifica que los microservicios estén activos.
   - Finalmente, despliega el balanceador de carga:
     ```docker stack deploy -c docker-swarm-nginx.yml HCE```

### Fallo de certificados

- Asegúrate de instalar todos los certificados de la aplicación, ya que estos son autofirmados. Estos se encuentran en la carpeta ssl del directorio raíz, así como la carpeta ssl dentro de frontCliente, frontAdmin y frontMedico

---

## Contribuidores

- Integrantes del equipo de desarrollo: 
  `Ignacio Díaz`
  `Rodrigo de León`
  `Santiago Sosa`
  `Lautaro Portillo`