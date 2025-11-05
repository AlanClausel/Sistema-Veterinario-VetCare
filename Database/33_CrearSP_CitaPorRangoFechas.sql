-- =============================================
-- Script: 33_CrearSP_CitaPorRangoFechas.sql
-- Descripción: Stored Procedure para obtener citas por rango de fechas
-- Útil para reportes como "Citas de la Semana"
-- =============================================

USE VetCareDB;
GO

PRINT '════════════════════════════════════════════════════'
PRINT 'Creando SP_Cita_SelectByFechaRango...'
PRINT '════════════════════════════════════════════════════'

-- =============================================
-- SP: SP_Cita_SelectByFechaRango
-- Descripción: Obtiene todas las citas entre dos fechas
-- =============================================
IF OBJECT_ID('SP_Cita_SelectByFechaRango', 'P') IS NOT NULL
    DROP PROCEDURE SP_Cita_SelectByFechaRango;
GO

CREATE PROCEDURE SP_Cita_SelectByFechaRango
    @FechaDesde DATETIME,
    @FechaHasta DATETIME
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
      AND c.FechaCita >= @FechaDesde
      AND c.FechaCita < DATEADD(DAY, 1, @FechaHasta)  -- Incluir todo el día final
    ORDER BY c.FechaCita ASC;
END;
GO

PRINT '✓ SP_Cita_SelectByFechaRango creado exitosamente'
PRINT ''
GO
