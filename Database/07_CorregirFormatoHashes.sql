-- =============================================
-- Script: Corregir Formato de Hashes
-- Descripción: Convierte todos los hashes a MAYÚSCULAS
--              para coincidir con el formato de C# (ToString("X2"))
-- =============================================

USE SeguridadBiblioteca;
GO

PRINT '========================================';
PRINT 'Corrigiendo formato de hashes';
PRINT '========================================';
PRINT '';

-- Mostrar estado actual
PRINT 'Estado actual de hashes:';
SELECT
    Nombre,
    LEFT(Clave, 16) + '...' AS Clave_Preview,
    CASE
        WHEN Clave = UPPER(Clave) THEN 'MAYÚSCULAS ✓'
        WHEN Clave = LOWER(Clave) THEN 'minúsculas (convertir)'
        ELSE 'MIXTO'
    END AS FormatoClave
FROM Usuario;
PRINT '';

-- Paso 1: Convertir todos los hashes de contraseñas a MAYÚSCULAS
PRINT 'Convirtiendo hashes de contraseñas a MAYÚSCULAS...';
UPDATE Usuario
SET Clave = UPPER(Clave);

PRINT '  ✓ Hashes de contraseñas convertidos';
PRINT '';

-- Paso 2: Recalcular DVH con los nuevos hashes
PRINT 'Recalculando DVH...';
UPDATE Usuario
SET DVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256',
    UPPER(CAST(IdUsuario AS NVARCHAR(36))) + '|' +
    Nombre + '|' +
    Clave + '|' +
    CASE WHEN Activo = 1 THEN '1' ELSE '0' END
), 2);

PRINT '  ✓ DVH recalculado';
PRINT '';

-- Verificar resultado
PRINT 'Estado después de corrección:';
SELECT
    Nombre,
    LEFT(Clave, 16) + '...' AS Clave_Preview,
    CASE
        WHEN Clave = UPPER(Clave) THEN 'MAYÚSCULAS ✓'
        ELSE 'ERROR'
    END AS FormatoClave,
    LEFT(DVH, 16) + '...' AS DVH_Preview
FROM Usuario;
PRINT '';

-- Verificación de integridad
PRINT 'Verificación de integridad del DVH:';
SELECT
    Nombre,
    CASE
        WHEN DVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256',
            UPPER(CAST(IdUsuario AS NVARCHAR(36))) + '|' + Nombre + '|' + Clave + '|' + CASE WHEN Activo = 1 THEN '1' ELSE '0' END
        ), 2) THEN 'VÁLIDO ✓'
        ELSE 'INVÁLIDO ✗'
    END AS EstadoDVH
FROM Usuario;

PRINT '';
PRINT '========================================';
PRINT 'Corrección completada exitosamente';
PRINT '========================================';
PRINT '';
PRINT 'Ahora los hashes están en MAYÚSCULAS y coinciden con C# (ToString("X2"))';
PRINT '';

GO
