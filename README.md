# 🚗 Alquiler de Vehículos - API REST + Blazor

Sistema integral de gestión de alquiler de vehículos desarrollado con **.NET 9**, **Dapper**, **Blazor** y **MySQL**. Arquitectura limpia con separación clara de capas y patrones de diseño profesionales.

[![.NET Version](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Repository](https://img.shields.io/badge/repository-github-green.svg)](https://github.com/MiguelDV84/AlquilerVehiculos-DAPPER-BLAZOR)

---

## 📋 Tabla de Contenidos

- [🎯 Descripción General](#-descripción-general)
- [✨ Características](#-características)
- [🏗️ Arquitectura](#-arquitectura)
- [🛠️ Tecnologías](#-tecnologías)
- [📁 Estructura del Proyecto](#-estructura-del-proyecto)
- [📋 Requisitos Previos](#-requisitos-previos)
- [⚙️ Instalación y Configuración](#-instalación-y-configuración)
- [🚀 Ejecución](#-ejecución)
- [📚 Documentación de la API](#-documentación-de-la-api)
  - [🚗 Endpoints de Vehículos](#-endpoints-de-vehículos)
  - [🔑 Endpoints de Autenticación](#-endpoints-de-autenticación)
  - [📋 Endpoints de Alquileres](#-endpoints-de-alquileres)
- [📊 Enumeraciones](#-enumeraciones)
- [🚦 Códigos de Estado HTTP](#-códigos-de-estado-http)
- [⚠️ Manejo de Errores](#-manejo-de-errores)
- [🗄️ Migraciones de Base de Datos](#-migraciones-de-base-de-datos)
- [👨‍💻 Guía de Desarrollo](#-guía-de-desarrollo)
- [🤝 Contribución](#-contribución)
- [📄 Licencia](#-licencia)

---

## 🎯 Descripción General

**Alquiler de Vehículos** es una solución empresarial completa para gestionar el alquiler de vehículos. Proporciona:

- **API REST** robusta y escalable basada en Minimal APIs de ASP.NET Core
- **Frontend Blazor** moderno y reactivo
- **Base de datos MySQL** con migraciones automáticas
- **Autenticación JWT** segura
- **Manejo centralizado de excepciones** con respuestas consistentes

---

## ✨ Características

### 🎯 Funcionalidades Principales

- ✅ **Gestión de Vehículos**: CRUD completo con validaciones robustas
- ✅ **Sistema de Alquileres**: Crear, finalizar y consultar alquileres
- ✅ **Autenticación JWT**: Sistema de login/registro seguro con roles
- ✅ **Validaciones Inteligentes**: DataAnnotations + lógica de negocio
- ✅ **Manejo Global de Errores**: Respuestas JSON consistentes
- ✅ **Interfaz Blazor**: Aplicación web moderna y responsiva
- ✅ **Aplicación Móvil**: .NET MAUI para iOS y Android

### 🏗️ Características Técnicas

- 🎯 **Clean Architecture**: Separación por capas (Core, Application, Infrastructure, Presentation)
- 🚀 **Dapper ORM**: Alto rendimiento en acceso a datos
- 🔄 **EF Core Migrations**: Gestión automática del esquema de BD
- 📦 **Repository Pattern**: Abstracción del acceso a datos
- 🔧 **Unit of Work**: Gestión de transacciones
- 🗺️ **AutoMapper**: Mapeo automático entre DTOs y entidades
- 🛡️ **Global Exception Handler**: Manejo centralizado de excepciones
- 📝 **Swagger/OpenAPI**: Documentación interactiva de la API

---

## 🏗️ Arquitectura

El proyecto sigue los principios de **Clean Architecture** con separación clara de responsabilidades:

```plaintext
┌────────────────────────────────────────────────┐
│          Presentation Layer                    │
│  (Endpoints, Middleware, Controllers)          │
│  ↓ Recibe requests, valida entrada            │
└────────────────────┬─────────────────────────┘
                     │
┌────────────────────▼─────────────────────────┐
│          Application Layer                    │
│  (Services, DTOs, Mappings, Validators)      │
│  ↓ Lógica de negocio y reglas                │
└────────────────────┬─────────────────────────┘
                     │
┌────────────────────▼─────────────────────────┐
│        Infrastructure Layer                   │
│  (Repositories, Data Access, EF Context)     │
│  ↓ Acceso a datos y persistencia             │
└────────────────────┬─────────────────────────┘
                     │
┌────────────────────▼─────────────────────────┐
│            Core Layer                        │
│  (Entities, Interfaces, Exceptions, Enums)   │
│  ↓ Definiciones del dominio                  │
└────────────────────────────────────────────────┘
```

### Flujo de Petición

```plaintext
Request HTTP
    ↓
┌─ GlobalExceptionHandler (captura excepciones)
│
Endpoint (Minimal API)
    ↓
Service (Lógica de negocio)
    ↓
Repository (Dapper Query)
    ↓
MySQL Database
    ↓
Response JSON (ApiResponse<T>)
```

---

## 🛠️ Tecnologías

### Backend (API)

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| .NET | 9.0 | Framework principal |
| ASP.NET Core | 9.0 | Web API y Minimal APIs |
| Dapper | 2.1.66 | Micro-ORM para acceso a datos |
| Entity Framework Core | 9.0.13 | Migraciones de esquema |
| MySQL | 8.0+ | Base de datos relacional |
| Pomelo.EntityFrameworkCore.MySql | 9.0.0 | Provider de MySQL |
| AutoMapper | 16.0.0 | Mapeo automático de objetos |
| BCrypt.Net | 4.1.0 | Hash y validación de contraseñas |
| JWT Bearer | 9.0.13 | Autenticación basada en tokens |

### Frontend

| Tecnología | Propósito |
|------------|-----------|
| Blazor Web App | Interfaz web interactiva |
| .NET MAUI | Aplicaciones móviles (iOS/Android) |
| HttpClient | Consumo de API |

---

## 📁 Estructura del Proyecto

```plaintext
📦 AlquilerVehiculos-DAPPER-API/
│
├── 📂 WebApiNet/                              # Proyecto API Principal
│   ├── 📂 Core/                               # Capa de Dominio
│   │   ├── 📂 Entities/
│   │   │   ├── Vehiculo.cs
│   │   │   ├── Alquiler.cs
│   │   │   └── Cliente.cs
│   │   ├── 📂 Interfaces/
│   │   │   ├── IVehiculoService.cs
│   │   │   ├── IAlquilerService.cs
│   │   │   └── IAuthService.cs
│   │   ├── 📂 Enums/
│   │   │   ├── TipoVehiculo.cs
│   │   │   ├── EstadoVehiculo.cs
│   │   │   └── Role.cs
│   │   └── 📂 Exceptions/
│   │       ├── NotFoundException.cs
│   │       ├── DuplicateEntityException.cs
│   │       └── ValidationException.cs
│   │
│   ├── 📂 Application/                        # Capa de Aplicación
│   │   ├── 📂 Services/
│   │   │   ├── VehiculoService.cs
│   │   │   ├── AlquilerService.cs
│   │   │   └── AuthService.cs
│   │   ├── 📂 DTOs/
│   │   │   ├── 📂 Vehiculo/
│   │   │   ├── 📂 Alquiler/
│   │   │   └── 📂 Auth/
│   │   └── 📂 Mappings/
│   │       └── MappingProfile.cs
│   │
│   ├── 📂 Infrastructure/                     # Capa de Infraestructura
│   │   ├── 📂 Data/
│   │   │   └── ApplicationDbContext.cs
│   │   ├── 📂 Repositories/
│   │   │   ├── 📂 Vehiculo/
│   │   │   ├── 📂 Alquiler/
│   │   │   └── 📂 UnitOfWork/
│   │   └── 📂 DependencyInjection/
│   │       └── ServiceRegistration.cs
│   │
│   ├── 📂 Presentation/                       # Capa de Presentación
│   │   ├── 📂 Endpoints/
│   │   │   ├── VehiculoEndpoints.cs
│   │   │   ├── AlquilerEndpoints.cs
│   │   │   └── AuthEndpoints.cs
│   │   └── 📂 Middleware/
│   │       └── GlobalExceptionHandler.cs
│   │
│   ├── 📂 Database/
│   │   ├── 📂 Migrations/
│   │   └── 📂 StoredProcedures/
│   │
│   ├── Program.cs
│   ├── appsettings.json
│   └── WebApiNet.csproj
│
├── 📂 WebApiNet.Shared/                       # DTOs y Enums Compartidos
│   ├── 📂 DTOs/
│   │   ├── 📂 Vehiculo/
│   │   ├── 📂 Alquiler/
│   │   └── 📂 Common/
│   └── 📂 Enums/
│
└── 📂 AlquilerVehiculosWeb/                   # Cliente Blazor
    ├── 📂 AlquilerVehiculosWeb.Web/
    │   ├── 📂 Pages/
    │   │   ├── Flota.razor
    │   │   └── Alquileres.razor
    │   ├── 📂 Services/
    │   └── Program.cs
    └── 📂 AlquilerVehiculosWeb.Shared/
```

---

## 📋 Requisitos Previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (v9.0.0 o superior)
- [MySQL Server](https://dev.mysql.com/downloads/) (v8.0+)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (v17.12+) o [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)
- [Postman](https://www.postman.com/downloads/) (opcional, para testing)

---

## ⚙️ Instalación y Configuración

### 1️⃣ Clonar el Repositorio

```bash
git clone https://github.com/MiguelDV84/AlquilerVehiculos-DAPPER-BLAZOR.git
cd AlquilerVehiculos-DAPPER-API
```

### 2️⃣ Restaurar Dependencias

```bash
dotnet restore
```

### 3️⃣ Configurar Base de Datos

```sql
CREATE DATABASE alquiler_vehiculos CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 4️⃣ Actualizar appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=alquiler_vehiculos;User=root;Password=tu_password;"
  },
  "Jwt": {
    "Key": "tu_clave_secreta_super_segura_de_al_menos_32_caracteres",
    "Issuer": "WebApiNet",
    "Audience": "BlazorClient"
  }
}
```

### 5️⃣ Aplicar Migraciones

```bash
cd WebApiNet
dotnet ef database update
```

---

## 🚀 Ejecución

### Ejecutar la API

```bash
cd WebApiNet
dotnet run
```

**URLs disponibles:**

- 🌐 **HTTP**: `http://localhost:5276`
- 🔒 **HTTPS**: `https://localhost:7267`
- 📚 **Swagger**: `https://localhost:7267/swagger`

### Ejecutar Blazor

```bash
cd AlquilerVehiculosWeb/AlquilerVehiculosWeb.Web
dotnet run
```

**URLs disponibles:**

- 🌐 **HTTP**: `http://localhost:5001`
- 🔒 **HTTPS**: `https://localhost:7001`

---

## 📚 Documentación de la API

### Base URL

```
https://localhost:7267/api
```

### Formato de Respuesta Estándar

```json
{
    "success": true | false,
    "message": "Mensaje descriptivo",
    "timestamp": "2026-03-02",
    "data": { ... }
}
```

---

## 🚗 Endpoints de Vehículos

### 1. Crear Vehículo

```http
POST /api/vehiculos
Content-Type: application/json

{
    "matricula": "1234BBB",
    "tipoVehiculo": 0,
    "kilometraje": 0,
    "marca": "Toyota",
    "modelo": "Corolla",
    "precio": 25000.00,
    "litrosTanque": 50.0
}
```

**Response (201 Created):**

```json
{
    "success": true,
    "message": "Vehículo creado exitosamente",
    "timestamp": "2026-03-02",
    "data": {
        "matricula": "1234BBB",
        "tipoVehiculo": 0,
        "marca": "Toyota",
        "modelo": "Corolla",
        "precio": 25000.00,
        "estado": 0
    }
}
```

**Validaciones:**

- Matrícula: Formato `^[0-9]{4}[BCDFGHJKLMNPQRSTVWXYZ]{3}$`
- TipoVehiculo: 0-5
- Precio: > 0

---

### 2. Obtener Todos los Vehículos

```http
GET /api/vehiculos
```

**Response (200 OK):**

```json
{
    "success": true,
    "message": "Vehículos obtenidos exitosamente",
    "timestamp": "2026-03-02",
    "data": [
        {
            "matricula": "1234BBB",
            "tipoVehiculo": 0,
            "marca": "Toyota",
            "modelo": "Corolla",
            "precio": 25000.00,
            "estado": 0
        },
        {
            "matricula": "5678CCC",
            "tipoVehiculo": 1,
            "marca": "Ford",
            "modelo": "Ranger",
            "precio": 35000.00,
            "estado": 1
        }
    ]
}
```

---

### 3. Obtener Vehículo por Matrícula

```http
GET /api/vehiculos/{matricula}
```

**Ejemplo:**

```http
GET /api/vehiculos/1234BBB
```

**Response (200 OK):**

```json
{
    "success": true,
    "message": "Vehículo obtenido correctamente",
    "timestamp": "2026-03-02",
    "data": {
        "matricula": "1234BBB",
        "tipoVehiculo": 0,
        "marca": "Toyota",
        "modelo": "Corolla",
        "precio": 25000.00,
        "estado": 0
    }
}
```

**Errores Posibles:**

| Código | Error |
|--------|-------|
| 404 | Vehículo no encontrado |
| 500 | Error del servidor |

---

### 4. Actualizar Vehículo

```http
PUT /api/vehiculos/{matricula}
Content-Type: application/json

{
    "kilometraje": 15000,
    "modelo": "Corolla GLI",
    "precio": 28000.00
}
```

**Response (200 OK):**

```json
{
    "success": true,
    "message": "Vehículo actualizado exitosamente",
    "timestamp": "2026-03-02",
    "data": {
        "matricula": "1234BBB",
        "kilometraje": 15000,
        "modelo": "Corolla GLI",
        "precio": 28000.00
    }
}
```

**Nota:** Todos los campos son opcionales.

---

### 5. Eliminar Vehículo

```http
DELETE /api/vehiculos/{matricula}
```

**Ejemplo:**

```http
DELETE /api/vehiculos/1234BBB
```

**Response (200 OK - Éxito):**

```json
{
    "success": true,
    "message": "Vehículo eliminado correctamente",
    "timestamp": "2026-03-02",
    "data": {
        "matricula": "1234BBB"
    }
}
```

**Errores Posibles:**

| Código | Error |
|--------|-------|
| 404 | Vehículo no encontrado |
| 409 | No se puede eliminar (tiene alquileres) |

---

## 🔑 Endpoints de Autenticación

### Login

```http
POST /api/auth/login
Content-Type: application/json

{
    "email": "usuario@example.com",
    "password": "Password123!"
}
```

### Registro

```http
POST /api/auth/register
Content-Type: application/json

{
    "dni": "12345678A",
    "nombre": "Juan Pérez",
    "email": "usuario@example.com",
    "password": "Password123!"
}
```

---

## 📋 Endpoints de Alquileres

### Crear Alquiler

```http
POST /api/alquileres
Authorization: Bearer <token>
```


### Obtener Alquileres

```http
GET /api/alquileres
Authorization: Bearer <token>
```

### Finalizar Alquiler

```http
PUT /api/alquileres/{vehiculoMatricula}/finalizar
Authorization: Bearer <token>
```

---

## 📊 Enumeraciones

### TipoVehiculo

```
0 = Furgoneta
1 = Camión
2 = Turismo
3 = Deportivo
4 = Monovolumen
5 = Caravana
```

### EstadoVehiculo

```
0 = Disponible
1 = Alquilado
2 = Mantenimiento
3 = NoDisponible
```

### Role

```
0 = User
1 = Admin
```

---

## 🚦 Códigos de Estado HTTP

| Código | Descripción |
|--------|-------------|
| 200 | OK |
| 201 | Created |
| 400 | Bad Request |
| 401 | Unauthorized |
| 404 | Not Found |
| 409 | Conflict |
| 500 | Internal Server Error |

---

## ⚠️ Manejo de Errores

Todos los errores devuelven:

```json
{
    "success": false,
    "message": "Descripción del error",
    "timestamp": "2026-03-02",
    "data": null
}
```

---

## 🗄️ Migraciones de Base de Datos

### Crear Migración

```bash
cd WebApiNet
dotnet ef migrations add NombreMigracion --output-dir Database/Migrations
```

### Aplicar Migraciones

```bash
dotnet ef database update
```

---

## 👨‍💻 Guía de Desarrollo

Ver [DEVELOPMENT.md](DEVELOPMENT.md) para instrucciones detalladas sobre cómo agregar nuevas funcionalidades.

---

## 🤝 Contribución

1. Fork el proyecto
2. Crea una rama (`git checkout -b feature/MiFeature`)
3. Commit cambios (`git commit -m 'Add MiFeature'`)
4. Push (`git push origin feature/MiFeature`)
5. Abre un Pull Request

---

## 📄 Licencia

MIT License - ver [LICENSE](LICENSE)

---

## ⭐ Si te ha sido útil, ¡dale una estrella en GitHub!
