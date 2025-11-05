-- =============================================
-- Script: 28_CrearPatentesModuloVeterinario.sql
-- Descripción: Crea las patentes para el módulo de veterinario
--              y las asigna a la familia ROL_Veterinario
-- Base de datos: SecurityVet
-- =============================================

USE SecurityVet;
GO

PRINT '════════════════════════════════════════════════════'
PRINT 'Creando patentes para módulo Veterinario...'
PRINT '════════════════════════════════════════════════════'

-- =============================================
-- Variables para IDs
-- =============================================
DECLARE @IdFamiliaVeterinario UNIQUEIDENTIFIER;
DECLARE @IdPatenteMisCitas UNIQUEIDENTIFIER = NEWID();
DECLARE @IdPatenteHistorialClinico UNIQUEIDENTIFIER = NEWID();

-- Obtener ID de la familia ROL_Veterinario
SELECT @IdFamiliaVeterinario = IdFamilia
FROM Familia
WHERE Nombre = 'ROL_Veterinario';

-- Si no existe la familia, crearla
IF @IdFamiliaVeterinario IS NULL
BEGIN
    SET @IdFamiliaVeterinario = NEWID();

    INSERT INTO Familia (IdFamilia, Nombre, Descripcion, FechaCreacion)
    VALUES (@IdFamiliaVeterinario, 'ROL_Veterinario', 'Rol de Veterinario - Acceso a gestión de consultas médicas', GETDATE());

    PRINT '✓ Familia ROL_Veterinario creada';
END
ELSE
BEGIN
    PRINT '✓ Familia ROL_Veterinario ya existe';
END

-- =============================================
-- Crear Patentes del Módulo Veterinario
-- =============================================

-- Patente: Mis Citas
IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'MisCitas')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (
        @IdPatenteMisCitas,
        'MisCitas',
        'Mis Citas',
        100,
        'Permite al veterinario ver sus citas asignadas e iniciar consultas médicas'
    );

    PRINT '✓ Patente MisCitas creada';
END
ELSE
BEGIN
    SELECT @IdPatenteMisCitas = IdPatente FROM Patente WHERE FormName = 'MisCitas';
    PRINT '✓ Patente MisCitas ya existe';
END

-- Patente: Historial Clínico
IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'FormHistorialClinico')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (
        @IdPatenteHistorialClinico,
        'FormHistorialClinico',
        'Historial Clínico',
        110,
        'Permite al veterinario consultar el historial médico completo de las mascotas'
    );

    PRINT '✓ Patente FormHistorialClinico creada';
END
ELSE
BEGIN
    SELECT @IdPatenteHistorialClinico = IdPatente FROM Patente WHERE FormName = 'FormHistorialClinico';
    PRINT '✓ Patente FormHistorialClinico ya existe';
END

-- =============================================
-- Asignar Patentes a ROL_Veterinario
-- =============================================

-- Asignar MisCitas a ROL_Veterinario
IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaVeterinario AND idPatente = @IdPatenteMisCitas)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente)
    VALUES (@IdFamiliaVeterinario, @IdPatenteMisCitas);

    PRINT '✓ Patente MisCitas asignada a ROL_Veterinario';
END
ELSE
BEGIN
    PRINT '✓ Patente MisCitas ya estaba asignada a ROL_Veterinario';
END

-- Asignar Historial Clínico a ROL_Veterinario
IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaVeterinario AND idPatente = @IdPatenteHistorialClinico)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente)
    VALUES (@IdFamiliaVeterinario, @IdPatenteHistorialClinico);

    PRINT '✓ Patente FormHistorialClinico asignada a ROL_Veterinario';
END
ELSE
BEGIN
    PRINT '✓ Patente FormHistorialClinico ya estaba asignada a ROL_Veterinario';
END

-- =============================================
-- Resumen
-- =============================================
PRINT ''
PRINT '════════════════════════════════════════════════════'
PRINT 'Patentes del módulo Veterinario creadas exitosamente'
PRINT '════════════════════════════════════════════════════'
PRINT ''
PRINT 'Familia: ROL_Veterinario'
PRINT 'Patentes creadas: 2'
PRINT '  - Mis Citas (MisCitas)'
PRINT '  - Historial Clínico (FormHistorialClinico)'
PRINT ''
PRINT 'NOTA: Los formularios FormConsultaMedica, FormHistorialDetallado'
PRINT '      y FormDetalleConsulta no tienen patente porque se abren'
PRINT '      desde otros formularios, no desde el menú'
PRINT ''

GO
