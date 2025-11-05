-- =============================================
-- Script: 25_CrearTablaConsultaMedica.sql
-- Descripción: Crea las tablas ConsultaMedica y ConsultaMedicamento en VetCareDB
-- =============================================

USE VetCareDB;
GO

-- Crear tabla ConsultaMedica
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConsultaMedica]') AND type in (N'U'))
BEGIN
    CREATE TABLE ConsultaMedica
    (
        IdConsulta UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        IdCita UNIQUEIDENTIFIER NOT NULL,
        IdVeterinario UNIQUEIDENTIFIER NOT NULL,
        Sintomas NVARCHAR(1000) NOT NULL,
        Diagnostico NVARCHAR(1000) NOT NULL,
        Tratamiento NVARCHAR(1000),
        Observaciones NVARCHAR(500),
        FechaConsulta DATETIME NOT NULL DEFAULT GETDATE(),
        Activo BIT NOT NULL DEFAULT 1,

        CONSTRAINT FK_ConsultaMedica_Cita FOREIGN KEY (IdCita) REFERENCES Cita(IdCita),
        CONSTRAINT FK_ConsultaMedica_Veterinario FOREIGN KEY (IdVeterinario) REFERENCES Veterinario(IdVeterinario)
    );

    PRINT 'Tabla ConsultaMedica creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La tabla ConsultaMedica ya existe';
END
GO

-- Crear tabla ConsultaMedicamento (relación muchos a muchos)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConsultaMedicamento]') AND type in (N'U'))
BEGIN
    CREATE TABLE ConsultaMedicamento
    (
        IdConsulta UNIQUEIDENTIFIER NOT NULL,
        IdMedicamento UNIQUEIDENTIFIER NOT NULL,
        Cantidad INT NOT NULL DEFAULT 1,
        Indicaciones NVARCHAR(500),

        CONSTRAINT PK_ConsultaMedicamento PRIMARY KEY (IdConsulta, IdMedicamento),
        CONSTRAINT FK_ConsultaMedicamento_Consulta FOREIGN KEY (IdConsulta) REFERENCES ConsultaMedica(IdConsulta) ON DELETE CASCADE,
        CONSTRAINT FK_ConsultaMedicamento_Medicamento FOREIGN KEY (IdMedicamento) REFERENCES Medicamento(IdMedicamento),
        CONSTRAINT CK_ConsultaMedicamento_Cantidad CHECK (Cantidad > 0)
    );

    PRINT 'Tabla ConsultaMedicamento creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La tabla ConsultaMedicamento ya existe';
END
GO

-- Crear índices para optimizar búsquedas
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ConsultaMedica_Cita' AND object_id = OBJECT_ID('ConsultaMedica'))
BEGIN
    CREATE INDEX IX_ConsultaMedica_Cita ON ConsultaMedica(IdCita);
    PRINT 'Índice IX_ConsultaMedica_Cita creado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ConsultaMedica_Veterinario' AND object_id = OBJECT_ID('ConsultaMedica'))
BEGIN
    CREATE INDEX IX_ConsultaMedica_Veterinario ON ConsultaMedica(IdVeterinario);
    PRINT 'Índice IX_ConsultaMedica_Veterinario creado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ConsultaMedica_Fecha' AND object_id = OBJECT_ID('ConsultaMedica'))
BEGIN
    CREATE INDEX IX_ConsultaMedica_Fecha ON ConsultaMedica(FechaConsulta DESC);
    PRINT 'Índice IX_ConsultaMedica_Fecha creado';
END
GO
