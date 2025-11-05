/*******************************************************************
 * Script: 17_CrearTablaCita.sql
 * Descripción: Crea la tabla Cita para gestión de citas veterinarias
 * Fecha: 2025-01-11
 * Base de Datos: VetCareDB
 *******************************************************************/

USE VetCareDB;
GO

-- Eliminar tabla si existe (solo para desarrollo)
IF OBJECT_ID('Cita', 'U') IS NOT NULL
    DROP TABLE Cita;
GO

-- Crear tabla Cita
CREATE TABLE Cita
(
    IdCita UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IdMascota UNIQUEIDENTIFIER NOT NULL,
    FechaCita DATETIME NOT NULL,
    TipoConsulta NVARCHAR(100) NOT NULL,
    Veterinario NVARCHAR(150) NOT NULL,
    Estado NVARCHAR(20) NOT NULL, -- 'Agendada', 'Confirmada', 'Completada', 'Cancelada', 'NoAsistio'
    Observaciones NVARCHAR(500),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Cita_Mascota FOREIGN KEY (IdMascota)
        REFERENCES Mascota(IdMascota) ON DELETE CASCADE,

    CONSTRAINT CK_Cita_Estado CHECK (Estado IN ('Agendada', 'Confirmada', 'Completada', 'Cancelada', 'NoAsistio')),
    CONSTRAINT CK_Cita_FechaCita CHECK (FechaCita >= CAST(GETDATE() AS DATE) OR Estado IN ('Completada', 'Cancelada', 'NoAsistio'))
);
GO

-- Índices para optimizar búsquedas
CREATE INDEX IX_Cita_IdMascota ON Cita(IdMascota);
CREATE INDEX IX_Cita_FechaCita ON Cita(FechaCita);
CREATE INDEX IX_Cita_Estado ON Cita(Estado);
CREATE INDEX IX_Cita_Veterinario ON Cita(Veterinario);
GO

PRINT 'Tabla Cita creada exitosamente';
GO
