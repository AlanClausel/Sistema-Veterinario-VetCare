# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

VetCare is a desktop veterinary clinic management system built with .NET Framework 4.7.2 and Windows Forms. It manages clients, pets, appointments, medical consultations, medications, and includes a comprehensive security/audit system.

## Build and Development Commands

### Build the Solution
```bash
# Using the build script (recommended)
build.bat

# Using MSBuild directly
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "Sistema Veterinario VetCare.sln" /p:Configuration=Debug

# Clean all build artifacts
clean_all.bat
```

### Database Setup

**Initial Setup (Security Database):**
```bash
# Install SecurityVet database with all initial data
sqlcmd -S localhost -i "Database\00_EJECUTAR_TODO.sql"

# This creates:
# - Database: SecurityVet
# - Default admin user: admin / admin123
# - Role-based permission system (Familias/Patentes)
```

**Business Database Setup:**
```bash
# Install VetCareDB with all business tables
sqlcmd -S localhost -i "Database\00_EJECUTAR_TODO_NEGOCIO.sql"

# This creates:
# - Database: VetCareDB
# - Tables: Cliente, Mascota, Cita, Veterinario, Medicamento, ConsultaMedica
# - 54 stored procedures for business operations
```

**Bitacora (Audit Log) Module:**
```bash
# Install audit trail system (optional but recommended)
sqlcmd -S localhost -i "Database\40_EJECUTAR_TODO_BITACORA.sql"
sqlcmd -S localhost -i "Database\42_CrearPatenteBitacora.sql"

# See INSTALACION_BITACORA.md for detailed instructions
```

### Running the Application
```bash
# After building, run the UI project
cd UI\bin\Debug
UI.exe

# Default credentials for testing:
# Username: admin
# Password: admin123
```

## Architecture Overview

### Solution Structure

The solution follows a strict layered architecture with dual-database design:

```
VetCareNegocio/               # Business logic domain
├── DomainModel/              # Pure domain entities (Cliente, Mascota, etc.)
├── DAL/                      # Data Access Layer with Repository pattern
│   ├── Contracts/            # Repository interfaces (IClienteRepository, etc.)
│   ├── Implementations/      # Repository implementations + Adapters
│   └── Tools/                # SqlHelper for raw SQL/SP execution
├── BLL/                      # Business Logic Layer (ClienteBLL, CitaBLL, etc.)
└── Services/                 # (Currently empty - placeholder)

ServicesSeguridad/            # Security/Auth domain (separate bounded context)
├── DomainModel/Security/     # Security entities (Usuario, Familia, Patente)
│   └── Composite/            # Composite pattern for permissions hierarchy
├── DAL/                      # Security data access + Unit of Work
├── BLL/                      # Security business logic
└── Services/                 # LoginService, Bitacora, ExceptionManager

UI/                           # WinForms presentation layer
└── WinUi/
    ├── Login.cs              # Application entry point
    ├── Negocio/              # Business forms (Clientes, Mascotas, Citas, etc.)
    └── Administración/       # Admin forms (Bitacora, Permisos, Usuarios)
```

### Dual Database Architecture

**VetCareDB (Business Domain):**
- Connection String: `VetCareConString` in App.config
- Contains: Cliente, Mascota, Cita, Veterinario, Medicamento, ConsultaMedica
- All business logic and operational data

**SecurityVet (Security/Auth Domain):**
- Connection String: `ServicesConString` in App.config
- Contains: Usuario, Familia, Patente, UsuarioFamilia, Bitacora
- Authentication, authorization, and audit trail

### Key Design Patterns

**1. Repository Pattern with Singleton**
- All repositories implement singleton pattern: `ClienteRepository.Current`
- Each repository has an interface: `IClienteRepository`
- Adapters convert DataTable rows to domain objects
- Located in: `VetCareNegocio/DAL/Implementations/`

**2. Composite Pattern (Permissions)**
- Hierarchical permission system using Composite pattern
- `Component` (abstract) → `Familia` (composite) + `Patente` (leaf)
- Roles are `Familia` objects with name prefix `ROL_`
- Users can have multiple roles and individual permissions
- Recursive permission loading via `CargarHijosDeFamilia()`
- Located in: `ServicesSeguridad/DomainModel/Security/Composite/`

**3. Unit of Work Pattern**
- Recent addition for transactional atomicity
- `IUnitOfWork` interface with `SecurityUnitOfWork` implementation
- Used in: `FamiliaBLL.ActualizarPatentesDeRol()` and `UsuarioBLL.CambiarRol()`
- Ensures all-or-nothing for multi-step DB operations
- See: `UNIT_OF_WORK_IMPLEMENTATION.md` for details

**4. Adapter Pattern**
- Each entity has an adapter: `ClienteAdapter`, `MascotaAdapter`, etc.
- Converts DataRow from stored procedures to domain objects
- Located in: `DAL/Implementations/Adapter/`

**5. Singleton Pattern for Services**
- Stateless services use singleton: `Bitacora.Current`, `LoginService`, etc.
- Thread-safe via static readonly instances

**6. Observer Pattern (Multi-Language System)**
- **LanguageManager** acts as Subject/Observable
- **ILanguageObserver** interface for observers
- **BaseObservableForm** provides automatic subscription/unsubscription
- When language changes via `LanguageManager.CambiarIdioma()`, all open forms update automatically
- Thread-safe implementation with proper UI thread handling
- Forms inherit from `BaseObservableForm` and implement `ActualizarTextos()`
- Located in: `ServicesSeguridad/Services/ILanguageObserver.cs` and `UI/WinUi/BaseObservableForm.cs`
- **See: `PATRON_OBSERVER_IDIOMA.md` for complete documentation and usage examples**

### Data Access Layer Details

**SqlHelper Pattern:**
- Located in: `VetCareNegocio/DAL/Tools/SqlHelper.cs`
- Core methods:
  - `ExecuteNonQuery()` - INSERT/UPDATE/DELETE
  - `ExecuteScalar()` - Single value returns
  - `ExecuteDataTable()` - Bulk data retrieval
  - `ExecuteReader()` - Streaming large results
- **CRITICAL:** System uses stored procedures exclusively (no inline SQL)
- All methods use `CommandType.StoredProcedure` to prevent SQL injection

**Transaction Support:**
- SqlHelper has overloads accepting `SqlConnection` + `SqlTransaction`
- Used by Unit of Work pattern for atomic operations
- Example: `ExecuteNonQuery(connection, transaction, "SP_Name", ...)`

### Security & Permission System

**Authentication Flow:**
1. User enters credentials in `Login.cs`
2. `LoginService.Login(username, password)` validates credentials
3. Password hashing via `CryptographyService.HashPassword()`
4. DVH (Dígito Verificador Horizontal) validates user data integrity
5. On success: Load all Familias (roles) and Patentes (permissions) recursively
6. Store in static `_usuarioLogueado` for session management
7. Log to Bitacora: `RegistrarLogin()` or `RegistrarLoginFallido()`

**Permission Checking:**
```csharp
// Get current user anywhere in the app
var usuario = LoginService.GetUsuarioLogueado();

// Check if user has a specific role
bool esAdmin = usuario.TieneRol("ROL_Administrador");

// Check for specific permission (form access)
bool puedeAcceder = usuario.TienePatente("FormGestionClientes");
```

**Role Hierarchy:**
- Familias can contain other Familias (nested roles)
- Familias can contain Patentes (permissions)
- Users assigned to Familias inherit all child permissions recursively

### Audit Trail (Bitacora)

**Two-Level Logging:**

1. **File-Based Logging** (via LoggerService):
   - `Bitacora.Current.LogInfo()`, `LogWarning()`, `LogError()`, `LogCritical()`
   - Writes to application log files

2. **Database Audit Trail** (SecurityVet.Bitacora table):
   - `Bitacora.Current.RegistrarLogin()` - Successful logins
   - `Bitacora.Current.RegistrarLoginFallido()` - Failed login attempts
   - `Bitacora.Current.RegistrarAlta()` - CREATE operations
   - `Bitacora.Current.RegistrarBaja()` - DELETE operations
   - `Bitacora.Current.RegistrarModificacion()` - UPDATE operations
   - `Bitacora.Current.RegistrarViolacionDVH()` - Data integrity violations

**Usage Example:**
```csharp
var usuario = LoginService.GetUsuarioLogueado();
var cliente = ClienteBLL.Current.RegistrarCliente(nuevoCliente);

// Log to audit trail
Bitacora.Current.RegistrarAlta(
    usuario.IdUsuario,
    usuario.Nombre,
    "Clientes",           // Module
    "Cliente",            // Entity type
    cliente.IdCliente.ToString(),
    $"Cliente registrado: {cliente.NombreCompleto}"
);
```

## Domain Model Entities

All entities use **GUID for primary keys** (not auto-increment integers).

**Key Entities:**
- `Cliente` - Pet owners (DNI must be unique)
- `Mascota` - Pets (belongs to Cliente)
- `Cita` - Appointments (references Mascota and Veterinario)
- `Veterinario` - Veterinary professionals (Matricula must be unique)
- `Medicamento` - Medication inventory with stock management
- `ConsultaMedica` - Medical consultation records
- `MedicamentoRecetado` - Medications prescribed in consultations

**Entity Relationships:**
```
Cliente (1) ──→ (N) Mascota
Mascota (1) ──→ (N) Cita
Veterinario (1) ──→ (N) Cita
Cita (1) ──→ (1) ConsultaMedica
ConsultaMedica (1) ──→ (N) MedicamentoRecetado
Medicamento (1) ──→ (N) MedicamentoRecetado
```

## Important Coding Conventions

### Working with Repositories

**Always use singleton pattern:**
```csharp
// CORRECT
var cliente = ClienteRepository.Current.ObtenerPorId(id);

// INCORRECT - Don't create new instances
var repo = new ClienteRepository(); // DON'T DO THIS
```

### Working with BLL

**Use Current property:**
```csharp
// Business logic always goes through BLL layer
var cliente = ClienteBLL.Current.RegistrarCliente(cliente);
var citas = CitaBLL.Current.ObtenerCitasPendientes();
```

### Stored Procedure Naming Convention

All stored procedures follow this pattern:
- `[Entity]_Insert` - Create operations
- `[Entity]_Update` - Update operations
- `[Entity]_Delete` - Delete operations (soft delete - sets Activo=0)
- `[Entity]_SelectOne` - Get by ID
- `[Entity]_SelectAll` - Get all active records
- `[Entity]_SelectBy[Criteria]` - Custom queries

**Examples:**
- `Cliente_Insert`, `Cliente_Update`, `Cliente_Delete`
- `Mascota_SelectByCliente` - Get all pets for a client
- `Cita_SelectByRangoFechas` - Get appointments in date range

### Transaction Pattern with Unit of Work

**When to use Unit of Work:**
- Multiple related INSERT/UPDATE/DELETE operations
- Operations that must succeed/fail together atomically
- Role/permission updates

**Example:**
```csharp
using (var uow = new SecurityUnitOfWork())
{
    uow.BeginTransaction();
    try
    {
        // Multiple operations within transaction
        repository.Delete(item1, uow);
        repository.Insert(item2, uow);
        repository.Update(item3, uow);

        uow.Commit(); // All succeed
    }
    catch (Exception ex)
    {
        uow.Rollback(); // All rollback
        throw;
    }
}
```

### Exception Handling

**Use ExceptionManager for centralized handling:**
```csharp
try
{
    // Operation
}
catch (Exception ex)
{
    ExceptionManager.GetInstance().HandleException(ex);
    // Logs to Bitacora and shows user-friendly message
    throw;
}
```

## Multi-Language Support

The system supports multiple languages via `LanguageManager` using the **Observer Pattern**:

```csharp
// Get localized string
string message = LanguageManager.Translate("key_name");

// Change language (notifies all open forms automatically)
LanguageManager.CambiarIdioma("es-AR"); // or "en-GB"

// User language preference stored in Usuario.IdiomaPreferido
```

**Making Forms Observable:**
Forms should inherit from `BaseObservableForm` and implement `ActualizarTextos()`:

```csharp
public partial class MiFormulario : BaseObservableForm
{
    protected override void ActualizarTextos()
    {
        this.Text = LanguageManager.Translate("titulo");
        btnGuardar.Text = LanguageManager.Translate("guardar");
        // ... update all controls
    }
}
```

When users change language in `FormMiCuenta`, all open forms update automatically via the Observer pattern.

Resource files located in: `UI/Resources/I18n/idioma/`

**See `PATRON_OBSERVER_IDIOMA.md` for complete documentation.**

## Testing

**Manual Testing Guide:**
- See `GUIA_TESTING_MANUAL.md` for comprehensive test cases
- Includes 120+ manual test cases organized by module
- Covers CRUD operations, validations, permissions, and edge cases

**Test Data:**
- Default admin user: `admin` / `admin123`
- Create test users with different roles via UI or SQL scripts
- Use GUID generators for test data IDs

## Common Development Tasks

### Adding a New Entity

1. Create domain model class in `VetCareNegocio/DomainModel/`
2. Create SQL table in new migration script `Database/XX_CreateTable[Entity].sql`
3. Create stored procedures in `Database/XX_CreateSP_[Entity].sql`
4. Create repository interface in `DAL/Contracts/I[Entity]Repository.cs`
5. Create adapter in `DAL/Implementations/Adapter/[Entity]Adapter.cs`
6. Create repository implementation in `DAL/Implementations/[Entity]Repository.cs`
7. Create BLL in `BLL/[Entity]BLL.cs`
8. Create UI form in `UI/WinUi/Negocio/FormGestion[Entity].cs`
9. Create Patente (permission) for the form via SQL script
10. Add Bitacora logging in BLL methods

### Adding a New Permission (Patente)

1. Create SQL script: `Database/XX_CrearPatente[Feature].sql`
2. Insert into Patente table:
```sql
INSERT INTO Patente (FormName, MenuItemName, Orden, Descripcion)
VALUES ('FormGestionXYZ', 'Gestión de XYZ', '100', 'Permite gestionar XYZ');
```
3. Assign to default roles (usually ROL_Administrador):
```sql
INSERT INTO FamiliaPatente (idFamilia, idPatente)
SELECT f.IdFamilia, p.IdPatente
FROM Familia f, Patente p
WHERE f.Nombre = 'ROL_Administrador'
  AND p.FormName = 'FormGestionXYZ';
```

### Modifying Database Schema

**NEVER modify existing scripts!** Always create new migration scripts:
```bash
Database/
├── 45_[DescriptiveChangeName].sql  # New migration
├── 46_[AnotherChange].sql          # Next migration
```

**Example:**
- Adding column: `45_AgregarColumnaEmailACliente.sql`
- Modifying constraint: `46_ModificarConstraintMascota.sql`

### Working with Cita (Appointments)

**Important:** The `Cita` entity has both legacy and new fields for veterinarian:
- `IdVeterinario` (Guid, nullable) - NEW: Foreign key to Veterinario table
- `Veterinario` (string) - LEGACY: Deprecated, kept for backward compatibility

**Always use `IdVeterinario` for new code.**

**Estado (State) Enum:**
- `Agendada` - Scheduled
- `Confirmada` - Confirmed by client
- `Completada` - Consultation finished
- `Cancelada` - Cancelled
- `NoAsistio` - No-show

### Working with ConsultaMedica

**Important:** The `ConsultaMedica_Finalizar` stored procedure handles:
1. Creating ConsultaMedica record
2. Inserting prescribed medications (ConsultaMedicamento)
3. Reducing medication stock
4. Updating Cita status to 'Completada'
5. All within a transaction (BEGIN TRAN / COMMIT / ROLLBACK)

**Do NOT wrap this in Unit of Work** - transaction is already in the SP.

## Database Migration Workflow

1. Write new `.sql` script in `Database/` folder
2. Use sequential numbering: `45_`, `46_`, etc.
3. Test script in development environment first
4. Run via sqlcmd:
```bash
sqlcmd -S localhost -i "Database\45_YourScript.sql"
```
5. Verify changes in SQL Server Management Studio
6. Update corresponding C# code (repositories, adapters, domain models)
7. Rebuild solution and test

## Known Issues and Gotchas

1. **Quoted Identifier Issues**: Some old stored procedures had SET QUOTED_IDENTIFIER OFF issues. See scripts `31_CorregirSP_VeterinarioQuotedIdentifier.sql` and `32_CorregirSP_CitaQuotedIdentifier.sql` for examples.

2. **DVH (Data Integrity)**: The Usuario table has DVH (Dígito Verificador Horizontal) for integrity checking. If you modify user data directly in SQL, you MUST recalculate DVH via `04_RecalcularDVH.sql`.

3. **Password Hashing**: Passwords are hashed using `CryptographyService.HashPassword()`. Never store plain text passwords. If manually creating users via SQL, use the pattern from `08_CrearUsuarioAdminNuevo.sql`.

4. **Soft Deletes**: All entities use soft delete (Activo = 0). Never use `DELETE FROM`. Always use the repository's Delete method or UPDATE to set Activo=0.

5. **GUID Generation**: When inserting via stored procedures, use NEWID() in SQL or Guid.NewGuid() in C#. Never use Guid.Empty.

6. **Cascading Deletes**: When deleting Cliente, all related Mascota records should also be soft-deleted. This logic is in `Cliente_Delete` stored procedure.

## File Organization Notes

- **Database Scripts**: Numbered sequentially starting from 00. Execute in order.
- **Documentation**:
  - `INSTALACION_BITACORA.md` - Bitacora module setup
  - `UNIT_OF_WORK_IMPLEMENTATION.md` - Unit of Work pattern details
  - `GUIA_TESTING_MANUAL.md` - Manual testing guide
  - `PLANTILLA_REPORTE_BUG.md` - Bug report template
- **Images**: Screenshots and diagrams are in the root for documentation purposes

## Security Considerations

- Always use stored procedures (never inline SQL)
- Validate user input in BLL before calling repositories
- Check permissions before showing/enabling UI forms
- Log sensitive operations to Bitacora
- Use Unit of Work for atomic permission/role changes
- Never log passwords or sensitive data
- Verify DVH integrity for Usuario records

## Connection String Configuration

Located in `UI/App.config`:

```xml
<connectionStrings>
  <!-- Business database -->
  <add name="VetCareConString"
       connectionString="Data Source=localhost;Initial Catalog=VetCareDB;Integrated Security=True;TrustServerCertificate=True" />

  <!-- Security/Auth database -->
  <add name="ServicesConString"
       connectionString="Data Source=localhost;Initial Catalog=SecurityVet;Integrated Security=True;TrustServerCertificate=True" />
</connectionStrings>
```

Update these for different environments (dev/staging/production).
