# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia el archivo de solución
COPY ["Farmacia.sln", "./"]

# Copia TODOS los archivos .csproj
COPY ["Farmacia/*.csproj", "Farmacia/"]
COPY ["Farmacia.Application/*.csproj", "Farmacia.Application/"]
COPY ["Farmacia.Core/*.csproj", "Farmacia.Core/"]
COPY ["Farmacia.Infrastructure/*.csproj", "Farmacia.Infrastructure/"]
COPY ["Farmacia.Validations/*.csproj", "Farmacia.Validations/"]
COPY ["Farmacia.Tests/*.csproj", "Farmacia.Tests/"]

# Restaura las dependencias
RUN dotnet restore "Farmacia.sln"

# Copia todo el código fuente
COPY . .

# Publica el proyecto Farmacia (el API principal)
WORKDIR "/src/Farmacia"
RUN dotnet publish "Farmacia.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:$PORT
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Farmacia.Api.dll"]