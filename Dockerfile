# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia el archivo de solución
COPY ["Farmacia.sln", "./"]

# Copia todos los archivos .csproj
COPY ["Farmacia.Application/*.csproj", "Farmacia.Application/"]
COPY ["Farmacia.Core/*.csproj", "Farmacia.Core/"]
COPY ["Farmacia.Infrastructure/*.csproj", "Farmacia.Infrastructure/"]
COPY ["Farmacia.Validations/*.csproj", "Farmacia.Validations/"]
COPY ["Farmacia.Tests/*.csproj", "Farmacia.Tests/"]

# Restaura las dependencias usando la solución
RUN dotnet restore "Farmacia.sln"

# Copia todo el código fuente
COPY . .

# Publica solo el proyecto Application
WORKDIR "/src/Farmacia.Application"
RUN dotnet publish "Farmacia.Application.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage (más ligero)
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Render asigna el puerto dinámicamente con la variable $PORT
ENV ASPNETCORE_URLS=http://+:$PORT
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Farmacia.Application.dll"]