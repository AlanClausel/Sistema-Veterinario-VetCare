-- =============================================
-- Script: Crear Stored Procedures faltantes
-- Descripción: SPs para FamiliaFamilia y FamiliaPatente
-- =============================================

USE SeguridadBiblioteca;
GO

PRINT 'Creando Stored Procedures...';
GO

-- =============================================
-- SP: Familia_SelectAll
-- Descripción: Retorna todas las familias
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Familia_SelectAll')
    DROP PROCEDURE Familia_SelectAll;
GO

CREATE PROCEDURE Familia_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdFamilia, Nombre, Descripcion
    FROM Familia
    ORDER BY Nombre;
END
GO

PRINT '  ✓ Familia_SelectAll creado';
GO

-- =============================================
-- SP: Familia_Select
-- Descripción: Retorna una familia por ID
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Familia_Select')
    DROP PROCEDURE Familia_Select;
GO

CREATE PROCEDURE Familia_Select
    @IdFamilia UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdFamilia, Nombre, Descripcion
    FROM Familia
    WHERE IdFamilia = @IdFamilia;
END
GO

PRINT '  ✓ Familia_Select creado';
GO

-- =============================================
-- SP: Familia_Familia_Insert
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Familia_Familia_Insert')
    DROP PROCEDURE Familia_Familia_Insert;
GO

CREATE PROCEDURE Familia_Familia_Insert
    @IdFamiliaPadre UNIQUEIDENTIFIER,
    @IdFamiliaHija UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO FamiliaFamilia (IdFamiliaPadre, IdFamiliaHijo)
    VALUES (@IdFamiliaPadre, @IdFamiliaHija);
END
GO

PRINT '  ✓ Familia_Familia_Insert creado';
GO

-- =============================================
-- SP: Familia_Familia_DeleteParticular
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Familia_Familia_DeleteParticular')
    DROP PROCEDURE Familia_Familia_DeleteParticular;
GO

CREATE PROCEDURE Familia_Familia_DeleteParticular
    @IdFamiliaPadre UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM FamiliaFamilia
    WHERE IdFamiliaPadre = @IdFamiliaPadre;
END
GO

PRINT '  ✓ Familia_Familia_DeleteParticular creado';
GO

-- =============================================
-- SP: Familia_Familia_SelectParticular
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Familia_Familia_SelectParticular')
    DROP PROCEDURE Familia_Familia_SelectParticular;
GO

CREATE PROCEDURE Familia_Familia_SelectParticular
    @IdFamiliaPadre UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdFamiliaHijo
    FROM FamiliaFamilia
    WHERE IdFamiliaPadre = @IdFamiliaPadre;
END
GO

PRINT '  ✓ Familia_Familia_SelectParticular creado';
GO

-- =============================================
-- SP: Familia_Patente_Insert
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Familia_Patente_Insert')
    DROP PROCEDURE Familia_Patente_Insert;
GO

CREATE PROCEDURE Familia_Patente_Insert
    @IdFamilia UNIQUEIDENTIFIER,
    @IdPatente UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO FamiliaPatente (idFamilia, idPatente)
    VALUES (@IdFamilia, @IdPatente);
END
GO

PRINT '  ✓ Familia_Patente_Insert creado';
GO

-- =============================================
-- SP: Familia_Patente_Delete
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Familia_Patente_Delete')
    DROP PROCEDURE Familia_Patente_Delete;
GO

CREATE PROCEDURE Familia_Patente_Delete
    @IdFamilia UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM FamiliaPatente
    WHERE idFamilia = @IdFamilia;
END
GO

PRINT '  ✓ Familia_Patente_Delete creado';
GO

-- =============================================
-- SP: Familia_Patente_Select
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Familia_Patente_Select')
    DROP PROCEDURE Familia_Patente_Select;
GO

CREATE PROCEDURE Familia_Patente_Select
    @IdFamilia UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT idPatente
    FROM FamiliaPatente
    WHERE idFamilia = @IdFamilia;
END
GO

PRINT '  ✓ Familia_Patente_Select creado';
GO

PRINT '';
PRINT 'Todos los Stored Procedures han sido creados exitosamente.';
GO
