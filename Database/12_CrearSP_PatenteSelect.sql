USE SecurityVet;
GO

-- Crear el stored procedure Patente_Select que faltaba
-- IMPORTANTE: El orden de columnas debe coincidir con PatenteAdapter.cs
-- Adapter espera: values[0]=IdPatente, values[1]=FormName, values[2]=MenuItemName, values[3]=Orden
CREATE PROCEDURE Patente_Select
    @IdPatente UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdPatente, FormName, MenuItemName, Orden
    FROM Patente
    WHERE IdPatente = @IdPatente;
END
GO

-- Crear o actualizar Patente_SelectAll
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Patente_SelectAll]') AND type in (N'P'))
BEGIN
    DROP PROCEDURE Patente_SelectAll;
END
GO

CREATE PROCEDURE Patente_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    -- IMPORTANTE: FormName ANTES de MenuItemName (orden correcto para el Adapter)
    SELECT IdPatente, FormName, MenuItemName, Orden
    FROM Patente
    ORDER BY CAST(ISNULL(Orden, '999') AS INT);
END
GO

PRINT 'Stored procedures para Patente creados exitosamente en SecurityVet'
