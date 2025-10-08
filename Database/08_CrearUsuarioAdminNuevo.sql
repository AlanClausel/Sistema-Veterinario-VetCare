-- =====================================================
-- Script: Crear Usuario Admin Nuevo (UTF-16 Compatible)
-- Descripción: Crea un usuario admin desde cero con
--              hashes calculados correctamente en UTF-16
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT '========================================';
PRINT 'Creando usuario admin nuevo';
PRINT '========================================';
PRINT '';

-- Paso 1: Eliminar usuario admin anterior si existe
PRINT 'Eliminando usuario admin anterior si existe...';

DECLARE @IdUsuarioAnterior UNIQUEIDENTIFIER

SELECT @IdUsuarioAnterior = IdUsuario
FROM Usuario
WHERE Nombre = 'admin'

IF @IdUsuarioAnterior IS NOT NULL
BEGIN
    -- Eliminar relaciones
    DELETE FROM UsuarioFamilia WHERE idUsuario = @IdUsuarioAnterior
    DELETE FROM UsuarioPatente WHERE idUsuario = @IdUsuarioAnterior

    -- Eliminar usuario
    DELETE FROM Usuario WHERE IdUsuario = @IdUsuarioAnterior

    PRINT '  ✓ Usuario admin anterior eliminado'
END
ELSE
BEGIN
    PRINT '  → No existía usuario admin anterior'
END

PRINT '';

-- Paso 2: Crear nuevo usuario admin
PRINT 'Creando nuevo usuario admin...';

DECLARE @IdUsuarioAdmin UNIQUEIDENTIFIER = NEWID()
DECLARE @NombreAdmin NVARCHAR(100) = N'admin'
DECLARE @EmailAdmin NVARCHAR(150) = N'admin@vetcare.com'
DECLARE @PasswordPlain NVARCHAR(100) = N'admin123'
DECLARE @IdiomaAdmin NVARCHAR(10) = N'es-AR'

-- Calcular hash de la contraseña con UTF-16 (NVARCHAR)
-- IMPORTANTE: La N antes del string hace que SQL use UTF-16
DECLARE @ClaveHasheada NVARCHAR(256) = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', @PasswordPlain), 2)

-- Calcular DVH
DECLARE @DVH NVARCHAR(64)
DECLARE @DatosParaDVH NVARCHAR(MAX) =
    UPPER(CAST(@IdUsuarioAdmin AS NVARCHAR(36))) + N'|' +
    @NombreAdmin + N'|' +
    @ClaveHasheada + N'|' +
    N'1'

SET @DVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', @DatosParaDVH), 2)

-- Insertar usuario
INSERT INTO Usuario (IdUsuario, Nombre, Email, Clave, Activo, IdiomaPreferido, DVH)
VALUES (@IdUsuarioAdmin, @NombreAdmin, @EmailAdmin, @ClaveHasheada, 1, @IdiomaAdmin, @DVH);

PRINT '  ✓ Usuario admin creado'
PRINT ''
PRINT '  IdUsuario: ' + CAST(@IdUsuarioAdmin AS NVARCHAR(36))
PRINT '  Nombre: ' + @NombreAdmin
PRINT '  Email: ' + @EmailAdmin
PRINT '  Password: ' + @PasswordPlain + ' (¡CAMBIAR EN PRODUCCIÓN!)'
PRINT '  Clave (hash): ' + LEFT(@ClaveHasheada, 32) + '...'
PRINT '  DVH: ' + LEFT(@DVH, 32) + '...'
PRINT '';

-- Paso 3: Asignar ROL_Administrador
PRINT 'Asignando ROL_Administrador...';

DECLARE @IdRolAdmin UNIQUEIDENTIFIER

SELECT @IdRolAdmin = IdFamilia
FROM Familia
WHERE Nombre = 'ROL_Administrador'

IF @IdRolAdmin IS NULL
BEGIN
    PRINT '  ERROR: No se encontró ROL_Administrador'
    RAISERROR('No existe el ROL_Administrador en la base de datos', 16, 1)
    RETURN
END

INSERT INTO UsuarioFamilia (idUsuario, idFamilia)
VALUES (@IdUsuarioAdmin, @IdRolAdmin);

PRINT '  ✓ ROL_Administrador asignado'
PRINT '';

-- Paso 4: Verificación
PRINT 'Verificación del usuario creado:';
PRINT '';

SELECT
    'Usuario' = Nombre,
    'Email' = Email,
    'Activo' = CASE WHEN Activo = 1 THEN 'Sí' ELSE 'No' END,
    'Hash Password' = LEFT(Clave, 16) + '...',
    'DVH' = LEFT(DVH, 16) + '...',
    'DVH Válido' = CASE
        WHEN DVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256',
            UPPER(CAST(IdUsuario AS NVARCHAR(36))) + N'|' + Nombre + N'|' + Clave + N'|' + CASE WHEN Activo = 1 THEN N'1' ELSE N'0' END
        ), 2) THEN 'SÍ ✓'
        ELSE 'NO ✗'
    END
FROM Usuario
WHERE Nombre = 'admin';

PRINT '';
PRINT '========================================';
PRINT 'Usuario admin creado exitosamente';
PRINT '========================================';
PRINT '';
PRINT 'CREDENCIALES:';
PRINT '  Usuario: admin';
PRINT '  Password: admin123';
PRINT '';
PRINT '⚠ IMPORTANTE: Todos los hashes usan UTF-16 (NVARCHAR)';
PRINT '⚠ CAMBIAR la contraseña en producción';
PRINT '';

GO
