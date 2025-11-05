/*******************************************************************
 * Script: 18_CrearSP_Cita.sql
 * Descripción: Stored Procedures para la gestión de Citas
 * Fecha: 2025-01-11
 * Base de Datos: VetCareDB
 *******************************************************************/

USE VetCareDB;
GO

/*******************************************************************
 * SP: SP_Cita_Insert
 * Descripción: Inserta una nueva cita
 *******************************************************************/
IF OBJECT_ID('SP_Cita_Insert', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_Insert;
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

    -- Generar nuevo GUID
    SET @IdCita = NEWID();

    INSERT INTO Cita (IdCita, IdMascota, FechaCita, TipoConsulta, IdVeterinario, Veterinario, Estado, Observaciones, FechaRegistro, Activo)
    VALUES (@IdCita, @IdMascota, @FechaCita, @TipoConsulta, @IdVeterinario, @Veterinario, @Estado, @Observaciones, GETDATE(), 1);

    SELECT @IdCita AS IdCita;
END;
GO

/*******************************************************************
 * SP: SP_Cita_Update
 * Descripción: Actualiza una cita existente
 *******************************************************************/
IF OBJECT_ID('SP_Cita_Update', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_Update;
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

/*******************************************************************
 * SP: SP_Cita_UpdateEstado
 * Descripción: Actualiza solo el estado de una cita
 *******************************************************************/
IF OBJECT_ID('SP_Cita_UpdateEstado', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_UpdateEstado;
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

/*******************************************************************
 * SP: SP_Cita_Delete
 * Descripción: Elimina lógicamente una cita (marca como inactiva)
 *******************************************************************/
IF OBJECT_ID('SP_Cita_Delete', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_Delete;
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

/*******************************************************************
 * SP: SP_Cita_SelectOne
 * Descripción: Obtiene una cita por su ID con información del cliente
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectOne', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectOne;
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

/*******************************************************************
 * SP: SP_Cita_SelectAll
 * Descripción: Obtiene todas las citas activas con información del cliente
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectAll', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectAll;
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

/*******************************************************************
 * SP: SP_Cita_SelectByMascota
 * Descripción: Obtiene todas las citas de una mascota específica
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectByMascota', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectByMascota;
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

/*******************************************************************
 * SP: SP_Cita_SelectByCliente
 * Descripción: Obtiene todas las citas de un cliente (todas sus mascotas)
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectByCliente', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectByCliente;
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

/*******************************************************************
 * SP: SP_Cita_SelectByFecha
 * Descripción: Obtiene todas las citas de una fecha específica
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectByFecha', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectByFecha;
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

/*******************************************************************
 * SP: SP_Cita_SelectByVeterinario
 * Descripción: Obtiene todas las citas de un veterinario específico
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectByVeterinario', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectByVeterinario;
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

/*******************************************************************
 * SP: SP_Cita_SelectByEstado
 * Descripción: Obtiene todas las citas por estado
 *******************************************************************/
IF OBJECT_ID('SP_Cita_SelectByEstado', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectByEstado;
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

/*******************************************************************
 * SP: SP_Cita_Search
 * Descripción: Búsqueda combinada (fecha, veterinario, estado)
 *******************************************************************/
IF OBJECT_ID('SP_Cita_Search', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_Search;
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

PRINT 'Stored Procedures de Cita creados exitosamente';
GO
