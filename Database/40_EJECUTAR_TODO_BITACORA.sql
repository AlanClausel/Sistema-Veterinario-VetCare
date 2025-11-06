-- =============================================
-- Script de instalación completa del módulo de Bitácora
-- Ejecutar este archivo para crear la tabla y todos los SPs
-- =============================================

PRINT '========================================';
PRINT 'Instalación del Módulo de Bitácora';
PRINT '========================================';
PRINT '';

-- 1. Crear tabla Bitacora
PRINT 'Paso 1: Creando tabla Bitacora...';
:r 40_CrearTablaBitacora.sql
PRINT '';

-- 2. Crear stored procedures
PRINT 'Paso 2: Creando stored procedures...';
:r 41_StoredProceduresBitacora.sql
PRINT '';

PRINT '========================================';
PRINT 'Instalación completada exitosamente';
PRINT '========================================';
PRINT '';
PRINT 'La bitácora está lista para usar.';
PRINT 'Recuerde ejecutar también el script para crear la Patente:';
PRINT '  42_CrearPatenteBitacora.sql';
