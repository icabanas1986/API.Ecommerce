# Imagen base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar SOLO el csproj primero (mejora cache)
COPY API.Ecommerce.csproj ./
RUN dotnet restore

# Copiar el resto del código
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "API.Ecommerce.dll"]

