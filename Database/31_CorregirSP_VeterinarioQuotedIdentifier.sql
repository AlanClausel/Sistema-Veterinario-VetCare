/*******************************************************************
 * Script: 31_CorregirSP_VeterinarioQuotedIdentifier.sql
 * Descripción: Corrige los SP de Veterinario agregando QUOTED_IDENTIFIER
 *              necesario para índices filtrados
 * Base de Datos: VetCareDB
 *******************************************************************/

USE VetCareDB;
GO

PRINT 'Corrigiendo Stored Procedures de Veterinario con QUOTED_IDENTIFIER...'
GO

-- =====================================================
-- SP: Veterinario_Insert
-- =====================================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Veterinario_Insert')
    DROP PROCEDURE Veterinario_Insert;
GO

SET QUOTED_IDENTIFIER ON;
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
    SET QUOTED_IDENTIFIER ON;

    INSERT INTO Veterinario (IdVeterinario, Nombre, Matricula, Telefono, Email, Observaciones, Activo)
    VALUES (@IdVeterinario, @Nombre, @Matricula, @Telefono, @Email, @Observaciones, @Activo);

    SELECT * FROM Veterinario WHERE IdVeterinario = @IdVeterinario;
END
GO

PRINT 'SP Veterinario_Insert corregido';
GO

-- =====================================================
-- SP: Veterinario_Update
-- =====================================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Veterinario_Update')
    DROP PROCEDURE Veterinario_Update;
GO

SET QUOTED_IDENTIFIER ON;
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
    SET QUOTED_IDENTIFIER ON;

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

PRINT 'SP Veterinario_Update corregido';
GO

-- =====================================================
-- SP: Veterinario_SincronizarNombre
-- =====================================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Veterinario_SincronizarNombre')
    DROP PROCEDURE Veterinario_SincronizarNombre;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE Veterinario_SincronizarNombre
    @IdVeterinario UNIQUEIDENTIFIER,
    @NombreActualizado NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;
    SET QUOTED_IDENTIFIER ON;

    UPDATE Veterinario
    SET Nombre = @NombreActualizado
    WHERE IdVeterinario = @IdVeterinario;

    SELECT * FROM Veterinario WHERE IdVeterinario = @IdVeterinario;
END
GO

PRINT 'SP Veterinario_SincronizarNombre corregido';
GO

PRINT 'Stored Procedures de Veterinario corregidos exitosamente';
GO
