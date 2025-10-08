USE SeguridadBiblioteca;
GO

-- Corregir el stored procedure Familia_Patente_Select para que devuelva ambas columnas
DROP PROCEDURE IF EXISTS Familia_Patente_Select;
GO

CREATE PROCEDURE Familia_Patente_Select
    @IdFamilia UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Devolver AMBAS columnas: idFamilia e idPatente
    -- El código espera values[1] para obtener el idPatente
    SELECT idFamilia, idPatente
    FROM FamiliaPatente
    WHERE idFamilia = @IdFamilia;
END
GO

PRINT 'Stored procedure Familia_Patente_Select corregido exitosamente'
