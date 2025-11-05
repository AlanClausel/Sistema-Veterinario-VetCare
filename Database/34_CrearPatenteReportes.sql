-- =============================================
-- Script: 34_CrearPatenteReportes.sql
-- Descripción: Crea la patente "Reportes" y la asigna
--              a ROL_Administrador, ROL_Veterinario y ROL_Recepcionista
-- Base de datos: SecurityVet
-- =============================================

USE SecurityVet;
GO

PRINT '════════════════════════════════════════════════════'
PRINT 'Creando patente Reportes...'
PRINT '════════════════════════════════════════════════════'

-- =============================================
-- Variables para IDs
-- =============================================
DECLARE @IdPatenteReportes UNIQUEIDENTIFIER = NEWID();
DECLARE @IdFamiliaAdmin UNIQUEIDENTIFIER;
DECLARE @IdFamiliaVeterinario UNIQUEIDENTIFIER;
DECLARE @IdFamiliaRecepcionista UNIQUEIDENTIFIER;

-- Obtener IDs de las familias (roles)
SELECT @IdFamiliaAdmin = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador';
SELECT @IdFamiliaVeterinario = IdFamilia FROM Familia WHERE Nombre = 'ROL_Veterinario';
SELECT @IdFamiliaRecepcionista = IdFamilia FROM Familia WHERE Nombre = 'ROL_Recepcionista';

-- =============================================
-- Crear Patente: Reportes
-- =============================================
IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'FormReportes')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (
        @IdPatenteReportes,
        'FormReportes',
        'Reportes',
        200,
        'Permite acceder a reportes del sistema: citas por período, estadísticas, exportación'
    );

    PRINT '✓ Patente FormReportes creada';
END
ELSE
BEGIN
    SELECT @IdPatenteReportes = IdPatente FROM Patente WHERE FormName = 'FormReportes';
    PRINT '✓ Patente FormReportes ya existe';
END

-- =============================================
-- Asignar Patente a ROL_Administrador
-- =============================================
IF @IdFamiliaAdmin IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaAdmin AND idPatente = @IdPatenteReportes)
    BEGIN
        INSERT INTO FamiliaPatente (idFamilia, idPatente)
        VALUES (@IdFamiliaAdmin, @IdPatenteReportes);

        PRINT '✓ Patente Reportes asignada a ROL_Administrador';
    END
    ELSE
    BEGIN
        PRINT '✓ Patente Reportes ya estaba asignada a ROL_Administrador';
    END
END

-- =============================================
-- Asignar Patente a ROL_Veterinario
-- =============================================
IF @IdFamiliaVeterinario IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaVeterinario AND idPatente = @IdPatenteReportes)
    BEGIN
        INSERT INTO FamiliaPatente (idFamilia, idPatente)
        VALUES (@IdFamiliaVeterinario, @IdPatenteReportes);

        PRINT '✓ Patente Reportes asignada a ROL_Veterinario';
    END
    ELSE
    BEGIN
        PRINT '✓ Patente Reportes ya estaba asignada a ROL_Veterinario';
    END
END

-- =============================================
-- Asignar Patente a ROL_Recepcionista
-- =============================================
IF @IdFamiliaRecepcionista IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteReportes)
    BEGIN
        INSERT INTO FamiliaPatente (idFamilia, idPatente)
        VALUES (@IdFamiliaRecepcionista, @IdPatenteReportes);

        PRINT '✓ Patente Reportes asignada a ROL_Recepcionista';
    END
    ELSE
    BEGIN
        PRINT '✓ Patente Reportes ya estaba asignada a ROL_Recepcionista';
    END
END

-- =============================================
-- Resumen
-- =============================================
PRINT ''
PRINT '════════════════════════════════════════════════════'
PRINT 'Patente Reportes creada exitosamente'
PRINT '════════════════════════════════════════════════════'
PRINT ''
PRINT 'Patente: FormReportes'
PRINT 'Nombre en menú: Reportes'
PRINT 'Orden: 200'
PRINT ''
PRINT 'Asignada a:'
PRINT '  - ROL_Administrador'
PRINT '  - ROL_Veterinario'
PRINT '  - ROL_Recepcionista'
PRINT ''

GO
