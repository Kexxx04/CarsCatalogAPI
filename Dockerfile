# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo .csproj y restaurar dependencias
COPY CarsCatalog2.csproj ./
RUN dotnet restore

# Copiar el resto del código y publicar
COPY . .
RUN dotnet publish CarsCatalog2.csproj -c Release -o /app/publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Puerto en el que corre tu API
EXPOSE 8020
ENTRYPOINT ["dotnet", "CarsCatalog2.dll"]
