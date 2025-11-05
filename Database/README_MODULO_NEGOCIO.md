# Módulo de Negocio VetCare - Guía de Instalación y Uso

## 📋 Resumen

Este módulo implementa la lógica de negocio para la gestión de **Clientes** y **Mascotas** en VetCare, siguiendo patrones de arquitectura en capas con repositorios específicos y casos de uso de negocio.

## 🏗️ Arquitectura Implementada

```
UI (Presentación)
  └── WinForms (gestionClientes.cs, gestionMascotas.cs)
      ↓
BLL (Lógica de Negocio)
  ├── ClienteBLL - Casos de uso de clientes
  └── MascotaBLL - Casos de uso de mascotas
      ↓
DAL (Acceso a Datos)
  ├── Contracts/
  │   ├── IClienteRepository - Contrato específico
  │   └── IMascotaRepository - Contrato específico
  ├── Implementations/
  │   ├── ClienteRepository - Implementación con SPs
  │   └── MascotaRepository - Implementación con SPs
  ├── Adapters/
  │   ├── ClienteAdapter - DataRow → Cliente
  │   └── MascotaAdapter - DataRow → Mascota
  └── Tools/
      └── SqlHelper - Helper de base de datos
          ↓
Base de Datos: VetCareDB
  ├── Tabla Cliente
  ├── Tabla Mascota
  └── 14 Stored Procedures
```

## 📦 Instalación

### Paso 1: Crear Base de Datos

```bash
# Opción A: Instalación completa (recomendado)
sqlcmd -S localhost -i "Database\00_EJECUTAR_TODO_NEGOCIO.sql"

# Opción B: Paso a paso
sqlcmd -S localhost -i "Database\14_CrearBaseDatosNegocio.sql"
sqlcmd -S localhost -i "Database\15_CrearTablasNegocio.sql"
sqlcmd -S localhost -i "Database\16_CrearSP_Negocio.sql"
```

### Paso 2: Verificar Instalación

```sql
USE VetCareDB;

-- Verificar tablas
SELECT * FROM INFORMATION_SCHEMA.TABLES;

-- Verificar stored procedures
SELECT name FROM sys.procedures ORDER BY name;

-- Debe mostrar:
--   Cliente_Delete, Cliente_Insert, Cliente_Search, Cliente_SelectAll,
--   Cliente_SelectByDNI, Cliente_SelectOne, Cliente_Update
--   Mascota_Delete, Mascota_Insert, Mascota_Search, Mascota_SelectAll,
--   Mascota_SelectByCliente, Mascota_SelectOne, Mascota_Update
```

### Paso 3: Configurar Connection String

El archivo `UI/App.config` ya está configurado con:

```xml
<connectionStrings>
  <add name="VetCareConString"
       connectionString="Data Source=localhost;Initial Catalog=VetCareDB;Integrated Security=True;TrustServerCertificate=True"/>
</connectionStrings>
```

**Nota:** Si usas SQL Server con autenticación SQL, modifica así:
```xml
connectionString="Data Source=localhost;Initial Catalog=VetCareDB;User Id=tuUsuario;Password=tuPassword;TrustServerCertificate=True"
```

### Paso 4: Compilar Solución

```bash
msbuild "Sistema Veterinario VetCare.sln" /p:Configuration=Debug
```

## 🎯 Uso de la Capa BLL

### Ejemplo 1: Registrar un Cliente

```csharp
using BLL;
using DomainModel;

// Obtener instancia del BLL
var clienteBLL = ClienteBLL.Current;

// Crear nuevo cliente
var cliente = new Cliente
{
    Nombre = "Juan",
    Apellido = "Pérez",
    DNI = "12345678",
    Telefono = "1234567890",
    Email = "juan.perez@email.com",
    Direccion = "Calle Falsa 123",
    Activo = true
};

try
{
    // Caso de uso: Registrar cliente (incluye validaciones)
    var clienteCreado = clienteBLL.RegistrarCliente(cliente);
    MessageBox.Show($"Cliente creado con ID: {clienteCreado.IdCliente}");
}
catch (ArgumentException ex)
{
    // Validación de negocio falló
    MessageBox.Show($"Error de validación: {ex.Message}");
}
catch (InvalidOperationException ex)
{
    // Regla de negocio falló (ej: DNI duplicado)
    MessageBox.Show($"Error: {ex.Message}");
}
```

### Ejemplo 2: Registrar una Mascota

```csharp
using BLL;
using DomainModel;

var mascotaBLL = MascotaBLL.Current;

var mascota = new Mascota
{
    IdCliente = idCliente, // GUID del dueño
    Nombre = "Firulais",
    Especie = "Perro",
    Raza = "Labrador",
    FechaNacimiento = new DateTime(2020, 5, 15),
    Sexo = "Macho",
    Peso = 25.5m,
    Color = "Dorado",
    Observaciones = "Vacunado al día"
};

try
{
    var mascotaCreada = mascotaBLL.RegistrarMascota(mascota);
    MessageBox.Show($"Mascota registrada: {mascotaCreada.Nombre}");
}
catch (Exception ex)
{
    MessageBox.Show($"Error: {ex.Message}");
}
```

### Ejemplo 3: Buscar Clientes

```csharp
var clienteBLL = ClienteBLL.Current;

// Listar todos los clientes
var todosLosClientes = clienteBLL.ListarTodosLosClientes();

// Listar solo activos
var clientesActivos = clienteBLL.ListarClientesActivos();

// Buscar por criterio (nombre, apellido, DNI, email)
var resultados = clienteBLL.BuscarClientes("Pérez");

// Buscar por DNI específico
var cliente = clienteBLL.BuscarClientePorDNI("12345678");
```

### Ejemplo 4: Cliente con sus Mascotas

```csharp
var clienteBLL = ClienteBLL.Current;

// Obtener cliente con todas sus mascotas
var clienteCompleto = clienteBLL.ObtenerClienteConMascotas(idCliente);

Console.WriteLine($"Cliente: {clienteCompleto.NombreCompleto}");
Console.WriteLine($"Mascotas: {clienteCompleto.Mascotas.Count}");

foreach (var mascota in clienteCompleto.Mascotas)
{
    Console.WriteLine($"  - {mascota.Nombre} ({mascota.Especie})");
}
```

### Ejemplo 5: Transferir Mascota a Otro Dueño

```csharp
var mascotaBLL = MascotaBLL.Current;

try
{
    var mascota = mascotaBLL.TransferirMascota(
        idMascota: idMascotaATransferir,
        idNuevoDueno: idNuevoCliente
    );

    MessageBox.Show($"{mascota.Nombre} ahora pertenece a otro dueño");
}
catch (InvalidOperationException ex)
{
    MessageBox.Show(ex.Message);
}
```

## 📊 Casos de Uso Disponibles

### ClienteBLL

| Método | Descripción |
|--------|-------------|
| `RegistrarCliente(Cliente)` | Crea un nuevo cliente con validaciones |
| `ModificarCliente(Cliente)` | Actualiza datos de cliente existente |
| `EliminarCliente(Guid)` | Elimina cliente (cascada a mascotas) |
| `DesactivarCliente(Guid)` | Baja lógica de cliente |
| `ActivarCliente(Guid)` | Reactiva un cliente |
| `ObtenerClientePorId(Guid)` | Obtiene un cliente por ID |
| `BuscarClientePorDNI(string)` | Busca cliente por DNI |
| `ListarTodosLosClientes()` | Lista todos los clientes |
| `ListarClientesActivos()` | Lista solo clientes activos |
| `BuscarClientes(string)` | Busca por nombre/apellido/DNI/email |
| `ObtenerClienteConMascotas(Guid)` | Cliente + sus mascotas |
| `ObtenerEstadisticasCliente(Guid)` | Estadísticas del cliente |

### MascotaBLL

| Método | Descripción |
|--------|-------------|
| `RegistrarMascota(Mascota)` | Registra nueva mascota con validaciones |
| `ModificarMascota(Mascota)` | Actualiza datos de mascota |
| `TransferirMascota(Guid, Guid)` | Cambia de dueño |
| `EliminarMascota(Guid)` | Elimina mascota físicamente |
| `DesactivarMascota(Guid)` | Baja lógica (fallecimiento/pérdida) |
| `ActivarMascota(Guid)` | Reactiva una mascota |
| `ObtenerMascotaPorId(Guid)` | Obtiene mascota por ID |
| `ListarTodasLasMascotas()` | Lista todas las mascotas |
| `ListarMascotasActivas()` | Solo mascotas activas |
| `ListarMascotasPorCliente(Guid)` | Mascotas de un cliente |
| `BuscarMascotas(string)` | Busca por nombre/especie/raza |
| `ObtenerEstadisticasPorEspecie()` | Cantidad por especie |
| `ObtenerMascotasProximoCumpleanos()` | Próximos 30 días |
| `ObtenerDetalleMascota(Guid)` | Mascota + dueño + edad |

## 🔒 Validaciones de Negocio

### Cliente

- ✅ Nombre y apellido obligatorios (mínimo 2 caracteres)
- ✅ DNI obligatorio y único (mínimo 6 caracteres)
- ✅ Email con formato válido (si se proporciona)
- ✅ Teléfono mínimo 7 caracteres (si se proporciona)
- ✅ DNI no duplicado al crear/modificar

### Mascota

- ✅ Nombre obligatorio (mínimo 2 caracteres)
- ✅ Especie obligatoria (mínimo 2 caracteres)
- ✅ Sexo debe ser "Macho" o "Hembra"
- ✅ Fecha de nacimiento no puede ser futura
- ✅ Peso no negativo y menor a 1000 kg
- ✅ Cliente dueño debe existir y estar activo

## 🗄️ Esquema de Base de Datos

### Tabla Cliente

| Campo | Tipo | Descripción |
|-------|------|-------------|
| IdCliente | UNIQUEIDENTIFIER | PK |
| Nombre | NVARCHAR(100) | Obligatorio |
| Apellido | NVARCHAR(100) | Obligatorio |
| DNI | NVARCHAR(20) | Único, obligatorio |
| Telefono | NVARCHAR(20) | Opcional |
| Email | NVARCHAR(150) | Opcional |
| Direccion | NVARCHAR(255) | Opcional |
| FechaRegistro | DATETIME | Auto |
| Activo | BIT | Default 1 |

**Índices:** DNI, Apellido

### Tabla Mascota

| Campo | Tipo | Descripción |
|-------|------|-------------|
| IdMascota | UNIQUEIDENTIFIER | PK |
| IdCliente | UNIQUEIDENTIFIER | FK → Cliente |
| Nombre | NVARCHAR(100) | Obligatorio |
| Especie | NVARCHAR(50) | Obligatorio |
| Raza | NVARCHAR(100) | Opcional |
| FechaNacimiento | DATE | Obligatorio |
| Sexo | NVARCHAR(10) | 'Macho'/'Hembra' |
| Peso | DECIMAL(6,2) | En kilogramos |
| Color | NVARCHAR(50) | Opcional |
| Observaciones | NVARCHAR(500) | Opcional |
| FechaRegistro | DATETIME | Auto |
| Activo | BIT | Default 1 |

**Índices:** IdCliente, Especie
**FK:** IdCliente → Cliente.IdCliente (CASCADE DELETE)

## 🎨 Patrones Utilizados

1. **Repository Pattern (Específico)**: Repositorios con métodos del dominio, no genéricos
2. **Adapter Pattern**: Conversión DataRow → Entidad
3. **Singleton Pattern**: BLL y Repositorios
4. **Use Case Pattern**: Métodos de BLL representan casos de uso de negocio
5. **Layered Architecture**: UI → BLL → DAL → DB

## ⚠️ Notas Importantes

1. **Dos Bases de Datos Separadas:**
   - `SeguridadBiblioteca` (módulo de seguridad/usuarios)
   - `VetCareDB` (módulo de negocio/clientes/mascotas)

2. **Stored Procedures:** Toda comunicación con BD usa SPs, no queries directos

3. **Eliminación en Cascada:** Al eliminar un cliente, se eliminan sus mascotas automáticamente (FK CASCADE)

4. **Validaciones en Dos Niveles:**
   - BD: Constraints y checks
   - BLL: Validaciones de negocio y reglas complejas

5. **Singleton:** Los BLL y Repositorios son singleton (`.Current`)

## 🧪 Testing

### Probar Instalación

```sql
USE VetCareDB;

-- Insertar cliente de prueba
DECLARE @IdCliente UNIQUEIDENTIFIER = NEWID();
EXEC Cliente_Insert @IdCliente, 'Juan', 'Pérez', '12345678', '555-1234',
     'juan@test.com', 'Calle 123', 1;

-- Verificar
SELECT * FROM Cliente;

-- Insertar mascota de prueba
DECLARE @IdMascota UNIQUEIDENTIFIER = NEWID();
EXEC Mascota_Insert @IdMascota, @IdCliente, 'Firulais', 'Perro', 'Labrador',
     '2020-01-01', 'Macho', 25.5, 'Dorado', 'Prueba', 1;

-- Verificar
SELECT * FROM Mascota;
```

## 📞 Soporte

Para problemas con la arquitectura, revisa:
1. Connection string en `UI/App.config`
2. Base de datos creada: `USE VetCareDB;`
3. Stored procedures instalados: `SELECT * FROM sys.procedures;`
4. Documentación en `CLAUDE.md`
