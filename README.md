# DataVision API

API REST para sistema de análisis y reportes con datos meteorológicos en tiempo real, construida con ASP.NET Core 8.

## Tecnologías

- **Framework:** ASP.NET Core 8 (.NET 8)
- **Base de datos:** SQL Server (LocalDB)
- **ORM:** Entity Framework Core 9
- **Autenticación:** JWT (JSON Web Tokens)
- **Documentación:** Swagger / OpenAPI
- **API externa:** OpenWeatherMap

## Características

- Autenticación y autorización basada en roles (Admin / User)
- Consulta de datos meteorológicos en tiempo real
- Generación de reportes de temperatura, humedad y presión atmosférica
- Sistema de logs y auditoría de uso de la API
- Estadísticas de uso (endpoints más consultados, usuarios más activos)
- Documentación interactiva con Swagger UI

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB o instancia completa)
- API Key de [OpenWeatherMap](https://openweathermap.org/api)

## Configuración

1. **Clonar el repositorio**

   ```bash
   git clone https://github.com/alessg1414/DataVisionAPI.git
   cd DataVisionAPI
   ```

2. **Configurar `appsettings.json`**

   Actualizar la cadena de conexión y las claves:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DataVisionDB;Trusted_Connection=true;MultipleActiveResultSets=true"
     },
     "JwtSettings": {
       "SecretKey": "tu-clave-secreta-de-al-menos-32-caracteres",
       "Issuer": "DataVisionAPI",
       "Audience": "DataVisionClients",
       "ExpiryInHours": "24"
     },
     "OpenWeatherMap": {
       "ApiKey": "tu-api-key-de-openweathermap",
       "BaseUrl": "https://api.openweathermap.org/data/2.5/"
     }
   }
   ```

3. **Aplicar migraciones y crear la base de datos**

   ```bash
   dotnet ef database update
   ```

4. **Ejecutar la aplicación**

   ```bash
   dotnet run
   ```

   La API estará disponible en `https://localhost:5001` y Swagger UI se mostrará en la raíz (`/`).

## Usuarios por defecto

| Usuario | Contraseña | Rol   |
|---------|------------|-------|
| admin   | admin123   | Admin |
| user    | user123    | User  |

## Endpoints

### Autenticación (`/api/auth`)

| Método | Ruta               | Descripción                  | Auth |
|--------|--------------------|------------------------------|------|
| POST   | `/api/auth/login`    | Iniciar sesión               | No   |
| GET    | `/api/auth/validate` | Validar token JWT            | Sí   |

### Usuarios (`/api/users`)

| Método | Ruta                          | Descripción              | Auth  |
|--------|-------------------------------|--------------------------|-------|
| POST   | `/api/users`                  | Crear usuario            | Admin |
| GET    | `/api/users/me`               | Obtener usuario actual   | Sí    |
| GET    | `/api/users/{username}`       | Obtener usuario por nombre | Admin |
| PUT    | `/api/users/change-password`  | Cambiar contraseña       | Sí    |

### Clima (`/api/weather`)

| Método | Ruta                        | Descripción                      | Auth |
|--------|-----------------------------|----------------------------------|------|
| GET    | `/api/weather/city/{city}`  | Clima de una ciudad              | Sí   |
| GET    | `/api/weather/cities?cities=` | Clima de múltiples ciudades    | Sí   |

### Reportes (`/api/reports`)

| Método | Ruta                             | Descripción                  | Auth |
|--------|----------------------------------|------------------------------|------|
| GET    | `/api/reports/temperature?cities=` | Reporte de temperatura     | Sí   |
| GET    | `/api/reports/humidity?cities=`    | Reporte de humedad         | Sí   |
| GET    | `/api/reports/pressure?cities=`    | Reporte de presión atmosférica | Sí |
| GET    | `/api/reports/all?cities=`         | Reporte combinado          | Sí   |

### Logs (`/api/logs`)

| Método | Ruta                        | Descripción                          | Auth  |
|--------|-----------------------------|--------------------------------------|-------|
| GET    | `/api/logs`                 | Todos los logs del sistema           | Admin |
| GET    | `/api/logs/my-logs`         | Logs del usuario autenticado         | Sí    |
| GET    | `/api/logs/user/{userId}`   | Logs de un usuario específico        | Admin |
| GET    | `/api/logs/statistics`      | Estadísticas de uso de la API        | Admin |

## Estructura del proyecto

```
DataVisionAPI/
├── Controllers/          # Controladores de la API
│   ├── AuthController.cs
│   ├── LogsController.cs
│   ├── ReportsController.cs
│   ├── UsersController.cs
│   └── WeatherController.cs
├── Data/
│   └── ApplicationDbContext.cs   # Contexto de Entity Framework
├── Models/
│   ├── Log.cs                    # Entidad de log
│   ├── Usuario.cs                # Entidad de usuario
│   └── DTOs/                     # Objetos de transferencia de datos
│       ├── AuthResponseDto.cs
│       ├── ChangePasswordDto.cs
│       ├── CreateUserDto.cs
│       ├── LoginDto.cs
│       ├── ReportDto.cs
│       └── WeatherDataDto.cs
├── Services/                     # Capa de servicios (lógica de negocio)
│   ├── AuthService.cs
│   ├── LogService.cs
│   ├── ReportService.cs
│   ├── UserService.cs
│   └── WeatherService.cs
├── Program.cs                    # Configuración de la aplicación
└── appsettings.json              # Configuración general
```
