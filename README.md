# DBD Hub

> Wiki interactiva y panel de gestion para Dead by Daylight, construida con ASP.NET Core MVC, Entity Framework Core y PostgreSQL.

![.NET](https://img.shields.io/badge/.NET-10.0-blueviolet?style=flat-square)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC-blue?style=flat-square)
![Entity Framework Core](https://img.shields.io/badge/EF_Core-Code_First-7f52ff?style=flat-square)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Database-316192?style=flat-square)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat-square)
![Identity](https://img.shields.io/badge/Identity-Auth-orange?style=flat-square)

---

## Overview

DBD Hub centraliza informacion de personajes, perks, addons, mapas, partidas y reportes en una sola aplicacion web.

El objetivo del proyecto es ofrecer una experiencia clara, ordenada y responsive para consultar y administrar el contenido del universo de Dead by Daylight.

---

## Highlights

- Gestion completa de killers, survivors, perks, addons y maps
- Modulo de matches para registrar y consultar partidas
- Reportes y vistas de resumen para analisis rapido
- Autenticacion con login y registro de usuarios
- Arquitectura MVC con capas separadas por responsabilidad
- Persistencia con Entity Framework Core y PostgreSQL

---

## Tech Stack

| Tecnologia | Uso |
|---|---|
| ASP.NET Core 10 | Framework web MVC |
| Entity Framework Core | ORM Code First |
| PostgreSQL | Base de datos relacional |
| Npgsql | Provider para PostgreSQL |
| ASP.NET Core Identity | Autenticacion y autorizacion |
| Bootstrap 5 | Interfaz responsiva |

---

## Main Modules

| Modulo | Descripcion |
|---|---|
| Killers | Gestion de asesinos, detalles y relacion con perks/addons |
| Survivors | Administracion de supervivientes y sus perks |
| Perks | Registro y visualizacion de habilidades |
| Addons | Catalogo de complementos asociados a killers |
| Maps | Mapa principal y variantes |
| Matches | Registro y consulta de partidas |
| Reports | Resumenes y metricas del sistema |
| Account | Login y registro de usuarios |

---

## Project Structure

```text
DBDHub/
|-- Controllers/             Controladores MVC
|-- Data/                    Contexto de base de datos
|-- Migrations/              Migraciones de EF Core
|-- Models/                  Entidades del dominio
|-- Services/                Logica de negocio
|-- ViewModels/              Modelos para vistas
|-- Views/                   Vistas Razor
|-- wwwroot/                 Archivos estaticos
|-- Program.cs               Punto de entrada
|-- appsettings.json         Configuracion local
|-- appsettings.example.json Plantilla de configuracion
|-- DBDHub.csproj            Configuracion del proyecto
```

---

## Local Setup

### Prerequisites

- .NET 10 SDK
- PostgreSQL 12 o superior
- Visual Studio, Visual Studio Code o cualquier editor compatible

### Run locally

1. Clonar el repositorio

```bash
git clone https://github.com/EMLarco/DBDHub.git
cd DBDHub
```

2. Restaurar dependencias

```bash
dotnet restore
```

3. Configurar la cadena de conexion

```bash
cp appsettings.example.json appsettings.json
```

Edita `appsettings.json` y coloca tus credenciales de PostgreSQL.

4. Aplicar migraciones

```bash
dotnet ef database update
```

5. Ejecutar la aplicacion

```bash
dotnet run
```

La aplicacion quedara disponible en la direccion local que muestre la consola, normalmente `https://localhost:5001`.

---

## Configuration Example

`appsettings.example.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=DBDHubDB;Username=your_username;Password=your_password"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## Notes

- No subas `appsettings.json` con credenciales reales.
- Si cambias el nombre de la base o el usuario, actualiza la cadena de conexion.
- El proyecto usa Identity para autenticar usuarios.

---

## Proyecto academico

Proyecto desarrollado para la materia de Desarrollo para Integracion de Tecnologias Web.
