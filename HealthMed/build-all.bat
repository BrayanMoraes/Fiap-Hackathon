@echo off

:: Define os nomes dos projetos e as tags das imagens (em letras minúsculas)
set projects=HealthMed.API HealthMed.AgendaConsumer HealthMed.ConsultaAgendadaConsumer
set tags=healthmed/api:latest healthmed/agenda-consumer:latest healthmed/consulta-agendada-consumer:latest

:: Caminho do Dockerfile (ajuste se necessário)
set DOCKERFILE_PATH=.\Dockerfile

:: Converte as variáveis em arrays
setlocal enabledelayedexpansion
set i=0

:: Loop para construir cada projeto
for %%p in (%projects%) do (
    for /f "tokens=1,* delims= " %%t in ("!tags!") do (
        set tag=%%t
        set tags=!tags:%%t =!
        echo Building Docker image for %%p with tag !tag!...
        docker build --no-cache -t !tag! --build-arg PROJECT_PATH=%%p -f %DOCKERFILE_PATH% .
        if errorlevel 1 (
            echo Failed to build %%p. Exiting...
            pause
            exit /b 1
        )
        echo Successfully built !tag!
        set /a i+=1
    )
)

echo All images built successfully!
pause