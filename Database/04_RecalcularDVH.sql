-- =============================================
-- Script: Recalcular DVH para usuarios existentes
-- Descripción: Actualiza el DVH de todos los usuarios
--              usando la misma fórmula que C#: SHA256(IdUsuario|Nombre|Clave|Activo)
-- Uso: Ejecutar este script después de implementar la validación automática de DVH
-- =============================================

USE SeguridadBiblioteca;
GO

PRINT '========================================';
PRINT 'Recalculando DVH de todos los usuarios';
PRINT '========================================';
PRINT '';

-- Mostrar usuarios antes de la actualización
PRINT 'Estado actual de DVH:';
SELECT
    Nombre,
    CASE
        WHEN DVH IS NULL THEN 'SIN DVH'
        WHEN LEN(DVH) = 64 THEN 'OK (64 chars)'
        ELSE 'INVÁLIDO (longitud: ' + CAST(LEN(DVH) AS VARCHAR) + ')'
    END AS EstadoDVH
FROM Usuario;
PRINT '';

-- Recalcular DVH para todos los usuarios
-- IMPORTANTE: Usar UPPER() para que coincida con C# (Guid.ToString().ToUpper())
UPDATE Usuario
SET DVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256',
    UPPER(CAST(IdUsuario AS NVARCHAR(36))) + '|' +
    Nombre + '|' +
    Clave + '|' +
    CASE WHEN Activo = 1 THEN '1' ELSE '0' END
), 2);

PRINT 'DVH recalculado para todos los usuarios.';
PRINT '';

-- Verificar que todos tienen DVH válido
PRINT 'Estado después de recalcular:';
SELECT
    Nombre,
    LEFT(DVH, 16) + '...' AS DVH_Preview,
    LEN(DVH) AS Longitud,
    CASE
        WHEN DVH IS NULL THEN 'ERROR: NULL'
        WHEN LEN(DVH) = 64 THEN 'OK ✓'
        ELSE 'ERROR: Longitud inválida'
    END AS Validacion
FROM Usuario;

PRINT '';
PRINT '========================================';
PRINT 'Proceso completado';
PRINT '========================================';

-- Verificación adicional: Comprobar que el DVH es correcto
PRINT '';
PRINT 'Verificación de integridad:';
SELECT
    Nombre,
    CASE
        WHEN DVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256',
            UPPER(CAST(IdUsuario AS NVARCHAR(36))) + '|' + Nombre + '|' + Clave + '|' + CASE WHEN Activo = 1 THEN '1' ELSE '0' END
        ), 2) THEN 'VÁLIDO ✓'
        ELSE 'INVÁLIDO ✗ - RECALCULAR'
    END AS EstadoIntegridad
FROM Usuario;

GO
