/*******************************************************************
 * Script: 32_CorregirSP_CitaQuotedIdentifier.sql
 * Descripción: Corrige stored procedures de Cita agregando SET QUOTED_IDENTIFIER ON
 * Fecha: 2025-10-31
 * Base de Datos: VetCareDB
 *
 * PROBLEMA DETECTADO:
 * Los SPs fueron creados sin SET QUOTED_IDENTIFIER ON
 * La tabla Cita tiene FKs que requieren QUOTED_IDENTIFIER ON
 * ERROR: "INSERT failed because the following SET options have incorrect settings: 'QUOTED_IDENTIFIER'"
 *
 * SOLUCIÓN:
 * Recrear todos los SPs de Cita con SET QUOTED_IDENTIFIER ON
 *******************************************************************/

USE VetCareDB;
GO

PRINT '========================================';
PRINT 'CORRIGIENDO STORED PROCEDURES DE CITA';
PRINT '========================================';
PRINT '';

/*******************************************************************
 * SP: SP_Cita_Insert
 *******************************************************************/
IF OBJECT_ID('SP_Cita_Insert', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_Insert;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE SP_Cita_Insert
    @IdCita UNIQUEIDENTIFIER OUTPUT,
    @IdMascota UNIQUEIDENTIFIER,
    @FechaCita DATETIME,
    @TipoConsulta NVARCHAR(100),
    @IdVeterinario UNIQUEIDENTIFIER = NULL,
    @Veterinario NVARCHAR(150) = NULL,
    @Estado NVARCHAR(20),
    @Observaciones NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdCita = NEWID();

    INSERT INTO Cita (IdCita, IdMascota, FechaCita, TipoConsulta, IdVeterinario, Veterinario, Estado, Observaciones, FechaRegistro, Activo)
    VALUES (@IdCita, @IdMascota, @FechaCita, @TipoConsulta, @IdVeterinario, @Veterinario, @Estado, @Observaciones, GETDATE(), 1);

    SELECT @IdCita AS IdCita;
END;
GO

PRINT 'SP_Cita_Insert corregido';
GO

/*******************************************************************
 * SP: SP_Cita_Update
 *******************************************************************/
IF OBJECT_ID('SP_Cita_Update', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_Update;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE SP_Cita_Update
    @IdCita UNIQUEIDENTIFIER,
    @IdMascota UNIQUEIDENTIFIER,
    @FechaCita DATETIME,
    @TipoConsulta NVARCHAR(100),
    @IdVeterinario UNIQUEIDENTIFIER = NULL,
    @Veterinario NVARCHAR(150) = NULL,
    @Estado NVARCHAR(20),
    @Observaciones NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Cita
    SET IdMascota = @IdMascota,
        FechaCita = @FechaCita,
        TipoConsulta = @TipoConsulta,
        IdVeterinario = @IdVeterinario,
        Veterinario = @Veterinario,
        Estado = @Estado,
        Observaciones = @Observaciones
    WHERE IdCita = @IdCita AND Activo = 1;

    RETURN @@ROWCOUNT;
END;
GO

PRINT 'SP_Cita_Update corregido';
GO

/*******************************************************************
 * SP: SP_Cita_UpdateEstado
 *******************************************************************/
IF OBJECT_ID('SP_Cita_UpdateEstado', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_UpdateEstado;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE SP_Cita_UpdateEstado
    @IdCita UNIQUEIDENTIFIER,
    @Estado NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Cita
    SET Estado = @Estado
    WHERE IdCita = @IdCita AND Activo = 1;

    RETURN @@ROWCOUNT;
END;
GO

PRINT 'SP_Cita_UpdateEstado corregido';
GO

/*******************************************************************
 * SP: SP_Cita_Delete
 *******************************************************************/
IF OBJECT_ID('SP_Cita_Delete', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_Delete;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE SP_Cita_Delete
    @IdCita UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Cita
    SET Activo = 0
    WHERE IdCita = @IdCita;

    RETURN @@ROWCOUNT;
END;
GO

PRINT 'SP_Cita_Delete corregido';
GO

/*******************************************************************
 * SP: SP_Cita_SelectOne
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectOne', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectOne;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE SP_Cita_SelectOne
    @IdCita UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.IdCita,
        c.IdMascota,
        c.FechaCita,
        c.TipoConsulta,
        c.IdVeterinario,
        c.Veterinario,
        c.Estado,
        c.Observaciones,
        c.FechaRegistro,
        c.Activo,
        m.Nombre AS MascotaNombre,
        m.Especie,
        m.Raza,
        cl.IdCliente,
        cl.Nombre AS ClienteNombre,
        cl.Apellido AS ClienteApellido,
        cl.DNI AS ClienteDNI,
        cl.Telefono AS ClienteTelefono
    FROM Cita c
    INNER JOIN Mascota m ON c.IdMascota = m.IdMascota
    INNER JOIN Cliente cl ON m.IdCliente = cl.IdCliente
    WHERE c.IdCita = @IdCita;
END;
GO

PRINT 'SP_Cita_SelectOne corregido';
GO

/*******************************************************************
 * SP: SP_Cita_SelectAll
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectAll', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectAll;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE SP_Cita_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.IdCita,
        c.IdMascota,
        c.FechaCita,
        c.TipoConsulta,
        c.IdVeterinario,
        c.Veterinario,
        c.Estado,
        c.Observaciones,
        c.FechaRegistro,
        c.Activo,
        m.Nombre AS MascotaNombre,
        m.Especie,
        m.Raza,
        cl.IdCliente,
        cl.Nombre AS ClienteNombre,
        cl.Apellido AS ClienteApellido,
        cl.DNI AS ClienteDNI,
        cl.Telefono AS ClienteTelefono
    FROM Cita c
    INNER JOIN Mascota m ON c.IdMascota = m.IdMascota
    INNER JOIN Cliente cl ON m.IdCliente = cl.IdCliente
    WHERE c.Activo = 1
    ORDER BY c.FechaCita DESC;
END;
GO

PRINT 'SP_Cita_SelectAll corregido';
GO

/*******************************************************************
 * SP: SP_Cita_SelectByMascota
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectByMascota', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectByMascota;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE SP_Cita_SelectByMascota
    @IdMascota UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.IdCita,
        c.IdMascota,
        c.FechaCita,
        c.TipoConsulta,
        c.IdVeterinario,
        c.Veterinario,
        c.Estado,
        c.Observaciones,
        c.FechaRegistro,
        c.Activo,
        m.Nombre AS MascotaNombre,
        m.Especie,
        m.Raza,
        cl.IdCliente,
        cl.Nombre AS ClienteNombre,
        cl.Apellido AS ClienteApellido,
        cl.DNI AS ClienteDNI,
        cl.Telefono AS ClienteTelefono
    FROM Cita c
    INNER JOIN Mascota m ON c.IdMascota = m.IdMascota
    INNER JOIN Cliente cl ON m.IdCliente = cl.IdCliente
    WHERE c.IdMascota = @IdMascota AND c.Activo = 1
    ORDER BY c.FechaCita DESC;
END;
GO

PRINT 'SP_Cita_SelectByMascota corregido';
GO

/*******************************************************************
 * SP: SP_Cita_SelectByCliente
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectByCliente', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectByCliente;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE SP_Cita_SelectByCliente
    @IdCliente UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.IdCita,
        c.IdMascota,
        c.FechaCita,
        c.TipoConsulta,
        c.IdVeterinario,
        c.Veterinario,
        c.Estado,
        c.Observaciones,
        c.FechaRegistro,
        c.Activo,
        m.Nombre AS MascotaNombre,
        m.Especie,
        m.Raza,
        cl.IdCliente,
        cl.Nombre AS ClienteNombre,
        cl.Apellido AS ClienteApellido,
        cl.DNI AS ClienteDNI,
        cl.Telefono AS ClienteTelefono
    FROM Cita c
    INNER JOIN Mascota m ON c.IdMascota = m.IdMascota
    INNER JOIN Cliente cl ON m.IdCliente = cl.IdCliente
    WHERE cl.IdCliente = @IdCliente AND c.Activo = 1
    ORDER BY c.FechaCita DESC;
END;
GO

PRINT 'SP_Cita_SelectByCliente corregido';
GO

/*******************************************************************
 * SP: SP_Cita_SelectByFecha
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectByFecha', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectByFecha;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE SP_Cita_SelectByFecha
    @Fecha DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.IdCita,
        c.IdMascota,
        c.FechaCita,
        c.TipoConsulta,
        c.IdVeterinario,
        c.Veterinario,
        c.Estado,
        c.Observaciones,
        c.FechaRegistro,
        c.Activo,
        m.Nombre AS MascotaNombre,
        m.Especie,
        m.Raza,
        cl.IdCliente,
        cl.Nombre AS ClienteNombre,
        cl.Apellido AS ClienteApellido,
        cl.DNI AS ClienteDNI,
        cl.Telefono AS ClienteTelefono
    FROM Cita c
    INNER JOIN Mascota m ON c.IdMascota = m.IdMascota
    INNER JOIN Cliente cl ON m.IdCliente = cl.IdCliente
    WHERE CAST(c.FechaCita AS DATE) = @Fecha AND c.Activo = 1
    ORDER BY c.FechaCita;
END;
GO

PRINT 'SP_Cita_SelectByFecha corregido';
GO

/*******************************************************************
 * SP: SP_Cita_SelectByVeterinario
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectByVeterinario', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectByVeterinario;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE SP_Cita_SelectByVeterinario
    @Veterinario NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.IdCita,
        c.IdMascota,
        c.FechaCita,
        c.TipoConsulta,
        c.IdVeterinario,
        c.Veterinario,
        c.Estado,
        c.Observaciones,
        c.FechaRegistro,
        c.Activo,
        m.Nombre AS MascotaNombre,
        m.Especie,
        m.Raza,
        cl.IdCliente,
        cl.Nombre AS ClienteNombre,
        cl.Apellido AS ClienteApellido,
        cl.DNI AS ClienteDNI,
        cl.Telefono AS ClienteTelefono
    FROM Cita c
    INNER JOIN Mascota m ON c.IdMascota = m.IdMascota
    INNER JOIN Cliente cl ON m.IdCliente = cl.IdCliente
    WHERE c.Veterinario = @Veterinario AND c.Activo = 1
    ORDER BY c.FechaCita DESC;
END;
GO

PRINT 'SP_Cita_SelectByVeterinario corregido';
GO

/*******************************************************************
 * SP: SP_Cita_SelectByEstado
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectByEstado', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectByEstado;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE SP_Cita_SelectByEstado
    @Estado NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.IdCita,
        c.IdMascota,
        c.FechaCita,
        c.TipoConsulta,
        c.IdVeterinario,
        c.Veterinario,
        c.Estado,
        c.Observaciones,
        c.FechaRegistro,
        c.Activo,
        m.Nombre AS MascotaNombre,
        m.Especie,
        m.Raza,
        cl.IdCliente,
        cl.Nombre AS ClienteNombre,
        cl.Apellido AS ClienteApellido,
        cl.DNI AS ClienteDNI,
        cl.Telefono AS ClienteTelefono
    FROM Cita c
    INNER JOIN Mascota m ON c.IdMascota = m.IdMascota
    INNER JOIN Cliente cl ON m.IdCliente = cl.IdCliente
    WHERE c.Estado = @Estado AND c.Activo = 1
    ORDER BY c.FechaCita;
END;
GO

PRINT 'SP_Cita_SelectByEstado corregido';
GO

/*******************************************************************
 * SP: SP_Cita_Search
 *******************************************************************/
IF OBJECT_ID('SP_Cita_Search', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_Search;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE SP_Cita_Search
    @Fecha DATE = NULL,
    @Veterinario NVARCHAR(150) = NULL,
    @Estado NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.IdCita,
        c.IdMascota,
        c.FechaCita,
        c.TipoConsulta,
        c.IdVeterinario,
        c.Veterinario,
        c.Estado,
        c.Observaciones,
        c.FechaRegistro,
        c.Activo,
        m.Nombre AS MascotaNombre,
        m.Especie,
        m.Raza,
        cl.IdCliente,
        cl.Nombre AS ClienteNombre,
        cl.Apellido AS ClienteApellido,
        cl.DNI AS ClienteDNI,
        cl.Telefono AS ClienteTelefono
    FROM Cita c
    INNER JOIN Mascota m ON c.IdMascota = m.IdMascota
    INNER JOIN Cliente cl ON m.IdCliente = cl.IdCliente
    WHERE c.Activo = 1
        AND (@Fecha IS NULL OR CAST(c.FechaCita AS DATE) = @Fecha)
        AND (@Veterinario IS NULL OR c.Veterinario = @Veterinario)
        AND (@Estado IS NULL OR c.Estado = @Estado)
    ORDER BY c.FechaCita;
END;
GO

PRINT 'SP_Cita_Search corregido';
GO

PRINT '';
PRINT '========================================';
PRINT 'CORRECCIÓN COMPLETADA';
PRINT '========================================';
PRINT '';
PRINT 'Todos los stored procedures de Cita han sido corregidos con SET QUOTED_IDENTIFIER ON';
PRINT '';
GO
