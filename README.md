# 🚗 Cars Catalog API 

Una API RESTful desarrollada en **ASP.NET Core** para gestionar un catálogo de automóviles. Este proyecto utiliza arquitectura por capas (Controller, Infraestructura, Modelos, Repositorio) y se conecta a una base de datos SQL mediante **Entity Framework Core**.

Incluye:
- Autenticación y autorización (si decides añadirla)
- Validaciones
- Logs con middleware
- Manejo de errores personalizado
- Filtros avanzados
- Pruebas unitarias
- Dockerización

---

## 📦 Tecnologías

- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- Docker
- xUnit
- Serilog (para logs)
- Swagger (documentación interactiva)

---

## 📁 Estructura del Proyecto

CarsCatalog2/ │ 

                ├── Controllers/ # Controladores de API 

                ├── Models/ # Entidades (Car, Brand) 

                ├── Infrastructure/ # Interfaces y repositorios 

                ├── Data/ # DbContext y configuraciones 

                ├── Middleware/ # Middleware para logs y errores 

                ├── Tests/ # Pruebas unitarias (xUnit) 

                ├── Dockerfile # Dockerfile del contenedor 

                ├─README.md 

---

## 📌 Endpoints Principales

| Método | Ruta                      | Descripción                                 |
|--------|---------------------------|---------------------------------------------|
| GET    | `/api/cars`               | Listar todos los autos con paginación       |
| GET    | `/api/cars/{id}`          | Obtener un auto por ID                      |
| POST   | `/api/cars`               | Crear un nuevo auto                         |
| GET    | `/api/cars/filter`        | Filtrar por modelo, precio y kilometraje    |

---

## 🚀 Cómo Ejecutar el Proyecto Localmente

### 1. Clonar el repositorio

# bash
git clone https://github.com/Kexxx04/CarsCatalogAPI.git
cd CarsCatalog2

### 2. Configurar la base de datos

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=CarsDb;Trusted_Connection=True;"
}

### 3. Ejecutar migraciones

# bash
dotnet ef database update

### 4. Ejecutar migraciones

# bash
dotnet run --urls http://localhost:8020

### 5. Ver la documentación Swagger

http://localhost:8020/swagger

---

## 🐳 Uso con Docker

### 1. Construir la imagen

# bash
docker build -t cars-api .

### 2. Levantar el contenedor

# bash
docker run -d -p 8020:8020 --name cars-api cars-api

---

## 🔍 Logs y Middleware

Middleware de logging personalizado para registrar todas las solicitudes al endpoint /api/cars.

Manejo robusto de excepciones con mensajes HTTP claros y específicos.

Validación de entrada en el modelo Car, incluyendo:

Campos requeridos

Longitud máxima

Protección contra SQL Injection (ORM)

---

## 🧪 Pruebas

Pruebas unitarias implementadas con xUnit.

Al menos el 50% de cobertura sobre la lógica del repositorio y controlador.

# bash
dotnet test

---

## 📚 Documentación de la API

Toda la documentación está disponible en Swagger en:
http://localhost:8020/swagger
