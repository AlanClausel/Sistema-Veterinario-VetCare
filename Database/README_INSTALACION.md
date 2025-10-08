# 🗄️ Instalación Base de Datos - Sistema VetCare

## 📋 Descripción

Scripts SQL para crear la base de datos **SecurityDB** con arquitectura Composite para permisos y **Dígito Verificador Horizontal (DVH)** en la tabla Usuario.

---

## 🚀 Instalación Rápida

### Opción 1: Script Maestro (Recomendado)
```bash
sqlcmd -S DESKTOP-UFSCO3C -i "Database\00_EJECUTAR_TODO.sql"
```

### Opción 2: SQL Server Management Studio
1. Abrir `00_EJECUTAR_TODO.sql`
2. Ejecutar (F5)

### Opción 3: Paso a Paso
```bash
sqlcmd -S DESKTOP-UFSCO3C -i "Database\01_CrearBaseDatos.sql"
sqlcmd -S DESKTOP-UFSCO3C -i "Database\02_CrearTablas.sql"
sqlcmd -S DESKTOP-UFSCO3C -i "Database\03_DatosIniciales.sql"
```

---

## 📁 Estructura de Scripts

| Script | Descripción |
|--------|-------------|
| `00_EJECUTAR_TODO.sql` | **Script maestro** - Ejecuta todo en orden |
| `01_CrearBaseDatos.sql` | Elimina y crea BD SecurityDB |
| `02_CrearTablas.sql` | Crea todas las tablas del sistema |
| `03_DatosIniciales.sql` | Inserta roles, patentes y usuario admin |

---

## 🔐 Credenciales Iniciales

**Usuario Administrador:**
- **Usuario:** `admin`
- **Email:** `admin@biblioteca.edu`
- **Password:** `admin123`
- **Rol:** ROL_Administrador

⚠️ **IMPORTANTE:** Cambiar la contraseña en producción

---

## 🗂️ Tablas Creadas

### Tablas Principales

#### **Usuario** (con DVH)
```sql
IdUsuario          UNIQUEIDENTIFIER PRIMARY KEY
Nombre             NVARCHAR(100) UNIQUE
Clave              NVARCHAR(256)      -- Password hasheado
FechaCreacion      DATETIME
FechaUltimoAcceso  DATETIME
Activo             BIT
DVH                NVARCHAR(64)       -- ⭐ Dígito Verificador Horizontal
```

#### **Familia** (Roles y Grupos de Permisos)
```sql
IdFamilia      UNIQUEIDENTIFIER PRIMARY KEY
Nombre         NVARCHAR(100) UNIQUE
Descripcion    NVARCHAR(255)
```
**Tipos de Familia:**
- Familias con nombre `ROL_*` → **Roles** (ej: ROL_Administrador)
- Otras familias → **Grupos de permisos** (ej: Gestión Usuarios)

#### **Patente** (Permisos Atómicos)
```sql
IdPatente      UNIQUEIDENTIFIER PRIMARY KEY
FormName       NVARCHAR(100)       -- Nombre del formulario
MenuItemName   NVARCHAR(100)       -- Texto del menú
Orden          INT
Descripcion    NVARCHAR(255)
```

### Tablas de Relación (Composite Pattern)

| Tabla | Descripción |
|-------|-------------|
| **UsuarioFamilia** | Usuario → Rol o Grupo |
| **UsuarioPatente** | Usuario → Permiso directo |
| **FamiliaFamilia** | Familia → Familia (jerarquía) |
| **FamiliaPatente** | Familia → Patente |

### Tablas de Sistema

| Tabla | Descripción |
|-------|-------------|
| **Language** | Traducciones i18n |
| **Logger** | Logs del sistema |

---

## 🛡️ Dígito Verificador Horizontal (DVH)

### ¿Qué es el DVH?
El **Dígito Verificador Horizontal** es un hash que se calcula por cada registro (fila) de la tabla Usuario. Sirve para detectar si alguien modificó directamente los datos en la base de datos.

**Ubicación:** Tabla `Usuario`, columna `DVH`

**Cálculo:**
```csharp
string datos = $"{IdUsuario}|{Nombre}|{Clave}|{Activo}";
DVH = SHA256(datos);
```

**Propósito:** Detectar modificaciones no autorizadas en un registro específico

### Validación en Código C#

```csharp
// Al leer usuario de BD
public Usuario SelectOne(Guid id)
{
    var usuario = /* leer de BD */;

    // Verificar DVH
    string dvhCalculado = CalcularDVH(usuario);
    if (dvhCalculado != usuario.DVH)
        throw new IntegridadException("DVH no coincide - registro alterado");

    return usuario;
}

// Al insertar usuario
public void Insert(Usuario usuario)
{
    usuario.DVH = CalcularDVH(usuario);
    /* insertar en BD */
}

// Al actualizar usuario
public void Update(Usuario usuario)
{
    usuario.DVH = CalcularDVH(usuario); // Recalcular
    /* actualizar en BD */
}

// Método auxiliar
private string CalcularDVH(Usuario u)
{
    string datos = $"{u.IdUsuario}|{u.Nombre}|{u.Clave}|{(u.Activo ? 1 : 0)}";
    return CryptographyService.HashPassword(datos); // SHA256
}
```

### Ejemplo de uso

```csharp
// Crear usuario
var usuario = new Usuario
{
    IdUsuario = Guid.NewGuid(),
    Nombre = "jperez",
    Clave = CryptographyService.HashPassword("password123"),
    Activo = true
};
usuario.DVH = CalcularDVH(usuario);

// Insertar
UsuarioRepository.Insert(usuario);

// Al recuperar, validar integridad
var usuarioRecuperado = UsuarioRepository.SelectOne(usuario.IdUsuario);
if (CalcularDVH(usuarioRecuperado) != usuarioRecuperado.DVH)
{
    throw new Exception("¡Datos alterados!");
}
```

---

## 📊 Datos Iniciales Insertados

### Roles (Familias ROL_*)
- ✅ **ROL_Administrador** (acceso total administración)
- ✅ **ROL_Veterinario** (sin permisos - pendiente módulos negocio)
- ✅ **ROL_Recepcionista** (sin permisos - pendiente módulos negocio)

### Grupos de Permisos
- **Gestión de Usuarios** (4 patentes): Alta, Baja, Modificar, Ver
- **Gestión de Permisos** (4 patentes): Alta Familia, Baja Familia, Modificar Familia, Asignar Permisos
- **Configuración** (3 patentes): Configuración Sistema, Ver Logs, Backup/Restore

**Total:** 11 patentes de administración

### Jerarquía Composite

```
ROL_Administrador
├── Gestión de Usuarios
│   ├── Alta de Usuario
│   ├── Baja de Usuario
│   ├── Modificar Usuario
│   └── Ver Usuarios
├── Gestión de Permisos
│   ├── Alta de Familia
│   ├── Baja de Familia
│   ├── Modificar Familia
│   └── Asignar Permisos
└── Configuración
    ├── Configuración Sistema
    ├── Ver Logs
    └── Backup/Restore

ROL_Veterinario
└── (sin permisos - agregar según módulos de negocio)

ROL_Recepcionista
└── (sin permisos - agregar según módulos de negocio)
```

---

## ✅ Verificación de Instalación

### 1. Verificar Tablas
```sql
USE SecurityDB;
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME;
```

**Debe mostrar:** 10 tablas (Usuario, Familia, Patente, UsuarioFamilia, UsuarioPatente, FamiliaFamilia, FamiliaPatente, Language, Logger)

### 2. Verificar Usuario Admin
```sql
SELECT IdUsuario, Nombre, Activo, DVH FROM Usuario;
```

**Debe mostrar:** 1 usuario (admin)

### 3. Verificar Roles
```sql
SELECT * FROM Familia WHERE Nombre LIKE 'ROL_%';
```

**Debe mostrar:** 3 roles

### 4. Verificar Jerarquía
```sql
-- Ver permisos del ROL_Administrador
SELECT
    f.Nombre AS Rol,
    ff.Nombre AS GrupoPermiso,
    p.MenuItemName AS Permiso
FROM Familia f
LEFT JOIN FamiliaFamilia ffrel ON f.IdFamilia = ffrel.IdFamiliaPadre
LEFT JOIN Familia ff ON ffrel.IdFamiliaHijo = ff.IdFamilia
LEFT JOIN FamiliaPatente fp ON ff.IdFamilia = fp.idFamilia
LEFT JOIN Patente p ON fp.idPatente = p.IdPatente
WHERE f.Nombre = 'ROL_Administrador'
ORDER BY ff.Nombre, p.Orden;
```

### 5. Verificar DVH del Admin
```sql
SELECT
    Nombre,
    DVH,
    CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256',
        CAST(IdUsuario AS NVARCHAR(36)) + '|' + Nombre + '|' + Clave + '|' + CAST(Activo AS CHAR(1))
    ), 2) AS DVH_Calculado,
    CASE
        WHEN DVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256',
            CAST(IdUsuario AS NVARCHAR(36)) + '|' + Nombre + '|' + Clave + '|' + CAST(Activo AS CHAR(1))
        ), 2) THEN 'VÁLIDO ✓'
        ELSE 'INVÁLIDO ✗'
    END AS Estado
FROM Usuario;
```

---

## 🔧 Configuración App.config

Actualizar la connection string en `UI/App.config`:

```xml
<connectionStrings>
    <add name="ServicesConString"
         connectionString="Data Source=TU_SERVIDOR;Initial Catalog=SecurityDB;Integrated Security=True"/>
</connectionStrings>
```

Reemplazar `TU_SERVIDOR` con tu instancia SQL Server.

---

## 🐛 Troubleshooting

### Error: "Cannot drop database SecurityDB because it is currently in use"
```sql
USE master;
ALTER DATABASE SecurityDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE SecurityDB;
```

### Error: "Login failed for user"
- Verificar que tienes permisos de administrador en SQL Server
- Usar autenticación Windows (Integrated Security=True)

### DVH no coincide después de migración
```sql
-- Recalcular DVH de todos los usuarios
UPDATE Usuario
SET DVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256',
    CAST(IdUsuario AS NVARCHAR(36)) + '|' + Nombre + '|' + Clave + '|' + CAST(Activo AS CHAR(1))
), 2);
```

---

## 📝 Próximos Pasos

1. ✅ Ejecutar scripts de instalación
2. ✅ Verificar credenciales admin
3. ✅ Actualizar connection string en App.config
4. ✅ Compilar aplicación
5. ✅ Probar login con admin/admin123
6. ⚠️ Cambiar password de admin
7. ✅ Crear usuarios adicionales desde la aplicación

---

## 🔒 Seguridad

- ✅ Passwords hasheados con SHA256
- ✅ DVH (Dígito Verificador Horizontal) implementado en tabla Usuario
- ✅ Constraints de integridad referencial
- ✅ Validación de longitud de campos
- ✅ Validación de DVH en cada lectura de usuario
- ⚠️ Cambiar password admin en producción
- ⚠️ No exponer DVH en logs ni UI
- ⚠️ Recalcular DVH en cada INSERT/UPDATE de usuario

---

## 📚 Documentación Adicional

- **Arquitectura Composite:** Ver `CLAUDE.md`
- **Migración desde versión anterior:** Ver `README_MIGRACION.md`
- **Código C#:** Ver `ServicesSeguridad/`
