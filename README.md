# 📊 DataVision API

A REST API for real-time weather data analysis and reporting, built with ASP.NET Core 8. It provides JWT-based authentication, role-based authorization, weather data retrieval from OpenWeatherMap, report generation, and full API usage auditing.

## 📦 Installation

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB or full instance)
- An [OpenWeatherMap API key](https://openweathermap.org/api)

### Setup

```bash
git clone https://github.com/alessg1414/DataVisionAPI.git
cd DataVisionAPI
```

Configure `appsettings.json` with your connection string and keys:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DataVisionDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key-at-least-32-characters-long",
    "Issuer": "DataVisionAPI",
    "Audience": "DataVisionClients",
    "ExpiryInHours": "24"
  },
  "OpenWeatherMap": {
    "ApiKey": "your-openweathermap-api-key",
    "BaseUrl": "https://api.openweathermap.org/data/2.5/"
  }
}
```

Apply migrations and start the server:

```bash
dotnet ef database update
dotnet run
```

The API will be available at `https://localhost:5001` with Swagger UI at the root (`/`).

## 🛠 Usage

The database is seeded with two default users:

| Username | Password | Role  |
|----------|----------|-------|
| admin    | admin123 | Admin |
| user     | user123  | User  |

1. Open Swagger UI at `https://localhost:5001`
2. Authenticate via `POST /api/auth/login` to get a JWT token
3. Click **Authorize** in Swagger and enter `Bearer {token}`
4. Query weather data, generate reports, or manage users

## ✨ Features

- **JWT authentication** — Login, token validation, and role-based access control (Admin / User)
- **Real-time weather data** — Fetches current conditions from OpenWeatherMap for any city
- **Report generation** — Temperature, humidity, and atmospheric pressure reports with chart-ready data
- **API usage auditing** — Every request is logged with user, endpoint, and timestamp
- **Usage statistics** — Top endpoints, most active users, and daily query counts (Admin only)
- **Interactive docs** — Swagger UI with JWT support built in

## 🧰 Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 8 (.NET 8) |
| Database | SQL Server (LocalDB) |
| ORM | Entity Framework Core 9 |
| Authentication | JWT (System.IdentityModel.Tokens.Jwt) |
| External API | OpenWeatherMap |
| Documentation | Swashbuckle (Swagger / OpenAPI) |
| Serialization | Newtonsoft.Json |

## 📡 API Endpoints

### Authentication (`/api/auth`)

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/auth/login` | Log in and receive JWT token | No |
| `GET` | `/api/auth/validate` | Validate current JWT token | Yes |

### Users (`/api/users`)

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/users` | Create a new user | Admin |
| `GET` | `/api/users/me` | Get current user info | Yes |
| `GET` | `/api/users/{username}` | Get user by username | Admin |
| `PUT` | `/api/users/change-password` | Change password | Yes |

### Weather (`/api/weather`)

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/api/weather/city/{city}` | Get weather for a single city | Yes |
| `GET` | `/api/weather/cities?cities=` | Get weather for multiple cities | Yes |

### Reports (`/api/reports`)

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/api/reports/temperature?cities=` | Temperature report | Yes |
| `GET` | `/api/reports/humidity?cities=` | Humidity report | Yes |
| `GET` | `/api/reports/pressure?cities=` | Atmospheric pressure report | Yes |
| `GET` | `/api/reports/all?cities=` | Combined report (all metrics) | Yes |

### Logs (`/api/logs`)

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/api/logs` | Get all system logs | Admin |
| `GET` | `/api/logs/my-logs` | Get current user's logs | Yes |
| `GET` | `/api/logs/user/{userId}` | Get logs for a specific user | Admin |
| `GET` | `/api/logs/statistics` | API usage statistics | Admin |

## 📄 License

MIT License

## 🙌 Credits

- [OpenWeatherMap](https://openweathermap.org/) for weather data
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) for Swagger integration
- [Entity Framework Core](https://github.com/dotnet/efcore) for data access
