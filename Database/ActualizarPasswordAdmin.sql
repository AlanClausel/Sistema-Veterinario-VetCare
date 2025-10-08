-- =====================================================
-- Script: Actualizar Password del Admin
-- Descripción: Actualiza el hash de la contraseña del admin
--              para que coincida con el formato de C#
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT 'Actualizando password del usuario admin...'
PRINT ''

-- Password en texto plano
DECLARE @PasswordTextoPlano NVARCHAR(100) = N'admin123'

-- Calcular hash usando NVARCHAR (UTF-16) para coincidir con C# Encoding.Unicode
DECLARE @ClaveHasheada NVARCHAR(256) = CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', @PasswordTextoPlano), 2)

PRINT 'Password texto plano: admin123'
PRINT 'Hash calculado: ' + @ClaveHasheada
PRINT ''

-- Actualizar el usuario admin
UPDATE Usuario
SET Clave = @ClaveHasheada
WHERE Nombre = 'admin'

-- Recalcular DVH para el admin
DECLARE @IdUsuario UNIQUEIDENTIFIER
DECLARE @Nombre NVARCHAR(100)
DECLARE @Activo BIT

SELECT @IdUsuario = IdUsuario, @Nombre = Nombre, @Activo = Activo
FROM Usuario
WHERE Nombre = 'admin'

-- Calcular nuevo DVH
DECLARE @DatosParaDVH NVARCHAR(MAX) = UPPER(CAST(@IdUsuario AS NVARCHAR(36))) + N'|' + @Nombre + N'|' + @ClaveHasheada + N'|' + CASE WHEN @Activo = 1 THEN N'1' ELSE N'0' END
DECLARE @DVH NVARCHAR(64) = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', @DatosParaDVH), 2)

-- Actualizar DVH
UPDATE Usuario
SET DVH = @DVH
WHERE Nombre = 'admin'

PRINT '✓ Password del admin actualizado correctamente'
PRINT '✓ DVH recalculado'
PRINT ''
PRINT 'Ahora puede hacer login con:'
PRINT '  Usuario: admin'
PRINT '  Password: admin123'
PRINT ''

-- Verificar
SELECT
    Nombre,
    Email,
    Clave AS HashPassword,
    DVH,
    Activo
FROM Usuario
WHERE Nombre = 'admin'

GO
