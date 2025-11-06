# Módulo de Bitácora - Sistema VetCare

## Descripción

El módulo de Bitácora proporciona un sistema completo de auditoría para rastrear todos los eventos críticos del sistema VetCare. Registra acciones de usuarios, operaciones CRUD, errores, violaciones de seguridad y más.

## Características

- ✅ Registro automático de login/logout
- ✅ Registro de operaciones CRUD (Alta, Baja, Modificación)
- ✅ Registro de excepciones y errores
- ✅ Detección de violaciones de DVH (integridad de datos)
- ✅ Registro de intentos de acceso no autorizado
- ✅ Interfaz gráfica para consultar y filtrar registros
- ✅ Exportación a Excel
- ✅ Niveles de criticidad (Info, Advertencia, Error, Crítico)
- ✅ Filtrado por fecha, usuario, módulo, acción y criticidad

## Instalación

### Paso 1: Ejecutar Scripts SQL

Ejecutar los siguientes scripts en orden:

```bash
# Opción A: Instalación completa (recomendada)
sqlcmd -S localhost -i "Database\40_EJECUTAR_TODO_BITACORA.sql"

# Opción B: Scripts individuales
sqlcmd -S localhost -i "Database\40_CrearTablaBitacora.sql"
sqlcmd -S localhost -i "Database\41_StoredProceduresBitacora.sql"

# Crear la Patente para el formulario (necesario para el menú)
sqlcmd -S localhost -i "Database\42_CrearPatenteBitacora.sql"
```

### Paso 2: Compilar el Proyecto

El código ya está integrado en las siguientes capas:

- **DomainModel**: `ServicesSecurity/DomainModel/Security/Bitacora.cs`
- **DAL**: `ServicesSecurity/DAL/Implementations/BitacoraRepository.cs`
- **BLL**: `ServicesSecurity/BLL/BitacoraBLL.cs`
- **Services**: `ServicesSecurity/Services/Bitacora.cs` (extendido)
- **UI**: `UI/WinUi/Administración/FormBitacora.cs`

Compilar el proyecto:

```bash
build.bat
```

### Paso 3: Verificar la Instalación

1. Iniciar sesión en el sistema con un usuario **Administrador**
2. En el menú principal, verificar que aparezca la opción **"Bitácora del Sistema"**
3. Hacer clic para abrir el formulario de bitácora
4. Deberían aparecer registros de login recientes

## Uso

### Consultar la Bitácora (Administradores)

1. Iniciar sesión como **Administrador**
2. Click en **"Bitácora del Sistema"** en el menú principal
3. Aplicar filtros opcionales:
   - **Fecha Desde/Hasta**: Rango de fechas
   - **Módulo**: Sistema, Clientes, Citas, Usuarios, etc.
   - **Acción**: Login, Alta, Baja, Modificación, etc.
   - **Criticidad**: Info, Advertencia, Error, Crítico
4. Click en **"Buscar"** para aplicar filtros
5. Los registros se muestran con código de colores:
   - 🔴 **Rojo**: Eventos críticos
   - 🟠 **Naranja**: Errores
   - 🟡 **Amarillo**: Advertencias
   - ⚪ **Blanco**: Información

### Exportar Registros

1. Aplicar los filtros deseados
2. Click en **"Exportar a Excel"**
3. Seleccionar ubicación y nombre de archivo
4. El archivo `.xlsx` incluirá todos los registros filtrados

### Limpiar Registros Antiguos (Mantenimiento)

Para limpiar registros antiguos y liberar espacio, ejecutar:

```sql
USE SecurityVet;
GO

-- Eliminar registros anteriores a 6 meses
DECLARE @FechaLimite DATETIME = DATEADD(MONTH, -6, GETDATE());
EXEC Bitacora_DeleteOlderThan @FechaLimite;
GO
```

## Eventos Registrados Automáticamente

### Eventos de Autenticación
- ✅ Login exitoso
- ✅ Login fallido (usuario no encontrado)
- ✅ Login fallido (contraseña incorrecta)
- ✅ Logout

### Eventos de Seguridad
- ✅ Violación de DVH (datos alterados en BD)
- ✅ Intento de acceso no autorizado
- ✅ Excepciones del sistema

## Agregar Registros de Bitácora Personalizados

### Desde BLL o Services

```csharp
using ServicesSecurity.Services;
using ServicesSecurity.DomainModel.Security;

// Registrar un alta (INSERT)
Bitacora.Current.RegistrarAlta(
    usuarioLogueado.IdUsuario,
    usuarioLogueado.Nombre,
    "Clientes",
    "Cliente",
    cliente.IdCliente.ToString(),
    $"Cliente creado: {cliente.Nombre} {cliente.Apellido}"
);

// Registrar una baja (DELETE)
Bitacora.Current.RegistrarBaja(
    usuarioLogueado.IdUsuario,
    usuarioLogueado.Nombre,
    "Clientes",
    "Cliente",
    cliente.IdCliente.ToString(),
    $"Cliente eliminado: {cliente.Nombre} {cliente.Apellido}"
);

// Registrar una modificación (UPDATE)
Bitacora.Current.RegistrarModificacion(
    usuarioLogueado.IdUsuario,
    usuarioLogueado.Nombre,
    "Clientes",
    "Cliente",
    cliente.IdCliente.ToString(),
    $"Cliente modificado: {cliente.Nombre} {cliente.Apellido}"
);

// Registrar un error personalizado
Bitacora.Current.RegistrarError(
    "Error al enviar email de confirmación",
    usuarioLogueado?.IdUsuario,
    usuarioLogueado?.Nombre ?? "Sistema",
    "Citas"
);

// Registrar una excepción
try
{
    // código que puede fallar
}
catch (Exception ex)
{
    Bitacora.Current.RegistrarExcepcion(
        ex,
        usuarioLogueado?.IdUsuario,
        usuarioLogueado?.Nombre,
        "NombreDelModulo"
    );
}
```

### Ejemplo: Agregar a ClienteBLL

```csharp
// En ClienteBLL.cs
public static Cliente RegistrarCliente(Cliente cliente)
{
    try
    {
        // Validaciones...
        ValidarCliente(cliente);

        // Crear en BD
        var nuevoCliente = ClienteRepository.Current.Crear(cliente);

        // Registrar en bitácora
        var usuarioLogueado = LoginService.GetUsuarioLogueado();
        if (usuarioLogueado != null)
        {
            Bitacora.Current.RegistrarAlta(
                usuarioLogueado.IdUsuario,
                usuarioLogueado.Nombre,
                "Clientes",
                "Cliente",
                nuevoCliente.IdCliente.ToString(),
                $"Cliente registrado: {nuevoCliente.Nombre} {nuevoCliente.Apellido}, DNI: {nuevoCliente.DNI}"
            );
        }

        return nuevoCliente;
    }
    catch (Exception ex)
    {
        // Registrar excepción
        var usuarioLogueado = LoginService.GetUsuarioLogueado();
        Bitacora.Current.RegistrarExcepcion(ex, usuarioLogueado?.IdUsuario, usuarioLogueado?.Nombre, "Clientes");
        throw;
    }
}
```

## Estructura de la Base de Datos

### Tabla: Bitacora

| Campo | Tipo | Descripción |
|-------|------|-------------|
| IdBitacora | UNIQUEIDENTIFIER | PK, identificador único |
| IdUsuario | UNIQUEIDENTIFIER | FK a Usuario (puede ser NULL) |
| NombreUsuario | VARCHAR(50) | Nombre del usuario (denormalizado) |
| FechaHora | DATETIME | Fecha y hora del evento |
| Modulo | VARCHAR(50) | Módulo del sistema (ej: "Clientes", "Citas") |
| Accion | VARCHAR(50) | Acción realizada (ej: "Login", "Alta", "Baja") |
| Descripcion | VARCHAR(500) | Descripción detallada del evento |
| Tabla | VARCHAR(50) | Tabla afectada (opcional) |
| IdRegistro | VARCHAR(100) | ID del registro afectado (opcional) |
| Criticidad | VARCHAR(20) | Nivel: "Info", "Advertencia", "Error", "Critico" |
| IP | VARCHAR(45) | Dirección IP (opcional, soporta IPv6) |

### Stored Procedures Disponibles

- **Bitacora_Insert**: Insertar nuevo registro
- **Bitacora_SelectAll**: Obtener todos los registros
- **Bitacora_SelectByFiltros**: Buscar con filtros
- **Bitacora_SelectByUsuario**: Registros de un usuario
- **Bitacora_SelectByRangoFechas**: Registros en rango de fechas
- **Bitacora_DeleteOlderThan**: Eliminar registros antiguos
- **Bitacora_GetEstadisticas**: Obtener estadísticas

## Constantes Disponibles

### AccionBitacora

```csharp
ServicesSecurity.DomainModel.Security.AccionBitacora
```

- `Login`, `LoginFallido`, `Logout`
- `Alta`, `Baja`, `Modificacion`, `Consulta`
- `AsignacionPermiso`, `RevocacionPermiso`, `CambioRol`
- `Error`, `Excepcion`, `ViolacionDVH`, `AccesoNoAutorizado`
- `AgendarCita`, `CancelarCita`, `FinalizarConsulta`, `MovimientoStock`

### CriticidadBitacora

```csharp
ServicesSecurity.DomainModel.Security.CriticidadBitacora
```

- `Info`: Eventos normales
- `Advertencia`: Situaciones que requieren atención
- `Error`: Errores recuperables
- `Critico`: Eventos críticos de seguridad

## Consultas SQL Útiles

### Ver últimos 100 registros

```sql
USE SecurityVet;
SELECT TOP 100 *
FROM Bitacora
ORDER BY FechaHora DESC;
```

### Ver eventos críticos del último mes

```sql
USE SecurityVet;
SELECT *
FROM Bitacora
WHERE FechaHora >= DATEADD(MONTH, -1, GETDATE())
  AND Criticidad IN ('Critico', 'Error')
ORDER BY FechaHora DESC;
```

### Ver actividad de un usuario específico

```sql
USE SecurityVet;
SELECT *
FROM Bitacora
WHERE NombreUsuario = 'admin'
ORDER BY FechaHora DESC;
```

### Ver estadísticas por módulo (último mes)

```sql
USE SecurityVet;
EXEC Bitacora_GetEstadisticas
    @FechaDesde = NULL,  -- NULL = últimos 30 días
    @FechaHasta = NULL;
```

### Contar logins por día (última semana)

```sql
USE SecurityVet;
SELECT
    CAST(FechaHora AS DATE) AS Fecha,
    COUNT(*) AS TotalLogins,
    COUNT(CASE WHEN Accion = 'LoginFallido' THEN 1 END) AS LoginsFallidos
FROM Bitacora
WHERE Accion IN ('Login', 'LoginFallido')
  AND FechaHora >= DATEADD(DAY, -7, GETDATE())
GROUP BY CAST(FechaHora AS DATE)
ORDER BY Fecha DESC;
```

## Notas Importantes

1. **Permisos**: Solo usuarios con rol **ROL_Administrador** pueden ver la bitácora
2. **Performance**: La tabla usa índices en FechaHora, IdUsuario, Modulo, Accion y Criticidad
3. **Almacenamiento**: Considerar limpieza periódica de registros antiguos (recomendado: cada 6-12 meses)
4. **Seguridad**: Los registros de bitácora no se pueden modificar ni eliminar desde la interfaz (solo consultar)
5. **Recursión**: El servicio Bitacora silencia sus propios errores para evitar loops infinitos

## Troubleshooting

### No aparece la opción en el menú

Verificar que la Patente esté asignada al rol Administrador:

```sql
USE SecurityVet;
SELECT f.Nombre AS Rol, p.MenuItemName AS Patente
FROM FamiliaPatente fp
JOIN Familia f ON f.IdFamilia = fp.idFamilia
JOIN Patente p ON p.IdPatente = fp.idPatente
WHERE p.FormName = 'FormBitacora';
```

Si no aparece, ejecutar:

```bash
sqlcmd -S localhost -i "Database\42_CrearPatenteBitacora.sql"
```

### Error al abrir el formulario

Verificar que la tabla exista:

```sql
USE SecurityVet;
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Bitacora';
```

Si no existe, ejecutar:

```bash
sqlcmd -S localhost -i "Database\40_EJECUTAR_TODO_BITACORA.sql"
```

### No se registran eventos

Verificar que los stored procedures existan:

```sql
USE SecurityVet;
SELECT name FROM sys.procedures WHERE name LIKE 'Bitacora%';
```

Debe mostrar al menos 7 stored procedures.

## Contacto y Soporte

Para reportar problemas o solicitar nuevas funcionalidades, contactar al equipo de desarrollo del Sistema VetCare.
