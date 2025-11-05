-- =============================================
-- Script: 24_CrearTablaMedicamento.sql
-- Descripción: Crea la tabla Medicamento en VetCareDB
-- =============================================

USE VetCareDB;
GO

-- Crear tabla Medicamento
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Medicamento]') AND type in (N'U'))
BEGIN
    CREATE TABLE Medicamento
    (
        IdMedicamento UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Nombre NVARCHAR(150) NOT NULL,
        Presentacion NVARCHAR(100),  -- Ej: "500mg", "Suspensión 250mg/5ml"
        Stock INT NOT NULL DEFAULT 0,
        PrecioUnitario DECIMAL(10,2) NOT NULL DEFAULT 0,
        Observaciones NVARCHAR(500),
        FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
        Activo BIT NOT NULL DEFAULT 1,

        CONSTRAINT CK_Medicamento_Stock CHECK (Stock >= 0),
        CONSTRAINT CK_Medicamento_Precio CHECK (PrecioUnitario >= 0)
    );

    PRINT 'Tabla Medicamento creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La tabla Medicamento ya existe';
END
GO

-- Crear índices para optimizar búsquedas
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Medicamento_Nombre' AND object_id = OBJECT_ID('Medicamento'))
BEGIN
    CREATE INDEX IX_Medicamento_Nombre ON Medicamento(Nombre);
    PRINT 'Índice IX_Medicamento_Nombre creado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Medicamento_Activo' AND object_id = OBJECT_ID('Medicamento'))
BEGIN
    CREATE INDEX IX_Medicamento_Activo ON Medicamento(Activo);
    PRINT 'Índice IX_Medicamento_Activo creado';
END
GO
