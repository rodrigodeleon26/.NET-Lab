# Ruta al archivo .env
$envFilePath = ".env"

# Función para cargar variables de entorno desde el archivo .env
function Load-EnvFile {
    Write-Host "Cargando variables desde $envFilePath..."
    Get-Content $envFilePath | ForEach-Object {
        $line = $_.Trim()
        if ($line -and -not $line.StartsWith("#")) {
            $parts = $line -split "="
            [System.Environment]::SetEnvironmentVariable($parts[0], $parts[1], "Process")
        }
    }
    Write-Host "Variables de entorno cargadas."
}

# Función para esperar hasta que los contenedores estén listos e intentar reiniciar si no están listos
function Wait-For-Containers {
    param (
        [string]$stackName,
        [int]$timeout = 600,  # 10 minutos en segundos
        [int]$retryDelay = 30 # Tiempo de espera entre reintentos en segundos
    )
    Write-Host "Esperando a que los contenedores del stack '$stackName' estén listos..."
    $startTime = Get-Date
    while ($true) {
        $services = docker service ls --filter name=$stackName --format "{{.Name}} {{.Replicas}}"
        $allReady = $true
        foreach ($service in $services) {
            $parts = $service -split "\s+"
            $replicas = $parts[1] -split "/"
            if ($replicas[0] -ne $replicas[1]) {
                $allReady = $false
                break
            }
        }
        if ($allReady) {
            Write-Host "Todos los contenedores del stack '$stackName' están listos."
            break
        }
        if (((Get-Date) - $startTime).TotalSeconds -ge $timeout) {
            Write-Host "Intentando reiniciar los servicios del stack '$stackName'..."
            docker stack deploy -c "docker-swarm-$stackName.yml" $stackName
            $startTime = Get-Date  # Reinicia el tiempo de espera tras un nuevo intento
        }
        Write-Host "Esperando $retryDelay segundos antes de volver a verificar..."
        Start-Sleep -Seconds $retryDelay
    }
}

# Paso 1: Cargar las variables de entorno
Load-EnvFile

# Paso 2: Desplegar servicios vitales
Write-Host "Desplegando servicios vitales..."
docker stack deploy -c docker-swarm-vital.yml HCE
Wait-For-Containers -stackName "HCE" -timeout 600 -retryDelay 30

# Paso 3: Desplegar servicios principales
Write-Host "Desplegando servicios principales..."
docker stack deploy -c docker-swarm-servicios.yml HCE
Wait-For-Containers -stackName "HCE" -timeout 600 -retryDelay 30

# Paso 4: Desplegar configuración de NGINX
Write-Host "Desplegando configuración de NGINX..."
docker stack deploy -c docker-swarm-nginx.yml HCE

# Paso 5: Ejecutar ng serve en el directorio frontCliente
Write-Host "Iniciando servidor Angular en el directorio frontCliente..."
$env:PATH += ";C:\Users\aperalta\AppData\Roaming\npm"
Start-Process -FilePath "cmd.exe" -ArgumentList "/k", "cd /d $PWD\\frontCliente && npx ng serve --ssl --ssl-key ./ssl/server.key --ssl-cert ./ssl/server.crt --port 4200"

# Paso 6: Ejecutar ng serve en el directorio frontMedico
Write-Host "Iniciando servidor Angular en el directorio frontMedico..."
Start-Process -FilePath "cmd.exe" -ArgumentList "/k", "cd /d $PWD\\frontMedico && npx ng serve --ssl --ssl-key ./ssl/server.key --ssl-cert ./ssl/server.crt --port 4201"

# Paso 6: Ejecutar ng serve en el directorio frontAdmin
Write-Host "Iniciando servidor Angular en el directorio frontAdmin..."
Start-Process -FilePath "cmd.exe" -ArgumentList "/k", "cd /d $PWD\\frontAdmin && npx ng serve --ssl --ssl-key ./ssl/server.key --ssl-cert ./ssl/server.crt --port 4202"

Write-Host "Despliegue completado con éxito."
