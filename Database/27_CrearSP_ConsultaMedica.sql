-- =============================================
-- Script: 27_CrearSP_ConsultaMedica.sql
-- Descripción: Stored Procedures para gestión de Consultas Médicas
-- =============================================

USE VetCareDB;
GO

-- =============================================
-- SP: ConsultaMedica_Insert
-- Inserta una consulta médica sin medicamentos (se agregan después)
-- =============================================
IF OBJECT_ID('ConsultaMedica_Insert', 'P') IS NOT NULL
    DROP PROCEDURE ConsultaMedica_Insert;
GO

CREATE PROCEDURE ConsultaMedica_Insert
    @IdConsulta UNIQUEIDENTIFIER,
    @IdCita UNIQUEIDENTIFIER,
    @IdVeterinario UNIQUEIDENTIFIER,
    @Sintomas NVARCHAR(1000),
    @Diagnostico NVARCHAR(1000),
    @Tratamiento NVARCHAR(1000),
    @Observaciones NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ConsultaMedica (IdConsulta, IdCita, IdVeterinario, Sintomas, Diagnostico, Tratamiento, Observaciones, FechaConsulta, Activo)
    VALUES (@IdConsulta, @IdCita, @IdVeterinario, @Sintomas, @Diagnostico, @Tratamiento, @Observaciones, GETDATE(), 1);

    -- Retornar el registro insertado
    SELECT * FROM ConsultaMedica WHERE IdConsulta = @IdConsulta;
END
GO

-- =============================================
-- SP: ConsultaMedica_Update
-- =============================================
IF OBJECT_ID('ConsultaMedica_Update', 'P') IS NOT NULL
    DROP PROCEDURE ConsultaMedica_Update;
GO

CREATE PROCEDURE ConsultaMedica_Update
    @IdConsulta UNIQUEIDENTIFIER,
    @Sintomas NVARCHAR(1000),
    @Diagnostico NVARCHAR(1000),
    @Tratamiento NVARCHAR(1000),
    @Observaciones NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ConsultaMedica
    SET Sintomas = @Sintomas,
        Diagnostico = @Diagnostico,
        Tratamiento = @Tratamiento,
        Observaciones = @Observaciones
    WHERE IdConsulta = @IdConsulta;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO

-- =============================================
-- SP: ConsultaMedica_Delete (Soft Delete)
-- =============================================
IF OBJECT_ID('ConsultaMedica_Delete', 'P') IS NOT NULL
    DROP PROCEDURE ConsultaMedica_Delete;
GO

CREATE PROCEDURE ConsultaMedica_Delete
    @IdConsulta UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ConsultaMedica
    SET Activo = 0
    WHERE IdConsulta = @IdConsulta;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO

-- =============================================
-- SP: ConsultaMedica_SelectOne
-- =============================================
IF OBJECT_ID('ConsultaMedica_SelectOne', 'P') IS NOT NULL
    DROP PROCEDURE ConsultaMedica_SelectOne;
GO

CREATE PROCEDURE ConsultaMedica_SelectOne
    @IdConsulta UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM ConsultaMedica
    WHERE IdConsulta = @IdConsulta;
END
GO

-- =============================================
-- SP: ConsultaMedica_SelectByCita
-- Obtiene la consulta médica de una cita específica
-- =============================================
IF OBJECT_ID('ConsultaMedica_SelectByCita', 'P') IS NOT NULL
    DROP PROCEDURE ConsultaMedica_SelectByCita;
GO

CREATE PROCEDURE ConsultaMedica_SelectByCita
    @IdCita UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM ConsultaMedica
    WHERE IdCita = @IdCita
      AND Activo = 1;
END
GO

-- =============================================
-- SP: ConsultaMedica_SelectByVeterinario
-- Obtiene todas las consultas de un veterinario
-- =============================================
IF OBJECT_ID('ConsultaMedica_SelectByVeterinario', 'P') IS NOT NULL
    DROP PROCEDURE ConsultaMedica_SelectByVeterinario;
GO

CREATE PROCEDURE ConsultaMedica_SelectByVeterinario
    @IdVeterinario UNIQUEIDENTIFIER,
    @FechaDesde DATETIME = NULL,
    @FechaHasta DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM ConsultaMedica
    WHERE IdVeterinario = @IdVeterinario
      AND Activo = 1
      AND (@FechaDesde IS NULL OR FechaConsulta >= @FechaDesde)
      AND (@FechaHasta IS NULL OR FechaConsulta <= @FechaHasta)
    ORDER BY FechaConsulta DESC;
END
GO

-- =============================================
-- SP: ConsultaMedicamento_Insert
-- Agrega un medicamento a una consulta
-- =============================================
IF OBJECT_ID('ConsultaMedicamento_Insert', 'P') IS NOT NULL
    DROP PROCEDURE ConsultaMedicamento_Insert;
GO

CREATE PROCEDURE ConsultaMedicamento_Insert
    @IdConsulta UNIQUEIDENTIFIER,
    @IdMedicamento UNIQUEIDENTIFIER,
    @Cantidad INT,
    @Indicaciones NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ConsultaMedicamento (IdConsulta, IdMedicamento, Cantidad, Indicaciones)
    VALUES (@IdConsulta, @IdMedicamento, @Cantidad, @Indicaciones);

    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO

-- =============================================
-- SP: ConsultaMedicamento_Delete
-- Elimina un medicamento de una consulta
-- =============================================
IF OBJECT_ID('ConsultaMedicamento_Delete', 'P') IS NOT NULL
    DROP PROCEDURE ConsultaMedicamento_Delete;
GO

CREATE PROCEDURE ConsultaMedicamento_Delete
    @IdConsulta UNIQUEIDENTIFIER,
    @IdMedicamento UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM ConsultaMedicamento
    WHERE IdConsulta = @IdConsulta
      AND IdMedicamento = @IdMedicamento;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO

-- =============================================
-- SP: ConsultaMedicamento_SelectByConsulta
-- Obtiene todos los medicamentos de una consulta
-- =============================================
IF OBJECT_ID('ConsultaMedicamento_SelectByConsulta', 'P') IS NOT NULL
    DROP PROCEDURE ConsultaMedicamento_SelectByConsulta;
GO

CREATE PROCEDURE ConsultaMedicamento_SelectByConsulta
    @IdConsulta UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cm.IdConsulta,
        cm.IdMedicamento,
        cm.Cantidad,
        cm.Indicaciones,
        m.Nombre AS NombreMedicamento,
        m.Presentacion
    FROM ConsultaMedicamento cm
    INNER JOIN Medicamento m ON cm.IdMedicamento = m.IdMedicamento
    WHERE cm.IdConsulta = @IdConsulta;
END
GO

-- =============================================
-- SP: ConsultaMedica_Finalizar
-- Finaliza una consulta y actualiza el estado de la cita a Completada
-- También reduce el stock de medicamentos
-- =============================================
IF OBJECT_ID('ConsultaMedica_Finalizar', 'P') IS NOT NULL
    DROP PROCEDURE ConsultaMedica_Finalizar;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE ConsultaMedica_Finalizar
    @IdConsulta UNIQUEIDENTIFIER,
    @IdCita UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        -- 1. PRIMERO: Verificar que TODOS los medicamentos tienen stock suficiente
        IF EXISTS (
            SELECT 1
            FROM ConsultaMedicamento cm
            INNER JOIN Medicamento m ON cm.IdMedicamento = m.IdMedicamento
            WHERE cm.IdConsulta = @IdConsulta
              AND m.Stock < cm.Cantidad
        )
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Stock insuficiente para uno o más medicamentos', 16, 1);
            RETURN;
        END

        -- 2. Actualizar estado de la cita a Completada (EstadoCita.Completada = 3)
        UPDATE Cita
        SET Estado = 'Completada'
        WHERE IdCita = @IdCita;

        -- 3. Reducir stock de medicamentos (ahora sabemos que hay suficiente stock)
        UPDATE m
        SET m.Stock = m.Stock - cm.Cantidad
        FROM Medicamento m
        INNER JOIN ConsultaMedicamento cm ON m.IdMedicamento = cm.IdMedicamento
        WHERE cm.IdConsulta = @IdConsulta;

        COMMIT TRANSACTION;
        SELECT 1 AS Resultado, 'Consulta finalizada exitosamente' AS Mensaje;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

-- =============================================
-- SP: ConsultaMedica_SelectByMascota
-- Obtiene el historial de consultas de una mascota
-- =============================================
IF OBJECT_ID('ConsultaMedica_SelectByMascota', 'P') IS NOT NULL
    DROP PROCEDURE ConsultaMedica_SelectByMascota;
GO

CREATE PROCEDURE ConsultaMedica_SelectByMascota
    @IdMascota UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cm.IdConsulta,
        cm.IdCita,
        cm.IdVeterinario,
        cm.Sintomas,
        cm.Diagnostico,
        cm.Tratamiento,
        cm.Observaciones,
        cm.FechaConsulta,
        cm.Activo,
        v.Nombre AS NombreVeterinario,
        c.FechaCita,
        m.Nombre AS MascotaNombre,
        m.Especie,
        cl.Nombre + ' ' + cl.Apellido AS ClienteNombre,
        cl.Telefono AS ClienteTelefono
    FROM ConsultaMedica cm
    INNER JOIN Cita c ON cm.IdCita = c.IdCita
    INNER JOIN Mascota m ON c.IdMascota = m.IdMascota
    INNER JOIN Cliente cl ON m.IdCliente = cl.IdCliente
    INNER JOIN Veterinario v ON cm.IdVeterinario = v.IdVeterinario
    WHERE c.IdMascota = @IdMascota
      AND cm.Activo = 1
    ORDER BY cm.FechaConsulta DESC;
END
GO

PRINT 'Stored Procedures de ConsultaMedica creados exitosamente';
GO
