# 📝 Cómo Insertar Usuarios Manualmente por SQL

## ⚠️ IMPORTANTE sobre DVH

**SÍ puedes** insertar usuarios directamente por SQL, **PERO** debes calcular el DVH correctamente.

Si **NO calculas el DVH** o lo calculas **mal** → El login fallará con error de "datos alterados"

---

## 🚀 Método Recomendado: Usar el Script Template

### Paso 1: Abrir el script
```bash
Database\06_InsertarUsuarioManual.sql
```

### Paso 2: Modificar SOLO estas líneas (al inicio del script):

```sql
DECLARE @NombreUsuario NVARCHAR(100) = 'jperez'  -- ← CAMBIAR
DECLARE @Email NVARCHAR(100) = 'jperez@vetcare.com'  -- ← CAMBIAR
DECLARE @PasswordTextoPlano NVARCHAR(100) = 'password123'  -- ← CAMBIAR
DECLARE @Activo BIT = 1  -- ← 1=activo, 0=inactivo
DECLARE @IdiomaPreferido NVARCHAR(10) = 'es-AR'  -- ← 'es-AR' o 'en-GB'
DECLARE @RolNombre NVARCHAR(100) = 'ROL_Veterinario'  -- ← CAMBIAR
```

### Paso 3: Ejecutar
```bash
sqlcmd -S DESKTOP-UFSCO3C -i "Database\06_InsertarUsuarioManual.sql"
```

O desde SQL Server Management Studio:
- Abrir el archivo
- Modificar los parámetros
- F5 (ejecutar)

---

## ✅ Ejemplos de Uso

### Ejemplo 1: Crear Veterinario

```sql
DECLARE @NombreUsuario NVARCHAR(100) = 'drlopez'
DECLARE @Email NVARCHAR(100) = 'dra.lopez@vetcare.com'
DECLARE @PasswordTextoPlano NVARCHAR(100) = 'veterinario123'
DECLARE @Activo BIT = 1
DECLARE @IdiomaPreferido NVARCHAR(10) = 'es-AR'
DECLARE @RolNombre NVARCHAR(100) = 'ROL_Veterinario'
```

**Login:**
- Usuario: `drlopez`
- Password: `veterinario123`

### Ejemplo 2: Crear Recepcionista

```sql
DECLARE @NombreUsuario NVARCHAR(100) = 'mgarcia'
DECLARE @Email NVARCHAR(100) = 'mgarcia@vetcare.com'
DECLARE @PasswordTextoPlano NVARCHAR(100) = 'recepcion123'
DECLARE @Activo BIT = 1
DECLARE @IdiomaPreferido NVARCHAR(10) = 'es-AR'
DECLARE @RolNombre NVARCHAR(100) = 'ROL_Recepcionista'
```

**Login:**
- Usuario: `mgarcia`
- Password: `recepcion123`

### Ejemplo 3: Crear Administrador

```sql
DECLARE @NombreUsuario NVARCHAR(100) = 'admin2'
DECLARE @Email NVARCHAR(100) = 'admin2@vetcare.com'
DECLARE @PasswordTextoPlano NVARCHAR(100) = 'admin456'
DECLARE @Activo BIT = 1
DECLARE @IdiomaPreferido NVARCHAR(10) = 'en-GB'
DECLARE @RolNombre NVARCHAR(100) = 'ROL_Administrador'
```

**Login:**
- Usuario: `admin2`
- Password: `admin456`

---

## 🔐 Fórmula DVH Correcta

**Si quieres hacer INSERT manual sin el template:**

```sql
-- 1. Generar GUID
DECLARE @IdUsuario UNIQUEIDENTIFIER = NEWID()

-- 2. Hashear password (SHA256)
DECLARE @ClaveHasheada NVARCHAR(256) =
    CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'password_texto_plano'), 2)

-- 3. Calcular DVH (MUY IMPORTANTE: GUID en MAYÚSCULAS)
DECLARE @DVH NVARCHAR(64) = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256',
    UPPER(CAST(@IdUsuario AS NVARCHAR(36))) + '|' +
    'nombre_usuario' + '|' +
    @ClaveHasheada + '|' +
    '1'  -- 1=activo, 0=inactivo
), 2)

-- 4. Insertar
INSERT INTO Usuario (IdUsuario, Nombre, Email, Clave, Activo, IdiomaPreferido, DVH)
VALUES (@IdUsuario, 'nombre_usuario', 'email@example.com', @ClaveHasheada, 1, 'es-AR', @DVH)
```

---

## ❌ Errores Comunes

### 1. DVH incorrecto - GUID en minúsculas
```sql
-- ❌ MAL - GUID en minúsculas
CAST(@IdUsuario AS NVARCHAR(36))  -- genera: f7de898f-61c7-...

-- ✅ BIEN - GUID en MAYÚSCULAS
UPPER(CAST(@IdUsuario AS NVARCHAR(36)))  -- genera: F7DE898F-61C7-...
```

### 2. DVH incorrecto - Incluir Email o Idioma
```sql
-- ❌ MAL - NO incluir Email ni Idioma en DVH
@IdUsuario + '|' + @Nombre + '|' + @Email + '|' + @Clave + '|' + @Activo + '|' + @Idioma

-- ✅ BIEN - Solo GUID|Nombre|Clave|Activo
UPPER(CAST(@IdUsuario AS NVARCHAR(36))) + '|' + @Nombre + '|' + @Clave + '|' + '1'
```

### 3. Password sin hashear
```sql
-- ❌ MAL - Password en texto plano
INSERT INTO Usuario (..., Clave, ...) VALUES (..., 'password123', ...)

-- ✅ BIEN - Password hasheada
DECLARE @Hash NVARCHAR(256) = CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', 'password123'), 2)
INSERT INTO Usuario (..., Clave, ...) VALUES (..., @Hash, ...)
```

---

## 🔍 Verificar que el DVH sea correcto

Después de insertar un usuario, verifica:

```sql
SELECT
    Nombre,
    'DVH OK' = CASE
        WHEN DVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256',
            UPPER(CAST(IdUsuario AS NVARCHAR(36))) + '|' + Nombre + '|' + Clave + '|' + CASE WHEN Activo = 1 THEN '1' ELSE '0' END
        ), 2) THEN 'SÍ ✓'
        ELSE 'NO ✗ - LOGIN FALLARÁ'
    END
FROM Usuario
WHERE Nombre = 'nombre_del_usuario'
```

---

## 📋 Roles Disponibles

```sql
SELECT Nombre, Descripcion FROM Familia WHERE Nombre LIKE 'ROL_%'
```

| Nombre | Descripción |
|--------|-------------|
| ROL_Administrador | Acceso completo al sistema |
| ROL_Veterinario | Atención de mascotas |
| ROL_Recepcionista | Gestión de turnos y clientes |

---

## 🎯 Resumen

1. ✅ **USA** el script template `06_InsertarUsuarioManual.sql`
2. ✅ **MODIFICA** solo los parámetros al inicio
3. ✅ **EJECUTA** el script
4. ✅ El DVH se calcula **automáticamente** y **correctamente**
5. ✅ El usuario podrá hacer login sin problemas

**NO intentes hacer INSERT manual sin calcular el DVH correctamente.**
