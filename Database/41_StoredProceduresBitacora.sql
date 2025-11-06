USE SecurityVet;
GO

-- =============================================
-- SP: Bitacora_Insert
-- Descripción: Inserta un nuevo registro en la bitácora
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Bitacora_Insert')
    DROP PROCEDURE Bitacora_Insert;
GO

CREATE PROCEDURE Bitacora_Insert
    @IdUsuario UNIQUEIDENTIFIER = NULL,
    @NombreUsuario VARCHAR(50) = NULL,
    @Modulo VARCHAR(50),
    @Accion VARCHAR(50),
    @Descripcion VARCHAR(500),
    @Tabla VARCHAR(50) = NULL,
    @IdRegistro VARCHAR(100) = NULL,
    @Criticidad VARCHAR(20),
    @IP VARCHAR(45) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NuevoId UNIQUEIDENTIFIER = NEWID();

    INSERT INTO Bitacora (
        IdBitacora,
        IdUsuario,
        NombreUsuario,
        FechaHora,
        Modulo,
        Accion,
        Descripcion,
        Tabla,
        IdRegistro,
        Criticidad,
        IP
    )
    VALUES (
        @NuevoId,
        @IdUsuario,
        @NombreUsuario,
        GETDATE(),
        @Modulo,
        @Accion,
        @Descripcion,
        @Tabla,
        @IdRegistro,
        @Criticidad,
        @IP
    );

    SELECT @NuevoId AS IdBitacora;
END
GO

-- =============================================
-- SP: Bitacora_SelectAll
-- Descripción: Obtiene todos los registros de bitácora ordenados por fecha descendente
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Bitacora_SelectAll')
    DROP PROCEDURE Bitacora_SelectAll;
GO

CREATE PROCEDURE Bitacora_SelectAll
    @Top INT = 1000 -- Límite por defecto para evitar cargar demasiados registros
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@Top)
        IdBitacora,
        IdUsuario,
        NombreUsuario,
        FechaHora,
        Modulo,
        Accion,
        Descripcion,
        Tabla,
        IdRegistro,
        Criticidad,
        IP
    FROM Bitacora
    ORDER BY FechaHora DESC;
END
GO

-- =============================================
-- SP: Bitacora_SelectByFiltros
-- Descripción: Busca registros de bitácora aplicando filtros opcionales
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Bitacora_SelectByFiltros')
    DROP PROCEDURE Bitacora_SelectByFiltros;
GO

CREATE PROCEDURE Bitacora_SelectByFiltros
    @FechaDesde DATETIME = NULL,
    @FechaHasta DATETIME = NULL,
    @IdUsuario UNIQUEIDENTIFIER = NULL,
    @Modulo VARCHAR(50) = NULL,
    @Accion VARCHAR(50) = NULL,
    @Criticidad VARCHAR(20) = NULL,
    @Top INT = 1000
AS
BEGIN
    SET NOCOUNT ON;

    -- Si FechaHasta se especifica, incluir todo el día
    IF @FechaHasta IS NOT NULL
        SET @FechaHasta = DATEADD(DAY, 1, @FechaHasta);

    SELECT TOP (@Top)
        IdBitacora,
        IdUsuario,
        NombreUsuario,
        FechaHora,
        Modulo,
        Accion,
        Descripcion,
        Tabla,
        IdRegistro,
        Criticidad,
        IP
    FROM Bitacora
    WHERE
        (@FechaDesde IS NULL OR FechaHora >= @FechaDesde)
        AND (@FechaHasta IS NULL OR FechaHora < @FechaHasta)
        AND (@IdUsuario IS NULL OR IdUsuario = @IdUsuario)
        AND (@Modulo IS NULL OR Modulo = @Modulo)
        AND (@Accion IS NULL OR Accion = @Accion)
        AND (@Criticidad IS NULL OR Criticidad = @Criticidad)
    ORDER BY FechaHora DESC;
END
GO

-- =============================================
-- SP: Bitacora_SelectByUsuario
-- Descripción: Obtiene registros de bitácora de un usuario específico
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Bitacora_SelectByUsuario')
    DROP PROCEDURE Bitacora_SelectByUsuario;
GO

CREATE PROCEDURE Bitacora_SelectByUsuario
    @IdUsuario UNIQUEIDENTIFIER,
    @Top INT = 100
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@Top)
        IdBitacora,
        IdUsuario,
        NombreUsuario,
        FechaHora,
        Modulo,
        Accion,
        Descripcion,
        Tabla,
        IdRegistro,
        Criticidad,
        IP
    FROM Bitacora
    WHERE IdUsuario = @IdUsuario
    ORDER BY FechaHora DESC;
END
GO

-- =============================================
-- SP: Bitacora_SelectByRangoFechas
-- Descripción: Obtiene registros de bitácora en un rango de fechas
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Bitacora_SelectByRangoFechas')
    DROP PROCEDURE Bitacora_SelectByRangoFechas;
GO

CREATE PROCEDURE Bitacora_SelectByRangoFechas
    @FechaDesde DATETIME,
    @FechaHasta DATETIME,
    @Top INT = 1000
AS
BEGIN
    SET NOCOUNT ON;

    -- Incluir todo el día de FechaHasta
    SET @FechaHasta = DATEADD(DAY, 1, @FechaHasta);

    SELECT TOP (@Top)
        IdBitacora,
        IdUsuario,
        NombreUsuario,
        FechaHora,
        Modulo,
        Accion,
        Descripcion,
        Tabla,
        IdRegistro,
        Criticidad,
        IP
    FROM Bitacora
    WHERE FechaHora >= @FechaDesde AND FechaHora < @FechaHasta
    ORDER BY FechaHora DESC;
END
GO

-- =============================================
-- SP: Bitacora_DeleteOlderThan
-- Descripción: Elimina registros de bitácora anteriores a una fecha (para limpieza)
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Bitacora_DeleteOlderThan')
    DROP PROCEDURE Bitacora_DeleteOlderThan;
GO

CREATE PROCEDURE Bitacora_DeleteOlderThan
    @FechaLimite DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Bitacora
    WHERE FechaHora < @FechaLimite;

    SELECT @@ROWCOUNT AS RegistrosEliminados;
END
GO

-- =============================================
-- SP: Bitacora_GetEstadisticas
-- Descripción: Obtiene estadísticas de la bitácora
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Bitacora_GetEstadisticas')
    DROP PROCEDURE Bitacora_GetEstadisticas;
GO

CREATE PROCEDURE Bitacora_GetEstadisticas
    @FechaDesde DATETIME = NULL,
    @FechaHasta DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Si no se especifican fechas, usar últimos 30 días
    IF @FechaDesde IS NULL
        SET @FechaDesde = DATEADD(DAY, -30, GETDATE());

    IF @FechaHasta IS NULL
        SET @FechaHasta = GETDATE();
    ELSE
        SET @FechaHasta = DATEADD(DAY, 1, @FechaHasta);

    -- Estadísticas por módulo
    SELECT
        Modulo,
        COUNT(*) AS CantidadEventos,
        SUM(CASE WHEN Criticidad = 'Critico' THEN 1 ELSE 0 END) AS EventosCriticos,
        SUM(CASE WHEN Criticidad = 'Error' THEN 1 ELSE 0 END) AS EventosError,
        SUM(CASE WHEN Criticidad = 'Advertencia' THEN 1 ELSE 0 END) AS EventosAdvertencia,
        SUM(CASE WHEN Criticidad = 'Info' THEN 1 ELSE 0 END) AS EventosInfo
    FROM Bitacora
    WHERE FechaHora >= @FechaDesde AND FechaHora < @FechaHasta
    GROUP BY Modulo
    ORDER BY CantidadEventos DESC;
END
GO

PRINT 'Stored Procedures de Bitacora creados exitosamente';
