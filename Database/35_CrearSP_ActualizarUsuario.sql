-- =============================================
-- Script: 35_CrearSP_ActualizarUsuario.sql
-- Descripción: Stored Procedures para actualizar contraseña e idioma del usuario
--              Recalcula DVH en ambos casos
-- Base de datos: SecurityVet
-- =============================================

USE SecurityVet;
GO

PRINT '════════════════════════════════════════════════════'
PRINT 'Creando SPs para actualización de usuario...'
PRINT '════════════════════════════════════════════════════'

-- =============================================
-- SP: SP_Usuario_ActualizarPassword
-- Descripción: Cambia la contraseña del usuario y recalcula DVH
-- =============================================
IF OBJECT_ID('SP_Usuario_ActualizarPassword', 'P') IS NOT NULL
    DROP PROCEDURE SP_Usuario_ActualizarPassword;
GO

CREATE PROCEDURE SP_Usuario_ActualizarPassword
    @IdUsuario UNIQUEIDENTIFIER,
    @NuevaClaveHasheada NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar que el usuario existe
    IF NOT EXISTS (SELECT 1 FROM Usuario WHERE IdUsuario = @IdUsuario)
    BEGIN
        RAISERROR('El usuario no existe', 16, 1);
        RETURN;
    END

    -- Obtener datos del usuario
    DECLARE @Nombre NVARCHAR(100);
    DECLARE @Activo BIT;

    SELECT @Nombre = Nombre, @Activo = Activo
    FROM Usuario
    WHERE IdUsuario = @IdUsuario;

    -- Calcular nuevo DVH = SHA256(IdUsuario|Nombre|Clave|Activo)
    DECLARE @DatosParaDVH NVARCHAR(MAX);
    DECLARE @NuevoDVH NVARCHAR(64);

    SET @DatosParaDVH = UPPER(CAST(@IdUsuario AS NVARCHAR(36))) + N'|' + @Nombre + N'|' + @NuevaClaveHasheada + N'|' + CAST(@Activo AS NVARCHAR(1));
    SET @NuevoDVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', @DatosParaDVH), 2);

    -- Actualizar contraseña y DVH
    UPDATE Usuario
    SET Clave = @NuevaClaveHasheada,
        DVH = @NuevoDVH
    WHERE IdUsuario = @IdUsuario;

    RETURN @@ROWCOUNT;
END;
GO

PRINT '✓ SP_Usuario_ActualizarPassword creado';

-- =============================================
-- SP: SP_Usuario_ActualizarIdioma
-- Descripción: Cambia el idioma preferido del usuario y recalcula DVH
-- =============================================
IF OBJECT_ID('SP_Usuario_ActualizarIdioma', 'P') IS NOT NULL
    DROP PROCEDURE SP_Usuario_ActualizarIdioma;
GO

CREATE PROCEDURE SP_Usuario_ActualizarIdioma
    @IdUsuario UNIQUEIDENTIFIER,
    @NuevoIdioma NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar que el usuario existe
    IF NOT EXISTS (SELECT 1 FROM Usuario WHERE IdUsuario = @IdUsuario)
    BEGIN
        RAISERROR('El usuario no existe', 16, 1);
        RETURN;
    END

    -- Obtener datos del usuario
    DECLARE @Nombre NVARCHAR(100);
    DECLARE @Clave NVARCHAR(256);
    DECLARE @Activo BIT;

    SELECT @Nombre = Nombre, @Clave = Clave, @Activo = Activo
    FROM Usuario
    WHERE IdUsuario = @IdUsuario;

    -- Calcular nuevo DVH = SHA256(IdUsuario|Nombre|Clave|Activo)
    DECLARE @DatosParaDVH NVARCHAR(MAX);
    DECLARE @NuevoDVH NVARCHAR(64);

    SET @DatosParaDVH = UPPER(CAST(@IdUsuario AS NVARCHAR(36))) + N'|' + @Nombre + N'|' + @Clave + N'|' + CAST(@Activo AS NVARCHAR(1));
    SET @NuevoDVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', @DatosParaDVH), 2);

    -- Actualizar idioma y DVH
    UPDATE Usuario
    SET IdiomaPreferido = @NuevoIdioma,
        DVH = @NuevoDVH
    WHERE IdUsuario = @IdUsuario;

    RETURN @@ROWCOUNT;
END;
GO

PRINT '✓ SP_Usuario_ActualizarIdioma creado';
PRINT ''
PRINT '════════════════════════════════════════════════════'
PRINT 'SPs de actualización de usuario creados exitosamente'
PRINT '════════════════════════════════════════════════════'
PRINT ''

GO
