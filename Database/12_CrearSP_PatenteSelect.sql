USE SeguridadBiblioteca;
GO

-- Crear el stored procedure Patente_Select que faltaba
CREATE PROCEDURE Patente_Select
    @IdPatente UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdPatente, FormName, MenuItemName, Orden, Descripcion
    FROM Patente
    WHERE IdPatente = @IdPatente;
END
GO

-- Verificar que también exista Patente_SelectAll
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Patente_SelectAll]') AND type in (N'P'))
BEGIN
    EXEC('
    CREATE PROCEDURE Patente_SelectAll
    AS
    BEGIN
        SET NOCOUNT ON;

        SELECT IdPatente, FormName, MenuItemName, Orden, Descripcion
        FROM Patente
        ORDER BY Orden, MenuItemName;
    END
    ')
    PRINT 'Stored procedure Patente_SelectAll creado'
END
ELSE
BEGIN
    PRINT 'Stored procedure Patente_SelectAll ya existe'
END
GO

PRINT 'Stored procedures para Patente creados exitosamente'
