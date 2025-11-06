# Guía de Instalación - Módulo de Bitácora

Esta guía te ayudará a instalar completamente el módulo de Bitácora en tu sistema VetCare.

## Pre-requisitos

- SQL Server instalado y corriendo
- Base de datos SecurityVet existente (del módulo de seguridad)
- Usuario con permisos de administrador en SecurityVet
- El sistema VetCare ya instalado y funcionando

## Pasos de Instalación

### 1. Instalar Base de Datos

Abrir **Command Prompt** o **PowerShell** y ejecutar:

```bash
cd "C:\Users\AlanC\Desktop\UAI\Proyecto Final 3 año\Mi proyecto\Proyecto en codigo\Sistema Veterinaria VetCare"

# Instalación completa de la bitácora (tabla + stored procedures)
sqlcmd -S localhost -i "Database\40_EJECUTAR_TODO_BITACORA.sql"

# Crear la Patente y asignarla al ROL_Administrador
sqlcmd -S localhost -i "Database\42_CrearPatenteBitacora.sql"
```

**Salida esperada:**
```
Tabla Bitacora creada exitosamente
Stored Procedures de Bitacora creados exitosamente
Patente "FormBitacora" creada exitosamente
Patente asignada al ROL_Administrador exitosamente
```

### 2. Compilar el Proyecto

```bash
# Desde la carpeta raíz del proyecto
build.bat
```

Si no tienes `build.bat`, usar MSBuild directamente:

```bash
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "Sistema Veterinario VetCare.sln" /p:Configuration=Debug
```

### 3. Verificar Instalación

#### 3.1. Verificar Base de Datos

```sql
USE SecurityVet;
GO

-- Verificar que la tabla existe
SELECT COUNT(*) AS ExisteTabla
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = 'Bitacora';
-- Debe retornar: 1

-- Verificar stored procedures
SELECT COUNT(*) AS CantidadSPs
FROM sys.procedures
WHERE name LIKE 'Bitacora%';
-- Debe retornar: 7

-- Verificar Patente
SELECT p.FormName, p.MenuItemName, f.Nombre AS AsignadaA
FROM Patente p
LEFT JOIN FamiliaPatente fp ON fp.idPatente = p.IdPatente
LEFT JOIN Familia f ON f.IdFamilia = fp.idFamilia
WHERE p.FormName = 'FormBitacora';
-- Debe mostrar: FormBitacora | Bitácora del Sistema | ROL_Administrador
```

#### 3.2. Verificar en la Aplicación

1. Iniciar la aplicación VetCare
2. Hacer login con usuario **admin** (o cualquier usuario con ROL_Administrador)
3. En el menú principal, buscar la opción **"Bitácora del Sistema"**
4. Hacer clic en la opción
5. Debe abrir el formulario de Bitácora mostrando registros de login recientes

## Resultado Esperado

Después de la instalación:

✅ Tabla `Bitacora` creada en la base de datos SecurityVet
✅ 7 Stored Procedures creados
✅ Patente "FormBitacora" creada
✅ Patente asignada al ROL_Administrador
✅ Opción "Bitácora del Sistema" visible en el menú (solo para admins)
✅ Formulario funcional con filtros y exportación
✅ Login/Logout se registran automáticamente

## Características del Sistema

### Eventos Registrados Automáticamente
- ✅ Login exitoso
- ✅ Login fallido
- ✅ Logout
- ✅ Violaciones de DVH (integridad de datos)

### Características del Formulario
- 📅 Filtro por rango de fechas
- 👤 Filtro por usuario
- 📦 Filtro por módulo
- ⚡ Filtro por acción
- 🎯 Filtro por criticidad
- 📊 Exportación a Excel
- 🎨 Código de colores por criticidad:
  - 🔴 Crítico (rojo)
  - 🟠 Error (naranja)
  - 🟡 Advertencia (amarillo)
  - ⚪ Info (blanco)

## Siguientes Pasos (Opcional)

Para agregar registros de bitácora en otros módulos del sistema (Clientes, Mascotas, Citas, etc.), ver:

📖 **Database/README_BITACORA.md** - Guía completa con ejemplos de código

### Ejemplo Rápido

Para registrar operaciones de Clientes, agregar en `ClienteBLL.cs`:

```csharp
using ServicesSecurity.Services;

public static Cliente RegistrarCliente(Cliente cliente)
{
    var nuevoCliente = ClienteRepository.Current.Crear(cliente);

    // Registrar en bitácora
    var usuario = LoginService.GetUsuarioLogueado();
    if (usuario != null)
    {
        Bitacora.Current.RegistrarAlta(
            usuario.IdUsuario,
            usuario.Nombre,
            "Clientes",
            "Cliente",
            nuevoCliente.IdCliente.ToString(),
            $"Cliente registrado: {nuevoCliente.Nombre} {nuevoCliente.Apellido}"
        );
    }

    return nuevoCliente;
}
```

## Solución de Problemas

### Problema: "Tabla Bitacora ya existe"
**Solución:** La tabla ya está instalada. Continuar con el paso de crear la Patente.

### Problema: No aparece la opción en el menú
**Causa 1:** El usuario no tiene ROL_Administrador
**Solución:** Asignar el rol de Administrador al usuario

**Causa 2:** La Patente no está asignada
**Solución:** Ejecutar `Database\42_CrearPatenteBitacora.sql`

### Problema: Error al abrir el formulario
**Causa:** La tabla no existe
**Solución:** Ejecutar `Database\40_EJECUTAR_TODO_BITACORA.sql`

### Problema: No se registran eventos
**Causa:** Los stored procedures no existen
**Solución:**
```bash
sqlcmd -S localhost -i "Database\41_StoredProceduresBitacora.sql"
```

## Archivos Creados/Modificados

### Nuevos Archivos

**Base de Datos:**
- `Database/40_CrearTablaBitacora.sql`
- `Database/41_StoredProceduresBitacora.sql`
- `Database/40_EJECUTAR_TODO_BITACORA.sql`
- `Database/42_CrearPatenteBitacora.sql`
- `Database/README_BITACORA.md`

**DomainModel:**
- `ServicesSeguridad/DomainModel/Security/Bitacora.cs`
- `ServicesSeguridad/DomainModel/Security/AccionBitacora.cs`
- `ServicesSeguridad/DomainModel/Security/CriticidadBitacora.cs`

**DAL:**
- `ServicesSeguridad/DAL/Contracts/IBitacoraRepository.cs`
- `ServicesSeguridad/DAL/Implementations/BitacoraRepository.cs`
- `ServicesSeguridad/DAL/Implementations/Adapter/BitacoraAdapter.cs`

**BLL:**
- `ServicesSeguridad/BLL/BitacoraBLL.cs`

**UI:**
- `UI/WinUi/Administración/FormBitacora.cs`
- `UI/WinUi/Administración/FormBitacora.Designer.cs`

### Archivos Modificados

**Services:**
- `ServicesSeguridad/Services/Bitacora.cs` (extendido con métodos de BD)
- `ServicesSeguridad/Services/LoginService.cs` (agregados registros de login/logout)

**UI:**
- `UI/WinUi/Administración/menu.cs` (agregado registro de logout)

## Soporte

Para más información, consultar:
- `Database/README_BITACORA.md` - Documentación completa del módulo
- `CLAUDE.md` - Arquitectura general del sistema

---

**¡La instalación está completa! El sistema de Bitácora está listo para usar.**
