-- =============================================
-- Script: 30_CorregirConstraintMatricula.sql
-- Descripción: Corrige la constraint UNIQUE en Matricula
--              para permitir múltiples valores NULL
-- =============================================

USE VetCareDB;
GO

SET QUOTED_IDENTIFIER ON;
GO

PRINT 'Corrigiendo constraint UNIQUE en Matricula...';
GO

-- Eliminar constraint UNIQUE existente
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Veterinario_Matricula' AND object_id = OBJECT_ID('Veterinario'))
BEGIN
    ALTER TABLE Veterinario DROP CONSTRAINT UQ_Veterinario_Matricula;
    PRINT 'Constraint UQ_Veterinario_Matricula eliminada';
END
GO

-- Crear UNIQUE INDEX filtrado (permite múltiples NULL)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Veterinario_Matricula' AND object_id = OBJECT_ID('Veterinario'))
BEGIN
    CREATE UNIQUE INDEX UQ_Veterinario_Matricula
    ON Veterinario(Matricula)
    WHERE Matricula IS NOT NULL;

    PRINT 'UNIQUE INDEX creado: ahora permite múltiples NULL en Matricula';
END
GO

PRINT 'Constraint corregida exitosamente';
GO
