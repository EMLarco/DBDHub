# ► DBD Hub - Wiki Interactiva de Dead by Daylight

> Una plataforma centralizada para consultar información actualizada de asesinos, supervivientes, perks y mapas de Dead by Daylight.

![.NET](https://img.shields.io/badge/.NET-10.0-blueviolet?style=flat-square)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-green?style=flat-square)
![License](https://img.shields.io/badge/License-MIT-blue?style=flat-square)

---

## ◆ Problema & Solución

### ► Problema
La información de Dead by Daylight (asesinos, supervivientes, perks, addons, mapas) está dispersa en múltiples fuentes. Los jugadores necesitan un sitio centralizado para tomar decisiones informadas.

### ► Solución
**DBD Hub** ofrece:
- ◆ Catálogo completo y actualizado
- ◆ Información centralizada y de fácil acceso
- ◆ Herramientas para jugadores y creadores de contenido

---

## ► Usuarios Objetivo

- ◆ Jugadores de Dead by Daylight de todos los niveles
- ◆ Creadores de contenido (streamers, YouTubers)
- ◆ Analistas del meta que monitorean tendencias

---

## ◇ Características Principales

| Característica | Descripción |
|---|---|
| ► Catálogo de Asesinos | Listado completo con perks únicos y habilidades |
| ► Catálogo de Supervivientes | Información detallada de cada superviviente |
| ► Catálogo de Mapas | Mapas originales y variantes asociadas |
| ► Gestión de Perks | Base de datos de perks exclusivos |
| ► Gestión de Addons | Addons clasificados por rareza |
| ► Relaciones Muchos-a-Muchos | Mapas vinculados a asesinos correspondientes |

---

## ◇ Stack Tecnológico

| Tecnología | Propósito |
|---|---|
| **ASP.NET Core 10.0** | Framework web MVC |
| **Entity Framework Core** | ORM (Code First) |
| **PostgreSQL** | Base de datos relacional |
| **Npgsql** | Proveedor EF Core para PostgreSQL |
| **Bootstrap 5** | UI/Frontend responsivo |
| **Visual Studio Code** | Editor de desarrollo |

---

## ◆ Estructura del Proyecto

```
DBDHub/
├── Controllers/              # ► Controladores MVC
│   ├── HomeController.cs
│   ├── KillersController.cs
│   └── SurvivorsController.cs
├── Data/                     # ► Contexto de base de datos
│   └── AppDbContext.cs
├── Migrations/               # ► Migraciones EF Core
├── Models/                   # ► Entidades del dominio
│   ├── Killer.cs
│   ├── Survivor.cs
│   ├── Perk.cs
│   ├── Addon.cs
│   ├── Map.cs
│   └── MapKiller.cs
├── ViewModels/               # ► ViewModels
├── Views/                    # ► Vistas Razor
│   ├── Home/
│   ├── Killers/
│   ├── Survivors/
│   └── Shared/
├── Services/                 # ► Lógica de negocio
├── wwwroot/                  # ► Archivos estáticos (CSS, JS)
├── Program.cs                # ► Punto de entrada
├── appsettings.json          # ► Config (NO SUBIR)
├── appsettings.example.json  # ► Plantilla
└── DBDHub.csproj            # ► Configuración del proyecto
```

---

## ◆ Cómo Empezar

### ► Requisitos Previos
- **.NET 10.0 SDK** o superior
- **PostgreSQL 12+** instalado y en ejecución
- **Visual Studio Code** (recomendado)

### ► Instalación

1. **Clonar el repositorio**
```bash
git clone https://github.com/tu-usuario/DBDHub.git
cd DBDHub
```

2. **Restaurar dependencias**
```bash
dotnet restore
```

3. **Configurar la base de datos**
   - Copiar `appsettings.example.json` a `appsettings.json`
   - Actualizar las credenciales de PostgreSQL en `appsettings.json`

```bash
cp appsettings.example.json appsettings.json
```

4. **Ejecutar migraciones**
```bash
dotnet ef database update
```

5. **Ejecutar la aplicación**
```bash
dotnet run
```

La aplicación estará disponible en `https://localhost:5001` (o el puerto configurado)

---

### ► Pasos locales:
1. Copia el archivo `.example` a su forma real
2. Llena con tus credenciales locales
3. Git ignorará automáticamente los cambios

---

## ◆ Estructura de la Base de Datos

Se diseñaron **6 tablas principales** más una tabla de unión para relacionar entidades.

### ► Diagrama de Relaciones
```
Killer (1) ──< Perk (N)
Killer (1) ──< Addon (N)
Survivor (1) ──< Perk (N)
Map (1) ──< Map (N) [auto-referencia para variantes]
Map (N) ── MapKiller ── Killer (N)
```

### ► Detalle de Tablas

**Killer**
| Campo | Tipo | Descripción |
|---|---|---|
| Id | int (PK) | Identificador único |
| Nombre | string(100) | Nombre del asesino |
| Poder | string(200) | Nombre del poder especial |
| Velocidad | decimal | Velocidad base de movimiento |
| Dificultad | string(50) | Fácil, Moderado, Muy Difícil |
| Historia | text | Trasfondo del personaje |
| Activo | bool | Eliminación lógica |
| FechaCreacion | DateTime | Fecha de registro |

**Survivor**
| Campo | Tipo | Descripción |
|---|---|---|
| Id | int (PK) | Identificador único |
| Nombre | string(100) | Nombre del superviviente |
| Historia | text | Trasfondo del personaje |
| Dificultad | string(50) | Fácil, Moderado, Difícil |
| Activo | bool | Eliminación lógica |
| FechaCreacion | DateTime | Fecha de registro |

**Perk**
| Campo | Tipo | Descripción |
|---|---|---|
| Id | int (PK) | Identificador único |
| Nombre | string(100) | Nombre de la habilidad |
| Descripcion | text | Descripción completa |
| Nivel | int | 1, 2 o 3 |
| TipoPersonaje | string(20) | "Killer" o "Survivor" |
| KillerId | int? (FK) | Referencia a Killer |
| SurvivorId | int? (FK) | Referencia a Survivor |
| Activo | bool | Eliminación lógica |
| FechaCreacion | DateTime | Fecha de registro |

**Addon**
| Campo | Tipo | Descripción |
|---|---|---|
| Id | int (PK) | Identificador único |
| Nombre | string(100) | Nombre del addon |
| Rareza | string(50) | Común, Poco Común, Raro, Muy Raro, Ultra Raro |
| Descripcion | text | Efecto del addon |
| KillerId | int (FK) | Referencia a Killer |
| Activo | bool | Eliminación lógica |
| FechaCreacion | DateTime | Fecha de registro |

**Map**
| Campo | Tipo | Descripción |
|---|---|---|
| Id | int (PK) | Identificador único |
| Nombre | string(100) | Nombre del mapa |
| ImagenUrl | string(200) | URL de la imagen |
| Tipo | string(50) | "Original" o "Variante" |
| OriginalMapId | int? (FK) | Referencia al mapa original |
| Activo | bool | Eliminación lógica |
| FechaCreacion | DateTime | Fecha de registro |

**MapKiller** (tabla de unión)
| Campo | Tipo | Descripción |
|---|---|---|
| Id | int (PK) | Identificador único |
| MapId | int (FK) | Referencia a Map |
| KillerId | int (FK) | Referencia a Killer |
| Activo | bool | Eliminación lógica |
| FechaCreacion | DateTime | Fecha de registro |

---

## ◆ Licencia

Este proyecto está bajo la licencia MIT. Ver `LICENSE` para más detalles.

---

**Proyecto realizado para la materia de Desarrollo para Integración de Tecnologías Web**
