# Implementación del Patrón Unit of Work

## Resumen

Se implementó el patrón **Unit of Work** para garantizar la atomicidad de operaciones críticas que involucran múltiples transacciones en la base de datos SecurityVet.

## Análisis Previo

### Operaciones que SÍ necesitaban Unit of Work

1. **FamiliaBLL.ActualizarPatentesDeRol()** (ServicesSeguridad/BLL/FamiliaBLL.cs:17)
   - Ejecuta múltiples DELETE e INSERT de patentes en loops
   - Sin transacción: si falla en medio, queda estado inconsistente
   - Riesgo: patentes eliminadas pero nuevas no agregadas

2. **UsuarioBLL.CambiarRol()** (ServicesSeguridad/BLL/UsuarioBLL.cs:203)
   - Elimina roles anteriores + inserta nuevo rol
   - Sin transacción: si falla, el usuario podría quedar sin rol
   - Riesgo crítico: usuario sin permisos de acceso

### Operaciones que NO necesitaban Unit of Work

- **ConsultaMedicaBLL.FinalizarConsulta()** (VetCareNegocio/BLL/ConsultaMedicaBLL.cs:78)
  - Ya tiene transacción en el stored procedure `ConsultaMedica_Finalizar`
  - El SP maneja BEGIN TRANSACTION/COMMIT/ROLLBACK internamente (Database/27_CrearSP_ConsultaMedica.sql:241)

## Archivos Creados

### 1. Interfaz IUnitOfWork
**Archivo:** `ServicesSeguridad/DAL/Contracts/IUnitOfWork.cs`

```csharp
public interface IUnitOfWork : IDisposable
{
    SqlConnection Connection { get; }
    SqlTransaction Transaction { get; }
    void BeginTransaction();
    void Commit();
    void Rollback();
}
```

Define el contrato para coordinar múltiples operaciones en una transacción atómica.

### 2. Implementación SecurityUnitOfWork
**Archivo:** `ServicesSeguridad/DAL/Implementations/SecurityUnitOfWork.cs`

Implementación concreta para la base de datos **SecurityVet**:
- Abre conexión a SecurityVet en el constructor
- Maneja transacciones SQL con BEGIN/COMMIT/ROLLBACK
- Implementa IDisposable para liberar recursos automáticamente
- Manejo seguro de errores con rollback automático en caso de fallo

## Archivos Modificados

### 3. SqlHelper extendido
**Archivo:** `ServicesSeguridad/DAL/Tools/SqlHelper.cs` (líneas 94-128)

Se agregaron métodos sobrecargados para soportar transacciones:

```csharp
public static Int32 ExecuteNonQuery(SqlConnection connection, SqlTransaction transaction,
    String commandText, CommandType commandType, params SqlParameter[] parameters)

public static Object ExecuteScalar(SqlConnection connection, SqlTransaction transaction,
    String commandText, CommandType commandType, params SqlParameter[] parameters)
```

### 4. FamiliaPatenteRepository
**Archivo:** `ServicesSeguridad/DAL/Implementations/FamiliaPatenteRepository.cs` (líneas 47-123)

Métodos modificados:
- `Insert(FamiliaPatente obj, IUnitOfWork unitOfWork)` - Versión con UoW
- `DeleteRelacion(FamiliaPatente obj, IUnitOfWork unitOfWork)` - Versión con UoW

Mantiene compatibilidad hacia atrás con versiones sin parámetros.

### 5. UsuarioFamiliaRepository
**Archivo:** `ServicesSeguridad/DAL/Implementations/UsuarioFamiliaRepository.cs` (líneas 73-167)

Métodos modificados:
- `Insert(UsuarioFamilia obj, IUnitOfWork unitOfWork)` - Versión con UoW
- `DeleteRelacion(UsuarioFamilia obj, IUnitOfWork unitOfWork)` - Versión con UoW

### 6. FamiliaBLL refactorizado
**Archivo:** `ServicesSeguridad/BLL/FamiliaBLL.cs` (líneas 12-80)

Método `ActualizarPatentesDeRol()` ahora:
1. Crea SecurityUnitOfWork con `using` para dispose automático
2. Inicia transacción con `BeginTransaction()`
3. Ejecuta todas las operaciones DELETE/INSERT dentro de la transacción
4. Confirma con `Commit()` si todo es exitoso
5. Revierte con `Rollback()` si hay cualquier error

### 7. UsuarioBLL refactorizado
**Archivo:** `ServicesSeguridad/BLL/UsuarioBLL.cs` (líneas 199-254)

Método `CambiarRol()` ahora:
1. Crea SecurityUnitOfWork
2. Elimina todos los roles antiguos del usuario
3. Asigna el nuevo rol
4. Todo en una sola transacción atómica

## Ventajas de la Implementación

### 1. Atomicidad Garantizada
- Todas las operaciones se completan o ninguna se completa
- No hay estados intermedios inconsistentes
- Rollback automático en caso de error

### 2. Compatibilidad Hacia Atrás
- Los métodos existentes sin UoW siguen funcionando
- No se rompe código existente
- Migración gradual posible

### 3. Patrón Reutilizable
- IUnitOfWork se puede implementar para otras bases de datos
- Fácil agregar más operaciones transaccionales
- Separación de responsabilidades clara

### 4. Manejo Seguro de Recursos
- `using` statement garantiza Dispose
- Conexiones cerradas automáticamente
- Transacciones liberadas correctamente

## Ejemplo de Uso

```csharp
// Antes: Sin Unit of Work (riesgo de inconsistencia)
foreach (var patente in patentesEliminar) {
    QuitarPatenteDeFamilia(idFamilia, patente.Id); // Cada llamada = nueva conexión
}
foreach (var patente in patentesAgregar) {
    AsignarPatenteAFamilia(idFamilia, patente.Id); // Si falla aquí, ya se eliminaron algunas
}

// Después: Con Unit of Work (atomicidad garantizada)
using (var uow = new SecurityUnitOfWork()) {
    uow.BeginTransaction();
    try {
        foreach (var patente in patentesEliminar) {
            repository.Delete(patente, uow); // Misma transacción
        }
        foreach (var patente in patentesAgregar) {
            repository.Insert(patente, uow); // Misma transacción
        }
        uow.Commit(); // Todo o nada
    }
    catch {
        uow.Rollback(); // Revertir todo automáticamente
        throw;
    }
}
```

## Testing

Para verificar que la implementación funciona correctamente:

### Test 1: ActualizarPatentesDeRol
1. Ir a la UI de administración de permisos (gestionPermisos.cs)
2. Seleccionar un rol (Familia)
3. Modificar sus patentes (agregar y quitar varias)
4. Verificar que todas se actualicen correctamente
5. Provocar un error (ej: desconectar DB en medio) y verificar rollback

### Test 2: CambiarRol
1. Ir a la gestión de usuarios
2. Cambiar el rol de un usuario
3. Verificar que se elimine el rol anterior y se asigne el nuevo
4. Verificar que el usuario nunca quede sin rol
5. Provocar error y verificar que mantenga el rol original

## Compilación

Para compilar el proyecto con los cambios:

```bash
# Opción 1: Usar el script batch
build.bat

# Opción 2: Usar MSBuild directamente
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "Sistema Veterinario VetCare.sln" /p:Configuration=Debug

# Opción 3: Desde Visual Studio
Abrir la solución y presionar Ctrl+Shift+B
```

## Conclusión

La implementación del patrón Unit of Work resuelve problemas críticos de consistencia de datos en operaciones que involucran múltiples transacciones relacionadas. La solución es robusta, reutilizable y mantiene compatibilidad con el código existente.

### Archivos Afectados (Resumen)
- **Nuevos:** 2 archivos (IUnitOfWork.cs, SecurityUnitOfWork.cs)
- **Modificados:** 5 archivos (SqlHelper.cs, FamiliaPatenteRepository.cs, UsuarioFamiliaRepository.cs, FamiliaBLL.cs, UsuarioBLL.cs)
- **Total:** 7 archivos

---
Implementado: 2025-01-30
Desarrollador: Claude Code
Patrón: Unit of Work (Martin Fowler - Patterns of Enterprise Application Architecture)
