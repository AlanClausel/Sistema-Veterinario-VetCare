-- =============================================
-- Script: Ver logs del sistema
-- Descripción: Muestra los últimos logs para debugging
-- =============================================

USE SeguridadBiblioteca;
GO

-- Ver los últimos 50 logs ordenados por fecha
SELECT TOP 50
    IdLog,
    FechaHora,
    Nivel,
    Usuario,
    Mensaje
FROM Logger
ORDER BY FechaHora DESC;
GO

-- Ver solo errores y críticos recientes
PRINT '';
PRINT '========================================';
PRINT 'ERRORES Y CRÍTICOS RECIENTES:';
PRINT '========================================';

SELECT
    FechaHora,
    Nivel,
    Usuario,
    Mensaje
FROM Logger
WHERE Nivel IN ('Error', 'Critical')
ORDER BY FechaHora DESC;
GO
