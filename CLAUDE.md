# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Sistema Veterinaria VetCare** is a Windows Forms veterinary management system built with C# (.NET Framework 4.7.2) that manages clients, pets, appointments, medical consultations, and user permissions. The system uses a layered architecture with two separate SQL Server databases.

## Build Commands

### Build the solution
```bash
# Using build.bat
build.bat

# Using MSBuild directly
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "Sistema Veterinario VetCare.sln" /p:Configuration=Debug
```

### Clean all build artifacts
```bash
clean_all.bat
```

## Database Setup

The system uses **two separate SQL Server databases**:

### 1. SecurityVet (Security Module)
```bash
# Complete installation (recommended)
sqlcmd -S localhost -i "Database\00_EJECUTAR_TODO.sql"

# Step by step
sqlcmd -S localhost -i "Database\01_CrearBaseDatos.sql"
sqlcmd -S localhost -i "Database\02_CrearTablas.sql"
sqlcmd -S localhost -i "Database\03_DatosIniciales.sql"
```

**Default admin credentials:** `admin` / `admin123`

### 2. VetCareDB (Business Module)
```bash
# Complete installation (recommended)
sqlcmd -S localhost -i "Database\00_EJECUTAR_TODO_NEGOCIO.sql"

# Individual scripts available in Database folder (14-32)
```

### Connection Strings
Located in `UI/App.config`:
- **ServicesConString**: SecurityVet database
- **VetCareConString**: VetCareDB database

Both use `localhost` by default with Integrated Security and TrustServerCertificate=True.

## Architecture

### Layered Architecture (3-Tier)

```
UI (Windows Forms)
  └── WinUi/
      ├── Administración/ (Security module forms)
      ├── Negocio/ (Business module forms)
      └── Login.cs (Entry point)
          ↓
BLL (Business Logic Layer)
  ├── ServicesSeguridad/BLL/ (Security module)
  │   ├── UsuarioBLL - User management
  │   └── FamiliaBLL - Permission management
  └── VetCareNegocio/BLL/ (Business module)
      ├── ClienteBLL - Client use cases
      ├── MascotaBLL - Pet use cases
      ├── CitaBLL - Appointment use cases
      ├── ConsultaMedicaBLL - Medical consultation use cases
      ├── MedicamentoBLL - Medication use cases
      └── VeterinarioBLL - Veterinarian use cases
          ↓
DAL (Data Access Layer)
  ├── ServicesSeguridad/DAL/ (SecurityVet DB)
  │   ├── Contracts/ (Interfaces)
  │   ├── Implementations/ (Repositories using SPs)
  │   └── Tools/SqlHelper
  └── VetCareNegocio/DAL/ (VetCareDB)
      ├── Contracts/ (Interfaces)
      ├── Implementations/ (Repositories using SPs)
      ├── Adapters/ (DataRow → Entity conversion)
      └── Tools/SqlHelper
          ↓
Database
  ├── SecurityVet (Usuario, Familia, Patente tables)
  └── VetCareDB (Cliente, Mascota, Cita, ConsultaMedica, Medicamento, Veterinario tables)
```

### Key Architectural Patterns

1. **Repository Pattern (Specific)**: Each entity has a specific repository with domain methods, not generic CRUD
2. **Singleton Pattern**: All BLL and Repository classes use singleton (`ClassName.Current`)
3. **Adapter Pattern**: `Adapters/` convert ADO.NET DataRows to domain entities
4. **Unit of Work Pattern**: For transactional operations (see `UNIT_OF_WORK_IMPLEMENTATION.md`)
5. **Composite Pattern**: Security permissions use composite pattern (Familia can contain Familias and Patentes)
6. **Stored Procedures Only**: All database access uses SPs, no inline SQL queries

### Project Structure

- **UI/** - Windows Forms presentation layer (.csproj: UI.csproj)
- **ServicesSeguridad/** - Security module (authentication, authorization) (.csproj: ServicesSecurity.csproj)
  - Uses `ServicesConString` connection
  - Database: SecurityVet
- **VetCareNegocio/** - Business module (split into 4 projects)
  - **DomainModel/** - Domain entities (Cliente, Mascota, Cita, etc.)
  - **DAL/** - Data access with repositories and adapters
  - **BLL/** - Business logic and use cases
  - **Services/** - Cross-cutting services
  - Uses `VetCareConString` connection
  - Database: VetCareDB

## Security Module

### Composite Permissions System

The security module implements a **Composite Pattern** for permissions:

- **Patente** (Leaf): Atomic permission (e.g., "Alta de Usuario", "Gestión de Clientes")
  - Properties: `FormName` (form to open), `MenuItemName` (display text), `Orden`
- **Familia** (Composite): Group of permissions or roles
  - Naming convention: `ROL_*` = Role (e.g., `ROL_Administrador`, `ROL_Veterinario`, `ROL_Recepcionista`)
  - Other names = Permission groups (e.g., "Gestión de Usuarios")
- **Relationships**:
  - Usuario → Familia (roles assigned to user)
  - Usuario → Patente (direct permissions)
  - Familia → Familia (hierarchy)
  - Familia → Patente (permissions in group)

### Key Security Features

1. **DVH (Dígito Verificador Horizontal)**: SHA256 hash per Usuario row to detect unauthorized DB modifications
2. **Password Hashing**: SHA256 via `CryptographyService.HashPassword()`
3. **Dynamic Menu**: Menu loads based on user's Patentes (`menu.cs:CargarMenuDinamico()`)
4. **Permission Checking**: UI forms check permissions via `_usuarioLogueado.TienePermiso(formName)`

### LoginService

Static service in `ServicesSeguridad/Services/LoginService.cs`:
- `Login(nombre, password)` - Authenticates and loads permissions recursively
- `GetUsuarioLogueado()` - Returns current logged-in user
- Loads Composite tree recursively with `CargarHijosDeFamilia()`

## Business Module Entities

### Main Domain Models (VetCareNegocio/DomainModel/)

- **Cliente**: Name, Apellido, DNI (unique), Telefono, Email, Direccion
- **Mascota**: Belongs to Cliente, has Nombre, Especie, Raza, FechaNacimiento, Sexo, Peso, Color
- **Veterinario**: Synced from SecurityVet Usuario when assigned ROL_Veterinario
- **Cita**: Appointment linking Cliente, Mascota, Veterinario, with Estado (Agendada, Confirmada, Completada, Cancelada, NoAsistio)
- **ConsultaMedica**: Links to Cita, contains Sintomas, Diagnostico, Tratamiento
- **Medicamento**: Stock, PrecioUnitario, Nombre, Presentacion
- **ConsultaMedicamento**: Many-to-many linking ConsultaMedica and Medicamento with Cantidad and Indicaciones

### Business Validations

All validations occur in BLL classes before calling repositories:

**ClienteBLL:**
- Nombre/Apellido: min 2 chars
- DNI: min 6 chars, unique
- Email: valid format if provided

**MascotaBLL:**
- Nombre: min 2 chars
- Sexo: must be "Macho" or "Hembra"
- FechaNacimiento: cannot be future
- Peso: >= 0, < 1000 kg
- Cliente must exist and be active

**ConsultaMedicaBLL:**
- Sintomas: min 10 chars
- Diagnostico: min 10 chars
- Stock validation before finalizing (uses transaction in SP)

## Important Development Patterns

### Using BLL Classes

All BLL classes are singletons accessed via `.Current`:

```csharp
var clienteBLL = ClienteBLL.Current;
var cliente = clienteBLL.RegistrarCliente(new Cliente { ... });
```

### Using Repositories

Repositories use Stored Procedures only:

```csharp
// All repositories follow this pattern
var repo = ClienteRepository.Current;
var cliente = repo.Crear(clienteEntity);
var clientes = repo.ObtenerTodos();
```

### Using Unit of Work (for transactional operations)

```csharp
using (var uow = new SecurityUnitOfWork())
{
    uow.BeginTransaction();
    try
    {
        // Multiple operations
        repository.Insert(entity, uow);
        repository.Delete(otherEntity, uow);
        uow.Commit();
    }
    catch
    {
        uow.Rollback();
        throw;
    }
}
```

See `UNIT_OF_WORK_IMPLEMENTATION.md` for detailed examples.

### Connection String Usage

Always use named connection strings from App.config:

```csharp
// Security module
SqlHelper.ExecuteNonQuery(commandText, commandType, parameters);
// Uses "ServicesConString" by default

// Business module
SqlHelper.ExecuteNonQuery(commandText, commandType, parameters);
// Uses "VetCareConString" by default (configured in SqlHelper constructor)
```

## Critical Implementation Notes

### 1. Two Databases, One System

The system coordinates data across two databases:
- When a user is assigned `ROL_Veterinario`, a corresponding record is created in `VetCareDB.Veterinario` table
- This synchronization happens in `UsuarioBLL.AsignarFamiliaAUsuario()` and `UsuarioBLL.CambiarRol()`
- The `IdUsuario` (GUID) is shared between both databases as the linking key

### 2. All Database Access Uses Stored Procedures

Never write inline SQL. All operations use SPs:
- SecurityVet SPs: prefixed by entity (e.g., `Usuario_Insert`, `Familia_SelectAll`)
- VetCareDB SPs: prefixed by entity (e.g., `Cliente_Insert`, `Mascota_SelectByCliente`)
- Check `Database/` folder for SP definitions

### 3. DVH Validation

When working with Usuario table:
- **Always recalculate DVH** on INSERT/UPDATE
- **Always validate DVH** on SELECT
- Formula: `SHA256(IdUsuario|Nombre|Clave|Activo)`
- Validation in `UsuarioRepository.SelectOne()` and similar methods

### 4. Dynamic Menu System

The menu loads dynamically based on Patentes:
- Menu items created at runtime in `menu.cs:CargarMenuDinamico()`
- Uses `FormName` to instantiate form: `Type.GetType($"UI.WinUi.Negocio.{patente.FormName}")`
- New forms need matching Patente records in SecurityVet database

### 5. Adapter Pattern for Data Access

Each repository uses an Adapter to convert DataRows to entities:
- Located in `DAL/Adapters/`
- Example: `ClienteAdapter.DataRowToCliente(DataRow row)`
- Always use adapters, never manual field mapping in repositories

### 6. Transactional Operations

Some operations require Unit of Work:
- **FamiliaBLL.ActualizarPatentesDeRol()** - Multiple DELETE/INSERT of patentes
- **UsuarioBLL.CambiarRol()** - Delete old roles + insert new role
- **ConsultaMedicaBLL.FinalizarConsulta()** - Already has transaction in SP `ConsultaMedica_Finalizar`

### 7. Exception Handling

The system has custom exceptions in `DomainModel/Exceptions/`:
- `ValidacionException` - Business rule violations
- `IntegridadException` - DVH/data integrity issues
- `UsuarioNoEncontradoException`, `ContraseñaInvalidaException` - Authentication
- Always catch and handle appropriately in UI layer

## Testing

### Manual Testing
See `GUIA_TESTING_MANUAL.md` and `Testing_Manual_VetCare.csv` for 120 test cases covering:
- Login validation
- CRUD operations per module
- Permission checks
- Data integrity

### Database Verification
```sql
-- Verify SecurityVet installation
USE SecurityVet;
SELECT * FROM Usuario;
SELECT * FROM Familia WHERE Nombre LIKE 'ROL_%';

-- Verify VetCareDB installation
USE VetCareDB;
SELECT * FROM Cliente;
SELECT * FROM Mascota;
```

## Common Development Tasks

### Adding a New Form with Permissions

1. Create the form in `UI/WinUi/Negocio/` or `UI/WinUi/Administración/`
2. Create Patente in SecurityVet:
   ```sql
   INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
   VALUES (NEWID(), 'NombreFormulario', 'Texto del Menú', 100, 'Descripción');
   ```
3. Assign Patente to Familia/Role:
   ```sql
   INSERT INTO FamiliaPatente (idFamilia, idPatente)
   SELECT f.IdFamilia, p.IdPatente
   FROM Familia f, Patente p
   WHERE f.Nombre = 'ROL_Administrador' AND p.FormName = 'NombreFormulario';
   ```
4. The menu will auto-load the form for users with that permission

### Adding a New BLL Use Case

1. Define method in `BLL/EntityBLL.cs`
2. Add validations (throw `ValidacionException` if needed)
3. Call repository methods
4. Return domain entity

### Adding a New Repository Method

1. Create Stored Procedure in `Database/` folder
2. Add method to interface in `DAL/Contracts/IEntityRepository.cs`
3. Implement in `DAL/Implementations/EntityRepository.cs`
4. Use `SqlHelper.ExecuteNonQuery/ExecuteReader/ExecuteScalar`
5. Use Adapter to convert results

### Modifying Database Schema

1. Create SQL script in `Database/` with sequential numbering (e.g., `33_NombreDelCambio.sql`)
2. Update corresponding SP scripts
3. Update domain model in `DomainModel/`
4. Update Adapter in `DAL/Adapters/`
5. Update Repository in `DAL/Implementations/`
6. Add migration documentation

## File References

- **Architecture**: See layered structure above
- **Database Setup**: `Database/README_INSTALACION.md`, `Database/README_MODULO_NEGOCIO.md`, `Database/README_MODULO_VETERINARIO.md`
- **Unit of Work**: `UNIT_OF_WORK_IMPLEMENTATION.md`
- **Testing**: `GUIA_TESTING_MANUAL.md`, `Testing_Manual_VetCare.csv`
- **Entry Point**: `UI/WinUi/Login.cs`
- **Main Menu**: `UI/WinUi/Administración/menu.cs`
