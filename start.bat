@echo off
cd C:\Repos\chatbotapi

:: Instalar dependencias de Node si no existen
IF NOT EXIST "node_modules" (
    echo 📦 Instalando dependencias Node...
    npm install
)

start "" cmd /k "docker compose up"
start "" cmd /k "node webhook.js"

cd C:\Repos\
start "" cmd /k "ngrok http 3001"

:: Esperar 60 segundos antes de lanzar updatewebhook
timeout /t 60 /nobreak > NUL

cd C:\Repos\chatbotapi
start "" cmd /k "node updatewebhook.js"