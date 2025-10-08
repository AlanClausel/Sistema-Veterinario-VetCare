# Migración: Roles como Familias Composite

## 📋 Resumen del Cambio

Se migró el sistema de roles de un campo `Usuario.Rol` (string) a usar el patrón **Composite** con Familias especiales.

### Antes
```
Usuario
├── Rol: "Administrador" (campo string)
└── Permisos: List<Component>
```

### Después
```
Usuario
└── Permisos: List<Component>
    └── Familia: "ROL_Administrador" (Familia especial)
        └── [otras familias y patentes...]
```

## 🔄 Cambios Realizados

### Base de Datos
1. ✅ Creadas 3 Familias especiales:
   - `ROL_Administrador`
   - `ROL_Veterinario`
   - `ROL_Recepcionista`

2. ✅ Migrados roles existentes a tabla `UsuarioFamilia`

3. ⏳ Pendiente: Eliminar columna `Usuario.Rol` (comentado por seguridad)

### Código
1. ✅ `Familia.cs`: Agregadas propiedades `EsRol` y `ObtenerNombreRol()`
2. ✅ `Usuario.cs`: Eliminado campo `Rol`, agregados métodos `ObtenerFamiliaRol()`, `ObtenerNombreRol()`, `TieneRol()`
3. ✅ `UsuarioBLL.cs`: Métodos actualizados para usar Familias en lugar de strings
4. ✅ `Login.cs`: Actualizado para obtener rol desde Composite
5. ✅ `gestionUsuarios.cs`: ComboBox ahora muestra Familias de rol

## 🚀 Pasos para Migrar

### 1. Backup de Base de Datos
```sql
BACKUP DATABASE SecurityDB
TO DISK = 'C:\Backups\SecurityDB_PreMigracion.bak'
WITH FORMAT;
```

### 2. Ejecutar Script de Migración
```bash
sqlcmd -S DESKTOP-UFSCO3C -d SecurityDB -i "Database\MigracionRolesComposite.sql"
```

O desde SQL Server Management Studio:
- Abrir `Database\MigracionRolesComposite.sql`
- Ejecutar

### 3. Compilar Aplicación
```bash
msbuild "Sistema Veterinaria VetCare.sln" /p:Configuration=Debug
```

### 4. Probar Funcionalidad

#### Probar Login:
1. Iniciar aplicación
2. Login con usuario Administrador
3. Verificar que redirige a menú correcto
4. Verificar que muestra el perfil "Administrador"

#### Probar Gestión de Usuarios:
1. Ir a "Gestión de Usuarios"
2. Crear nuevo usuario
3. Seleccionar rol del ComboBox (debe mostrar "ROL_Administrador", etc.)
4. Verificar que se guarda correctamente
5. Modificar usuario y cambiar rol
6. Verificar que actualiza

### 5. Eliminar Columna Rol (Opcional)

**Solo después de verificar que todo funciona:**

1. Abrir `Database\MigracionRolesComposite.sql`
2. Ir a **PASO 4**
3. Descomentar el bloque:
```sql
ALTER TABLE Usuario DROP COLUMN Rol
```
4. Ejecutar solo esa sección

## 🔍 Verificación

### Query para verificar migración:
```sql
-- Ver usuarios y sus roles (nuevo sistema)
SELECT
    u.Nombre AS Usuario,
    f.Nombre AS FamiliaRol
FROM Usuario u
LEFT JOIN UsuarioFamilia uf ON u.IdUsuario = uf.idUsuario
LEFT JOIN Familia f ON uf.idFamilia = f.IdFamilia
WHERE f.Nombre LIKE 'ROL_%'
ORDER BY u.Nombre
```

### Query para detectar problemas:
```sql
-- Usuarios sin rol asignado
SELECT
    u.IdUsuario,
    u.Nombre
FROM Usuario u
WHERE NOT EXISTS (
    SELECT 1 FROM UsuarioFamilia uf
    INNER JOIN Familia f ON uf.idFamilia = f.IdFamilia
    WHERE uf.idUsuario = u.IdUsuario
    AND f.Nombre LIKE 'ROL_%'
)
```

## 🐛 Troubleshooting

### Problema: Usuario no puede login después de migración
**Solución**: Verificar que el usuario tenga una Familia de rol asignada:
```sql
SELECT * FROM UsuarioFamilia WHERE idUsuario = '<guid-del-usuario>'
```

### Problema: ComboBox de roles vacío
**Solución**: Verificar que existen las Familias de rol:
```sql
SELECT * FROM Familia WHERE Nombre LIKE 'ROL_%'
```

### Problema: Error al crear usuario
**Solución**: Verificar que las Familias de rol tienen IdComponent (IdFamilia) válido

## 📝 Notas Importantes

1. **No eliminar la columna Rol inmediatamente**: Primero verifica que todo funciona
2. **Los permisos granulares se mantienen**: Esta migración solo afecta el rol, no los permisos individuales
3. **Compatibilidad**: El código antiguo que usaba `usuario.Rol` debe actualizarse a `usuario.ObtenerNombreRol()`

## 🔙 Rollback (si algo sale mal)

```sql
-- 1. Restaurar backup
RESTORE DATABASE SecurityDB
FROM DISK = 'C:\Backups\SecurityDB_PreMigracion.bak'
WITH REPLACE;

-- 2. Revertir código a versión anterior desde git
git checkout <commit-anterior>
```

## ✅ Checklist Final

- [ ] Backup de base de datos realizado
- [ ] Script de migración ejecutado exitosamente
- [ ] Aplicación compilada sin errores
- [ ] Login funciona con todos los roles
- [ ] Crear usuario funciona
- [ ] Modificar usuario funciona
- [ ] Cambiar rol de usuario funciona
- [ ] Eliminar usuario funciona
- [ ] Gestión de permisos (familias/patentes) funciona
- [ ] Columna Rol eliminada (opcional)

## 📞 Soporte

Si encuentras problemas, revisa:
1. Los logs de la aplicación
2. Los mensajes de error del script SQL
3. La documentación en `CLAUDE.md`
