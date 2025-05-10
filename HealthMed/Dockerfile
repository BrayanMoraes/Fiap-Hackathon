# Etapa base para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ARG PROJECT_PATH
COPY ["$PROJECT_PATH/$PROJECT_PATH.csproj", "$PROJECT_PATH/"]
RUN dotnet restore "$PROJECT_PATH/$PROJECT_PATH.csproj"
COPY . .
WORKDIR "/src/$PROJECT_PATH"
RUN dotnet build -c Release -o /app/build

# Etapa de publicação
FROM build AS publish
WORKDIR "/src/$PROJECT_PATH"
RUN dotnet publish -c Release -o /app/publish

# Etapa final para execução
FROM base AS final
WORKDIR /app
ARG PROJECT_PATH
ENV PROJECT_PATH=${PROJECT_PATH}
COPY --from=publish /app/publish .
ENTRYPOINT dotnet "$PROJECT_PATH.dll"
