# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

VetCare Veterinary Management System - A Windows Forms application (.NET Framework 4.7.2) for managing veterinary clinic operations with a robust security subsystem using the Composite pattern for permissions and roles.

**Note:** The solution file is named "Sistema Biblioteca Escolar.sln" but this is actually a Veterinary Management System (VetCare). The database is "SeguridadBiblioteca" (legacy naming).

## Build and Run Commands

### Build the Solution
```bash
msbuild "Sistema Biblioteca Escolar.sln" /p:Configuration=Debug
```

### Run the Application
```bash
UI\bin\Debug\UI.exe
```

### Database Setup

**Initial Installation (Run once):**
```bash
sqlcmd -S localhost -i "Database\00_EJECUTAR_TODO.sql"
```

**Step-by-step Installation:**
```bash
sqlcmd -S localhost -i "Database\01_CrearBaseDatos.sql"
sqlcmd -S localhost -i "Database\02_CrearTablas.sql"
sqlcmd -S localhost -i "Database\03_DatosIniciales.sql"
```

**Default Admin Credentials:**
- Username: `admin`
- Password: `admin123`
- Email: `admin@biblioteca.edu`

**Connection String:** Located in `UI/App.config`, modify the `Data Source` to match your SQL Server instance.

## Architecture

### Layered Architecture (Model-View-Controller variant)

The solution follows a layered architecture with clear separation of concerns:

```
UI (View Layer)
  └── Windows Forms - User interface
      ├── Login.cs
      ├── Administración/ - Admin forms (gestionUsuarios, gestionPermisos, menu, gestionCatalogo)
      └── Transacciones/ - Business transaction forms

ServicesSecurity (Security Module) - Complete security subsystem
  ├── BLL/ - Business logic for security
  │   ├── UsuarioBLL.cs - User management logic
  │   ├── FamiliaBLL.cs - Role/permission group logic
  │   └── ValidationBLL.cs - Business validation rules
  ├── DAL/ - Data access layer for security
  │   ├── Contracts/ - Repository interfaces
  │   ├── Implementations/ - Concrete repositories
  │   │   ├── UsuarioRepository.cs
  │   │   ├── FamiliaRepository.cs
  │   │   ├── PatenteRepository.cs
  │   │   └── Adapter/ - Entity-to-model adapters
  │   └── Tools/ - Database utilities
  ├── DomainModel/Security/Composite/ - Security domain entities
  │   ├── Component.cs - Base class (Composite pattern)
  │   ├── Familia.cs - Composite (roles/permission groups)
  │   ├── Patente.cs - Leaf (atomic permissions)
  │   └── Usuario.cs - User entity
  └── Services/ - Cross-cutting services
      ├── CryptographyService.cs - SHA256 hashing (uses Encoding.Unicode)
      ├── LanguageManager.cs - i18n support
      ├── LoggerService.cs - Application logging
      └── ExceptionManager.cs - Exception handling

BLL/ - Business logic layer (future business modules)
DAL/ - Data access layer (future business modules)
DomainModel/ - Domain entities (future business modules)
Services/ - Application services (future business modules)

Database/ - SQL scripts for database setup
```

### Key Architectural Patterns

**1. Composite Pattern for Permissions (Critical)**

The security system uses the Composite design pattern to build hierarchical permission structures:

- `Component` (abstract base): Interface for both `Familia` and `Patente`
- `Familia` (Composite): Container that can hold other Familias or Patentes
- `Patente` (Leaf): Atomic permission (e.g., "Alta de Usuario", "Ver Logs")

**Roles as Special Familias:**
- Roles are Familias with names starting with `ROL_` (e.g., `ROL_Administrador`, `ROL_Veterinario`)
- `Familia.EsRol` property identifies if a Familia is a role
- `Usuario.Permisos` contains a list of Components (Familias and Patentes)
- To get a user's role, use `usuario.ObtenerFamiliaRol()` or `usuario.ObtenerNombreRol()`

**Hierarchy Example:**
```
ROL_Administrador (Familia)
  ├── Gestión de Usuarios (Familia)
  │   ├── Alta de Usuario (Patente)
  │   ├── Baja de Usuario (Patente)
  │   └── Modificar Usuario (Patente)
  └── Gestión de Permisos (Familia)
      └── Asignar Permisos (Patente)
```

**2. Repository Pattern with Adapter**

Data access uses Repository pattern with Adapters to map between DataTable rows and domain entities:

- `IGenericRepository<T>` - Base repository interface
- Concrete repositories (e.g., `UsuarioRepository.Current` is a singleton)
- Adapters (`UsuarioAdapter`, `FamiliaAdapter`, etc.) convert ADO.NET DataTables to entities

**3. DVH (Dígito Verificador Horizontal) - Data Integrity**

The `Usuario` table includes a DVH column for detecting unauthorized database modifications:

**DVH Calculation:**
```csharp
string datos = $"{UPPER(IdUsuario)}|{Nombre}|{Clave}|{(Activo ? 1 : 0)}";
DVH = CryptographyService.HashPassword(datos); // SHA256 with Encoding.Unicode
```

**IMPORTANT:**
- GUID must be in UPPERCASE when calculating DVH
- Only includes: IdUsuario, Nombre, Clave, Activo (NOT Email or IdiomaPreferido)
- Must be recalculated on every INSERT/UPDATE of Usuario
- Must be validated on every SELECT

**4. Password Hashing**

Uses `CryptographyService.HashPassword()`:
- Algorithm: SHA256
- Encoding: `Encoding.Unicode` (UTF-16) to match SQL Server NVARCHAR
- Output: Uppercase hexadecimal string
- Matches SQL Server: `CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', @input), 2)`

## Database Schema

**Database Name:** SeguridadBiblioteca

**Core Tables:**
- `Usuario` - Users (with DVH for integrity)
- `Familia` - Roles and permission groups (Composite pattern)
- `Patente` - Atomic permissions (Leaf pattern)
- `UsuarioFamilia` - User-to-role/group assignments
- `UsuarioPatente` - User-to-permission direct assignments
- `FamiliaFamilia` - Hierarchy (Familia contains Familia)
- `FamiliaPatente` - Familia contains Patente
- `Language` - i18n translations
- `Logger` - Application logs

**Relationships:**
- Many-to-many: Usuario ↔ Familia (through UsuarioFamilia)
- Many-to-many: Usuario ↔ Patente (through UsuarioPatente)
- Many-to-many: Familia ↔ Familia (through FamiliaFamilia) - Composite hierarchy
- Many-to-many: Familia ↔ Patente (through FamiliaPatente)

## Critical Implementation Details

### When Creating/Updating Users

Always calculate DVH:
```csharp
usuario.DVH = CalcularDVH(usuario);

private string CalcularDVH(Usuario u)
{
    string datos = $"{u.IdUsuario.ToString().ToUpper()}|{u.Nombre}|{u.Clave}|{(u.Activo ? 1 : 0)}";
    return CryptographyService.HashPassword(datos);
}
```

### When Reading Users

Always validate DVH:
```csharp
var usuario = UsuarioRepository.Current.SelectOne(id);
string dvhCalculado = CalcularDVH(usuario);
if (dvhCalculado != usuario.DVH)
    throw new IntegridadException("DVH no coincide - registro alterado");
```

### Working with Roles

```csharp
// Get user role
var rolNombre = usuario.ObtenerNombreRol(); // Returns "Administrador", "Veterinario", etc.

// Check if user has a role
if (usuario.TieneRol("Administrador")) { ... }

// Get role Familia
var familiaRol = usuario.ObtenerFamiliaRol();
```

### Menu Construction

Use `usuario.GetPatentesAll()` to retrieve all permissions recursively through the Composite hierarchy, then filter unique patentes by `FormName` to build the menu.

## Configuration

**App.config Settings:**
- `connectionStrings/ServicesConString` - Database connection
- `appSettings/LanguagePath` - i18n file path (`Resources\I18n\idioma`)
- `appSettings/SecurityRepositoryServices` - DAL namespace for dependency injection

**i18n Files:**
- `UI/Resources/I18n/idioma.es-AR` - Spanish (Argentina)
- `UI/Resources/I18n/idioma.en-GB` - English (UK)

## Common Tasks

### Adding a New User Manually

Use the template script:
```bash
# Edit Database/06_InsertarUsuarioManual.sql with user details
sqlcmd -S localhost -i "Database\06_InsertarUsuarioManual.sql"
```

### Adding a New Permission (Patente)

1. Insert into `Patente` table with `FormName`, `MenuItemName`, `Orden`, `Descripcion`
2. Link to a Familia via `FamiliaPatente` table
3. Update `03_DatosIniciales.sql` for future installations

### Creating a New Role

1. Create a Familia with name starting with `ROL_` (e.g., `ROL_Enfermero`)
2. Link child Familias (permission groups) via `FamiliaFamilia`
3. Or link Patentes directly via `FamiliaPatente`

### Viewing Logs

```sql
sqlcmd -S localhost -i "Database\VerLogs.sql"
```

## Important Notes

- **Singleton Repositories:** DAL repositories use `.Current` singleton pattern (e.g., `UsuarioRepository.Current`)
- **Never modify the database directly** without recalculating DVH for Usuario table
- **Password storage:** Always hash passwords using `CryptographyService.HashPassword()` before storing
- **GUID format:** When working with DVH, always use UPPERCASE GUID strings
- **Composite pattern:** When traversing permissions, use `component.ChildrenCount()` to distinguish between Familia (>0) and Patente (=0)
- **Migration notes:** See `Database/README_MIGRACION.md` for details on the roles-as-Familias migration

## Future Development

The BLL, DAL, DomainModel, and Services folders at the root level are placeholders for future business logic modules (e.g., veterinary operations, appointments, medical records). The ServicesSecurity project is a complete, standalone security module.
