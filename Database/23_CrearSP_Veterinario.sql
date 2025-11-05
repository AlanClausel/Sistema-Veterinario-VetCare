/*******************************************************************
 * Script: 23_CrearSP_Veterinario.sql
 * Descripción: Stored Procedures para gestión de Veterinarios
 * Fecha: 2025-01-19
 * Base de Datos: VetCareDB
 *******************************************************************/

USE VetCareDB;
GO

PRINT 'Creando Stored Procedures de Veterinario...'
PRINT ''

-- =====================================================
-- STORED PROCEDURES: VETERINARIO
-- =====================================================

-- SP: Veterinario_Insert
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Veterinario_Insert')
    DROP PROCEDURE Veterinario_Insert;
GO

CREATE PROCEDURE Veterinario_Insert
    @IdVeterinario UNIQUEIDENTIFIER,
    @Nombre NVARCHAR(150),
    @Matricula NVARCHAR(50) = NULL,
    @Telefono NVARCHAR(20) = NULL,
    @Email NVARCHAR(100) = NULL,
    @Observaciones NVARCHAR(500) = NULL,
    @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Veterinario (IdVeterinario, Nombre, Matricula, Telefono, Email, Observaciones, Activo)
    VALUES (@IdVeterinario, @Nombre, @Matricula, @Telefono, @Email, @Observaciones, @Activo);

    SELECT * FROM Veterinario WHERE IdVeterinario = @IdVeterinario;
END
GO

PRINT '✓ SP Veterinario_Insert creado'
GO

-- SP: Veterinario_Update
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Veterinario_Update')
    DROP PROCEDURE Veterinario_Update;
GO

CREATE PROCEDURE Veterinario_Update
    @IdVeterinario UNIQUEIDENTIFIER,
    @Nombre NVARCHAR(150),
    @Matricula NVARCHAR(50) = NULL,
    @Telefono NVARCHAR(20) = NULL,
    @Email NVARCHAR(100) = NULL,
    @Observaciones NVARCHAR(500) = NULL,
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Veterinario
    SET Nombre = @Nombre,
        Matricula = @Matricula,
        Telefono = @Telefono,
        Email = @Email,
        Observaciones = @Observaciones,
        Activo = @Activo
    WHERE IdVeterinario = @IdVeterinario;

    SELECT * FROM Veterinario WHERE IdVeterinario = @IdVeterinario;
END
GO

PRINT '✓ SP Veterinario_Update creado'
GO

-- SP: Veterinario_Delete
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Veterinario_Delete')
    DROP PROCEDURE Veterinario_Delete;
GO

CREATE PROCEDURE Veterinario_Delete
    @IdVeterinario UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Soft delete: marcar como inactivo en lugar de eliminar
    UPDATE Veterinario
    SET Activo = 0
    WHERE IdVeterinario = @IdVeterinario;
END
GO

PRINT '✓ SP Veterinario_Delete creado'
GO

-- SP: Veterinario_SelectOne
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Veterinario_SelectOne')
    DROP PROCEDURE Veterinario_SelectOne;
GO

CREATE PROCEDURE Veterinario_SelectOne
    @IdVeterinario UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM Veterinario WHERE IdVeterinario = @IdVeterinario;
END
GO

PRINT '✓ SP Veterinario_SelectOne creado'
GO

-- SP: Veterinario_SelectAll
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Veterinario_SelectAll')
    DROP PROCEDURE Veterinario_SelectAll;
GO

CREATE PROCEDURE Veterinario_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM Veterinario ORDER BY Nombre;
END
GO

PRINT '✓ SP Veterinario_SelectAll creado'
GO

-- SP: Veterinario_SelectActivos
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Veterinario_SelectActivos')
    DROP PROCEDURE Veterinario_SelectActivos;
GO

CREATE PROCEDURE Veterinario_SelectActivos
AS
BEGIN
    SET NOCOUNT ON;

    -- Selecciona solo veterinarios activos para combos/listas
    SELECT * FROM Veterinario
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

PRINT '✓ SP Veterinario_SelectActivos creado'
GO

-- SP: Veterinario_SincronizarNombre
-- Actualiza solo el campo Nombre desde SecurityVet
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Veterinario_SincronizarNombre')
    DROP PROCEDURE Veterinario_SincronizarNombre;
GO

CREATE PROCEDURE Veterinario_SincronizarNombre
    @IdVeterinario UNIQUEIDENTIFIER,
    @NombreActualizado NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Veterinario
    SET Nombre = @NombreActualizado
    WHERE IdVeterinario = @IdVeterinario;

    SELECT * FROM Veterinario WHERE IdVeterinario = @IdVeterinario;
END
GO

PRINT '✓ SP Veterinario_SincronizarNombre creado'
GO

-- SP: Veterinario_Existe
-- Verifica si existe un veterinario con el ID dado
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Veterinario_Existe')
    DROP PROCEDURE Veterinario_Existe;
GO

CREATE PROCEDURE Veterinario_Existe
    @IdVeterinario UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CASE WHEN EXISTS (SELECT 1 FROM Veterinario WHERE IdVeterinario = @IdVeterinario)
        THEN CAST(1 AS BIT)
        ELSE CAST(0 AS BIT)
    END AS Existe;
END
GO

PRINT '✓ SP Veterinario_Existe creado'
GO

PRINT ''
PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║   STORED PROCEDURES VETERINARIO CREADOS           ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Veterinario: 8 SPs creados exitosamente'
PRINT '  - Insert, Update, Delete (soft), SelectOne, SelectAll'
PRINT '  - SelectActivos, SincronizarNombre, Existe'
PRINT ''
GO
